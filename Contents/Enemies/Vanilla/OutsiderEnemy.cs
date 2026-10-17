using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Places.MothCave;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<OutsiderEnemy>("coc:outsider")]
    public class OutsiderEnemy : Enemy
    {
        public int _turnsPassed;

        public int _timesWaited;

        public bool _attacked;

        public int _damage;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1; 

            anal.Wetness = 1;   

            Tallness = 60;

            Body.SetHipsRating("slender");

            Body.SetButtRating("average");

            WeaponName = "strange tendril";

            WeaponVerb = "strikes";

            Fatigue = 0;

            WeaponAttack = 0;

            ArmorName = "inky mist";

            ArmorDefense = 99;

            Lust = 0;

            LustVulnerability = 0.01f;

            Level = 99;

            CreatePerk(PerkIds.BleedImmune);

            CreateStatusEffect(StatusEffectIds.GenericRunDisabled, 0, 0, 0, 0);

            AdditionalExperience = 0;
        }

        public override string Description =>

            (Configuration.OptionStore.Instance.Get("metric", 0) > 0

                ? "Npcs.Outsider.Desc.Metric"

                : "Npcs.Outsider.Desc").Localized();

        public override bool BeforeCombatAction(CombatContext context)
        {
            TookAction = true;

            _turnsPassed += 1;

            if (_turnsPassed == 3 && context.Player.Health > 0)
                Health = 0;

            return true;
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction == CombatReactionContext.PlayerWaited)
            {

                switch (++_timesWaited)
                {
                    case 1:
                        context.WriteAction("Combat.Outsider.Wait.First".Localized());

                        break;

                    case 2:
                        context.WriteAction("Combat.Outsider.Wait.Second".Localized());

                        break;

                    case 3:
                        context.WriteAction("Combat.Outsider.Wait.Third".Localized());

                        break;
                }

                return false;
            }

            if (reaction == CombatReactionContext.BeforeAttacked)
            {
                if (_attacked)
                {
                    context.WriteAction("Combat.Outsider.React.Tendril".Localized());

                    context.Player.TakeDamage(_damage);
                }
                else
                {
                    context.WriteAction("Combat.Outsider.React.NoResistance".Localized());

                    _damage = (int)(context.Player.Health * 0.55f);

                    _attacked = true;
                }

                return false;
            }

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<DoloresScene>.Get().DoloresSummoningWin();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<DoloresScene>.Get().DoloresSummoningLose();

            return true;
        }
    }
}