using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using MDK_UI.MockupExtensions;

namespace IngameScript.Mockups.Base
{
    [DisplayName("Unsupported Block"), MetadataType(typeof(MockTerminalBlockMetaData))]
    public partial class MockTerminalBlock { }

    internal class MockTerminalBlockMetaData
    {
        [DisplayName("Custom Name")]
        public object CustomName { get; set; }

        [DisplayName("Custom Data"), DataType(DataType.MultilineText)]
        public object CustomData { get; set; }

        [DisplayName("Show On HUD")]
        public object ShowOnHUD { get; set; }

        [DisplayName("Show In Terminal")]
        public object ShowInTerminal { get; set; }

        [DisplayName("Show In Toolbar Config")]
        public object ShowInToolbarConfig { get; set; }

        [DisplayName("Show In Inventory")]
        public object ShowInInventory { get; set; }
    }
}
