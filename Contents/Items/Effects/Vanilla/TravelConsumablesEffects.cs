using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 WinterPudding.hx ───────────────────────────────────────────
    public class WinterPuddingEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            var flags = CoC.Instance.Flags;

            var changes = 0;

            context.Write("Items.WinterPudding.Eat".Localized());

            player.RefillHunger(30);

            if (player.Thickness < 100 || player.Tone > 0)
            {
                context.Write(player.ModTone(0, 2));

                context.Write(player.ModThickness(100, 2));
            }

            context.Write("Items.WinterPudding.Lick".Localized());

            // 原版 dynStats(Lust(10 + lib/10), NoScale)
            player.ApplyStats(new DynStatsOptions(Scale: false),

                new DynStat(GameStats.Lust, 10 + player.Libido / 10));

            var horns = player.Body.Get<HornsPart>();

            if (horns.Value == 0 && random.Next(2) == 0)
            {
                context.Write("Items.WinterPudding.GrowAntlers".Localized());

                horns.Id = new Identifier("coc", "antlers");

                horns.Value = 4 + random.Next(12);

                changes += 1;
            }
            else if (horns.Value > 0 && horns.Id.Path != "antlers" && random.Next(2) == 0)
            {
                context.Write("Items.WinterPudding.BecomeAntlers".Localized());

                horns.Id = new Identifier("coc", "antlers");

                horns.Value = 4 + random.Next(12);

                changes += 1;
            }

            if (random.Next(5) == 0)
                Mutations.Mutations.Instance.UpdateOvipositionPerk("winterPudding");

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + changes);
        }
    }

    // ── 原版 HairExtensionSerum.hx ──────────────────────────────────────
    public class HairExtensionSerumEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var flags = CoC.Instance.Flags;

            var hair = player.Body.Get<HairPart>();

            context.Write("Items.HairSerum.Apply".Localized());

            if (hair.Id.Path == "basilisk_spines")
            {
                context.Write("Items.HairSerum.Immune".Localized(hair.Color));

                return;
            }

            var remaining = flags.Get(GameFlagIds.IncreasedHairGrowthTimeRemaining);

            var applied = flags.Get(GameFlagIds.IncreasedHairGrowthSerumTimesApplied);

            if (remaining <= 0)
            {
                context.Write("Items.HairSerum.FirstApply".Localized());

                flags.Set(GameFlagIds.IncreasedHairGrowthTimeRemaining, 7);

                flags.Set(GameFlagIds.IncreasedHairGrowthSerumTimesApplied, 1);
            }
            else if (applied == 1)
            {
                context.Write("Items.HairSerum.SecondApply".Localized());

                flags.Set(GameFlagIds.IncreasedHairGrowthSerumTimesApplied, applied + 1);
            }
            else if (applied == 2)
            {
                context.Write("Items.HairSerum.ThirdApply".Localized());

                flags.Set(GameFlagIds.IncreasedHairGrowthSerumTimesApplied, applied + 1);
            }

            if (flags.Get(GameFlagIds.HairGrowthStoppedBecauseLizard) > 0 && hair.Id.Path != "anemone")
            {
                flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 0);

                context.Write("Items.HairSerum.GrowingAgain".Localized());
            }

            if (flags.Get(GameFlagIds.IncreasedHairGrowthTimeRemaining) < 7)
                flags.Set(GameFlagIds.IncreasedHairGrowthTimeRemaining, 7);
        }
    }

    // ── 原版 BeautifulSwordShard.hx ─────────────────────────────────────
    public class BeautifulSwordShardEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var combat = CombatController.Instance;

            if (combat?.Context is not { } combatContext || combatContext.Enemy is null)
            {
                if (context.Item != null)
                    context.Player.Inventory.Add(context.Item, 1);

                return;
            }

            var enemy = combatContext.Enemy;

            context.Write("Items.SwordShard.Point".Localized());

            if (enemy.Speed - 80 > context.Random.Next(100) + 1)
                context.Write(enemy.Plural

                    ? "Items.SwordShard.DodgedPlural".Localized()

                    : "Items.SwordShard.Dodged".Localized());
            else
            {
                var damage = (70 + context.Random.Next(61)) * (1 + enemy.Corruption / 100f);

                context.Write("Items.SwordShard.Hit".Localized((int)damage));

                enemy.CreateStatusEffect(StatusEffectIds.Blind, 2, 0, 0, 0);

                enemy.HealthChange(-damage);
            }
        }
    }

    // ── 原版 WingStick.hx ───────────────────────────────────────────────
    public class WingstickEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var combat = CombatController.Instance;

            if (combat?.Context is not { } combatContext || combatContext.Enemy is null)
            {
                if (context.Item != null)
                    context.Player.Inventory.Add(context.Item, 1);

                return;
            }

            var player = context.Player;

            var enemy = combatContext.Enemy;

            context.Write("Items.Wingstick.Throw".Localized());

            var avoid = CombatCalculator.Avoid(combatContext, new AvoidDamageParameters
            {
                Attacker = player,

                Defender = enemy,

                DoDodge = true,

                DoParry = false,

                DoBlock = false,

                ToHitChance = enemy.StandardDodgeFunc(player, 10),
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
                context.Write(enemy.Plural

                    ? "Items.Wingstick.DodgedPlural".Localized()

                    : "Items.Wingstick.Dodged".Localized());
            else
            {
                var damage = 50 + context.Random.Next(61) + player.Strength * 2;

                damage = CombatCalculator.ReduceDamage(combatContext, enemy, damage);

                context.Write("Items.Wingstick.Hit".Localized());

                context.Write("Combat.DamageText.Format".Localized("#ff0000", (int)damage));

                enemy.HealthChange(-damage);
            }
        }
    }
}