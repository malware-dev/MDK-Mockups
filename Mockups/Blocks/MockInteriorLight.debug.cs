using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRageMath;

namespace IngameScript.Mockups.Blocks
{
    class MockInteriorLight : MockTerminalBlock, IMyInteriorLight
    {
        protected override IEnumerable<ITerminalProperty> Properties { get; } = new List<ITerminalProperty>()
        {
            new MockBoolTerminalProperty<IMyInteriorLight>("OnOff", b => b.Enabled),
            new MockBoolTerminalProperty<IMyInteriorLight>("ShowInTerminal", b => b.ShowInTerminal),
            new MockBoolTerminalProperty<IMyInteriorLight>("ShowInToolbarConfig", b => b.ShowInToolbarConfig),
            new MockBoolTerminalProperty<IMyInteriorLight>("ShowOnHUD", b => b.ShowOnHUD),
            new MockColorTerminalProperty<IMyInteriorLight>("Color", b => b.Color),
            new MockFloatTerminalProperty<IMyInteriorLight>("Radius", b => b.Radius),
            new MockFloatTerminalProperty<IMyInteriorLight>("Falloff", b => b.Falloff),
            new MockFloatTerminalProperty<IMyInteriorLight>("Intensity", b => b.Intensity),
            new MockFloatTerminalProperty<IMyInteriorLight>("Blink Interval", b => b.BlinkIntervalSeconds),
            new MockFloatTerminalProperty<IMyInteriorLight>("Blink Lenght", b => b.BlinkLength),
            new MockFloatTerminalProperty<IMyInteriorLight>("Blink Offset", b => b.BlinkOffset)
        };

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

        public void RequestEnable(bool enable) => Enabled = enable;
    }
}
