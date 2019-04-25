using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataExtractor
{
    static partial class Program
    {
        const string TextScriptNamespace = "Sandbox.Game.GameSystems.TextSurfaceScripts";
        const string TextScriptAttribute = "MyTextSurfaceScriptAttribute";

        private static async Task GenerateTextSurfaceValues(string sePath, string output, string header)
        {
            using (var writer = new StreamWriter(Path.Combine(output, "MockTextSurface.consts.debug.cs"), false))
            {
                await writer.WriteAsync(header);

                await writer.WriteLineAsync("using System.Collections.Generic;");
                await writer.WriteLineAsync("using VRageMath;");
                await writer.WriteLineAsync();
                await writer.WriteLineAsync("namespace IngameScript.Mockups.Base");
                await writer.WriteLineAsync("{");
                await writer.WriteLineAsync("\tpublic partial class MockTextSurface");
                await writer.WriteLineAsync("\t{");
                await writer.WriteLineAsync();
                {
                    Console.WriteLine("Extracting LCD fonts...");
                    await writer.WriteLineAsync("\t\tstatic readonly IReadOnlyDictionary<string, string> _fonts = new Dictionary<string, string>");
                    await writer.WriteLineAsync("\t\t{");

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

                        await writer.WriteAsync($"\t\t\t{{ \"{name}\", \"{rootFont}\" }}");

                        if (font == fontDefinitions.Fonts.Font.Last())
                        {
                            await writer.WriteLineAsync();
                        }
                        else
                        {
                            await writer.WriteLineAsync(",");
                        }
                    }

                    await writer.WriteLineAsync("\t\t};");
                }
                await writer.WriteLineAsync();
                {
                    Console.WriteLine("Extracting LCD font dimensions...");
                    await writer.WriteLineAsync("\t\tstatic readonly IReadOnlyDictionary<string, IReadOnlyDictionary<Vector2, string>> _charMap = new Dictionary<string, IReadOnlyDictionary<Vector2, string>>()");
                    await writer.WriteLineAsync("\t\t{");

                    var directories = Directory.GetDirectories(Path.Combine(sePath, "Content", "Fonts"));
                    var charSerializer = new XmlSerializer(typeof(FontData.Font));
                    foreach (var dir in directories)
                    {
                        var fontName = dir.Split(Path.DirectorySeparatorChar).Last();
                        var fontData = default(FontData.Font);

                        using (var reader = new StreamReader(Path.Combine(dir, "FontDataPA.xml")))
                        {
                            fontData = (FontData.Font)charSerializer.Deserialize(reader);
                        }

                        await writer.WriteLineAsync("\t\t\t{");
                        await writer.WriteLineAsync($"\t\t\t\t\"{fontName}\",");
                        await writer.WriteLineAsync("\t\t\t\tnew Dictionary<Vector2, string>()");
                        await writer.WriteLineAsync("\t\t\t\t{");

                        var glyphs = fontData.Glyphs.Glyph.GroupBy(g => g.Size);
                        foreach (var glyph in glyphs)
                        {
                            var characters = string.Join("", glyph.Select(g => g.Ch));
                            characters = characters.Replace("\\", "\\\\").Replace("\"", "\\\"");
                            var key = glyph.Key.Split('x');

                            await writer.WriteAsync($"\t\t\t\t\t{{ new Vector2({key[0]}, {key[1]}), \"{characters}\" }}");

                            if (glyph.Key == glyphs.Last().Key)
                            {
                                await writer.WriteLineAsync();
                            }
                            else
                            {
                                await writer.WriteLineAsync(",");
                            }
                        }

                        await writer.WriteLineAsync("\t\t\t\t}");
                        await writer.WriteAsync("\t\t\t}");

                        if (dir == directories.Last())
                        {
                            await writer.WriteLineAsync();
                        }
                        else
                        {
                            await writer.WriteLineAsync(",");
                        }
                    }

                    await writer.WriteLineAsync("\t\t};");
                }
                await writer.WriteLineAsync();
                {
                    Console.WriteLine("Extracting LCD textures...");
                    await writer.WriteLineAsync("\t\tstatic readonly IEnumerable<string> _textures = new List<string>");
                    await writer.WriteLineAsync("\t\t{");

                    var textureSerializer = new XmlSerializer(typeof(TextureDefinitions.Definitions));
                    var textureDefinitions = default(TextureDefinitions.Definitions);
                    using (var reader = new StreamReader(Path.Combine(sePath, "Content", "Data", "LCDTextures.sbc")))
                    {
                        textureDefinitions = (TextureDefinitions.Definitions)textureSerializer.Deserialize(reader);
                    }

                    var textures = textureDefinitions.LCDTextures.LCDTextureDefinition.Where(t => !string.IsNullOrWhiteSpace(t.Id.SubtypeId));
                    foreach (var texture in textures)
                    {
                        await writer.WriteAsync($"\t\t\t\"{texture.Id.SubtypeId}\"");
                        if (texture == textures.Last())
                        {
                            await writer.WriteLineAsync();
                        }
                        else
                        {
                            await writer.WriteLineAsync(",");
                        }
                    }

                    await writer.WriteLineAsync("\t\t};");
                }
                await writer.WriteLineAsync();
                {
                    Console.WriteLine("Extracting LCD sprites...");
                    await writer.WriteLineAsync("\t\tstatic readonly IEnumerable<string> _sprites = new List<string>");
                    await writer.WriteLineAsync("\t\t{");

                    var sprites = Directory.GetFiles(Path.Combine(sePath, "Content", "Textures", "Sprites"), "*.dds", SearchOption.TopDirectoryOnly);
                    foreach (var sprite in sprites)
                    {
                        var name = Path.GetFileNameWithoutExtension(sprite);

                        await writer.WriteAsync($"\t\t\t\"{name}\"");
                        if (sprite == sprites.Last())
                        {
                            await writer.WriteLineAsync();
                        }
                        else
                        {
                            await writer.WriteLineAsync(",");
                        }
                    }

                    await writer.WriteLineAsync("\t\t};");
                }
                await writer.WriteLineAsync();
                {
                    Console.WriteLine("Extracting LCD built-in scripts...");

                    await writer.WriteLineAsync("\t\tstatic readonly IEnumerable<string> _scripts = new List<string>");
                    await writer.WriteLineAsync("\t\t{");

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

                        await writer.WriteAsync($"\t\t\t\"{name}\"");
                        if (script == scripts.Last())
                        {
                            await writer.WriteLineAsync();
                        }
                        else
                        {
                            await writer.WriteLineAsync(",");
                        }
                    }

                    await writer.WriteLineAsync("\t\t};");
                }
                await writer.WriteLineAsync("\t}");
                await writer.WriteLineAsync("}");
            }
        }
    }
}
