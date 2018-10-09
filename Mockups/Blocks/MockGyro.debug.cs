using System.Collections.Generic;
using System.Linq;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

namespace IngameScript.Mockups.Blocks
{
    public class MockGyro : MockFunctionalBlock, IMyGyro
    {
        public virtual float GyroPower { get; set; }

        public virtual bool GyroOverride { get; set; }

        public virtual float Yaw { get; set; }

        public virtual float Pitch { get; set; }

        public virtual float Roll { get; set; }

        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new ITerminalProperty[]
            {
                new MockTerminalProperty<IMyGyro, float>("Power", b => b.GyroPower, (b, v) => b.GyroPower = v),
                new MockTerminalProperty<IMyGyro, bool>("Override", b => b.GyroOverride, (b, v) => b.GyroOverride = v),
                new MockTerminalProperty<IMyGyro, float>("Yaw", b => b.Yaw, (b, v) => b.Yaw = v),
                new MockTerminalProperty<IMyGyro, float>("Pitch", b => b.Pitch, (b, v) => b.Pitch = v),
                new MockTerminalProperty<IMyGyro, float>("Roll", b => b.Roll, (b, v) => b.Roll = v)
            });
        }
    }
}
