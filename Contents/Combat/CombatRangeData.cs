using MonoCoC.Contents.Creatures;
using System;

namespace MonoCoC.Contents.Combat
{

    public static class CombatRangeData
    {
        public static void CloseDistance(Creature target)
        {
            target.ExtraDistance = Math.Max(target.ExtraDistance - 1, 0);

            if (target.ExtraDistance == 0)
                target.Distance = CombatDistance.Melee;
        }

        public static void ResetDistance(Creature target)
        {
            target.Distance = CombatDistance.Melee;

            target.ExtraDistance = 0;
        }

        public static void MoveDistantSafe(Creature target) => target.Distance = CombatDistance.Distant;

        public static void MoveMonsterDistant(Creature target) => MoveDistantSafe(target);

        public static void MovePlayerDistant(Creature target) => MoveDistantSafe(target);

        public static bool CanReach(Creature source, Creature target, CombatDistance distance, CombatRange rangeType)
        {
            var eitherFlying = source.IsFlying || target.IsFlying;

            var bothFlying = source.IsFlying && target.IsFlying;

            return rangeType switch
            {

                CombatRange.Omni or CombatRange.Self or CombatRange.Tease or CombatRange.Ranged => true,

                CombatRange.Melee => bothFlying || (!eitherFlying && distance == CombatDistance.Melee),

                CombatRange.FlyingMelee => eitherFlying || distance == CombatDistance.Melee,

                CombatRange.ChargingMelee => bothFlying ||

                    (!eitherFlying && (distance == CombatDistance.Melee || target.ExtraDistance < 2)),

                _ => true
            };
        }
    }
}