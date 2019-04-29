using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;

namespace IngameScript.Mockups.Blocks
{
    public class MockAirVent : MockFunctionalBlock, IMyAirVent
    {
        public float OxygenLevel { get; set; } = 0;

        public bool CanPressurize { get; set; }

        public bool IsDepressurizing { get; set; }

        public bool Depressurize { get; set; } = false;

        public VentStatus Status { get; set; }

        public bool PressurizationEnabled { get; } = true;

        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new[]
            {
                new MockTerminalProperty<IMyAirVent, bool>("Depressurize", b => b.Depressurize, (b, v) => b.Depressurize = v)
            });
        }

        public float GetOxygenLevel() => OxygenLevel;

        public void SetCustomName(string text) => CustomName = text;

        public void SetCustomName(StringBuilder text) => CustomName = text.ToString();

        public bool IsPressurized() => PressurizationEnabled && (Status == VentStatus.Pressurized || Status == VentStatus.Pressurizing);
    }
}
