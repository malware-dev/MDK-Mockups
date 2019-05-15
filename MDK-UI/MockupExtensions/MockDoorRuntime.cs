using MDK_UI.MockupExtensions;
using Sandbox.ModAPI.Ingame;
using System.ComponentModel;

namespace IngameScript.Mockups.Blocks
{
    [DisplayName("Sliding Door")]
    public class MockRuntimeDoor: MockDoor, IMockupRuntimeProvider
    {
        const float OpenRate = 0.1666f;

        public event PropertyChangedEventHandler PropertyChanged;

        public int ProcessPriority => 1;

        public override string CustomName
        {
            get => base.CustomName;
            set
            {
                if (base.CustomName != value)
                {
                    base.CustomName = value;
                    OnPropertyChanged(nameof(CustomName));
                }
            }
        }

        public override DoorStatus Status
        {
            get => base.Status;
            set
            {
                if (base.Status != value)
                {
                    base.Status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public override float OpenRatio
        {
            get => base.OpenRatio;
            set
            {
                if (base.OpenRatio != value)
                {
                    base.OpenRatio = value;
                    OnPropertyChanged(nameof(OpenRatio));
                }
            }
        }

        public override void OpenDoor()
        {
            if (Status != DoorStatus.Open)
                Status = DoorStatus.Opening;
        }

        public override void CloseDoor()
        {
            if (Status != DoorStatus.Closed)
                Status = DoorStatus.Closing;
        }
        
        public void ProcessGameTick(IMyGridTerminalSystem gridTerminalSystem, int tick)
        {
            switch (Status)
            {
                case DoorStatus.Closed:
                    if (OpenRatio != 0)
                        OpenRatio = 0;
                    break;
                case DoorStatus.Open:
                    if (OpenRatio != 1)
                        OpenRatio = 1;
                    break;

                case DoorStatus.Closing:
                    OpenRatio -= OpenRate;
                    break;

                case DoorStatus.Opening:
                    OpenRatio += OpenRate;
                    break;
            }

            if (OpenRatio >= 1 && Status != DoorStatus.Open)
                Status = DoorStatus.Open;

            if (OpenRatio <= 0 && Status != DoorStatus.Closed)
                Status = DoorStatus.Closed;
        }

        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
