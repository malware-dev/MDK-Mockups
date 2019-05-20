using MDK_UI.MockupExtensions;
using Sandbox.ModAPI.Ingame;
using System.ComponentModel;
using System.Windows.Media;

namespace IngameScript.Mockups.Blocks
{
    [DisplayName("Sliding Door")]
    public class MockRuntimeDoor: MockDoor, IMockupRuntimeProvider
    {
        const float OpenRate = 0.1666f;

        public int ProcessPriority => 1;

        public override Brush Preview
        {
            get
            {
                if (!Enabled)
                    return base.Preview;

                switch (Status)
                {
                    case DoorStatus.Closed:
                        return new SolidColorBrush(new Color
                        {
                            R = 255,
                            G = 0,
                            B = 0,
                            A = 255
                        });
                    case DoorStatus.Closing:
                    case DoorStatus.Opening:
                        return new SolidColorBrush(new Color
                        {
                            R = 255,
                            G = 255,
                            B = 0,
                            A = 255
                        });
                    case DoorStatus.Open:
                        return new SolidColorBrush(new Color
                        {
                            R = 0,
                            G = 255,
                            B = 0,
                            A = 255
                        });
                }

                return base.Preview;
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
    }
}
