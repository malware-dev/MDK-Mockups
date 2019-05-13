using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IngameScript.Mockups.Base;
using SpaceEngineers.Game.ModAPI.Ingame;

namespace IngameScript.Mockups.Blocks
{
    [DisplayName("Air Vent"), MetadataType(typeof(MockAirVentMetadata))]
    public partial class MockAirVent { }

    internal class MockAirVentMetadata: MockFunctionalBlockMetadata
    {
        [DisplayName("Oxygen Level"), Range(0, 1)]
        public object OxygenLevel { get; set; }

        [DisplayName("Can Pressurize")]
        public object CanPressurize { get; set; }

        [DisplayName("Is Depressurizing"), ReadOnly(true)]
        public object IsDepressurizing { get; set; }

        [DisplayName("Pressurize")]
        public object Depressurize { get; set; }

        [DisplayName("Status")]
        public virtual VentStatus Status { get; set; }
    }
}
