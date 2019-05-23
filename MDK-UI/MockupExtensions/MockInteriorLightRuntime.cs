using System;
using System.ComponentModel;
using System.Windows.Media;
using MDK_UI.MockupExtensions;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups.Blocks
{
    [DisplayName("Interior Light")]
    public class MockInteriorLightRuntime: MockInteriorLight, IMockupRuntimeProvider
    {
        public int ProcessPriority => 1;

        private int CurrentTick = 0;

        public override Brush Preview
        {
            get
            {
                var enabled = Enabled;

                if (BlinkIntervalSeconds > 0)
                {
                    var currentSecond = Convert.ToSingle(CurrentTick) / RuntimeConstants.TicksPerSecond;
                    var currentStep = currentSecond % BlinkIntervalSeconds;
                    var offtime = BlinkIntervalSeconds * BlinkLength;
                    var offset = BlinkIntervalSeconds * BlinkOffset;

                    if (currentStep > offset && currentStep <= (offtime + offset))
                        enabled = false;
                }

                return new SolidColorBrush(new Color()
                {
                    R = Color.R,
                    G = Color.G,
                    B = Color.B,
                    A = enabled ? Color.A : (byte) 0
                });
            }
        }

        public MockInteriorLightRuntime()
            :base()
        {
            Radius = 2;
            Intensity = 1;
            Falloff = 1;
            BlinkIntervalSeconds = 0;
            BlinkLength = 0;
            BlinkOffset = 0;
            Color = new VRageMath.Color(255, 255, 255, 255);
            Enabled = true;

            PropertyChanged += (sender, args) => 
            {
                switch (args.PropertyName)
                {
                    case nameof(Radius):
                    case nameof(Intensity):
                    case nameof(Falloff):
                    case nameof(BlinkIntervalSeconds):
                    case nameof(BlinkLength):
                    case nameof(BlinkOffset):
                    case nameof(Color):
                        OnPropertyChanged(nameof(Preview));
                        break;
                }
            };
        }

        public void ProcessGameTick(IMyGridTerminalSystem gridTerminalSystem, int tick)
        {
            CurrentTick = tick;
            if (Enabled && BlinkIntervalSeconds > 0)
            {
                OnPropertyChanged(nameof(Preview));
            }
        }
    }
}
