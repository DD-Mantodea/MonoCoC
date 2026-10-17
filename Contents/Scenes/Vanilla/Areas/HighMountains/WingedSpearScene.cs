using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using System;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains
{
    [Register<WingedSpearScene>("coc:winged_spear")]
    public class WingedSpearScene : Scene
    {

        public bool encountered = false;

        public bool examined = false;

        public bool takenSpear = false;

        public static SaveContent Save => SaveContent(SceneIds.HighMountains.WingedSpear);

        public string debugName => "Winged Spear";

        public string debugHint => "";

        public override void Entry()
        {
            ExecEncounter();
        }

        public void Reset()
        {

            Save.SetBool("encountered", false);

            Save.SetBool("examined", false);

            Save.SetBool("takenSpear", false);
        }

        public void ExecEncounter()
        {

            FallenKnight();
        }

        public void FallenKnight()
        {
            ClearOutput();

            OutputText("Scenes.Wingedspearscene.FallenKnight.WindBuffets".Localized());

            OutputText("Scenes.Wingedspearscene.FallenKnight.GlanceSky".Localized((CoC.Instance.Flags.Get(GameFlagIds.CodexEntryHarpies) > 0 ? " know you're near harpy territory, and though you think you can see one flying overhead some distance away, she doesn't seem interested in you yet" : "'ve seen shadows, larger than the usual bird, flying overhead some distance away, though right now you can find no sign of them, whatever they were")));

            OutputText("Scenes.Wingedspearscene.FallenKnight.InvestigatePrompt".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), FallenKnightYes);

            AddButton(1, "Common.Button.No".Localized(), FallenKnightNo);
        }

        public void FallenKnightNo()
        {
            ClearOutput();

            OutputText("Scenes.Wingedspearscene.FallenKnightNo.NoRiskContinue".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FallenKnightYes()
        {
            ClearOutput();

            Save.SetBool("encountered", true);

            OutputText("Scenes.Wingedspearscene.FallenKnightYes.DecidesRisk".Localized());

            if (Player.Intelligence + Player.Speed < 50)
            {
                OutputText("Scenes.Wingedspearscene.FallenKnightYes.OverestimatedFall".Localized());

                if (silly && Player.Toughness > 100)
                {
                    OutputText("Scenes.Wingedspearscene.FallenKnightYes.ChipsDustFall".Localized());

                    OutputText("Scenes.Wingedspearscene.FallenKnightYes.MudComeTo".Localized());
                }
                else
                {
                    if (Player.IsGoo())
                    {
                        OutputText("Scenes.Wingedspearscene.FallenKnightYes.GooTorn".Localized());

                        OutputText("Scenes.Wingedspearscene.FallenKnightYes.GooDigested".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Wingedspearscene.FallenKnightYes.AgonySnaps".Localized());

                        OutputText("Scenes.Wingedspearscene.FallenKnightYes.DarknessFall".Localized());
                    }

                    GameOver();

                    return ;
                }
            }
            else
            {
                OutputText("Scenes.Wingedspearscene.FallenKnightYes.MightiestBlast".Localized());

                OutputText("Scenes.Wingedspearscene.FallenKnightYes.PathDownSafe".Localized());

                Player.ChangeFatigue(15);
            }

            RegisterMacro("exhausted", Player.Intelligence + Player.Speed >= 50);

            OutputText("Scenes.Wingedspearscene.FallenKnightYes.StreamGurgles".Localized());

            OutputText("Scenes.Wingedspearscene.FallenKnightYes.KnightlyRegalia".Localized());

            OutputText("Scenes.Wingedspearscene.FallenKnightYes.ArmorUnsalvageable".Localized());

            FallenKnightMenu();
        }

        public void FallenKnightMenu()
        {

            RegisterMacro("takenspear", Save.GetBool("takenSpear"));

            RegisterMacro("examined", Save.GetBool("examined"));

            ClearButtons();

            AddNextButton("Common.Button.Examine".Localized(), FallenKnightExamine);

            AddNextButton("Common.Button.TakeSpear".Localized(), FallenKnightTake);

            AddNextButton("Common.Button.Leave".Localized(), FallenKnightLeave);
        }

        public void FallenKnightExamine()
        {
            ClearOutput();

            OutputText("Scenes.Wingedspearscene.FallenKnightExamine.LeanCorpse".Localized());

            OutputText("Scenes.Wingedspearscene.FallenKnightExamine.RustedPlate".Localized(
                 CoC.Instance.Flags.Get(GameFlagIds.CodexEntryHarpies) > 0 ? ". Harpies seem a plausible culprit, " + (new[] { BodyPartTypes.Harpy, BodyPartTypes.Cockatrice }.Contains(Player.Body.Get<LowerBodyPart>().Id) ? "which you're able to all but confirm by carefully aligning your own foot's fierce talons with one of the puncture groups" : "their fierce talons fitting well what you see here") + ". Supporting that conclusion are" : ", though you're less sure about what manner of creature could be responsible as you note"
            ));

            OutputText("Scenes.Wingedspearscene.FallenKnightExamine.HelmetLoosened".Localized());

            OutputText("Scenes.Wingedspearscene.FallenKnightExamine.NothingOnHim".Localized());

            Save.SetBool("examined", true);

            FallenKnightMenu();
        }

        public void FallenKnightTake()
        {
            ClearOutput();

            OutputText("Scenes.Wingedspearscene.FallenKnightTake.GripsSpear".Localized());

            OutputText("Scenes.Wingedspearscene.FallenKnightTake.BladeNeedsPolish".Localized());

            Save.SetBool("takenSpear", true);

            FallenKnightMenu();

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.WingedSpear), FallenKnightMenu);
        }

        public void FallenKnightLeave()
        {
            ClearOutput();

            OutputText("Scenes.Wingedspearscene.FallenKnightLeave.YouLeaveLead".Localized());

            OutputText("Scenes.Wingedspearscene.FallenKnightLeave.RavineAscends".Localized());

            ReturnToCamp(1);
        }

    }
}