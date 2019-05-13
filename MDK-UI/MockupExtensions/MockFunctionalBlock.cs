using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IngameScript.Mockups.Base
{
    [MetadataType(typeof(MockFunctionalBlockMetadata))]
    public partial class MockFunctionalBlock { }

    internal class MockFunctionalBlockMetadata: MockTerminalBlockMetaData
    {
        [DisplayName("Enabled")]
        public object Enabled { get; set; }
    }
}
