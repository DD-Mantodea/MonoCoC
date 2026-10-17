using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    [Register<GretasGarmentsScene>("coc:gretas_garments")]
    public class GretasGarmentsScene : Scene
    {

        public static readonly CockSockEntry[] cockSockDb =

        [
            new("Wool", "wool", 10, "You spy a thick, woolen sock sitting on a counter and take it up to Greta. [say: Ah, yes. That's our basic sock. Warm and cozy, great for those chilly nights. That one's a mere 10 gems. A steal, of course.]"),

            new("Cockring", "cockring", 100, "You pick up one sock, surprised to find how heavy it is. Large metal rings encircle the base of the smooth cock-sock, with one loose ring dangling down, no doubt intending to wrap around the base of your ball sack. [say: Oh yes, that's a fun one. Those rings will constantly constrict your manhood, so you'll always be hard and ready to go.] She giggles and waves a hand, [say: That's actually a very popular sock... so many demons come in to get these for their harems. It's 100 gems.]"),

            new("Alabaster", "alabaster", 25, "You pick up a sock and inspect it. It's a pure white cock sock, edged with delicate lace. It almost appears to be some kind of bridal wear... although you don't know of any kind of bride that would wear something like this. [say: Ah yeah, that's a popular one. Some folks like the purity that it suggests... though I can't guess why. It's 25 gems, though.]"),

            new("Viridian", "viridian", 1000, "You pick up one sock and inspect it. The whole thing is a rich, natural green color and completely lace, accentuated with vivid red roses. Just touching it makes you feel healthier and more alive. [say: Ahh, that's a fun one right there. It hastens your natural healing. Very useful, and pretty, too, if I say so myself. It's 1,000 gems.] You pale at the price, but Greta waves a hand, [say: Trust me, honey, it's worth it.]"),

            new("Scarlet", "scarlet", 250, "You pick up one sock and inspect it. It's an incredible plush red, and made of soft satin and detailed with red lace. It seems smaller than the other socks you've seen, and you can't help but wonder how tight it will feel on your dick. [say: Mmm, that one's special. It increases the blood flow to your little dick, enabling it to grow a lot faster. This one goes quick. Everyone wants to be a minotaur! It's 250 gems.]"),

            new("Cobalt", "cobalt", 250, "You pick up one sock and inspect it. It's a cool, soft blue color, made from satin and detailed in light blue lace. It seems extremely small, compared to the other socks in the shop, and you can't help but think it must be extremely uncomfortable to wear. [say: Oho, that's a fun one right there. The cute little femboys go crazy for it. As you can see, it's a bit small, and it will actually inhibit your cock from growing too big. It's 250 gems.]"),

            new("Purple", "amaranthine", 1000, "You pick up one sock and inspect it. It's a silky smooth lavish purple color, with fine lace depicting some kind of six-legged wolf-like creature. Overall, though, the sock is an odd shape, seemingly intended for someone with a knot AND some kind of equine-like flare. Greta's eyebrows raise as she sees the item you're holding, [say: Ohh, that one. That, honey, was an experiment. I took some magic channeled down from the stars themselves and infused it into a new sock, and that was the result. Truth be told, I'm not entirely sure what it does, but I'll sell it to you for 1,000 gems.]"),

            new("Gilded", "gilded", 3000, "You pick up one sock and inspect it, surprised to see how rigid and heavy it is. Unlike the others in the shop, this one seems to be made of a cool golden metallic material. Glittering gems are embedded into the top side, while the bottom is cinched closed with leather cords. [say: You've got a good eye,] Greta says, her eyes twinkling greedily. [say: With that bad boy, you can actually convert some of your... sweet cum into even sweeter gems. Of course, with that kind of awesome power, you've got to understand that it'll cost you 3,000 gems.]"),

            new("Mtl. Green", "green", 500, "You pick up one sock and inspect it. It's a dark, metallic green in color and interlaced with brighter green highlights. Greta's eyebrows raise as she sees the item you're holding, [say: Ohh, that one. It's part of my newest lineup of cock-socks. If you're unsure of your endurance, this might help as the magic bolsters your overall health capacity. I'll sell it to you for 500 gems.]"),

            new("Mtl. Red", "red", 500, "You pick up one sock and inspect it. It's a dark, metallic red in color and interlaced with brighter red highlights. Overall, it looks a bit menacing. Greta's eyebrows raise as she sees the item you're holding, [say: Ohh, that one. It's part of my newest lineup of cock-socks. This cock-sock will enhance your power so you can physically beat tougher opponents. I'll sell it to you for 500 gems.]"),

            new("Mtl. Blue", "blue", 500, "You pick up one sock and inspect it. It's a dark, metallic blue in color and interlaced with brighter blue highlights that seems to glow in the dark. Greta's eyebrows raise as she sees the item you're holding, [say: Ohh, that one. It's part of my newest lineup of cock-socks. This cock-sock will enhance your spellpower. I'll sell it to you for 500 gems.]"),
        ];

        public static SaveContent Save => SaveContent(SceneIds.Bazaar.GretasGarments);

        public override void Entry()
        {
            GretasGarments();
        }

        public void GretasGarments()
        {
            ClearOutput();

            var seaShirt = Undergarments.SEA_SHIRT;

            if (CoC.Instance.Flags.Get(GameFlagIds.FoundSocks) == 1 && CoC.Instance.Flags.Get(GameFlagIds.SockCounter) == 0)
            {
                OutputText("Scenes.GretasGarments.GretasGarments.EntersStore".Localized());

                OutputText("Scenes.GretasGarments.GretasGarments.GretaSpies".Localized());

                OutputText("Scenes.GretasGarments.GretasGarments.ClicksTongue".Localized());

                Player.UnlockCodexEntry(GameFlagIds.CodexEntrySuccubus);

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), GretasGarments);

                CoC.Instance.Flags.Set(GameFlagIds.FoundSocks, 2);

                return ;
            }

            if (seaShirt.Save.GetBool("gretaBoughtUnbind") && View.Time.Days > seaShirt.Save.GetInt("gretaBoughtUnbindDay", 0))
            {
                RegisterMacro("dispell", seaShirt.Save.GetInt("gretaDidDispell", 0) != 0);

                OutputText("Scenes.GretasGarments.GretasGarments.ShopInterior".Localized());

                OutputText("Scenes.GretasGarments.GretasGarments.HoldsGarment".Localized());

                OutputText("Scenes.GretasGarments.GretasGarments.TakesGarment".Localized());

                if (seaShirt.Save.GetBool("gretaDidDispell"))
                    seaShirt.Save.SetBool("enchanted", false);

                seaShirt.Save.SetBool("bound", false);

                seaShirt.Save.SetBool("gretaBoughtUnbind", false);

                InventoryScene.TakeItem(seaShirt, GretasGarments);

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.FoundSocks) < 2)
            {
                OutputText("Scenes.GretasGarments.GretasGarments.InteriorDescribed".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.FoundSocks) == 0)
                    CoC.Instance.Flags.Set(GameFlagIds.FoundSocks, 0.5f);
            }
            else
                OutputText("Scenes.GretasGarments.GretasGarments.InteriorAgain".Localized());

            OutputText("Scenes.GretasGarments.GretasGarments.SitsUp".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntrySuccubus);

            if (CoC.Instance.Flags.Get(GameFlagIds.OwnMaidenBikini) == 0)
                OutputText("Scenes.GretasGarments.GretasGarments.SkimpyChain".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 2f));

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.FoundSocks) < 2)
                AddButton(0, "Common.Button.LowStock".Localized(), AskGretaAboutInventory);
            else
            {
                AddNextButton("Common.Button.BrowseSocks".Localized(), BrowseDemSocksSon);

                AddNextButton("Common.Button.RemoveSock".Localized(), TakeOffDatSock);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.OwnMaidenBikini) == 0)
                AddRowButton(1, "Bikini", AskGretaAboutZeBikini);

            if (Player.UpperGarment == seaShirt && seaShirt.knownBound())
                AddRowButton(2, "Cursed Shirt", AskGretaAboutNobleShirt);
            else
                if (Player.HasItem(seaShirt) && seaShirt.Save.GetBool("unequipTried") && !seaShirt.IsNormalShirt() && !seaShirt.Save.GetBool("gretaBoughtUnbind"))
                    AddRowButton(2, "Disenchant Shirt", AskGretaToUnbind);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go<BazaarScene>());
        }

        public void AskGretaAboutInventory()
        {
            ClearOutput();

            OutputText("Scenes.GretasGarments.AskGretaAboutInventory.CuriousNoStock".Localized());

            OutputText("Scenes.GretasGarments.AskGretaAboutInventory.GretaDescribed".Localized());

            OutputText("Scenes.GretasGarments.AskGretaAboutInventory.SeeSomething".Localized());

            OutputText("Scenes.GretasGarments.AskGretaAboutInventory.LooksStore".Localized());

            OutputText("Scenes.GretasGarments.AskGretaAboutInventory.AsksSocks".Localized());

            OutputText("Scenes.GretasGarments.AskGretaAboutInventory.LaughsHard".Localized());

            OutputText("Scenes.GretasGarments.AskGretaAboutInventory.BackOrder".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FoundSocks) < 1)
                CoC.Instance.Flags.Set(GameFlagIds.FoundSocks, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.SockCounter) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.SockCounter, 24);

            DoNext(() => GretasGarments());
        }

        public void AskGretaAboutZeBikini()
        {
            ClearOutput();

            OutputText("Scenes.GretasGarments.AskGretaAboutZeBikini.GretaAsksBikini".Localized());

            OutputText("Scenes.GretasGarments.AskGretaAboutZeBikini.SetsDownItem".Localized());

            OutputText("Scenes.GretasGarments.AskGretaAboutZeBikini.ShrugsReply".Localized());

            OutputText("Scenes.GretasGarments.AskGretaAboutZeBikini.BlowsLock".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.BuyBikini".Localized(), BuyGretasBikini);

            AddButton(1, "Common.Button.NoThanks".Localized(), GretasGarments);
        }

        public void BuyGretasBikini()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.OwnMaidenBikini, 1);

            Player.Gems -= 500;

            OutputText("Scenes.GretasGarments.BuyGretasBikini.EyesWiden".Localized());

            OutputText("Scenes.GretasGarments.BuyGretasBikini.PaysBikini".Localized());

            OutputText("Scenes.GretasGarments.BuyGretasBikini.ThanksSugar".Localized());

            OutputText("Scenes.GretasGarments.BuyGretasBikini.KeepInMind".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.LightMaidenArmor), 1);

GretasGarments();
        }

        public void BrowseDemSocksSon()
        {
            ClearOutput();

            OutputText("Scenes.GretasGarments.BrowseDemSocksSon.SockTypePrompt".Localized());

            ClearButtons();

            foreach (var i in Enumerable.Range(0, cockSockDb.Length))
            {
                var data = cockSockDb[i];

                AddButton(i, data.buttonText, () => CockSelectionMenu(data));
            }

            AddButton(14, "Common.Button.Back".Localized(), GretasGarments);
        }

        public void CockSelectionMenu(CockSockEntry selection)
        {
            ClearOutput();

            OutputText(selection.description);

            ClearButtons();

            if (Player.Gems >= selection.price)
                AddButton(0, "Common.Button.Buy".Localized(), () => PickACockForSock(selection));
            else
                OutputText("Scenes.GretasGarments.CockSelectionMenu.CantAffordBanner".Localized());

            AddButton(14, "Common.Button.Back".Localized(), BrowseDemSocksSon);
        }

        public void PickACockForSock(CockSockEntry selection)
        {
            ClearOutput();

            OutputText("Scenes.GretasGarments.PickACockForSock.TakesSock".Localized());

            ClearButtons();

            if (Player.CockTotal() == 1)
                AddButton(0, "Common.Button.Next".Localized(), () => CockSockTarget(0, selection));
            else
            {
                OutputText("Scenes.GretasGarments.PickACockForSock.WhichCock".Localized());

                foreach (var i in Enumerable.Range(0, Player.CockTotal()))
                    if (Player.Cocks[i].Sock == "")
                        AddNextButton((i + 1).ToString(), () => CockSockTarget(i, selection));
            }
        }

        public void CockSockTarget(int target, CockSockEntry selection)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.SocksBought, CoC.Instance.Flags.Get(GameFlagIds.SocksBought) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.SocksBought) == 1)
            {
                OutputText("Scenes.GretasGarments.CockSockTarget.GesturesAt".Localized());

                OutputText("Scenes.GretasGarments.CockSockTarget.CacklesLightly".Localized());

                OutputText("Scenes.GretasGarments.CockSockTarget.BalksLittle".Localized());

                OutputText("Scenes.GretasGarments.CockSockTarget.CalmlyExplains".Localized());

                OutputText("Scenes.GretasGarments.CockSockTarget.GulpsPrompt".Localized());
            }
            else
            {
                OutputText("Scenes.GretasGarments.CockSockTarget.MotionsHand".Localized());

                OutputText("Scenes.GretasGarments.CockSockTarget.WellPrompt".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => YesPutDatSockOnMe(target, selection));

            AddButton(1, "Common.Button.No".Localized(), NoCockSock);
        }

        public void YesPutDatSockOnMe(int target, CockSockEntry selection)
        {
            ClearOutput();

            var conflict = false;

            switch (selection.Id)
            {
                case "cockring":
                {
                if (!Player.HasPerk(PerkIds.PentUp))
                    Player.CreatePerk(PerkIds.PentUp, 10, 0, 0, 0);
                else
                    Player.AddPerkValue(PerkIds.PentUp, 1, 5);
                }

                break;

                case "viridian":
                {
                if (!Player.HasPerk(PerkIds.LustyRegeneration))
                    Player.CreatePerk(PerkIds.LustyRegeneration, 0, 0, 0, 0);
                else
                    conflict = true;
                }

                break;

                case "scarlet":
                {
                if (!Player.HasPerk(PerkIds.PhallicPotential))
                    Player.CreatePerk(PerkIds.PhallicPotential, 0, 0, 0, 0);
                }

                break;

                case "cobalt":
                {
                if (!Player.HasPerk(PerkIds.PhallicRestraint))
                    Player.CreatePerk(PerkIds.PhallicRestraint, 0, 0, 0, 0);
                }

                break;

                case "gilded":
                {
                if (!Player.HasPerk(PerkIds.PentUp))
                    Player.CreatePerk(PerkIds.MidasCock, 5, 0, 0, 0);
                else
                    Player.AddPerkValue(PerkIds.MidasCock, 1, 5);
                }

                break;

                case "blue":
                {
                if (!Player.HasPerk(PerkIds.PhallicPower))
                    Player.CreatePerk(PerkIds.PhallicPower, 0, 0, 0, 0);
                }

                break;
            }

            Player.Gems -= selection.price;

            OutputText("Scenes.GretasGarments.YesPutDatSockOnMe.NodsAgreement".Localized(
                 Player.CockDescript(target)
            ));

            if (!conflict)
            {
                Player.Cocks[target].sock = selection.Id;

                OutputText("Scenes.GretasGarments.YesPutDatSockOnMe.LiftsCock".Localized());

                OutputText("Scenes.GretasGarments.YesPutDatSockOnMe.ThumbApproaches".Localized(
                     Player.CockHead(target)
                ));

                OutputText("Scenes.GretasGarments.YesPutDatSockOnMe.SmilesKnowingly".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 5f), new DynStat(GameStats.Corruption, 2f));

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), GretasGarments);
            }
            else
            {
                OutputText("Scenes.GretasGarments.YesPutDatSockOnMe.SuddenlyStops".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), GretasGarments);
            }
        }

        public void NoCockSock()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.SockHolding, "");

            OutputText("Scenes.GretasGarments.NoCockSock.ShakesHead".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GretasGarments);
        }

        public void TakeOffDatSock()
        {
            ClearOutput();

            OutputText("Scenes.GretasGarments.TakeOffDatSock.WhichSockRemove".Localized());

            ClearButtons();

            var i = 0;

            while (i < Player.CockTotal())
            {
                if (Player.Cocks[i].Sock != "")
                    AddNextButton((i + 1).ToString(), () => RemoveTargettedSock(i));

                i += 1;
            }

            AddButton(14, "Scenes.Options.Cancel".Localized(), GretasGarments);
        }

        public void RemoveTargettedSock(int index)
        {
            ClearOutput();

            OutputText("Scenes.GretasGarments.RemoveTargettedSock.WalksCounter".Localized());

            OutputText("Scenes.GretasGarments.RemoveTargettedSock.SaysSmoothly".Localized(
                 Player.CockDescript(index),

                 Player.CockHead(index)
            ));

            OutputText("Scenes.GretasGarments.RemoveTargettedSock.WhiteHot".Localized());

            if (Player.CockArea(index) >= 100)
                OutputText("Scenes.GretasGarments.RemoveTargettedSock.AlmostKnocks".Localized());

            OutputText("Scenes.GretasGarments.RemoveTargettedSock.UnforgivingTight".Localized());

            OutputText("Scenes.GretasGarments.RemoveTargettedSock.DickDeflates".Localized());

            var storage = Player.Cocks[index].sock;

            var extra = false;

            Player.Cocks[index].sock = "";

            var temp = Player.CockTotal();

            while (temp > 0)
            {
                temp--;

                if (Player.Cocks[temp].sock == storage)
                    extra = true;
            }

            if (extra)
            {
                if (storage == "cockring")
                    Player.SetPerkValue(PerkIds.PentUp, 1, 5 + (Player.CountCockSocks("cockring") * 5));

                if (storage == "gilded")
                    Player.SetPerkValue(PerkIds.MidasCock, 1, Player.CountCockSocks("gilded") * 5);
            }
            else
            {
                if (storage == "gilded")
                    Player.RemovePerk(PerkIds.MidasCock);

                if (storage == "cobalt")
                    Player.RemovePerk(PerkIds.PhallicRestraint);

                if (storage == "scarlet")
                    Player.RemovePerk(PerkIds.PhallicPotential);

                if (storage == "viridian")
                    Player.RemovePerk(PerkIds.LustyRegeneration);

                if (storage == "cockring")
                    Player.RemovePerk(PerkIds.PentUp);
            }

            OutputText("Scenes.GretasGarments.RemoveTargettedSock.NeedAnother".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -10f), new DynStat(GameStats.Corruption, 1f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GretasGarments);
        }

        public void AskGretaAboutNobleShirt()
        {
            ClearOutput();

            OutputText("Scenes.GretasGarments.AskGretaAboutNobleShirt.AddressesSeamstress".Localized());

            OutputText("Scenes.GretasGarments.AskGretaAboutNobleShirt.MightBeRude".Localized());

            if (Undergarments.SEA_SHIRT.Save.GetInt("armorStage", 0) > 2)
            {
                OutputText("Scenes.GretasGarments.AskGretaAboutNobleShirt.GlowingHands".Localized());

                DoNext(() => GretasGarments());
            }
            else
            {
                OutputText("Scenes.GretasGarments.AskGretaAboutNobleShirt.WasteTamper".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Pay".Localized(), DoPayGreta);

                AddButton(1, "Common.Button.No".Localized(), DontPayGreta);
            }
        }

        public void DontPayGreta()
        {

            OutputText("Scenes.GretasGarments.DontPayGreta.TakesChances".Localized());

            DoNext(() => GretasGarments());
        }

        public void DoPayGreta()
        {
            ClearOutput();

            OutputText("Scenes.GretasGarments.DoPayGreta.BusinessOwner".Localized());

            OutputText("Scenes.GretasGarments.DoPayGreta.SilenceSettles".Localized());

            OutputText("Scenes.GretasGarments.DoPayGreta.YellsPalm".Localized());

            OutputText("Scenes.GretasGarments.DoPayGreta.AssuresWiping".Localized());

            Player.SetUndergarment(Undergarment.Nothing, Undergarment.TypeUpperwear);

            Player.Gems -= 200;

            DoNext(() => GretasGarments());
        }

        public void AskGretaToUnbind()
        {
            ClearOutput();

            OutputText("Scenes.GretasGarments.AskGretaToUnbind.AdvertisedKnack".Localized());

            OutputText("Scenes.GretasGarments.AskGretaToUnbind.TakesClothing".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Pay".Localized(), () => DoPayGretaUnbind(false));

            AddButton(1, "Common.Button.No".Localized(), DontPayGretaUnbind);

            AddButton(2, "Common.Button.Dispel".Localized(), AskGretaToDispellToo);
        }

        public void DoPayGretaUnbind(bool dispell)
        {

            OutputText("Scenes.GretasGarments.DoPayGretaUnbind.HeftyBreath".Localized());

            if (dispell)
            {
                Undergarments.SEA_SHIRT.Save.SetBool("gretaDidDispell", true);

                Player.Gems -= 300;
            }
            else
            {
                Undergarments.SEA_SHIRT.Save.SetBool("gretaDidDispell", false);

                Player.Gems -= 200;
            }

            Undergarments.SEA_SHIRT.Save.SetBool("gretaBoughtUnbind", true);

            Undergarments.SEA_SHIRT.Save.SetInt("gretaBoughtUnbindDay", View.Time.Days);

            Player.DestroyItems(Undergarments.SEA_SHIRT.Id, 1);

            DoNext(() => GretasGarments());
        }

        public void DontPayGretaUnbind()
        {

            OutputText("Scenes.GretasGarments.DontPayGretaUnbind.NotPaying".Localized());

            DoNext(() => GretasGarments());
        }

        public void AskGretaToDispellToo()
        {

            OutputText("Scenes.GretasGarments.AskGretaToDispellToo.StopsClarify".Localized());

            OutputText("Scenes.GretasGarments.AskGretaToDispellToo.FineHoney".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Pay".Localized(), () => DoPayGretaUnbind(true));

            AddButton(1, "Common.Button.No".Localized(), DontPayGretaUnbind);
        }

    }
}