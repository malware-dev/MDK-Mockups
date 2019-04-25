using System;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRageMath;

namespace IngameScript.Mockups.Base
{
    public abstract class MockCubeBlock : MockEntity, IMyCubeBlock
    {
        public virtual MyRelationsBetweenPlayerAndBlock PlayerRelationToOwner { get; set; }

        public virtual SerializableDefinitionId BlockDefinition { get; set; }

        public virtual bool CheckConnectionAllowed { get; set; }

        public virtual IMyCubeGrid CubeGrid { get; set; }

        public virtual string DefinitionDisplayNameText { get; set; } = "";

        public virtual float DisassembleRatio { get; set; } = 0;

        public virtual string DisplayNameText { get; set; } = "";

        public virtual bool IsBeingHacked { get; set; } = false;

        public virtual bool IsFunctional { get; set; } = true;

        public virtual bool IsWorking { get; set; } = true;

        public virtual Vector3I Max { get; set; }

        public virtual float Mass { get; set; }

        public virtual Vector3I Min { get; set; }

        public virtual int NumberInGrid { get; set; }

        public virtual MyBlockOrientation Orientation { get; set; }

        public virtual long OwnerId { get; set; }

        public virtual Vector3I Position { get; set; }

        public virtual MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId)
            => MyRelationsBetweenPlayerAndBlock.Neutral;

        [Obsolete("This method should not be referenced by ingame scripts.", true)]
        public virtual void UpdateIsWorking() { }

        [Obsolete("This method should not be referenced by ingame scripts.", true)]
        public virtual void UpdateVisual() { }

        public virtual string GetOwnerFactionTag()
        {
            var faction = "";
            //MockGridSystem.PlayerFactions.TryGetValue(OwnerId, out faction);

            return faction;
        }

        public virtual MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner() => PlayerRelationToOwner;

        public override string ToString()
        {
            var name = GetType().Name + " #" + EntityId;

            if (!string.IsNullOrWhiteSpace(DisplayName))
            {
                name += ": " + DisplayName;
            }

            return name;
        }
    }
}
