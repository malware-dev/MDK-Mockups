using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

namespace IngameScript.Mockups.Base
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public abstract partial class MockFunctionalBlock : MockTerminalBlock, IMyFunctionalBlock
    {
        private bool _enabled = true;

        [DisplayName("Enabled")]
        public virtual bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    OnPropertyChanged();
                }
            }
        }

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
