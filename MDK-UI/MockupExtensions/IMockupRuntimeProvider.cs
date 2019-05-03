﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI.Ingame;

namespace MDK_UI.MockupExtensions
{
    interface IMockupRuntimeProvider
    {
        void ProcessGameTick(IMyGridTerminalSystem gridTerminalSystem);
        int ProcessPriority { get; }
    }
}
