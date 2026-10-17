using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Followers
{

    public class FollowerDefinition
    {
        public Identifier Id { get; }

        public Identifier Role { get; }

        public string Name { get; }

        public string Description { get; }

        public Func<bool> IsPresent { get; }

        public Action? Interact { get; }

        public int Order { get; }

        public Func<bool>? IsGuard { get; }

        public FollowerDefinition(
            Identifier id,

            Identifier role,

            string name,

            Func<bool> isPresent,

            Action? interact = null,

            string description = "",

            int order = 0,

            Func<bool>? isGuard = null)
        {
            Id = id;

            Role = role;

            Name = name;

            IsPresent = isPresent;

            Interact = interact;

            Description = description;

            Order = order;

            IsGuard = isGuard;
        }
    }
}