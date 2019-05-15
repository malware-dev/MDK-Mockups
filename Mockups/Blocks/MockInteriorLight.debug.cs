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

        [DisplayName("Radius")]
        public virtual float Radius { get; set; } = 2;

        [DisplayName("Intensity")]
        public virtual float Intensity { get; set; } = 1;

        [DisplayName("Falloff")]
        public virtual float Falloff { get; set; } = 1;

        [DisplayName("Blink Interval Seconds")]
        public virtual float BlinkIntervalSeconds { get; set; } = 0;

        [DisplayName("Blink Length")]
        public virtual float BlinkLength { get; set; } = 0;

        [DisplayName("Blink Offset")]
        public virtual float BlinkOffset { get; set; } = 0;

        [DisplayName("Color")]
        public virtual Color Color { get; set; } = new Color(255, 255, 255);

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
