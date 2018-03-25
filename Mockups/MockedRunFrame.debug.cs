namespace IngameScript.Mockups
{
    /// <summary>
    /// Provides information about a single frame in a <see cref="MockedRun"/>
    /// </summary>
    public struct MockedRunFrame
    {
        public MockedRunFrame(long tick, bool isRunning, int scheduledPBs, int runPBs)
        {
            Tick = tick;
            IsRunning = isRunning;
            ScheduledPBs = scheduledPBs;
            RunPBs = runPBs;
        }

        /// <summary>
        /// The tick number of this frame
        /// </summary>
        public readonly long Tick;

        /// <summary>
        /// Whether the run is currently active
        /// </summary>
        public readonly bool IsRunning;

        /// <summary>
        /// The number of programmable blocks which are scheduled to run in the future
        /// </summary>
        public readonly int ScheduledPBs;

        /// <summary>
        /// The number of programmable blocks which were run this tick
        /// </summary>
        public readonly int RunPBs;
    }
}