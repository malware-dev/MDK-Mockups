using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IngameScript.Mockups.Blocks
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public partial class MockDoor : MockFunctionalBlock, IMyDoor
    {
        public bool Open => OpenRatio != 0;

        public virtual DoorStatus Status { get; set; } = DoorStatus.Closed;

        public virtual float OpenRatio { get; set; } = 0;

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

        public virtual void CloseDoor()
        {
            if (Enabled && IsFunctional)
            {
                Status = DoorStatus.Closed;
                OpenRatio = 0;
            }
        }

        public virtual void OpenDoor()
        {
            if (Enabled && IsFunctional)
            {
                Status = DoorStatus.Open;
                OpenRatio = 1;
            }
        }

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
