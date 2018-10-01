using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    class MockStringTerminalProperty: MockTerminalProperty<string>
    {
        public MockStringTerminalProperty(string name)
            : base(name, TYPEOF_STRING) { }

        public override String GetDefaultValue(IMyCubeBlock block) => "";
    }

    class MockBoolTerminalProperty: MockTerminalProperty<bool>
    {
        public MockBoolTerminalProperty(string name)
            : base(name, TYPEOF_BOOL) { }

        public override Boolean GetDefaultValue(IMyCubeBlock block)
        {
            if (block is IMyTerminalBlock)
            {
                switch (Id)
                {
                    case "Enabled":
                    case "ShowInInventory":
                    case "ShowInToolbarConfig":
                    case "ShowInTerminal":
                    case "ShowOnHUD":
                        return true;
                    default:
                        return false;
                }
            }

            return false;
        }
    }

    class MockFloatTerminalProperty: MockTerminalProperty<float>
    {
        public MockFloatTerminalProperty(string name)
            : base(name, TYPEOF_FLOAT) { }
    }

    class MockColorTerminalProperty: MockTerminalProperty<Color>
    {
        public MockColorTerminalProperty(string name)
            : base(name, TYPEOF_COLOR) { }

        public override Color GetDefaultValue(IMyCubeBlock block) => new Color(255, 255, 255);
    }

    abstract class MockTerminalProperty<T> : ITerminalProperty<T>
    {
        protected const string TYPEOF_FLOAT = "float";
        protected const string TYPEOF_BOOL = "bool";
        protected const string TYPEOF_COLOR = "color";
        protected const string TYPEOF_STRING = "";

        public string Id { get; }

        public string TypeName { get; }

        protected MockTerminalProperty(string name, string type)
        {
            Id = name;
            TypeName = type;
        }

        public virtual T GetDefaultValue(IMyCubeBlock block) => default(T);

        public virtual T GetMaximum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public virtual T GetMinimum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public virtual T GetMininum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public T GetValue(IMyCubeBlock block)
        {
            return (T) (GetProperty(block)?.GetValue(block) ?? GetDefaultValue(block));
        }

        public void SetValue(IMyCubeBlock block, T value)
        {
            GetProperty(block).SetValue(block, value);
        }

        private PropertyInfo GetProperty(IMyCubeBlock block)
        {
            return block.GetType().GetProperties().FirstOrDefault(p => p.Name == Id && p.PropertyType == typeof(T) && p.IsMemberPublic() && p.CanRead && p.CanWrite);
        }
    }
}
