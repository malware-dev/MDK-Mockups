using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

namespace IngameScript.Base
{
    public abstract class MockTerminalBlock : MockCubeBlock, IMyTerminalBlock
    {
        public virtual bool HasLocalPlayerAccess()
        {
            throw new NotImplementedException();
        }

        public virtual bool HasPlayerAccess(long playerId)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SetCustomName(string text)
        {
            CustomName = text;
        }

        void IMyTerminalBlock.SetCustomName(StringBuilder text)
        {
            CustomName = text.ToString();
        }

        public virtual void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public virtual void SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public virtual ITerminalAction GetActionWithName(string name)
        {
            throw new NotImplementedException();
        }

        public virtual ITerminalProperty GetProperty(string id)
        {
            throw new NotImplementedException();
        }

        public virtual void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public virtual string CustomName { get; set; }

        public virtual string CustomNameWithFaction
        {
            get
            {
                if (string.IsNullOrEmpty(GetOwnerFactionTag()))
                    return CustomName;
                return $"{GetOwnerFactionTag()}.{CustomName}";
            }
        }

        public virtual string DetailedInfo { get; set; }

        public virtual string CustomInfo { get; set; }

        public virtual string CustomData { get; set; }

        public virtual bool ShowOnHUD { get; set; }

        public virtual bool ShowInTerminal { get; set; }

        public virtual bool ShowInToolbarConfig { get; set; }

        public virtual bool ShowInInventory { get; set; }
    }
}