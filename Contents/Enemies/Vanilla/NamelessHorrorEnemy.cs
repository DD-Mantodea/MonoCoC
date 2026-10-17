using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<NamelessHorrorEnemy>("coc:nameless_horror")]
    public class NamelessHorrorEnemy : Enemy
    {

        public float DoomLevel = 1;

        public string OriginalName = "";

        public float[] PlayerStats = [0f, 0f, 0f, 0f];

        public static readonly Identifier OverwhelmId = new("coc", "nameless_horror_overwhelm");

        public static readonly Identifier RefashionItId = new("coc", "nameless_horror_refashion_it");

        public static readonly Identifier RevealId = new("coc", "nameless_horror_reveal");

        public static readonly Identifier BecomeNothingnessId = new("coc", "nameless_horror_become_nothingness");

        public static readonly Identifier ConjoinId = new("coc", "nameless_horror_conjoin");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateBreastRow(BreastCups.A);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 1;

            Tallness = 180;

            Body.SetHipsRating("slender");

            Body.SetButtRating("average");

            var skin = Body.Get<SkinPart>();

            skin.Tone = "gray";

            skin.Id = BodyPartTypes.Plain;

            var hair = Body.Get<HairPart>();

            hair.Color = "white";

            hair.Length = 0;

            WeaponName = "tentacle";

            WeaponVerb = "lash";

            Fatigue = 0;

            WeaponAttack = 20;

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = "taut flesh";

            ArmorDefense = 17;

            LustVulnerability = 0;

        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {

            var ascensionMultiplier = CoC.Instance.Flags.Get(GameFlagIds.Ascensioning) == 1

                ? 1.10f * CoC.Instance.Flags.Get(GameFlagIds.NewGamePlusLevel)

                : 1f;

            if (MaxHealth - Health >= 3500f * ascensionMultiplier)
            {
                Banish();

                context.EndCombatRequested = true;

                return null;
            }

            var round = CombatController.Instance!.State.Round;

            if (round % 5 == 0 && round != 0)
                return new CombatAiAction { Ability = ConjoinId };

            var player = context.Player;

            var canRefashion = !player.HasStatusEffect(StatusEffectIds.Revelation)

                && !player.HasStatusEffect(StatusEffectIds.Refashioned);

            var choices = new List<(Identifier Id, float Weight, int Cost, FatigueType Fatigue, CombatRange Range)>
            {
                (OverwhelmId, 1f, 0, FatigueType.None, CombatRange.FlyingMelee)
            };

            if (canRefashion)
            {
                choices.Add((RefashionItId, 1f, 10, FatigueType.Magical, CombatRange.Omni));

                choices.Add((RevealId, 0.33f, 10, FatigueType.Magical, CombatRange.Omni));
            }

            if (!player.HasStatusEffect(StatusEffectIds.Nothingness))
                choices.Add((BecomeNothingnessId, 0.5f, 10, FatigueType.Magical, CombatRange.Omni));

            var eligible = choices
                .Where(c => context.Enemy.HasFatigue(c.Cost, c.Fatigue))
                .Where(c => CombatRangeData.CanReach(context.Enemy, player, context.Enemy.Distance, c.Range))
                .ToList();

            if (eligible.Count == 0)
                return new CombatAiAction { Ability = OverwhelmId, Cost = 0, Fatigue = FatigueType.None };

            var total = eligible.Sum(c => c.Weight);

            var roll = context.Random.NextDouble() * total;

            foreach (var choice in eligible)
            {
                roll -= choice.Weight;

                if (roll <= 0)
                    return new CombatAiAction { Ability = choice.Id, Cost = choice.Cost, Fatigue = choice.Fatigue };
            }

            var last = eligible[^1];

            return new CombatAiAction { Ability = last.Id, Cost = last.Cost, Fatigue = last.Fatigue };
        }

        public override float ModifyIncomingDamage(CombatContext context, float damage, CombatDamageKind kind)
        {
            if (!context.Player.HasStatusEffect(StatusEffectIds.Nothingness))
                return damage;

            if (kind == CombatDamageKind.Magical && context.Player.Intelligence >= 90)
            {
                context.Append("Combat.NamelessHorror.HandleDamaged.YouManageTo".Localized());

                return damage * 0.4f;
            }

            return 0;
        }

        public override bool HandleStun(CombatContext context)
        {
            if (context.Random.Next(3) == 0)
            {
                context.WriteAction("Combat.NamelessHorror.HandleStun.TheCyclopeanCreature".Localized());

                RemoveStatusEffect(StatusEffectIds.Stunned);

                return true;
            }

            return base.HandleStun(context);
        }

        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.NamelessHorror.HandleFear.TheCreatureShrieks".Localized());

            RemoveStatusEffect(StatusEffectIds.Fear);

            context.Player.RemoveStatusEffect(StatusEffectIds.Resolve);

            SceneInstance<ManorScene>.Get().TestResolve();

            return true;
        }

        public override int HandleCombatLossText(CombatContext context, bool inDungeon, int gemsLost) => 1;

        public void Banish(bool defeatedHorror = false) =>

            SceneInstance<ManorScene>.Get().Banish(defeatedHorror);

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().LoseToNamelessHorror();

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            var scene = SceneInstance<ManorScene>.Get();

            if (Configuration.OptionStore.Instance.Get("silly", 0) > 0)
            {
                Scene.ClearOutput();

                Scene.OutputText("Combat.NamelessHorror.Defeated.ThroughMiraculousPower".Localized());

                Scene.ClearButtons();

                Scene.AddButton(0, "Scenes.Dullahan.AskDullSex.ButtonBlowjob".Localized(), AreYouFuckingSerious,

                    "Charge in and let the Nameless Horror blowjob you.");

                Scene.AddButton(1, "Common.Button.Wait".Localized(), () => scene.Banish(true), "Better be cautious and wait.");

                return true;
            }

            scene.Banish(true);

            return true;
        }

        public static void AreYouFuckingSerious()
        {
            CoC.Instance.Flags.Set(GameFlagIds.NhBj, 1);

            Scene.ClearOutput();

            if (!Scene.Player.HasCock())
                Scene.OutputText("Combat.NamelessHorror.AreYouFuckingSerious.YouSuddenlySprout".Localized());

            Scene.OutputText("Combat.NamelessHorror.AreYouFuckingSerious.ShatteredAndBroken".Localized());

            Scene.OutputText("Combat.NamelessHorror.AreYouFuckingSerious.YouApproachThe".Localized());

            Scene.OutputText("Combat.NamelessHorror.AreYouFuckingSerious.FamiliarScreechingRings".Localized());

            Scene.OutputText("Combat.NamelessHorror.AreYouFuckingSerious.OnwardStillThe".Localized());

            Scene.AwardAchievement(Achievements.AchievementIds.LordBritishPostulate, true, true);

            Scene.DoNext(() => SceneInstance<ManorScene>.Get().Banish(true));
        }
    }
}
