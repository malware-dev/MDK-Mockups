using System;
using IngameScript.Base;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Blocks
{
    public class MockProgrammableBlock : MockFunctionalBlock, IMyProgrammableBlock
    {
        public virtual bool IsRunning { get; set; }

        public virtual string TerminalRunArgument { get; set; }

        public virtual bool TryRun(string argument)
        {
            throw new NotImplementedException();
        }
    }
}