using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    public class PetLocation
    {
        public string[] Texts = [];

        public string[] VisibleFrom = [];

        public string Descript = "";
    }

    [Register<AbstractPetScene>("coc:abstract_pet")]
    public class AbstractPetScene : Scene
    {

        public const int NONE = -1;

        public string location = "Camp";

        public int actionSeen = NONE;

        public string _name = "Pet";

        protected Dictionary<string, PetLocation> actions = [];

        protected Dictionary<string, PetLocation> statics = [];

        public string name
        {
            get => _name;

            set => _name = value;
        }

        public override void Entry()
        {
            IsOwned();
        }

        public virtual bool IsOwned()
        {

            return false;
        }

        public virtual void PetMenu(Action returnFunc, bool descOnly = false)
        {

        }

        public virtual void MenuButton(Action returnFunc, int pos = 0, string fromLocation = "Camp")
        {
            if (IsVisible(fromLocation))
                AddButton(pos, name, () => PetMenu(returnFunc), name + " is here.");
            else
                AddButtonDisabled(pos, name, name + " isn't here right now.");
        }

        public virtual bool IsVisible(string fromLocation = "Camp")
        {

            return statics.TryGetValue(location, out var info) && info.VisibleFrom.Contains(fromLocation);
        }

        public virtual string LocationShort()
        {

            return statics.TryGetValue(location, out var info) ? info.Descript : "";
        }

        public virtual void LocationDesc(string fromLocation, bool nlPre = false, bool nlPost = true)
        {

            if (!IsOwned())
                return ;

            var desc = "";

            actions.TryGetValue(location, out var actionInfo);

            statics.TryGetValue(location, out var staticInfo);

            var actionVisible = actionInfo != null && actionInfo.VisibleFrom.Contains(fromLocation);

            var staticVisible = staticInfo != null && staticInfo.VisibleFrom.Contains(fromLocation);

            if (actionSeen != NONE)
                desc = staticInfo != null && actionSeen < staticInfo.Texts.Length ? staticInfo.Texts[actionSeen] : "";
            else
                if (actionVisible)
                {
                    desc = actionInfo.Texts[Random.Shared.Next(actionInfo.Texts.Length)];

                    actionSeen = Random.Shared.Next(staticInfo.Texts.Length);
                }
                else
                    if (staticVisible && actionInfo != null)
                    {
                        var current = (View.Time.Days + View.Time.Hours) % staticInfo.Texts.Length;

                        desc = staticInfo.Texts[current];
                    }
                    else
                        if (staticVisible)
                        {
                            actionSeen = Random.Shared.Next(staticInfo.Texts.Length);

                            desc = staticInfo.Texts[actionSeen];
                        }

            if (desc.Length > 0)

                OutputText((nlPre ? "[pg]" : "") + desc + (nlPost ? "[pg]" : "")); // 原版 AbstractPet.hx:95 直接拼接输出，没有额外的包装文本
        }

    }
}