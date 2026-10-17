using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Combat
{
    public enum CombatRange
    {
        Melee,

        Ranged,

        Self,

        Tease,

        Omni,

        FlyingMelee,

        ChargingMelee
    }

    public enum CombatAbilityType
    {
        Physical,

        Magical,

        Tease,

        Support
    }

    public enum FatigueType
    {
        None,

        Magical,

        Physical,

        MagicalHeal
    }

    public enum CombatAbilityCategory
    {
        Enemy,

        WhiteMagic,

        BlackMagic,

        GrayMagic,

        Physical,

        Magical
    }

    public class CombatAbilityDefinition : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string NameKey = "";

        public string DescriptionKey = "";

        public CombatAbilityType Type;

        public CombatAbilityCategory Category;

        public CombatRange Range;

        public int Cost;

        public FatigueType Fatigue;

        public int Cooldown;

        public float Power;

        public Identifier Handler;

        public Identifier DamageType;

        public Identifier KnownWhen;

        public int DominikaOrder;

        public Identifier AvailableWhen;

        public Identifier DisabledWhen;

        public string DisabledTooltip = "";

        public bool IsSelf;

        public bool IsHeal;
    }

    public class CombatAiAction
    {
        public Identifier Ability;

        public float Weight = 1;

        public int? Cost;

        public FatigueType? Fatigue;

        public Identifier? When;

        public Identifier? RequiresStatus;

        public Identifier? ForbidStatus;

        public int Priority;
    }

    public class CombatAiDefinition : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string NameKey = "";

        public List<CombatAiAction> Actions = [];
    }

    public static class CombatAbilityRegistry
    {
        public static Registry<CombatAbilityDefinition> Abilities =>

            ManagerInstance<CombatAbilityManager>.Instance.Registry;

        public static CombatAbilityDefinition? Get(Identifier id) =>

            Abilities.TryGet(id, out var ability) ? ability : null;
    }

    public static class CombatAiRegistry
    {
        public static Registry<CombatAiDefinition> Ais =>

            ManagerInstance<CombatAiManager>.Instance.Registry;

        public static CombatAiDefinition? Get(Identifier id) =>

            Ais.TryGet(id, out var ai) ? ai : null;
    }

    public static class CombatConditionRegistry
    {

        static CombatConditionRegistry() => Registry.RegistrationScanner.OnMethods(RegisterMethod);

        public static readonly Dictionary<Identifier, System.Func<CombatContext, CombatState, bool>> Conditions = [];

        public static readonly System.Type ConditionSignature = typeof(System.Func<CombatContext, CombatState, bool>);

        public static void Register(
            Identifier id,

            System.Func<CombatContext, CombatState, bool> condition) =>

            Conditions[id] = condition;

        public static bool IsMet(Identifier id, CombatContext context, CombatState state) =>

            Conditions.TryGetValue(id, out var condition) && condition(context, state);

        public static bool IsRegistered => true;

        public static bool Has(Identifier id) => Conditions.ContainsKey(id);

        internal static bool RegisterMethod(System.Reflection.MethodInfo method, object[] attributes)
        {
            if (method.ReturnType != typeof(bool))
                return false;

            var parameters = method.GetParameters();

            if (parameters.Length != 2
                || parameters[0].ParameterType != typeof(CombatContext)

                || parameters[1].ParameterType != typeof(CombatState))

                return false;

            var condition = (System.Func<CombatContext, CombatState, bool>)method.CreateDelegate(ConditionSignature);

            foreach (Attributes.RegisterAttribute attribute in attributes)
                Register(Identifier.Parse(attribute.Id), condition);

            return true;
        }
    }
}