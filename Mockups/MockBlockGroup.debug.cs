using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups
{
    public class MockBlockGroup : IMyBlockGroup
    {
        public string Name { get; }
        public List<IMyTerminalBlock> Blocks { get; } = new List<IMyTerminalBlock>();

        public MockBlockGroup(string name)
        {
            Name = name;
        }

        public void GetBlocks(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            blocks?.Clear();
            blocks?.AddRange(Blocks.Where(b => collect?.Invoke(b) ?? false));
        }

        public void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class
        {
            blocks?.Clear();
            blocks?.AddRange(Blocks.Where(b => b is T && (collect?.Invoke(b) ?? false)));
        }


        public void GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect = null) where T : class
        {
            blocks?.Clear();
            blocks?.AddRange(Blocks.OfType<T>().Where(b => collect?.Invoke(b) ?? false));
        }

        public override string ToString() => $"Group: {Name} ({Blocks.Count} blocks)";
    }
}
