using MonoCoC.Contents.World;
using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;
using System.Linq;
using MonoCoC.Contents.Scenes.Vanilla.Monsters;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<ImpGangBangScene>("coc:imp_gang_bang")]
    public class ImpGangBangScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Core.ImpGangBang);

        public string debugName => "Imp Gangbang";

        public string debugHint => "";

        public override void Entry()
        {
            OutsideYes();
        }

        public int checkedDay;

        public int askedDay;

        public bool steppedOutside;

        public bool TimeChange() => false;

        public bool TimeChangeLarge()
        {
            if (View.Time.Hours != 2 || checkedDay == View.Time.Days)
                return false;

            return GangbangCheck();
        }

        public void Reset()
        {

            Save.Clear();
        }

        public void OutsideYes()
        {
            steppedOutside = true;

            ClearOutput();

            SceneInstance<ImpScene>.Get().ImpGangabangaEXPLOSIONS();
        }

        public void OutsideNo()
        {
            ClearOutput();

            GangbangCheck();
        }

        public void OutsideNever()
        {
            ClearOutput();

            Save.SetBool("neverAsk", true);

            GangbangCheck();
        }

        public bool SleepingInside()
        {

            return !steppedOutside && CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0 && CabinInterior.HasBed && (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Marble" || CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "");
        }

        public bool GangbangCheck()
        {

            if (Player.IsGenderless() || Player.HasStatusEffect(StatusEffectIds.DefenseCanopy))
                return false;

            checkedDay = View.Time.Days;

            steppedOutside = false;

            var lockedDoor = Random.Shared.Next(100) < (10 + Player.Intelligence * 1.5f);

            var sleepInside = lockedDoor && SleepingInside();

            var fullChance = Math.Clamp( 1 + Player.StatusEffectValue(StatusEffectIds.BirthedImps, 0) * 2, 0, 7);

            if (Player.HasPerk(PerkIds.PiercedLethite))
                fullChance += 4;

            if (Player.InHeat)
                fullChance += 2;

            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaFollower) > 0)
                fullChance += 7;

            float scarePercent = Math.Clamp( CoC.Instance.Flags.Get(GameFlagIds.CampWallSkulls) + CoC.Instance.Flags.Get(GameFlagIds.CampWallStatues) * 4, 0,  100);

            float wallFactor = 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.CampWallProgress) > 0)
                wallFactor *= 1 + (CoC.Instance.Flags.Get(GameFlagIds.CampWallProgress) / 100);

            if (CoC.Instance.Flags.Get(GameFlagIds.CampWallGate) > 0)
                wallFactor *= 2;

            var reducedChance = fullChance * (1 - (scarePercent / 100)) / wallFactor;

            if (!Save.GetBool("neverAsk") && askedDay != View.Time.Days && Player.HasPerk(PerkIds.BroodMother) && !CampScene.CampGuarded() && (wallFactor > 1 || scarePercent > 0) && Random.Shared.Next(100) < (fullChance))
            {
                askedDay = View.Time.Days;

                OutputText("Scenes.Impgangbang.GangbangCheck.WombCallsAgain".Localized());

                OutputText("Scenes.Impgangbang.GangbangCheck.SpendNightOutsidePrompt".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), OutsideYes);

                AddButton(1, "Common.Button.No".Localized(), OutsideNo);

                AddButton(2, "Scenes.Faerie.SexMenu.ButtonNever".Localized(), OutsideNever);

                return true;
            }
            else
                if (!(Random.Shared.Next(100) < (reducedChance)))
                    return false;

            if (CampScene.CampGuarded())
            {
                OutputText("Scenes.Impgangbang.GangbangCheck.BoldStartTag".Localized());

                var guards = new[] { "Jojo", "Helia", "KidA", "Holli", "Kiha", "Nieve" }.Where(CampGuards.IsGuard).ToArray();

                if (guards.Length > 0)
                    switch (guards[Random.Shared.Next(guards.Length)])
                    {
                        case "Jojo":
                            OutputText("Scenes.Impgangbang.GangbangCheck.JojoDispatchedImps".Localized());

                            break;

                        case "Helia":
                            OutputText("Scenes.Impgangbang.GangbangCheck.HeliaWhuppedImps".Localized());

                            break;

                        case "KidA":
                            OutputText("Scenes.Impgangbang.GangbangCheck.ClawedFeetSkitter".Localized());

                            break;

                        case "Holli":
                            OutputText("Scenes.Impgangbang.GangbangCheck.HolliCanoeImps".Localized());

                            break;

                        case "Kiha":
                            OutputText("Scenes.Impgangbang.GangbangCheck.KihaCharredImps".Localized());

                            break;

                        case "Nieve":
                            OutputText("Scenes.Impgangbang.GangbangCheck.NieveFrozenImps".Localized());

                            break;
                    }

                OutputText("Scenes.Impgangbang.GangbangCheck.BoldEndTag".Localized());

                Go<CampScene>();
            }
            else
                if (sleepInside)
                {
                    OutputText("Scenes.Impgangbang.GangbangCheck.LockedDoorImps".Localized());

                    Go<CampScene>();
                }
                else

                    SceneInstance<ImpScene>.Get().ImpGangabangaEXPLOSIONS();

            return true;
        }

    }
}