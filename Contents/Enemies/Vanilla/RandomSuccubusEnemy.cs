using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<RandomSuccubusEnemy>("coc:random_succubus")]
    public class RandomSuccubusEnemy : Enemy
    {

        public string RequestedGender = "R";

        public string Special = "";

        public static readonly Identifier Grappling = new("coc", "grappling");

        public static readonly Identifier TentacleType = new("coc", "tentacle");

        public static readonly Identifier GrappleId = new("coc", "random_succubus_grapple");

        public static readonly Identifier SwipeId = new("coc", "random_succubus_swipe");

        public static readonly Identifier LustMagicAttackId = new("coc", "random_succubus_lust_magic_attack");

        public static readonly Identifier TailWhipId = new("coc", "random_succubus_tail_whip");

        public static readonly Identifier HeelKickId = new("coc", "random_succubus_heel_kick");

        public static readonly Identifier HlGrappleId = new("coc", "random_succubus_hl_grapple");

        public static RandomSuccubusEnemy Create(EnemyDefinition definition, string gender, string special)
        {
            var enemy = new RandomSuccubusEnemy { RequestedGender = gender, Special = special };

            enemy.Configure(definition);

            return enemy;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var gender = RequestedGender;

            var vag = true;

            Tallness = 3 * 12;

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.DemonicHighHeels;

            if (gender == "R" && R(10) < 1)
                gender = R(2) switch
                {
                    0 => "M",

                    1 => "H",

                    _ => gender
                };

            if (gender == "M" || gender == "H")
            {

                CreateCock(5 + R(7) + R(7) - R(5), R(501) * 0.01f + 1, BodyPartTypes.Demon);

                Femininity = gender == "M"

                    ? R(10) + R(10) + R(20) + R(60) + R(4) + 1

                    : R(100) + 1;

                vag = false;
            }

            if (gender == "H")
                vag = true;

            if (vag)

                CreateVagina(false, R(5) + 1, R(5) + 1);

            switch (Gender)
            {
                case Creatures.Gender.Female:
                    ShortName = "succubus";

                    CreateBreastRow(R(6) + R(5) + 1);

                    Body.Hips = R(6) + R(6) + R(6) + R(6);

                    Body.Butt = R(6) + R(6) + R(6) + R(6);

                    Tallness += R(7) + R(7) + R(13) + R(13) + R(13) + R(13);

                    Body.Get<HairPart>().Length = R(7) + R(7) + R(7) + R(7) + R(7) + R(7) + R(7) + R(7);

                    break;

                case Creatures.Gender.Male:
                    ShortName = "incubus";

                    Body.Hips = R(2) + R(2) + R(2) + R(2) + R(2) + R(2) + R(2) + R(2);

                    Body.Butt = R(3) + R(3) + R(3);

                    Balls = R(2) * 2 + 2 - R(2) * 2;

                    BallSize = R(10) + 1;

                    Tallness += R(7) + R(7) + R(7) + R(7) + R(13) + R(13) + R(13) + R(13);

                    Body.Get<LowerBodyPart>().Id = BodyPartTypes.Human;

                    Body.Get<HairPart>().Length = R(13);

                    break;

                case Creatures.Gender.Hermaphrodite:
                    ShortName = "omnibus";

                    CreateBreastRow(R(11));

                    Body.Hips = R(21);

                    Body.Butt = R(21);

                    Balls = R(2) * 2 + 2 - R(2) * 2;

                    BallSize = R(11);

                    Tallness += R(7) + R(7) + R(7) + R(7) + R(13) + R(13) + R(25);

                    Body.Get<HairPart>().Length = R(31);

                    break;
            }

            var anal = Body.Get<AnalPart>();

            anal.Looseness = R(5) + 1;

            anal.Wetness = R(6);

            string[] skinTones = ["blue", "purple", "light purple", "dark purple", "dark blue", "indigo", "burgundy", "lavender", "midnight-black"];

            Body.Get<SkinPart>().Tone = skinTones[R(skinTones.Length)];

            string[] hairColors = ["blue", "purple", "light purple", "dark purple", "dark blue", "blond"];

            Body.Get<HairPart>().Color = hairColors[R(hairColors.Length)];

            Identifier[] tails = [BodyPartTypes.None, BodyPartTypes.None, BodyPartTypes.Demonic];

            Body.Get<TailPart>().Id = tails[R(tails.Length)];

            Identifier[] horns =

            [
                BodyPartTypes.None, BodyPartTypes.None,

                BodyPartTypes.Demon, BodyPartTypes.Ram
            ];

            var hornsPart = Body.Get<HornsPart>();

            hornsPart.Id = horns[R(horns.Length)];

            if (hornsPart.Id != BodyPartTypes.None)
                hornsPart.Value = R(4) + 1;

            Identifier[] wings =

            [
                BodyPartTypes.None, BodyPartTypes.None, BodyPartTypes.None,

                BodyPartTypes.BatLikeTiny, BodyPartTypes.BatLikeTiny,

                BodyPartTypes.BatLikeLarge, BodyPartTypes.Imp,

                BodyPartTypes.ImpLarge, BodyPartTypes.DraconicSmall,

                BodyPartTypes.FeatheredLarge
            ];

            var wingsPart = Body.Get<WingsPart>();

            wingsPart.Id = wings[R(wings.Length)];

            if (wingsPart.Id == BodyPartTypes.FeatheredLarge)
                wingsPart.Color = "black";

            Identifier[] ears =

            [
                BodyPartTypes.Elfin, BodyPartTypes.Elfin, BodyPartTypes.Elfin,

                BodyPartTypes.Human, BodyPartTypes.Imp
            ];

            Body.Get<EarsPart>().Id = ears[R(ears.Length)];

            if (HasCock())
            {
                Identifier[] cockTypes =

                [
                    BodyPartTypes.Demon, BodyPartTypes.Dog, BodyPartTypes.Horse,

                    BodyPartTypes.Human, TentacleType
                ];

                Cocks[0].Id = cockTypes[R(cockTypes.Length)];

                if (Cocks[0].Id == TentacleType)
                    while (R(3) < 1)

                        CreateCock(5 + R(7) + R(7) - R(5), R(501) * 0.01f + 1, TentacleType);
            }

            Strength = 50 + R(51);

            Toughness = 40 + R(61);

            Speed = 50 + R(26) + R(26);

            Intelligence = 80 + R(21);

            Libido = 70 + R(31);

            Sensitivity = 60 + R(41);

            Corruption = 100 - R(11);

            Level = 7 + R(5);

            WeaponName = "claws";

            WeaponVerb = "punch";

            WeaponAttack = 10 + R(11);

            ArmorName = "demonic skin";

            ArmorDefense = 5 + R(6);

            var bonusHealth = (float)(R(11) * 100);

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            Gems = 100 + R(1001) + R(11) * 10;

            AdditionalExperience = 250 + R(251);

            if (Special == "LiddelliumCampSucc")
            {
                string[] specialSkin =

                [
                    "blue", "indigo", "purple", "burgundy", "lavender",

                    "blue", "indigo", "purple", "burgundy", "lavender", "midnight-black"
                ];

                Body.Get<SkinPart>().Tone = specialSkin[R(specialSkin.Length)];

                hornsPart.Id = BodyPartTypes.Demon;

                hornsPart.Value = R(4) + 1;

                string[] specialHair = ["black ", "auburn ", "violet ", "navy-blue ", ""];

                Body.Get<HairPart>().Color = specialHair[R(specialHair.Length)];

                Body.Get<TailPart>().Id = BodyPartTypes.Demonic;

                wingsPart.Id = BodyPartTypes.BatLikeLarge;
            }
            else if (Special == "LiddelliumHL")
            {
                Body.Get<SkinPart>().Tone = "deep purple";

                Body.Get<HairPart>().Color = "black";

                Body.Get<TailPart>().Id = BodyPartTypes.Demonic;

                wingsPart.Id = BodyPartTypes.BatLikeLarge;

                Strength += 25;

                Toughness += 30;

                Speed += 30;

                Intelligence += 10;

                Libido += 15;

                Sensitivity += 20;

                Corruption += 10;

                Level += 2;

                WeaponAttack += 5;

                ArmorDefense += 3;

                Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                    () => 500f, new TemporarySource(Guid.NewGuid()));

                Health = MaxHealth;
            }
        }

        public static int R(int n) => n <= 0 ? 0 : Random.Shared.Next(n);

        public override bool BeforeCombatAction(CombatContext context)
        {
            var player = context.Player;

            if (player.HasStatusEffect(StatusEffectIds.Grappled)
                && !HasStatusEffect(Grappling))
            {
                if (Special == "LiddelliumHL")
                    HlGrappleTease(context, (int)player.StatusEffectValue(StatusEffectIds.Grappled, 2));
                else
                    Grapple(context);

                return true;
            }

            if (HasStatusEffect(Grappling))
            {
                if (Special == "LiddelliumHL")
                    HlGrappleTease(context, (int)player.StatusEffectValue(StatusEffectIds.Grappled, 2));
                else
                    GrappleTease(context);

                return true;
            }

            var options = new List<(Identifier Id, float Weight, int Cost, FatigueType Fatigue)>();

            if (Special != "LiddelliumHL" && Distance == CombatDistance.Melee)
                options.Add((GrappleId, 1, 0, FatigueType.None));

            options.Add((SwipeId, 1, 0, FatigueType.None));

            if (HasFatigue(10, FatigueType.Magical))
                options.Add((LustMagicAttackId, 0.5f, 10, FatigueType.Magical));

            if (Body.Get<TailPart>().Id == BodyPartTypes.Demonic && HasFatigue(5, FatigueType.Physical))
                options.Add((TailWhipId, 0.5f, 5, FatigueType.Physical));

            if (Body.Get<LowerBodyPart>().Id == BodyPartTypes.DemonicHighHeels
                && HasFatigue(5, FatigueType.Physical))

                options.Add((HeelKickId, 0.5f, 5, FatigueType.Physical));

            if (Special == "LiddelliumHL"
                && !player.HasStatusEffect(StatusEffectIds.Grappled)

                && context.Enemies.Count >= 2

                && context.Enemies[0].Health > 0 && context.Enemies[1].Health > 0

                && context.Enemies[0].Lust < context.Enemies[0].MaxLust

                && context.Enemies[1].Lust < context.Enemies[1].MaxLust

                && HasFatigue(10, FatigueType.Physical))

                options.Add((HlGrappleId, 1, 10, FatigueType.Physical));

            if (options.Count == 0)
            {
                CombatController.Instance!.Resolver.EnemyBasicAttack();

                return true;
            }

            var total = 0f;

            foreach (var option in options)
                total += option.Weight;

            var roll = context.Random.NextDouble() * total;

            var chosen = options[^1];

            foreach (var option in options)
            {
                roll -= option.Weight;

                if (roll <= 0)
                {
                    chosen = option;

                    break;
                }
            }

            var ability = CombatAbilityRegistry.Get(chosen.Id)

                ?? new CombatAbilityDefinition { Id = chosen.Id, Handler = chosen.Id };

            if (chosen.Cost > 0)
                ChangeFatigue(chosen.Cost, chosen.Fatigue);

            CombatController.Instance!.Resolver.EnemyAction(ability);

            return true;
        }

        public override void Struggle(CombatContext context)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(StatusEffectIds.Grappled))
                return;

            context.Clear();

            var totalStrength = 0f;

            foreach (var grappler in context.Enemies)
                if (grappler.HasStatusEffect(Grappling))
                    totalStrength += (int)grappler.Strength;

            var grapplers = Math.Max(1, (int)player.StatusEffectValue(StatusEffectIds.Grappled, 1));

            var escape = player.StatusEffectValue(StatusEffectIds.Grappled, 2) > 6

                || (player.Strength + context.Random.Next(100))

                    > (totalStrength / grapplers + context.Random.Next(100) + 10 * grapplers);

            if (escape)
            {
                context.Append(Special == "LiddelliumHL"

                    ? "Combat.RandomSuccubus.Struggle.YouOvercomeThe".Localized()

                    : "Combat.RandomSuccubus.Struggle.YouManageTo".Localized());

                player.RemoveStatusEffect(StatusEffectIds.Grappled);

                foreach (var grappler in context.Enemies)
                    grappler.RemoveStatusEffect(Grappling);
            }
            else
                context.Append(Special == "LiddelliumHL"

                    ? "Combat.RandomSuccubus.Struggle.YouStruggleTo".Localized()

                    : "Combat.RandomSuccubus.Struggle.YouStruggleTo2".Localized());
        }

        public void Grapple(CombatContext context)
        {
            var player = context.Player;

            if (player.HasStatusEffect(StatusEffectIds.Grappled))
            {
                context.Append("Combat.RandomSuccubus.Grapple.The0Joins".Localized(ShortName));

                player.CreateOrFindStatusEffect(StatusEffectIds.Grappled).Values[1] += 1;

                CreateStatusEffect(Grappling, 0, 0, 0, 0);

                return;
            }

            context.Append("Combat.RandomSuccubus.Grapple.The0Charges".Localized(ShortName, IsMale() ? "Common.Text.His".Localized() : "Common.Text.Her".Localized()));

            if (player.Speed + context.Random.Next(100) < Speed + context.Random.Next(100))
            {
                context.Append("Combat.RandomSuccubus.Grapple.ZeroTacklesYou".Localized(IsMale() ? "Common.Text.He".Localized() : "Common.Text.She".Localized(), IsMale() ? "Common.Text.His".Localized() : "Common.Text.Her".Localized()));

                if (player.Strength + context.Random.Next(100) < Strength + context.Random.Next(100))
                {
                    context.Append("Combat.RandomSuccubus.Grapple.ZeroHasYou".Localized(IsMale() ? "Common.Text.He".Localized() : "Common.Text.She".Localized(), IsMale() ? "Common.Text.His".Localized() : "Common.Text.Her".Localized()));

                    player.CreateOrFindStatusEffect(StatusEffectIds.Grappled).Values[1] += 1;

                    CreateStatusEffect(Grappling, 0, 0, 0, 0);
                }
                else
                    context.Append("Combat.RandomSuccubus.Grapple.ThankfullyYouManage".Localized(IsMale() ? "Common.Text.Him".Localized() : "Common.Text.Her".Localized()));
            }
            else
                context.Append("Combat.RandomSuccubus.Grapple.HoweverYourSpeed".Localized(IsMale() ? "Common.Text.He".Localized() : "Common.Text.She".Localized()));
        }

        public void GrappleTease(CombatContext context)
        {
            var player = context.Player;

            if (!IsMale() && Body.Get<TailPart>().Id != BodyPartTypes.None)
                context.Append("Combat.RandomSuccubus.GrappleTease.HerTailWriggles".Localized());

            var lust = (int)((5 + context.Random.Next(6))

                * ((player.Sensitivity + player.Corruption + player.Libido) / 100));

            if (lust > 10)
                context.Append("Combat.RandomSuccubus.GrappleTease.ThisLewdEmbrace".Localized());

            player.TakeLustDamage(lust, true);
        }

        public void HlGrapple(CombatContext context)
        {
            context.Append("Combat.RandomSuccubus.HlGrapple.TheSuccubusDashes".Localized());

            CreateStatusEffect(Grappling, 0, 0, 0, 0);

            context.Player.CreateStatusEffect(StatusEffectIds.Grappled, 0, 0, 1, 0);
        }

        public void HlGrappleTease(CombatContext context, int turn)
        {
            var player = context.Player;

            CreateOrFindStatusEffect(Grappling);

            player.CreateOrFindStatusEffect(StatusEffectIds.Grappled).Values[2] += 1;

            var lust = (int)((10 + context.Random.Next(11))

                * ((player.Sensitivity + player.Corruption + player.Libido) / 100));

            switch (turn)
            {
                case 1:
                    return;

                case 2:
                    context.Append("Combat.RandomSuccubus.HlGrappleTease.StillFirmlyGrabbed".Localized());

                    player.TakeLustDamage(lust, true);

                    return;

                case 3:
                    return;

                case 4:
                    context.Append("Combat.RandomSuccubus.HlGrappleTease.TheSuccubusLays".Localized());

                    player.TakeLustDamage(lust + 5, true);

                    return;

                case 5:
                    return;

                case 6:
                    context.Append("Combat.RandomSuccubus.HlGrappleTease.LargeBatLike".Localized());

                    player.TakeLustDamage(lust + 10, true);

                    return;

                default:
                    Struggle(context);

                    return;
            }
        }

    }
}