using System;
using IngameScript.Mockups.Base;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace IngameScript.Mockups
{
    public class MockCubeGrid : MockEntity, IMyCubeGrid
    {
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

        public virtual MyCubeSize GridSizeEnum { get; set; }

        public virtual bool IsStatic { get; set; }

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
