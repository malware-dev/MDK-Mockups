using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI.Ingame;

namespace MDK_UI.MockupExtensions
{
    interface IMockupRuntimeProvider: INotifyPropertyChanged
    {
        void ProcessGameTick(IMyGridTerminalSystem gridTerminalSystem, int tick);
        int ProcessPriority { get; }
    }
}
