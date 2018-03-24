using Sandbox.ModAPI.Ingame;

namespace IngameScript.Base
{
    public abstract class MockFunctionalBlock : MockTerminalBlock, IMyFunctionalBlock
    {
        public virtual bool Enabled { get; set; }

        public void RequestEnable(bool enable)
        {
            Enabled = enable;
        }
    }
}