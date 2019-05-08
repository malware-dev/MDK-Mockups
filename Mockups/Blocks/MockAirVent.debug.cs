using System;
using System.Collections.Generic;
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
    public partial class MockAirVent : MockFunctionalBlock, IMyAirVent
    {
        public virtual float OxygenLevel { get; set; } = 0;

        public virtual bool CanPressurize { get; set; } = true;

        public virtual bool IsDepressurizing => Enabled && (Status == VentStatus.Depressurizing || Status == VentStatus.Depressurized);

        public virtual bool Depressurize { get; set; } = false;

        public virtual VentStatus Status { get; set; }

        public virtual bool PressurizationEnabled { get; } = true;

        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new[]
            {
                new MockTerminalProperty<IMyAirVent, bool>("Depressurize", b => b.Depressurize, (b, v) => b.Depressurize = v)
            });
        }

        public virtual float GetOxygenLevel() => OxygenLevel;

        public virtual bool IsPressurized() => PressurizationEnabled && (Status == VentStatus.Pressurized || Status == VentStatus.Pressurizing);
    }
}
