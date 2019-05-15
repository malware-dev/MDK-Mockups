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
        public override float Radius
        {
            get => base.Radius;
            set
            {
                if (value != base.Radius)
                {
                    base.Radius = value;

                    OnPropertyChanged(nameof(Radius));
                    // OnPropertyChanged(nameof(Preview));
                }
            }
        }

        public override float Intensity
        {
            get => base.Intensity;
            set
            {
                if (value != base.Intensity)
                {
                    base.Intensity = value;

                    OnPropertyChanged(nameof(Intensity));
                    // OnPropertyChanged(nameof(Preview));
                }
            }
        }

        public override float Falloff
        {
            get => base.Falloff;
            set
            {
                if (value != base.Falloff)
                {
                    base.Falloff = value;

                    OnPropertyChanged(nameof(Falloff));
                    // OnPropertyChanged(nameof(Preview));
                }
            }
        }

        public override float BlinkIntervalSeconds
        {
            get => base.BlinkIntervalSeconds;
            set
            {
                if (value != base.BlinkIntervalSeconds)
                {
                    base.BlinkIntervalSeconds = value;

                    OnPropertyChanged(nameof(BlinkIntervalSeconds));
                    OnPropertyChanged(nameof(Preview));
                }
            }
        }

        public override float BlinkLength
        {
            get => base.BlinkLength;
            set
            {
                if (value != base.BlinkLength)
                {
                    base.BlinkLength = value;

                    OnPropertyChanged(nameof(BlinkLength));
                    OnPropertyChanged(nameof(Preview));
                }
            }
        }

        public override float BlinkOffset
        {
            get => base.BlinkOffset;
            set
            {
                if (value != base.BlinkOffset)
                {
                    base.BlinkOffset = value;

                    OnPropertyChanged(nameof(BlinkOffset));
                    OnPropertyChanged(nameof(Preview));
                }
            }
        }

        public override VRageMath.Color Color
        {
            get => base.Color;
            set
            {
                if (value != base.Color)
                {
                    base.Color = value;

                    OnPropertyChanged(nameof(Color));
                    OnPropertyChanged(nameof(Preview));
                }
            }
        }

        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                if (value != base.Enabled)
                {
                    base.Enabled = value;

                    OnPropertyChanged(nameof(Enabled));
                    OnPropertyChanged(nameof(Preview));
                }
            }
        }

        public Brush Preview
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

        public event PropertyChangedEventHandler PropertyChanged;

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
        }

        public void ProcessGameTick(IMyGridTerminalSystem gridTerminalSystem, int tick)
        {
            CurrentTick = tick;
            if (Enabled && BlinkIntervalSeconds > 0)
            {
                OnPropertyChanged(nameof(Preview));
            }
        }

        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
