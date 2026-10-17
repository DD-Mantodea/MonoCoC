using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Factory;
using System;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<SecretarialSuccubusEnemy>("coc:secretarial_succubus")]
    public class SecretarialSuccubusEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 2);

            CreateBreastRow(4);

            WeaponName = "claws";

            WeaponVerb = "claw";

            WeaponAttack = 10;

            ArmorName = "demonic skin";

            ArmorDefense = 4;
        }

        public bool _whipReady;

        public override void PerformCombatAction()
        {
            var controller = CombatController.Instance!;

            var context = controller.Context;

            var player = context.Player;

            switch (Random.Shared.Next(4))
            {
                case 0:
                    Kiss(context, player);

                    break;

                case 1:
                    Seduce(context, player);

                    break;

                case 2:
                    Whip(context, player);

                    break;

                default:
                    controller.Resolver.EnemyBasicAttack();

                    break;
            }
        }

        public void Kiss(CombatContext context, Creatures.Player player)
        {
            context.Write("Common.Text.TheSuccubusDancesForwardsCockingHer".Localized());

            var avoided = (player.Speed > Speed && Random.Shared.Next(4) == 0)

                || (player.HasPerk(PerkIds.Evade) && Random.Shared.Next(4) == 0)

                || (player.HasPerk(PerkIds.Misdirection) && Random.Shared.Next(4) == 0 && player.Armor.Name == "red, high-society bodysuit");

            if (avoided)
            {
                context.Write("Common.Text.YouStartToSidestepAndRealize".Localized());

                if (player.Lust >= player.MaxLust * 0.7f)
                    context.Write("Common.Text.MaybeYouShouldntHaveBotheredTo".Localized());

                return;
            }

            context.Write("Common.Text.YouStartToDodgeToThe".Localized());

            if (!player.HasStatusEffect(StatusEffectIds.KissOfDeath))
                player.CreateStatusEffect(StatusEffectIds.KissOfDeath, 0, 0, 0, 0);
        }

        public void Seduce(CombatContext context, Creatures.Player player)
        {
            var lustDamage = 0f;

            switch (Random.Shared.Next(3))
            {
                case 0:
                {
                    context.Write("Common.Text.TheSuccubusSlapsHer".Localized() + ButtDescription(this));

                    var rating = (int)Body.Get<ButtPart>().Rating;

                    if (rating >= 10)
                    {
                        context.Write("Common.Text.MakingItJiggleDelightfully".Localized());

                        if (Random.Shared.Next(100) <= 95)
                        {
                            lustDamage = Random.Shared.Next(rating) + 10;

                            context.Write("Common.Text.NtheDisplayIsQuiteArousing".Localized());
                        }
                        else context.Write("Common.Text.NyoureUnimpressed".Localized());
                    }
                    else
                    {
                        context.Write(".");

                        if (Random.Shared.Next(100) <= 70 + rating * 2)
                        {
                            lustDamage = Random.Shared.Next(rating) + 9;

                            context.Write("Common.Text.NtheDisplayIsQuiteArousing".Localized());
                        }
                        else context.Write("Common.Text.NyoureUnimpressed".Localized());
                    }

                    break;
                }

                case 1:
                {
                    if (player.BreastRows.Count == 0 || player.BreastRows[0].BreastRating < 2)
                        break;

                    if (player.BreastRows.Count == 1)
                    {
                        context.Write("Common.Text.TheSuccubusCaressesSomeOfHer".Localized());

                        if (player.Lust >= player.MaxLust * 0.5f)
                            context.Write("Common.Text.HerHardNipplesSeemToDemand".Localized());

                        if (Random.Shared.Next(100) <= 65 + player.BiggestBreastSize())
                        {
                            context.Write("Common.Text.NtheDisplayIsQuiteArousing".Localized());

                            lustDamage = Random.Shared.Next((int)player.BreastRows[0].BreastRating) + player.BreastRows.Count + 10;
                        }
                        else context.Write("Common.Text.NyoureUnimpressed".Localized());
                    }
                    else
                    {
                        context.Write("Common.Text.TheSuccubusCaressesSomeOfHer2".Localized());

                        if (player.Lust >= player.MaxLust * 0.5f)
                            context.Write("Common.Text.YourNipplesPainfullyVisible".Localized());
                        else context.Write(".");

                        if (Random.Shared.Next(100) <= 54 + (player.BreastRows.Count - 1) * 15 + player.BreastRows[0].BreastRating)
                        {
                            context.Write("Common.Text.NtheDisplayIsQuiteArousing".Localized());

                            lustDamage = Random.Shared.Next((int)player.BreastRows[0].BreastRating) + player.BreastRows.Count * player.BreastRows[0].Breasts + 5;
                        }
                        else context.Write("Common.Text.NyoureUnimpressed".Localized());
                    }

                    break;
                }

                case 2:
                {
                    context.Write("Common.Text.TheSuccubusReachesDownAndStrokes".Localized());

                    if (Random.Shared.Next(101) <= 65 + player.Vaginas.Count * 10 + player.Cocks.Count * 10)
                    {
                        context.Write("Common.Text.NtheDisplayIsQuiteArousing".Localized());

                        lustDamage = Random.Shared.Next(player.Vaginas.Count * 2 + player.Cocks.Count * 2) + 13;
                    }
                    else context.Write("Common.Text.NyoureUnimpressed".Localized());

                    break;
                }
            }

            if (lustDamage > 0)
                player.TakeLustDamage(lustDamage);
        }

        public void Whip(CombatContext context, Creatures.Player player)
        {
            if (!_whipReady)
            {
                context.Write("Common.Text.TheSuccubusFlicksHerWristAllowing".Localized());

                _whipReady = true;

                Strength += 20;

                WeaponName = "whip";

                WeaponVerb = "brutal whip-crack";

                return;
            }

            if (HasStatusEffect(StatusEffectIds.Blind))
            {
                context.Write("Common.Text.TheSuccubusSwingsHerWhipAt".Localized());

                return;
            }

            context.Write("Common.Text.GrinningDeviouslyTheSuccubusCracksHer".Localized());

            var damage = 0f;

            var lustDamage = 0f;

            switch (Random.Shared.Next(6))
            {
                case 0:
                    context.Write("Common.Text.Ass".Localized());

                    damage = 4;

                    lustDamage = 6 + player.Sensitivity / 20;

                    break;

                case 1:
                    context.Write(player.BiggestBreastSize() >= 1 ? player.AllBreastsDescript() : "Common.Text.Chest".Localized());

                    damage = Math.Max(0, 9 - player.Armor.DefenseFor(player));

                    if (damage <= 0) context.Write("Common.Text.ButYouFeelNoPainThanks".Localized());
                    lustDamage = 4 + player.Sensitivity / 15;

                    break;

                case 2:
                {
                    var groinProtection = 0;

                    if (player.Armor.Id.Path != "nothing")
                    {
                        groinProtection = player.Armor.Perk switch
                        {
                            "Light" => 1,

                            "Medium" => 2,

                            _ => 3
                        };

                        if (player.Armor.Id.Path is "bee_armor" or "silk_armor" or "dbarmor" or "tbarmor" or "goo_armor")
                            groinProtection = 2;
                    }

                    if (player.LowerGarment.Id.Path is "ss_loin" or "ss_panty")
                        groinProtection += 1;
                    else if (player.LowerGarment.Id.Path is "ds_loin" or "ds_thong")
                        groinProtection += 2;

                    if (groinProtection >= 3)
                    {
                        context.Write("Common.Text.GroinButLuckilyYoureWearingGroin".Localized());

                        damage = 0;
                    }
                    else if (!player.HasCock() && !player.HasVagina())
                    {
                        context.Write("Common.Text.Groin".Localized());

                        damage = 5f / (groinProtection + 1);
                    }
                    else if (player.HasCock() && !player.HasVagina())
                    {
                        context.Write("Common.Text.GroinDealingPainfulDamageToYour".Localized());

                        damage = player.MaxHealth / 4f;

                        if (groinProtection > 0)
                        {
                            context.Write("Common.Text.OfCourseItWouldHaveBeen".Localized());

                            damage /= groinProtection + 1;
                        }

                        damage = CombatCalculator.ReduceDamage(context, player, damage);

                        if (damage < 20) damage = 10;
                        lustDamage = -15;
                    }
                    else if (!player.HasCock() && player.HasVagina())
                    {
                        context.Write("Common.Text.GroinMakingYourVaginaStingWith".Localized());

                        damage = 10;

                        lustDamage = -8;
                    }
                    else
                    {
                        context.Write("Common.Text.GroinDealingPainfulDamageToYour2".Localized());

                        damage = player.MaxHealth / 3f;

                        if (groinProtection > 0)
                        {
                            context.Write("Common.Text.OfCourseItWouldHaveBeen".Localized());

                            damage /= groinProtection + 1;
                        }

                        damage = CombatCalculator.ReduceDamage(context, player, damage);

                        if (damage < 30) damage = 30;
                        lustDamage = -20;
                    }

                    break;
                }

                case 3:
                    context.Write("Common.Text.Legs".Localized());

                    damage = 7;

                    break;

                case 4:
                    context.Write("Common.Text.Arms".Localized());

                    damage = 8;

                    break;

                default:
                    context.Write("Common.Text.Neck".Localized());

                    damage = 24;

                    break;
            }

            context.Write("!");

            player.HealthChange(-damage);

            if (lustDamage != 0) player.TakeLustDamage(lustDamage);
        }

        public static string ButtDescription(Creatures.Creature creature) =>

            creature.Body.Get<ButtPart>().Id.Path == "none" ? "butt" : Creatures.Appearance.ButtDescript(creature);

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<SecretarialSuccubusScene>.Get().DefeatSuccubus();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<SecretarialSuccubusScene>.Get().DoLossSuccubus();

            return true;
        }
    }

    [Register<OmnibusOverseerEnemy>("coc:omnibus_overseer")]
    public class OmnibusOverseerEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(12, 2);

            Balls = 2;

            BallSize = 2;

            WeaponName = "wrench";

            WeaponVerb = "smack";

            WeaponAttack = 10;

            ArmorName = "overalls";

            ArmorDefense = 15;
        }

        public override void PerformCombatAction()
        {
            var context = CombatController.Instance!.Context;

            var player = context.Player;

            switch (Random.Shared.Next(3))
            {
                case 0:
                    LustAura(context, player);

                    break;

                case 1:
                    MilkAttack(context, player);

                    break;

                default:
                    CombatController.Instance!.Resolver.EnemyBasicAttack();

                    break;
            }
        }

        public void LustAura(CombatContext context, Creatures.Player player)
        {
            context.Write("Common.Text.TheDemonessBlinksHerEyesClosed".Localized());

            if (HasStatusEffect(StatusEffectIds.LustAura))
            {
                context.Write("Common.Text.YourEyesCrossWithUnexpectedFeelings".Localized());

                player.TakeLustDamage(8 + player.Libido / 20 + player.Corruption / 25);
            }
            else
                CreateStatusEffect(StatusEffectIds.LustAura, 0, 0, 0, 0);
        }

        public void MilkAttack(CombatContext context, Creatures.Player player)
        {
            context.Write(Random.Shared.Next(2) == 0

                ? "The demoness grips her sizable breasts and squeezes, spraying milk at you.\n"

                : "Your foe curls up to pinch her nipples, tugging hard and squirting milk towards you.\n");

            var avoided = (player.Speed > 50 && Random.Shared.Next(4) == 0)

                || (player.HasPerk(PerkIds.Evade) && Random.Shared.Next(3) == 0)

                || (player.HasPerk(PerkIds.Misdirection) && Random.Shared.Next(4) == 0 && player.Armor.Name == "red, high-society bodysuit");

            if (avoided)
            {
                context.Write("Scenes.OmnibusOverseer.MilkAttack.MilkDodged".Localized());

                return;
            }

            if (Random.Shared.Next(2) == 0)
                context.Write("Scenes.OmnibusOverseer.MilkAttack.MilkOnFace".Localized());
            else
            {
                context.Write("Common.Text.TheMilkSplashesIntoYourArmor".Localized());

                if (player.Cocks.Count > 0)
                {
                    context.Write("Common.Text.YourCockGetsHardAsThe".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 5);
                }

                if (player.Vaginas.Count > 0)
                {
                    context.Write("Scenes.OmnibusOverseer.MilkAttack.MilkArousesPussy".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 5);
                }
            }

            player.TakeLustDamage(7 + player.Sensitivity / 20);

            if (player.BiggestLactation() > 1)
                context.Write("Common.Text.MilkDribblesFromYourBreastsIn".Localized());
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<OmnibusOverseerScene>.Get().WinAgainstOmnibus();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<OmnibusOverseerScene>.Get().DoLossOmnibus();

            return true;
        }
    }
}