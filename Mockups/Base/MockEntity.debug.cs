using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace IngameScript.Mockups.Base
{
    public abstract class MockEntity : IMyEntity
    {
        public virtual Vector3D WorldPosition { get; set; }

        public virtual MyEntityComponentContainer Components
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long EntityId { get; set; }// = MockGridSystem.GetNextEntityId();

        public virtual string Name { get; set; } = "";

        public virtual string DisplayName { get; set; } = "";

        public virtual bool HasInventory { get; } = false;

        public virtual int InventoryCount { get; } = 0;

        public virtual BoundingBoxD WorldAABB { get; set; }

        public virtual BoundingBoxD WorldAABBHr { get; set; }

        public virtual MatrixD WorldMatrix { get; set; }

        public virtual BoundingSphereD WorldVolume { get; set; }

        public virtual BoundingSphereD WorldVolumeHr { get; set; }

        public virtual IMyInventory GetInventory()
        {
            throw new NotSupportedException("This block type does not have an inventory.");
        }

        public virtual IMyInventory GetInventory(int index)
        {
            throw new NotSupportedException("This block type does not have an inventory.");
        }

        public virtual Vector3D GetPosition() => WorldPosition;
    }
}
