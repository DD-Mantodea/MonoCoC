using MonoCoC.UI.Framework;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Data
{
    public sealed record UIPartRequirement(string Name, Type ComponentType);

    public sealed record UIViewContract(string Id, IReadOnlyList<UIPartRequirement> RequiredParts)
    {
        public static UIViewContract Empty { get; } = new("Empty", []);

        public void Validate(Component root)
        {
            foreach (var requirement in RequiredParts)
            {
                var component = FindPart(root, requirement.Name);

                if (component is null)
                    throw new UIViewContractException(
                        $"View '{Id}' is missing required part '{requirement.Name}'");

                if (!requirement.ComponentType.IsInstanceOfType(component))
                    throw new UIViewContractException(
                        $"View '{Id}' part '{requirement.Name}' must be " +
                        $"{requirement.ComponentType.Name}, got {component.GetType().Name}");
            }
        }

        private static Component? FindPart(Component component, string name)
        {
            if (string.Equals(component.PartName, name, StringComparison.OrdinalIgnoreCase))
                return component;

            foreach (var child in component.LayoutData.Children)
            {
                var result = FindPart(child, name);

                if (result is not null)
                    return result;
            }

            return null;
        }
    }

    public sealed class UIViewContractException : Exception
    {
        public UIViewContractException(string message)
            : base(message)
        {
        }
    }
}

