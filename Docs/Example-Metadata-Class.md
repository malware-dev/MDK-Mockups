Here is an example template for mocking an ingame block.

```cs
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IngameScript.Mockups.Base;
using SpaceEngineers.Game.ModAPI.Ingame;

namespace IngameScript.Mockups.Blocks
{
// Decorate with DisplayName and MetadataType attributes.
    [DisplayName("Air Vent"), MetadataType(typeof(MockAirVentMetadata))]
    public partial class MockAirVent { }

// Internal class in same file with correct name convention.
// Must extend from metadata of parent objects parent class.
    internal class MockAirVentMetadata: MockFunctionalBlockMetadata
    {
// Attributes applied to properties.
        [DisplayName("Oxygen Level"), Range(0, 1)]
// All properties are public object regardless of type.
        public object OxygenLevel { get; set; }

        [DisplayName("Can Pressurize")]
        public object CanPressurize { get; set; }

// Properties which are for display only are marked as such.
        [DisplayName("Is Depressurizing"), ReadOnly(true)]
        public object IsDepressurizing { get; set; }

        [DisplayName("Pressurize")]
        public object Depressurize { get; set; }

        [DisplayName("Status")]
        public virtual VentStatus Status { get; set; }
    }
}

```

Additional things to consider:
* Space Engineers only supports C# 6.0, do not use any language features from higher versions (the TestScript will help confirm this).
* Do not create `private` properties unless they are only required for your specific implementation, these classes should be easily extendable.
