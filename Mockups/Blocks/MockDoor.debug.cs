using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IngameScript.Mockups.Blocks
{
    public class MockDoor : MockFunctionalBlock, IMyDoor
    {
        public bool Open { get; set; } = false;

        public DoorStatus Status { get; set; } = DoorStatus.Closed;

        public float OpenRatio { get; set; } = 0;

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

        public void CloseDoor()
        {
            if (Enabled && IsFunctional)
            {
                Open = false;
                Status = DoorStatus.Closed;
                OpenRatio = 0;
            }
        }

        public void OpenDoor()
        {
            if (Enabled && IsFunctional)
            {
                Open = true;
                Status = DoorStatus.Open;
                OpenRatio = 1;
            }
        }

        public void ToggleDoor()
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
