using System.Collections.Generic;
using IngameScript.Mockups;
using IngameScript.Mockups.Base;
using VRage.Game;

namespace MDK_UI.Blueprints
{
    public class BlueprintDefinition
    {
        public string Name { get; }
        public MyCubeSize Size { get; }
        public bool IsStatic { get; }

        public bool UnsupportedBlocks { get; }

        public IEnumerable<MockBlockGroup> Groups { get; }
        public IEnumerable<MockTerminalBlock> Blocks { get; }

        public BlueprintDefinition(string name, MyCubeSize size, bool isStatic, bool unsupported, IEnumerable<MockBlockGroup> groups, IEnumerable<MockTerminalBlock> blocks)
        {
            Name = name;
            Size = size;
            IsStatic = isStatic;

            UnsupportedBlocks = unsupported;

            Groups = groups;
            Blocks = blocks;
        }
    }
}
