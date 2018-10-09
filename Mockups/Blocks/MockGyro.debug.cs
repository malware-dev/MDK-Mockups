using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System.Collections.Generic;

namespace IngameScript.Mockups.Blocks
{
    public class MockGyro : MockFunctionalBlock, IMyGyro
    {
        protected override IEnumerable<ITerminalProperty> Properties { get; } = new List<ITerminalProperty>()
        {
            new MockBoolTerminalProperty<IMyGyro>("OnOff", b => b.Enabled),
            new MockBoolTerminalProperty<IMyGyro>("ShowInTerminal", b => b.ShowInTerminal),
            new MockBoolTerminalProperty<IMyGyro>("ShowInToolbarConfig", b => b.ShowInToolbarConfig),
            new MockBoolTerminalProperty<IMyGyro>("ShowOnHUD", b => b.ShowOnHUD),
            new MockFloatTerminalProperty<IMyGyro>("Power", b => b.GyroPower),
            new MockBoolTerminalProperty<IMyGyro>("Override", b => b.GyroOverride),
            new MockFloatTerminalProperty<IMyGyro>("Yaw", b => b.Yaw),
            new MockFloatTerminalProperty<IMyGyro>("Pitch", b => b.Pitch),
            new MockFloatTerminalProperty<IMyGyro>("Roll", b => b.Roll)
        };

        public virtual float GyroPower { get; set; }

        public virtual bool GyroOverride { get; set; }

        public virtual float Yaw { get; set; }

        public virtual float Pitch { get; set; }

        public virtual float Roll { get; set; }
    }
}
