using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/CumWitchCombatHandlers.cs（已实现，120 行）
    [Register<CumWitchEnemy>("coc:cum_witch")]
    public class CumWitchEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(12, 2, BodyPartTypes.Human);

            Balls = 0;

            BallSize = 0;

            CreateVagina(false, 2, 3);

            Tallness = Random.Shared.Next(12) + 55;

            Body.Get<SkinPart>().Tone = "black";

            var hair = Body.Get<HairPart>();

            hair.Color = "sandy-blonde";

            hair.Length = 15;

            WeaponName = "fists";

            ArmorName = "robes";
        }

        public static readonly Identifier BukkakeId = new("coc", "cum_witch_bukkake_attack");

        public static readonly Identifier CumMagicId = new("coc", "cum_witch_cum_magic_attack");

        public static readonly Identifier CumHungerId = new("coc", "cum_witch_cum_hunger_attack");

        public static readonly Identifier GenderConfusionId = new("coc", "cum_witch_gender_confusion_attack");

        public static readonly Identifier ShellId = new("coc", "cum_witch_shell_defense");

        public static readonly Identifier CocknosisId = new("coc", "cum_witch_cocknosis_attack");

        public static readonly Identifier HealId = new("coc", "cum_witch_sand_witch_cunt_heals");

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (Health < MaxHealth * 0.6f)
                return new CombatAiAction { Ability = HealId };

            if (!HasStatusEffect(StatusEffectIds.Shell))
                return new CombatAiAction { Ability = ShellId };

            if (context.Player.Gender == Gender.Hermaphrodite)
                return new CombatAiAction { Ability = GenderConfusionId };

            if (context.Player.HasVagina())
                return new CombatAiAction { Ability = CumHungerId };

            if (context.Player.HasCock())
                return new CombatAiAction { Ability = CumMagicId };

            return new CombatAiAction { Ability = Random.Shared.Next(2) == 0 ? BukkakeId : CocknosisId };
        }

        public static bool LastHpVictory;

        public override bool Defeated(bool hpVictory)
        {
            LastHpVictory = hpVictory;

            CoC.Instance.Flags.Set(GameFlagIds.CumWitchDefeated, 1);

            CombatController.Instance?.Context.Append("Combat.Hel.CumWitchVictory".Localized());

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            var p = CoC.Instance.Player;

            if (Dungeons.DungeonSession.InDungeon)
            {
                var ctx = Dungeons.DungeonScene.ActiveContext;

                if (ctx != null)
                {
                    if (p.HasCock() && (p.Gender == Gender.Male || Random.Shared.Next(2) == 0))
                        Dungeons.DesertCaveRooms.CumWitchCumPumpBadEnd(ctx, false);
                    else
                        Dungeons.DesertCaveRooms.TurnIntoASammitch(ctx, false);

                    return true;
                }
            }

            if (p.HasCock() && (!p.HasVagina() || Random.Shared.Next(2) == 0))
            {
                if (Random.Shared.Next(2) == 0)
                    Dungeons.DesertCaveRooms.TDMsLoseToCumWitchScene();
                else
                    Dungeons.DesertCaveRooms.RepeatLoseToCumWitchForDudes();
            }
            else if (p.HasVagina())
                Dungeons.DesertCaveRooms.SavinMakesAwesomeFemdom();
            else
                Scenes.Scene.Go<Scenes.Vanilla.CampScene>();

            return true;
        }

    }
}