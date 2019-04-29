using System.Xml.Serialization;
using System.Collections.Generic;

namespace DataExtractor.FontDefinitions
{
    [XmlRoot(ElementName = "Id")]
    public class Id
    {
        [XmlElement(ElementName = "TypeId")]
        public string TypeId { get; set; }
        [XmlElement(ElementName = "SubtypeId")]
        public string SubtypeId { get; set; }
    }

    [XmlRoot(ElementName = "Resource")]
    public class Resource
    {
        [XmlAttribute(AttributeName = "Path")]
        public string Path { get; set; }
    }

    [XmlRoot(ElementName = "Resources")]
    public class Resources
    {
        [XmlElement(ElementName = "Resource")]
        public Resource Resource { get; set; }
    }

    [XmlRoot(ElementName = "LanguageDefinition")]
    public class LanguageDefinition
    {
        [XmlElement(ElementName = "Resources")]
        public Resources Resources { get; set; }
        [XmlAttribute(AttributeName = "Language")]
        public string Language { get; set; }
    }

    [XmlRoot(ElementName = "LanguageSpecificDefinitions")]
    public class LanguageSpecificDefinitions
    {
        [XmlElement(ElementName = "LanguageDefinition")]
        public LanguageDefinition LanguageDefinition { get; set; }
    }

    [XmlRoot(ElementName = "Font")]
    public class Font
    {
        [XmlElement(ElementName = "Id")]
        public Id Id { get; set; }
        [XmlElement(ElementName = "Default")]
        public string Default { get; set; }
        [XmlElement(ElementName = "Resources")]
        public Resources Resources { get; set; }
        [XmlElement(ElementName = "LanguageSpecificDefinitions")]
        public LanguageSpecificDefinitions LanguageSpecificDefinitions { get; set; }
        [XmlElement(ElementName = "ColorMask")]
        public ColorMask ColorMask { get; set; }
    }

    [XmlRoot(ElementName = "ColorMask")]
    public class ColorMask
    {
        [XmlElement(ElementName = "X")]
        public string X { get; set; }
        [XmlElement(ElementName = "Y")]
        public string Y { get; set; }
        [XmlElement(ElementName = "Z")]
        public string Z { get; set; }
    }

    [XmlRoot(ElementName = "Fonts")]
    public class Fonts
    {
        [XmlElement(ElementName = "Font")]
        public List<Font> Font { get; set; }
    }

    [XmlRoot(ElementName = "Definitions")]
    public class Definitions
    {
        [XmlElement(ElementName = "Fonts")]
        public Fonts Fonts { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }
}
