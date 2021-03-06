﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript.Mockups.Base
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public partial class MockTextSurface : IMyTextSurface
    {
        const int MaxCharacterCount = 100000;

        readonly List<string> _selectedImages = new List<string>();
        readonly StringBuilder _text = new StringBuilder();
        List<MySprite> MySprites { get; } = new List<MySprite>();

        public string CurrentlyShownImage { get; set; }

        public float FontSize { get; set; } = 1;
        public Color FontColor { get; set; } = new Color(255, 255, 255);
        public Color BackgroundColor { get; set; } = new Color(0, 0, 0);
        public byte BackgroundAlpha { get; set; } = new byte();
        public float ChangeInterval { get; set; } = 0;
        public string Font { get; set; } = "Debug";
        public TextAlignment Alignment { get; set; } = TextAlignment.LEFT;
        public string Script { get; set; } = "";
        public ContentType ContentType { get; set; } = ContentType.NONE;

        public Vector2 SurfaceSize { get; }

        public Vector2 TextureSize { get; }

        public bool PreserveAspectRatio { get; set; } = true;
        public float TextPadding { get; set; } = 0f;
        public Color ScriptBackgroundColor { get; set; } = new Color(255, 255, 255);
        public Color ScriptForegroundColor { get; set; } = new Color(0, 0, 0);

        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public IEnumerable<MySprite> SpriteBuffer => MySprites;

        public MockTextSurface(Vector2 surfaceSize, Vector2 textureSize)
        {
            SurfaceSize = surfaceSize;
            TextureSize = textureSize;
        }

        public virtual void AddImagesToSelection(List<string> ids, bool checkExistence = false)
        {
            Debug.Assert(ids != null, $"{nameof(ids)} cannot be null");
            foreach (var id in ids)
                AddImageToSelection(id, checkExistence);
        }

        public virtual void AddImageToSelection(string id, bool checkExistence = false)
        {
            Debug.Assert(id != null, $"{nameof(id)} cannot be null");
            if (checkExistence && _selectedImages.Contains(id))
                return;

            if (_textures.Contains(id))
                _selectedImages.Add(id);
        }

        public virtual void ClearImagesFromSelection()
        {
            _selectedImages.Clear();
        }

        public virtual void GetFonts(List<string> fonts)
        {
            Debug.Assert(fonts != null, $"{nameof(fonts)} cannot be null!");
            fonts.Clear();

            fonts.AddRange(_fonts.Keys);
        }

        public MySpriteDrawFrame DrawFrame() => new MySpriteDrawFrame(frame =>
        {
            MySprites.Clear();
            frame.AddToList(MySprites);
        });

        public void GetScripts(List<string> scripts)
        {
            Debug.Assert(scripts != null, $"{nameof(scripts)} cannot be null!");
            scripts.Clear();
            foreach (var script in _scripts)
                scripts.Add(script);
        }

        public void GetSelectedImages(List<string> output)
        {
            Debug.Assert(output != null, $"{nameof(output)} cannot be null!");
            output.Clear();
            foreach (var image in _selectedImages)
                output.Add(image);
        }

        public void GetSprites(List<string> sprites)
        {
            Debug.Assert(sprites != null, $"{nameof(sprites)} cannot be null!");
            sprites.Clear();
            foreach (var image in _sprites)
                sprites.Add(image);
        }

        public string GetText() => _text.ToString();

        public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale)
        {
            try
            {
                var result = GetFontDefinition(font)?.MeasureString(text, scale);
                return result ?? Vector2.Zero;
            }
            catch (Exception)
            {
                return Vector2.Zero;
            }
        }

        public void ReadText(StringBuilder buffer, bool append = false)
        {
            Debug.Assert(buffer != null, $"{nameof(buffer)} cannot be null");
            if (!append)
                buffer.Clear();
            buffer.AppendStringBuilder(_text);
        }

        public void RemoveImageFromSelection(string id, bool removeDuplicates = false)
        {
            Debug.Assert(id != null, $"{nameof(id)} cannot be null");
            if (removeDuplicates)
                _selectedImages.RemoveAll(img => img == id);
            else
                _selectedImages.Remove(id);
        }

        public void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false)
        {
            Debug.Assert(ids != null, $"{nameof(ids)} cannot be null");
            foreach (var id in ids)
                RemoveImageFromSelection(id, removeDuplicates);
        }

        public bool WriteText(string value, bool append = false)
        {
            Debug.Assert(value != null, $"{nameof(value)} cannot be null");
            if (!append)
                _text.Clear();

            if (_text.Length + value.Length > MaxCharacterCount)
                value = value.Remove(MaxCharacterCount - _text.Length);
            _text.Append(value);

            return true;
        }

        public virtual bool WriteText(StringBuilder value, bool append = false)
        {
            Debug.Assert(value != null, $"{nameof(value)} cannot be null");
            if (!append)
                _text.Clear();

            if (_text.Length + value.Length > MaxCharacterCount)
                _text.AppendSubstring(value, 0, MaxCharacterCount);
            else
                _text.AppendStringBuilder(value);

            return true;
        }

#region Mock Context for IMyTextSurface.MeasureStringInPixels
        private static IDictionary<string, MockFont> FontDefinitions { get; } = new Dictionary<string, MockFont>();

        private static MockFont GetFontDefinition(string font)
        {
            var definition = _fonts.First(f => f.Key.ToLower() == font.ToLower()).Value;

            if (!FontDefinitions.ContainsKey(definition))
            {
                var dir = Path.GetDirectoryName(Assembly.GetAssembly(typeof(IMyAssembler)).Location);
                var file = Path.Combine(dir, "..", "Content", "Fonts", definition, "FontDataPA.xml");

                if (!File.Exists(file))
                {
                    return null;
                }

                var fontNode = default(XmlNode);
                using (var stream = File.OpenRead(file))
                {
                    var reader = new XmlDocument();
                    reader.Load(stream);
                    fontNode = reader.ChildNodes.OfType<XmlNode>().First(n => n.Name == "font");
                }

                var baseline = int.Parse(fontNode.Attributes.GetNamedItem("base").Value);
                var lineheight = int.Parse(fontNode.Attributes.GetNamedItem("height").Value);
                var glyphs = new Dictionary<char, MockFont.MockGlyphInfo>();
                var kerns = new Dictionary<MockFont.MockKernPair, sbyte>();

                foreach (var child in fontNode.ChildNodes.Cast<XmlNode>())
                {
                    switch (child.Name)
                    {
                        case "glyphs":
                            foreach (var glyph in child.ChildNodes.Cast<XmlNode>().Where(n => n.Name == "glyph"))
                            {
                                var loc = glyph.Attributes.GetNamedItem("loc")?.Value;
                                if (loc == null)
                                    loc = glyph.Attributes.GetNamedItem("origin")?.Value;

                                var locA = loc.Split(',');
                                var size = glyph.Attributes.GetNamedItem("size").Value;
                                var sizeA = size.Split('x');

                                glyphs.Add(glyph.Attributes.GetNamedItem("ch").Value[0], new MockFont.MockGlyphInfo
                                {
                                    nBitmapID = ushort.Parse(glyph.Attributes.GetNamedItem("bm").Value),
                                    pxLocX = ushort.Parse(locA[0]),
                                    pxLocY = ushort.Parse(locA[1]),
                                    pxWidth = byte.Parse(sizeA[0]),
                                    pxHeight = byte.Parse(sizeA[1]),
                                    pxAdvanceWidth = byte.Parse(glyph.Attributes.GetNamedItem("aw").Value),
                                    pxLeftSideBearing = sbyte.Parse(glyph.Attributes.GetNamedItem("lsb").Value)
                                });
                            }
                            break;
                        case "kernpairs":
                            foreach (var kern in child.ChildNodes.Cast<XmlNode>().Where(n => n.Name == "kernpair"))
                            {
                                var l = kern.Attributes.GetNamedItem("left").Value[0];
                                var r = kern.Attributes.GetNamedItem("right").Value[0];
                                var a = kern.Attributes.GetNamedItem("adjust").Value;

                                kerns.Add(new MockFont.MockKernPair { Left = l, Right = r }, sbyte.Parse(a));
                            }
                            break;
                    }
                }

                FontDefinitions.Add(definition, new MockFont(definition, lineheight, glyphs, kerns));
            }

            return FontDefinitions[definition];
        }

        // Mock of VRage.Render.dll MyFont for IMyTextSurface.MeasureStringInPixels
        private class MockFont
        {
            private static readonly MockKernPairComparer KernComparer = new MockKernPairComparer();

            private class MockKernPairComparer : IComparer<MockFont.MockKernPair>, IEqualityComparer<MockFont.MockKernPair>
            {
                public int Compare(MockFont.MockKernPair x, MockFont.MockKernPair y)
                {
                    if (x.Left != y.Left)
                    {
                        return x.Left.CompareTo(y.Left);
                    }
                    return x.Right.CompareTo(y.Right);
                }

                public bool Equals(MockFont.MockKernPair x, MockFont.MockKernPair y)
                {
                    return x.Left == y.Left && x.Right == y.Right;
                }

                public int GetHashCode(MockFont.MockKernPair x)
                {
                    return x.Left.GetHashCode() ^ x.Right.GetHashCode();
                }
            }

            public struct MockKernPair
            {
                public MockKernPair(char l, char r)
                {
                    this.Left = l;
                    this.Right = r;
                }

                public char Left;
                public char Right;
            }

            public struct MockGlyphInfo
            {
                public ushort nBitmapID;
                public ushort pxLocX;
                public ushort pxLocY;
                public byte pxWidth;
                public byte pxHeight;
                public byte pxAdvanceWidth;
                public sbyte pxLeftSideBearing;
            };

            public bool KernEnabled { get; set; }
            public int Spacing { get; set; }
            public string Name { get; }
            public int LineHeight { get; private set; }

            private IReadOnlyDictionary<char, MockGlyphInfo> GlyphInfo { get; }
            private IReadOnlyDictionary<MockKernPair, sbyte> KernPairs { get; } = new Dictionary<MockKernPair, sbyte>(KernComparer);

            public MockFont(string name, int height, IReadOnlyDictionary<char, MockGlyphInfo> glyphs, IReadOnlyDictionary<MockKernPair, sbyte> kernPairs, int spacing = 1)
            {
                Name = name;
                Spacing = spacing;
                LineHeight = height;
                GlyphInfo = glyphs;
                KernPairs = kernPairs;
            }

            public Vector2 MeasureString(StringBuilder text, float scale)
            {
                scale *= 0.778378367f;
                float num = 0f;
                char chLeft = '\0';
                float num2 = 0f;
                int num3 = 1;
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    if (c == '\n')
                    {
                        num3++;
                        num = 0f;
                        chLeft = '\0';
                    }
                    else if (this.CanWriteOrReplace(ref c))
                    {
                        var myGlyphInfo = this.GlyphInfo[c];
                        if (this.KernEnabled)
                        {
                            num += (float)this.CalcKern(chLeft, c);
                            chLeft = c;
                        }
                        num += (float)myGlyphInfo.pxAdvanceWidth;
                        if (i < text.Length - 1)
                        {
                            num += (float)this.Spacing;
                        }
                        if (num > num2)
                        {
                            num2 = num;
                        }
                    }
                }
                return new Vector2(num2 * scale, (float)(num3 * this.LineHeight) * scale);
            }

            protected bool CanWriteOrReplace(ref char c)
            {
                if (!this.GlyphInfo.ContainsKey(c))
                {
                    if (!this.CanUseReplacementCharacter(c))
                    {
                        return false;
                    }
                    c = '□';
                }
                return true;
            }

            protected int CalcKern(char chLeft, char chRight)
            {
                sbyte result;
                this.KernPairs.TryGetValue(new MockKernPair(chLeft, chRight), out result);
                return result;
            }

            protected bool CanUseReplacementCharacter(char c)
            {
                return !char.IsWhiteSpace(c) && !char.IsControl(c);
            }
        }
#endregion
    }
}
