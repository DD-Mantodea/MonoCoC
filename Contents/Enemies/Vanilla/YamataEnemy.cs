using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Combat.Handlers;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<YamataEnemy>("coc:yamata")]
    public class YamataEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 2);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 8000);

            CreateBreastRow(6f);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 2;

            anal.Wetness = 2;

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 200);

            Tallness = 69;

            Body.SetHipsRating("ample");

            Body.SetButtRating("average", 1);

            Body.Get<SkinPart>().Tone = "light tan";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 22;

            WeaponName = "Muramasa";

            WeaponVerb = "slash";

            ArmorName = "tight chest wrap and baggy pants";

            ArmorDefense = 16;

            LustVulnerability = 0.35f;

            Gems = 30 + Random.Shared.Next(20);

            var tail = Body.Get<TailPart>();

            tail.Id = BodyPartTypes.Fox;

            tail.Venom = 9;

            CreatePerk(PerkIds.EnlightenedNinetails);

            CreatePerk(PerkIds.SpellcastingAffinity, 25);
        }

        public override bool HandleFear(CombatContext context)
        {
            RemoveStatusEffect(StatusEffectIds.Fear);

            context.WriteAction("Combat.Yamata.HandleFear".Localized());

            return true;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            if (!HasStatusEffect(StatusEffectIds.Blind))
                return false;

            RemoveStatusEffect(StatusEffectIds.Blind);

            context.WriteAction("Combat.Yamata.HandleBlind".Localized());

            return true;
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.PlayerWaited)
                return true;

            if (!context.Player.HasStatusEffect(StatusEffectIds.YamataEntwine))
                return true;

            context.WriteAction("Combat.Yamata.EntwineWait".Localized());

            context.Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CoC.Instance.Flags.Set(Flags.GameFlagIds.YamataMasochist, CoC.Instance.Flags.Get(Flags.GameFlagIds.YamataMasochist) + 2);

            TookAction = true;

            return false;
        }

        public override void Struggle(CombatContext context)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(StatusEffectIds.YamataEntwine))
                return;

            context.WriteAction("Combat.Yamata.Struggle.One".Localized());

            var brokeFree = context.Random.Next((int)Math.Max(1, player.Strength)) > (Strength / 4f) * 3f ||

                context.Random.Next(5) == 0;

            if (brokeFree)
            {
                context.WriteAction("Combat.Yamata.Struggle.Escape".Localized());

                player.RemoveStatusEffect(StatusEffectIds.YamataEntwine);

                context.WriteAction("Combat.Yamata.Struggle.EscapeDone".Localized());
            }
            else
            {
                context.WriteAction("Combat.Yamata.Struggle.Two".Localized());

                context.WriteAction(player.Strength < 90

                    ? "Combat.Yamata.Struggle.Immense".Localized()

                    : "Combat.Yamata.Struggle.Impressive".Localized());

                context.WriteAction("Combat.Yamata.Struggle.Three".Localized());

                var damage = 25 + context.Random.Next(15);

                player.HealthChange(-damage);

                YamataCombatHandlers.SadomasochistApply(context, damage);

                player.Teased(context.Random.Next(6) + 10);

                player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
            }

            TookAction = true;
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.AikoScene>.Get().YamataLoses();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.AikoScene>.Get().YamataWins();

            return true;
        }
    }
}