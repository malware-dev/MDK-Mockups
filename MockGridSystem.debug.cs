using System;
using System.Collections.Generic;
using System.Text;

namespace IngameScript
{
    static class MockGridSystem
    {
        static int NextEntityId = 1;
        static int NextNumberInGrid = 1;
        public static int PlayerId { get; } = 1;

        public static Dictionary<long, string> PlayerFactions { get; } = new Dictionary<long, string>()
        {
            { PlayerId, "" }
        };

        public static int GetNextNumberInGrid() => NextNumberInGrid++;
        public static long GetNextEntityId() => NextEntityId++;
    }
}
