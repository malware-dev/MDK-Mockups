using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;

namespace IngameScript.Mockups.Blocks
{
    public class MockAirVent : MockFunctionalBlock, IMyAirVent
    {
        protected override IEnumerable<ITerminalProperty> Properties { get; } = new List<ITerminalProperty>()
        {
            new MockBoolTerminalProperty<IMyAirVent>("OnOff", b => b.Enabled),
            new MockBoolTerminalProperty<IMyAirVent>("ShowInTerminal", b => b.ShowInTerminal),
            new MockBoolTerminalProperty<IMyAirVent>("ShowInToolbarConfig", b => b.ShowInToolbarConfig),
            new MockBoolTerminalProperty<IMyAirVent>("ShowOnHUD", b => b.ShowOnHUD),
            new MockBoolTerminalProperty<IMyAirVent>("Depressurize", b => b.Depressurize)
        };

        public bool CanPressurize { get; set; }

        public bool IsDepressurizing { get; set; }

        public bool Depressurize { get; set; } = false;

        public VentStatus Status { get; set; }

        public bool PressurizationEnabled { get; } = true;
        
        public float GetOxygenLevel()
        {
            throw new NotImplementedException();
        }

        public void SetCustomName(string text) => CustomName = text;

        public void SetCustomName(StringBuilder text) => CustomName = text.ToString();

        public bool IsPressurized() => PressurizationEnabled && (Status == VentStatus.Pressurized || Status == VentStatus.Pressurizing);
    }
}
