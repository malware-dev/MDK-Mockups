using IngameScript.Mockups.Base;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Text;

namespace IngameScript.Mockups.Blocks
{
    public class MockAirVent : MockTerminalBlock, IMyAirVent
    {
        public bool CanPressurize { get; set; }

        public bool IsDepressurizing { get; set; }

        public bool Depressurize { get; set; } = false;

        public VentStatus Status { get; set; }

        public bool PressurizationEnabled { get; } = true;

        public bool Enabled { get; set; } = true;
        
        public float GetOxygenLevel()
        {
            throw new NotImplementedException();
        }

        public void RequestEnable(bool enable) => Enabled = enable;

        public void SetCustomName(string text) => CustomName = text;

        public void SetCustomName(StringBuilder text) => CustomName = text.ToString();

        public bool IsPressurized() => PressurizationEnabled && (Status == VentStatus.Pressurized || Status == VentStatus.Pressurizing);
    }
}
