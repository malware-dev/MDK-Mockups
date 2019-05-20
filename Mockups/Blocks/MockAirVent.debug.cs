using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;

namespace IngameScript.Mockups.Blocks
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [DisplayName("Air Vent")]
    public partial class MockAirVent : MockFunctionalBlock, IMyAirVent
    {
        private float _oxygenLevel = 0;
        private bool _canPressurize = true;
        private bool _dePressurize = false;
        private VentStatus _status = VentStatus.Pressurized;

        [DisplayName("Oxygen Level"), Range(0d, 1d)]
        public virtual float OxygenLevel
        {
            get { return _oxygenLevel; }
            set
            {
                if (_oxygenLevel != value)
                {
                    _oxygenLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        [DisplayName("Can Pressurize")]
        public virtual bool CanPressurize
        {
            get { return _canPressurize; }
            set
            {
                if (_canPressurize != value)
                {
                    _canPressurize = value;
                    OnPropertyChanged();
                }
            }
        }

        [DisplayName("De-pressurize")]
        public virtual bool Depressurize
        {
            get { return _dePressurize; }
            set
            {
                if (_dePressurize != value)
                {
                    _dePressurize = value;
                    OnPropertyChanged();
                }
            }
        }

        [DisplayName("Status")]
        public virtual VentStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual bool PressurizationEnabled => true;

        [DisplayName("Is Depressurizing"), ReadOnly(true)]
        public virtual bool IsDepressurizing => Enabled && (Status == VentStatus.Depressurizing || Status == VentStatus.Depressurized);

        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new[]
            {
                new MockTerminalProperty<IMyAirVent, bool>("Depressurize", b => b.Depressurize, (b, v) => b.Depressurize = v)
            });
        }

        [DisplayName("Get Oxygen Level")]
        public virtual float GetOxygenLevel() => OxygenLevel;

        [DisplayName("Is Pressurized")]
        public virtual bool IsPressurized() => PressurizationEnabled && (Status == VentStatus.Pressurized || Status == VentStatus.Pressurizing);
    }
}
