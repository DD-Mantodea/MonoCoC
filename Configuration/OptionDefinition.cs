using Hjson;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Configuration
{
    public class OptionValueDefinition
    {
        public int Value;

        public string LabelKey = "";

        public string DescriptionKey = "";

        public string Data = "";
    }

    public class OptionDefinition
    {
        public Identifier Id;

        public string LabelKey = "";

        public string DescriptionKey = "";

        public string ButtonKey = "";

        public string Action = "";

        public string Kind = "";

        public string Apply = "";

        public string Source = "";

        public string Control = "";

        public bool OverridesLabel;

        public int DefaultValue;

        public List<OptionValueDefinition> Values = [];

        public JsonObject? SubPage;
    }

    public class OptionPaneDefinition
    {
        public string Id = "";

        public string TitleKey = "";

        public string DescriptionKey = "";

        public string ButtonKey = "";

        public bool Global;

        public string Kind = "";

        public int Order;

        public List<OptionDefinition> Settings = [];
    }
}
