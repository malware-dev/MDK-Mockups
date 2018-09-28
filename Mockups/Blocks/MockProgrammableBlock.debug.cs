﻿using System;
using System.Diagnostics;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using IMyProgrammableBlock = Sandbox.ModAPI.Ingame.IMyProgrammableBlock;

namespace IngameScript.Mockups.Blocks
{
    public class MockProgrammableBlock : MockFunctionalBlock, IMyProgrammableBlock
    {
        string _storage = string.Empty;

        public virtual Type ProgramType { get; set; }

        public virtual IMyGridProgram Program { get; set; }

        public virtual IMyGridProgramRuntimeInfo Runtime { get; set; }

        public virtual string Storage
        {
            get { return _storage; }
            set { _storage = value ?? ""; }
        }

        public virtual bool IsRunning { get; set; }

        public virtual string TerminalRunArgument { get; set; }

        public MockProgrammableBlock()
        {
            Runtime = new MockGridProgramRuntimeInfo();
        }

        public virtual bool Run(string argument, UpdateType updateType)
        {
            if (!Enabled)
                return false;
            if (Program == null)
                return false;
            if (IsRunning)
                return false;
            try
            {
                IsRunning = true;
                Malware.MDKUtilities.MDK.Run(Program, argument ?? "", updateType: updateType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                IsRunning = false;
            }
        }

        public virtual bool TryRun(string argument)
        {
            return Run(argument, UpdateType.Script);
        }

        /// <summary>
        /// Installs the program for the given mocked run. Any programmable block may only be
        /// a part of a single mocked run or behavior is undefined.
        /// </summary>
        /// <param name="mockedRun"></param>
        public virtual void Install(MockedRun mockedRun)
        {
            if (Program != null)
                return;
            if (ProgramType == null)
                return;

            Debug.Assert(Runtime != null, "Runtime != null");
            var config = new Malware.MDKUtilities.MDK.ProgramConfig
            {
                GridTerminalSystem = mockedRun.GridTerminalSystem,
                Runtime = Runtime,
                ProgrammableBlock = this,
                Echo = mockedRun.Echo,
                Storage = Storage
            };
            Program = Malware.MDKUtilities.MDK.CreateProgram(ProgramType, config);
        }

        /// <summary>
        /// Attempts to retrieve the update type for this block.
        /// </summary>
        /// <param name="ticks">The tick count to get the update type for</param>
        /// <param name="updateType">The detected update type</param>
        /// <returns><c>true</c> if this block is scheduled for a later run, <c>false</c> if the block is effectively halted.</returns>
        public virtual bool TryGetUpdateTypeFor(long ticks, out UpdateType updateType)
        {
            updateType = UpdateType.None;
            var runtime = Runtime;
            if (runtime == null || runtime.UpdateFrequency == UpdateFrequency.None)
                return false;

            if ((runtime.UpdateFrequency & UpdateFrequency.Once) != 0)
            {
                updateType |= UpdateType.Once;
                runtime.UpdateFrequency &= ~UpdateFrequency.Once;
                if (runtime.UpdateFrequency == UpdateFrequency.None)
                    return false;
            }

            if ((runtime.UpdateFrequency & UpdateFrequency.Update1) != 0)
                updateType |= UpdateType.Update1;

            if ((runtime.UpdateFrequency & UpdateFrequency.Update10) != 0 && ticks % 10 == 0)
                updateType |= UpdateType.Update10;

            if ((runtime.UpdateFrequency & UpdateFrequency.Update100) != 0 && ticks % 100 == 0)
                updateType |= UpdateType.Update100;

            return updateType != UpdateType.None;
        }

        /// <summary>
        /// Determines if this programmable block is scheduled to be run at a later stage.
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public virtual bool IsScheduledForLater(long ticks)
        {
            var runtime = Runtime;
            if (runtime == null)
                return false;

            var freq = runtime.UpdateFrequency;
            if (freq == UpdateFrequency.None)
                return false;

            return true;
        }

        /// <summary>
        /// Removes the Once flag from this programmable block.
        /// </summary>
        public virtual void ToggleOnceFlag()
        {
            var runtime = Runtime;
            if (runtime == null)
                return;

            runtime.UpdateFrequency &= ~UpdateFrequency.Once;
        }
    }
}