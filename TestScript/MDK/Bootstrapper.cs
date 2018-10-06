using System;
using System.Text;
using System.Threading;
using IngameScript.Mockups;
using IngameScript.Mockups.Blocks;
using Malware.MDKUtilities;

namespace IngameScript.MDK
{
    public class TestBootstrapper
    {
        // All the files in this folder, as well as all files containing the file ".debug.", will be excluded
        // from the build process. You can use this to create utilites for testing your scripts directly in 
        // Visual Studio.

        static TestBootstrapper()
        {
            // Initialize the MDK utility framework
            MDKUtilityFramework.Load();
        }

        public static void Main()
        {
            var echoOutput = new StringBuilder();
            
            // Create a run instance, this one is running in the console.
            var run = new ConsoleMockedRun(echoOutput)
            {
                // We need a terminal system for the script.
                GridTerminalSystem = new MockGridTerminalSystem
                {
                    // This is the programmable block mockup which
                    // will pretend to be the running block for our
                    // script, the `Me` property
                    new MockProgrammableBlock
                    {
                        // Name it for convenience
                        CustomName = "Our PB",

                        // Rather than actually instantiating the script
                        // we just tell it the type of the script we want
                        // it to run
                        ProgramType = typeof(Program)
                    }

                    // We can add more blocks here, separated by a comma.
                    // We can even add multiple mocked programmable
                    // blocks if we are so inclined.
                }
            };

            // If our script doesn't start itself in its constructor, we'll need
            // to run it manually.
            //run.Trigger("Our PB", "An optional argument");

            // If our script doesn't utilize the `Runtime.UpdateFrequency` at all,
            // the following isn't needed. It's not harmful either, though.
            MockedRunFrame frame;
            Console.CursorVisible = false;
            while (run.NextTick(out frame))
            {
                Console.Clear();
                Console.WriteLine($"Tick #{frame.Tick,6}   Active PBs {frame.RunPBs,3}   Scheduled PBs {frame.ScheduledPBs,3}");
                Console.WriteLine("- - - - - - - - - -");
                Console.WriteLine(echoOutput.ToString());
                echoOutput.Clear();
                // Just insert a little delay between ticks. You can change this
                // to your specifications, it's not important.
                Thread.Sleep(160);
            }
        }
    }
}
