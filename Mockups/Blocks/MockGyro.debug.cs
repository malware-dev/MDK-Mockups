using System.Collections.Generic;
using System.Linq;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

namespace IngameScript.Mockups.Blocks
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public partial class MockGyro : MockFunctionalBlock, IMyGyro
    {
        private float _gyroPower;
        private bool _gyroOverride;
        private float _yaw;
        private float _pitch;
        private float _roll;

        public virtual float GyroPower
        {
            get { return _gyroPower; }
            set
            {
                if (_gyroPower != value)
                {
                    _gyroPower = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual bool GyroOverride
        {
            get { return _gyroOverride; }
            set
            {
                if (_gyroOverride != value)
                {
                    _gyroOverride = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual float Yaw
        {
            get { return _yaw; }
            set
            {
                if (_yaw != value)
                {
                    _yaw = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual float Pitch
        {
            get { return _pitch; }
            set
            {
                if (_pitch != value)
                {
                    _pitch = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual float Roll
        {
            get { return _roll; }
            set
            {
                if (_roll != value)
                {
                    _roll = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new ITerminalProperty[]
            {
                new MockTerminalProperty<IMyGyro, float>("Power", b => b.GyroPower, (b, v) => b.GyroPower = v),
                new MockTerminalProperty<IMyGyro, bool>("Override", b => b.GyroOverride, (b, v) => b.GyroOverride = v),
                new MockTerminalProperty<IMyGyro, float>("Yaw", b => b.Yaw, (b, v) => b.Yaw = v),
                new MockTerminalProperty<IMyGyro, float>("Pitch", b => b.Pitch, (b, v) => b.Pitch = v),
                new MockTerminalProperty<IMyGyro, float>("Roll", b => b.Roll, (b, v) => b.Roll = v)
            });
        }
    }
}
