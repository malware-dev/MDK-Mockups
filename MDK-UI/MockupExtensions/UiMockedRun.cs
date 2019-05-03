using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngameScript.Mockups;
using Sandbox.ModAPI.Ingame;

namespace MDK_UI.MockupExtensions
{
    class UiMockedRun : MockedRun
    {
        private Action<string> EchoAction { get; }

        public UiMockedRun(Action<string> echo, IMyGridTerminalSystem terminalSystem)
            :base()
        {
            EchoAction = echo;
            GridTerminalSystem = terminalSystem;
        }

        public override void Echo(string text)
            => EchoAction(text);
    }
}
