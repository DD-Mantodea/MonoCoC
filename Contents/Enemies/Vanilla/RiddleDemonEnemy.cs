using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Places.MothCave;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<RiddleDemonEnemy>("coc:riddle_demon")]
    public class RiddleDemonEnemy : Enemy
    {

        public int _whisperCooldown;

        public bool DoloresFree;

        public void SetWhisperCooldown(int value) => _whisperCooldown = value;

        public static readonly Identifier SlashId = new("coc", "riddle_demon_slash");

        public static readonly Identifier FeintId = new("coc", "riddle_demon_feint");

        public static readonly Identifier WhisperId = new("coc", "riddle_demon_whisper");

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var whisperReady = _whisperCooldown-- <= 0;

            var choices = new List<(Identifier Id, float Weight)>
            {
                (SlashId, 3f),

                (FeintId, 1f)
            };

            if (whisperReady)
                choices.Add((WhisperId, 1f));

            var total = 0f;

            foreach (var (_, weight) in choices)
                total += weight;

            var roll = (float)context.Random.NextDouble() * total;

            foreach (var (id, weight) in choices)
            {
                roll -= weight;

                if (roll < 0)
                    return new CombatAiAction { Ability = id };
            }

            return new CombatAiAction { Ability = choices[^1].Id };
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.TurnStart)
                return true;

            if (context.Random.Next(100) < 10)
            {
                context.Append("Combat.RiddleDemon.React.TheDemonMakes".Localized());

                TookAction = true;
            }
            else if (DoloresFree)
                if (context.Random.Next(100) < 10 && context.Player.Health < context.Player.MaxHealth)
                {
                    context.Append("Combat.RiddleDemon.React.ASlightMurmur".Localized());

                    context.Player.HealthChange(context.Player.MaxHealth * 0.1f);
                }
                else if (context.Random.Next(100) < 10)
                {
                    context.Append("Combat.RiddleDemon.React.TheDemonIs".Localized());

                    HealthChange(-(50 + context.Random.Next(50)));
                }

            return true;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(10, 1.5f, BodyPartTypes.Demon);

            Balls = 2;

            BallSize = 3;

            Tallness = 78;

            CreateBreastRow();

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 1;

            Body.Get<SkinPart>().Tone = "pale";

            Body.Get<HairPart>().Color = "gray";

            Body.SetHipsRating("slender");

            Body.SetButtRating("average");

            WeaponName = "nails";

            WeaponVerb = "slash";

            WeaponAttack = 30;

            ArmorName = "shabby clothing";

            ArmorDefense = 10;

            LustVulnerability = 0.4f;

            if (!HasStatusEffect(StatusEffectIds.GenericRunDisabled))
                CreateStatusEffect(StatusEffectIds.GenericRunDisabled);

            if (!HasPerk(PerkIds.StunImmune))
                CreatePerk(PerkIds.StunImmune);

            if (!HasPerk(PerkIds.SpeedyRecovery))
                CreatePerk(PerkIds.SpeedyRecovery);

            AdditionalExperience = 200;

        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<DoloresScene>.Get().HikkiFightWin();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<DoloresScene>.Get().HikkiFightLoss();

            return true;
        }
    }
}