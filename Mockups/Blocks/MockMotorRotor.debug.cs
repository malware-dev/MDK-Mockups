using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups.Blocks
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public partial class MockMotorRotor : MockCubeBlock, IMyMotorRotor
    {
        public virtual bool IsAttached => Base != null;
        public virtual IMyMechanicalConnectionBlock Base { get; set; }
    }
}
