using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.StatusEffects
{

    [Flags]
    public enum EffectFlags
    {
        None = 0,

        BlocksMagic = 1 << 0,

        BlocksFlee = 1 << 1,

        PreventsAction = 1 << 2,

        PreventsTease = 1 << 3,

        PreventsPhysical = 1 << 4
    }

    public enum EffectKind
    {
        Combat,

        Timed,

        Permanent
    }

    public enum EffectStacking
    {
        Refresh, 

        Extend,  

        Stack,   

        Ignore   
    }

    public class EffectDefinition : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string NameKey = "";

        public string DescriptionKey = "";

        public EffectKind Kind = EffectKind.Combat;

        public float Duration;

        public EffectStacking Stacking = EffectStacking.Refresh;

        public int MaxStacks = 1;

        public bool CombatOnly;

        public EffectFlags Flags;

        public string BlockedTextKey = "";

        public Identifier? OnApply;

        public Identifier? OnRound;

        public Identifier? OnExpire;

        public string Name => NameKey.Length > 0 ? NameKey.Localized() : Id.Path;

        public string Description => DescriptionKey.Length > 0 ? DescriptionKey.Localized() : "";
    }
}