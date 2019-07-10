using System.ComponentModel;
using Sandbox.ModAPI.Ingame;

namespace MDK_UI.MockupExtensions
{
    interface IMockupRuntimeProvider
    {
        void ProcessGameTick(IMyGridTerminalSystem gridTerminalSystem, int tick);
        int ProcessPriority { get; }
    }
}
