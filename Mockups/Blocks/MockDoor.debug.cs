using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IngameScript.Mockups.Blocks
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [DisplayName("Sliding Door")]
    public partial class MockDoor : MockFunctionalBlock, IMyDoor
    {
        private DoorStatus _status = DoorStatus.Closed;

        private float _openRatio = 0;

        public virtual bool Open => OpenRatio != 0;


        [DisplayName("Status"), ReadOnly(true)]
        public virtual DoorStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        [DisplayName("Open Ratio"), Range(0d, 1d)]
        public virtual float OpenRatio
        {
            get { return _openRatio; }
            set
            {
                if (_openRatio != value)
                {
                    _openRatio = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new[]
            {
                new MockTerminalProperty<IMyDoor, bool>("Open", 
                    (b) => b.Status == DoorStatus.Open || b.Status == DoorStatus.Opening, 
                    (b, v) => 
                    {
                        if (v) {
                            b.OpenDoor();
                        } else {
                            b.CloseDoor();
                        }
                    }
                , false)
            });
        }

        [DisplayName("Close")]
        public virtual void CloseDoor()
        {
            if (Enabled && IsFunctional)
            {
                Status = DoorStatus.Closed;
                OpenRatio = 0;
            }
        }

        [DisplayName("Open")]
        public virtual void OpenDoor()
        {
            if (Enabled && IsFunctional)
            {
                Status = DoorStatus.Open;
                OpenRatio = 1;
            }
        }

        [DisplayName("Toggle")]
        public virtual void ToggleDoor()
        {
            switch (Status)
            {
                case DoorStatus.Open:
                case DoorStatus.Opening:
                    CloseDoor();
                    return;
                case DoorStatus.Closed:
                case DoorStatus.Closing:
                    OpenDoor();
                    return;
            }
        }
    }
}
