using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRageMath;

namespace IngameScript.Mockups.Blocks
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [DisplayName("Interior Light")]
    public partial class MockInteriorLight : MockFunctionalBlock, IMyInteriorLight
    {
        protected float _offset;
        protected float _radius = 2;
        protected float _intensity = 1;
        protected float _falloff = 1;
        protected float _blinkIntervalSeconds = 0;
        protected float _blinkLength = 0;
        protected float _blinkOffset = 0;
        protected Color _color = new Color(255, 255, 255);

        [DisplayName("Radius")]
        public virtual float Radius
        {
            get { return _radius; }
            set
            {
                if (_radius != value)
                {
                    _radius = value;
                    OnPropertyChanged();
                }
            }
        }

        [DisplayName("Intensity")]
        public virtual float Intensity
        {
            get { return _intensity; }
            set
            {
                if (_intensity != value)
                {
                    _intensity = value;
                    OnPropertyChanged();
                }
            }
        }

        [DisplayName("Falloff")]
        public virtual float Falloff
        {
            get { return _falloff; }
            set
            {
                if (_falloff != value)
                {
                    _falloff = value;
                    OnPropertyChanged();
                }
            }
        }

        [DisplayName("Blink Interval Seconds")]
        public virtual float BlinkIntervalSeconds
        {
            get { return _blinkIntervalSeconds; }
            set
            {
                if (_blinkIntervalSeconds != value)
                {
                    _blinkIntervalSeconds = value;
                    OnPropertyChanged();
                }
            }
        }

        [DisplayName("Blink Length")]
        public virtual float BlinkLength
        {
            get { return _blinkLength; }
            set
            {
                if (_blinkLength != value)
                {
                    _blinkLength = value;
                    OnPropertyChanged();
                }
            }
        }

        [DisplayName("Blink Offset")]
        public virtual float BlinkOffset
        {
            get { return _blinkOffset; }
            set
            {
                if (_blinkOffset != value)
                {
                    _blinkOffset = value;
                    OnPropertyChanged();
                }
            }
        }

        [DisplayName("Color")]
        public virtual Color Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new ITerminalProperty[]
            {
                new MockTerminalProperty<IMyInteriorLight, Color>("Color", b => b.Color, (b, v) => b.Color = v, Color.White),
                new MockTerminalProperty<IMyInteriorLight, float>("Radius", b => b.Radius, (b, v) => b.Radius = v, 2),
                new MockTerminalProperty<IMyInteriorLight, float>("Falloff", b => b.Falloff, (b, v) => b.Falloff = v, 1),
                new MockTerminalProperty<IMyInteriorLight, float>("Intensity", b => b.Intensity, (b, v) => b.Intensity = v, 4),
                new MockTerminalProperty<IMyInteriorLight, float>("Blink Interval", b => b.BlinkIntervalSeconds, (b, v) => b.BlinkIntervalSeconds = v),
                new MockTerminalProperty<IMyInteriorLight, float>("Blink Lenght", b => b.BlinkLength, (b, v) => b.BlinkLength = v),
                new MockTerminalProperty<IMyInteriorLight, float>("Blink Offset", b => b.BlinkOffset, (b, v) => b.BlinkOffset = v),
                new MockTerminalProperty<IMyInteriorLight, float>("Offset", b => _offset, (b, v) => _offset = v)
            });
        }

        [Obsolete("Use " + nameof(BlinkLength) + " instead.")]
        public virtual float BlinkLenght => BlinkLength;

        [Obsolete("Use " + nameof(Radius))]
        public virtual float ReflectorRadius => Radius;
    }
}
