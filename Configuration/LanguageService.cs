using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Configuration
{
    public static class LanguageService
    {
        public static void ApplySaved()
        {
            var store = OptionStore.Instance;

            store.Initialize();

            var setting = store.Panes.SelectMany(pane => pane.Settings).FirstOrDefault(candidate => candidate.Apply == "language");

            if (setting == null)
                return;

            var values = Choices();

            var value = store.Get(setting.Id, setting.DefaultValue);

            var choice = values.FirstOrDefault(candidate => candidate.Value == value);

            if (choice != null)
                Use(choice.Data);
        }

        public static List<OptionValueDefinition> Choices()
        {
            var result = new List<OptionValueDefinition>();

            var manager = ManagerInstance<LocalizationManager>.Instance;

            for (var index = 0; index < manager.Languages.Count; index++)
            {
                var language = manager.Languages[index];

                result.Add(new OptionValueDefinition
                {
                    Value = index,

                    LabelKey = language.Name,

                    Data = language.Code
                });
            }

            return result;
        }

        public static void Use(string language)
        {
            if (string.IsNullOrEmpty(language))
                return;

            try
            {
                ManagerInstance<LocalizationManager>.Instance.Use(language);

                UI.UIFonts.ApplyLanguage(language);

                UI.UIFonts.ApplyToParser(Contents.Scenes.Scene.ScriptContext);
            }
            catch
            {
                // Unknown language: keep the current one.
            }
        }
    }
}