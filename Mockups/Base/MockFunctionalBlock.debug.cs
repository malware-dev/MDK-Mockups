using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

namespace IngameScript.Mockups.Base
{
    public abstract class MockFunctionalBlock : MockTerminalBlock, IMyFunctionalBlock
    {
        public virtual bool Enabled { get; set; } = true;

        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new[]
            {
                new MockTerminalProperty<IMyFunctionalBlock, bool>("OnOff", b => b.Enabled, (b, v) => b.Enabled = v, true)
            });
        }

        [Obsolete("Use " + nameof(Enabled) + " instead")]
        public void RequestEnable(bool enable) => Enabled = enable;
    }
}
