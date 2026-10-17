using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<JeanClaudeEnemy>("coc:jean_claude")]
    public class JeanClaudeEnemy : Enemy
    {

        public static readonly Identifier RemovedArmorId = new("coc", "removed_armor");

        public static readonly Identifier JcLustLevelId = new("coc", "jc_lust_level");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            Race = "Basilisk";

            CreateCock(12, 2, BodyPartTypes.Lizard);

            Balls = 2;

            BallSize = 6;

            HoursSinceCum = 9999;

            CreateBreastRow(0);

            Tallness = 86;

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;

            anal.Wetness = 0;

            Body.Hips = 4;

            Body.Butt = 4;

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.Lizard;

            Body.Get<TailPart>().Id = BodyPartTypes.Lizard;

            Body.Get<SkinPart>().DescKey = "Npcs.JeanClaude.SkinDesc";

            Strength = 80;

            Toughness = 100;

            Speed = 80;

            Intelligence = 60;

            Libido = 40;

            Sensitivity = 40;

            Corruption = 80;

            Body.Get<FacePart>().Id = CoC.Instance.Player.HasFur()

                ? BodyPartTypes.Lizard

                : BodyPartTypes.Human;

            Body.Get<EarsPart>().Id = BodyPartTypes.Lizard;

            Body.Get<EyesPart>().Id = BodyPartTypes.Basilisk;

            WeaponName = "cutlass";

            WeaponVerb = "slash";

            WeaponAttack = 20;

            ArmorName = "leather tunic";

            ArmorDefense = 20;

            var bonusHealth = 400f;

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            Lust = 5;

            LustVulnerability = 0.6f;

            Level = 20;

            Gems = Random.Shared.Next(55) + 300;

            CreatePerk(PerkIds.BasiliskResistance);

            CreatePerk(PerkIds.BlindImmune);
        }

        public override bool ShouldMove(CombatDistance newPos, bool forceAction = false) => false;

        public override void OnTeased(CombatContext context, float lustDelta)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(RemovedArmorId) && player.Armor != Armor.Nothing)
            {
                context.Append("Combat.JeanClaude.HandleTease.JeanClaudeStops".Localized());

                context.Append("Combat.JeanClaude.HandleTease.HeHusksHis".Localized());

                context.Append("Combat.JeanClaude.HandleTease.YourIntentIs".Localized());

                context.Append("Combat.JeanClaude.HandleTease.JeanClaudeBreathes".Localized());

                player.CreateStatusEffect(RemovedArmorId);

                if (!HasStatusEffect(JcLustLevelId))
                {
                    CreateStatusEffect(JcLustLevelId, 1, 0, 0, 0);

                    LustVulnerability += 0.1f;
                }
                else
                    ChangeStatusValue(JcLustLevelId, 0, 1);

                player.TakeLustDamage(20, true);
            }
            else
            {
                context.Append("Combat.JeanClaude.HandleTease.JeanClaudeSighs".Localized());

                player.TakeLustDamage(20, true);
            }
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            var player = context.Player;

            switch (reaction)
            {
                case CombatReactionContext.AfterAttacked:
                    if (!player.HasStatusEffect(StatusEffectIds.FirstAttack) &&
                        player.StatusEffectValue(StatusEffectIds.CounterAB, 0) != 1)
                    {
                        if (Health < 1 || Lust >= MaxLust)
                            return true;

                        if (player.Lust <= 30)
                        {
                            context.Append("Combat.JeanClaude.React.JeanClaudeDoesn".Localized());

                            context.Append("Combat.JeanClaude.React.HeSaysThe".Localized());
                        }
                        else if (player.Lust <= 50)
                        {
                            context.Append("Combat.JeanClaude.React.AgainYourThumps".Localized());

                            context.Append("Combat.JeanClaude.React.HeSaysHe".Localized());
                        }
                        else if (player.Lust <= 80)
                            context.Append("Combat.JeanClaude.React.GrowlsJeanClaude".Localized());
                        else
                            context.Append("Combat.JeanClaude.React.YouCanT".Localized());

                        player.TakeLustDamage(25, true);

                        return true;
                    }

                    return true;

                case CombatReactionContext.Blinded:
                    context.Append("Combat.JeanClaude.React.JeanClaudeHowls".Localized());

                    context.Append("Combat.JeanClaude.React.HeSnarlsThe".Localized());

                    if (context.Random.Next(Math.Max(1, (int)player.Speed)) >= 50 ||
                        context.Random.Next(Math.Max(1, (int)player.Intelligence)) >= 50)
                    {
                        context.Append("Combat.JeanClaude.React.TheLightSears".Localized());

                        context.Append("Combat.JeanClaude.React.TheMonsterSnaps".Localized());

                        var damage = (int)(10 +

                            (player.Intelligence / 3f +

                             context.Random.Next(Math.Max(1, (int)(player.Intelligence / 2)))) *

                            player.SpellMod());

                        context.DealDamageToPlayer(damage);
                    }
                    else
                    {
                        context.Append("Combat.JeanClaude.React.TheLightSears2".Localized());

                        context.Append("Combat.JeanClaude.React.Text".Localized());

                        player.CreateStatusEffect(StatusEffectIds.Blind,

                            context.Random.Next(4) + 1, 0, 0, 0);
                    }

                    return false;

                default:
                    return true;
            }
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Dungeons.LethicesKeep.JeanClaudeScenes>.Get()
                .BeatDaFuckhugeBasilisk(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Dungeons.LethicesKeep.JeanClaudeScenes>.Get()
                .FuckhugeBasiliskFuckedYouUp(hpVictory);

            return true;
        }
    }
}
