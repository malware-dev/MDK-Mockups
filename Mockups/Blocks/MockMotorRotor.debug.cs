using System;
using System.Collections.Generic;
using System.Text;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRageMath;

namespace IngameScript.Mockups.Blocks
{
    public class MockMotorRotor : MockCubeBlock, IMyMotorRotor
    {
        public bool IsAttached => Base != null;
        public IMyMechanicalConnectionBlock Base { get; set; }
    }
}
