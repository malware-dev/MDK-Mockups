using System;
using System.Collections.Generic;
using System.Linq;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRageMath;

namespace IngameScript.Mockups.Blocks
{
    public class MockInteriorLight : MockTerminalBlock, IMyInteriorLight
    {
        float _offset;
        public float Radius { get; set; } = 2;

        [Obsolete("Use " + nameof(Radius))]
        public float ReflectorRadius => Radius;

        public float Intensity { get; set; } = 1;
        public float Falloff { get; set; } = 1;
        public float BlinkIntervalSeconds { get; set; } = 0;

        [Obsolete("Use " + nameof(BlinkLength) + " instead.")]
        public float BlinkLenght => BlinkLength;

        public float BlinkLength { get; set; } = 0;
        public float BlinkOffset { get; set; } = 0;
        public Color Color { get; set; } = new Color(255, 255, 255);
        public bool Enabled { get; set; } = true;

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

        [Obsolete("Use " + nameof(Enabled) + " instead.")]
        public void RequestEnable(bool enable) => Enabled = enable;
    }
}
