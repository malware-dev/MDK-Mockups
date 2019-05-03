using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups.Blocks
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public class MockMotorRotor : MockCubeBlock, IMyMotorRotor
    {
        public bool IsAttached => Base != null;
        public IMyMechanicalConnectionBlock Base { get; set; }
    }
}
