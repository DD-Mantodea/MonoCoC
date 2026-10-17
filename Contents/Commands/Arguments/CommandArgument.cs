using MonoCoC.Contents.Items;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Scenes;
using MonoCoC.Registry;
using MonoCoC.Saving.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Commands.Arguments
{
    public abstract class CommandArgument
    {
        protected CommandArgument(string name, bool optional = false)
        {
            Name = name;

            IsOptional = optional;
        }

        public string Name;

        public bool IsOptional;

        public abstract string UsageText { get; }

        public abstract bool TryParse(string text, out object value);

        public virtual IEnumerable<string> GetCompletions() => [];

        public virtual bool InScope(object value, object last) => true;
    }

    public class StringArgument : CommandArgument
    {
        public StringArgument(string name, bool optional = false) : base(name, optional) { }

        public override string UsageText => "<" + Name + ">";

        public override bool TryParse(string text, out object value)
        {
            value = text;

            return true;
        }
    }

    public class IntArgument : CommandArgument
    {
        public IntArgument(string name, bool optional = false) : base(name, optional) { }

        public override string UsageText => "<" + Name + ">";

        public override bool TryParse(string text, out object value)
        {
            if (int.TryParse(text, out var number))
            {
                value = number;

                return true;
            }

            value = 0;

            return false;
        }
    }

    public class FloatArgument : CommandArgument
    {
        public FloatArgument(string name, bool optional = false) : base(name, optional) { }

        public override string UsageText => "<" + Name + ">";

        public override bool TryParse(string text, out object value)
        {
            if (float.TryParse(text, out var number))
            {
                value = number;

                return true;
            }

            value = 0f;

            return false;
        }
    }

    public class BoolArgument : CommandArgument
    {
        public BoolArgument(string name, bool optional = false) : base(name, optional) { }

        public override string UsageText => "<" + Name + ">";

        public override bool TryParse(string text, out object value)
        {
            if (bool.TryParse(text, out var result))
            {
                value = result;

                return true;
            }

            if (text is "on" or "yes" or "1")
            {
                value = true;

                return true;
            }

            if (text is "off" or "no" or "0")
            {
                value = false;

                return true;
            }

            value = false;

            return false;
        }
    }

    public class EnumArgument<T> : CommandArgument where T : struct, Enum
    {
        public EnumArgument(string name, bool optional = false) : base(name, optional) { }

        public override string UsageText => "<" + Name + ">";

        public override bool TryParse(string text, out object value)
        {
            if (Enum.TryParse<T>(text, true, out var result))
            {
                value = result;

                return true;
            }

            value = default(T)!;

            return false;
        }

        public override IEnumerable<string> GetCompletions() => Enum.GetNames<T>();
    }

    public class ItemArgument : CommandArgument
    {
        public ItemArgument(string name, bool optional = false) : base(name, optional) { }

        public override string UsageText => "<item>";

        public override bool TryParse(string text, out object value)
        {
            if (ItemIdMap.Resolve(text, out var id) && ItemRegistry.GetDefinition(id) != null)
            {
                value = ItemRegistry.Create(id);

                return true;
            }

            value = Item.Nothing;

            return false;
        }

        public override IEnumerable<string> GetCompletions() => ItemRegistry.Ids.Select(id => id.ToString());
    }

    public class SceneArgument : CommandArgument
    {
        public SceneArgument(string name, bool optional = false) : base(name, optional) { }

        public override string UsageText => "<scene>";

        public override bool TryParse(string text, out object value)
        {
            if (Identifier.TryParse(text, out var id) && SceneRegistry.Scenes.Has(id))
            {
                value = id;

                return true;
            }

            value = default(Identifier);

            return false;
        }

        public override IEnumerable<string> GetCompletions() => SceneRegistry.Scenes.Ids.Select(id => id.ToString());
    }

    public class EnemyArgument : CommandArgument
    {
        public EnemyArgument(string name, bool optional = false) : base(name, optional) { }

        public override string UsageText => "<enemy>";

        public override bool TryParse(string text, out object value)
        {
            var idText = text.Contains(':') ? text : "coc:" + text;

            if (Identifier.TryParse(idText, out var id) && EnemyRegistry.Definitions.Has(id))
            {
                value = id;

                return true;
            }

            value = default(Identifier);

            return false;
        }

        public override IEnumerable<string> GetCompletions() =>

            EnemyRegistry.Definitions.Ids.Select(id => id.ToString());
    }

    public class TimeArgument : CommandArgument
    {
        public TimeArgument(string name, bool optional = false) : base(name, optional) { }

        public override string UsageText => "<hour>";

        public override bool TryParse(string text, out object value)
        {
            if (int.TryParse(text, out var hour) && hour >= 0 && hour <= 23)
            {
                value = hour;

                return true;
            }

            value = 0;

            return false;
        }

        public override IEnumerable<string> GetCompletions() => Enumerable.Range(0, 24).Select(value => value.ToString());
    }
}