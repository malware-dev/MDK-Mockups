using System;
using System.Collections.Generic;
using System.Text;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using IMyGyro = Sandbox.ModAPI.Ingame.IMyGyro;

namespace IngameScript.Mockups.Blocks
{
  class MockGyro : MockFunctionalBlock, IMyGyro
  {
    public virtual float GyroPower { get; set; }

    public virtual bool GyroOverride { get; set; }

    public virtual float Yaw { get; set; }

    public virtual float Pitch { get; set; }

    public virtual float Roll { get; set; }
  }
}
