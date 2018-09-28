using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups.Base
{
    public abstract class MockFunctionalBlock : MockTerminalBlock, IMyFunctionalBlock
    {
        public virtual bool Enabled { get; set; }

        public MockFunctionalBlock()
        {
            Enabled = true;
        }

        public void RequestEnable(bool enable)
        {
            Enabled = enable;
        }
    }
}