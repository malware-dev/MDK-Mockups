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
        public virtual bool HasLocalPlayerAccess() => HasPlayerAccess(MockGridSystem.PlayerId);

        public virtual bool HasPlayerAccess(long playerId)
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

        private static readonly IReadOnlyDictionary<Type, Func<PropertyInfo, ITerminalProperty>> PropTypes
            = new Dictionary<Type, Func<PropertyInfo, ITerminalProperty>>()
            {
                { typeof(string), prop => new MockStringTerminalProperty(prop.Name) },
                { typeof(float), prop => new MockFloatTerminalProperty(prop.Name) },
                { typeof(bool), prop => new MockBoolTerminalProperty(prop.Name) },
                { typeof(Color), prop => new MockColorTerminalProperty(prop.Name) }
            };

        public virtual void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null)
        {
            if (resultList == null)
                return;

            resultList.Clear();

            var props = GetType().GetProperties().Where(p => p.IsMemberPublic() && p.CanRead && p.CanWrite)
                .Where(p => PropTypes.ContainsKey(p.PropertyType))
                .Select(p => PropTypes[p.PropertyType](p));

            if (collect == null)
            {
                resultList.AddRange(props);
            }
            else
            {
                resultList.AddRange(props.Where(collect));
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