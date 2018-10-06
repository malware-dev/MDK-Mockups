using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage.Game;
using VRageMath;

namespace IngameScript.Mockups.Base
{
    public abstract class MockTerminalBlock : MockCubeBlock, IMyTerminalBlock
    {
        protected abstract IEnumerable<ITerminalProperty> Properties { get; }

        public virtual bool HasLocalPlayerAccess()
        {
            switch (PlayerRelationToOwner)
            {
                case MyRelationsBetweenPlayerAndBlock.FactionShare:
                case MyRelationsBetweenPlayerAndBlock.Owner:
                case MyRelationsBetweenPlayerAndBlock.NoOwnership:
                    return true;
                default:
                    return false;
            }
        }

        public virtual bool HasPlayerAccess(long playerId)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SetCustomName(string text) => CustomName = text;

        void IMyTerminalBlock.SetCustomName(StringBuilder text) => CustomName = text.ToString();

        public virtual void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public virtual void SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            if (resultList == null)
                return;

            resultList.Clear();

            var templist = new List<ITerminalAction>();
            GetActions(templist, collect);

            resultList.AddRange(templist.Where(a => a.Id.Contains(name)));
        }

        public ITerminalAction GetActionWithName(string name)
        {
            var actions = new List<ITerminalAction>();

            GetActions(actions, a => a.Id == name);

            return actions.FirstOrDefault();
        }

        public ITerminalProperty GetProperty(string id)
        {
            var properties = new List<ITerminalProperty>();

            GetProperties(properties, prop => prop.Id == id);

            return properties.FirstOrDefault();
        }

        public void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null)
        {
            if (resultList == null)
                return;

            resultList.Clear();

            foreach (var prop in Properties)
            {
                if (collect?.Invoke(prop) ?? true)
                {
                    resultList.Add(prop);
                }
            }
        }

        public virtual string CustomName { get; set; } = "";

        public virtual string CustomNameWithFaction
        {
            get
            {
                if (string.IsNullOrEmpty(GetOwnerFactionTag()))
                    return CustomName;

                return $"{GetOwnerFactionTag()}.{CustomName}";
            }
        }

        public virtual string DetailedInfo { get; set; } = "";

        public virtual string CustomInfo { get; set; } = "";

        public virtual string CustomData { get; set; } = "";

        public virtual bool ShowOnHUD { get; set; } = true;

        public virtual bool ShowInTerminal { get; set; } = true;

        public virtual bool ShowInToolbarConfig { get; set; } = true;

        public virtual bool ShowInInventory { get; set; } = true;
    }
}