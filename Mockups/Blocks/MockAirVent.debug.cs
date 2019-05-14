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
    [DisplayName("Air Vent")]
    public partial class MockAirVent : MockFunctionalBlock, IMyAirVent
    {
        [DisplayName("Oxygen Level"), Range(0d, 1d)]
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

        [DisplayName("Get Oxygen Level")]
        public virtual float GetOxygenLevel() => OxygenLevel;

        public virtual bool IsPressurized() => PressurizationEnabled && (Status == VentStatus.Pressurized || Status == VentStatus.Pressurizing);
    }
}
