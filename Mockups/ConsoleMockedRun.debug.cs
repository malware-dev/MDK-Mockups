using System;
using System.Threading;

namespace IngameScript.Mockups
{
    public class ConsoleMockedRun : MockedRun
    {
        static readonly char[] SpinnerChars = {'|', '/', '-', '\\'};

        public bool IsPaused { get; set; }

        public override void Echo(string text)
        {
            Console.Write('\r');
            if (text.Length < Console.BufferWidth)
                text = text.PadRight(Console.BufferWidth - text.Length, ' ');
            Console.WriteLine(text);
        }

        protected override bool Tick(long ticks, int scheduledBlocks)
        {
            if (scheduledBlocks == 0)
                return false;
            if (!IsPaused)
                return ContinuousTick(ticks);

            if (IsPaused)
                return PausedTick(ticks);

            return false;
        }

        bool PausedTick(long ticks)
        {
            var spinnerChar = SpinnerChars[ticks % SpinnerChars.Length];
            ClearLine();
            Console.Write($"\r(P:Pause;Q:Quit;Space:Tick) {spinnerChar} >");
            while (true)
            {
                var key = Console.ReadKey(true);
                switch (char.ToUpper(key.KeyChar))
                {
                    case ' ':
                        return true;

                    case 'P':
                        IsPaused = !IsPaused;
                        return true;

                    case 'Q':
                        return false;
                }
            }
        }

        bool ContinuousTick(long ticks)
        {
            Thread.Sleep(16);
            var spinnerChar = SpinnerChars[ticks % SpinnerChars.Length];
            WritePadded($"\r(P:Pause;Q:Quit) {spinnerChar} >");
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                switch (char.ToUpper(key.KeyChar))
                {
                    case 'P':
                        IsPaused = !IsPaused;
                        break;

                    case 'Q':
                        return false;
                }
            }

            return true;
        }

        protected void ClearLine()
        {
            Console.Write('\r');
            Console.Write(new string(' ', Console.BufferWidth - 1));
            Console.Write('\r');
        }

        protected void WritePadded(string text)
        {
            Console.Write('\r');
            if (text.Length < Console.BufferWidth)
                text = text.PadRight(Console.BufferWidth - text.Length, ' ');
            Console.Write(text);
        }
    }
}
