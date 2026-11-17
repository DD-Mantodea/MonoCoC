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

        // 控件覆盖（例如 language 用 dropdown 而不是 switcher）。
        public string Control = "";

        public bool OverridesLabel;

        public int DefaultValue;

        public List<OptionValueDefinition> Values = [];

        // 选项的子页定义（数据驱动）：kind/title/message/rows 等，见 OptionsView.RenderSubPage。
        public JsonObject? SubPage;
    }

    public class OptionPaneDefinition
    {
        public string Id = "";

        public string TitleKey = "";

        public string DescriptionKey = "";

        public string ButtonKey = "";

        public bool Global;

        // 面板类型（空 = 普通设置页；controls = 键位绑定页，无 settings）。
        public string Kind = "";

        // 标签顺序；未声明时按文件加载顺序 × 10。
        public int Order;

        public List<OptionDefinition> Settings = [];
    }
}