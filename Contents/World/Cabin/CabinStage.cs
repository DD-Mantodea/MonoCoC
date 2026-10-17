using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.World.Cabin
{
    public interface ICabinRequirement
    {
        bool IsMet(CabinBuildContext context);

        string DescribeKey { get; }
    }

    public class CabinResource : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string NameKey = "";

        public Flag<int>? Flag;

        public Identifier KeyItemId;

        public int KeyItemValue;

        public int Max;

        public bool IsKeyItem => !string.IsNullOrEmpty(KeyItemId.Path);

        public int Get(Player player)
        {
            if (Flag is { } flag)
                return CoC.Instance.Flags.Get(flag);

            return IsKeyItem ? (int)player.KeyItemV1(KeyItemId) : 0;
        }

        public void Add(Player player, int amount)
        {
            if (Flag is { } flag)
            {
                var value = CoC.Instance.Flags.Get(flag) + amount;

                if (value < 0)
                    value = 0;

                if (Max > 0 && value > Max)
                    value = Max;

                CoC.Instance.Flags.Set(flag, value);

                return;
            }

            if (!IsKeyItem)
                return;

            var current = (int)player.KeyItemV1(KeyItemId);

            var target = current + amount;

            if (target < 0)
                target = 0;

            if (Max > 0 && target > Max)
                target = Max;

            if (target == current)
                return;

            player.AddKeyValue(KeyItemId, KeyItemValue, target - current);
        }
    }

    public readonly record struct CabinCost(CabinResource Resource, int Amount);

    public class CabinStage : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string NameKey = "";

        public string DescriptionKey = "";

        public int Order;

        public List<ICabinRequirement> Requirements = [];

        public List<CabinCost> Costs = [];

        public int Fatigue;

        public int Hours;

        public Identifier Handler;
    }

    public class CabinFurniture : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string NameKey = "";

        public string DescriptionKey = "";

        public List<ICabinRequirement> Requirements = [];

        public List<CabinCost> Costs = [];

        public Flag<int>? BuiltFlag;

        public Flag<int>? SecondFlag;
    }

    public class CabinBuildContext
    {
        public Player Player = null!;

        public GameFlags Flags = null!;

        public CabinState State = null!;

        public CabinStage Stage = null!;
    }

    public class CabinResourceRequirement(CabinResource resource, int amount) : ICabinRequirement
    {
        public string DescribeKey => resource.NameKey;

        public bool IsMet(CabinBuildContext context) => resource.Get(context.Player) >= amount;
    }

    public class CabinFlagRequirement(Flag<bool> flag) : ICabinRequirement
    {
        public string DescribeKey => "";

        public bool IsMet(CabinBuildContext context) => context.Flags.Get(flag);
    }

    public class CabinKeyItemRequirement(Identifier item) : ICabinRequirement
    {
        public string DescribeKey => "";

        public bool IsMet(CabinBuildContext context) => context.Player.HasKeyItem(item);
    }

    public class CabinStageRequirement(Identifier stage) : ICabinRequirement
    {
        public string DescribeKey => "";

        public bool IsMet(CabinBuildContext context) => context.State.IsCompleted(stage);
    }

    public class CabinFuncRequirement(Func<CabinBuildContext, bool> predicate, string describeKey) : ICabinRequirement
    {
        public string DescribeKey => describeKey;

        public bool IsMet(CabinBuildContext context) => predicate(context);
    }
}