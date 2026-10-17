using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using System;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<NephilaCovenEnemy>("coc:nephila_coven")]
    public class NephilaCovenEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 4, 0);

            CreateBreastRow(9);

            Tallness = 66;

            Body.Get<SkinPart>().Tone = "opaque white";

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.Goo;

            WeaponName = "back tentacles";

            WeaponVerb = "tentacle strikes";

            WeaponAttack = 15;

            ArmorName = "soaked nun's habit";
        }

        public override void PerformCombatAction()
        {
            var context = CombatController.Instance!.Context;

            var player = context.Player;

            if (player.HasStatusEffect(StatusEffectIds.Bound))
            {
                WhipBinding(context, player);

                return;
            }

            if (HasStatusEffect(StatusEffectIds.Uber))
            {
                UberCharge(context, player);

                return;
            }

            switch (Random.Shared.Next(4))
            {
                case 0:
                    CombatController.Instance!.Resolver.EnemyBasicAttack();

                    break;

                case 1:
                    UberCharge(context, player);

                    break;

                case 2:
                    WhipBinding(context, player);

                    break;

                default:
                    DoubleAttack(context);

                    break;
            }
        }

        public void UberCharge(CombatContext context, Player player)
        {
            if (!HasStatusEffect(StatusEffectIds.Uber))
            {
                context.Write(Random.Shared.Next(2) == 0

                    ? "The nephila coven sister caresses your fecundity with her tentacles. \"So beautiful,\" she says. \"You're so much bigger than I imagined.\""

                    : "The nephila coven sister rubs her bloated, gelatinous belly. \"This is your doing, Mother,\" she says. \"You've packed us so full with your children and we're all so very, very excited to finally get to meet you.\"");

                context.Write("Common.Text.SheRollsOntoHerBellySpreading".Localized());

                CreateStatusEffect(StatusEffectIds.Uber, 0, 0, 0, 0);

                return;
            }

            if (StatusEffectValue(StatusEffectIds.Uber, 0) == 0)
            {
                ChangeStatusValue(StatusEffectIds.Uber, 0, 1);

                context.Write("Common.Text.TheSlimeGroansAndFallsTo".Localized());

                return;
            }

            RemoveStatusEffect(StatusEffectIds.Uber);

            if (CoC.Instance.Flags.Get(GameFlagIds.InCombatUsePlayerWaitedFlag) == 1)
            {
                context.Write("Common.Text.HerCreakingGooeyBellyFinallyGives".Localized());

                return;
            }

            context.Write("Common.Text.IgnoringTheVoiceOfWorryIn".Localized());

            player.Lust = player.MaxLust;
        }

        public void WhipBinding(CombatContext context, Player player)
        {
            if (!player.HasStatusEffect(StatusEffectIds.Bound))
            {
                context.Write("Scenes.NephilaCoven.NephilaCovenSpecial2.SnapsTentacles".Localized());

                player.CreateStatusEffect(StatusEffectIds.Bound, 2 + Random.Shared.Next(5), 0, 0, 0);

                return;
            }

            if (Random.Shared.Next(2) == 0)
            {
                context.Write("Common.Text.TheNephilaCovenSisterCuddlesUp".Localized());

                player.TakeLustDamage(9 + player.Sensitivity / 10);

                return;
            }

            context.Write("Common.Text.TheNephilaCovenSisterBlowsHot".Localized());

            player.TakeLustDamage(5 + player.Corruption / 10 + player.Libido / 20);
        }

        public void DoubleAttack(CombatContext context)
        {
            if (Lust >= MaxLust * 0.75f)
            {
                context.Write("Common.Text.TheNephilaCovenSisterSpreadsHer".Localized());

                Level += 1;

                Strength += 10;

                Toughness += 10;

                Health = Math.Min(MaxHealth, Health + 20);

                Lust = 33;

                context.Player.TakeLustDamage(3);

                return;
            }

            context.Write("Common.Text.TheGooGirlHeavesHerBody".Localized());

            CombatController.Instance!.Resolver.EnemyBasicAttack();

            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<NephilaCovenScene>.Get().WinRapeChoices();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<NephilaCovenScene>.Get().NephilaCovenRapesYou();

            return true;
        }
    }
}