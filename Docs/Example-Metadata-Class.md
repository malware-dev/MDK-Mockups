Here is an example template for decorating a mocked ingame block.

```cs
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;

namespace IngameScript.Mockups.Blocks
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
// Decorate the class with a DisplayName attribute to have it visible in the block picker.
    [DisplayName("Air Vent")]
    public partial class MockAirVent : MockFunctionalBlock, IMyAirVent
    {
// Decorate a property with a DisplayName attribute to have it visible in the block details screen.
// Add Range and ReadOnly attributes when appropriate to control how the property is rendered.
        [DisplayName("Oxygen Level"), Range(0, 1)]
        public virtual float OxygenLevel { get; set; } = 0;

        [DisplayName("Can Pressurize")]
        public virtual bool CanPressurize { get; set; } = true;

        [DisplayName("Is Depressurizing"), ReadOnly(true)]
        public virtual bool IsDepressurizing => Enabled && (Status == VentStatus.Depressurizing || Status == VentStatus.Depressurized);

        [DisplayName("De-pressurize")]
        public virtual bool Depressurize { get; set; } = false;

        [DisplayName("Status")]
        public virtual VentStatus Status { get; set; }

        public virtual bool PressurizationEnabled { get; } = true;

        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new[]
            {
                new MockTerminalProperty<IMyAirVent, bool>("Depressurize", b => b.Depressurize, (b, v) => b.Depressurize = v)
            });
        }

// Decorate methods with a DisplayName attribute to add them to the list of actions in the block details screen.
        [DisplayName("Get Oxygen Level")]
        public virtual float GetOxygenLevel() => OxygenLevel;

        public virtual bool IsPressurized() => PressurizationEnabled && (Status == VentStatus.Pressurized || Status == VentStatus.Pressurizing);
    }
}
```

Additional things to consider:
* Space Engineers only supports C# 6.0, do not use any language features from higher versions (the TestScript will help confirm this).
* Do not create `private` properties unless they are only required for your specific implementation, these classes should be easily extendable.
