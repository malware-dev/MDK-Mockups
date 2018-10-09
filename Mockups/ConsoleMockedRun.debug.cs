using System;

namespace IngameScript.Mockups
{
    /// <summary>
    /// A simple mocked run designed to run in the console
    /// </summary>
    public class ConsoleMockedRun : MockedRun
    {
        static readonly char[] SpinnerChars = {'|', '/', '-', '\\'};

        /// <summary>
        /// Get or set whether the run should advance automatically (<c>false</c>) or manually (<c>true</c>)
        /// </summary>
        public bool IsPaused { get; set; }

        public override void Echo(string text)
        {
            Console.Write('\r');
            if (text.Length < Console.BufferWidth)
                text = text.PadRight(Console.BufferWidth - text.Length, ' ');
            Console.WriteLine(text);
        }

        public override bool NextTick(out MockedRunFrame frame)
        {
            if (!base.NextTick(out frame))
                return false;

            if (!IsPaused)
                return ContinuousTick(frame.Tick);

            if (IsPaused)
                return PausedTick(frame.Tick);

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
