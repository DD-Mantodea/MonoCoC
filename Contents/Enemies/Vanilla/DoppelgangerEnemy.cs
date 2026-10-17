using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<DoppelgangerEnemy>("coc:doppelganger")]
    public class DoppelgangerEnemy : Enemy
    {

        public int _roundCount;

        public static readonly Identifier StunnedId = StatusEffectIds.Stunned;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var player = CoC.Instance.Player;

            Race = "Demon";

            Tallness = player.Tallness;

            Balls = player.Balls;

            BallSize = player.BallSize;

            HoursSinceCum = player.HoursSinceCum;

            Body.Get<HipsPart>().Rating = Math.Max(1f, player.Body.Get<HipsPart>().Rating);

            Body.Get<ButtPart>().Rating = Math.Max(1f, player.Body.Get<ButtPart>().Rating);

            Body.Get<LowerBodyPart>().Id = player.Body.Get<LowerBodyPart>().Id;

            var skin = Body.Get<SkinPart>();

            var playerSkin = player.Body.Get<SkinPart>();

            skin.Id = playerSkin.Id;

            skin.Tone = playerSkin.Tone;

            skin.DescKey = playerSkin.DescKey;

            Body.Get<FacePart>().Id = player.Body.Get<FacePart>().Id;

            Strength = player.Strength;

            Toughness = player.Toughness;

            Speed = player.Speed;

            Intelligence = player.Intelligence;

            Libido = player.Libido;

            Sensitivity = player.Sensitivity;

            Corruption = Math.Max(player.Corruption, 50f);

            WeaponName = player.WeaponName;

            WeaponAttack = player.WeaponAttack;

            WeaponVerb = player.WeaponVerb;

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = player.ArmorName;

            ArmorDefense = player.ArmorDefense;

            Level = player.Level;

            var anal = Body.Get<AnalPart>();

            var playerAnal = player.Body.Get<AnalPart>();

            anal.Looseness = playerAnal.Looseness;

            anal.Wetness = playerAnal.Wetness;

            foreach (var cock in player.Cocks)
                CreateCock(cock.Length, cock.Thickness, cock.Id);

            if (player.Vaginas.Count > 0)
            {

                var vagina = player.Vaginas[0];

                CreateVagina(vagina.Virgin, vagina.Wetness, vagina.Looseness);
            }
            else if (player.Cocks.Count == 0)

                CreateVagina(false, 6, 2);

            BreastRows.Clear();

            foreach (var row in player.BreastRows)
            {
                CreateBreastRow(row.BreastRating, row.NipplesPerBreast);

                var created = BreastRows[^1];

                created.Breasts = row.Breasts;

                created.Fuckable = row.Fuckable;

                created.Lactation = row.Lactation;

                created.MilkFullness = row.MilkFullness;
            }

            Health = MaxHealth;
        }

        public override string Description
        {
            get
            {
                var player = CoC.Instance.Player;

                var he = Capitalize(player.Mf("he", "she"));

                var his = Capitalize(player.Mf("his", "her"));

                var text = "You are fighting the doppelganger. " + he + " is a "

                    + (int)Math.Floor(player.Tallness / 12) + " foot " + (int)(player.Tallness % 12) + " inch tall "

                    + "[race], with [bodytype]. ";

                text += his + " face has [facelong].";

                text += " " + his + " [hair] is parted by [ears].";

                text += " " + he + " keeps exploring the area around [his] mouth with [his] tongue with a horribly acquisitive, sensual interest.";

                text += " " + he + " moves around on [his] [legs] with a twitchy jerkiness, [his] [hips] swinging and tightening.";

                if (player.Body.Get<TailPart>().Id.Path != "none")
                    text += " " + his + " tail flicks this way and that.";

                text += " " + he + " wields the exact same [weapon] you do, and is dressed in the mirror image of your [armor].";

                if (player.BiggestBreastSize() >= 2)
                    text += " It's difficult not to notice the way the mirror image of your "

                        + player.BreastDescript(player.BiggestTitRow()) + " ebbs and heaves within it.";

                return text;
            }
        }

        public static string Capitalize(string text) =>

            string.IsNullOrEmpty(text) ? text : char.ToUpperInvariant(text[0]) + text[1..];

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<DoppelgangerScenes>.Get().PunchYourselfInTheBalls();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<DoppelgangerScenes>.Get().InSovietCoCSelfFucksYou();

            return true;
        }

        public override bool HandleSpellResistance(CombatContext context, Identifier spell)
        {
            context.WriteAction("Scenes.Doppelganger.HandleSpellResistance.BarelyFlinches".Localized());

            context.WriteAction("Scenes.Doppelganger.HandleSpellResistance.YawnsFlame".Localized());

            var player = context.Player;

            switch (spell.Path)
            {
                case "spell_fireball":

                    context.Player.HealthChange(-(player.Level * 10 + 45 + context.Random.Next(10)));

                    break;

                case "spell_whitefire":

                    context.Player.HealthChange(-(10 + player.Intelligence / 3f

                        + context.Random.Next(Math.Max(1, (int)(player.Intelligence / 2)))));

                    break;

                case "spell_blackfire":

                    context.Player.HealthChange(-(30 + player.Intelligence / 3f

                        + context.Random.Next(Math.Max(1, (int)(player.Intelligence / 2)))));

                    break;
            }

            AddTalkShit(context);

            TookAction = true;

            return true;
        }

        public override bool HandlePlayerWait(CombatContext context)
        {

            context.WriteAction("Scenes.Doppelganger.React.NoAction".Localized());

            context.WriteAction("Scenes.Doppelganger.HandlePlayerWait.WaitsToo".Localized());

            AddTalkShit(context);

            TookAction = true;

            return true;
        }

        public override bool HandleStun(CombatContext context)
        {
            context.WriteAction("Scenes.Doppelganger.DoAI.DuplicateStunned".Localized());

            RemoveStatusEffect(StunnedId);

            return false;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            context.WriteAction("Scenes.Doppelganger.DoAI.DuplicateChuckles".Localized());

            AddTalkShit(context);

            return true;
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.WhenAttacked)
                return true;

            if (HasStatusEffect(StunnedId))
                return true;

            var damage = CombatCalculator.PhysicalDamage(context, context.Player, this);

            MirrorAttack(context, damage);

            return false;
        }

        public void MirrorAttack(CombatContext context, float damage)
        {

            context.WriteAction("Scenes.Doppelganger.MirrorAttack.SwingWeapon".Localized());

            if (damage > 0 && context.Player.Intelligence > context.Random.Next(140))
            {
                context.Append("Scenes.Doppelganger.MirrorAttack.TwistsDownwards".Localized(
                    "#ff0000", (int)Math.Round(damage)));

                HealthChange(-damage);
            }
            else
            {
                context.Append("Scenes.Doppelganger.MirrorAttack.YourWord".Localized());

                context.Append(context.Player.IsUnarmed()

                    ? "Scenes.Doppelganger.MirrorAttack.WeaponWord".Localized()

                    : "Scenes.Doppelganger.MirrorAttack.WeaponsPlural".Localized());

                context.Append("Scenes.Doppelganger.MirrorAttack.BoneJarringImpact".Localized());

                context.Append("Scenes.Doppelganger.MirrorAttack.TryAgainTaunt".Localized());
            }

            AddTalkShit(context);
        }

        public override void OnTeased(CombatContext context, float lustDelta)
        {
            context.WriteAction("Scenes.Doppelganger.MirrorTease.HandsSeductive".Localized());

            context.WriteAction("Scenes.Doppelganger.MirrorTease.DisplaysCarnal".Localized());

            context.Append("Scenes.Doppelganger.MirrorTease.ItsBeenSoLong".Localized());

            AddTalkShit(context);
        }

        public void AddTalkShit(CombatContext context)
        {
            switch (_roundCount)
            {
                case 0:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.YouFeelIt".Localized());

                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.DullCroon".Localized());

                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.FlutteringArm".Localized());

                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.FinishFastBanner".Localized());

                    break;

                case 1:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.CantRemember".Localized());

                    break;

                case 2:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.ClosesEyes".Localized());

                    if (context.Player.HasCock())
                        context.Append("Scenes.Doppelganger.AddTalkShit.StrokesCock".Localized());
                    else if (context.Player.HasVagina())
                        context.Append("Scenes.Doppelganger.AddTalkShit.FingersVagina".Localized());
                    else
                        context.Append("Scenes.Doppelganger.AddTalkShit.SlipsHand".Localized());

                    context.Append("Scenes.Doppelganger.AddTalkShit.BizarrenessSeen".Localized(ArmorName));

                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.HarderToIgnore".Localized());

                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.SomethingRealize".Localized());

                    break;

                case 3:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.MemoriesFlow".Localized());

                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.NotBadTaunt".Localized());

                    break;

                case 4:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.DidYouThink".Localized());

                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.DareNotOpen".Localized());

                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.LastGaspBanner".Localized());

                    break;

                case 5:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.ShimmerIntensifies".Localized());

                    context.Player.TakeLustDamage(1000, true);

                    break;

                default:
                    context.WriteAction("Scenes.Doppelganger.AddTalkShit.HowSurvive".Localized());

                    break;
            }

            _roundCount += 1;

            TookAction = true;
        }
    }
}