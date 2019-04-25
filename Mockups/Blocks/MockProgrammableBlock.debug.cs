using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IngameScript.Mockups.Base;
using Malware.MDKUtilities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using IMyProgrammableBlock = Sandbox.ModAPI.Ingame.IMyProgrammableBlock;
using IMyTextSurface = Sandbox.ModAPI.Ingame.IMyTextSurface;

namespace IngameScript.Mockups.Blocks
{
    public class MockProgrammableBlock : MockFunctionalBlock, IMyProgrammableBlock
    {
        string _storage = string.Empty;
        readonly IMyTextSurface _primary = new MockTextSurface(new VRageMath.Vector2(512, 512), new VRageMath.Vector2(512, 512));
        readonly IMyTextSurface _keyboard = new MockTextSurface(new VRageMath.Vector2(512, 256), new VRageMath.Vector2(512, 256));

        public virtual int SurfaceCount { get; } = 1;
     
        public virtual Type ProgramType { get; set; }

        public virtual IMyGridProgram Program { get; set; }

        public virtual IMyGridProgramRuntimeInfo Runtime { get; set; } = new MockGridProgramRuntimeInfo();

        public virtual string Storage
        {
            get { return _storage; }
            set { _storage = value ?? ""; }
        }

        public virtual bool IsRunning { get; set; }

        public virtual string TerminalRunArgument { get; set; }

        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new[]
            {
                // Ugh... >_<
                new MockTerminalProperty<IMyProgrammableBlock, StringBuilder>("Title", b => new StringBuilder(TerminalRunArgument), (b, v) => TerminalRunArgument = v.ToString())
            });
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
                ToggleOnceFlag();
                MDKFactory.Run(Program, argument ?? "", updateType: updateType);
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

            Debug.Assert(Runtime != null, $"{nameof(Runtime)} != null");
            var config = new MDKFactory.ProgramConfig
            {
                GridTerminalSystem = mockedRun.GridTerminalSystem,
                Runtime = Runtime,
                ProgrammableBlock = this,
                Echo = mockedRun.Echo,
                Storage = Storage
            };
            Program = MDKFactory.CreateProgram(ProgramType, config);
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
                updateType |= UpdateType.Once;

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

        public override void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public virtual IMyTextSurface GetSurface(int index)
        {
            switch (index)
            {
                case 0:
                    return _primary;
                case 1:
                    return _keyboard;
                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }
}
