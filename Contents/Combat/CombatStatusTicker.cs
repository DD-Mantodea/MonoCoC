using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoCoC.Contents.Combat
{
    public static class CombatStatusTicker
    {
        public static void Tick(CombatContext context)
        {
            foreach (var status in context.Player.StatusEffects.ToList())
            {
                var definition = status.Definition;

                if (definition?.OnRound is { } round)
                {
                    EffectHandlerRegistry.Invoke(round, context, context.Player, status);

                    if (!context.Player.StatusEffects.Contains(status))
                        continue;
                }
                else if (status.Duration <= 0)
                    continue;

                if (status.Duration <= 0)
                    continue;

                if (status.Id == StatusEffectIds.MothPheromones)
                {
                    context.Player.TakeLustDamage(5 + context.Random.Next(10));

                    context.Write("Common.Text.YourFaceFlushesAsAWave".Localized());
                }

                status.Duration -= 1;

                if (status.Duration <= 0)
                {
                    if (definition?.OnExpire is { } expire)
                        EffectHandlerRegistry.Invoke(expire, context, context.Player, status);
                    else

                        CombatStatusExpiryRegistry.Invoke(status.Id, context, context.Player);

                    context.Player.StatusEffects.Remove(status);
                }
            }

            foreach (var status in context.Enemy.StatusEffects.ToList())
            {
                if (status.Duration <= 0)
                    continue;

                var enemyDefinition = status.Definition;

                if (enemyDefinition?.OnRound is { } enemyRound)
                {
                    EffectHandlerRegistry.Invoke(enemyRound, context, context.Enemy, status);

                    if (!context.Enemy.StatusEffects.Contains(status))
                        continue;
                }

                if (status.Id.Path == "izma_bleed")
                {

                    var bleed = context.Enemy.BleedDamage();

                    context.Enemy.HealthChange(-bleed);

                    context.Write("Combat.Bleed.Enemy".Localized(context.Enemy.ShortName, bleed));
                }

                status.Duration -= 1;

                if (status.Duration <= 0)
                {
                    if (status.Id.Path == "izma_bleed")
                        context.Write("Combat.Bleed.EnemyStopped".Localized(context.Enemy.ShortName));

                    CombatStatusExpiryRegistry.Invoke(status.Id, context, context.Enemy);

                    context.Enemy.StatusEffects.Remove(status);
                }
            }
        }

        public static bool PreventsFlee(CombatContext context) =>

            context.Player.HasStatusEffect(StatusEffectIds.NoFlee);
    }

    public static class CombatStatusModifiers
    {
        public static float DamageTakenMultiplier(Creature host)
        {
            var multiplier = 1f;

            foreach (var status in host.StatusEffects)
                if (status.Id == StatusEffectIds.StimulatingAura)
                    multiplier *= (100f + status.Values[1]) / 100f;

            return multiplier;
        }
    }

    public static class CombatStatusHooks
    {

        public static bool BeforePlayerAbilityUse(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            foreach (var status in player.StatusEffects)
            {
                var definition = status.Definition;

                if (definition == null || !definition.Flags.HasFlag(EffectFlags.BlocksMagic))
                    continue;

                if (ability.Type != CombatAbilityType.Magical)
                    continue;

                var text = definition.BlockedTextKey.Length > 0

                    ? definition.BlockedTextKey.Localized()

                    : "Combat.Effect.BlocksMagic".Localized();

                context.WriteAction(text);

                return false;
            }

            if (player.HasStatusEffect(StatusEffectIds.ScorpBlind))
            {
                player.RemoveStatusEffect(StatusEffectIds.ScorpBlind);

                if (ability.Type != CombatAbilityType.Support && context.Random.Next(100) < 50)
                {
                    context.WriteAction("Combat.Scorpion.Blind.Wasted".Localized());

                    return false;
                }
            }

            if (player.HasStatusEffect(StatusEffectIds.ScorpVenom)
                && ability.Type == CombatAbilityType.Magical

                && context.Random.Next(100) < 90)
            {
                context.WriteAction("Combat.Scorpion.Venom.SpellFails".Localized());

                return false;
            }

            return true;
        }
    }

    public static class CombatStatusExpiryRegistry
    {
        static CombatStatusExpiryRegistry() => RegistrationScanner.OnMethods(RegisterMethod);

        public static readonly Dictionary<Identifier, Action<CombatContext, Creature>> Handlers = [];

        public static readonly Type Signature = typeof(Action<CombatContext, Creature>);

        public static void Register(Identifier id, Action<CombatContext, Creature> handler) => Handlers[id] = handler;

        public static bool Has(Identifier id) => Handlers.ContainsKey(id);

        public static void Invoke(Identifier id, CombatContext context, Creature host)
        {
            if (Handlers.TryGetValue(id, out var handler))
                handler(context, host);
        }

        internal static bool RegisterMethod(MethodInfo method, object[] attributes)
        {
            if (method.ReturnType != typeof(void))
                return false;

            var parameters = method.GetParameters();

            if (parameters.Length != 2
                || parameters[0].ParameterType != typeof(CombatContext)

                || parameters[1].ParameterType != typeof(Creature))

                return false;

            var handler = (Action<CombatContext, Creature>)method.CreateDelegate(Signature);

            foreach (RegisterAttribute attribute in attributes)
                Register(Identifier.Parse(attribute.Id), handler);

            return true;
        }
    }
}