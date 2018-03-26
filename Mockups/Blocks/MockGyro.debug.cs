using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups.Blocks
{
    public class MockGyro : MockFunctionalBlock, IMyGyro
    {
        public virtual float GyroPower { get; set; }

        public virtual bool GyroOverride { get; set; }

        public virtual float Yaw { get; set; }

        public virtual float Pitch { get; set; }

        public virtual float Roll { get; set; }
    }
}
