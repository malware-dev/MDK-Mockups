using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    abstract class MockTerminalProperty<TBlock, T>: ITerminalProperty<T>
        where TBlock: IMyCubeBlock
    {
        private Expression<Func<TBlock, T>> Selector { get; }
        public String Id { get; }

        public String TypeName
        {
            get
            {
                if (typeof(T) == typeof(float))
                    return "float";

                if (typeof(T) == typeof(bool))
                    return "bool";

                if (typeof(T) == typeof(Color))
                    return "color";

                return "";
            }
        }

        public MockTerminalProperty(string name, Expression<Func<TBlock, T>> selector)
        {
            Id = name;
            Selector = selector;
        }

        public abstract T GetDefaultValue(IMyCubeBlock block);

        public virtual T GetMaximum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public virtual T GetMininum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public virtual T GetMinimum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public virtual T GetValue(IMyCubeBlock block)
        {
            if (block is TBlock)
                return Selector.Compile().Invoke((TBlock) block);
            else
                return default(T);
        }

        public void SetValue(IMyCubeBlock block, T value)
        {
            var property = ((Selector.Body as MemberExpression)?.Member as PropertyInfo);
            
            property?.SetValue(block, value);
        }
    }

    class MockStringTerminalProperty<TBlock> : MockTerminalProperty<TBlock, string>
        where TBlock : IMyCubeBlock
    {
        public MockStringTerminalProperty(String name, Expression<Func<TBlock, String>> selector)
            : base(name, selector)
        {
        }

        public override String GetDefaultValue(IMyCubeBlock block) => "";
    }

    class MockBoolTerminalProperty<TBlock> : MockTerminalProperty<TBlock, bool>
        where TBlock : IMyCubeBlock
    {
        public MockBoolTerminalProperty(String name, Expression<Func<TBlock, bool>> selector)
            : base(name, selector)
        {
        }

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

    class MockFloatTerminalProperty<TBlock> : MockTerminalProperty<TBlock, float>
        where TBlock : IMyCubeBlock
    {
        public MockFloatTerminalProperty(String name, Expression<Func<TBlock, float>> selector)
            : base(name, selector)
        {
        }

        public override Single GetDefaultValue(IMyCubeBlock block) => 0;
    }

    class MockColorTerminalProperty<TBlock> : MockTerminalProperty<TBlock, Color>
            where TBlock : IMyCubeBlock
    {
        public MockColorTerminalProperty(String name, Expression<Func<TBlock, Color>> selector)
            : base(name, selector)
        {
        }

        public override Color GetDefaultValue(IMyCubeBlock block) => new Color(0, 0, 0);
    }
}
