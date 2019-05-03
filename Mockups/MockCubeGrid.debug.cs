using System;
using IngameScript.Mockups.Base;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace IngameScript.Mockups
{
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public class MockCubeGrid : MockEntity, IMyCubeGrid
    {
        private MyCubeSize _gridEnumSize = MyCubeSize.Large;
        private bool _isStatic = false;

        public virtual string CustomName { get; set; }

        public virtual float GridSize
        {
            get
            {
                switch (GridSizeEnum)
                {
                    case MyCubeSize.Large:
                        return 2.5f;
                    case MyCubeSize.Small:
                        return 0.5f;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public virtual MyCubeSize GridSizeEnum {
            get { return _gridEnumSize; }
            set
            {
                if (value == MyCubeSize.Small)
                    IsStatic = false;

                _gridEnumSize = value;
            }
        }

        public virtual bool IsStatic
        {
            get { return _isStatic; }
            set
            {
                if (value && _gridEnumSize == MyCubeSize.Small)
                    throw new InvalidOperationException("Small Grids cannot be static.");

                _isStatic = value;
            }
        }

        public virtual Vector3I Max { get; set; }

        public virtual Vector3I Min { get; set; }

        public virtual bool CubeExists(Vector3I pos)
        {
            throw new NotImplementedException();
        }

        public virtual IMySlimBlock GetCubeBlock(Vector3I pos)
        {
            throw new NotImplementedException();
        }

        public virtual Vector3D GridIntegerToWorld(Vector3I gridCoords)
        {
            throw new NotImplementedException();
        }

        public virtual Vector3I WorldToGridInteger(Vector3D coords)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsSameConstructAs(IMyCubeGrid other)
        {
            if (other.EntityId == this.EntityId)
                return true;

            throw new NotSupportedException("Cannot currently find links between joined grids");
        }
    }
}
