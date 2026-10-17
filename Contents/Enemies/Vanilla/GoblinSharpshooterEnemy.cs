using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<GoblinSharpshooterEnemy>("coc:goblin_sharpshooter")]
    public class GoblinSharpshooterEnemy : Enemy
    {

        public bool Ammo = true;

        public bool SmokeGrenade = true;

        public bool ItsADraw;

        public static readonly Identifier AttackId = new("coc", "goblin_sharpshooter_attack");

        public static readonly Identifier PushBackId = new("coc", "goblin_sharpshooter_push_back");

        public static readonly Identifier SkeetShotId = new("coc", "goblin_sharpshooter_skeet_shot");

        public static readonly Identifier ReloadId = new("coc", "goblin_sharpshooter_reload");

        public static readonly Identifier TakeAimId = new("coc", "goblin_sharpshooter_take_aim");

        public static readonly Identifier BuggerOutId = new("coc", "goblin_sharpshooter_bugger_out");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 3, 1);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 90);

            CreateBreastRow(BreastCups.E);

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 50);

            Tallness = 35 + Random.Shared.Next(4);

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();

            hair.Color = "blue";

            hair.Length = 7;

            WeaponName = "blunderbuss";

            ArmorName = "leather straps";

            NormalAttackAvoid = new AvoidDamageParameters
            {
                DoDodge = true,

                DoParry = false,

                DoBlock = false,

                DoCounter = false,

                DoFatigue = true
            };

            CreatePerk(PerkIds.Evade);

            AdditionalExperience = 350;

            Ammo = true;

            SmokeGrenade = true;

            ItsADraw = false;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<GoblinSharpshooterScene>.Get().DefeatSharpshooter();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Player.IsGenderless())
            {
                Scene.OutputText("Scenes.Goblinsharpshooter.Won.KickedUnconscious".Localized());

                CombatScene.CleanupAfterCombat();

                return true;
            }

            SceneInstance<GoblinWarriorScene>.Get().GobboWarriorBeatYaUp();

            return true;
        }

        public override int HandleCombatLossText(CombatContext context, bool inDungeon, int gemsLost)
        {
            if (ItsADraw)
            {
                context.Write("Scenes.Goblinsharpshooter.HandleCombatLossText.SighBackToCamp".Localized());

                return 1;
            }

            return base.HandleCombatLossText(context, inDungeon, gemsLost);
        }

        public override float ChanceToHit(Creature? defender = null) =>

            base.ChanceToHit(defender) + (HasStatusEffect(StatusEffectIds.TargetMarked) ? 30 : 0);

        public override float GetBaseCritChance() =>

            base.GetBaseCritChance() + (HasStatusEffect(StatusEffectIds.TargetMarked) ? 25 : 0);

        public override float ModifyOutgoingDamage(CombatContext context, float damage) =>

            HasStatusEffect(StatusEffectIds.TargetMarked) ? damage * 1.75f : damage;

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (Health < MaxHealth * 0.25f && SmokeGrenade)
                return new CombatAiAction { Ability = BuggerOutId };

            var choices = new List<(Identifier Ability, float Weight)>();

            if (Ammo)
            {
                choices.Add((AttackId, 3));

                if (Distance == CombatDistance.Melee)
                    choices.Add((PushBackId, 1));

                if (Distance == CombatDistance.Distant)
                    choices.Add((SkeetShotId, 1));
            }
            else
                choices.Add((ReloadId, 1));

            if (!HasStatusEffect(StatusEffectIds.TargetMarked))
                choices.Add((TakeAimId, 1));

            var total = 0f;

            foreach (var (_, weight) in choices)
                total += weight;

            if (total <= 0)
                return new CombatAiAction { Ability = AttackId };

            var roll = context.Random.NextDouble() * total;

            foreach (var (ability, weight) in choices)
            {
                roll -= weight;

                if (roll <= 0)
                    return new CombatAiAction { Ability = ability };
            }

            return new CombatAiAction { Ability = choices[^1].Ability };
        }

        public void TryToGrabGobbo(GoblinSharpshooterScene scene)
        {
            Scene.ClearOutput();

            var player = CoC.Instance.Player;

            Scene.OutputText("Scenes.Goblinsharpshooter.TryToGrabGobbo.DashIntoSmoke".Localized());

            if (Random.Shared.Next(Math.Max(1, (int)player.Speed)) > Random.Shared.Next(Math.Max(1, (int)Speed)))
            {
                Scene.OutputText("Scenes.Goblinsharpshooter.TryToGrabGobbo.GrabbedGoblin".Localized());

                Scene.OutputText("Scenes.Goblinsharpshooter.TryToGrabGobbo.DefeatedGoblin".Localized());

                ItsADraw = false;

                Health = 0;

                GoblinScene.GenerateGobboSexMenu();

                return;
            }

            Scene.OutputText("Scenes.Goblinsharpshooter.TryToGrabGobbo.GrabMisses".Localized());

            Scene.OutputText("Scenes.Goblinsharpshooter.TryToGrabGobbo.DirtiedChampion".Localized());

            CombatScene.CleanupAfterCombat();
        }
    }
}