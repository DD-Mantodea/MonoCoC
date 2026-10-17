using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.Handlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<LethiceEnemy>("coc:lethice")]
    public class LethiceEnemy : Enemy
    {

        public int RoundCount;

        public int FightPhase = 1;

        public bool WingsDestroyed;

        public int DefMode;

        public int HpLastTurn;

        public int LustLastTurn;

        public enum Phase2MenuState
        {
            None,

            Top,

            DemonFuckSub,

            Phase3Next
        }

        public Phase2MenuState Phase2Menu;

        public bool P2VictoryWasHp;

        public static readonly Identifier LethicesRapeTentaclesId = new("coc", "lethices_rape_tentacles");

        public static readonly Identifier PigbysHandsId = new("coc", "pigbys_hands");

        public static readonly Identifier WhipSilenceId = new("coc", "whip_silence");

        public static readonly Identifier MinotaurKingsTouchId = new("coc", "minotaur_kings_touch");

        public static readonly Identifier OnFireId = new("coc", "on_fire");

        public static readonly Identifier PermaFlyStatusId = new("coc", "perma_fly_status");

        public static readonly Identifier DemonicArouseId = new("coc", "lethice_demonic_arouse");

        public static readonly Identifier DemonfireId = new("coc", "lethice_demonfire");

        public static readonly Identifier RapetaclesId = new("coc", "lethice_rapetacles");

        public static readonly Identifier WingbuffetId = new("coc", "lethice_wingbuffet");

        public static readonly Identifier ClawId = new("coc", "lethice_claw");

        public static readonly Identifier DemonLustMagicId = new("coc", "lethice_demon_lust_magic");

        public static readonly Identifier DirtyDancingId = new("coc", "lethice_dirty_dancing");

        public static readonly Identifier CrushingBodiesId = new("coc", "lethice_crushing_bodies");

        public static readonly Identifier HornyPokeId = new("coc", "lethice_horny_poke");

        public static readonly Identifier BukkakeTimeId = new("coc", "lethice_bukkake_time");

        public static readonly Identifier ParasiteThrowingStarsId = new("coc", "lethice_parasite_throwing_stars");

        public static readonly Identifier WhiptripId = new("coc", "lethice_whiptrip");

        public static readonly Identifier SonicwhipId = new("coc", "lethice_sonicwhip");

    public static readonly Identifier TriplestrokeId = new("coc", "lethice_triplestroke");

        public static readonly Identifier WhipchokeId = new("coc", "lethice_whipchoke");

        public static readonly Identifier GropehandsId = new("coc", "lethice_gropehands");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            Race = "Demon";

            Tallness = 108;

            CreateVagina(false, 3, 3);

            CreateBreastRow(3);

            Body.Hips = 2;

            Body.Butt = 2;

            Strength = 110;

            Toughness = 110;

            Speed = 110;

            Intelligence = 110;

            Libido = 100;

            Sensitivity = 40;

            Corruption = 100;

            WeaponName = "whip";

            WeaponAttack = 35;

            WeaponVerb = "whip";

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = "wraps";

            ArmorDefense = 15;

            if (CoC.Instance.Player.NewGamePlusMod() >= 3)
                ArmorDefense += 30;

            var bonusHealth = 650f;

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            AdditionalExperience = 1000;

            Gems = Random.Shared.Next(50) + 500;

            Level = 25;

            LustVulnerability = 0.15f;

            CreatePerk(PerkIds.Tank);

            CreatePerk(PerkIds.Tank2);

            CreatePerk(PerkIds.ImprovedSelfControl);

            CreatePerk(PerkIds.SpellcastingAffinity, 30);

            Body.Get<WingsPart>().Id = BodyPartTypes.DraconicLarge;
        }

        public override string Description
        {
            get
            {
                var player = CoC.Instance.Player;

                string text;

                if (FightPhase == 1)
                    text = "Npcs.Lethice.Desc".Localized();
                else if (FightPhase == 2)
                {
                    text = "Npcs.Lethice.Desc.Phase2".Localized();

                    if (HasStatusEffect(StatusEffectIds.Blind))
                        text += "Npcs.Lethice.Desc.Phase2Blind".Localized();
                    else if (HasStatusEffect(OnFireId))
                        text += "Npcs.Lethice.Desc.Phase2OnFire".Localized();
                }
                else
                {
                    text = "Npcs.Lethice.Desc.Phase3Lead".Localized();

                    if (WingsDestroyed)
                        text += "Npcs.Lethice.Desc.Phase3Wings".Localized();

                    text += "Npcs.Lethice.Desc.Phase3Rest".Localized();

                    if (FightPhase == 3 && DefMode != 0)
                        text += DefMode == 1

                            ? "Npcs.Lethice.Desc.DefMode1".Localized()

                            : "Npcs.Lethice.Desc.DefMode2".Localized();
                }

                if (player.HasStatusEffect(LethicesRapeTentaclesId))
                {
                    text += "Npcs.Lethice.Desc.Tentacles".Localized();

                    if (player.StatusEffectValue(LethicesRapeTentaclesId, 2) != 0)
                        text += "Npcs.Lethice.Desc.TentaclesGrabbed".Localized();
                    else if (player.HasStatusEffect(StatusEffectIds.KnowsWhitefire))
                        text += player.CanFly()

                            ? "Npcs.Lethice.Desc.TentaclesFlying".Localized()

                            : "Npcs.Lethice.Desc.TentaclesQuick".Localized();
                }

                if (HasStatusEffect(StatusEffectIds.Shell))
                    text += "Npcs.Lethice.Desc.Shell".Localized();

                if (player.HasStatusEffect(PigbysHandsId))
                    text += "Npcs.Lethice.Desc.PigbysHands".Localized();

                return text;
            }
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            RoundCount += 1;

            if (context.Player.HasStatusEffect(LethicesRapeTentaclesId))
                LethiceCombatHandlers.RapetacleTurn(context);

            if (FightPhase == 3)
                LethiceCombatHandlers.UpdatePhase3Stats(context);

            return false;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter) =>

            FightPhase switch
            {
                2 => Phase2Action(context),

                3 => Phase3Action(context),

                _ => Phase1Action(context)
            };

        public CombatAiAction Phase1Action(CombatContext context)
        {
            var options = new List<Identifier> { DemonicArouseId, DemonfireId, ClawId };

            if (!context.Player.HasStatusEffect(StatusEffectIds.Blind))
                options.Add(WingbuffetId);

            if (RoundCount % 10 == 3)
                options.Add(RapetaclesId);

            return new CombatAiAction { Ability = options[context.Random.Next(options.Count)] };
        }

        public CombatAiAction Phase2Action(CombatContext context)
        {
            var options = new List<Identifier>
            {
                DemonLustMagicId, DirtyDancingId, CrushingBodiesId, HornyPokeId
            };

            if (context.Random.Next(10) == 0 && !context.Player.HasStatusEffect(StatusEffectIds.Blind))
                options.Add(BukkakeTimeId);

            return new CombatAiAction { Ability = options[context.Random.Next(options.Count)] };
        }

        public CombatAiAction Phase3Action(CombatContext context)
        {
            if (RoundCount == 5)
                return new CombatAiAction { Ability = GropehandsId };

            var options = new List<Identifier>
            {
                ParasiteThrowingStarsId, WhiptripId, SonicwhipId, TriplestrokeId
            };

            if (!context.Player.HasStatusEffect(WhipSilenceId))
                options.Add(WhipchokeId);

            return new CombatAiAction { Ability = options[context.Random.Next(options.Count)] };
        }

        public override void Struggle(CombatContext context) => LethiceCombatHandlers.Struggle(context);

        public override bool CustomCombatMenu(CombatController controller, CombatContext context)
        {
            switch (Phase2Menu)
            {
                case Phase2MenuState.Top:

                    if (context.Player.HasCock() || context.Player.HasVagina())
                        context.AddButton(0, "Common.Button.Demonfuck".Localized(), () =>
                        {
                            Phase2Menu = Phase2MenuState.None;

                            LethiceCombatHandlers.P2DemonFuck(context, controller);
                        });

                    if (context.Player.HasStatusEffect(StatusEffectIds.KnowsHeal))
                        context.AddButton(1, "Common.Button.Heal".Localized(), () =>
                        {
                            Phase2Menu = Phase2MenuState.None;

                            LethiceCombatHandlers.P2Heal(context, controller);
                        });

                    context.AddButton(2, "Common.Button.Next".Localized(), () =>
                    {
                        Phase2Menu = Phase2MenuState.None;

                        LethiceCombatHandlers.P2Next(context, controller);
                    });

                    return true;

                case Phase2MenuState.DemonFuckSub:

                    context.AddButton(0, "Common.Button.Oralfinish".Localized(), () =>
                    {
                        Phase2Menu = Phase2MenuState.None;

                        LethiceCombatHandlers.OralFinish(context, controller);
                    });

                    if (context.Player.HasCock())
                        context.AddButton(1, "Common.Button.Fuckdemon".Localized(), () =>
                        {
                            Phase2Menu = Phase2MenuState.None;

                            LethiceCombatHandlers.FuckDemon(context, controller);
                        });

                    context.AddButton(2, "Common.Button.RideCock".Localized(), () =>
                    {
                        Phase2Menu = Phase2MenuState.None;

                        LethiceCombatHandlers.RideCock(context, controller);
                    });

                    return true;

                case Phase2MenuState.Phase3Next:

                    context.AddButton(0, "Common.Button.Next".Localized(), () =>
                    {
                        Phase2Menu = Phase2MenuState.None;

                        LethiceCombatHandlers.P2Next(context, controller);
                    });

                    return true;

                default:
                    return false;
            }
        }

        public override DefeatResult ResolveDefeated(CombatContext context, bool hpVictory)
        {
            if (FightPhase == 1)
            {
                LethiceCombatHandlers.Phase1Ends(context, hpVictory);

                return DefeatResult.Continue;
            }

            if (FightPhase == 2)
            {
                LethiceCombatHandlers.Phase2Ends(context, hpVictory);

                return DefeatResult.Continue;
            }

            return DefeatResult.Ended;
        }

        public override bool Defeated(bool hpVictory)
        {
            if (FightPhase != 3)
                return false;

            SceneInstance<LethiceScenes>.Get().Defeated(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<LethiceScenes>.Get().Won(hpVictory);

            return true;
        }
    }
}