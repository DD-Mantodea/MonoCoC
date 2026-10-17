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
        public string Id = "";

        public string LabelKey = "";

        public string DescriptionKey = "";

        public string ButtonKey = "";

        public string Action = "";

        public string Kind = "";

        public string Apply = "";

        public string Source = "";

        public bool OverridesLabel;

        public int DefaultValue;

        public List<OptionValueDefinition> Values = [];
    }

    public class OptionPaneDefinition
    {
        public string Id = "";

        public string TitleKey = "";

        public string DescriptionKey = "";

        public string ButtonKey = "";

        public bool Global;

        public List<OptionDefinition> Settings = [];
    }
}