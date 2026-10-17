using MonoCoC.Contents.World;
using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;
using MonoCoC.Contents.Followers;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<AkkyScene>("coc:akky")]
    public class AkkyScene : AbstractPetScene, ITimeAware
    {

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Akky);

        public string debugName => "Akky";

        public string debugHint => "";

        public override void Entry()
        {
            var ownedName = CoC.Instance.Flags.Get(GameFlagIds.AkkyName);

            if (!string.IsNullOrEmpty(ownedName))
                name = ownedName;

            IsOwned();
        }

        // 原版 Akky.hx:73 timeChange()：把手风琴（Akky）每天随机安置在营地各处
        public virtual bool TimeChange()
        {
            actionSeen = NONE;

            if (!IsOwned())
            {
                location = "Camp";

                return false;
            }

            var choices = new List<string> { "Camp", "Stream" };

            if (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) >= 1)
                choices.Add("Cabin");

            if (SceneInstance<JojoScene>.Get().JojoFollower())
                choices.Add("Jojo");

            if (MarbleScene.FollowerRathazul())
                choices.Add("Rathazul");

            if (SceneInstance<MarbleScene>.Get().MarbleAtCamp())
                choices.Add("Marble");

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) >= 1)
                choices.Add("MarbleChildren");

            if (SceneInstance<IzmaScene>.Get().TotalIzmaChildren() >= 1)
                choices.Add("SharkChildren");

            if (SceneInstance<AmilyScene>.Get().AmilyFollowerPure()
                && CoC.Instance.Flags.Get(GameFlagIds.AmilyBlockCountdownBecauseCorruptedJojo) == 0)

                choices.Add("Amily");

            if (SceneInstance<HolliScene>.Get().HolliFollower())
                choices.Add("Holli");

            location = choices[Random.Shared.Next(choices.Count)];

            var hours = View.Time.Hours;

            if ((hours >= 20 || hours <= 8) && choices.Contains("Cabin"))
            {
                if (Random.Shared.Next(3) > 0)
                    location = "Cabin";
            }
            else if (Random.Shared.Next(2) > 0)
                location = Random.Shared.Next(2) == 0 ? "Camp" : "Stream";

            BuildDescs();

            return false;
        }

        // 原版 Akky.hx:116 timeChangeLarge()
        public virtual bool TimeChangeLarge() => false;

        public void Reset()
        {

            Save.SetBool("giftedBear", false);

            Save.SetBool("harpiesHarassed", false);
        }

        public override bool IsOwned()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.AkkyName) != "";
        }

        // 原版 Akky.hx menuButton：在营地可见时打开宠物菜单，不可见时显示位置提示
        public override void MenuButton(Action returnFunc, int pos = 0, string fromLocation = "Camp")
        {
            if (!IsVisible(fromLocation))
            {
                AddButtonDisabled(pos, name + " isn't here right now.");

                return;
            }

            AddButton(pos, name, () => PetMenu(returnFunc), LocationShort());
        }

        public override void PetMenu(Action returnFunc, bool descOnly = false)
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) > 0 && !Save.GetBool("harpiesHarassed") && Random.Shared.Next(10) == 0)
            {
                AkkyPlaysWithBirds();

                return;
            }

            var catFood = new[] { ConsumableIds.FishFillet };

            Identifier hasFood = null;

            foreach (var food in catFood)
                if (Player.HasItem(food))
                {
                    hasFood = food;

                    break;
                }

            OutputText("Scenes.Akky.PetMenu.HousecatDesc".Localized());

            OutputText("Scenes.Akky.PetMenu.PetMenuPrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Pet".Localized(), () => PetAkky(returnFunc));

            AddButton(1, "Common.Button.Feed".Localized(), () => FeedAkky(hasFood, returnFunc));

            AddButton(2, "Common.Button.Talk".Localized(), () => TalkingToCats(returnFunc));

            if (silly && !Save.GetBool("giftedBear"))
                AddNextButton("Common.Button.GiftBear".Localized(), GiftAkkyBear);

            AddButton(14, "Common.Button.Back".Localized(), returnFunc);
        }

        public void BuildDescs()
        {

            actions = new()
            {
                ["Rathazul"] = new PetLocation
                {
                    Texts = ["[akky] stares at Rathazul while... shaking? Suddenly [akky] leaps forward toward the alchemist, disrupting his experimentation. Cat vs. mouse instincts, you suppose. Good thing he's not a jaguar anymore."],

                    VisibleFrom = ["Rathazul"],

                    Descript = ""
                }
            };

            statics = new()
            {
                ["Stream"] = new PetLocation
                {
                    Texts = ["You can see [akky] playing happily in the stream, showing off his excellent swimming skills. Unusual for a housecat, maybe it's a remnant of his time as a jaguar?", "[akky] is sleeping curled up on a large rock in the middle of the stream.", "You find [akky] stretched out on the bank of the stream, sleeping peacefully.", "[akky] sits beside the stream, his sharp gaze locked onto a fish swimming near the water's edge."],

                    VisibleFrom = ["Stream", "Camp"],

                    Descript = "near the stream",
                },

                ["Cabin"] = new PetLocation
                {
                    Texts = ["[akky] is lazily stretched out by the window.", "[akky] curiously roots through your stuff.", "[akky] is here, staring intently at an unremarkable spot on the wall. No matter how long you watch him he doesn't shift his gaze."],

                    VisibleFrom = ["Cabin"],

                    Descript = "in the cabin",
                },

                ["Jojo"] = new PetLocation
                {
                    Texts = [""],

                    VisibleFrom = [""],

                    Descript = "with Jojo",
                },

                ["Rathazul"] = new PetLocation
                {
                    Texts = ["Rathazul is keeping a watchful eye on the nearby [akky]."],

                    VisibleFrom = ["Rathazul"],

                    Descript = "with Rathazul",
                },

                ["Marble"] = new PetLocation
                {
                    Texts = ["[akky] is here cuddling against Marble. She seems very pleased to have an innocuously adorable companion at camp."],

                    VisibleFrom = ["Marble"],

                    Descript = "with Marble",
                },

                ["MarbleChildren"] = new PetLocation
                {
                    Texts = ["[akky] is playing around with your bovine offspring."],

                    VisibleFrom = ["Marble"],

                    Descript = "with Marble's children",
                },

                ["SharkChildren"] = new PetLocation
                {
                    Texts = ["[akky] is making mock poses of intimidation at a shark-daughter of yours. She seems to be playing along very happily, doing clawing motions with [akky]."],

                    VisibleFrom = ["Izma"],

                    Descript = "with your shark children",
                },

                ["Amily"] = new PetLocation
                {
                    Texts = ["[akky] is following Amily and jumping up to bump his head against her hand. Seems he's taken very well to her."],

                    VisibleFrom = ["Amily"],

                    Descript = "with Amily",
                },

                ["Holli"] = new PetLocation
                {
                    Texts = ["Holli watches as [akky] claws at her bark. She seems annoyed, but isn't stopping him."],

                    VisibleFrom = ["Holli"],

                    Descript = "with Holli"
                },
            };

            var campTexts = new List<string>
            {
                "[akky] greets you with a happy meow. Or maybe a hungry meow, it's hard to tell.",

                "[akky] appears to be 'sun-bathing' in front of the warm glow of the portal.",

                "[akky] is amusing himself by jumping around on the rocks for no apparent reason.",

                "[akky] is tossing and turning on the ground. Stretching? Scratching his back against the roughness? You aren't sure.",
            };

            if (CoC.Instance.Flags.Get(GameFlagIds.CampWallProgress) >= 20)
                campTexts.Add("[akky] is walking around atop the wall.");

            if (CabinInterior.HasDresser)
                campTexts.Add("[akky] watches you from his hiding place behind the dresser.");

            if (CabinInterior.HasBed)
                campTexts.Add("[akky] has confiscated the bed, his body sprawled out seemingly with the intent of taking up as much space as possible.");

            if (CabinInterior.HasDesk)
                campTexts.Add("[akky] is curled up on top of the desk, sleeping without a care in the world. Meanwhile, the former contents of the desk are scattered on the floor.");

            statics["Camp"] = new PetLocation
            {
                Texts = campTexts.ToArray(),

                VisibleFrom = ["Camp"],

                Descript = "in the camp",
            };
        }

        public void PetAkky(Action returnFunc)
        {
            ClearOutput();

            OutputText("Scenes.Akky.PetAkky.YouPetEnjoyingThe".Localized(
                RandomUtils.Choice(
            "You pet [akky], enjoying the gentle vibrations of his purring beneath your stroking. Such a cute companion to have around.",

            "You stroke [akky]'s fine fuzz, and the little kitty rolls over to receive a belly-rub.",

            "[Akky] presses his head into your hand as you reach for him, purring loudly when you scratch and rub him.",

            "The little kitty bites your hand immediately on contact! However, he quickly follows up with several licks as you continue to pet him.",

            "You lightly pat the cat on the head. [Akky] rubs his face against your palm for more.",

            "[Akky] slowly shuts his eyes as you pet him, purring loudly to express his pleasure."
        )
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => PetMenu(returnFunc));
        }

        public void FeedAkky(Identifier food, Action returnFunc)
        {
            ClearOutput();

            OutputText("Scenes.Akky.FeedAkky.RetrievesFish".Localized());

            OutputText("Scenes.Akky.FeedAkky.SniffsFish".Localized());

            Player.ConsumeItem(food);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => PetMenu(returnFunc));
        }

        public void TalkingToCats(Action returnFunc)
        {
            ClearOutput();

            var options = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            if (FollowerRegistry.HasFollower(FollowerIds.ShouldraFollower) && Random.Shared.Next(100) < (50))
                options.Add(9);

            if (Player.Body.Get<HairPart>().Length >= 16)
                options.Add(10);

            if (Player.Race.IndexOf("cat-") >= 0 || Player.Race.IndexOf("kitten-") >= 0)
                options.Add(11);

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) > 0)
                options.Add(12);

            if (silly)
                options.Add(13);

            switch (options[Random.Shared.Next(options.Count)])
            {
                case 0:
                {
                OutputText("Scenes.Akky.TalkingToCats.DiscussesMarethian".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Akky.TalkingToCats.CallsAttention".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Akky.TalkingToCats.EducatedPet".Localized());

                OutputText("Scenes.Akky.TalkingToCats.LicksFingers".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Akky.TalkingToCats.MeowYouSay".Localized());

                OutputText("Scenes.Akky.TalkingToCats.MeowHeSays".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Akky.TalkingToCats.TakesLead".Localized());

                OutputText("Scenes.Akky.TalkingToCats.CleansHimself".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Akky.TalkingToCats.TellsDay".Localized());

                OutputText("Scenes.Akky.TalkingToCats.NotSurprised".Localized());
                }

                break;

                case 6:
                {
                OutputText("Scenes.Akky.TalkingToCats.FeelsAboutCat".Localized());

                OutputText("Scenes.Akky.TalkingToCats.SeemsContent".Localized());
                }

                break;

                case 7:
                {
                OutputText("Scenes.Akky.TalkingToCats.NothingToSay".Localized());
                }

                break;

                case 8:
                {
                OutputText("Scenes.Akky.TalkingToCats.TooMuchPussy".Localized());

                if (FollowerRegistry.HasFollower(FollowerIds.ShouldraFollower))
                    OutputText("Scenes.Akky.TalkingToCats.SnickeringEchoes".Localized());
                }

                break;

                case 9:
                {
                OutputText("Scenes.Akky.TalkingToCats.OnHisMind".Localized());

                OutputText("Scenes.Akky.TalkingToCats.GiveMeDick".Localized());

                OutputText("Scenes.Akky.TalkingToCats.DamnShouldra".Localized());

                OutputText("Scenes.Akky.TalkingToCats.RaisesBrow".Localized());
                }

                break;

                case 10:
                {
                OutputText("Scenes.Akky.TalkingToCats.AsksLittleCat".Localized());

                OutputText("Scenes.Akky.TalkingToCats.SwipesHair".Localized());
                }

                break;

                case 11:
                {
                OutputText("Scenes.Akky.TalkingToCats.YourKindOught".Localized());
                }

                break;

                case 12:
                {
                OutputText("Scenes.Akky.TalkingToCats.CantFuckAss".Localized());

                OutputText("Scenes.Akky.TalkingToCats.PouncingStance".Localized());
                }

                break;

                case 13:
                {
                OutputText("Scenes.Akky.TalkingToCats.WhatsNewPussycat".Localized());

                OutputText("Scenes.Akky.TalkingToCats.NovelYawn".Localized());
                }

                break;
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => PetMenu(returnFunc));
        }

        public void GiftAkkyBear()
        {
            ClearOutput();

            OutputText("Scenes.Akky.GiftAkkyBear.ShowsBear".Localized());

            OutputText("Scenes.Akky.GiftAkkyBear.RegardsBear".Localized());

            OutputText("Scenes.Akky.GiftAkkyBear.ShakesRear".Localized());

            OutputText("Scenes.Akky.GiftAkkyBear.CantBearWatch".Localized());

            Save.SetBool("giftedBear", true);

            Player.ConsumeItem(UseableIds.TeddyBear);

            ReturnToCamp(1);
        }

        public void AkkyPlaysWithBirds()
        {
            ClearOutput();

            OutputText("Scenes.Akky.AkkyPlaysWithBirds.HangOnMoment".Localized());

            OutputText("Scenes.Akky.AkkyPlaysWithBirds.SpotsHim".Localized());

            Save.SetBool("harpiesHarassed", true);

            ClearButtons();

            AddNextButton("Common.Button.Letthembe".Localized(), AkkyPlaysWithBirdsLeave);

            AddNextButton("Common.Button.Join".Localized(), AkkyPlaysWithBirdsJoin);
        }

        public void AkkyPlaysWithBirdsLeave()
        {
            ClearOutput();

            OutputText("Scenes.Akky.AkkyPlaysWithBirdsLeave.FarBeIt".Localized());

            Go<CampScene>();
        }

        public void AkkyPlaysWithBirdsJoin()
        {
            ClearOutput();

            OutputText("Scenes.Akky.AkkyPlaysWithBirdsJoin.WorthwhileSight".Localized());

            OutputText("Scenes.Akky.AkkyPlaysWithBirdsJoin.AlarmedNoise".Localized());

            OutputText("Scenes.Akky.AkkyPlaysWithBirdsJoin.YourTurn".Localized());

            OutputText("Scenes.Akky.AkkyPlaysWithBirdsJoin.ThroughToyed".Localized());

            OutputText("Scenes.Akky.AkkyPlaysWithBirdsJoin.PatsGirl".Localized());

            ReturnToCamp(1);
        }

    }

}