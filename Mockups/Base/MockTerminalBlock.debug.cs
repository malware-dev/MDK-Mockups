using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage.Game;

namespace IngameScript.Mockups.Base
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [DisplayName("Unsupported Block")]
    public abstract partial class MockTerminalBlock : MockCubeBlock, IMyTerminalBlock
    {
        public Dictionary<long, MyRelationsBetweenPlayerAndBlock> Relationships { get; }
            = new Dictionary<long, MyRelationsBetweenPlayerAndBlock>();

        ReadOnlyCollection<ITerminalProperty> _properties;
        StringBuilder _customName = new StringBuilder();

        protected ReadOnlyCollection<ITerminalProperty> Properties
        {
            get
            {
                if (_properties == null)
                    _properties = new ReadOnlyCollection<ITerminalProperty>(CreateTerminalProperties().ToList());
                return _properties;
            }
        }

        [DisplayName("Custom Name")]
        public virtual string CustomName
        {
            // Ugh... >_<
            // This is horrible, but it's _actually_ how SE does it.
            get
            {
                return _customName.ToString();
            }
            set
            {
                _customName.Clear();
                _customName.Append(value);
            }
        }

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

        [DisplayName("Custom Data"), DataType(DataType.MultilineText)]
        public virtual string CustomData { get; set; } = "";

        [DisplayName("Show On HUD")]
        public virtual bool ShowOnHUD { get; set; } = true;

        [DisplayName("Show In Terminal")]
        public virtual bool ShowInTerminal { get; set; } = true;

        [DisplayName("Show In Toolbar Config")]
        public virtual bool ShowInToolbarConfig { get; set; } = true;

        [DisplayName("Show In Inventory")]
        public virtual bool ShowInInventory { get; set; } = true;

        protected virtual IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return new ITerminalProperty[]
            {
                new MockTerminalProperty<IMyTerminalBlock, bool>("ShowInTerminal", b => b.ShowInTerminal, (b, v) => b.ShowInTerminal = true, true),
                new MockTerminalProperty<IMyTerminalBlock, bool>("ShowInToolbarConfig", b => b.ShowInToolbarConfig, (b, v) => b.ShowInToolbarConfig = v, true),
                new MockTerminalProperty<IMyTerminalBlock, bool>("ShowInInventory", b => b.ShowInInventory, (b, v) => b.ShowInInventory = v, false),
                new MockTerminalProperty<IMyTerminalBlock, bool>("ShowOnHUD", b => b.ShowOnHUD, (b, v) => b.ShowOnHUD = v, false),
                
                // Ugh... >_<
                new MockTerminalProperty<IMyTerminalBlock, StringBuilder>("Name", b => _customName, (b, v) =>
                {
                    _customName.Clear();
                    _customName.Append(v);
                }),
            };
        }

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

        [Obsolete("This method should not be referenced by ingame scripts.", true)]
        public virtual bool HasPlayerAccess(long playerId)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SetCustomName(string text) => CustomName = text;

        void IMyTerminalBlock.SetCustomName(StringBuilder text) => CustomName = text.ToString();

        bool IMyTerminalBlock.IsSameConstructAs(IMyTerminalBlock other)
        {
            if (other.CubeGrid.EntityId == this.CubeGrid.EntityId)
                return true;

            throw new NotSupportedException("Cannot currently find links between joined grids");
        }

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
    }
}
