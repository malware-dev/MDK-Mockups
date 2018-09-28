﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups
{
    public class MockGridTerminalSystem : IMyGridTerminalSystem, IEnumerable<IMyTerminalBlock>
    {
        public List<IMyTerminalBlock> Blocks = new List<IMyTerminalBlock>();
        public List<IMyBlockGroup> Groups = new List<IMyBlockGroup>();

        public void Add(IMyBlockGroup group)
        {
            Groups.Add(group);
        }

        public void Add(IMyTerminalBlock block)
        {
            Blocks.Add(block);
        }

        public void GetBlocks(List<IMyTerminalBlock> blocks)
        {
            blocks.Clear();
            blocks.AddRange(Blocks);
        }

        public void GetBlockGroups(List<IMyBlockGroup> blockGroups, Func<IMyBlockGroup, bool> collect = null)
        {
            if (blockGroups == null)
                return;

            blockGroups.Clear();

            foreach (var group in Groups)
            {
                if (collect == null || collect.Invoke(group))
                {
                    blockGroups.Add(group);
                }
            }
        }

        public void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class
        {
            if (blocks == null)
                return;

            blocks.Clear();
            foreach (var block in Blocks)
            {
                if (block is T && (collect == null || collect.Invoke(block)))
                    blocks.Add(block);
            }
        }

        public void GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect = null) where T : class
        {
            if (blocks == null)
                return;

            blocks.Clear();
            foreach (var block in Blocks)
            {
                var typedBlock = block as T;
                if (typedBlock != null && (collect == null || collect.Invoke(typedBlock)))
                    blocks.Add(typedBlock);
            }
        }

        public void SearchBlocksOfName(string name, List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            if (blocks == null)
                return;

            blocks.Clear();
            foreach (var block in Blocks)
            {
                if (block.CustomName != null && block.CustomName.Contains(name, StringComparison.Ordinal) && (collect == null || collect.Invoke(block)))
                    blocks.Add(block);
            }
        }

        public IMyTerminalBlock GetBlockWithName(string name)
        {
            return Blocks.FirstOrDefault(block => block.CustomName == name);
        }

        public IMyBlockGroup GetBlockGroupWithName(string name)
        {
            return Groups.FirstOrDefault(block => block.Name == name);
        }

        public IMyTerminalBlock GetBlockWithId(long id)
        {
            return Blocks.FirstOrDefault(block => block.EntityId == id);
        }

        public IEnumerator<IMyTerminalBlock> GetEnumerator() { return Blocks.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}