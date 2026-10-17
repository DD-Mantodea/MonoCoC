using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<YaraPiercingStudio>("coc:yara_piercing")]
    public class YaraPiercingStudio : Scene
    {

        public bool inverse = false;

        public const int LOC_CLIT = 0;

        public const int LOC_DICK = 1;

        public const int LOC_EARS = 2;

        public const int LOC_EYEBROW = 3;

        public const int LOC_LIP = 4;

        public const int LOC_NIPPLES = 5;

        public const int LOC_NOSE = 6;

        public const int LOC_TONGUE = 7;

        public const int LOC_VULVA = 8;

        public const int MAT_AMETHYST = 1;

        public const int MAT_DIAMOND = 2;

        public const int MAT_GOLD = 3;

        public const int MAT_EMERALD = 4;

        public const int MAT_JADE = 5;

        public const int MAT_ONYX = 6;

        public const int MAT_RUBY = 7;

        public const int MAT_STEEL = 8;

        public const int MAT_LETHITE = 9;

        public const int MAT_FERRITE = 10;

        public const int MAT_FURRITE = 11;

        public const int MAT_CRIMSTONE = 12;

        public const int MAT_ICESTONE = 13;

        public const int TYPE_NONE = 0;

        public const int TYPE_STUD = 1;

        public const int TYPE_RING = 2;

        public const int TYPE_LADDER = 3;

        public const int TYPE_HOOP = 4;

        public const int TYPE_CHAIN = 5;

        public override void Entry()
        {
            PiercingStudio();
        }

        public void PiercingStudio()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Yarapiercingstudio.PiercingStudio.StudioInterior".Localized(
                 noFur ? "" : " You dully notice that unlike everyone else in this town, she's mostly human. Perhaps she came through a portal as well?"
            ));

            OutputText("Scenes.Yarapiercingstudio.PiercingStudio.IntroducesHerself".Localized(
                 Player.Mf("sir", "cutie")
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Pierce".Localized(), PierceMenu);

            AddButton(1, "Common.Button.Remove".Localized(), PiercingRemove);

            if (!Player.HasStatusEffect(StatusEffectIds.Yara))
                AddButton(2, "Common.Button.AboutHer".Localized(), AboutYara);

            AddButton(14, "Common.Button.Leave".Localized(), ReturnToPrevious);
        }

        public void AboutYara()
        {
            ClearOutput();

            Player.CreateStatusEffect(StatusEffectIds.Yara, 0, 0, 0, 0);

            OutputText("Scenes.Yarapiercingstudio.AboutYara.IntroducesSelf".Localized());

            if (Player.HumanScore() <= 2)
                OutputText("Scenes.Yarapiercingstudio.AboutYara.OnceHuman".Localized());
            else
                OutputText("Scenes.Yarapiercingstudio.AboutYara.FewHumans".Localized());

            OutputText("Scenes.Yarapiercingstudio.AboutYara.BlushesLittle".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 2f), new DynStat(GameStats.Lust, -5f), new DynStat(GameStats.Corruption, -1f));

            DoNext(() => PiercingStudio());
        }

        public bool Check(int toCheck)
        {

            if (inverse)
                return toCheck != 0;

            return toCheck == 0;
        }

        public bool BuildLocChoices(Action<int> fun, bool inverse = false)
        {

            var buttonsAdded = false;

            this.inverse = inverse;

            if ((Player.HasVagina()) && (Check(Player.Vaginas[0].ClitPierced)))
                {
                    AddButton(0, "Scenes.PlayerAppearance.Label.Clit".Localized(), () => fun(LOC_CLIT));

                    buttonsAdded = true;
                }

            if ((Player.Cocks.Count > 0) && (Check((int)(Player.Cocks[0].Pierced))))
                {
                    AddButton(1, "Common.Button.Dick".Localized(), () => fun(LOC_DICK));

                    buttonsAdded = true;
                }

            if (Check(Player.Body.Get<EarsPart>().Pierced))
            {
                AddButton(2, "Scenes.PlayerAppearance.Label.Ears".Localized(), () => fun(LOC_EARS));

                buttonsAdded = true;
            }

            if (Check(Player.Body.Get<FacePart>().EyebrowPierced))
            {
                AddButton(3, "Common.Button.Eyebrow".Localized(), () => fun(LOC_EYEBROW));

                buttonsAdded = true;
            }

            if (Check(Player.Body.Get<FacePart>().LipPierced))
            {
                AddButton(4, "Common.Button.Lip".Localized(), () => fun(LOC_LIP));

                buttonsAdded = true;
            }

            if (Check(Player.Body.Get<BreastRowPart>().NipplesPierced))
            {
                AddButton(5, "Common.Button.Nipples".Localized(), () => fun(LOC_NIPPLES));

                buttonsAdded = true;
            }

            if (Check(Player.Body.Get<FacePart>().NosePierced))
            {
                AddButton(6, "Common.Button.Nose".Localized(), () => fun(LOC_NOSE));

                buttonsAdded = true;
            }

            if (Check(Player.Body.Get<TonguePart>().Pierced))
            {
                AddButton(7, "Common.Button.Tongue".Localized(), () => fun(LOC_TONGUE));

                buttonsAdded = true;
            }

            if ((Player.HasVagina()) && (Check(Player.Vaginas[0].LabiaPierced)))
                {
                    AddButton(8, "Common.Button.Labia".Localized(), () => fun(LOC_VULVA));

                    buttonsAdded = true;
                }

            AddButton(14, "Common.Button.Back".Localized(), PiercingStudio);

            return buttonsAdded;
        }

        public void PierceMenu()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Yarapiercingstudio.PierceMenu.YaraAsks".Localized(
                 Player.Mf("sir", "cutie")
            ));

            if (!BuildLocChoices(ChooseLoc))
            {
                OutputText("Scenes.Yarapiercingstudio.PierceMenu.OnceOver".Localized());

                DoNext(() => PiercingStudio());
            }
        }

        public void ChooseLoc(int loc)
        {

            DoNext(() => PierceMenu());

            if (loc == LOC_DICK && !Player.HasCock())
            {
                OutputText("Scenes.Yarapiercingstudio.ChooseLoc.NoDick".Localized());

                return ;
            }

            if ((loc == LOC_CLIT || loc == LOC_VULVA) && !Player.HasVagina())
            {
                OutputText("Scenes.Yarapiercingstudio.ChooseLoc.PickSomethingElse".Localized((loc == LOC_CLIT ? "[pg]You realize you don't have a clit to pierce." : "[pg]You realize you don't have a pussy to pierce.")));

                return ;
            }

            switch (loc)
            {
                case LOC_CLIT:
                {
                OutputText("Scenes.Yarapiercingstudio.ChooseLoc.ThatsGonnaBe".Localized());
                }

                break;

                case LOC_DICK:
                {
                OutputText("Scenes.Yarapiercingstudio.ChooseLoc.YaraAsksHurt".Localized());
                }

                break;

                case LOC_EARS:
                {
                OutputText("Scenes.Yarapiercingstudio.ChooseLoc.AsksSuture".Localized());
                }

                break;

                case LOC_EYEBROW:
                {
                OutputText("Scenes.Yarapiercingstudio.ChooseLoc.AsksGoodLook".Localized());
                }

                break;

                case LOC_LIP:
                {
                OutputText("Scenes.Yarapiercingstudio.ChooseLoc.AsksHawt".Localized());
                }

                break;

                case LOC_NIPPLES:
                {
                OutputText("Scenes.Yarapiercingstudio.ChooseLoc.AdmitsSure".Localized());
                }

                break;

                case LOC_NOSE:
                {
                OutputText("Scenes.Yarapiercingstudio.ChooseLoc.WrinklesNose".Localized());
                }

                break;

                case LOC_TONGUE:
                {
                OutputText("Scenes.Yarapiercingstudio.ChooseLoc.PurrsHappy".Localized());
                }

                break;

                case LOC_VULVA:
                {
                OutputText("Scenes.Yarapiercingstudio.ChooseLoc.YaraExplains".Localized());
                }

                break;
            }

            ClearButtons();

            AddButton(0, "Common.Button.Stud".Localized(), () => ChooseMaterials(loc, TYPE_STUD));

            if (loc != LOC_TONGUE)
                AddButton(1, "Common.Button.Ring".Localized(), () => ChooseMaterials(loc, TYPE_RING));

            if (loc == LOC_DICK)
                AddButton(2, "Common.Button.Ladder".Localized(), () => ChooseMaterials(loc, TYPE_LADDER));

            if (loc == LOC_EARS)
                AddButton(3, "Common.Button.Hoop".Localized(), () => ChooseMaterials(loc, TYPE_HOOP));

            if (loc == LOC_NIPPLES)
                AddButton(4, "Common.Button.Chain".Localized(), () => ChooseMaterials(loc, TYPE_CHAIN));
        }

        public void ChooseMaterials(int loc, int type)
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Yarapiercingstudio.ChooseMaterials.GathersMaterials".Localized());

            if (Player.Gems < 100)
            {
                OutputText("Scenes.Yarapiercingstudio.ChooseMaterials.NotEnough".Localized());

                DoNext(() => PiercingStudio());

                return ;
            }

            ClearButtons();

            AddButton(0, "Common.Button.Amethyst".Localized(), () => AreYouSure(loc, type, MAT_AMETHYST));

            AddButton(1, "Common.Button.Diamond".Localized(), () => AreYouSure(loc, type, MAT_DIAMOND));

            AddButton(2, "Common.Button.Gold".Localized(), () => AreYouSure(loc, type, MAT_GOLD));

            AddButton(3, "Common.Button.Emerald".Localized(), () => AreYouSure(loc, type, MAT_EMERALD));

            AddButton(4, "Common.Button.Jade".Localized(), () => AreYouSure(loc, type, MAT_JADE));

            AddButton(5, "Common.Button.Onyx".Localized(), () => AreYouSure(loc, type, MAT_ONYX));

            AddButton(6, "Common.Button.Ruby".Localized(), () => AreYouSure(loc, type, MAT_RUBY));

            AddButton(7, "Common.Button.Steel".Localized(), () => AreYouSure(loc, type, MAT_STEEL));

            if (Player.Gems >= 1000)
                AddButton(8, "Common.Button.RareMenu".Localized(), () => ChooseAdvancedMaterials(loc, type));
            else
                AddButton(8, "Common.Button.RareMenu".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), PiercingStudio);
        }

        public void AreYouSure(int loc, int type, int mat)
        {
            ClearOutput();

            OutputText("Scenes.Yarapiercingstudio.AreYouSure.LastChance".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => NormalPierceAssemble(loc, type, mat));

            AddButton(1, "Common.Button.No".Localized(), PiercingStudio);
        }

        public void ChooseAdvancedMaterials(int loc, int type)
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Yarapiercingstudio.ChooseAdvancedMaterials.GoesBack".Localized());

            OutputText("Scenes.Yarapiercingstudio.ChooseAdvancedMaterials.MaterialsListed".Localized());

            OutputText("Scenes.Yarapiercingstudio.ChooseAdvancedMaterials.LethiteInfo".Localized());

            OutputText("Scenes.Yarapiercingstudio.ChooseAdvancedMaterials.FertiteInfo".Localized());

            OutputText("Scenes.Yarapiercingstudio.ChooseAdvancedMaterials.FurriteInfo".Localized());

            OutputText("Scenes.Yarapiercingstudio.ChooseAdvancedMaterials.CrimstoneInfo".Localized());

            OutputText("Scenes.Yarapiercingstudio.ChooseAdvancedMaterials.IcestoneInfo".Localized());

            OutputText("Scenes.Yarapiercingstudio.ChooseAdvancedMaterials.DisclaimerBanner".Localized());

            AddButton(0, "Common.Button.Lethite".Localized(), () => AreYouSure(loc, type, MAT_LETHITE));

            AddButton(1, "Common.Button.Fertite".Localized(), () => AreYouSure(loc, type, MAT_FERRITE));

            AddButton(2, "Common.Button.Furrite".Localized(), () => AreYouSure(loc, type, MAT_FURRITE));

            AddButton(3, "Common.Button.Crimstone".Localized(), () => AreYouSure(loc, type, MAT_CRIMSTONE));

            if (Player.Gems >= 2000)
                AddButton(4, "Common.Button.Icestone".Localized(), () => AreYouSure(loc, type, MAT_ICESTONE));

            AddButton(14, "Common.Button.Back".Localized(), PiercingStudio);
        }

        public void NormalPierceAssemble(int loc, int type, int mat)
        {
            ClearOutput();

            OutputText("Scenes.Yarapiercingstudio.NormalPierceAssemble.MakesComfortable".Localized());

            var shortP = "";

            var longP = "";

            Player.Gems -= 100;

            if (mat > 8)
                Player.Gems -= 900;

            if (mat == 13)
                Player.Gems -= 1000;

            switch (mat)
            {
                case MAT_AMETHYST:
                {
                shortP += "amethyst ";

                Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f), new DynStat(GameStats.Libido, 1f));

                longP += "Amethyst ";
                }

                break;

                case MAT_DIAMOND:
                {
                shortP += "diamond ";

                Player.ApplyStats(new DynStat(GameStats.Intelligence, 2f), new DynStat(GameStats.Corruption, -1f));

                longP += "Diamond ";
                }

                break;

                case MAT_GOLD:
                {
                shortP += "gold ";

                Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f), new DynStat(GameStats.Sensitivity, 1f));

                longP += "Gold ";
                }

                break;

                case MAT_EMERALD:
                {
                shortP += "emerald ";

                Player.ApplyStats(new DynStat(GameStats.Speed, 1f));

                longP += "Emerald ";
                }

                break;

                case MAT_JADE:
                {
                shortP += "jade ";

                Player.ApplyStats(new DynStat(GameStats.Toughness, -.5f), new DynStat(GameStats.Intelligence, 1f), new DynStat(GameStats.Corruption, -1f));

                longP += "Jade ";
                }

                break;

                case MAT_ONYX:
                {
                shortP += "onyx ";

                Player.ApplyStats(new DynStat(GameStats.Toughness, 1f), new DynStat(GameStats.Speed, -1f));

                longP += "Onyx ";
                }

                break;

                case MAT_RUBY:
                {
                shortP += "ruby ";

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

                longP += "Ruby ";
                }

                break;

                case MAT_STEEL:
                {
                shortP += "steel ";

                Player.ApplyStats(new DynStat(GameStats.Strength, 2f), new DynStat(GameStats.Intelligence, -2f));

                longP += "Steel ";
                }

                break;

                case MAT_LETHITE:
                {
                shortP += "lethite ";

                if (!Player.HasPerk(PerkIds.PiercedLethite))
                    Player.CreatePerk(PerkIds.PiercedLethite, 0, 0, 0, 0);

                longP += "Lethite ";
                }

                break;

                case MAT_FERRITE:
                {
                shortP += "fertite ";

                if (!Player.HasPerk(PerkIds.PiercedFertite))
                    Player.CreatePerk(PerkIds.PiercedFertite, 5, 0, 0, 0);
                else
                    Player.AddPerkValue(PerkIds.PiercedFertite, 1, 5);

                longP += "Fertite ";
                }

                break;

                case MAT_FURRITE:
                {
                shortP += "furrite ";

                if (!Player.HasPerk(PerkIds.PiercedFurrite))
                    Player.CreatePerk(PerkIds.PiercedFurrite, 0, 0, 0, 0);

                longP += "Furrite ";
                }

                break;

                case MAT_CRIMSTONE:
                {
                shortP += "crimstone ";

                if (Player.HasPerk(PerkIds.PiercedIcestone))
                {
                    Player.AddPerkValue(PerkIds.PiercedIcestone, 1, -5);

                    if (Player.Perkv1(PerkIds.PiercedIcestone) <= 0)
                        Player.RemovePerk(PerkIds.PiercedIcestone);
                }
                else
                    if (Player.PerkValue(PerkIds.PiercedCrimstone) <= 0)
                        Player.CreatePerk(PerkIds.PiercedCrimstone, 5, 0, 0, 0);
                    else
                        Player.AddPerkValue(PerkIds.PiercedCrimstone, 1, 5);

                longP += "Crimstone ";
                }

                break;

                case MAT_ICESTONE:
                {
                shortP += "icestone ";

                if (Player.HasPerk(PerkIds.PiercedCrimstone))
                {
                    Player.AddPerkValue(PerkIds.PiercedCrimstone, 1, -5);

                    if (Player.Perkv1(PerkIds.PiercedCrimstone) <= 0)
                        Player.RemovePerk(PerkIds.PiercedCrimstone);
                }
                else
                    if (Player.PerkValue(PerkIds.PiercedIcestone) <= 0)
                        Player.CreatePerk(PerkIds.PiercedIcestone, 5, 0, 0, 0);
                    else
                        Player.AddPerkValue(PerkIds.PiercedIcestone, 1, 5);

                longP += "Icestone ";
                }

                break;
            }

            switch (loc)
            {
                case LOC_CLIT:
                {
                shortP += "clit-";

                longP += "clit-";

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));
                }

                break;

                case LOC_DICK:
                {
                if (type != 3)
                {
                    shortP += "cock-";

                    longP += "cock-";

                    Player.ApplyStats(new DynStat(GameStats.Libido, 2f));
                }
                }

                break;

                case LOC_EARS:
                {
                shortP += "ear";

                longP += "ear";
                }

                break;

                case LOC_EYEBROW:
                {
                Player.ApplyStats(new DynStat(GameStats.Toughness, -.5f));

                shortP += "eyebrow-";

                longP += "eyebrow-";
                }

                break;

                case LOC_LIP:
                {
                Player.ApplyStats(new DynStat(GameStats.Toughness, -.5f));

                shortP += "lip-";

                longP += "lip-";
                }

                break;

                case LOC_NIPPLES:
                {
                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

                shortP += "nipple-";

                longP += "nipple-";
                }

                break;

                case LOC_NOSE:
                {
                Player.ApplyStats(new DynStat(GameStats.Strength, .5f));

                shortP += "nose-";

                longP += "nose-";
                }

                break;

                case LOC_TONGUE:
                {
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

                shortP += "tongue-";

                longP += "tongue-";
                }

                break;

                case LOC_VULVA:
                {
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

                shortP += "labia-";

                longP += "labia-";
                }

                break;
            }

            switch (type)
            {
                case TYPE_STUD:
                {
                if (loc == 2 || loc == 5 || loc == 8)
                {
                    shortP += "studs";

                    longP += "studs";
                }
                else
                {
                    shortP += "stud";

                    longP += "stud";
                }
                }

                break;

                case TYPE_RING:
                {
                if (loc == 2 || loc == 5 || loc == 8)
                {
                    shortP += "rings";

                    longP += "rings";
                }
                else
                {
                    shortP += "ring";

                    longP += "ring";
                }
                }

                break;

                case TYPE_LADDER:
                {
                shortP += "jacob's ladder";

                longP += "jacob's ladder";
                }

                break;

                case TYPE_HOOP:
                {
                if (loc == 2 || loc == 5 || loc == 8)
                {
                    shortP += "hoops";

                    longP += "hoops";
                }
                else
                {
                    shortP += "hoop";

                    longP += "hoop";
                }
                }

                break;

                case TYPE_CHAIN:
                {
                shortP += "chain";

                longP += "chain";
                }

                break;
            }

            ApplyPiercing(loc, type, shortP, longP);

            if (loc == 8 || loc == 0)
            {
                YaraSex(true);

                return ;
            }
            else
                if (loc == 1 && (Player.HasCockThatFits(36) || GameSettings.Hyper))
                {
                    YaraSex(false);

                    return ;
                }

            DoNext(() => PiercingStudio());
        }

        public void ApplyPiercing(int loc, int type = TYPE_NONE, string shortP = "", string longP = "")
        {

            switch (loc)
            {
                case LOC_CLIT:
                {
                Player.Vaginas[0].ClitPierced = type;

                Player.Vaginas[0].ClitPShort = shortP;

                Player.Vaginas[0].ClitPLong = longP;
                }

                break;

                case LOC_DICK:
                {
                Player.Cocks[0].Pierced = type;

                Player.Cocks[0].PShortDesc = shortP;

                Player.Cocks[0].PLongDesc = longP;
                }

                break;

                case LOC_EARS:
                {
                Player.Body.Get<EarsPart>().Pierced = type;

                Player.EarsPShort = shortP;
                }

                break;

                case LOC_EYEBROW:
                {
                Player.Body.Get<FacePart>().EyebrowPierced = type;

                Player.EyebrowPShort = shortP;

                Player.EyebrowPLong = longP;
                }

                break;

                case LOC_LIP:
                {
                Player.Body.Get<FacePart>().LipPierced = type;

                Player.LipPShort = shortP;

                Player.LipPLong = longP;
                }

                break;

                case LOC_NIPPLES:
                {
                Player.Body.Get<BreastRowPart>().NipplesPierced = type;

                Player.NipplesPShort = shortP;

                Player.NipplesPLong = longP;
                }

                break;

                case LOC_NOSE:
                {
                Player.Body.Get<FacePart>().NosePierced = type;

                Player.NosePShort = shortP;

                Player.NosePLong = longP;
                }

                break;

                case LOC_TONGUE:
                {
                Player.Body.Get<TonguePart>().Pierced = type;

                Player.TonguePShort = shortP;

                Player.TonguePLong = longP;
                }

                break;

                case LOC_VULVA:
                {
                Player.Vaginas[0].LabiaPierced = type;

                Player.Vaginas[0].LabiaPShort = shortP;

                Player.Vaginas[0].LabiaPLong = longP;
                }

                break;
            }
        }

        public void PiercingRemove()
        {
            ClearOutput();

            if (!BuildLocChoices(DoRemove, true))
            {
                OutputText("Scenes.Yarapiercingstudio.PiercingRemove.GigglesTease".Localized());

                DoNext(() => PiercingStudio());

                return ;
            }

            OutputText("Scenes.Yarapiercingstudio.PiercingRemove.AsksReally".Localized());

            if (Player.Gems < 100)
            {
                OutputText("Scenes.Yarapiercingstudio.PiercingRemove.NotEnoughGems".Localized());

                DoNext(() => PiercingStudio());

                return ;
            }

            if (Player.Toughness <= 5.5f)
            {
                OutputText("Scenes.Yarapiercingstudio.PiercingRemove.LooksUpDown".Localized());

                DoNext(() => PiercingStudio());
            }
        }

        public void DoRemove(int loc)
        {
            ClearOutput();

            OutputText("Scenes.Yarapiercingstudio.DoRemove.GivesDrink".Localized());

            ApplyPiercing(loc);

            Player.ApplyStats(new DynStat(GameStats.Toughness, -5f));

            Player.Gems -= 100;

            DoNext(() => PiercingStudio());
        }

        public void YaraSex(bool girl)
        {
            ClearOutput();

            OutputText("Scenes.Yarapiercingstudio.YaraSex.MakesComfortable".Localized());
            // 原版 YaraPiercingStudio.hx:604 — CeraphScene.hx：已解锁 Ceraph 的露出剧情
            if (SceneInstance<Npcs.CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Yarapiercingstudio.YaraSex.GuiltyThrill".Localized());

            OutputText("Scenes.Yarapiercingstudio.YaraSex.CommandsSoftly".Localized());

            OutputText("Scenes.Yarapiercingstudio.YaraSex.IntentLoving".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TurnDown".Localized(), PiercingStudio);

            AddButton(1, "Common.Button.OhYeah".Localized(), () => LetsDoYaraSex(girl));
        }

        public void LetsDoYaraSex(bool girl = true)
        {
            ClearOutput();

            int x = Player.CockThatFits(36);

            if (GameSettings.Hyper)
                x = Player.CockThatFits(50000);
            else
                if ((x == -1) && !girl)
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.Crestfallen".Localized());

                        girl = true;
                    }
                    else
                    {
                        OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.CrestfallenAgain".Localized());

                        return ;
                    }

            OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.EyesWiden".Localized());

            if (Player.Lust < 50)
                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.ProtestWord".Localized());
            else
                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.SpeakWord".Localized());

            OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.NeatlySilencing".Localized());

            OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.HandSlides".Localized());

            if (!girl && Player.HasCock())
                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.RisesAttention".Localized(
                     Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.PuddleLove".Localized());

            OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.ParagraphBreak".Localized());

            OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.LeansForward".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.YourChest".Localized());
            else
                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.YourNipples".Localized());

            OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.SmoochBelly".Localized());

            OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.NotContentPenetration".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.DexterousFingertips".Localized());

            OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.ScootsForward".Localized());

            if (!girl && Player.HasCock())
            {
                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.PursedLips".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.ReleasesHer".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.TakesControl".Localized());

                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.BeforeRespond".Localized());

                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.PowerfulMotion".Localized());

                if (Player.CumQ() < 500)
                {
                    OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.FrameSpasms".Localized());

                    OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.DismountsDick".Localized());
                }
                else
                    if (Player.CumQ() <= 1500)
                    {
                        OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.BellyPlumps".Localized());

                        OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.LiftsOff".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.EcstaticMoans".Localized());

                        OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.NobleAttempt".Localized());
                    }

                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.ConcludesRedress".Localized());

                if (Player.CumQ() > 1500)
                    OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.TriesToFit".Localized());

                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.ParagraphBreakTwo".Localized());
            }
            else
            {
                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.DuoForefingers".Localized());

                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.ParsingScene".Localized());

                if (Player.Wetness() >= 3)
                    OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.LickingMelting".Localized());
                else
                    OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.DryPerimeter".Localized());

                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.NotOutdone".Localized());

                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.LooksUpDown".Localized());

                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.ContortionistWobbly".Localized());

                OutputText("Scenes.Yarapiercingstudio.LetsDoYaraSex.HeadStopsShort".Localized());
            }

            Player.Orgasm(girl ? "Vaginal" : "Dick");

            DoNext(() => PiercingStudio());
        }

    }
}