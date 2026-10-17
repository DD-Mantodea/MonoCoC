using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Dungeons
{

    public class WizardTowerMirrorPuzzleScene : Scene
    {
        public static readonly int[] PuzzleLayout =

        [
            0,0,0,0,0,0,0,0,0,0,1,1,0,1,0,1,0,0,0,1,0,0,0,0,1,0,0,0,1,1,0,1,0,1,1,1,0,0,1,1,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,1,0,0,0,1,0,0,1,1,0,0,0,1,0,0,0,0,0,0,1,1,0,0,0
        ];

        public static readonly int[] MirrorLayout =

        [
            1,1,0,0,2,1,0,0,0,1,0,0,1,0,1,1,1,0,0,0,1,1,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,1,0,1,0,1,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,1,0,1,0,0,1,1,1,1,1,0,1,0,0,0,0,0,1,0,0,0,0
        ];

        public readonly DungeonScene _owner;

        public readonly int[] _puzzle;

        public readonly int[] _mirror;

        public int _playerLoc = 41;

        public int _mirrorLoc = 41;

        public WizardTowerMirrorPuzzleScene(DungeonScene owner)
        {
            _owner = owner;

            _puzzle = (int[])PuzzleLayout.Clone();

            _mirror = (int[])MirrorLayout.Clone();

            _puzzle[_playerLoc] = 3;

            _mirror[_mirrorLoc] = 3;
        }

        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.WizardTower.MirrorPuzzle2.OldTremblingVoice".Localized());

            OutputText("Scenes.WizardTower.MirrorPuzzle2.AnInvaderAfterYears".Localized());

            OutputText("Scenes.WizardTower.MirrorPuzzle2.LookAroundAttempting".Localized());

            OutputText("Scenes.WizardTower.MirrorPuzzle2.WeStillHaveTime".Localized());

            OutputText("Scenes.WizardTower.MirrorPuzzle2.TakeAnotherStepForward".Localized());

            OutputText("Scenes.WizardTower.MirrorPuzzle2.ForgotWhatImSupposed".Localized());

            OutputText("Scenes.WizardTower.MirrorPuzzle2.WhateverThatIsCant".Localized());

            OutputText("Scenes.WizardTower.MirrorPuzzle2.MarvelousTraps".Localized());

            OutputText("Scenes.WizardTower.MirrorPuzzle2.BlinkOfAnEye".Localized());

            AddButton(0, "Common.Button.Next".Localized(), EnterMaze);
        }

        public void EnterMaze()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.WizardTower.MirrorPuzzle3.ComeToCompletely".Localized());

            OutputText("Scenes.WizardTower.MirrorPuzzle3.SomethingMorphs".Localized());

            OutputText("Scenes.WizardTower.MirrorPuzzle3.CrouchForBetter".Localized());

            OutputText("Scenes.WizardTower.MirrorPuzzle3.BriefAnalysisNotice".Localized());

            AddButton(0, "Common.Button.Next".Localized(), Draw);
        }

        public void Draw()
        {
            ClearOutput();

            ClearButtons();

            if (_mirrorLoc == 4)
            {
                EndPuzzle();

                return;
            }

            OutputText("Scenes.WizardTower.Redraw.FontFaceTypewriter".Localized());

            OutputText("Scenes.WizardTower.Redraw.FontB".Localized());

            var draw = "";

            for (var i = 0; i < _puzzle.Length; i++)
            {
                draw += _puzzle[i] switch
                {
                    0 => "[ ]",

                    1 => "   ",

                    3 => "[P]",

                    _ => "[ ]"
                };

                if ((i + 1) % 9 == 0)
                    draw += "\n";
            }

            draw += "\n\n";

            for (var j = 0; j < _mirror.Length; j++)
            {
                draw += _mirror[j] switch
                {
                    0 => "[ ]",

                    1 => "   ",

                    2 => "[X]",

                    3 => "[P]",

                    _ => "[ ]"
                };

                if ((j + 1) % 9 == 0)
                    draw += "\n";
            }

            OutputText(draw);

            if (_puzzle[_playerLoc - 9] != 1 && _playerLoc - 9 >= 0)
                AddButton(6, "Common.Button.North".Localized(), () => Move(0));

            if (_playerLoc + 9 < 81 && _puzzle[_playerLoc + 9] != 1)
                AddButton(11, "Common.Button.South".Localized(), () => Move(1));

            if (_puzzle[_playerLoc - 1] != 1 && _playerLoc % 9 != 0 && _playerLoc != 0)
                AddButton(10, "Common.Button.West".Localized(), () => Move(2));

            if (_puzzle[_playerLoc + 1] != 1 && (_playerLoc + 1) % 9 != 0)
                AddButton(12, "Common.Button.East".Localized(), () => Move(3));

            if (CoC.Instance.Player.Intelligence >= 120 && CoC.Instance.Player.SpellMod() >= 2)
                AddButton(13, "Common.Button.BreakIllusion".Localized(), BreakIllusion);

            AddButton(0, "Common.Button.Reset".Localized(), () => Move(99));
        }

        public void Move(int direction)
        {
            _puzzle[_playerLoc] = 0;

            switch (direction)
            {
                case 0:
                    if (_mirrorLoc - 9 >= 0 && _mirror[_mirrorLoc - 9] != 1)
                    {
                        _mirror[_mirrorLoc] = 0;

                        _mirrorLoc -= 9;

                        _mirror[_mirrorLoc] = 3;
                    }

                    _playerLoc -= 9;

                    break;

                case 1:
                    if (_mirrorLoc + 9 < 81 && _mirror[_mirrorLoc + 9] != 1)
                    {
                        _mirror[_mirrorLoc] = 0;

                        _mirrorLoc += 9;

                        _mirror[_mirrorLoc] = 3;
                    }

                    _playerLoc += 9;

                    break;

                case 2:
                    if (_mirrorLoc % 9 != 0 && _mirrorLoc != 0 && _mirror[_mirrorLoc - 1] != 1)
                    {
                        _mirror[_mirrorLoc] = 0;

                        _mirrorLoc -= 1;

                        _mirror[_mirrorLoc] = 3;
                    }

                    _playerLoc -= 1;

                    break;

                case 3:
                    if ((_mirrorLoc + 1) % 9 != 0 && _mirror[_mirrorLoc + 1] != 1)
                    {
                        _mirror[_mirrorLoc] = 0;

                        _mirrorLoc += 1;

                        _mirror[_mirrorLoc] = 3;
                    }

                    _playerLoc += 1;

                    break;

                case 99:
                    _mirror[_mirrorLoc] = 0;

                    _playerLoc = 41;

                    _mirrorLoc = 41;

                    break;
            }

            _puzzle[_playerLoc] = 3;

            _mirror[_mirrorLoc] = 3;

            Draw();
        }

        public void BreakIllusion()
        {
            ClearOutput();

            ClearButtons();

            CoC.Instance.Flags.Set(Contents.Flags.GameFlagIds.WizardTowerProgress,

                CoC.Instance.Flags.Get(Contents.Flags.GameFlagIds.WizardTowerProgress)

                + WizardTowerRooms.SolvedMirrorPuzzleBit);

            OutputText("Scenes.WizardTower.EndPuzzle.AnalyzeEnvironment".Localized());

            OutputText("Scenes.WizardTower.EndPuzzle.LikeAPersonStuck".Localized());

            AddButton(0, "Common.Button.Next".Localized(), () => Go(_owner));
        }

        public void EndPuzzle()
        {
            ClearOutput();

            ClearButtons();

            CoC.Instance.Flags.Set(Contents.Flags.GameFlagIds.WizardTowerProgress,

                CoC.Instance.Flags.Get(Contents.Flags.GameFlagIds.WizardTowerProgress)

                + WizardTowerRooms.SolvedMirrorPuzzleBit);

            OutputText("Scenes.WizardTower.EndPuzzle.MirrorSelfReachesIts".Localized());

            OutputText("Scenes.WizardTower.EndPuzzle.AfterAMoment".Localized());

            OutputText("Scenes.WizardTower.EndPuzzle.BlinkOutOfExistence".Localized());

            AddButton(0, "Common.Button.Next".Localized(), () => Go(_owner));
        }
    }
}