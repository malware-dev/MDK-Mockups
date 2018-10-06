using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using IngameScript.Mockups.Blocks;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups
{
    /// <summary>
    /// Represents a mocked-up run of one or more scripts
    /// </summary>
    public abstract class MockedRun
    {
        readonly List<MockProgrammableBlock> _programmableBlocks = new List<MockProgrammableBlock>();
        long _tickCount;

        protected MockedRun()
        {
            MockedProgrammableBlocks = new ReadOnlyCollection<MockProgrammableBlock>(_programmableBlocks);
        }

        /// <summary>
        /// Determines whether this run has been initialized. 
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets or sets the grid terminal system to use during this run. This property must be populated.
        /// </summary>
        public virtual IMyGridTerminalSystem GridTerminalSystem { get; set; }

        /// <summary>
        /// A list of mocked programmable blocks available in the <see cref="GridTerminalSystem"/>. This collection
        /// is filled out during the initialization of the run.
        /// </summary>
        public ReadOnlyCollection<MockProgrammableBlock> MockedProgrammableBlocks { get; }

        /// <summary>
        /// Echos text onto the scripting console. This is the method used for the script Echo command
        /// </summary>
        /// <param name="text"></param>
        public abstract void Echo(string text);

        /// <summary>
        /// Runs a single tick of this mocked run.
        /// </summary>
        /// <returns><c>true</c> if the run should continue; <c>false</c> otherwise.</returns>
        public bool NextTick()
        {
            MockedRunFrame frame;
            return NextTick(out frame);
        }

        /// <summary>
        /// Runs a single tick of this mocked run.
        /// </summary>
        /// <returns><c>true</c> if the run should continue; <c>false</c> otherwise.</returns>
        public virtual bool NextTick(out MockedRunFrame frame)
        {
            EnsureInit();
            var scheduledPBs = 0;
            var runPBs = 0;
            foreach (var pb in MockedProgrammableBlocks)
            {
                UpdateType updateType;
                if (pb.TryGetUpdateTypeFor(_tickCount, out updateType))
                {
                    RunProgrammableBlock(pb, null, updateType);
                    runPBs++;
                }
                if (pb.IsScheduledForLater(_tickCount))
                    scheduledPBs++;
            }

            frame = new MockedRunFrame(_tickCount, scheduledPBs > 0, scheduledPBs, runPBs);
            _tickCount++;
            return scheduledPBs > 0;
        }

        /// <summary>
        /// Makes sure that the Run has been initialized.
        /// </summary>
        protected void EnsureInit()
        {
            if (IsInitialized)
                return;
            IsInitialized = true;

            Debug.Assert(GridTerminalSystem != null, nameof(GridTerminalSystem) + " != null");

            _tickCount = 0;
            FindProgrammableBlocks(_programmableBlocks);
            Starting();
            InstallPrograms();
        }

        /// <summary>
        /// Runs a programmable block by its name.
        /// </summary>
        /// <param name="programmableBlockName"></param>
        /// <param name="argument"></param>
        /// <param name="updateType"></param>
        public void Trigger(string programmableBlockName, string argument = null, UpdateType updateType = UpdateType.Trigger)
        {
            var pb = GridTerminalSystem.GetBlockWithName(programmableBlockName) as MockProgrammableBlock;
            if (pb == null)
                throw new InvalidOperationException($"Cannot find a mocked programmable block named {programmableBlockName}");
            EnsureInit();
            RunProgrammableBlock(pb, argument, updateType);
        }

        /// <summary>
        /// Runs a programmable block by its entity ID.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="argument"></param>
        /// <param name="updateType"></param>
        public void Trigger(long entityId, string argument = null, UpdateType updateType = UpdateType.Trigger)
        {
            var pb = GridTerminalSystem.GetBlockWithId(entityId) as MockProgrammableBlock;
            if (pb == null)
                throw new InvalidOperationException($"Cannot find a mocked programmable block with the ID {entityId}");
            EnsureInit();
            RunProgrammableBlock(pb, argument, updateType);
        }

        /// <summary>
        /// Called as the mock is starting. The <see cref="MockedProgrammableBlocks"/> collection has been
        /// filled at this point.
        /// </summary>
        protected virtual void Starting()
        { }

        /// <summary>
        /// Runs the given programmable block
        /// </summary>
        /// <param name="pb"></param>
        /// <param name="argument"></param>
        /// <param name="updateType"></param>
        protected virtual void RunProgrammableBlock(MockProgrammableBlock pb, string argument, UpdateType updateType)
        {
            pb.Run(argument, updateType);
        }

        /// <summary>
        /// Runs through all the detected mocked programmable blocks and
        /// installs them.
        /// </summary>
        protected virtual void InstallPrograms()
        {
            foreach (var pb in MockedProgrammableBlocks)
                pb.Install(this);
        }

        /// <summary>
        /// Finds all mocked programmable blocks and places them in the given list
        /// </summary>
        /// <param name="programmableBlocks"></param>
        protected virtual void FindProgrammableBlocks(List<MockProgrammableBlock> programmableBlocks)
        {
            GridTerminalSystem.GetBlocksOfType<IMyProgrammableBlock>(null as List<IMyTerminalBlock>, pb =>
            {
                var mock = pb as MockProgrammableBlock;
                if (mock != null)
                    programmableBlocks.Add(mock);
                return false;
            });
        }

        /// <summary>
        /// Called after all programmable blocks have been invoked.
        /// </summary>
        /// <param name="scheduledBlocks"></param>
        /// <returns><c>true</c> if the tick is valid and the run should continue; <c>false</c> to stop the run.</returns>
        protected virtual bool OnTick(int scheduledBlocks)
        {
            return scheduledBlocks > 0;
        }
    }
}
