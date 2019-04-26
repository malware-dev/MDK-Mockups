using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataExtractor
{
    static partial class Program
    {
        const string TextScriptNamespace = "Sandbox.Game.GameSystems.TextSurfaceScripts";
        const string TextScriptAttribute = "MyTextSurfaceScriptAttribute";

        private static async Task GenerateFile(string output, string file, IDictionary<string, Action<StringBuilder>> replacements)
        {
            var template = File.ReadAllText($"{file}.template.cs");

            var builder = new StringBuilder();
            foreach (var replace in replacements)
            {
                builder.Clear();
                replace.Value(builder);

                template = template.Replace("//%" + replace.Key + "%", builder.ToString());
            }

            using (var writer = new StreamWriter(Path.Combine(output, $"{file}.consts.debug.cs"), false))
            {
                await writer.WriteAsync(template);
            }
        }

        private static Task GenerateTextSurfaceValues(string sePath, string output, string header)
        {
            return GenerateFile(output, "MockTextSurface", new Dictionary<string, Action<StringBuilder>>
            {
                {
                    "HEADER",
                    builder => builder.Append(header)
                },
                {
                    "FONTS",
                    builder =>
                    {
                        Console.WriteLine("Extracting LCD fonts...");

                        var fontSerializer = new XmlSerializer(typeof(FontDefinitions.Definitions));
                        var fontDefinitions = default(FontDefinitions.Definitions);

                        using (var reader = new StreamReader(Path.Combine(sePath, "Content", "Data", "Fonts.sbc")))
                        {
                            fontDefinitions = (FontDefinitions.Definitions)fontSerializer.Deserialize(reader);
                        }

                        foreach (var font in fontDefinitions.Fonts.Font)
                        {
                            var name = font.Id.SubtypeId;
                            var path = font.Resources.Resource.Path;

                            var rootFont = path.Split('\\')[1];

                            builder.Append($"\t\t\t{{ \"{name}\", \"{rootFont}\" }}");

                            if (font != fontDefinitions.Fonts.Font.Last())
                            {
                                builder.AppendLine(",");
                            }
                        }
                    }
                },
                {
                    "TEXTURES",
                    builder =>
                    {
                        Console.WriteLine("Extracting LCD textures...");

                        var textureSerializer = new XmlSerializer(typeof(TextureDefinitions.Definitions));
                        var textureDefinitions = default(TextureDefinitions.Definitions);
                        using (var reader = new StreamReader(Path.Combine(sePath, "Content", "Data", "LCDTextures.sbc")))
                        {
                            textureDefinitions = (TextureDefinitions.Definitions)textureSerializer.Deserialize(reader);
                        }

                        var textures = textureDefinitions.LCDTextures.LCDTextureDefinition.Where(t => !string.IsNullOrWhiteSpace(t.Id.SubtypeId));
                        foreach (var texture in textures)
                        {
                            builder.Append($"\t\t\t\"{texture.Id.SubtypeId}\"");
                            if (texture != textures.Last())
                            {
                                builder.AppendLine(",");
                            }
                        }
                    }
                },
                {
                    "SPRITES",
                    builder =>
                    {
                        Console.WriteLine("Extracting LCD sprites...");

                        var sprites = Directory.GetFiles(Path.Combine(sePath, "Content", "Textures", "Sprites"), "*.dds", SearchOption.TopDirectoryOnly);
                        foreach (var sprite in sprites)
                        {
                            var name = Path.GetFileNameWithoutExtension(sprite);

                            builder.Append($"\t\t\t\"{name}\"");
                            if (sprite != sprites.Last())
                            {
                                builder.AppendLine(",");
                            }
                        }
                    }
                },
                {
                    "SCRIPTS",
                    builder =>
                    {
                        Console.WriteLine("Extracting LCD built-in scripts...");

                        #region Reflection Only Loads

                        var clrPath = RuntimeEnvironment.GetRuntimeDirectory();
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(clrPath, "System.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(clrPath, "System.Drawing.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(clrPath, "System.Core.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(clrPath, "System.Xml.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(clrPath, "System.Net.Http.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(clrPath, "System.Windows.Forms.dll"));

                        var binPath = Path.Combine(sePath, "Bin64");
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "VRage.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "VRage.Native.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "HavokWrapper.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "VRage.Library.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "VRage.Render.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "VRage.Math.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "VRage.Game.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "VRage.Input.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "VRage.OpenVRWrapper.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "VRage.Ansel.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "VRage.Audio.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "Sandbox.Common.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "Sandbox.RenderDirect.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "Sandbox.Graphics.dll"));
                        Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "SpaceEngineers.ObjectBuilders.dll"));

                        #endregion Reflection Only Loads

                        var scriptDll = Assembly.ReflectionOnlyLoadFrom(Path.Combine(binPath, "Sandbox.Game.dll"));
                        var scripts = scriptDll.DefinedTypes
                                .Where(t => t.Namespace == TextScriptNamespace)
                                .Where(t => t.CustomAttributes.Any(a => a.AttributeType.Name == TextScriptAttribute))
                                .Where(t => !t.IsAbstract).ToList();

                        foreach (var script in scripts)
                        {
                            var attr = CustomAttributeData.GetCustomAttributes(script).First();
                            var name = attr.ConstructorArguments.First().Value.ToString().Substring(4);

                            builder.Append($"\t\t\t\"{name}\"");
                            if (script != scripts.Last())
                            {
                                builder.AppendLine(",");
                            }
                        }
                    }
                }
            });
        }
    }
}
