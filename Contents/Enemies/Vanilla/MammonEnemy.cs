using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<MammonEnemy>("coc:mammon")]
    public class MammonEnemy : Enemy
    {

        public bool HasSwitched;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateBreastRow(BreastCups.D);

            CreateVagina(false, 1, 3);

            Tallness = 5 * 12 + 7;

            LustVulnerability = 0.75f;

            WeaponName = "spectral sword";

            WeaponVerb = "slash";

            WeaponAttack = 0;

            ArmorName = "";

            ArmorDefense = 0;

            IgnoreLust = true;

            AdditionalExperience = 3200;

            CreatePerk(PerkIds.Immovable);

            CreatePerk(PerkIds.Archmage);

            CreatePerk(PerkIds.Mage);

            CreatePerk(PerkIds.Spellpower);

            CreatePerk(PerkIds.MysticLearnings);

        }

        public override void OnTeased(CombatContext context, float lustDelta)
        {
            var capitalA = Utils.Utils.CapitalizeFirstLetter(Article);

            if (lustDelta == 0)
                context.WriteAction("Combat.Mammon.OutputDefaultTeaseReaction.OneDoesn".Localized(
                    capitalA, ShortName));

            context.WriteAction("Combat.Mammon.OutputDefaultTeaseReaction.OneShows".Localized(
                capitalA, ShortName));
        }

        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.Mammon.HandleFear.Text".Localized());

            RemoveStatusEffect(StatusEffectIds.Fear);

            return true;
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            switch (reaction)
            {
                case CombatReactionContext.TurnStart:
                    if (!HasSwitched)
                        HasSwitched = true;

                    break;
            }

            return true;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            HasSwitched = false;

            if (CombatController.Instance!.State.Round == 3)
            {
                Rewind(context);

                return true;
            }

            CombatController.Instance.Resolver.EnemyBasicAttack();

            return true;
        }

        public void Rewind(CombatContext context)
        {
            context.Append("Combat.Mammon.Rewind.MammonBreathesDeeply".Localized());

            context.Append("Combat.Mammon.Rewind.ThenYouAlso".Localized());

            context.EndOfRoundOverride = () => Rewind2(context);
        }

        public void Rewind2(CombatContext context)
        {
            context.Clear();

            var controller = CombatController.Instance!;

            var fresh = EnemyRegistry.Create(new Identifier("coc", "mammon"));

            context.ResetFor([fresh]);

            controller.State.Round = 0;

            controller.State.Outcome = null;

            context.EndOfRoundOverride = null;

            controller.Start();
        }

    }
}