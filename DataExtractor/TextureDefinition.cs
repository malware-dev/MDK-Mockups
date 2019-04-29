using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataExtractor.TextureDefinitions
{
    [XmlRoot(ElementName = "Id")]
    public class Id
    {
        [XmlElement(ElementName = "TypeId")]
        public string TypeId { get; set; }
        [XmlElement(ElementName = "SubtypeId")]
        public string SubtypeId { get; set; }
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "Subtype")]
        public string Subtype { get; set; }
    }

    [XmlRoot(ElementName = "LCDTextureDefinition")]
    public class LCDTextureDefinition
    {
        [XmlElement(ElementName = "Id")]
        public Id Id { get; set; }
        [XmlElement(ElementName = "LocalizationId")]
        public string LocalizationId { get; set; }
        [XmlElement(ElementName = "TexturePath")]
        public string TexturePath { get; set; }
        [XmlElement(ElementName = "SpritePath")]
        public string SpritePath { get; set; }
        [XmlElement(ElementName = "Selectable")]
        public string Selectable { get; set; }
        [XmlElement(ElementName = "Public")]
        public string Public { get; set; }
    }

    [XmlRoot(ElementName = "LCDTextures")]
    public class LCDTextures
    {
        [XmlElement(ElementName = "LCDTextureDefinition")]
        public List<LCDTextureDefinition> LCDTextureDefinition { get; set; }
    }

    [XmlRoot(ElementName = "Definitions")]
    public class Definitions
    {
        [XmlElement(ElementName = "LCDTextures")]
        public LCDTextures LCDTextures { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }
}
