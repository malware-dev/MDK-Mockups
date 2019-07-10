Here is an example template for mocking an ingame block.

```cs
// Usings outside the namespace.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;

// Always implement with this namespace.
namespace IngameScript.Mockups.Blocks
{
// Always include this conditional (to allow testing with the TestScript project
#if !MOCKUP_DEBUG
// Force step through for users including the shared project.
    [System.Diagnostics.DebuggerNonUserCode]
#endif
// Always declare as public and partial.
// Extend from MockFunctionalBlock if the interface implements IMyFunctionalBlock, otherwise extend from MockTerminalBlock
    public partial class MockAirVent : MockFunctionalBlock, IMyAirVent
    {
// Implement in-game default values where possible.
// Mark properties as virtual for MDK-UI overrides.    
        public virtual float OxygenLevel { get; set; } = 0;

        public virtual bool CanPressurize { get; set; } = true;

// Implement sensible auto-compute values where possible.
// Mark these as virtual for MDK-UI overrides.
        public virtual bool IsDepressurizing => Enabled && (Status == VentStatus.Depressurizing || Status == VentStatus.Depressurized);

        public virtual bool Depressurize { get; set; } = false;

        public virtual VentStatus Status { get; set; }

// Implement in-game default values for properties when possible.
        public bool PressurizationEnabled { get; } = true;

// Implement an override of CreateTerminalProperties() to include the extra properties exposed by the legacy GetProperties() method.
        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new[]
            {
                new MockTerminalProperty<IMyAirVent, bool>("Depressurize", b => b.Depressurize, (b, v) => b.Depressurize = v)
            });
        }

// Implement public backing properties for methods to allow mocking of return values.
// Mark custom methods as virtual in case MDK-UI needs to override them.
        public virtual float GetOxygenLevel() => OxygenLevel;
        
        public virtual bool IsPressurized() => PressurizationEnabled && (Status == VentStatus.Pressurized || Status == VentStatus.Pressurizing);
    }
}
```

Additional things to consider:
* Space Engineers only supports C# 6.0, do not use any language features from higher versions (the TestScript will help confirm this).
* Do not create `private` properties unless they are only required for your specific implementation, these classes should be easily extendable.
