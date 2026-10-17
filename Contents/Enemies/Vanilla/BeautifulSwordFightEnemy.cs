using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<BeautifulSwordFightEnemy>("coc:beautiful_sword_fight")]
    public class BeautifulSwordFightEnemy : Enemy
    {

        public int _swordLevel;

        public static readonly Identifier AttackId = new("coc", "beautiful_sword_fight_attack");

        public static readonly Identifier EviscerateId = new("coc", "beautiful_sword_fight_eviscerate");

        public static readonly Identifier HolyLightId = new("coc", "beautiful_sword_fight_holy_light");

        public static readonly Identifier PurgeId = new("coc", "beautiful_sword_fight_purge");

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var choices = new List<(Identifier Id, float Weight)> { (AttackId, 2f) };

            if (_swordLevel >= 2)
                choices.Add((EviscerateId, 1f));

            if (_swordLevel >= 3)
                choices.Add((HolyLightId, 1f));

            if (HealthRatio() < 0.6f)
                choices.Add((PurgeId, 1f));

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

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            _swordLevel = CoC.Instance.Flags.Get(GameFlagIds.BeautifulSwordLevel);

            CreateBreastRow();

            Balls = 0;

            BallSize = 0;

            Tallness = 36;

            Body.Get<SkinPart>().Tone = "metallic";

            WeaponName = "blessed blade";

            WeaponVerb = "slash";

            WeaponAttack = 20 + _swordLevel * 5;

            ArmorName = "holy steel";

            ArmorDefense = 15 + _swordLevel;

            LustVulnerability = 0;

            Level = 17 + _swordLevel * 2;

            Gems = 0;

        }

        public override bool Defeated(bool hpVictory)
        {
            Scene.OutputText("Combat.BeautifulSwordFight.Defeated.WithAFinal".Localized());

            SceneInstance<BeautifulSwordScene>.Get().DestroyBeautifulSword();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<BeautifulSwordScene>.Get().DefeatedBySword();

            return true;
        }
    }
}