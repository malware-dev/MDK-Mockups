using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataExtractor.FontData
{
    [XmlRoot(ElementName = "bitmap", Namespace = "http://xna.microsoft.com/bitmapfont")]
    public class Bitmap
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "size")]
        public string Size { get; set; }
    }
    [XmlRoot(ElementName = "bitmaps", Namespace = "http://xna.microsoft.com/bitmapfont")]
    public class Bitmaps
    {
        [XmlElement(ElementName = "bitmap", Namespace = "http://xna.microsoft.com/bitmapfont")]
        public Bitmap Bitmap { get; set; }
    }

    [XmlRoot(ElementName = "font", Namespace = "http://xna.microsoft.com/bitmapfont")]
    public class Font
    {
        [XmlAttribute(AttributeName = "base")]
        public string Base { get; set; }
        [XmlElement(ElementName = "bitmaps", Namespace = "http://xna.microsoft.com/bitmapfont")]
        public Bitmaps Bitmaps { get; set; }
        [XmlAttribute(AttributeName = "face")]
        public string Face { get; set; }
        [XmlElement(ElementName = "glyphs", Namespace = "http://xna.microsoft.com/bitmapfont")]
        public Glyphs Glyphs { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlElement(ElementName = "kernpairs", Namespace = "http://xna.microsoft.com/bitmapfont")]
        public Kernpairs Kernpairs { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
        [XmlAttribute(AttributeName = "size")]
        public string Size { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

    [XmlRoot(ElementName = "glyph", Namespace = "http://xna.microsoft.com/bitmapfont")]
    public class Glyph
    {
        [XmlAttribute(AttributeName = "aw")]
        public string Aw { get; set; }
        [XmlAttribute(AttributeName = "bm")]
        public string Bm { get; set; }
        [XmlAttribute(AttributeName = "ch")]
        public string Ch { get; set; }
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "forcewhite")]
        public string Forcewhite { get; set; }
        [XmlAttribute(AttributeName = "lsb")]
        public string Lsb { get; set; }
        [XmlAttribute(AttributeName = "origin")]
        public string Origin { get; set; }
        [XmlAttribute(AttributeName = "size")]
        public string Size { get; set; }
    }

    [XmlRoot(ElementName = "glyphs", Namespace = "http://xna.microsoft.com/bitmapfont")]
    public class Glyphs
    {
        [XmlElement(ElementName = "glyph", Namespace = "http://xna.microsoft.com/bitmapfont")]
        public List<Glyph> Glyph { get; set; }
    }

    [XmlRoot(ElementName = "kernpair", Namespace = "http://xna.microsoft.com/bitmapfont")]
    public class Kernpair
    {
        [XmlAttribute(AttributeName = "adjust")]
        public string Adjust { get; set; }
        [XmlAttribute(AttributeName = "left")]
        public string Left { get; set; }
        [XmlAttribute(AttributeName = "right")]
        public string Right { get; set; }
    }

    [XmlRoot(ElementName = "kernpairs", Namespace = "http://xna.microsoft.com/bitmapfont")]
    public class Kernpairs
    {
        [XmlElement(ElementName = "kernpair", Namespace = "http://xna.microsoft.com/bitmapfont")]
        public List<Kernpair> Kernpair { get; set; }
    }
}
