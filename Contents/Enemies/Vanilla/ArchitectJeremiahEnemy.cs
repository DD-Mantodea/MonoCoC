using MonoCoC.Attributes;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<ArchitectJeremiahEnemy>("coc:architect_jeremiah")]
    public class ArchitectJeremiahEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

                    CreateBreastRow(0, 1);

            Tallness = 6 * 12;

            WeaponName = "nothing";

            ArmorName = "cracked stone";
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.WizardTowerProgress,

                CoC.Instance.Flags.Get(GameFlagIds.WizardTowerProgress) | 16);

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.Scene.ClearButtons();

            Scenes.Scene.OutputText("Scenes.WizardTower.DefeatedByJeremiah.FallOverwhelmedInjuries".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.StatueBadEnd.NothingMuchChanges".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.StatueBadEnd.AdventurousLuckyExplorer".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.StatueBadEnd.WouldNeedSkill".Localized());

            Scenes.Scene.GameOver();

            return true;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            var quotes = new System.Collections.Generic.List<string>
            {
                "Combat.Hel.JeremiahRebuild1", "Combat.Hel.JeremiahRebuild2",

                "Combat.Hel.JeremiahRebuild3", "Combat.Hel.JeremiahRebuild4"
            };

            if (context.Player.DemonScore() > 3)
                quotes.Add("Combat.Hel.JeremiahRebuildDemon");

            context.Append(quotes[Random.Shared.Next(quotes.Count)].Localized());

            context.Append("Combat.Hel.JeremiahRebuildText".Localized());

            ArmorDefense = 100;

            foreach (var ally in context.Enemies)
            {
                if (ally == this || ally is not (IncubusStatueEnemy or SuccubusStatueEnemy or ImpStatueEnemy))
                    continue;

                ally.Health = Math.Min(ally.MaxHealth, ally.Health + 100);

                context.Append("Combat.Hel.StatueHealed".Localized(ally.ShortName, 100));
            }

            context.Append("Combat.Hel.JeremiahArmorRestored".Localized());

            return true;
        }

        public override bool HandleFear(CombatContext context) => true;

    }
}