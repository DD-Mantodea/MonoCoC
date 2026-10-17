using MonoCoC.Attributes;
using MonoCoC.Contents.Flags;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Seasonal
{
    [Register<SeasonsScene>("coc:seasons")]
    public class SeasonsScene : Scene
    {

        public const int WINTER = 0;

        public const int SPRING = 1;

        public const int SUMMER = 2;

        public const int AUTUMN = 3;

        public const int WITCH = -1;

        public override void Entry()
        {
            IsItSaturnalia();
        }

        public bool IsItSaturnalia(bool strict = false)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ItsEveryDay) > 0 && !strict;
        }

        public bool IsItAprilFools()
        {

            return DateTime.Now.Month == 3 && DateTime.Now.Day == 1;
        }

        public bool IsItValentine(bool strict = false)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ItsEveryDay) > 0 && !strict;
        }

        public bool IsItHalloween(bool strict = false)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ItsEveryDay) > 0 && !strict;
        }

        public bool IsItEaster(bool strict = false)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ItsEveryDay) > 0 && !strict;
        }

        public bool IsItThanksgiving(bool strict = false)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ItsEveryDay) > 0 && !strict;
        }

        public bool IsItWinter(bool strict = false)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ItsEveryDay) > 0 && !strict;
        }

        public bool IsItSpring(bool strict = false)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ItsEveryDay) > 0 && !strict;
        }

        public bool IsItSummer(bool strict = false)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ItsEveryDay) > 0 && !strict;
        }

        public bool IsItAutumn(bool strict = false)
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ItsEveryDay) > 0 && !strict;
        }

    }
}