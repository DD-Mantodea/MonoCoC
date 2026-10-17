using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Perks;

namespace MonoCoC.Contents.Creatures
{
    public partial class Player
    {
        // 原版 PlayerInfo.hx:770 raiseLevel()
        public virtual void RaiseLevel()
        {
            Experience -= RequiredXP();

            Level += 1;

            PerkPoints += 1;

            StatPoints += Age switch
            {
                Age.Child => 6,

                Age.Elder => 2,

                _ => 5,
            };

            if (Level % 2 == 0)
                AscensionPerkPoints += 1;
        }

        // 原版 Player.hx canBuyStats()
        public virtual bool CanBuyStats() => StatPoints > 0 && !StatsMaxed() && !CombatController.InCombat;

        // 原版 Player.hx canBuyPerks()
        public virtual bool CanBuyPerks() => PerkPoints > 0 && PerkPurchaseCatalog.Available(this).Count > 0 && !CombatController.InCombat;

        // 原版 Creature.hx getStatByString()
        public virtual int GetStatByString(string stat) => stat switch
        {
            "str" or "stre" or "strength" => (int)Strength,

            "tou" or "tough" or "toughness" => (int)Toughness,

            "spe" or "spd" or "speed" => (int)Speed,

            "int" or "inte" or "intelligence" => (int)Intelligence,

            _ => 0,
        };
    }
}