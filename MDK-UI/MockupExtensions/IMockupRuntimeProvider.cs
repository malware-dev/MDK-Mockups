using System.ComponentModel;
using Sandbox.ModAPI.Ingame;

namespace MDK_UI.MockupExtensions
{
    interface IMockupRuntimeProvider: INotifyPropertyChanged
    {
        void ProcessGameTick(IMyGridTerminalSystem gridTerminalSystem, int tick);
        int ProcessPriority { get; }
    }
}
