using IngameScript.Mockups.Base;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using VRageMath;

namespace IngameScript.Mockups.Blocks
{
    class MockInteriorLight : MockTerminalBlock, IMyInteriorLight
    {
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
