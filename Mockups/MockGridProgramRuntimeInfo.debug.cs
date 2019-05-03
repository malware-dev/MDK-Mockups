using System;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public class MockGridProgramRuntimeInfo : IMyGridProgramRuntimeInfo
    {
        public virtual TimeSpan TimeSinceLastRun { get; set; }

        public virtual double LastRunTimeMs { get; set; }

        public virtual int MaxInstructionCount { get; set; }

        public virtual int CurrentInstructionCount { get; set; }

        public virtual int MaxCallChainDepth { get; set; }

        public virtual int CurrentCallChainDepth { get; set; }

        public virtual UpdateFrequency UpdateFrequency { get; set; }
    }
}
