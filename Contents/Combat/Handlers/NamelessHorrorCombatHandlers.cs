using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class NamelessHorrorCombatHandlers
    {

        [Register("coc:nameless_horror_overwhelm")]
        public static void Overwhelm(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.NamelessHorror.Overwhelm.DozensOfTentacles".Localized());

            context.Enemy.CreateStatusEffect(StatusEffectIds.Attacks, 4);

            var lowHealth = context.Player.HealthRatio() < 0.4f;

            if (lowHealth)
                context.Enemy.Speed = 40;

            for (var i = 0; i < 4; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();

            if (lowHealth)
                context.Enemy.Speed = 100;

            context.Enemy.RemoveStatusEffect(StatusEffectIds.Attacks);
        }

        [Register("coc:nameless_horror_refashion_it")]
        public static void RefashionIt(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.NamelessHorror.RefashionIt.TheCreatureLifts".Localized());

            context.Append("Combat.NamelessHorror.RefashionIt.YouReHit".Localized());

            context.Append("Combat.NamelessHorror.RefashionIt.BeforeYouCan".Localized());

            var player = context.Player;

            player.TakeDamage(25 + context.Random.Next(50));

            player.CreateStatusEffect(StatusEffectIds.Refashioned, 2, 0, 0, 0, 2);

            var horror = context.EnemyAs<NamelessHorrorEnemy>();

            if (horror != null)
            {
                horror.PlayerStats[0] = player.Strength;

                horror.PlayerStats[1] = player.Toughness;

                horror.PlayerStats[2] = player.Intelligence;

                horror.PlayerStats[3] = player.Speed;
            }

            var lucky = context.Random.Next(3);

            player.Strength = 40 + context.Random.Next(180) + (lucky == 0 ? 30 : 0);

            player.Toughness = 40 + context.Random.Next(180) + (lucky == 1 ? 30 : 0);

            player.Intelligence = 40 + context.Random.Next(180) + (lucky == 2 ? 30 : 0);

            player.Speed = 40 + context.Random.Next(180) + (lucky == 3 ? 30 : 0);
        }

        [Register("coc:nameless_horror_reveal")]
        public static void Reveal(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.NamelessHorror.Reveal.TheCreatureLifts".Localized());

            var horror = context.EnemyAs<NamelessHorrorEnemy>();

            horror?.OriginalName = context.Player.ShortName;

            context.PendingNameRestore ??= context.Player.ShortName;

            context.Player.ShortName = "Ephraim";

            context.Player.CreateStatusEffect(StatusEffectIds.Revelation, 2, 50, 0, 0, 2);
        }

        [Register("coc:nameless_horror_become_nothingness")]
        public static void BecomeNothingness(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.NamelessHorror.BecomeNothingness.TheCreatureShrieks".Localized());

            context.Player.CreateStatusEffect(StatusEffectIds.Nothingness, 2, 0, 0, 0, 2);
        }

        [Register("coc:nameless_horror_conjoin")]
        public static void Conjoin(CombatContext context, CombatAbilityDefinition ability)
        {
            var horror = context.EnemyAs<NamelessHorrorEnemy>();

            var doomLevel = horror?.DoomLevel ?? 1f;

            context.Append("Combat.NamelessHorror.Conjoin.TheCreatureShrieks".Localized());

            context.Append("Combat.NamelessHorror.Conjoin.TheVisionShatters".Localized());

            context.Player.TakeDamage(context.Player.Health * 0.25f * doomLevel);

            horror?.DoomLevel += 1;
        }
    }
}