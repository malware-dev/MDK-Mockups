using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IngameScript.Mockups.Base;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript.Mockups.Blocks
{
    public partial class MockTextPanel
    {
        protected override string DataTemplateName => "TextSurface";
    }
}
