using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Combat
{
    public class CombatAiInterpreter
    {
        public CombatContext Context = null!;

        public CombatState State = null!;

        public virtual CombatAiAction? Choose()
        {
            if (Context.Enemy.AiId is not { } aiId)
                return null;

            var definition = CombatAiRegistry.Get(aiId);

            if (definition == null)
                return null;

            var eligible = new List<(CombatAiAction Action, CombatAbilityDefinition Ability)>();

            foreach (var action in definition.Actions)
            {
                var ability = CombatAbilityRegistry.Get(action.Ability);

                if (ability == null)
                    continue;

                if (action.RequiresStatus is { } required &&
                    !Context.Enemy.HasStatusEffect(required))

                    continue;

                if (action.ForbidStatus is { } forbidden &&
                    Context.Enemy.HasStatusEffect(forbidden))

                    continue;

                if (State.IsOnCooldown(ability.Id))
                    continue;

                if (action.When is { } condition &&
                    !CombatConditionRegistry.IsMet(condition, Context, State))

                    continue;

                var cost = action.Cost ?? ability.Cost;

                var fatigue = action.Fatigue ?? ability.Fatigue;

                if (cost > 0 && !Context.Enemy.HasFatigue(cost, fatigue))
                    continue;

                if (!CombatRangeData.CanReach(Context.Enemy, Context.Player, Context.Enemy.Distance, ability.Range))
                    continue;

                eligible.Add((action, ability));
            }

            if (eligible.Count == 0)
                return null;

            var priority = eligible.Max(pair => pair.Action.Priority);

            var top = eligible.Where(pair => pair.Action.Priority == priority).ToList();

            var total = top.Sum(pair => pair.Action.Weight);

            if (total <= 0)
                return top[0].Action;

            var roll = Context.Random.NextDouble() * total;

            foreach (var (Action, Ability) in top)
            {
                roll -= Action.Weight;

                if (roll <= 0)
                    return Action;
            }

            return top[^1].Action;
        }

        public virtual bool Execute(CombatAiAction action)
        {
            var ability = CombatAbilityRegistry.Get(action.Ability);

            if (ability == null)
                return false;

            Context.Enemy.ChangeFatigue(action.Cost ?? ability.Cost, action.Fatigue ?? ability.Fatigue);

            State.SetCooldown(ability.Id, ability.Cooldown);

            return true;
        }
    }
}