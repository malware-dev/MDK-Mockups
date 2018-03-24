using System;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups
{
    public class MockRuntime : IMyGridProgramRuntimeInfo
    {
        public virtual TimeSpan TimeSinceLastRun { get; set; }

        public virtual double LastRunTimeMs { get; set; }

        public virtual int MaxInstructionCount { get; set; }

        public virtual int CurrentInstructionCount { get; set; }

        public virtual int MaxMethodCallCount { get; set; }

        public virtual int CurrentMethodCallCount { get; set; }

        public virtual UpdateFrequency UpdateFrequency { get; set; }
    }
}