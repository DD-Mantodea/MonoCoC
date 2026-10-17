using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Factory;
using System;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<IncubusMechanicEnemy>("coc:incubus_mechanic")]
    public class IncubusMechanicEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(12, 2);

            Balls = 2;

            BallSize = 2;

            WeaponName = "wrench";

            WeaponVerb = "smack";

            WeaponAttack = 15;

            ArmorName = "overalls";

            ArmorDefense = 10;
        }

        public override void PerformCombatAction()
        {
            var controller = CombatController.Instance!;

            var context = controller.Context;

            switch (Random.Shared.Next(3))
            {
                case 0:
                    CockTrip(context);

                    break;

                case 1:
                    Spooge(context);

                    break;

                default:
                    controller.Resolver.EnemyBasicAttack();

                    break;
            }
        }

        public void CockTrip(CombatContext context)
        {
            var player = context.Player;

            if (HasStatusEffect(StatusEffectIds.Blind))
            {
                context.Write("Common.Text.TheIncubusSuddenlyGrowsHisDick".Localized());

                return;
            }

            context.Write("Common.Text.TheIncubusLungesForwardInA".Localized());

            if (player.Speed - 30 > Random.Shared.Next(60))
                context.Write("Common.Text.YouSpinAsYouFallTwisting".Localized());
            else
            {
                context.Write("Common.Text.YouLandHardOnYourAss".Localized());

                if (player.Lust >= player.MaxLust * 0.8f || player.Corruption >= 80)
                {
                    context.Write("Common.Text.MoaningWithDesireYouLickYour".Localized());

                    player.TakeLustDamage(13);

                    player.Corruption = Math.Min(100, player.Corruption + 1);
                }
                else if (player.Lust >= player.MaxLust * 0.5f || player.Corruption >= 50)
                {
                    context.Write("Common.Text.BlushingAtTheScentAndFeel".Localized());

                    player.TakeLustDamage(8 + player.Corruption / 20);
                }
                else
                {
                    context.Write("Common.Text.DisgustedYouPullAwayFromThe".Localized());

                    player.TakeLustDamage(5 + player.Corruption / 20);
                }

                player.HealthChange(-5);
            }

            context.Write("Common.Text.TheIncubusGivesAnOverconfidentSmile".Localized());
        }

        public void Spooge(CombatContext context)
        {
            var player = context.Player;

            if (HasStatusEffect(StatusEffectIds.Blind))
            {
                context.Write("Common.Text.TheIncubusPumpsAndThrustsHis".Localized());

                return;
            }

            context.Write("Common.Text.YourDemonicFoePlacesHisHands".Localized());

            if (player.Shield.Is(ItemIds.DragonShield) && Random.Shared.Next(2) == 0)
            {
                context.Write("Scenes.IncubusMechanic.SpoogeAttack.ShieldAbsorbsCum".Localized());

                return;
            }

            context.Write("Scenes.IncubusMechanic.SpoogeAttack.DodgePartialLead".Localized());

            switch (Random.Shared.Next(3))
            {
                case 0:
                    context.Write("Scenes.IncubusMechanic.SpoogeAttack.SeedOnFace".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 3);

                    if (!player.HasStatusEffect(StatusEffectIds.DemonSeed))
                        player.CreateStatusEffect(StatusEffectIds.DemonSeed, 5, 0, 0, 0);
                    else
                        player.ChangeStatusValue(StatusEffectIds.DemonSeed, 1, 7);

                    player.SlimeFeed();

                    break;

                case 1:
                    if (player.HasFuckableNipples())
                    {
                        context.Write(player.AllBreastsDescript() + "Common.Text.TheGooeyDemonSeedOozesAnd".Localized());

                        player.Lust = Math.Min(player.MaxLust, player.Lust + 3);

                        if (!player.HasStatusEffect(StatusEffectIds.DemonSeed))
                            player.CreateStatusEffect(StatusEffectIds.DemonSeed, 5, 0, 0, 0);
                        else
                            player.ChangeStatusValue(StatusEffectIds.DemonSeed, 1, 8);

                        player.SlimeFeed();
                    }
                    else
                        context.Write(player.AllBreastsDescript() + "Common.Text.ThankfullyItDoesntSeemToHave".Localized());

                    break;

                default:
                    if (player.Vaginas.Count > 0)
                    {
                        context.Write("crotch. The gooey demon-seed oozes and slides over you with a mind of its own, forcing its way past your armor and into your " + player.VaginaDescript(0) + "Common.Text.YouCanFeelItMovingAround".Localized());

                        player.Lust = Math.Min(player.MaxLust, player.Lust + 3);

                        if (!player.HasStatusEffect(StatusEffectIds.DemonSeed))
                            player.CreateStatusEffect(StatusEffectIds.DemonSeed, 5, 0, 0, 0);
                        else
                            player.ChangeStatusValue(StatusEffectIds.DemonSeed, 1, 8);

                        player.SlimeFeed();
                    }
                    else
                        context.Write("Scenes.IncubusMechanic.SpoogeAttack.CrotchNoEffect".Localized());

                    break;
            }

            Lust = Math.Max(10, Lust - 10);
        }

        public override bool Defeated(bool hpVictory)
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.D3Discovered) == 0)
                SceneInstance<IncubusMechanicScenes>.Get().DefeatIncubus();
            else
                SceneInstance<IncubusMechanicScenes>.Get().BeatDaMechanic(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<IncubusMechanicScenes>.Get().DoLossIncubus();

            return true;
        }
    }
}