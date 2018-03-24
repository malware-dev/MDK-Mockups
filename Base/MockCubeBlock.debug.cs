using System;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRageMath;

namespace IngameScript.Base
{
    public abstract class MockCubeBlock : MockEntity, IMyCubeBlock
    {
        public virtual string OwnerFactionTag { get; set; }

        public virtual MyRelationsBetweenPlayerAndBlock PlayerRelationToOwner { get; set; }

        public virtual SerializableDefinitionId BlockDefinition { get; set; }

        public virtual bool CheckConnectionAllowed { get; set; }

        public virtual IMyCubeGrid CubeGrid { get; set; }

        public virtual string DefinitionDisplayNameText { get; set; }

        public virtual float DisassembleRatio { get; set; }

        public virtual string DisplayNameText { get; set; }

        public virtual bool IsBeingHacked { get; set; }

        public virtual bool IsFunctional { get; set; }

        public virtual bool IsWorking { get; set; }

        public virtual Vector3I Max { get; set; }

        public virtual float Mass { get; set; }

        public virtual Vector3I Min { get; set; }

        public virtual int NumberInGrid { get; set; }

        public virtual MyBlockOrientation Orientation { get; set; }

        public virtual long OwnerId { get; set; }

        public virtual Vector3I Position { get; set; }

        public virtual MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateIsWorking()
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateVisual()
        {
            throw new NotImplementedException();
        }

        public virtual string GetOwnerFactionTag() => OwnerFactionTag;

        public virtual MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner() => PlayerRelationToOwner;
    }
}