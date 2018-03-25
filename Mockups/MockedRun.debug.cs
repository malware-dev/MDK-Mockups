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
        List<MockProgrammableBlock> _programmableBlocks = new List<MockProgrammableBlock>();

        protected MockedRun()
        {
            MockedProgrammableBlocks = new ReadOnlyCollection<MockProgrammableBlock>(_programmableBlocks);
        }

        /// <summary>
        /// Gets or sets the grid terminal system to use during this run. This property must be populated.
        /// </summary>
        public virtual IMyGridTerminalSystem GridTerminalSystem { get; set; }

        /// <summary>
        /// A list of mocked programmable blocks available in the <see cref="GridTerminalSystem"/>. This collection
        /// is filled out during the <see cref="Start"/> of the run.
        /// </summary>
        public ReadOnlyCollection<MockProgrammableBlock> MockedProgrammableBlocks { get; }

        /// <summary>
        /// Echos text onto the scripting console. This is the method used for the script Echo command
        /// </summary>
        /// <param name="text"></param>
        public abstract void Echo(string text);

        /// <summary>
        /// Start the run
        /// </summary>
        public virtual void Start()
        {
            Debug.Assert(GridTerminalSystem != null, nameof(GridTerminalSystem) + " != null");

            FindProgrammableBlocks(_programmableBlocks);
            Starting();
            InstallPrograms();

            long tickCount = 0;
            while (true)
            {
                var runningBlocks = 0;
                foreach (var pb in MockedProgrammableBlocks)
                {
                    UpdateType updateType;
                    if (pb.TryGetUpdateTypeFor(tickCount, out updateType))
                    {
                        runningBlocks++;
                        if (updateType == UpdateType.None)
                            continue;
                        RunProgrammableBlock(pb, null, updateType);
                    }
                }

                if (!Tick(tickCount, runningBlocks))
                    break;

                tickCount++;
            }
        }

        /// <summary>
        /// Called as the mock is starting. The <see cref="MockedProgrammableBlocks"/> collection has been
        /// filled at this point.
        /// </summary>
        protected virtual void Starting()
        {
        }

        /// <summary>
        /// Runs the given programmable block
        /// </summary>
        /// <param name="pb"></param>
        /// <param name="argument"></param>
        /// <param name="flags"></param>
        protected virtual void RunProgrammableBlock(MockProgrammableBlock pb, string argument, UpdateType flags)
        {
            pb.Run(argument, flags);
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
        /// <param name="ticks">The current tick count</param>
        /// <param name="scheduledBlocks"></param>
        /// <returns><c>true</c> if the tick is valid and the run should continue; <c>false</c> to stop the run.</returns>
        protected virtual bool Tick(long ticks, int scheduledBlocks)
        {
            return scheduledBlocks > 0;
        }
    }
}
