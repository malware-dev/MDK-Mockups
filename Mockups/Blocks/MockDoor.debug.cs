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
        protected float _openRatio = 0;
        protected float _lastRatio = 0;

        public bool Open => OpenRatio != 0;

        public DoorStatus Status
        {
            get
            {
                if (OpenRatio == 0)
                    return DoorStatus.Closed;

                if (OpenRatio == 1)
                    return DoorStatus.Open;

                if (OpenRatio > _lastRatio)
                    return DoorStatus.Opening;

                return DoorStatus.Closing;
            }
        }

        public float OpenRatio
        {
            get => _openRatio;
            set
            {
                _lastRatio = _openRatio;
                _openRatio = value;
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

        public virtual void CloseDoor()
        {
            if (Enabled && IsFunctional)
            {
                OpenRatio = 0;
            }
        }

        public virtual void OpenDoor()
        {
            if (Enabled && IsFunctional)
            {
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
