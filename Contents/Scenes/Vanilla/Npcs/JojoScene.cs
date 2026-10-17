using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
	[Register<JojoScene>("coc:jojo")]
	public class JojoScene : Scene
	{

		public const int JOJO_FULL_CORRUPTION_STATUS = 6;

		public static string EmberMF() => CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == 1 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized();

		public static float AverageCockLength()
		{
			var total = 0f;

			foreach (var cock in Player.Cocks)
				total += cock.Length;

			return Player.Cocks.Count == 0 ? 0f : total / Player.Cocks.Count;
		}

		public static float AverageVaginalLooseness()
		{
			var total = 0f;

			foreach (var vagina in Player.Vaginas)
				total += vagina.Looseness;

			return Player.Vaginas.Count == 0 ? 0f : total / Player.Vaginas.Count;
		}

		public static SaveContent Save => SaveContent(SceneIds.Npcs.Jojo);

		public string debugName => "Jojo";

		public string debugHint => "";

		public override void Entry() => JojoEncounterFn();

		public bool CanEncounterJojo() =>

			!(JojoFollower() ||

			  CampCorruptJojo() ||

			  CoC.Instance.Flags.Get(GameFlagIds.JojoDeadOrGone) > 0 ||

			  (Player.Level < 4 && Player.Corruption < 25 && View.Time.Days < 28));

		public void Reset()
		{

			Save.SetBool("forgiven", false);

			Save.SetBool("postRapeCampOffer", false);
		}

		public void JojoEncounterFn()
		{
			ClearOutput();

			if (float.IsNaN(CoC.Instance.Flags.Get(GameFlagIds.JojoStatus)))
				CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, 0);

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) == 0)
			{
				if (Player.Corruption < 25)
					LowCorruptionJojoEncounter();
				else
					HighCorruptionJojoEncounter();
			}
			else
				if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) >= 2)
					CorruptJojoEncounter();
				else
					RepeatJojoEncounter();
		}

		public void JojoSprite()
		{

		}

		public bool JojoFollower()
		{

			return Player.HasStatusEffect(StatusEffectIds.PureCampJojo) && CoC.Instance.Flags.Get(GameFlagIds.JojoDeadOrGone) == 0;
		}

		public string AssholeOrDP()
		{

			if (Player.HasVagina())
				return Player.VaginaDescript(0) + " and " + Player.AssholeDescript();

			return Player.AssholeDescript();
		}

		public string VaginaOrDicksOrCrotch()
		{

			if (Player.Gender == Gender.Genderless)
				return "crotch";

			if (Player.Gender == Gender.Male)
				return Player.MultiCockDescriptLight();

			if (Player.Gender == Gender.Female)
				return Player.VaginaDescript(0);

			if (Player.Gender == Gender.Hermaphrodite)
				return Player.MultiCockDescriptLight() + " and " + Player.VaginaDescript(0);

			return "FUKK: ERROR";
		}

		public string MouthMuzzle()
		{

			if (Player.HasMuzzle())
				return "muzzle";

			return "mouth";
		}

		public string FaceMuzzle()
		{

			if (Player.HasMuzzle())
				return "muzzle";

			return "face";
		}

		public bool TentacleJojo()
		{

			return Player.HasStatusEffect(StatusEffectIds.TentacleJojo);
		}

		public bool CampCorruptJojo()
		{

			return IsJojoCorrupted() && !Player.HasStatusEffect(StatusEffectIds.NoJojo) && CoC.Instance.Flags.Get(GameFlagIds.JojoDeadOrGone) == 0 && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmJojo) == 0;
		}

		public bool IsJojoCorrupted()
		{

			return CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) >= JOJO_FULL_CORRUPTION_STATUS;
		}

		public void JojoMutationOffer()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.JojoMutationOffer.WickedIdea".Localized());

			OutputText("Scenes.Jojoscene.JojoMutationOffer.AcceptWithWarning".Localized());

			AddButton(0, "Common.Button.Yes".Localized(), JojoMutationOfferYes);

			AddButton(1, "Common.Button.No".Localized(), JojoMutationOfferNo);
		}

		public void JojoMutationOfferNo()
		{

			JojoSprite();

			OutputText("Scenes.Jojoscene.JojoMutationOfferNo.DrawLineRefusal".Localized());

			OutputText("Scenes.Jojoscene.JojoMutationOfferNo.PrepareToCallSlut".Localized());

			DoNext(() => CorruptJojoSexMenu());
		}

		public void CorruptCampJojo()
		{
			ClearOutput();

			JojoSprite();

			if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmJojo) == 0)
			{
				if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2 && CampCorruptJojo() && CoC.Instance.Flags.Get(GameFlagIds.AmilyXJojoCooldown) <= 0 && Random.Shared.Next(5) == 0)
				{
					CoC.Instance.Flags.Set(GameFlagIds.AmilyXJojoCooldown, 7);

					ClearButtons();

					AmilyTeachingJojoBJ();

					return;
				}

				if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyDiscoveredTentatleJojo) == 0 && Random.Shared.Next(10) <= 1 && Player.HasStatusEffect(StatusEffectIds.TentacleJojo))
				{
					AmilyDiscoversTentacleJojo();

					return;
				}
				else
					if (CoC.Instance.Flags.Get(GameFlagIds.AmilyPissedPcCorrupedJojo) == 0 && Random.Shared.Next(10) <= 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1 && !Player.HasStatusEffect(StatusEffectIds.TentacleJojo))
					{
						AmilyIsPissedAtYou();

						return;
					}

				if (!Player.HasStatusEffect(StatusEffectIds.JojoTFOffer) && Player.HasKeyItem("Marae's Lethicite") && Player.KeyItemV2("Marae's Lethicite") < 3 && Player.IsCorruptEnough(75))
				{
					JojoMutationOffer();

					Player.CreateStatusEffect(StatusEffectIds.JojoTFOffer, 0, 0, 0, 0);

					return;
				}

				OutputText("Scenes.Jojoscene.CorruptCampJojo.ChoosePetUsage".Localized());
			}
			else
			{
				OutputText("Scenes.Jojoscene.CorruptCampJojo.JojoSlinksOut".Localized());

				OutputText("Scenes.Jojoscene.CorruptCampJojo.JojoStateDescription".Localized((noFur ? "Common.Text.MouseManWithImmaculateWhiteFurry".Localized() : "Common.Text.NAnthropomorphicMouseWithImmaculateWhite".Localized()), (noFur ? "Common.Text.Hairless".Localized() : "Common.Text.Fuzzy".Localized())));

				if (TentacleJojo())
					OutputText("Scenes.Jojoscene.CorruptCampJojo.TentaclesSprouted".Localized());

				if (!string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.JojoTattooCollarbone)) ||
					!string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.JojoTattooShoulders)) ||

					!string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.JojoTattooLowerback)) ||

					!string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.JojoTattooButt)))
				{
					OutputText("Scenes.Jojoscene.CorruptCampJojo.ParagraphBreak".Localized());

					var jojoTattooCount = new[] { CoC.Instance.Flags.Get(GameFlagIds.JojoTattooCollarbone), CoC.Instance.Flags.Get(GameFlagIds.JojoTattooShoulders), CoC.Instance.Flags.Get(GameFlagIds.JojoTattooLowerback), CoC.Instance.Flags.Get(GameFlagIds.JojoTattooButt) }.Count(x => !string.IsNullOrEmpty(x));

					if (jojoTattooCount == 4)
						OutputText("Scenes.Jojoscene.CorruptCampJojo.TattooCoveredBody".Localized());
					else
					{
						if (jojoTattooCount > 1)
							OutputText("Scenes.Jojoscene.CorruptCampJojo.TattooListHeader".Localized());
						else
							OutputText("Scenes.Jojoscene.CorruptCampJojo.HeHasPrefix".Localized());

						if (!string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.JojoTattooCollarbone)))
							OutputText("Scenes.Jojoscene.CorruptCampJojo.TattooTokenNewline".Localized(
								CoC.Instance.Flags.Get(GameFlagIds.JojoTattooCollarbone)
							));

						if (!string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.JojoTattooShoulders)))
							OutputText("Scenes.Jojoscene.CorruptCampJojo.TattooTokenNewlineAgain".Localized(
								CoC.Instance.Flags.Get(GameFlagIds.JojoTattooShoulders)
							));

						if (!string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.JojoTattooLowerback)))
							OutputText("Scenes.Jojoscene.CorruptCampJojo.TattooTokenNewlineThird".Localized(
								CoC.Instance.Flags.Get(GameFlagIds.JojoTattooLowerback)
							));

						if (!string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.JojoTattooButt)))
							OutputText("Scenes.Jojoscene.CorruptCampJojo.TattooTokenNewlineFourth".Localized(
								CoC.Instance.Flags.Get(GameFlagIds.JojoTattooButt)
							));

						OutputText("Scenes.Jojoscene.CorruptCampJojo.NewlineOnly".Localized());
					}
				}

				if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmJojoGibsDraft) == 1)
				{
					CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmJojoGibsDraft, 2);

					OutputText("Scenes.Jojoscene.CorruptCampJojo.WordlesslyTakeDraft".Localized());

					OutputText("Scenes.Jojoscene.CorruptCampJojo.ProfitShareQuote".Localized());

					Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.IncubiDraft), 1);

					CorruptCampJojo();

					return;
				}
				else
					OutputText("Scenes.Jojoscene.CorruptCampJojo.AskForOrders".Localized());
			}

			Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);

			ClearButtons();

			AddButton(0, "Common.Button.Sex".Localized(), null);

			AddButton(1, "Common.Button.Tentaclesex".Localized(), null);

			AddButton(2, "Common.Button.MilkHim".Localized(), null);

			AddButton(3, "Common.Button.Tentaclemilk".Localized(), null);

			AddButton(4, "Common.Button.Haircare".Localized(), null);

			AddButton(5, "Common.Button.LayEggs".Localized(), null);

			if (!Player.IsGenderless() && Player.Lust >= 33)
				AddButton(0, "Common.Button.Sex".Localized(), CorruptJojoSexMenu);

			if (TentacleJojo() && Player.Lust >= 33)
				AddButton(1, "Common.Button.Tentaclesex".Localized(), UseTentacleJojo);

			if (Player.HasKeyItem("Cock Milker - Installed At Whitney's Farm"))
				if (CoC.Instance.Flags.Get(GameFlagIds.JojoCockMilkingCooldown) > 0)
					OutputText("Scenes.Jojoscene.CorruptCampJojo.DrainedBlockedOption".Localized());
				else
					if (CoC.Instance.Flags.Get(GameFlagIds.JojoCockMilkingCounter) != 0)
					{
						AddButton(2, "Common.Button.MilkHim".Localized(), () => RepeatMilkJojo());

						if (TentacleJojo())
							AddButton(3, "Common.Button.Tentaclemilk".Localized(), () => RepeatMilkJojo(TentacleJojo()));
					}
					else
						AddButton(2, "Common.Button.MilkHim".Localized(), MilkJojoFirst);

			if (Player.HasStatusEffect(StatusEffectIds.HairdresserMeeting))
				AddButton(4, "Common.Button.Haircare".Localized(), JojoPaysForPerms);

			if (Player.CanOvipositBee())
				AddButton(5, "Common.Button.LayEggs".Localized(), BeeEggsInCorruptJojo);

			AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));

			if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1 && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmJojo) == 0)
				AddButton(6, "Common.Button.FarmWork".Localized(), SendToFarm);

			if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1 && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmJojo) == 1)
				AddButton(6, "Common.Button.GoCamp".Localized(), BackToCamp);

			if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmJojo) == 1 && CoC.Instance.Flags.Get(GameFlagIds.FollowerProductionJojo) == 0)
				AddButton(7, "Common.Button.HarvestDraft".Localized(), HarvestJojoDraft);

			if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmJojo) == 1 && CoC.Instance.Flags.Get(GameFlagIds.FollowerProductionJojo) == 1)
				AddButton(7, "Common.Button.StopHarvest".Localized(), StopHarvestJojoDraft);

			if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmJojo) == 1)
				AddButton(14, "Common.Button.Back".Localized(), () => ReturnToCamp(1));
		}

		public void HarvestJojoDraft()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.HarvestJojoDraft.OrderMilkerHarvest".Localized());

			OutputText("Scenes.Jojoscene.HarvestJojoDraft.JojoMouthOpens".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.FarmUpgradesRefinery) == 0)
				OutputText("Scenes.Jojoscene.HarvestJojoDraft.MissingMilkerDevice".Localized());
			else
				OutputText("Scenes.Jojoscene.HarvestJojoDraft.AsYouWishWhisper".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.FarmUpgradesRefinery) == 1)
				CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionJojo, 1);

		}

		public void StopHarvestJojoDraft()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.StopHarvestJojoDraft.StopDraftProduction".Localized());

			OutputText("Scenes.Jojoscene.StopHarvestJojoDraft.FluidsDrowningTaunt".Localized());

			OutputText("Scenes.Jojoscene.StopHarvestJojoDraft.LowlySubmission".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionJojo, 0);

		}

		public void SendToFarm()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.SendToFarm.TellPetMousePrefix".Localized());

			if (TentacleJojo())
				OutputText("Scenes.Jojoscene.SendToFarm.ComeTentacleBeast".Localized());

			OutputText("Scenes.Jojoscene.SendToFarm.HeadToLakeSuffix".Localized());

			OutputText("Scenes.Jojoscene.SendToFarm.MixedExpressions".Localized());

			OutputText("Scenes.Jojoscene.SendToFarm.ExpectFullService".Localized());

			OutputText("Scenes.Jojoscene.SendToFarm.ShakyDeparture".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmJojo, 1);

			DoNext(() => ReturnToCamp(1));
		}

		public void BackToCamp()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.BackToReturnToCampOrder".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmJojo, 0);

		}

		public void JojoMutationOfferYes()
		{

			JojoSprite();

			Player.AddKeyValue("Marae's Lethicite", 2, 1);

			OutputText("Scenes.Jojoscene.JojoMutationOfferYes.EagerAcceptance".Localized());

			OutputText("Scenes.Jojoscene.JojoMutationOfferYes.PhallicLethiciteChunk".Localized());

			OutputText("Scenes.Jojoscene.JojoMutationOfferYes.LickCrystalDildo".Localized());

			OutputText("Scenes.Jojoscene.JojoMutationOfferYes.ParagraphBreakDivider".Localized());

			OutputText("Scenes.Jojoscene.JojoMutationOfferYes.ParagraphBreakItalicOpen".Localized());

			if ((int)Player.Gender <= 1)
			{
				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.FantasyFuckHim".Localized());

				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.PullOutOfHim".Localized());

				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.ShoveOverLog".Localized());

				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.ParagraphBreakDividerAgain".Localized());

				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.SnapOutOfFantasy".Localized());

				if (Player.CockTotal() > 1)
					OutputText("Scenes.Jojoscene.JojoMutationOfferYes.SingleSLetter".Localized());

				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.TakeTaintedArtifactSuffix".Localized());
			}
			else
			{
				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.FantasyRideHim".Localized());

				if (Player.BiggestLactation() < 2)
					OutputText("Scenes.Jojoscene.JojoMutationOfferYes.TitsCoverHisHead".Localized());
				else
					OutputText("Scenes.Jojoscene.JojoMutationOfferYes.SuckleYourBreast".Localized());

				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.DildoBuriedHilt".Localized());

				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.CalmTremblingBody".Localized());

				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.ParagraphBreakDividerThird".Localized());

				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.SnapOutYourFantasy".Localized());

				if (Player.Vaginas[0].Virgin)
					OutputText("Scenes.Jojoscene.JojoMutationOfferYes.VirginityStaysIntact".Localized());

				OutputText("Scenes.Jojoscene.JojoMutationOfferYes.DildoBackToInventory".Localized());
			}

			OutputText("Scenes.Jojoscene.JojoMutationOfferYes.CantWaitCallNow".Localized());

			DoNext(() => MutateJojo());
		}

		public void MutateJojo()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.MutateJojo.CallOutJojoArrives".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.QuickPlaintiveWhine".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.JojoTakesOffering".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.OneHandPinned".Localized(
				VaginaOrDicksOrCrotch()
			));

			OutputText("Scenes.Jojoscene.MutateJojo.MoansFeveredPitch".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.ShakesHeadWhines".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.SqueezeBallsTaunt".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.EyesRollBack".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.TentaclesCurlDown".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.WhisperGoodBitch".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.FeelTailCurl".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.LickNewOpening".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.BodyObeysDespitePleasure".Localized());

			if (Player.BiggestBreastSize() < 1)
				OutputText("Scenes.Jojoscene.MutateJojo.BodiesWord".Localized());
			else
				OutputText(Appearance.AllBreastsDescript(Player));

			OutputText("Scenes.Jojoscene.MutateJojo.PopsOutUnderChin".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.WhisperGoodToy".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.SubmitsHappily".Localized((noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));

			if (Player.BiggestBreastSize() >= 1)
				OutputText(Appearance.AllBreastsDescript(Player));
			else
				OutputText("Scenes.Jojoscene.MutateJojo.ChestWord".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.WatchThemClimb".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.PullBackWatchPrefix".Localized());

			if (Player.CockTotal() > 0)
			{
				OutputText("Scenes.Jojoscene.MutateJojo.StrokingOwnMember".Localized());

				if (Player.CockTotal() > 1)
					OutputText("Scenes.Jojoscene.MutateJojo.SingleSLetter".Localized());

				if (Player.HasVagina())
					OutputText("Scenes.Jojoscene.MutateJojo.AndWord".Localized());
			}

			if (Player.HasVagina())
				OutputText("Scenes.Jojoscene.MutateJojo.CaressingAchingTwat".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.MousePaintsHimself".Localized());

			OutputText("Scenes.Jojoscene.MutateJojo.DildoGonePinkTinge".Localized((noFur ? "" : " the fur around")));

			OutputText("Scenes.Jojoscene.MutateJojo.TurnBackAroused".Localized());

			Player.ApplyStats(new DynStat(GameStats.Lust, 300f), new DynStat(GameStats.Corruption, 10f));

			Player.CreateStatusEffect(StatusEffectIds.NoJojo, 0, 0, 0, 0);

			Player.CreateStatusEffect(StatusEffectIds.TentacleJojo, 0, 0, 0, 0);

			DoNext(PlayerMenu);
		}

		public void UseTentacleJojo()
		{

			JojoSprite();

			Player.SlimeFeed();

			var nippleSucking = false;

			var mouthFucking = false;

			var titFucking = false;

			var cumPuddle = false;

			var milkPuddle = false;

			OutputText("Scenes.Jojoscene.UseTentacleJojo.CallOutSlut".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.JojoBoundingArrival".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.DesperationHangsThick".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.DropSplatPuddle".Localized());

			if (Player.HasVagina())
				OutputText("Scenes.Jojoscene.UseTentacleJojo.SplitsInHalf".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.HardToDominate".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.HandsOnHipsChallenge".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.StarvingPredatorSmile".Localized());

			if (Player.HasVagina())
				OutputText("Scenes.Jojoscene.UseTentacleJojo.AndWord".Localized(
					Player.VaginaDescript(0)
				));

			OutputText("Scenes.Jojoscene.UseTentacleJojo.SentencePeriodParagraphBreak".Localized());

			if (Player.Cocks.Count > 0)
			{
				OutputText("Scenes.Jojoscene.UseTentacleJojo.UpsideDownVantage".Localized());

				if (Player.CockTotal() == 1)
					if (Player.Balls > 0)
					{
						OutputText("Scenes.Jojoscene.UseTentacleJojo.OneTentacleLashes".Localized());

						if (Player.CockArea(0) > 60)
							OutputText("Scenes.Jojoscene.UseTentacleJojo.LaboriousStretching".Localized());
						else
							if (Player.CockArea(0) >= 20)
								OutputText("Scenes.Jojoscene.UseTentacleJojo.EngulfingWord".Localized());
							else
								OutputText("Scenes.Jojoscene.UseTentacleJojo.EasilySwallowingWord".Localized());

						OutputText("Scenes.Jojoscene.UseTentacleJojo.CockInTightVise".Localized(
							Player.BallsDescript()
						));

						OutputText("Scenes.Jojoscene.UseTentacleJojo.ParagraphBreak".Localized());
					}
					else
					{
						OutputText("Scenes.Jojoscene.UseTentacleJojo.OneTentacleLashesAgain".Localized());

						if (Player.CockArea(0) > 60)
							OutputText("Scenes.Jojoscene.UseTentacleJojo.LaboriousStretchingAgain".Localized());
						else
							if (Player.CockArea(0) >= 20)
								OutputText("Scenes.Jojoscene.UseTentacleJojo.EngulfingWordAgain".Localized());
							else
								OutputText("Scenes.Jojoscene.UseTentacleJojo.EasilySwallowingWordAgain".Localized());

						OutputText("Scenes.Jojoscene.UseTentacleJojo.TentaclesSlideUnderCock".Localized());

						if (!Player.HasVagina())
							OutputText("Scenes.Jojoscene.UseTentacleJojo.RubbingTaintSlime".Localized());
						else
						{
							OutputText("Scenes.Jojoscene.UseTentacleJojo.ThighRubLubricant".Localized());

							if (Player.GetClitLength() >= 4)
								OutputText("Scenes.Jojoscene.UseTentacleJojo.NearCumSpot".Localized());
						}

						OutputText("Scenes.Jojoscene.UseTentacleJojo.ParagraphBreakAgain".Localized());
					}

				if (Player.CockTotal() == 2)
					if (Player.Balls > 0)
					{
						OutputText("Scenes.Jojoscene.UseTentacleJojo.TwoTentaclesLash".Localized());

						if (Player.CockArea(0) > 60)
							OutputText("Scenes.Jojoscene.UseTentacleJojo.LaboriousStretchingThird".Localized());
						else
							if (Player.CockArea(0) >= 20)
								OutputText("Scenes.Jojoscene.UseTentacleJojo.EngulfingWordThird".Localized());
							else
								OutputText("Scenes.Jojoscene.UseTentacleJojo.EasilySwallowingWordThird".Localized());

						OutputText("Scenes.Jojoscene.UseTentacleJojo.CocksInTightVises".Localized());
					}
					else
					{
						OutputText("Scenes.Jojoscene.UseTentacleJojo.TwoTentaclesLashAgain".Localized());

						if (Player.CockArea(0) > 60)
							OutputText("Scenes.Jojoscene.UseTentacleJojo.LaboriousStretchingFourth".Localized());
						else
							if (Player.CockArea(0) >= 20)
								OutputText("Scenes.Jojoscene.UseTentacleJojo.EngulfingWordFourth".Localized());
							else
								OutputText("Scenes.Jojoscene.UseTentacleJojo.EasilySwallowingWordFourth".Localized());

						OutputText("Scenes.Jojoscene.UseTentacleJojo.RemainingSeeksClit".Localized());

						if (Player.HasVagina())
						{
							OutputText("Scenes.Jojoscene.UseTentacleJojo.SuckClitInside".Localized());

							if (Player.GetClitLength() >= 4)
								OutputText("Scenes.Jojoscene.UseTentacleJojo.NearCumSpotAgain".Localized());
						}
						else
							OutputText("Scenes.Jojoscene.UseTentacleJojo.RubAlongTaint".Localized());

						OutputText("Scenes.Jojoscene.UseTentacleJojo.ParagraphBreakThird".Localized());
					}

				if (Player.CockTotal() >= 3)
				{
					OutputText("Scenes.Jojoscene.UseTentacleJojo.ThreeTentaclesLash".Localized());

					if (Player.CockArea(0) > 60)
						OutputText("Scenes.Jojoscene.UseTentacleJojo.LaboriousSwallowWork".Localized());
					else
						if (Player.CockArea(0) >= 20)
							OutputText("Scenes.Jojoscene.UseTentacleJojo.EngulfingWordFifth".Localized());
						else
							OutputText("Scenes.Jojoscene.UseTentacleJojo.EasilySwallowingWordFifth".Localized());

					OutputText("Scenes.Jojoscene.UseTentacleJojo.ThreeCockOverwhelm".Localized());

					if (Player.CockTotal() > 3)
						OutputText("Scenes.Jojoscene.UseTentacleJojo.RemainingPulsesJealous".Localized(
							Appearance.CockNoun(BodyPartTypes.Human)
						));

					OutputText("Scenes.Jojoscene.UseTentacleJojo.ParagraphBreakFourth".Localized());
				}
			}

			if (Player.Cocks.Count == 0 && Player.HasVagina())
			{
				OutputText("Scenes.Jojoscene.UseTentacleJojo.JojoTailTentacleSeeks".Localized());

				if (Player.GetClitLength() >= 4)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.NearCumSpotThird".Localized());
				else
					OutputText("Scenes.Jojoscene.UseTentacleJojo.WishClitBigger".Localized());

				OutputText("Scenes.Jojoscene.UseTentacleJojo.ParagraphBreakFifth".Localized());
			}

			OutputText("Scenes.Jojoscene.UseTentacleJojo.TheWord".Localized());

			if (Player.HasVagina())
				OutputText("Scenes.Jojoscene.UseTentacleJojo.TentaclePair".Localized());
			else
				OutputText("Scenes.Jojoscene.UseTentacleJojo.LoneTentacleWord".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.HangingBottomDecide".Localized());

			if (!Player.HasVagina())
				OutputText("Scenes.Jojoscene.UseTentacleJojo.PluralSLetter".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.GoodTimeToPenetrate".Localized());

			if (Player.HasVagina())
				OutputText("Scenes.Jojoscene.UseTentacleJojo.PluralSLetterAgain".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.PressAgainstYou".Localized());

			if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin || Player.Anal.Looseness == 0)
			{
				OutputText("Scenes.Jojoscene.UseTentacleJojo.BarelySlippingInside".Localized());

				if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin && Player.Anal.Looseness == 0)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.PluralSLetterThird".Localized());
			}
			else
				if (Player.HasVagina())
				{
					if (Player.Vaginas[0].Looseness >= 4)
						OutputText("Scenes.Jojoscene.UseTentacleJojo.FallingIntoCumPlural".Localized());
					else
						if (Player.Vaginas[0].Wetness >= 3)
							OutputText("Scenes.Jojoscene.UseTentacleJojo.SlipInLubricated".Localized());
						else
							OutputText("Scenes.Jojoscene.UseTentacleJojo.ForceBothHoles".Localized(
								Player.VaginaDescript(0)
							));
				}
				else
					if (Player.Anal.Looseness >= 4)
						OutputText("Scenes.Jojoscene.UseTentacleJojo.FallingIntoCumSingular".Localized());
					else
						if (Player.Anal.Wetness >= 3)
							OutputText("Scenes.Jojoscene.UseTentacleJojo.SlipInLubricatedAgain".Localized());
						else
							OutputText("Scenes.Jojoscene.UseTentacleJojo.ForceSingleHole".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.OpensWithWordlessCry".Localized(
				MouthMuzzle()
			));

			if (Player.HasVagina())
				OutputText("Scenes.Jojoscene.UseTentacleJojo.AndPleasureWord".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.AsJojoTentacleWord".Localized());

			if (Player.HasVagina())
				OutputText("Scenes.Jojoscene.UseTentacleJojo.PluralSBeginWord".Localized());
			else
				OutputText("Scenes.Jojoscene.UseTentacleJojo.BeginsWord".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.ThoroughViolationPiston".Localized());

			if (Player.Cocks.Count > 0)
			{
				OutputText("Scenes.Jojoscene.UseTentacleJojo.StrangeAnalPleasure".Localized());

				if (Player.CockTotal() == 1)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.CockTentacleAppreciates".Localized());

				if (Player.CockTotal() > 1)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.CockTentaclesAppreciate".Localized());
			}

			if (Player.HasVagina())
				OutputText("Scenes.Jojoscene.UseTentacleJojo.SpasmsAroundInvader".Localized(
					Player.VaginaDescript(0)
				));

			OutputText("Scenes.Jojoscene.UseTentacleJojo.ParagraphBreakSixth".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.WaistBindingsLoosen".Localized(
				Appearance.AllBreastsDescript(Player)
			));

			if ((Player.BiggestBreastSize() >= 4 || (Player.BiggestBreastSize() >= 4 && Player.BreastRows.Count > 1)) && Player.BiggestLactation() < 2)
			{
				titFucking = true;

				OutputText("Scenes.Jojoscene.UseTentacleJojo.TwinTentaclesErupt".Localized(
					Player.BreastDescript(0),

					FaceMuzzle()
				));
			}

			if (Player.BiggestLactation() >= 2)
			{
				nippleSucking = true;

				OutputText("Scenes.Jojoscene.UseTentacleJojo.HeadsSplitIntoQuarters".Localized());

				if (Player.Corruption < 80)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.WantonSlutTaunt".Localized());
				else
					OutputText("Scenes.Jojoscene.UseTentacleJojo.GivingItUpEasily".Localized());

				OutputText("Scenes.Jojoscene.UseTentacleJojo.DickMouthsLatchOn".Localized());

				if (Player.CowScore() >= 5)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.MooingInContentment".Localized());

				OutputText("Scenes.Jojoscene.UseTentacleJojo.HappySmileWatch".Localized());

				if (Player.BreastRows.Count > 1)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.EnjoyUntilEmptied".Localized());

				OutputText("Scenes.Jojoscene.UseTentacleJojo.ParagraphBreakSeventh".Localized());
			}
			else
				if (Player.BiggestBreastSize() < 4 && Random.Shared.Next(2) == 0)
				{
					nippleSucking = true;

					OutputText("Scenes.Jojoscene.UseTentacleJojo.HeadsSplitRevealTongue".Localized());

					if (Player.BiggestLactation() >= 1)
						OutputText("Scenes.Jojoscene.UseTentacleJojo.MilkSquirtSucked".Localized());

					OutputText("Scenes.Jojoscene.UseTentacleJojo.RelentlessSucking".Localized());
				}
				else
					if (Player.BiggestBreastSize() < 4 && Player.BiggestLactation() < 2)
					{
						mouthFucking = true;

						OutputText("Scenes.Jojoscene.UseTentacleJojo.WaistBindingsLoosenAgain".Localized());

						OutputText("Scenes.Jojoscene.UseTentacleJojo.TwinnedDicksThroat".Localized());
					}

			OutputText("Scenes.Jojoscene.UseTentacleJojo.BestialPleasureHowl".Localized());

			if (Player.HasVagina())
			{
				OutputText("Scenes.Jojoscene.UseTentacleJojo.TentaclesThickenBulges".Localized(
					AssholeOrDP(),

					AssholeOrDP()
				));

				if (Player.Pregnancy.Incubation == 0)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.MildlyPregnant".Localized());
				else
					OutputText("Scenes.Jojoscene.UseTentacleJojo.EvenMorePregnant".Localized());

				OutputText("Scenes.Jojoscene.UseTentacleJojo.BodySpasmsLocked".Localized());

				if (Player.Vaginas[0].Wetness == 5)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.SoaksHimPussJuice".Localized());
				else
					if (Player.Vaginas[0].Wetness >= 3)
						OutputText("Scenes.Jojoscene.UseTentacleJojo.DroolingCuntPuddle".Localized());
					else
						OutputText("Scenes.Jojoscene.UseTentacleJojo.PeriodSpace".Localized());

				if (Player.CockTotal() <= 2 && Player.Balls == 0)
				{
					OutputText("Scenes.Jojoscene.UseTentacleJojo.ClitTentacleBulges".Localized());

					Player.Vaginas[0].ClitLength += .25f;
				}

				OutputText("Scenes.Jojoscene.UseTentacleJojo.BothTentaclesPullOut".Localized());
			}
			else
				OutputText("Scenes.Jojoscene.UseTentacleJojo.TentacleThickensBulges".Localized(
					Player.AssholeDescript()
				));

			if (Player.CockTotal() > 0)
			{
				if (Player.CockTotal() == 1)
				{
					OutputText("Scenes.Jojoscene.UseTentacleJojo.TentacleClampsWriggles".Localized(
						Player.MultiCockDescript()
					));

					if (Player.CumQ() >= 100)
					{
						OutputText("Scenes.Jojoscene.UseTentacleJojo.ItStrugglesOutput".Localized());

						if (Player.CumQ() >= 300)
						{
							OutputText("Scenes.Jojoscene.UseTentacleJojo.BloatedItDrips".Localized());

							if (Player.CumQ() >= 600)
							{
								OutputText("Scenes.Jojoscene.UseTentacleJojo.PumpTentacleFull".Localized());

								if (Player.CumQ() >= 800)
									OutputText("Scenes.Jojoscene.UseTentacleJojo.JizzPuddleSingular".Localized());

								OutputText("Scenes.Jojoscene.UseTentacleJojo.SentencePeriod".Localized());

								cumPuddle = true;
							}
						}
					}
					else
						OutputText("Scenes.Jojoscene.UseTentacleJojo.SentencePeriodAgain".Localized());
				}

				if (Player.CockTotal() > 1)
				{
					OutputText("Scenes.Jojoscene.UseTentacleJojo.TentaclesClampWriggle".Localized(
						Player.MultiCockDescript()
					));

					if (Player.CumQ() >= 100)
					{
						OutputText("Scenes.Jojoscene.UseTentacleJojo.TheyStruggleOutput".Localized());

						if (Player.CumQ() >= 300)
						{
							OutputText("Scenes.Jojoscene.UseTentacleJojo.BloatedTheyDrip".Localized());

							if (Player.CumQ() >= 600)
							{
								OutputText("Scenes.Jojoscene.UseTentacleJojo.PumpTentaclesFull".Localized());

								if (Player.CumQ() >= 800)
									OutputText("Scenes.Jojoscene.UseTentacleJojo.JizzPuddlePlural".Localized());

								OutputText("Scenes.Jojoscene.UseTentacleJojo.SentencePeriodThird".Localized());

								cumPuddle = true;
							}
						}
					}
					else
						OutputText("Scenes.Jojoscene.UseTentacleJojo.SentencePeriodFourth".Localized());
				}
			}

			if (Player.BiggestLactation() >= 2 && nippleSucking)
			{
				OutputText("Scenes.Jojoscene.UseTentacleJojo.MilkBlastsTentacles".Localized());

				if (Player.NippleLength >= 2)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.EvenLarger".Localized());
				else
					OutputText("Scenes.Jojoscene.UseTentacleJojo.ObsceneWord".Localized());

				OutputText("Scenes.Jojoscene.UseTentacleJojo.CowTeatsFromSuction".Localized());

				if (Player.NippleLength < 4 && Player.BreastRows[0].BreastRating >= 1)
				{
					OutputText("Scenes.Jojoscene.UseTentacleJojo.WontShrinkBack".Localized());

					Player.NippleLength += .25f;
				}
				else
					OutputText("Scenes.Jojoscene.UseTentacleJojo.WontGrowBigger".Localized());

				OutputText("Scenes.Jojoscene.UseTentacleJojo.FinallyEmptiedRelief".Localized(
					Appearance.AllBreastsDescript(Player)
				));

				milkPuddle = true;
			}

			if (titFucking)
			{
				OutputText("Scenes.Jojoscene.UseTentacleJojo.TitfuckTentaclesTighten".Localized(
					Appearance.AllBreastsDescript(Player)
				));

				if (Player.Corruption >= 80 && Player.Libido >= 70)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.LickLipsSwallow".Localized());

				if (Player.Body.Get<HairPart>().Length > 0)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.UnloadsIntoHair".Localized());
				else
					OutputText("Scenes.Jojoscene.UseTentacleJojo.UnloadsOntoHead".Localized());

				OutputText("Scenes.Jojoscene.UseTentacleJojo.BothTentaclesGoFlaccid".Localized());
			}

			if (mouthFucking)
			{
				OutputText("Scenes.Jojoscene.UseTentacleJojo.WideBulgeForcesJaw".Localized());

				if (Player.Pregnancy.Incubation == 0 && Player.ButtPregnancy.Incubation == 0)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.EightMonthPregnantWoman".Localized());
				else
					OutputText("Scenes.Jojoscene.UseTentacleJojo.BroodMotherImmobile".Localized());

				OutputText("Scenes.Jojoscene.UseTentacleJojo.BiggerThanOldSelf".Localized(
					FaceMuzzle()
				));

				cumPuddle = true;

				Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));
			}

			OutputText("Scenes.Jojoscene.UseTentacleJojo.TentaclesLoseStrength".Localized());

			if (cumPuddle)
			{
				if (milkPuddle)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.CumMilkPuddle".Localized());
				else
					OutputText("Scenes.Jojoscene.UseTentacleJojo.SpunkPuddle".Localized());
			}
			else
				if (milkPuddle)
					OutputText("Scenes.Jojoscene.UseTentacleJojo.MilkPuddle".Localized());
				else
					OutputText("Scenes.Jojoscene.UseTentacleJojo.OnTheGround".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.MoanInFrustration".Localized());

			OutputText("Scenes.Jojoscene.UseTentacleJojo.MouseKneelsLicking".Localized());

			Player.ButtChange(40, true);

			Player.CuntChange(40, true);

			Player.Orgasm("VaginalAndAnal");

			Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));

			DoNext(() => ReturnToCamp(1));
		}

		public void JojoMilkPay(bool tentacle = false)
		{

			JojoSprite();

			CoC.Instance.Flags.Set(GameFlagIds.JojoCockMilkingCooldown, 4);

			OutputText("Scenes.Jojoscene.JojoMilkPay.DingChimePanelDisplay".Localized());

			int payout = 0;

			int cap = 500;

			float cumQ = 0;

			if (GameSettings.Difficulty <= -2)
				cap *= 2;

			if (Debug)
			{
				CoC.Instance.Flags.Set(GameFlagIds.WhitneyGemsPaidThisWeek, 0);

				cap = 9999;
			}

			cumQ = JojoCumQ();

			if (tentacle)
				cumQ += 40000 + Random.Shared.Next(1000);

			cumQ -= Random.Shared.Next((int)(cumQ / 10));

			OutputText("Scenes.Jojoscene.JojoMilkPay.LsAmount".Localized(
				cumQ / 1000
			));

			if (cumQ > 100)
				payout = 2 + (int)(cumQ / 200) * 2;

			if (CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek) + payout >= cap)
			{
				payout = cap - CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek);

				OutputText("Scenes.Jojoscene.JojoMilkPay.GemsInsufficientWarning".Localized());
			}
			else
				if (CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek) >= cap)
				{
					payout = 0;

					OutputText("Scenes.Jojoscene.JojoMilkPay.FundsExhaustedWarning".Localized());
				}

			if (payout > 0)
			{
				Player.Gems += payout;

				CoC.Instance.Flags.Set(GameFlagIds.WhitneyGemsPaidThisWeek, CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek) + payout);

				CoC.Instance.Flags.Set(GameFlagIds.JojoCockMilkingCounter, CoC.Instance.Flags.Get(GameFlagIds.JojoCockMilkingCounter) + 1);

				if (payout == 1)
					OutputText("Scenes.Jojoscene.JojoMilkPay.GemRollsCollectionPlate".Localized(
						NumberUtils.Num2Text(payout)
					));
				else
					OutputText("Scenes.Jojoscene.JojoMilkPay.GemsRollCollectionPlate".Localized(
						NumberUtils.Num2Text(payout)
					));
			}
			else
				OutputText("Scenes.Jojoscene.JojoMilkPay.YouGPrefix".Localized());
		}

		public float JojoCumQ()
		{

			float cumQ = 0;

			cumQ = 400;

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoCockMilkingCounter) < 4)
				cumQ += CoC.Instance.Flags.Get(GameFlagIds.JojoCockMilkingCounter) * 200;
			else
				cumQ += CoC.Instance.Flags.Get(GameFlagIds.JojoCockMilkingCounter) * 100;

			if (TentacleJojo())
				cumQ += 500 + CoC.Instance.Flags.Get(GameFlagIds.JojoCockMilkingCounter) * 100;

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoBlowjobXp) < 10)
				cumQ += CoC.Instance.Flags.Get(GameFlagIds.JojoBlowjobXp) * 50;
			else
				cumQ += 500;

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) < 10)
				cumQ += CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) * 25;
			else
				cumQ += 250;

			return cumQ;
		}

		public void MilkJojoFirst()
		{
			ClearOutput();

			JojoSprite();

			int x = Player.CockThatFits(40);

			OutputText("Scenes.Jojoscene.MilkJojoFirst.WickedIdeaSummonsMouse".Localized());

			if (TentacleJojo())
				OutputText("Scenes.Jojoscene.MilkJojoFirst.TentacleWord".Localized());
			else
				OutputText("Scenes.Jojoscene.MilkJojoFirst.BoyWord".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.ToySuffix".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.MouseMewlsDisappointment".Localized((noFur ? "Common.Text.Eared".Localized() : "Common.Text.Furred".Localized())));

			OutputText("Scenes.Jojoscene.MilkJojoFirst.WhitneyFarmLooms".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.BarnDoorSlidesOpen".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.PcCockMilkedCounter) > 0)
				OutputText("Scenes.Jojoscene.MilkJojoFirst.MilkerReeksSpunk".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.JojoWhinesPleading".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.PushIntoHarness".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.MachineryKicksIn".Localized());

			if (Player.HasCock())
				OutputText("Scenes.Jojoscene.MilkJojoFirst.TubeTurnsTowardYou".Localized());
			else
				OutputText("Scenes.Jojoscene.MilkJojoFirst.TubeRisesSniffing".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.UndulatingTubeRise".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.HoseInteriorWrithes".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.MilkerBouncesTwitches".Localized());

			if (Player.HasCock())
			{
				if (x < 0)
				{
					x = Player.BiggestCockIndex();

					if (CoC.Instance.Flags.Get(GameFlagIds.PcCockMilkedCounter) > 0)
						OutputText("Scenes.Jojoscene.MilkJojoFirst.MachineTakesHour".Localized());
					else
						OutputText("Scenes.Jojoscene.MilkJojoFirst.MachinePumpsWhile".Localized());

					OutputText("Scenes.Jojoscene.MilkJojoFirst.ArousedShuckArmor".Localized());

					if (Player.LustPercent < 50)
						OutputText("Scenes.Jojoscene.MilkJojoFirst.CockStiffensHand".Localized());
					else
						OutputText("Scenes.Jojoscene.MilkJojoFirst.CockRigidityHand".Localized());

					OutputText("Scenes.Jojoscene.MilkJojoFirst.JojoMoansOblivious".Localized(
						Player.CockDescript(x)
					));
				}
				else
				{
					if (CoC.Instance.Flags.Get(GameFlagIds.PcCockMilkedCounter) > 0)
						OutputText("Scenes.Jojoscene.MilkJojoFirst.MachineTakesHourAgain".Localized());
					else
						OutputText("Scenes.Jojoscene.MilkJojoFirst.MachinePumpsWhileAgain".Localized());

					OutputText("Scenes.Jojoscene.MilkJojoFirst.ArousedShuckArmorAgain".Localized());

					if (Player.LustPercent < 50)
						OutputText("Scenes.Jojoscene.MilkJojoFirst.CockStiffensHandAgain".Localized());
					else
						OutputText("Scenes.Jojoscene.MilkJojoFirst.CockRigidityHandAgain".Localized());

					OutputText("Scenes.Jojoscene.MilkJojoFirst.JojoMoansObliviousAgain".Localized(Player.CockDescript(x), (noFur ? "" : ", furry")));
				}

				Player.Orgasm("Dick");
			}
			else
				if (Player.HasVagina())
				{
					if (CoC.Instance.Flags.Get(GameFlagIds.PcCockMilkedCounter) > 0)
						OutputText("Scenes.Jojoscene.MilkJojoFirst.MachineTakesHourThird".Localized());
					else
						OutputText("Scenes.Jojoscene.MilkJojoFirst.MachinePumpsWhileThird".Localized());

					OutputText("Scenes.Jojoscene.MilkJojoFirst.ArousedVaginaReveal".Localized());

					if (Player.GetClitLength() > 3)
						OutputText("Scenes.Jojoscene.MilkJojoFirst.ClitButtonFingered".Localized());

					OutputText("Scenes.Jojoscene.MilkJojoFirst.NaughtyBallsLick".Localized());

					Player.Orgasm("Vaginal");
				}
				else
				{
					if (CoC.Instance.Flags.Get(GameFlagIds.PcCockMilkedCounter) > 0)
						OutputText("Scenes.Jojoscene.MilkJojoFirst.MachineTakesHourFourth".Localized());
					else
						OutputText("Scenes.Jojoscene.MilkJojoFirst.MachinePumpsWhileFourth".Localized());

					OutputText("Scenes.Jojoscene.MilkJojoFirst.ArousedWithoutEquipment".Localized());

					Player.ApplyStats(new DynStat(GameStats.Lust, 50f));
				}

			OutputText("Scenes.Jojoscene.MilkJojoFirst.AppointedTimeSqueak".Localized());

			if (Player.HasCock())
				OutputText("Scenes.Jojoscene.MilkJojoFirst.AgonizingPressureSqueeze".Localized(
					Player.CockDescript(x)
				));

			OutputText("Scenes.Jojoscene.MilkJojoFirst.BackArchesTubeSwelling".Localized());

			if (!Player.HasCock() && Player.HasVagina())
				OutputText("Scenes.Jojoscene.MilkJojoFirst.BallsTrembleShrink".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.ParagraphBreak".Localized());

			if (Player.HasCock())
			{
				if (Player.CockThatFits(40) < 0)
					OutputText("Scenes.Jojoscene.MilkJojoFirst.CumOverBack".Localized((noFur ? "Common.Text.CoatingHisSkin".Localized() : "Common.Text.MattingHisWhiteFur".Localized())));
				else
					OutputText("Scenes.Jojoscene.MilkJojoFirst.CumAfterPullOut".Localized((noFur ? "Common.Text.CoatingHisSkin".Localized() : "Common.Text.MattingHisWhiteFur".Localized())));
			}
			else
				if (Player.HasVagina())
					OutputText("Scenes.Jojoscene.MilkJojoFirst.ClimaxClampsFingers".Localized());
				else
					OutputText("Scenes.Jojoscene.MilkJojoFirst.WistfulSighWish".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.MinutesLongOrgasm".Localized());

			OutputText("Scenes.Jojoscene.MilkJojoFirst.JojoLoweredNuzzles".Localized());

			JojoMilkPay();

			OutputText("Scenes.Jojoscene.MilkJojoFirst.OnYourWay".Localized());

			DoNext(() => ReturnToCamp(1));
		}

		public void RepeatMilkJojo(bool tentacle = false)
		{
			ClearOutput();

			JojoSprite();

			if (!tentacle || !TentacleJojo())
			{
				OutputText("Scenes.Jojoscene.RepeatMilkJojo.CorruptedPetBounds".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.JojoNodsFondles".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.BarnStallStrapping".Localized());

				if (JojoCumQ() < 1000)
					OutputText("Scenes.Jojoscene.RepeatMilkJojo.DrippedFloorMess".Localized());
				else
					if (JojoCumQ() < 4000)
						OutputText("Scenes.Jojoscene.RepeatMilkJojo.DrizzlingManSlime".Localized());
					else
						OutputText("Scenes.Jojoscene.RepeatMilkJojo.PrecumRiverPuddle".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.MouseImmobilizedHangs".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.SlutHauledUp".Localized((noFur ? "Common.Text.Eared".Localized() : "Common.Text.Furred".Localized())));

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.TubeSlurpsCockhead".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.AssSmackWalkAround".Localized((noFur ? "" : " furry")));

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.EyesRollFroth".Localized());

				if (!Player.HasCock() && !Player.HasVagina())
					OutputText("Scenes.Jojoscene.RepeatMilkJojo.ForcedAssholeStimulate".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.NoseSniffsPanting".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.OrgasmSeizeEarTug".Localized());

				if (JojoCumQ() < 1000)
				{
					OutputText("Scenes.Jojoscene.RepeatMilkJojo.SemenSlaveSwollenBalls".Localized());

					OutputText("Scenes.Jojoscene.RepeatMilkJojo.HoseSlidesOffSoft".Localized());

					OutputText("Scenes.Jojoscene.RepeatMilkJojo.PatHeadAnswerNow".Localized());
				}
				else
					if (JojoCumQ() < 4000)
					{
						OutputText("Scenes.Jojoscene.RepeatMilkJojo.PentUpGrapefruitBalls".Localized());

						OutputText("Scenes.Jojoscene.RepeatMilkJojo.HoseOffHardCock".Localized());

						OutputText("Scenes.Jojoscene.RepeatMilkJojo.AffectionateMildPraise".Localized());
					}
					else
					{
						OutputText("Scenes.Jojoscene.RepeatMilkJojo.PentUpHeavyBalls".Localized());

						OutputText("Scenes.Jojoscene.RepeatMilkJojo.ReleaseTubeBurps".Localized());

						OutputText("Scenes.Jojoscene.RepeatMilkJojo.PetGoodCumSlave".Localized());
					}

				JojoMilkPay();

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.OnYourWay".Localized());
			}
			else
			{
				OutputText("Scenes.Jojoscene.RepeatMilkJojo.MutatedMouseLumbers".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.ShoulderTentaclesEngorge".Localized((noFur ? "Common.Text.AwayFromHisBody".Localized() : "Common.Text.OutOfHisFur".Localized())));

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.BarnStallSnuggle".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.LurchLiftedMilking".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.StrokeShoulderTentacle".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.PaceStallEdge".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.LeanBackSelfTouch".Localized());

				if ((int)Player.Gender > 0)
					OutputText("Scenes.Jojoscene.RepeatMilkJojo.LoinsWord".Localized());
				else
					OutputText("Scenes.Jojoscene.RepeatMilkJojo.BodyWord".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.EyesClosedStrip".Localized());

				if (SceneInstance<CeraphScene>.Get().HasExhibition())
					OutputText("Scenes.Jojoscene.RepeatMilkJojo.ExhibitionistThrill".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.FastHardClimax".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.StarsBlinkLookUp".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.HarnessSagLeftovers".Localized());

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.TellHimGreatJob".Localized((noFur ? "Common.Text.PatHisJizzSoakedHead".Localized() : "Common.Text.ScratchHisJizzSlickedFur".Localized())));

				JojoMilkPay(true);

				OutputText("Scenes.Jojoscene.RepeatMilkJojo.OnYourWayAgain".Localized());
			}

			Player.Orgasm("Generic");

			DoNext(() => ReturnToCamp(1));
		}

		public void JojoPaysForPerms()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.JojoPaysForPerms.GoblinAnswersDoor".Localized());

			OutputText("Scenes.Jojoscene.JojoPaysForPerms.PetMousePaysToday".Localized());

			OutputText("Scenes.Jojoscene.JojoPaysForPerms.MoansReachesHair".Localized());

			if (JojoCumQ() >= 4000)
				OutputText("Scenes.Jojoscene.JojoPaysForPerms.SurprisedGurgleHeavy".Localized());
			else
				if (JojoCumQ() >= 1000)
					OutputText("Scenes.Jojoscene.JojoPaysForPerms.GurgleDiscoversSize".Localized());
				else
					OutputText("Scenes.Jojoscene.JojoPaysForPerms.RollsThemFingers".Localized());

			OutputText("Scenes.Jojoscene.JojoPaysForPerms.HisEnduranceEdge".Localized());

			OutputText("Scenes.Jojoscene.JojoPaysForPerms.WetStainsBlouse".Localized());

			if (JojoCumQ() < 1000)
				OutputText("Scenes.Jojoscene.JojoPaysForPerms.MostSwallowedFinish".Localized());
			else
				if (JojoCumQ() < 4000)
					OutputText("Scenes.Jojoscene.JojoPaysForPerms.BellyGrowsSwallow".Localized());
				else
					OutputText("Scenes.Jojoscene.JojoPaysForPerms.BellyJetsOverflow".Localized());

			OutputText("Scenes.Jojoscene.JojoPaysForPerms.DaughterPipePussy".Localized());

			OutputText("Scenes.Jojoscene.JojoPaysForPerms.LynnettePants".Localized());

			if (JojoCumQ() >= 1000 && JojoCumQ() < 4000)
				OutputText("Scenes.Jojoscene.JojoPaysForPerms.SmilePhrase".Localized());
			else
				if (JojoCumQ() >= 4000)
					OutputText("Scenes.Jojoscene.JojoPaysForPerms.DazedCrazySmile".Localized());

			OutputText("Scenes.Jojoscene.JojoPaysForPerms.CommaSayShopGreeting".Localized());
		}

		public void AmilyTeachingJojoBJ()
		{
			ClearOutput();

			if (CoC.Instance.Flags.Get(GameFlagIds.TimesAmilyAndJojoPlayedTimes) == 0)
			{
				OutputText("Scenes.Jojoscene.AmilyTeachingJojoBJ.MiceMakeAppearance".Localized());

				OutputText("Scenes.Jojoscene.AmilyTeachingJojoBJ.AmilyBoxesPetEars".Localized((noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));
			}
			else
			{
				OutputText("Scenes.Jojoscene.AmilyTeachingJojoBJ.MiceReuniteAtCamp".Localized());

				OutputText("Scenes.Jojoscene.AmilyTeachingJojoBJ.DemonTailChides".Localized());
			}

			CoC.Instance.Flags.Set(GameFlagIds.TimesAmilyAndJojoPlayedTimes, CoC.Instance.Flags.Get(GameFlagIds.TimesAmilyAndJojoPlayedTimes) + 1);

			OutputText("Scenes.Jojoscene.AmilyTeachingJojoBJ.DistractedByBoner".Localized());

			if (Player.HasCock())
				OutputText("Scenes.Jojoscene.AmilyTeachingJojoBJ.SurpriseThemBoth".Localized());
			else
				OutputText("Scenes.Jojoscene.AmilyTeachingJojoBJ.WatchThemFuck".Localized());

			ClearButtons();

			if (Player.HasCock())
				AddButton(0, "Common.Button.Demonstrate".Localized(), BJRidesGETYOUONE);

			AddButton(1, "Common.Button.Watch".Localized(), AmilyAndJojoFuck);

			AddButton(14, "Common.Button.Leave".Localized(), () => Go<CampScene>());
		}

		public void BJRidesGETYOUONE()
		{
			ClearOutput();

			OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.AskAboutFuss".Localized());

			OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.TaintedMouseGazes".Localized());

			OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.AmilyCaressesIt".Localized());

			if (Player.BiggestCockArea() > 50)
				OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.AsMuchAsPossible".Localized());
			else
				OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.YourEntireLength".Localized());

			OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.IntoMouthAndThroat".Localized());

			OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.EnjoysWetSuction".Localized());

			OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.AmilyPumpsFaster".Localized());

			OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.JojoLeansCloser".Localized());

			OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.DualSensationGroan".Localized());

			if (!!Player.HasFur())
				OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.MouthWordPlural".Localized());
			else
				OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.MuzzleWordPlural".Localized());

			OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.AlmostTooGreat".Localized());

			OutputText("Scenes.Jojoscene.BJRidesGETYOUONE.WhereDoYouCum".Localized());

			ClearButtons();

			AddButton(0, "Common.Button.AmilysMouth".Localized(), FillAmilysMouth);

			if (Player.Tallness > 55)
				AddButton(1, "Common.Button.AmilysPussy".Localized(), StuffAmilysMouthWithPostBJCUM);
			else
				AddButton(1, "Common.Button.AmilysPussy".Localized(), null);

			AddButton(2, "Common.Button.JojosMouth".Localized(), FillJojosMouthWithHotStickyCum);
		}

		public void FillAmilysMouth()
		{
			ClearOutput();

			OutputText("Scenes.Jojoscene.FillAmilysMouth.OpenWideCommand".Localized());

			if (Player.Balls > 0)
				OutputText("Scenes.Jojoscene.FillAmilysMouth.DesperateSackSuckle".Localized());
			else
				OutputText("Scenes.Jojoscene.FillAmilysMouth.CockBaseSlurp".Localized());

			OutputText("Scenes.Jojoscene.FillAmilysMouth.AmilyLooksUpPleading".Localized());

			OutputText("Scenes.Jojoscene.FillAmilysMouth.EngorgedLipsDevour".Localized());

			OutputText("Scenes.Jojoscene.FillAmilysMouth.SpermFilledBurp".Localized());

			OutputText("Scenes.Jojoscene.FillAmilysMouth.WhoreHugsYourLeg".Localized());

			Player.Orgasm("Dick");

			DoNext(() => ReturnToCamp(1));
		}

		public void StuffAmilysMouthWithPostBJCUM()
		{
			ClearOutput();

			OutputText("Scenes.Jojoscene.StuffAmilysMouthWithPostBJCUM.HurryUpCommand".Localized());

			OutputText("Scenes.Jojoscene.StuffAmilysMouthWithPostBJCUM.PushHerDeeper".Localized());

			OutputText("Scenes.Jojoscene.StuffAmilysMouthWithPostBJCUM.RigidFuckTool".Localized());

			OutputText("Scenes.Jojoscene.StuffAmilysMouthWithPostBJCUM.SqueakOfSurprise".Localized());

			OutputText("Scenes.Jojoscene.StuffAmilysMouthWithPostBJCUM.PullOutSatisfied".Localized((noFur ? "Common.Text.Tongue".Localized() : "Common.Text.Muzzle".Localized())));

			OutputText("Scenes.Jojoscene.StuffAmilysMouthWithPostBJCUM.LeaveWithSpring".Localized());

			Player.Orgasm("Dick");

			Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

			DoNext(() => ReturnToCamp(1));
		}

		public void FillJojosMouthWithHotStickyCum()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.FillJojosMouthWithHotStickyCum.PointAndCommand".Localized((noFur ? "Common.Text.Mouth".Localized() : "Common.Text.Muzzle".Localized())));

			OutputText("Scenes.Jojoscene.FillJojosMouthWithHotStickyCum.BlissExplosion".Localized((noFur ? "Common.Text.Skin".Localized() : "Common.Text.Fur".Localized()), (noFur ? "" : " and fur")));

			OutputText("Scenes.Jojoscene.FillJojosMouthWithHotStickyCum.MouseToyPolishing".Localized());

			Player.Orgasm("Dick");

			Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Corruption, 1f));

			DoNext(() => ReturnToCamp(1));
		}

		public void AmilyAndJojoFuck()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.PerchOnRock".Localized());

			OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.JojoBegsToCum".Localized((noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));

			OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.ClitLickDemand".Localized((noFur ? "Common.Text.FaceDarkens".Localized() : "Common.Text.MuzzleAndFaceDarken".Localized())));

			OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.JojoMountsBehind".Localized((noFur ? "" : "white-furred ")));

			OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.DemonicTailTwitch".Localized((noFur ? "-girl" : ""), (noFur ? "" : " lightly-furred")));

			OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.ChidesBetweenThrusts".Localized());

			OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.FrenziedPounding".Localized());

			OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.HairPullGrowl".Localized());

			OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.ObservingHandiwork".Localized());

			OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.SlideDownRock".Localized());

			if (Player.HasVagina())
				OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.WetCrotchRealization".Localized());
			else
				OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.ClearheadedRealization".Localized());

			OutputText("Scenes.Jojoscene.AmilyAndJojoFuck.CatchThemInAct".Localized());

			Player.Orgasm("Generic");

			DoNext(() => ReturnToCamp(1));
		}

		public void JojoFollowerMeditate()
		{
			ClearOutput();

			JojoSprite();

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoLastMeditation) == View.Time.Days)
			{
				OutputText("Scenes.Jojoscene.JojoFollowerMeditate.MeditateTogether".Localized());

				Player.ApplyStats(new DynStat(GameStats.Lust, -30f));
			}
			else
			{
				OutputText("Scenes.Jojoscene.JojoFollowerMeditate.MonkLeadsAway".Localized());

				OutputText("Scenes.Jojoscene.JojoFollowerMeditate.JojoNodsThinking".Localized());

				Player.ApplyStats(new DynStat(GameStats.Lust, -30f));

				var cleanse = -2;

				if (Player.Corruption > 80)
					cleanse -= 3;
				else
					if (Player.Corruption > 60)
						cleanse -= 2;
					else
						if (Player.Corruption > 40)
							cleanse -= 1;

				Player.ApplyStats(new DynStat(GameStats.Corruption, cleanse - Player.CountCockSocks("alabaster")));

				if ((100 * Player.Strength / Player.GetMaxStats("str")) < 45)
					Player.ApplyStats(new DynStat(GameStats.Strength, 1f));

				if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 45)
					Player.ApplyStats(new DynStat(GameStats.Toughness, 1f));

				if ((100 * Player.Speed / Player.GetMaxStats("spe")) < 75)
					Player.ApplyStats(new DynStat(GameStats.Speed, 1f));

				if (Player.Intelligence < 80)
					Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

				if (Player.LibidoPercent > 0)
					Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

				CoC.Instance.Flags.Set(GameFlagIds.JojoLastMeditation, View.Time.Days);

				Player.ChangeStatusValue(StatusEffectIds.JojoMeditationCount, 0, 1);
			}

			DoNext(() => ReturnToCamp(1));
		}

		public void JojoDefenseToggle()
		{
			ClearOutput();

			JojoSprite();

			if (CampGuards.IsGuard("Jojo"))
			{
				Player.RemoveStatusEffect(StatusEffectIds.JojoNightWatch);

				OutputText("Scenes.Jojoscene.JojoDefenseToggle.DismissNightWatch".Localized());
			}
			else
			{
				Player.CreateStatusEffect(StatusEffectIds.JojoNightWatch, 0, 0, 0, 0);

				OutputText("Scenes.Jojoscene.JojoDefenseToggle.AssignNightWatch".Localized());
			}

			DoNext(() => JojoCamp());
		}

		public void JojoAtCampRape()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.JojoAtCampRape.SuggestWoodsHunt".Localized());

			OutputText("Scenes.Jojoscene.JojoAtCampRape.ProactiveApproach".Localized());

			OutputText("Scenes.Jojoscene.JojoAtCampRape.RestrictingOpenLayout".Localized());

			ClearButtons();

			AddNextButton("Common.Button.Rape".Localized(), () => JojoRape());

			AddNextButton("Scenes.CorruptedGlade.NeverMind".Localized(), JojoNoRape);
		}

		public void JojoNoRape()
		{
			ClearOutput();

			OutputText("Scenes.Jojoscene.JojoNoRape.NoticingHesitation".Localized());

			OutputText("Scenes.Jojoscene.JojoNoRape.BrushAsideWorry".Localized());

			if (Player.EmptySlot() >= 0)
			{

			}

			OutputText("Scenes.Jojoscene.JojoNoRape.UneventfulOuting".Localized());

			OutputText("Scenes.Jojoscene.JojoNoRape.CampReturnInterrupted".Localized());

			OutputText("Scenes.Jojoscene.JojoNoRape.ErrantThought".Localized());

			DoNext(() => ReturnToCamp(1));
		}

		public void DefeatedJojo(bool hpVictory)
		{
			ClearOutput();

			JojoSprite();

			if (Player.Lust > 33 && (int)Player.Gender > 0)
			{
				OutputText("Scenes.Jojoscene.DefeatedJojo.SatisfiedButUnsated".Localized((hpVictory ? "Common.Text.CollapsesUnableToContinueFighting".Localized() : "Common.Text.CollapsesAndBeginsMasturbatingFeverishly".Localized())));

				OutputText("Scenes.Jojoscene.DefeatedJojo.RapeHimChoice".Localized());

				AddButton(0, "Common.Button.Yes".Localized(), PostCombatRape);

				AddButton(1, "Common.Button.No".Localized(), CombatScene.CleanupAfterCombat);
			}
			else
				CombatScene.CleanupAfterCombat();
		}

		public void PostCombatRape()
		{

			JojoSprite();

			OutputText("Scenes.Jojoscene.PostCombatRape.DisrobeAndPrepare".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) == 5)
				OutputText("Scenes.Jojoscene.PostCombatRape.FuckSlutSenseless".Localized());
			else
				OutputText("Scenes.Jojoscene.PostCombatRape.UppityMonkLesson".Localized());

			ClearButtons();

			AddButton(0, "Common.Button.Next".Localized(), () => JojoRape(true));
		}

		public void JojoRape(bool postCombat = false)
		{

			JojoSprite();

			Player.SlimeFeed();

			Player.RemoveStatusEffect(StatusEffectIds.JojoNightWatch);

			Player.RemoveStatusEffect(StatusEffectIds.PureCampJojo);

			if (!Player.HasStatusEffect(StatusEffectIds.EverRapedJojo))
				Player.CreateStatusEffect(StatusEffectIds.EverRapedJojo, 1, 0, 0, 0);
			else
				Player.ChangeStatusValue(StatusEffectIds.EverRapedJojo, 0, 1);

			var __sw0_0 = CoC.Instance.Flags.Get(GameFlagIds.JojoStatus);

			if (__sw0_0 == 0 || __sw0_0 == 1)
			{
				if (!postCombat && Save.GetBool("forgiven"))
					JojoPredictsRape();
				else
					JojosFirstRape(postCombat);

				return;
			}

			if (__sw0_0 == 2)
				JojosSecondRape();

			if (__sw0_0 == 3)
				JojosThirdRape();

			if (__sw0_0 == 4)
				JojosFourthRape();

			if (__sw0_0 == 5)
				JojosFifthRape();

			CombatScene.CleanupAfterCombat();
		}

		public void JojoPredictsRape()
		{
			ClearOutput();

			OutputText("Scenes.Jojoscene.JojoPredictsRape.CloseInOnMonk".Localized());

			OutputText("Scenes.Jojoscene.JojoPredictsRape.SternSadExpression".Localized());

			OutputText("Scenes.Jojoscene.JojoPredictsRape.HopesToPurgeLust".Localized());

			CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "jojo")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
		}

		public void JojosFirstRape(bool postCombat = false)
		{
			ClearOutput();

			if (!postCombat)
				OutputText("Scenes.Jojoscene.JojosFirstRape.PretendToAgree".Localized());

			if (Player.Gender == Gender.Male)
			{
				OutputText("Scenes.Jojoscene.JojosFirstRape.PushAndPin".Localized());

				if (Player.CockTotal() == 1)
				{
					OutputText("Scenes.Jojoscene.JojosFirstRape.GrinPressPuckered".Localized(Player.CockDescript(0), (noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));

					OutputText("Scenes.Jojoscene.JojosFirstRape.PullOutSemicolon".Localized());

					Player.Orgasm("Dick");

					Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, 4f));

					CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);
				}
				else
				{
					OutputText("Scenes.Jojoscene.JojosFirstRape.GrinPressCocksSoak".Localized((noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));

					OutputText("Scenes.Jojoscene.JojosFirstRape.PullOutComma".Localized());

					Player.Orgasm("Dick");

					Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, 4f));

					CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);
				}
			}
			else
				if (Player.Gender == Gender.Female)
				{
					OutputText("Scenes.Jojoscene.JojosFirstRape.SmackHeadDazing".Localized());

					if (Player.Wetness() < 2)
						OutputText("Scenes.Jojoscene.JojosFirstRape.FoldsGrind".Localized());
					else
						OutputText("Scenes.Jojoscene.JojosFirstRape.GrindDrainsWill".Localized(
							Player.VaginaDescript(0)
						));

					OutputText("Scenes.Jojoscene.JojosFirstRape.MarvelAtHisSize".Localized());

					Player.CuntChange(12, true, true, false);

					OutputText("Scenes.Jojoscene.JojosFirstRape.ParagraphBreak".Localized());

					OutputText("Scenes.Jojoscene.JojosFirstRape.GruntsAndWhines".Localized(
						Player.VaginaDescript(0),

						Player.VaginaDescript(0)
					));

					if (Player.BiggestLactation() >= 1 && Player.BiggestLactation() < 2)
						OutputText("Scenes.Jojoscene.JojosFirstRape.MilkSquirtSpray".Localized());

					if (Player.BiggestLactation() >= 2 && Player.BiggestLactation() < 3)
						OutputText("Scenes.Jojoscene.JojosFirstRape.MilkStreamSpray".Localized((noFur ? "" : " and puddling on his fur")));

					if (Player.BiggestLactation() >= 3)
						OutputText("Scenes.Jojoscene.JojosFirstRape.MilkGoutDrench".Localized());

					OutputText("Scenes.Jojoscene.JojosFirstRape.WobblyLegsTriumph".Localized());

					Player.Orgasm("Vaginal");

					Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, 4f));

					CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);

					Player.KnockUp(PregnancyIds.Jojo, PregnancyIds.MouseIncubation + 82);
				}
				else
					if (Player.Gender == Gender.Hermaphrodite)
					{
						OutputText("Scenes.Jojoscene.JojosFirstRape.PushAndPinAgain".Localized());

						if (Player.CockTotal() == 1)
						{
							OutputText("Scenes.Jojoscene.JojosFirstRape.GrinPressTight".Localized(Player.CockDescript(0), (noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));

							if (Player.BiggestLactation() >= 1 && Player.BiggestLactation() < 2)
								OutputText("Scenes.Jojoscene.JojosFirstRape.MilkSquirtSpace".Localized());

							if (Player.BiggestLactation() >= 2 && Player.BiggestLactation() < 3)
								OutputText("Scenes.Jojoscene.JojosFirstRape.MilkStreamSpace".Localized((noFur ? "" : " and puddling on his fur")));

							if (Player.BiggestLactation() >= 3)
								OutputText("Scenes.Jojoscene.JojosFirstRape.MilkGoutSpace".Localized());

							OutputText("Scenes.Jojoscene.JojosFirstRape.PullOutParagraph".Localized());

							Player.Orgasm("Dick");

							Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, 4f));

							CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);
						}
						else
						{
							OutputText("Scenes.Jojoscene.JojosFirstRape.GrinPressCocksVow".Localized((noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));

							if (Player.BiggestLactation() >= 1 && Player.BiggestLactation() < 2)
								OutputText("Scenes.Jojoscene.JojosFirstRape.MilkSquirtSpaceAgain".Localized());

							if (Player.BiggestLactation() >= 2 && Player.BiggestLactation() < 3)
								OutputText("Scenes.Jojoscene.JojosFirstRape.MilkStreamSpaceAgain".Localized((noFur ? "" : " and puddling on his fur")));

							if (Player.BiggestLactation() >= 3)
								OutputText("Scenes.Jojoscene.JojosFirstRape.MilkGoutSpaceAgain".Localized());

							OutputText("Scenes.Jojoscene.JojosFirstRape.PullOutParaAgain".Localized());

							Player.Orgasm("Dick");

							Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, 4f));

							CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);
						}
					}

			CombatScene.CleanupAfterCombat();
		}

		public void JojosSecondRape()
		{
			ClearOutput();

			OutputText("Scenes.Jojoscene.JojosSecondRape.PoorMouseRigid".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);

			Player.Orgasm("Generic");

			Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, 4f));

			if (Player.Gender == Gender.Male)
			{
				OutputText("Scenes.Jojoscene.JojosSecondRape.ForceOverLog".Localized((noFur ? "Common.Text.OverHisBody".Localized() : "Common.Text.ThroughHisFur".Localized())));

				if (Player.AverageCockThickness() >= 3)
					OutputText("Scenes.Jojoscene.JojosSecondRape.TightAsVirgin".Localized());

				OutputText("Scenes.Jojoscene.JojosSecondRape.GrabWaistAndPound".Localized((noFur ? "Common.Text.Legs".Localized() : "Common.Text.Fur".Localized())));

				if (Player.CockTotal() > 1)
					OutputText("Scenes.Jojoscene.JojosSecondRape.RemainingCockSpray".Localized());

				OutputText("Scenes.Jojoscene.JojosSecondRape.MoansAndCums".Localized());

				OutputText("Scenes.Jojoscene.JojosSecondRape.DiscardSpentMouse".Localized());
			}

			if (Player.Gender == Gender.Female || Player.Gender == Gender.Hermaphrodite)
			{
				OutputText("Scenes.Jojoscene.JojosSecondRape.ThrowOntoSoil".Localized(
					Player.VaginaDescript(0),

					Player.VaginaDescript(0)
				));

				Player.CuntChange(1.5f, true);

				if (Player.BiggestLactation() >= 1 && Player.BiggestLactation() < 2)
					OutputText("Scenes.Jojoscene.JojosSecondRape.MilkSquirtSpray".Localized());

				if (Player.BiggestLactation() >= 2 && Player.BiggestLactation() < 3)
					OutputText("Scenes.Jojoscene.JojosSecondRape.MilkStreamSpray".Localized((noFur ? "" : " and puddling on his fur")));

				if (Player.BiggestLactation() >= 3)
					OutputText("Scenes.Jojoscene.JojosSecondRape.MilkGoutDrench".Localized());

				if (Player.AverageVaginalWetness() == 5)
					OutputText("Scenes.Jojoscene.JojosSecondRape.SquirtingDrench".Localized(
						Player.VaginaDescript(0)
					));

				if (Player.CockTotal() == 1)
					OutputText("Scenes.Jojoscene.JojosSecondRape.ChestJizzSpray".Localized());

				if (Player.CockTotal() == 2)
					OutputText("Scenes.Jojoscene.JojosSecondRape.JizzHailSplatter".Localized());

				if (Player.CockTotal() == 3)
					OutputText("Scenes.Jojoscene.JojosSecondRape.ThickCumTorrent".Localized());

				OutputText("Scenes.Jojoscene.JojosSecondRape.SatisfiedPullAway".Localized());

				Player.KnockUp(PregnancyIds.Jojo, PregnancyIds.MouseIncubation + 82);
			}
		}

		public void JojosThirdRape()
		{
			ClearOutput();

			OutputText("Scenes.Jojoscene.JojosThirdRape.MonkBodyBetrayed".Localized(
				CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
			));

			if (Player.Gender == Gender.Male)
			{
				OutputText("Scenes.Jojoscene.JojosThirdRape.YankJojoOntoKnees".Localized());

				if (Player.CockTotal() == 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.PresentCockToHim".Localized());
				else
					OutputText("Scenes.Jojoscene.JojosThirdRape.PresentCocksToHim".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.MonkOpensWide".Localized());

				if (Player.CockTotal() == 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.RamCockIntoMaw".Localized());

				if (Player.CockTotal() > 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.CramTwoCocksDeep".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.JojoHandInGroin".Localized((noFur ? "Common.Text.Hands".Localized() : "BodyParts.Descript.LowerBody.Feet.Fox".Localized()), CurrentMonster!.CockDescriptShort(0)));

				if (Player.CockTotal() == 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.CockClenchesErupting".Localized());

				if (Player.CockTotal() > 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.TwinDicksErupting".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.SeedIntoMeekThroat".Localized());

				if (Player.Libido > 60 && Player.Corruption > 40)
				{
					OutputText("Scenes.Jojoscene.JojosThirdRape.FlipHimOntoBack".Localized());

					if (Player.CockTotal() == 1)
						OutputText("Scenes.Jojoscene.JojosThirdRape.ViolateSphincterOwnCock".Localized());
					else
						OutputText("Scenes.Jojoscene.JojosThirdRape.ViolateSphincterGenericCock".Localized());
				}

				OutputText("Scenes.Jojoscene.JojosThirdRape.LeaveMouseyBehind".Localized());

				Player.Orgasm("Dick");

				if (Player.Libido > 60 && Player.Corruption > 40)
				{
					OutputText("Scenes.Jojoscene.JojosThirdRape.HearHimMasturbate".Localized());

					CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);

					Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, 4f));
				}
				else
					Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Corruption, 1f));
			}

			if (Player.Gender == Gender.Female)
			{
				if (Player.IsBiped())
					OutputText("Scenes.Jojoscene.JojosThirdRape.SpreadLegsAndCrook".Localized());
				else
					OutputText("Scenes.Jojoscene.JojosThirdRape.CrookFinger".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.BeckonHimToKneel".Localized(
					Player.VaginaDescript(0),

					noFur ? "" : "whiskers tickling your " + Player.VaginaDescript(0) + ", his wet "
				));

				if (Player.BiggestBreastSize() >= 2)
					OutputText("Scenes.Jojoscene.JojosThirdRape.SighKneadBreasts".Localized());

				if (Player.BiggestLactation() >= 1.5f && Player.BiggestBreastSize() > 2 && Player.MostBreastsPerRow() >= 2 && Player.BreastRows.Count >= 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.SensualPeakMilk".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.MouseyEatsBoxTypo".Localized());

				if (Player.HasTailInsteadOfLegs())
					OutputText("Scenes.Jojoscene.JojosThirdRape.CoilsIt".Localized());
				else
					if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Goo)
						OutputText("Scenes.Jojoscene.JojosThirdRape.JigglingGooIt".Localized());
					else
						OutputText("Scenes.Jojoscene.JojosThirdRape.ThighsThey".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.QuiverAroundHisHead".Localized());

				if (Player.AverageVaginalWetness() <= 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.SqueezingHimTightly".Localized(
						Player.VaginaDescript(0)
					));

				if (Player.AverageVaginalWetness() > 1 && Player.AverageVaginalWetness() <= 3)
					OutputText("Scenes.Jojoscene.JojosThirdRape.CreamHisTongue".Localized());

				if (Player.AverageVaginalWetness() == 4)
					OutputText("Scenes.Jojoscene.JojosThirdRape.CreamAllOverHim".Localized());

				if (Player.AverageVaginalWetness() == 5)
					OutputText("Scenes.Jojoscene.JojosThirdRape.SplatterGirlcumOnHim".Localized(
						Player.VaginaDescript(0)
					));

				OutputText("Scenes.Jojoscene.JojosThirdRape.PullCuntDazedMouse".Localized());

				if (Player.IsBiped())
					OutputText("Scenes.Jojoscene.JojosThirdRape.SpreadingLegsAnd".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.ShakingRump".Localized());

				if (Player.HasLongTail())
					OutputText("Scenes.Jojoscene.JojosThirdRape.TailSwishesTeasingly".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.TantalizingBackside".Localized(
					Player.VaginaDescript(0),

					Player.VaginaDescript(0),

					CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
				));

				Player.CuntChange(2, true);

				OutputText("Scenes.Jojoscene.JojosThirdRape.ParagraphBreak".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.DrainedMousePopFree".Localized());

				if (Player.IsBiped())
					OutputText("Scenes.Jojoscene.JojosThirdRape.SpacedBetweenThighs".Localized());
				else
					OutputText("Scenes.Jojoscene.JojosThirdRape.SpacedCuntIt".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.JojoPassedOutBehind".Localized());

				Player.Orgasm("Vaginal");

				if (Player.Libido > 60 && Player.Corruption > 50)
				{
					OutputText("Scenes.Jojoscene.JojosThirdRape.WhisperStrangeWords".Localized());

					CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);

					Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, 4f));
				}
				else
					Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Corruption, 1f));

				Player.KnockUp(PregnancyIds.Jojo, PregnancyIds.MouseIncubation + 82);
			}

			if (Player.Gender == Gender.Hermaphrodite)
			{
				if (Player.IsBiped())
					OutputText("Scenes.Jojoscene.JojosThirdRape.SpreadLegsCrookFinger".Localized());
				else
					OutputText("Scenes.Jojoscene.JojosThirdRape.CrookFingerAgain".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.BeckonHimKneelAgain".Localized(Player.VaginaDescript(0), (noFur ? "" : "whiskers tickling your " + Player.VaginaDescript(0) + ", his wet ")));

				if (Player.CockTotal() == 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.SighCockDroops".Localized());

				if (Player.CockTotal() > 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.SighCocksPileAtop".Localized());

				if (Player.BiggestBreastSize() >= 2)
					OutputText("Scenes.Jojoscene.JojosThirdRape.KneadBreastsExcited".Localized());

				if (Player.BiggestLactation() >= 1.5f && Player.BiggestBreastSize() > 2 && Player.MostBreastsPerRow() >= 2 && Player.BreastRows.Count >= 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.SensualPeakMilkAgain".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.MouseyEatsBoxClean".Localized());

				if (Player.AverageVaginalWetness() <= 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.SqueezingHimTightlyAgain".Localized(
						Player.VaginaDescript(0)
					));

				if (Player.AverageVaginalWetness() > 1 && Player.AverageVaginalWetness() < 4)
					OutputText("Scenes.Jojoscene.JojosThirdRape.CreamHisTongueAgain".Localized());

				if (Player.AverageVaginalWetness() == 4)
					OutputText("Scenes.Jojoscene.JojosThirdRape.CreamOverHimAgain".Localized());

				if (Player.AverageVaginalWetness() >= 5)
					OutputText("Scenes.Jojoscene.JojosThirdRape.SplatterYouGirlcum".Localized(
						Player.VaginaDescript(0)
					));

				if (Player.CockTotal() > 0)
					OutputText("Scenes.Jojoscene.JojosThirdRape.PreDribblesHisNeck".Localized((noFur ? "Common.Text.Skin".Localized() : "Common.Text.Fur".Localized())));

				OutputText("Scenes.Jojoscene.JojosThirdRape.PullDazedMouseAgain".Localized());

				if (Player.IsBiped())
					OutputText("Scenes.Jojoscene.JojosThirdRape.SpreadingLegsAgain".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.ShakingRumpAgain".Localized());

				if (Player.HasLongTail())
					OutputText("Scenes.Jojoscene.JojosThirdRape.TailSwishesTeasingAgain".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.BacksidePounceForceful".Localized(
					Player.VaginaDescript(0),

					Player.VaginaDescript(0),

					CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
				));

				Player.CuntChange(2, true);

				if (Player.CockTotal() == 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.CockTremblesOrgasm".Localized());

				if (Player.CockTotal() > 1)
					OutputText("Scenes.Jojoscene.JojosThirdRape.CocksTrembleOrgasm".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.ParagraphBreakAgain".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.DrainedMousePopSpace".Localized());

				if (Player.IsBiped())
					OutputText("Scenes.Jojoscene.JojosThirdRape.BetweenThighs".Localized());
				else
					OutputText("Scenes.Jojoscene.JojosThirdRape.CuntIt".Localized());

				OutputText("Scenes.Jojoscene.JojosThirdRape.JojoUnconsciousBehind".Localized());

				Player.Orgasm("Generic");

				if (Player.Libido > 60 && Player.Corruption > 50)
				{
					OutputText("Scenes.Jojoscene.JojosThirdRape.WhisperStrangeUnwords".Localized());

					CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);

					Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, 4f));
				}
				else
					Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Corruption, 1f));

				Player.KnockUp(PregnancyIds.Jojo, PregnancyIds.MouseIncubation + 82);
			}
		}

		public void JojosFourthRape()
		{
			ClearOutput();

			OutputText("Scenes.Jojoscene.JojosFourthRape.JojoFlopsDownEager".Localized());

			if (Player.Gender == Gender.Male)
			{
				OutputText("Scenes.Jojoscene.JojosFourthRape.FormerMonkKneels".Localized());

				if (Player.CockTotal() == 1)
					OutputText("Scenes.Jojoscene.JojosFourthRape.TouchHisHeadGently".Localized(Player.CockDescript(0), (noFur ? "" : " fuzzy")));
				else
					OutputText("Scenes.Jojoscene.JojosFourthRape.TouchHisHeadAnticipate".Localized((noFur ? "" : " fuzzy")));

				if (Player.AverageCockThickness() > 3 || (Player.DogCocks() > 0 && Player.AverageCockThickness() > 3))
					OutputText("Scenes.Jojoscene.JojosFourthRape.JojoGaspForAir".Localized());

				if (AverageCockLength() > 10 || (Player.CountCocksOfType("Horse") > 0 && AverageCockLength() > 10))
					OutputText("Scenes.Jojoscene.JojosFourthRape.StrugglesNotToGag".Localized());

				OutputText("Scenes.Jojoscene.JojosFourthRape.RockBackAndForth".Localized());

				if (Player.CumQ() > 25)
					OutputText("Scenes.Jojoscene.JojosFourthRape.HeavyPreChokes".Localized());

				OutputText("Scenes.Jojoscene.JojosFourthRape.JojoPressesDeep".Localized((noFur ? "Common.Text.Hand".Localized() : "Common.Text.Paw".Localized()), (noFur ? "Common.Text.Mouth".Localized() : "Common.Text.Muzzle".Localized())));

				if (Player.CumQ() < 25)
					OutputText("Scenes.Jojoscene.JojosFourthRape.BucksTinySquirts".Localized());

				if (Player.CumQ() >= 25 && Player.CumQ() < 250)
					OutputText("Scenes.Jojoscene.JojosFourthRape.BucksThroatSqueeze".Localized());

				if (Player.CumQ() >= 250 && Player.CumQ() < 500)
					OutputText("Scenes.Jojoscene.JojosFourthRape.SpasmsIntoThroat".Localized());

				if (Player.CumQ() >= 500)
					OutputText("Scenes.Jojoscene.JojosFourthRape.BucksCheeksBulge".Localized((noFur ? "Common.Text.Lips".Localized() : "Common.Text.Muzzle".Localized())));

				OutputText("Scenes.Jojoscene.JojosFourthRape.JojoPantsSpitsCum".Localized((noFur ? "Common.Text.Hand".Localized() : "Common.Text.Paw".Localized())));

				if (Player.Libido > 50 && Player.Corruption > 80)
				{
					OutputText("Scenes.Jojoscene.JojosFourthRape.FamiliarPowerUnleashed".Localized());

					OutputText("Scenes.Jojoscene.JojosFourthRape.WhimpersFirstBead".Localized((noFur ? "Common.Text.Hands".Localized() : "BodyParts.Descript.LowerBody.Feet.Fox".Localized()), CurrentMonster!.CockDescriptShort(0)));

					OutputText("Scenes.Jojoscene.JojosFourthRape.WalksAwayFromPet".Localized());

					CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);

					Player.Orgasm("Dick");

					Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, 10f));
				}
				else
				{
					OutputText("Scenes.Jojoscene.JojosFourthRape.JojoCumsCollapsing".Localized());

					Player.Orgasm("Dick");

					Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Corruption, 1f));
				}
			}

			if (Player.Gender == Gender.Female || Player.Gender == Gender.Hermaphrodite)
			{
				OutputText("Scenes.Jojoscene.JojosFourthRape.OnceMonkKneels".Localized());

				OutputText("Scenes.Jojoscene.JojosFourthRape.TouchHeadGuiding".Localized(Player.VaginaDescript(0), (noFur ? "" : "wet "), Player.VaginaDescript(0)));

				OutputText("Scenes.Jojoscene.JojosFourthRape.JojoMoansIntoFolds".Localized(
					CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
				));

				if (Player.IsBiped())
					OutputText("Scenes.Jojoscene.JojosFourthRape.BrushesYourCalf".Localized());
				else
					OutputText("Scenes.Jojoscene.JojosFourthRape.BrushesYourLeg".Localized());

				OutputText("Scenes.Jojoscene.JojosFourthRape.DevilishIdea".Localized());

				if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Goo)
					OutputText("Scenes.Jojoscene.JojosFourthRape.PushesBetweenFolds".Localized(
						CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

						Player.VaginaDescript(0)
					));
				else
					OutputText("Scenes.Jojoscene.JojosFourthRape.SandwichBetweenLegs".Localized(
						CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

						Player.Foot(),

						Player.Foot(),

						Player.VaginaDescript(0),

						Player.Feet()
					));

				if (Player.AverageVaginalWetness() <= 1)
					OutputText("Scenes.Jojoscene.JojosFourthRape.ClampsInOrgasm".Localized((noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));

				if (Player.AverageVaginalWetness() > 1 && Player.AverageVaginalWetness() <= 3)
					OutputText("Scenes.Jojoscene.JojosFourthRape.LegsTrapInOrgasm".Localized((noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized()), Player.VaginaDescript(0)));

				if (Player.AverageVaginalWetness() == 4)
					OutputText("Scenes.Jojoscene.JojosFourthRape.LegsTrapGirlcumSlick".Localized((noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized()), Player.VaginaDescript(0), (noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));

				if (Player.AverageVaginalWetness() == 5)
					OutputText("Scenes.Jojoscene.JojosFourthRape.LegsSqueezeFaceGirlcum".Localized(
						Player.VaginaDescript(0)
					));

				if (Player.CockTotal() == 1)
					OutputText("Scenes.Jojoscene.JojosFourthRape.DrizzleCumHair".Localized());

				if (Player.CockTotal() > 1)
					OutputText("Scenes.Jojoscene.JojosFourthRape.CocksDrizzleCum".Localized());

				OutputText("Scenes.Jojoscene.JojosFourthRape.FallBackTwitching".Localized());

				OutputText("Scenes.Jojoscene.JojosFourthRape.PleasureWantsMore".Localized(
					CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

					Player.VaginaDescript(0),

					CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
				));

				if (AverageVaginalLooseness() == 0)
					OutputText("Scenes.Jojoscene.JojosFourthRape.PainfullyLarge".Localized());

				if (AverageVaginalLooseness() == 1)
					OutputText("Scenes.Jojoscene.JojosFourthRape.StretchesLikeGlove".Localized(
						Player.VaginaDescript(0),

						CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
					));

				if (AverageVaginalLooseness() == 2)
					OutputText("Scenes.Jojoscene.JojosFourthRape.StuffsCompletely".Localized(
						CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

						Player.VaginaDescript(0)
					));

				if (AverageVaginalLooseness() == 3)
					OutputText("Scenes.Jojoscene.JojosFourthRape.FitsPerfectlyDeep".Localized(
						CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
					));

				if (AverageVaginalLooseness() == 4)
					OutputText("Scenes.Jojoscene.JojosFourthRape.EasilyAccommodates".Localized(
						Player.VaginaDescript(0)
					));

				if (AverageVaginalLooseness() == 5)
					OutputText("Scenes.Jojoscene.JojosFourthRape.SlipsInHilt".Localized(
						CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

						Player.VaginaDescript(0)
					));

				OutputText("Scenes.Jojoscene.JojosFourthRape.RidesSlowlyGyrating".Localized((noFur ? "Common.Text.OverHisBody".Localized() : "Common.Text.ThroughHisFur".Localized()), Player.VaginaDescript(0)));

				OutputText("Scenes.Jojoscene.JojosFourthRape.UpTempoBouncing".Localized(CurrentMonster!.CockDescriptShort(0), (noFur ? "Common.Text.Hair".Localized() : "Common.Text.Fur".Localized()), Player.VaginaDescript(0)));

				if (Player.CockTotal() > 0)
				{
					if (Player.CockTotal() == 1)
					{
						if (Player.CountCocksOfType("Horse") == 1)
							OutputText("Scenes.Jojoscene.JojosFourthRape.BuildingPressure".Localized(
								Player.CockDescript(0)
							));

						if (Player.HasKnot())
							OutputText("Scenes.Jojoscene.JojosFourthRape.IntensePressureKnot".Localized());

						if (Player.CountCocksOfType("Human") == 1)
							OutputText("Scenes.Jojoscene.JojosFourthRape.TwitchesContractions".Localized());

						if (Player.CumQ() < 25)
							OutputText("Scenes.Jojoscene.JojosFourthRape.BodyTensesSprays".Localized());

						if (Player.CumQ() >= 25 && Player.CumQ() < 250)
						{
							if (Player.CountCocksOfType("Horse") == 1)
								OutputText("Scenes.Jojoscene.JojosFourthRape.FlaresHorseCum".Localized());

							if (Player.HasKnot())
								OutputText("Scenes.Jojoscene.JojosFourthRape.PressureRelentsDoggie".Localized());

							if (Player.Cocks[0].Id == BodyPartTypes.Human || Player.Cocks[0].TypeIndex > 2)
								OutputText("Scenes.Jojoscene.JojosFourthRape.TensesThickEruption".Localized());
						}

						if (Player.CumQ() >= 250)
						{
							if (Player.CountCocksOfType("Horse") == 1)
								OutputText("Scenes.Jojoscene.JojosFourthRape.RipplesAnimalVigor".Localized());

							if (Player.HasKnot())
								OutputText("Scenes.Jojoscene.JojosFourthRape.ReleasesConstantStream".Localized());

							if (Player.Cocks[0].Id == BodyPartTypes.Human || Player.Cocks[0].TypeIndex > 2)
								OutputText("Scenes.Jojoscene.JojosFourthRape.TensesThickEruptionAgain".Localized());
						}
					}

					if (Player.CockTotal() > 1)
						OutputText("Scenes.Jojoscene.JojosFourthRape.BothCocksPressure".Localized());
				}

				if (Player.BiggestLactation() >= 1 && Player.BiggestBreastSize() > 3)
				{
					if (Player.BiggestBreastSize() * Player.BiggestLactation() * Player.BreastRows.Count < 8)
						OutputText("Scenes.Jojoscene.JojosFourthRape.MilkSpraysTiny".Localized());

					if (Player.BiggestBreastSize() * Player.BiggestLactation() * Player.BreastRows.Count < 20 && Player.BiggestBreastSize() * Player.BiggestLactation() * Player.BreastRows.Count >= 8)
						OutputText("Scenes.Jojoscene.JojosFourthRape.MilkEruptsOverMouse".Localized());

					if (Player.BiggestBreastSize() * Player.BiggestLactation() * Player.BreastRows.Count < 35 && Player.BiggestBreastSize() * Player.BiggestLactation() * Player.BreastRows.Count >= 20)
						OutputText("Scenes.Jojoscene.JojosFourthRape.MilkPulsingBlasts".Localized());

					if (Player.BiggestBreastSize() * Player.BiggestLactation() * Player.BreastRows.Count >= 35)
						OutputText("Scenes.Jojoscene.JojosFourthRape.MilkFountainSoaks".Localized());
				}

				Player.CuntChange(3, true);

				Player.KnockUp(PregnancyIds.Jojo, PregnancyIds.MouseIncubation + 82);

				if (Player.Libido > 50 && Player.Corruption > 80)
				{
					OutputText("Scenes.Jojoscene.JojosFourthRape.ParagraphLeadFamiliarPower".Localized());

					OutputText("Scenes.Jojoscene.JojosFourthRape.WhimpersFirstBeadAgain".Localized((noFur ? "Common.Text.Hands".Localized() : "BodyParts.Descript.LowerBody.Feet.Fox".Localized()), CurrentMonster!.CockDescriptShort(0)));

					OutputText("Scenes.Jojoscene.JojosFourthRape.LeavesNewPetAgain".Localized());

					CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);

					Player.Orgasm("Vaginal");

					Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, 10f));
				}
				else
				{
					OutputText("Scenes.Jojoscene.JojosFourthRape.ExhaustedPullsFree".Localized());

					Player.Orgasm("Vaginal");

					Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Corruption, 1f));
				}
			}
		}

		public void JojosFifthRape()
		{
			ClearOutput();

			OutputText("Scenes.Jojoscene.JojosFifthRape.JojoSmilesSerenely".Localized());

			OutputText("Scenes.Jojoscene.JojosFifthRape.FucksSlutForHours".Localized());

			Player.Orgasm("Generic");

			Player.ChangeFatigue(-20);

			if (Player.Libido > 40)
			{
				OutputText("Scenes.Jojoscene.JojosFifthRape.DoneClearHeaded".Localized());

				Player.ApplyStats(new DynStat(GameStats.Libido, -4f));
			}

			CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) + 1);
		}

		public void LoseToJojo()
		{
			ClearOutput();

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) == 2 || CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) == 3)
			{
				OutputText("Scenes.Jojoscene.LoseToJojo.JojoGlaresPrays".Localized());

				if (!Save.GetBool("forgiven"))
				{
					OutputText("Scenes.Jojoscene.LoseToJojo.WakeFindsNote".Localized());

					OutputText("Scenes.Jojoscene.LoseToJojo.FightingExorcisedDemons".Localized());

					OutputText("Scenes.Jojoscene.LoseToJojo.JojoSignature".Localized());

					Save.SetBool("forgiven", true);
				}

				Player.Orgasm("Generic");

				Player.ApplyStats(new DynStat(GameStats.Libido, -10f), new DynStat(GameStats.Corruption, -15f));

				CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, 1);
			}
			else
			{
				OutputText("Scenes.Jojoscene.LoseToJojo.JojoGrinsWickedly".Localized(
					CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
				));

				if (Player.Lust >= Player.MaxLust)
				{
					if (Player.Gender == Gender.Male)
					{
						OutputText("Scenes.Jojoscene.LoseToJojo.BendsOverDangling".Localized(
							CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
						));

						if (Player.CockTotal() == 1)
							OutputText("Scenes.Jojoscene.LoseToJojo.PreDripsProstate".Localized());

						if (Player.CockTotal() > 1)
							OutputText("Scenes.Jojoscene.LoseToJojo.PreDripsBothCocks".Localized());

						Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);
					}

					if ((int)Player.Gender >= 2)
					{
						OutputText("Scenes.Jojoscene.LoseToJojo.BendsOverOpenTargets".Localized(
							Player.VaginaDescript(0)
						));

						if (Player.CockTotal() > 1)
							OutputText("Scenes.Jojoscene.LoseToJojo.SplattersGroundCum".Localized());
						else
							OutputText("Scenes.Jojoscene.LoseToJojo.CumsManyMoreTimes".Localized(
								Player.VaginaDescript(0)
							));

						OutputText("Scenes.Jojoscene.LoseToJojo.BlacksOutUterusFilled".Localized());

						Player.CuntChange(CombatController.Instance!.Context.Enemy.Cocks[0].Thickness, true);

						Player.KnockUp(PregnancyIds.Mouse, PregnancyIds.MouseIncubation + 82, 101);
					}

					if (Player.Gender == Gender.Genderless)
					{
						OutputText("Scenes.Jojoscene.LoseToJojo.BendsOverWiggling".Localized(
							CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
						));

						Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

						OutputText("Scenes.Jojoscene.LoseToJojo.WarmFlashRelief".Localized());

						OutputText("Scenes.Jojoscene.LoseToJojo.BlacksOutGutDistended".Localized());
					}

					Player.SlimeFeed();

					HideUpDown();

					Player.Orgasm("VaginalAnal");

					Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

				}
				else
					OutputText("Scenes.Jojoscene.LoseToJojo.BlacksOutFromPain".Localized());
			}

			CombatScene.CleanupAfterCombat();
		}

		public void CorruptJojoSexMenu()
		{

			ClearButtons();

			if (Player.HasVagina())
			{
				AddButton(2, "Common.Button.GentleVaginal".Localized(), CorruptJojoVaginalGentle);

				AddButton(7, "Common.Button.VagSmother".Localized(), CorruptJojoVaginalSmother);

				AddButton(8, "Common.Button.AnalSmother".Localized(), CorruptJojoAnalSmother);
			}

			if (Player.HasCock())
			{
				AddButton(0, "Common.Button.GentleBj".Localized(), CorruptJojoBJGentle);

				AddButton(5, "Common.Button.CruelBj".Localized(), CorruptJojoBJCruel);

				AddButton(3, "Common.Button.GentleAnal".Localized(), CorruptJojoAnalGentle);

				if (Player.HasPerk(PerkIds.Whispered))
					AddButton(8, "Combat.Ability.RiddleDemonWhisper.Name".Localized(), WhisperJojobait);
				else
					AddButton(8, "Common.Button.CruelAnal".Localized(), CorruptJojoAnalCruel);
			}

			AddButton(1, "Common.Button.GiveBj".Localized(), CorruptJojoCunnilingus);

			if (Player.BiggestBreastSize() >= 2)
				AddButton(6, (Player.BiggestLactation() > 1 ? "Common.Button.Suckle".Localized() : "Common.Button.Breasts".Localized()), CorruptJojoBreasts);

			AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
		}

		public void CorruptJojoBJCruel()
		{
			ClearOutput();

			JojoSprite();

			var x = Player.BiggestCockIndex();

			OutputText("Scenes.Jojoscene.CorruptJojoBJCruel.CallsSlutJungle".Localized((noFur ? "Common.Text.Hair".Localized() : "Common.Text.Fur".Localized())));

			OutputText("Scenes.Jojoscene.CorruptJojoBJCruel.ShakesHeadResists".Localized());

			if (Player.CockArea(x) < 10)
				OutputText("Scenes.Jojoscene.CorruptJojoBJCruel.GripsHeadPullsMouth".Localized((noFur ? "Common.Text.Hair".Localized() : "Common.Text.Fur".Localized()), Player.CockDescript(x), (noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));
			else
				if (Player.CockArea(x) < 36)
					OutputText("Scenes.Jojoscene.CorruptJojoBJCruel.GripsPullsMouthAgain".Localized((noFur ? "Common.Text.Hair".Localized() : "Common.Text.Fur".Localized()), Player.CockDescript(x), (noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized()), Player.CockDescript(x)));
				else
					OutputText("Scenes.Jojoscene.CorruptJojoBJCruel.ThrustsAgainstThroat".Localized((noFur ? "Common.Text.Hair".Localized() : "Common.Text.Fur".Localized()), Player.CockDescript(x), (noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized()), Player.CockDescript(x), Player.CockDescript(x)));

			OutputText("Scenes.Jojoscene.CorruptJojoBJCruel.EyesTurnInFear".Localized((Player.HasVagina() ? "A sympathetic orgasm hits your pussy, causing a surge of feminine juices to splash against his chest and dribble down your thighs lewdly. " : "")));

			OutputText("Scenes.Jojoscene.CorruptJojoBJCruel.OrgasmFillsBelly".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoBJCruel.SneersThreatens".Localized());

			Player.Orgasm("Dick");

			Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

			DoNext(() => ReturnToCamp(1));
		}

		public void CorruptJojoBJGentle()
		{
			ClearOutput();

			JojoSprite();

			var x = Player.BiggestCockIndex();

			OutputText("Scenes.Jojoscene.CorruptJojoBJGentle.JojoSlipsIntoCamp".Localized((noFur ? "Common.Text.Hair".Localized() : "Common.Text.Fur".Localized())));

			OutputText("Scenes.Jojoscene.CorruptJojoBJGentle.WhimpersCorruptionMusk".Localized());

			if (Player.CockArea(x) < 10)
				OutputText("Scenes.Jojoscene.CorruptJojoBJGentle.MouthOpensProtest".Localized(Player.CockDescript(x), Player.CockDescript(x), (Player.BiggestBreastSize() >= 2 ? "Your hands lift to massage your breasts and tug at your nipples, and you can see him watching transfixed as you fuck his throat. " : "")));
			else
				if (Player.CockArea(x) < 36)
					OutputText("Scenes.Jojoscene.CorruptJojoBJGentle.ProtestForcedDeep".Localized(Player.CockDescript(x), (Player.BiggestBreastSize() >= 2 ? "Your hands lift to massage your breasts and tug at your nipples, and you can see him watching transfixed as you fuck his throat. " : "")));
				else
					OutputText("Scenes.Jojoscene.CorruptJojoBJGentle.ProtestMutedBuckTeeth".Localized(Player.CockDescript(x), Player.CockDescript(x), (Player.BiggestBreastSize() >= 2 ? "His eyes move from your massive member to your bouncing breasts above with a look of wanton desire that makes you laugh softly. " : "")));

			OutputText("Scenes.Jojoscene.CorruptJojoBJGentle.EyesTurnInAwe".Localized((Player.HasVagina() ? "A sympathetic orgasm hits your pussy, causing a surge of feminine juices to splash against his chest and dribble down your thighs lewdly. " : "")));

			OutputText("Scenes.Jojoscene.CorruptJojoBJGentle.OrgasmBellyBulges".Localized());

			if (Player.BiggestBreastSize() >= 2)
				OutputText("Scenes.Jojoscene.CorruptJojoBJGentle.DrawsToBosomKiss".Localized());
			else
				OutputText("Scenes.Jojoscene.CorruptJojoBJGentle.LastFondCaress".Localized((noFur ? "Common.Text.Hair".Localized() : "Common.Text.Fur".Localized())));

			Player.Orgasm("Dick");

			Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

			DoNext(() => ReturnToCamp(1));
		}

		public void CorruptJojoCunnilingus()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.CorruptJojoCunnilingus.RewardsSlutService".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoCunnilingus.ConfusionTurnsBliss".Localized());

			switch (Random.Shared.Next(3))
			{
				case 0:
					{
						OutputText("Scenes.Jojoscene.CorruptJojoCunnilingus.SucklesTipCrown".Localized());
					}

					break;

				case 1:
					{
						OutputText("Scenes.Jojoscene.CorruptJojoCunnilingus.BobsHeadSqueezesBalls".Localized());
					}

					break;

			default:
			{
				OutputText("Scenes.Jojoscene.CorruptJojoCunnilingus.TakesDeepThroat".Localized());
			}

			break;
			}

			OutputText("Scenes.Jojoscene.CorruptJojoCunnilingus.SlutExplodesKiss".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoCunnilingus.VulgarKissDismissed".Localized());

			Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

			DoNext(() => ReturnToCamp(1));
		}

		public void CorruptJojoVaginalGentle()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.CorruptJojoVaginalGentle.UrgeToBeFilled".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoVaginalGentle.SwallowsNodsObeys".Localized((noFur ? "Common.Text.Lips".Localized() : "Common.Text.Muzzle".Localized()), (Player.HasCock() ? "Common.Text.OwnEndowments".Localized() : "Common.Text.Crotch".Localized())));

			if (Player.VaginalCapacity() < 10)
			{
				OutputText("Scenes.Jojoscene.CorruptJojoVaginalGentle.LinesUpEntrance".Localized());

				Player.CuntChange(36.4f, true);

				OutputText("Scenes.Jojoscene.CorruptJojoVaginalGentle.FeelsEveryVeinRidge".Localized());
			}
			else
				if (Player.VaginalCapacity() < 36)
				{
					OutputText("Scenes.Jojoscene.CorruptJojoVaginalGentle.LinesUpWithMoan".Localized());

					Player.CuntChange(36.4f, true);

					OutputText("Scenes.Jojoscene.CorruptJojoVaginalGentle.RisesAndFallsPace".Localized((noFur ? "" : " fur")));
				}
				else
				{
					OutputText("Scenes.Jojoscene.CorruptJojoVaginalGentle.ShiftsTiltsDrivesBack".Localized());

					Player.CuntChange(36.4f, true);

					OutputText("Scenes.Jojoscene.CorruptJojoVaginalGentle.LaughsBarelyFeels".Localized());
				}

			OutputText("Scenes.Jojoscene.CorruptJojoVaginalGentle.CriesOutJuicesSplash".Localized((Player.HasCock() ? ", and your own [cocks] to explode with thick splashes of your hot cum across his chest and belly" : "")));

			Player.KnockUp(PregnancyIds.Jojo, PregnancyIds.MouseIncubation + 82);

			Player.Orgasm("Vaginal");

			Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

			DoNext(() => ReturnToCamp(1));
		}

		public void CorruptJojoVaginalSmother()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.CorruptJojoVaginalSmother.CravingRelief".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoVaginalSmother.CrawlUpToFace".Localized());

			if (Player.Vaginas[0].Wetness > 4)
				OutputText("Scenes.Jojoscene.CorruptJojoVaginalSmother.MoanWhileLicking".Localized((noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));
			else
				if (Random.Shared.Next(2) == 0)
					OutputText("Scenes.Jojoscene.CorruptJojoVaginalSmother.GrindSlitOnHim".Localized());
				else
					OutputText("Scenes.Jojoscene.CorruptJojoVaginalSmother.SettleFullWeight".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoVaginalSmother.ClimaxFromTongue".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoVaginalSmother.GaspsForBreath".Localized());

			Player.Orgasm("Vaginal");

			Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

			DoNext(() => ReturnToCamp(1));
		}

		public void CorruptJojoAnalCruel()
		{
			ClearOutput();

			JojoSprite();

			var x = Player.BiggestCockIndex();

			OutputText("Scenes.Jojoscene.CorruptJojoAnalCruel.SeekPetMonk".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoAnalCruel.PressUpBehind".Localized((Player.BiggestBreastSize() >= 2 ? ", mashing your breasts against his back" : "")));

			if (Player.CockArea(x) < 10)
				OutputText("Scenes.Jojoscene.CorruptJojoAnalCruel.ErectionBetweenCheeks".Localized(
					Player.CockDescript(x)
				));
			else
				if (Player.CockArea(x) < 36)
					OutputText("Scenes.Jojoscene.CorruptJojoAnalCruel.PressMouseToTree".Localized(
						Player.CockDescript(x),

						Player.CockDescript(x)
					));
				else
					OutputText("Scenes.Jojoscene.CorruptJojoAnalCruel.SpearHisBowels".Localized(
						Player.CockDescript(x),

						Player.CockDescript(x)
					));

			OutputText("Scenes.Jojoscene.CorruptJojoAnalCruel.ThrustAtSquirming".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoAnalCruel.PullOutAndWipe".Localized(Player.CockDescript(x), (noFur ? "" : " the fur of")));

			Player.Orgasm("Dick");

			Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

			DoNext(() => ReturnToCamp(1));
		}

		public void CorruptJojoAnalGentle()
		{
			ClearOutput();

			JojoSprite();

			var x = Player.BiggestCockIndex();

			OutputText("Scenes.Jojoscene.CorruptJojoAnalGentle.WatchJojoEnter".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoAnalGentle.PossessiveHug".Localized());

			if (Player.CockArea(x) < 10)
				OutputText("Scenes.Jojoscene.CorruptJojoAnalGentle.SpitOnAnus".Localized());
			else
				if (Player.BiggestCockArea() < 36)
					OutputText("Scenes.Jojoscene.CorruptJojoAnalGentle.SlideCockheadUnder".Localized(
						Player.CockDescript(x)
					));
				else
					OutputText("Scenes.Jojoscene.CorruptJojoAnalGentle.ForceAgainstAss".Localized(
						Player.CockDescript(x),

						Player.CockDescript(x)
					));

			OutputText("Scenes.Jojoscene.CorruptJojoAnalGentle.PoundAwayHard".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoAnalGentle.EyesGlazeOver".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoAnalGentle.LaughAndLeave".Localized());

			Player.Orgasm("Dick");

			Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

			DoNext(() => ReturnToCamp(1));
		}

		public void CorruptJojoAnalSmother()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.CorruptJojoAnalSmother.CravingRelief".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoAnalSmother.CrawlUpThenSpin".Localized((noFur ? "Common.Text.Face".Localized() : "Common.Text.Muzzle".Localized())));

			switch (Random.Shared.Next(3))
			{
				case 0:
					{
						OutputText("Scenes.Jojoscene.CorruptJojoAnalSmother.GrindAssOnHim".Localized());
					}

					break;

				case 1:
					{
						OutputText("Scenes.Jojoscene.CorruptJojoAnalSmother.SettleFullWeight".Localized());
					}

					break;

			default:
			{
				OutputText("Scenes.Jojoscene.CorruptJojoAnalSmother.MoanAsTongueSpears".Localized());
			}

			break;
			}

			OutputText("Scenes.Jojoscene.CorruptJojoAnalSmother.TongueWorksAss".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoAnalSmother.GaspsForBreath".Localized());

			Player.Orgasm("Dick");

			Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

			DoNext(() => ReturnToCamp(1));
		}

		public void CorruptJojoBreasts()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.CorruptJojoBreasts.SelfPleasureMoment".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoBreasts.ObedientSlutKneels".Localized());

			if (Player.BiggestLactation() > 1)
			{
				OutputText("Scenes.Jojoscene.CorruptJojoBreasts.KissEachNipple".Localized((noFur ? "Common.Text.Mouth".Localized() : "Common.Text.Muzzle".Localized())));

				if (Player.BreastRows.Count > 1)
					OutputText("Scenes.Jojoscene.CorruptJojoBreasts.SuckleAllOthers".Localized());

				OutputText("Scenes.Jojoscene.CorruptJojoBreasts.DrinkTillFull".Localized());

				Player.Milked();
			}

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) < 3)
				CoC.Instance.Flags.Set(GameFlagIds.JojoAnalXp, CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) + 1);
			else
				if (Player.BiggestBreastSize() <= 5)
					OutputText("Scenes.Jojoscene.CorruptJojoBreasts.NuzzleSoftOrbs".Localized((Player.HasCock() ? "Common.Text.TakingHoldOfYourShaftAnd".Localized() : "Common.Text.SlippingYourFingersIntoYourSlit".Localized())));
				else
					OutputText("Scenes.Jojoscene.CorruptJojoBreasts.WorshipBothNipples".Localized((noFur ? "" : " fur at the")));

			OutputText("Scenes.Jojoscene.CorruptJojoBreasts.SpontaneousClimax".Localized());

			OutputText("Scenes.Jojoscene.CorruptJojoBreasts.SlinksIntoWoods".Localized());

			Player.Orgasm("Tits");

			Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

			DoNext(() => ReturnToCamp(1));
		}

		public void WhisperJojobait()
		{
			ClearOutput();

			ShowImage("akbal-deepwoods-male-jojosex");

			var x = Player.BiggestCockIndex();

			OutputText("Scenes.Jojoscene.WhisperJojobait.SenseJojosPresence".Localized((noFur ? "Common.Text.Hand".Localized() : "Common.Text.Paw".Localized())));

			OutputText("Scenes.Jojoscene.WhisperJojobait.ChorusOfWhispers".Localized());

			OutputText("Scenes.Jojoscene.WhisperJojobait.TurnUpPressure".Localized());

			OutputText("Scenes.Jojoscene.WhisperJojobait.EnterWithWail".Localized());

			OutputText("Scenes.Jojoscene.WhisperJojobait.PresentsHimself".Localized());

			OutputText("Scenes.Jojoscene.WhisperJojobait.ParagraphBreak".Localized());

			OutputText("Scenes.Jojoscene.WhisperJojobait.PatSwollenSac".Localized());

			if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
				OutputText("Scenes.Jojoscene.WhisperJojobait.RodentTailWraps".Localized(
					Player.CockDescript(x),

					Player.CockDescript(x),

					Player.CockDescript(x)
				));
			else
				if (Player.CockArea(x) < 13)
					OutputText("Scenes.Jojoscene.WhisperJojobait.HoleFlexesHungry".Localized(
						Player.CockDescript(x),

						Player.CockDescript(x),

						Player.CockDescript(x)
					));
				else
					if (Player.CockArea(x) < 25)
						OutputText("Scenes.Jojoscene.WhisperJojobait.TailWrapsPossessive".Localized(
							Player.CockDescript(x)
						));
					else
						OutputText("Scenes.Jojoscene.WhisperJojobait.TailTightensWaist".Localized(
							Player.CockDescript(x),

							Player.CockDescript(x)
						));

			OutputText("Scenes.Jojoscene.WhisperJojobait.PressureBuildingUp".Localized(
				Player.CockDescript(x)
			));

			if (Player.CumQ() >= 1500)
			{
				OutputText("Scenes.Jojoscene.WhisperJojobait.RoarDuringClimax".Localized());

				OutputText("Scenes.Jojoscene.WhisperJojobait.HowlsInHeat".Localized(Player.CockDescript(x), (noFur ? "" : " fuzzy"), (noFur ? "Common.Text.Skin".Localized() : "Common.Text.Fur".Localized())));

				OutputText("Scenes.Jojoscene.WhisperJojobait.WadeCumPuddle".Localized());
			}
			else
			{
				OutputText("Scenes.Jojoscene.WhisperJojobait.GritTeethThrust".Localized());

				OutputText("Scenes.Jojoscene.WhisperJojobait.BallsStartShrinking".Localized(
					Player.CockDescript(x)
				));

				OutputText("Scenes.Jojoscene.WhisperJojobait.WadeCumPuddleAgain".Localized());
			}

			Player.Orgasm("Dick");

			DoNext(() => ReturnToCamp(2));
		}

		public void BeeEggsInCorruptJojo()
		{
			ClearOutput();

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.TeasingJojoClose".Localized((noFur ? "" : " the soft fur of")));

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.JojoObliviousToProtrusion".Localized());

			if (!Player.HasCock())
				if (CoC.Instance.Flags.Get(GameFlagIds.TimesEggedJojo) == 0)
					OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.JojoPuzzledAtCock".Localized());
				else
					OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.JojoNodsInUnderstanding".Localized());

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.ObedientSlutBendsOver".Localized());

			if (TentacleJojo())
				OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.TentacleWord".Localized());

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.TailInvitingMotion".Localized());

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.MovingForwardPrefix".Localized());

			if (Player.HasTailInsteadOfLegs())
				OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.SlitherAroundHim".Localized());
			else
				if (Player.IsGoo())
					OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.OozeOverHim".Localized());
				else
					if (Player.IsTaur())
						OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.StraddleHim".Localized());
					else
						OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.ClimbAtopHim".Localized());

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.JojoLooksOverShoulder".Localized());

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.WithdrawInsectileSpear".Localized());

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.JojoMewlsInPleasure".Localized());

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.CooHereItComes".Localized());

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.JojoWhimpersAndNods".Localized());

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.MouseMoansLustily".Localized());

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.LayingEggsFeelsGood".Localized());

			if ((int)Player.Gender > 0)
			{
				OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.YourWord".Localized());

				if (Player.HasVagina())
					OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.PussyWord".Localized());

				if (Player.Gender == Gender.Hermaphrodite)
					OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.EachCockAndToken".Localized());

				if (Player.Gender == Gender.Male)
					OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.CocksToken".Localized());

				if (Player.Gender == Gender.Hermaphrodite || Player.CockTotal() > 1)
					OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.AreWord".Localized());
				else
					OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.IsWord".Localized());

				OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.GushingOverJojoBack".Localized());
			}

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.UtterlyFocusedOnSpheres".Localized());

			if (Player.Eggs() > 30)
				OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.BellyRoundedByEggs".Localized((noFur ? "Common.Text.Skin".Localized() : "Common.Text.Fur".Localized())));

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.PullOutSelfSatisfied".Localized());

			OutputText("Scenes.Jojoscene.BeeEggsInCorruptJojo.ShamblesOffToWoods".Localized());

			if (Player.FertilizedEggs() > 0 && !Player.ButtPregnancy.IsActive)
			{
				Player.ButtPregnancy.Type = PregnancyIds.BeeEggs;

				Player.ButtPregnancy.Incubation = 80;
			}

			CoC.Instance.Flags.Set(GameFlagIds.TimesEggedJojo, CoC.Instance.Flags.Get(GameFlagIds.TimesEggedJojo) + 1);

			Player.DumpEggs();

			Player.Orgasm("Ovi");

			CombatScene.CleanupAfterCombat();
		}

		public void JojoLaysEggs()
		{

			OutputText("Scenes.Jojoscene.JojoLaysEggs.HearingGruntsFromForest".Localized());

			OutputText("Scenes.Jojoscene.JojoLaysEggs.WatchingMouseGatherHoney".Localized());

			OutputText("Scenes.Jojoscene.JojoLaysEggs.MouseSeeksApproval".Localized());

			OutputText("Scenes.Jojoscene.JojoLaysEggs.AnswerInterruptedBySnore".Localized());

			Player.ButtPregnancy.Type = new Identifier("coc", "none");

			Player.ButtPregnancy.Incubation = 0;

			Player.ButtPregnancy.Count = 0;
		}

		public void LowCorruptionJojoEncounter()
		{
			ClearOutput();

			CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, 1);

			JojoSprite();

			OutputText("Scenes.Jojoscene.LowCorruptionJojoEncounter.TiredOfExploringForest".Localized());

			if (Player.Toughness >= 50 && Player.IsBiped())
				OutputText("Scenes.Jojoscene.LowCorruptionJojoEncounter.StepsBackMomentarilyDazed".Localized());
			else
				if (Player.Toughness < 50 && !Player.IsBiped())
					OutputText("Scenes.Jojoscene.LowCorruptionJojoEncounter.BlowKnocksYouFlat".Localized());
				else
					if (Player.IsTaur())
						OutputText("Scenes.Jojoscene.LowCorruptionJojoEncounter.BlowOnlyMomentarilyDazed".Localized());
					else
						OutputText("Scenes.Jojoscene.LowCorruptionJojoEncounter.RecoilWhenStruck".Localized());

			OutputText("Scenes.Jojoscene.LowCorruptionJojoEncounter.SurprisedByMouseVisage".Localized());

			OutputText("Scenes.Jojoscene.LowCorruptionJojoEncounter.ApologeticNeckRub".Localized());

			OutputText("Scenes.Jojoscene.LowCorruptionJojoEncounter.AcceptApologyPrompt".Localized());

			Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);

			ClearButtons();

			AddButton(0, "Common.Button.Yes".Localized(), AcceptJojosApology);

			AddButton(1, "Common.Button.No".Localized(), RefuseJojosApology);
		}

		public void AcceptJojosApology()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.AcceptJojosApology.ForgivingJojoAndApologizing".Localized());

			OutputText("Scenes.Jojoscene.AcceptJojosApology.RelievedFriendlyFaceSmile".Localized());

			LowCorruptionIntro();
		}

		public void RefuseJojosApology()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.RefuseJojosApology.PunchingUnsuspectingMouse".Localized());

			OutputText("Scenes.Jojoscene.RefuseJojosApology.MouseSaysOkayWarily".Localized());

			OutputText("Scenes.Jojoscene.RefuseJojosApology.WaterUnderTheBridge".Localized());

			LowCorruptionIntro();
		}

		public void LowCorruptionIntro()
		{

			OutputText("Scenes.Jojoscene.LowCorruptionIntro.JojoExtendsHand".Localized());

			OutputText("Scenes.Jojoscene.LowCorruptionIntro.NoticingSimpleGarbs".Localized());

			OutputText("Scenes.Jojoscene.LowCorruptionIntro.LitheButNotWeak".Localized((noFur ? "Common.Text.Skin".Localized() : "Common.Text.Fur".Localized())));

			OutputText("Scenes.Jojoscene.LowCorruptionIntro.SmilingKnowingly".Localized());

			OutputText("Scenes.Jojoscene.LowCorruptionIntro.JojoSighsOverWoes".Localized());

			if (Player.Corruption <= 5)
				OutputText("Scenes.Jojoscene.LowCorruptionIntro.DontWord".Localized());
			else
				OutputText("Scenes.Jojoscene.LowCorruptionIntro.BarelyWord".Localized());

			OutputText("Scenes.Jojoscene.LowCorruptionIntro.JoinMeInMeditation".Localized());

			ClearButtons();

			AddButton(0, "Scenes.Kitsune.KitsuneShrine.ButtonMeditate".Localized(), MeditateInForest);

			AddButton(1, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));

			if (Player.IsCorruptEnough(10) && Player.Lust >= 33 && (int)Player.Gender > 0 && CoC.Instance.Flags.Get(GameFlagIds.DisabledJojoRape) <= 0 && CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) >= 0)
				AddButton(4, "Common.Button.Rape".Localized(), () => JojoRape());
		}

		public void HighCorruptionJojoEncounter()
		{

			CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, 1);

			JojoSprite();

			OutputText("Scenes.Jojoscene.HighCorruptionJojoEncounter.MarvelingAtStrangeTrees".Localized());

			OutputText("Scenes.Jojoscene.HighCorruptionJojoEncounter.AngryLitheWhiteMouse".Localized());

			OutputText("Scenes.Jojoscene.HighCorruptionJojoEncounter.MouseHelpsYouUp".Localized());

			OutputText("Scenes.Jojoscene.HighCorruptionJojoEncounter.JojoSighsAtCorruption".Localized());

			Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);

			ClearButtons();

			AddButton(0, "Common.Button.Accept".Localized(), MeditateInForest);

			AddButton(1, "Scenes.Lumberjack.Button.Decline".Localized(), () => ReturnToCamp(1));

			if (Player.IsCorruptEnough(10) && Player.Lust >= 33 && (int)Player.Gender > 0 && CoC.Instance.Flags.Get(GameFlagIds.DisabledJojoRape) <= 0 && CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) >= 0)
				AddButton(2, "Common.Button.Rape".Localized(), () => JojoRape());
		}

		public void RepeatJojoEncounter()
		{

			if (Player.HasStatusEffect(StatusEffectIds.Infested) || Player.HasPerk(PerkIds.ParasiteMusk) || (Player.HasStatusEffect(StatusEffectIds.ParasiteSlug) && CoC.Instance.Flags.Get(GameFlagIds.Hadslugparasite) == 1))
			{
				JojoSprite();

				OutputText("Scenes.Jojoscene.RepeatJojoEncounter.ApproachingSereneMonk".Localized());

				OutputText("Scenes.Jojoscene.RepeatJojoEncounter.TempleOfYourBody".Localized());

				Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);

				ClearButtons();

				AddButton(0, "Scenes.Kitsune.KitsuneShrine.ButtonMeditate".Localized(), MeditateInForest);

				AddButton(1, "Combat.Ability.BeautifulSwordFightPurge.Name".Localized(), WormRemoval);

				if ((int)Player.Gender > 0 && CoC.Instance.Flags.Get(GameFlagIds.DisabledJojoRape) == 0 && Player.Lust >= 33)
					AddButton(2, "Common.Button.Rape".Localized(), () => JojoRape());

				SetExitButton("Common.Button.Back".Localized());

				return;
			}

			JojoSprite();

			if (Save.GetBool("forgiven"))
			{
				OutputText("Scenes.Jojoscene.RepeatJojoEncounter.LeavesSwayThenStaff".Localized((Player.Sensitivity > 50 || Player.Intelligence > 50 ? "Common.Text.NearlyStrikingYourNeckHadYou".Localized() : "Common.Text.StoppingAHairsWidthFromYour".Localized())));

				OutputText("Scenes.Jojoscene.RepeatJojoEncounter.ReleasesControlledBreath".Localized());

				OutputText("Scenes.Jojoscene.RepeatJojoEncounter.DefeatAllowsRepeatedRape".Localized());
			}
			else
				OutputText("Scenes.Jojoscene.RepeatJojoEncounter.MonkAppearsInBreeze".Localized((noFur ? "" : " and soft white fur")));

			Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);

			ClearButtons();

			AddButton(0, "Common.Button.Yes".Localized(), MeditateInForest);

			AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));

			if ((int)Player.Gender > 0 && Player.Lust >= 33 && CoC.Instance.Flags.Get(GameFlagIds.DisabledJojoRape) == 0)
				AddButton(2, "Common.Button.Rape".Localized(), () => JojoRape());
		}

		public void CorruptJojoEncounter()
		{

			JojoSprite();

			OutputText("Scenes.Jojoscene.CorruptJojoEncounter.JojoDropsFromTrees".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) == 2)
				OutputText("Scenes.Jojoscene.CorruptJojoEncounter.MouseyVisageFerociousSnarl".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) == 3)
				OutputText("Scenes.Jojoscene.CorruptJojoEncounter.UnsteadyFeetSeeksFight".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) == 4)
				OutputText("Scenes.Jojoscene.CorruptJojoEncounter.RobesTentedForFight".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) == 5)
				OutputText("Scenes.Jojoscene.CorruptJojoEncounter.NudePantingHugeCock".Localized((noFur ? "Common.Text.Hair".Localized() : "Common.Text.Fur".Localized())));

			Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);

			CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "jojo")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
		}

		public void MeditateInForest()
		{
			ClearOutput();

			JojoSprite();

			Player.ApplyStats(new DynStat(GameStats.Strength, .5f), new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Intelligence, .5f), new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Lust, -5f), new DynStat(GameStats.Corruption, -1 - Player.CountCockSocks("alabaster")));

			if (!Player.HasStatusEffect(StatusEffectIds.JojoMeditationCount))
				Player.CreateStatusEffect(StatusEffectIds.JojoMeditationCount, 1, 0, 0, 0);
			else
				Player.ChangeStatusValue(StatusEffectIds.JojoMeditationCount, 0, 1);

			if (Save.GetBool("forgiven"))
			{
				OutputText("Scenes.Jojoscene.MeditateInForest.LedToPeacefulClearing".Localized());

				OutputText("Scenes.Jojoscene.MeditateInForest.ParagraphBreak".Localized());

				if (Player.StatusEffectValue(StatusEffectIds.JojoMeditationCount, 0) >= 5 && !Save.GetBool("postRapeCampOffer"))
					AddButton(1, "Common.Button.JoinCamp".Localized(), PostRapeCampOffer);
			}
			else
			{
				OutputText("Scenes.Jojoscene.MeditateInForest.ReturnToClearing".Localized());

				OutputText("Scenes.Jojoscene.MeditateInForest.TimePassesInControl".Localized());

				if (Player.StatusEffectValue(StatusEffectIds.JojoMeditationCount, 0) >= 5)
				{
					OutputText("Scenes.Jojoscene.MeditateInForest.RespectfulNodAfterMeditation".Localized());

					if ((CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerLevel) >= 4 && CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerKilled) == 0) && Player.StatusEffectValue(StatusEffectIds.JojoMeditationCount, 0) % 5 == 0)
					{
						if (Player.HasKeyItem("Jojo's Talisman"))
						{
							OutputText("Scenes.Jojoscene.MeditateInForest.PraisingFocusTalent".Localized());

							OutputText("Scenes.Jojoscene.MeditateInForest.HoldsUpTalisman".Localized());
						}
						else
						{
							OutputText("Scenes.Jojoscene.MeditateInForest.PraisingDedication".Localized());

							OutputText("Scenes.Jojoscene.MeditateInForest.CannotStayInForest".Localized());

							OutputText("Scenes.Jojoscene.MeditateInForest.TalismanCallForHelp".Localized());

							OutputText("Scenes.Jojoscene.MeditateInForest.GainedJojoTalisman".Localized());

							Player.CreateKeyItem("Jojo's Talisman", 0, 0, 0, 0);
						}

						DoNext(() => ReturnToCamp(2));

						return;
					}
					else
						OutputText("Scenes.Jojoscene.MeditateInForest.MeditationTalentPraise".Localized());
				}

				if (Player.StatusEffectValue(StatusEffectIds.JojoMeditationCount, 0) % 5 == 0)
				{
					OutputText("Scenes.Jojoscene.MeditateInForest.JojoCampInvitationIdea".Localized());

					AddButton(0, "Common.Button.Yes".Localized(), AcceptJojoIntoYourCamp);

					AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));

					return;
				}
				else
					OutputText("Scenes.Jojoscene.MeditateInForest.BowsHeadSadlyDismisses".Localized());
			}

			DoNext(() => ReturnToCamp(2));
		}

		public void PostRapeCampOffer()
		{
			ClearOutput();

			OutputText("Scenes.Jojoscene.PostRapeCampOffer.InvitingJojoToCamp".Localized());

			OutputText("Scenes.Jojoscene.PostRapeCampOffer.JojoTensesRejectingOffer".Localized());

			Save.SetBool("postRapeCampOffer", true);

			DoNext(() => ReturnToCamp(1));
		}

		public void AcceptJojoIntoYourCamp()
		{
			ClearOutput();

			JojoSprite();

			if (Player.HasStatusEffect(StatusEffectIds.EverRapedJojo) || CoC.Instance.Flags.Get(GameFlagIds.JojoMoveInDisabled) == 1)
				OutputText("Scenes.Jojoscene.AcceptJojoIntoYourCamp.JojoShakesHeadNo".Localized());
			else
			{
				OutputText("Scenes.Jojoscene.AcceptJojoIntoYourCamp.JojoConsidersCampOffer".Localized());

				OutputText("Scenes.Jojoscene.AcceptJojoIntoYourCamp.SaferTogetherAgreement".Localized());

				OutputText("Scenes.Jojoscene.AcceptJojoIntoYourCamp.JojoScurriesIntoBushes".Localized());

				Player.CreateStatusEffect(StatusEffectIds.PureCampJojo, 0, 0, 0, 0);
			}

			DoNext(() => ReturnToCamp(1));
		}

		public void AcceptOfferOfHelp()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.AcceptOfferOfHelp.ThanksMaraeStronger".Localized());

			JojoFollowerMeditate();
		}

		public void RefuseOfferOfHelp()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.RefuseOfferOfHelp.AssuringJojoYoureFine".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.JojoLastMeditation, View.Time.Days);

			JojoCampMenu();
		}

		public void JojoCamp()
		{
			ClearOutput();

			JojoSprite();

			if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMetPureJojo) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)

				return;

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoRathazulInteractionCounter) == 1 && Random.Shared.Next(2) == 0)

				return;

			if (Player.HasStatusEffect(StatusEffectIds.Infested) || Player.HasPerk(PerkIds.ParasiteMusk) || (Player.HasStatusEffect(StatusEffectIds.ParasiteSlug) && CoC.Instance.Flags.Get(GameFlagIds.Hadslugparasite) == 1))
			{
				OutputText("Scenes.Jojoscene.JojoCamp.ApproachingMonkNoseTwitch".Localized());

				OutputText("Scenes.Jojoscene.JojoCamp.TempleOfYourBody".Localized());

				JojoCampMenu();

				return;
			}

			if (Player.Corruption > 10 && CoC.Instance.Flags.Get(GameFlagIds.JojoLastMeditation) != View.Time.Days)
			{
				if (Player.Corruption >= 40)
					OutputText("Scenes.Jojoscene.JojoCamp.JojoApproachesUrgently".Localized());
				else
					OutputText("Scenes.Jojoscene.JojoCamp.JojoSitsCrossLegged".Localized());

				OutputText("Scenes.Jojoscene.JojoCamp.AcceptJojoHelpPrompt".Localized());

				ClearButtons();

				AddButton(0, "Common.Button.Yes".Localized(), AcceptOfferOfHelp);

				AddButton(1, "Common.Button.No".Localized(), RefuseOfferOfHelp);

				if (Player.Lust >= 33 && !Player.IsGenderless() && CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) >= 0)
					AddButton(4, "Common.Button.Rape".Localized(), JojoAtCampRape);
			}
			else
			{
				if (Player.Corruption > 10)

					OutputText("Scenes.Jojoscene.JojoCamp.JojoSitsEyesClosed".Localized("", ""));
				else
					switch (Random.Shared.Next(3))
					{
						case 0:
							{
                    // 原版 JojoScene.hx:3040：Akky 在 Jojo 处时显示它的位置描述
								OutputText("Scenes.Jojoscene.JojoCamp.JojoSeenCrossLegged".Localized(SceneInstance<AkkyScene>.Get().location == "Jojo" ? SceneInstance<AkkyScene>.Get().LocationShort() : ""));
							}

							break;

						case 1:
							{
								OutputText("Scenes.Jojoscene.JojoCamp.JojoPracticesBehindBoulder".Localized());
							}

							break;

					default:
					{
						OutputText("Scenes.Jojoscene.JojoCamp.ThinksOnFlatRock".Localized());
					}

					break;
					}

				JojoCampMenu();
			}
		}

		public void JojoCampMenu()
		{

			if (CampGuards.IsGuard("Jojo"))
				OutputText("Scenes.Jojoscene.JojoCampMenu.JojoWatchesForEnemies".Localized());

			Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);

			ClearButtons();

			AddButton(0, "Common.Button.Appearance".Localized(), JojoAppearance);

			AddButton(1, "Common.Button.Talk".Localized(), () => TalkMenu());

			AddButton(2, "Scenes.Kitsune.KitsuneShrine.ButtonMeditate".Localized(), JojoFollowerMeditate);

			if (CoC.Instance.Flags.Get(GameFlagIds.UnlockedJojoTraining) == 1)
				AddButton(3, "Common.Button.Train".Localized(), ApparentlyJojoDOESlift);

			AddButton(4, CampGuards.IsGuard("Jojo") ? "Common.Button.StopGuarding".Localized() : "Common.Button.GuardCamp".Localized(), JojoDefenseToggle);

			if (Player.HasStatusEffect(StatusEffectIds.Infested) || Player.HasPerk(PerkIds.ParasiteMusk) || (Player.HasStatusEffect(StatusEffectIds.ParasiteSlug) && CoC.Instance.Flags.Get(GameFlagIds.Hadslugparasite) == 1))
				AddButton(5, "Combat.Ability.BeautifulSwordFightPurge.Name".Localized(), WormRemoval);

			if (Player.IsCorruptEnough(10) && Player.Lust >= 33 && (int)Player.Gender > 0 && CoC.Instance.Flags.Get(GameFlagIds.DisabledJojoRape) <= 0)
				AddRowButton(2, "Rape", JojoAtCampRape);

			AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
		}

		public void JojoAppearance()
		{
			ClearOutput();

			JojoSprite();

			ShowImage("jojo-appearance");

			OutputText("Scenes.Jojoscene.JojoAppearance.WhiteMouseAppearance".Localized((Player.Tallness > 45 && Player.Tallness < 51 ? "just about your size, " : "")));

			OutputText("Scenes.Jojoscene.JojoAppearance.LeanHonedMonkBody".Localized());

			OutputText("Scenes.Jojoscene.JojoAppearance.EndowmentRemainsConcealed".Localized());

			DoNext(() => JojoCamp());
		}

		public void TalkMenu(Action from = null)
		{

			JojoSprite();

			ClearButtons();

			AddButton(0, "Common.Button.Village".Localized(), JojoTalkVillage, "Ask him about the village he was raised in.");

			AddButton(1, "Common.Button.Monks".Localized(), JojoTalkJoiningTheMonks, "Ask him about how and why he became a monk.");

			AddButton(2, "Common.Button.Monksfall".Localized(), JojoTalkFallOfTheMonks, "Ask him about the demise of the monks.");

			AddButton(3, "Common.Button.Forest".Localized(), JojoTalkForestConvo, "Ask him about how he ended up in the forest.");

			if (CoC.Instance.Flags.Get(GameFlagIds.TimesTalkedWithJojo) >= 4)
				AddButton(4, "Common.Button.You".Localized(), JojoTalkYourOrigin, "Tell him about Ingnam and your history.");

			if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 0)
				AddButton(5, "Common.Button.Factory".Localized(), JojoTalkFactory, "Tell him about how you've shut down the factory.");

			if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchesCowed) == 1 || CoC.Instance.Flags.Get(GameFlagIds.SandWitchesFriendly) == 1 || CoC.Instance.Flags.Get(GameFlagIds.SandMotherDefeated) == 1)
				AddButton(6, "Common.Button.Sandcave".Localized(), JojoTalkSandCave, "Tell him about your encounter in the Sand Cave.");

			if (CoC.Instance.Flags.Get(GameFlagIds.UnlockedJojoTraining) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesTalkedWithJojo) >= 4)
				AddButton(7, "Common.Button.Training".Localized(), ApparentlyJojoDOESlift);

			if (CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationJojoTalked) == 1 && CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationProgress) < 10)
				AddButtonDisabled(8, "Purification", "");

			AddButton(14, "Common.Button.Back".Localized(), JojoCamp);
		}

		public void JojoTalkVillage()
		{
			ClearOutput();

			JojoSprite();

			CoC.Instance.Flags.Set(GameFlagIds.TimesTalkedWithJojo, CoC.Instance.Flags.Get(GameFlagIds.TimesTalkedWithJojo) + 1);

			OutputText("Scenes.Jojoscene.JojoTalkVillage.AskingAboutHisVillage".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkVillage.RecallingSmallVillage".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkVillage.BrightEyesAfterSadness".Localized((noFur ? "Common.Text.Lips".Localized() : "Common.Text.FuzzyCheeks".Localized())));

			OutputText("Scenes.Jojoscene.JojoTalkVillage.FatherFishermanSilverWhiskers".Localized());

			if (Player.Corruption < 40)
				OutputText("Scenes.Jojoscene.JojoTalkVillage.ThankingForMemories".Localized());
			else
				OutputText("Scenes.Jojoscene.JojoTalkVillage.FondChildhoodReflection".Localized());

			TalkMenu(JojoTalkVillage);
		}

		public void JojoTalkJoiningTheMonks()
		{
			ClearOutput();

			JojoSprite();

			CoC.Instance.Flags.Set(GameFlagIds.TimesTalkedWithJojo, CoC.Instance.Flags.Get(GameFlagIds.TimesTalkedWithJojo) + 1);

			OutputText("Scenes.Jojoscene.JojoTalkJoiningTheMonks.AskingWhyMonk".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkJoiningTheMonks.BigFamilyOfNineteen".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkJoiningTheMonks.SneakingOutChildhood".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkJoiningTheMonks.MotherHeldHimClose".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkJoiningTheMonks.TempleBecameImportant".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkJoiningTheMonks.QuietSmileThenChuckle".Localized());

			TalkMenu(JojoTalkJoiningTheMonks);
		}

		public void JojoTalkFallOfTheMonks()
		{
			ClearOutput();

			JojoSprite();

			CoC.Instance.Flags.Set(GameFlagIds.TimesTalkedWithJojo, CoC.Instance.Flags.Get(GameFlagIds.TimesTalkedWithJojo) + 1);

			OutputText("Scenes.Jojoscene.JojoTalkFallOfTheMonks.AskJojoAboutMonks".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkFallOfTheMonks.JojoSpeaksDowncast".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkFallOfTheMonks.BrotherLograyDeath".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkFallOfTheMonks.JojoShameThenResolve".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkFallOfTheMonks.VowOfResolve".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkFallOfTheMonks.PlayerComfortsJojo".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkFallOfTheMonks.StoryStirsResolve".Localized());

			TalkMenu(JojoTalkFallOfTheMonks);
		}

		public void JojoTalkForestConvo()
		{
			ClearOutput();

			JojoSprite();

			CoC.Instance.Flags.Set(GameFlagIds.TimesTalkedWithJojo, CoC.Instance.Flags.Get(GameFlagIds.TimesTalkedWithJojo) + 1);

			OutputText("Scenes.Jojoscene.JojoTalkForestConvo.AskJojoForestStory".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkForestConvo.JojoRecallsTraining".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkForestConvo.JojoRaisesFinger".Localized((noFur ? "" : " furry")));

			OutputText("Scenes.Jojoscene.JojoTalkForestConvo.VillageStruckFirst".Localized());

			if (Player.Corruption < 35)
				OutputText("Scenes.Jojoscene.JojoTalkForestConvo.PlayerImaginesRuin".Localized());
			else
				if (Player.Corruption >= 35 && Player.Corruption <= 75)
					OutputText("Scenes.Jojoscene.JojoTalkForestConvo.VillageStillStands".Localized());
				else
					OutputText("Scenes.Jojoscene.JojoTalkForestConvo.InsultWimpyVillagers".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkForestConvo.JojoBornThereSilent".Localized());

			if (Player.Corruption < 35)
				OutputText("Scenes.Jojoscene.JojoTalkForestConvo.ArmAroundShoulders".Localized((noFur ? "Scenes.Anemonescene.KidADreams.Cheek".Localized() : "Common.Text.Muzzle".Localized())));
			else
				if (Player.Corruption >= 35 && Player.Corruption <= 75)
					OutputText("Scenes.Jojoscene.JojoTalkForestConvo.RubMonkBack".Localized());
				else
				{
					OutputText("Scenes.Jojoscene.JojoTalkForestConvo.WrapArmsAroundMonk".Localized());

					Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
				}

			OutputText("Scenes.Jojoscene.JojoTalkForestConvo.AskWhatHappenedNext".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkForestConvo.JojoSquaredShoulders".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkForestConvo.MonkClenchedFists".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkForestConvo.BurnedMonasteryFields".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkForestConvo.JojoAppraisingLook".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkForestConvo.JojoRenewedPeace".Localized());

			TalkMenu(JojoTalkForestConvo);
		}

		public void JojoTalkYourOrigin()
		{
			ClearOutput();

			JojoSprite();

			CoC.Instance.Flags.Set(GameFlagIds.TimesTalkedWithJojo, CoC.Instance.Flags.Get(GameFlagIds.TimesTalkedWithJojo) + 1);

			OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.SmallTalkWithJojo".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.PortalOriginQuestion".Localized());

			if (Player.HasPerk(PerkIds.HistoryAlchemist))
			{
				if (Player.WasElder())
					OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.ApothecaryPast".Localized());
				else
					OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.RikuAssistantPast".Localized());
			}
			else
				if (Player.HasPerk(PerkIds.HistoryFighter))
				{
					if (Player.WasElder())
						OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.YouthFightsGuard".Localized());
					else
						OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.ChildhoodFightStories".Localized());
				}
				else
					if (Player.HasPerk(PerkIds.HistoryHealer))
					{
						if (Player.WasElder())
							OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.VillageHealerPast".Localized());
						else
							OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.DendeHealerAssistant".Localized());
					}
					else
						if (Player.HasPerk(PerkIds.HistoryReligious))
							OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.TempleLifePast".Localized());
						else
							if (Player.HasPerk(PerkIds.HistoryScholar))
							{
								if (Player.WasElder())
									OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.KnowledgePassedOn".Localized());
								else
									OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.SchoolKnowledgePast".Localized());
							}
							else
								if (Player.HasPerk(PerkIds.HistorySlut))
								{
									OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.PopularityFriendsPast".Localized());

									OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.JojoQuestionsPopularity".Localized());

									OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.LaughOverFriendsSkill".Localized());
								}
								else
									if (Player.HasPerk(PerkIds.HistorySlacker))
										OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.RelaxingWithFriends".Localized());
									else
										if (Player.HasPerk(PerkIds.HistoryThief))
											OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.HelpingCarryThings".Localized());
										else
											if (Player.HasPerk(PerkIds.HistorySmith))
											{
												if (Player.WasElder())
													OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.BlacksmithWorkPast".Localized());
												else
													OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.BlacksmithTrainingPast".Localized());
											}
											else
												if (Player.HasPerk(PerkIds.HistoryWhore))
												{
													if (Player.WasElder())
														OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.MoneyMakingTale".Localized());
													else
														OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.MoneyMakingAgain".Localized());
												}
												else
													if (Player.HasPerk(PerkIds.HistoryFortune))
														OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.LuckyMoneyPast".Localized());
													else
														if (Player.HasPerk(PerkIds.HistoryDEUSVULT))
														{
															OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.RigorousUpbringing".Localized());

															OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.JojoStrictChildhood".Localized());
														}
														else
															OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.MissingHistoryPerkBug".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkYourOrigin.JojoSmilesKnowingYou".Localized());

			TalkMenu(JojoTalkYourOrigin);
		}

		public void JojoTalkFactory()
		{
			ClearOutput();

			JojoSprite();

			CoC.Instance.Flags.Set(GameFlagIds.TimesTalkedWithJojo, CoC.Instance.Flags.Get(GameFlagIds.TimesTalkedWithJojo) + 1);

			OutputText("Scenes.Jojoscene.JojoTalkFactory.DescribeFactoryWin".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
			{
				OutputText("Scenes.Jojoscene.JojoTalkFactory.ControlsDestroyedReport".Localized());

				OutputText("Scenes.Jojoscene.JojoTalkFactory.JojoCocksHead".Localized());

				OutputText("Scenes.Jojoscene.JojoTalkFactory.JojoCounselPleased".Localized());
			}
			else
			{
				OutputText("Scenes.Jojoscene.JojoTalkFactory.FactoryShutdownReport".Localized());

				OutputText("Scenes.Jojoscene.JojoTalkFactory.JojoSwellsPride".Localized());
			}

			TalkMenu(JojoTalkFactory);
		}

		public void JojoTalkSandCave()
		{
			ClearOutput();

			JojoSprite();

			CoC.Instance.Flags.Set(GameFlagIds.TimesTalkedWithJojo, CoC.Instance.Flags.Get(GameFlagIds.TimesTalkedWithJojo) + 1);

			OutputText("Scenes.Jojoscene.JojoTalkSandCave.SandWitchesCaveFound".Localized());

			OutputText("Scenes.Jojoscene.JojoTalkSandCave.ConfusedSandWitchAllies".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchesCowed) == 1)
				OutputText("Scenes.Jojoscene.JojoTalkSandCave.BattleBlowByBlow".Localized());
			else
				if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchesFriendly) == 1 && CoC.Instance.Flags.Get(GameFlagIds.SandMotherDefeated) == 1)
				{
					OutputText("Scenes.Jojoscene.JojoTalkSandCave.BattleEndOutcome".Localized());

					OutputText("Scenes.Jojoscene.JojoTalkSandCave.JojoTiltsAltercation".Localized());
				}
				else
					if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchesFriendly) == 1 && CoC.Instance.Flags.Get(GameFlagIds.SandMotherDefeated) == 0)
					{
						OutputText("Scenes.Jojoscene.JojoTalkSandCave.SandMotherConversation".Localized());

						OutputText("Scenes.Jojoscene.JojoTalkSandCave.JojoMarvelsTact".Localized());
					}

			if (CoC.Instance.Flags.Get(GameFlagIds.MetMilkSlave) == 1)
			{
				OutputText("Scenes.Jojoscene.JojoTalkSandCave.MilkTubSlaveGirl".Localized());

				OutputText("Scenes.Jojoscene.JojoTalkSandCave.JojoShuddersAsks".Localized());

				if (CoC.Instance.Flags.Get(GameFlagIds.MetMilkSlave) <= 0)
				{
					OutputText("Scenes.Jojoscene.JojoTalkSandCave.SandMotherSaysUnfit".Localized());

					OutputText("Scenes.Jojoscene.JojoTalkSandCave.JojoChinHandsThinks".Localized());
				}
				else
				{
					OutputText("Scenes.Jojoscene.JojoTalkSandCave.ConfusedGlanceBathGirl".Localized());

					OutputText("Scenes.Jojoscene.JojoTalkSandCave.JojoSlapsForehead".Localized());

					if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 0)
						OutputText("Scenes.Jojoscene.JojoTalkSandCave.BetterInOurCare".Localized());
					else
						if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 1)
							OutputText("Scenes.Jojoscene.JojoTalkSandCave.AlreadyMuchBetter".Localized());
						else
							OutputText("Scenes.Jojoscene.JojoTalkSandCave.FondPraisesCare".Localized());
				}
			}

			if (CoC.Instance.Flags.Get(GameFlagIds.CumWitchDefeated) == 1 || CoC.Instance.Flags.Get(GameFlagIds.BeenBlessedByCumWitch) == 1)
			{
				OutputText("Scenes.Jojoscene.JojoTalkSandCave.CumWitchDuty".Localized());

				if (CoC.Instance.Flags.Get(GameFlagIds.MoreCumWitches) == 1)
				{
					OutputText("Scenes.Jojoscene.JojoTalkSandCave.RequestMoreWitches".Localized());

					OutputText("Scenes.Jojoscene.JojoTalkSandCave.JojoWhistlesBold".Localized());
				}
				else
					if (CoC.Instance.Flags.Get(GameFlagIds.CumWitchesFightable) == 1)
						OutputText("Scenes.Jojoscene.JojoTalkSandCave.WitchesRoamDesert".Localized());
					else
						OutputText("Scenes.Jojoscene.JojoTalkSandCave.AwkwardSilenceDone".Localized());
			}

			TalkMenu(JojoTalkSandCave);
		}

		public void ApparentlyJojoDOESlift()
		{
			ClearOutput();

			JojoSprite();

			if (CoC.Instance.Flags.Get(GameFlagIds.UnlockedJojoTraining) == 0)
			{
				CoC.Instance.Flags.Set(GameFlagIds.UnlockedJojoTraining, 1);

				OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.AskTeachMonkWays".Localized());

				OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.JojoConsidersTraining".Localized());

				if (!Player.IsPureEnough(25))
				{
					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.JojoUnworthyMaster".Localized());

					ClearButtons();

					DoNext(() => JojoCamp());

					return;
				}
				else
				{
					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.JojoFriendNotMaster".Localized());

					ClearButtons();

					DoNext(() => JojoCamp());

					return;
				}
			}
			else
			{
				if (Player.Fatigue >= Player.MaxFatigue() - 60)
				{
					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.ContinueTrainingRefused".Localized());

					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.RestBeforeTraining".Localized());

					ClearButtons();

					DoNext(() => JojoCamp());

					return;
				}

				if (!Player.IsPureEnough(25))
				{
					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.ContinueTrainingAgain".Localized());

					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.MeditateBeforeTraining".Localized());

					ClearButtons();

					AddButton(0, "Common.Button.Yes".Localized(), JojoFollowerMeditate);

					AddButton(1, "Common.Button.No".Localized(), JojoCamp);

					return;
				}
			}

			CoC.Instance.Flags.Set(GameFlagIds.TimesTrainedWithJojo, CoC.Instance.Flags.Get(GameFlagIds.TimesTrainedWithJojo) + 1);

			if (!Player.HasPerk(PerkIds.ControlledBreath) && !Player.HasPerk(PerkIds.CleansingPalm) && !Player.HasPerk(PerkIds.Enlightened))
			{
				OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.CheerfulLetsBegin".Localized());

				OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.TeachingLectureSparring".Localized());

				OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.IntenseSessionBreaks".Localized());

				OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.SessionEndSweat".Localized());

				OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.BowEarnedRest".Localized());

				Player.ChangeFatigue(60);

				if (CoC.Instance.Flags.Get(GameFlagIds.TimesTrainedWithJojo) == 5)
				{
					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.BreathingIsKey".Localized());

					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.LearnedBreathControl".Localized());

					if (Player.BiggestBreastSize() == 0)
						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.RisesAndFallsWord".Localized());
					else
						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.RiseAndFallWord".Localized());

					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.SmoothlyEvenInBattle".Localized());

					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.PerkControlledBreath".Localized());

					Player.CreatePerk(PerkIds.ControlledBreath, 0, 0, 0, 0);
				}
			}
			else
				if (Player.HasPerk(PerkIds.ControlledBreath) && !Player.HasPerk(PerkIds.CleansingPalm) && !Player.HasPerk(PerkIds.Enlightened))
				{
					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.ToothyGrinsBegin".Localized());

					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.EnduranceTrainingShift".Localized());

					OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.FormsMockStrikes".Localized());

					Player.ChangeFatigue(60);

					if (CoC.Instance.Flags.Get(GameFlagIds.TimesTrainedWithJojo) == 10)
					{
						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.MovementsSinkIn".Localized());

						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.SessionEndInsight".Localized());

						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.AbilityCleansingPalm".Localized());

						Player.CreatePerk(PerkIds.CleansingPalm, 0, 0, 0, 0);
					}
				}
				else
					if (Player.HasPerk(PerkIds.ControlledBreath) && Player.HasPerk(PerkIds.CleansingPalm) && !Player.HasPerk(PerkIds.Enlightened))
					{
						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.ProudSmileBegin".Localized());

						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.EnduranceSparring".Localized());

						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.SweepingFormsStrikes".Localized());

						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.ShakyBowsPraise".Localized());

						Player.ChangeFatigue(60);

						if (CoC.Instance.Flags.Get(GameFlagIds.TimesTrainedWithJojo) >= 16 && Player.Intelligence >= 70)
						{
							OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.MeditateAloneAfter".Localized());

							OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.CampNightSounds".Localized());

							OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.PerkEnlightenedMeditation".Localized());

							Player.CreatePerk(PerkIds.Enlightened, 0, 0, 0, 0);
						}
					}
					else
					{
						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.JojoWantsTeaching".Localized());

						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.NoLecturesExhausted".Localized());

						Player.ChangeFatigue(60);

						var enlightenedBlurbs = new List<string>
                        {
                            "You can hear Jojo's feet move through the campsite as he heads toward his rock, seeking rest after your training session."
                        };

						if (Player.HasStatusEffect(StatusEffectIds.CampMarble))
							enlightenedBlurbs.Add("You can hear Marble humming a song to herself you can't place.");

						if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) > 0)
							enlightenedBlurbs.Add("You can hear Amily changing the bedding to her nest.");

						if (CoC.Instance.Flags.Get(GameFlagIds.EmberHatched) > 0)
							enlightenedBlurbs.Add("You can hear Ember cleaning " + EmberMF() + " scales.");

						if (CoC.Instance.Flags.Get(GameFlagIds.RathazulCampInteractionCountdown) > 0)
							enlightenedBlurbs.Add("You can hear Rathazul experimenting with surprisingly nimble fingers.");

						if (CoC.Instance.Flags.Get(GameFlagIds.SophieRecruitedPure) > 0)
							enlightenedBlurbs.Add("You can hear Sophie breathing as she sleeps.");

						if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) > 0)
							enlightenedBlurbs.Add("You can hear Izma flipping through the pages of a book.");

						if (CoC.Instance.Flags.Get(GameFlagIds.HelFollowerLevel) > 0)
							enlightenedBlurbs.Add("You can hear Helia throwing her fists at nothing.");

						OutputText("Scenes.Jojoscene.ApparentlyJojoDOESlift.PlaceholderParagraphBreak".Localized(
							enlightenedBlurbs[Random.Shared.Next(enlightenedBlurbs.Count)]
						));
					}

			if ((100 * Player.Strength / Player.GetMaxStats("str")) < 50)
				Player.ApplyStats(new DynStat(GameStats.Strength, 0.5f));

			if ((100 * Player.Strength / Player.GetMaxStats("str")) < 80)
				Player.ApplyStats(new DynStat(GameStats.Strength, 0.5f));

			if (Player.Intelligence < 50)
				Player.ApplyStats(new DynStat(GameStats.Intelligence, 0.5f));

			if (Player.Intelligence < 80)
				Player.ApplyStats(new DynStat(GameStats.Intelligence, 0.5f));

			ClearButtons();

			DoNext(() => ReturnToCamp(1));
		}

		public void WormRemoval()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.WormRemoval.JojoContinues".Localized());

			OutputText("Scenes.Jojoscene.WormRemoval.JojoFetchesBox".Localized());

			if (Player.HasStatusEffect(StatusEffectIds.Infested))
				OutputText("Scenes.Jojoscene.WormRemoval.PainSetsIn".Localized());

			OutputText("Scenes.Jojoscene.WormRemoval.JojoDemandsEndurance".Localized());

			OutputText("Scenes.Jojoscene.WormRemoval.BodyCrampsWormStruggle".Localized());

			OutputText("Scenes.Jojoscene.WormRemoval.CulpritExorcisedRest".Localized());

			OutputText("Scenes.Jojoscene.WormRemoval.TooTiredToMeditate".Localized());

			if (Player.Health > (int)(Player.MaxHealth * .5f))
				Player.Health = (int)(Player.MaxHealth * .5f);

			Player.DamageHunger(30);

			Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlug);

			Player.RemoveStatusEffect(StatusEffectIds.Infested);

			Player.RemovePerk(PerkIds.ParasiteMusk);

			Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlugReproduction);

			Player.ApplyStats(new DynStat(GameStats.Sensitivity, Math.Max(Player.Sensitivity - 25f, 0f)), new DynStat(GameStats.Lust, -99f), new DynStat(GameStats.Corruption, -15f));

			Player.Orgasm("Generic");

			DoNext(() => ReturnToCamp(1));
		}

		public void OfferSexFirstTime()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.OfferSexFirstTime.AskJojoForHelp".Localized());

			OutputText("Scenes.Jojoscene.OfferSexFirstTime.JojoOffersHelp".Localized());

			OutputText("Scenes.Jojoscene.OfferSexFirstTime.NeedsAndDesires".Localized());

			OutputText("Scenes.Jojoscene.OfferSexFirstTime.JojoEyesOpenWide".Localized((noFur ? "Common.Text.Hard".Localized() : "Common.Text.SoHardYouCanEvenSee".Localized()), (Player.Femininity >= 50 ? "Common.Text.Beautiful".Localized() : "Common.Text.Handsome".Localized()), ((Player.Thickness >= 60 && Player.Tone < 60) ? "Common.Text.AlthoughYouShouldWorkHarderTo".Localized() : "Common.Text.AndYouObviouslyKeepYourselfIn".Localized())));

			OutputText("Scenes.Jojoscene.OfferSexFirstTime.YouPressFurther".Localized());

			OutputText("Scenes.Jojoscene.OfferSexFirstTime.ChastityVowRefusal".Localized());

			OutputText("Scenes.Jojoscene.OfferSexFirstTime.AcknowledgeAndExcuse".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, -1);

			AddButton(0, "Common.Button.Yes".Localized(), AgreeToMeditate);

			AddButton(1, "Common.Button.No".Localized(), NoThanksToMeditate);
		}

		public void AgreeToMeditate()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.AgreeToMeditate.AcceptMeditationOffer".Localized());

			DoNext(() => JojoFollowerMeditate());
		}

		public void NoThanksToMeditate()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.NoThanksToMeditate.DeclineAndLeave".Localized());

			DoNext(PlayerMenu);

			View.Time.PassMinutes((int)Math.Round((0.25) * 60));
		}

		public void OfferSexFirstTimeHighAffection()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.OfferSexFirstTimeHighAffection.CherishedTimeWithJojo".Localized());

			OutputText("Scenes.Jojoscene.OfferSexFirstTimeHighAffection.ApproachAndPropose".Localized());

			OutputText("Scenes.Jojoscene.OfferSexFirstTimeHighAffection.StillCannotBreakVows".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, -2);

			ClearButtons();

			AddButton(0, "Scenes.Kitsune.KitsuneShrine.ButtonMeditate".Localized(), JojoFollowerMeditate);

			AddButton(1, "Common.Button.DropIt".Localized(), NoThanksToMeditate);

			if (Player.Intelligence >= 60 && Player.IsPureEnough(10))
				AddButton(2, "Common.Button.Confront".Localized(), ConfrontChastity);
		}

		public void ConfrontChastity()
		{
			ClearOutput();

			JojoSprite();

			OutputText("Scenes.Jojoscene.ConfrontChastity.VowsCannotLastForever".Localized());

			OutputText("Scenes.Jojoscene.ConfrontChastity.PleasureOutburstStare".Localized((Player.HasVagina() ? "How about potential mate like me? " : "")));

			OutputText("Scenes.Jojoscene.ConfrontChastity.JojoConcedesVows".Localized());

			OutputText("Scenes.Jojoscene.ConfrontChastity.KnowingSmileBack".Localized());

			OutputText("Scenes.Jojoscene.ConfrontChastity.SexMenuUnlocked".Localized());

			if (silly)
				OutputText("Scenes.Jojoscene.ConfrontChastity.FourMovesLearned".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.DisabledJojoRape, 1);

			CoC.Instance.Flags.Set(GameFlagIds.JojoStatus, -3);

			DoNext(() => PureJojoSexMenu());
		}

		public void PureJojoSexMenu()
		{
			ClearOutput();

			var capacity = 40;

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) < 10)
				capacity += (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) * 3);
			else
				capacity += 30;

			CurrentMonster = new JojoEnemy();

			JojoSprite();

			OutputText("Scenes.Jojoscene.PureJojoSexMenu.AskJojoMood".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) < 3)
				OutputText("Scenes.Jojoscene.PureJojoSexMenu.GentleOnlyCondition".Localized());
			else
				OutputText("Scenes.Jojoscene.PureJojoSexMenu.EagerYesLetGo".Localized());

			OutputText("Scenes.Jojoscene.PureJojoSexMenu.EscortToPrivateForest".Localized());

			OutputText("Scenes.Jojoscene.PureJojoSexMenu.ChooseSexPosition".Localized());

			if (Debug)
			{
				OutputText("Scenes.Jojoscene.PureJojoSexMenu.DebugStatsHeader".Localized());

				OutputText("Scenes.Jojoscene.PureJojoSexMenu.AnalCapacityLabel".Localized(
					capacity
				));

				OutputText("Scenes.Jojoscene.PureJojoSexMenu.CumProductionLabel".Localized(
					JojoCumQ()
				));
			}

			ClearButtons();

			if (Player.HasCockThatFits(capacity))
				AddButton(0, "Common.Button.AnalPitch".Localized(), AnallyFuckTheMouseButtSlut);

			AddButton(1, "Common.Button.AnalCatch".Localized(), GetAnallyFuckedByMouseYouSlut);

			if (Player.HasVagina())
				AddButton(2, "Common.Button.VaginalCatch".Localized(), GetVagFuckedByMouse);

			AddButton(3, "Common.Button.BlowHim".Localized(), SuckJojosCock);

			AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), JojoCampMenu);
		}

		public void AnallyFuckTheMouseButtSlut()
		{
			ClearOutput();

			var capacity = 40;

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) < 10)
				capacity += (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) * 3);
			else
				capacity += 30;

			var x = Player.CockThatFits(40);

			JojoSprite();

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) == 0)
				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.FirstTimeButtResolve".Localized(
					Player.ClothedOrNaked("You remove your [armor] and ")
				));
			else
				if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) == 1)
					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.HesitantButtResolve".Localized(
						Player.ClothedOrNaked("You remove your [armor] and ")
					));
				else
					if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) >= 2)
						OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.EagerButtResolve".Localized(
							Player.ClothedOrNaked("You remove your [armor] and ")
						));

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) < 3)
			{
				if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) < 1)
					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.JojoWhimpersKneels".Localized());
				else
					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.NervousGentlenessPlea".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.CaressTonedCheeks".Localized());

				if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) < 1)
					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.TooTightNeedsStretching".Localized(
						Player.CockDescript(x)
					));
				else
					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.WarnHandTest".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.PrecumLeakAtBreach".Localized());

				if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) < 1)
					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.SlowCarefulPrep".Localized());
				else
					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.RelaxesFasterThisTime".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.SpitLubeCock".Localized(Player.CockDescript(x), Player.CockDescript(x), (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) > 0 ? "Common.Text.ByMaraeHesTightAsEver".Localized() : "Common.Text.ByMaraeHesTight".Localized())));

				if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) < 1)
				{
					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.AnalVirginityLost".Localized());

					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.CheckHeIsAlright".Localized());
				}
				else
					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.PantingThumbsUp".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.FullMastPaceUp".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.SeedIntoBowels".Localized());

				if (CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) < 1)
				{
					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.JojoOrgasmGrass".Localized());

					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.SpentBesideTrail".Localized());
				}
				else
				{
					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.PumpsBackTakeAll".Localized());

					OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.SpentBesidePraise".Localized());
				}
			}
			else
			{
				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.HurriedAllFours".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.EagerAlreadyHard".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.DigitsInvadePrecum".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.LooserThanBefore".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.EvenSalivaCoating".Localized(
					Player.CockDescript(x),

					Player.CockDescript(x)
				));

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.PantingOkReassure".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.SeedIntoBowelsAgain".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.HipsPumpTailWrap".Localized());

				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.SpentBesideLaugh".Localized());
			}

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) >= 4)
				OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.KissOnLips".Localized());

			OutputText("Scenes.Jojoscene.AnallyFuckTheMouseButtSlut.RestThenReturnCamp".Localized(
				Player.ClothedOrNaked("the two of you get redressed and", "Jojo gets redressed and the two of you")
			));

			Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

			CoC.Instance.Flags.Set(GameFlagIds.JojoAnalXp, CoC.Instance.Flags.Get(GameFlagIds.JojoAnalXp) + 1);

			CoC.Instance.Flags.Set(GameFlagIds.JojoSexCounter, CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) + 1);

			Player.Orgasm("Dick");

			DoNext(() => ReturnToCamp(1));
		}

		public void GetAnallyFuckedByMouseYouSlut()
		{
			ClearOutput();

			var isVirgin = (Player.Anal.Looseness == 0);

			JojoSprite();

			OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.InviteMouseCock".Localized(Player.ClothedOrNaked("You remove your [armor] and "), (CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) < 4 ? "hesitantly " : "")));

			OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.InstructedAllFours".Localized());

			OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.GentleButtMassage".Localized());

			Player.ButtChange(CombatController.Instance!.Context.Enemy.BiggestCockArea(), true);

			if (isVirgin)
				OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.TightnessEnjoyed".Localized());
			else
				if (Player.Anal.Looseness < 2)
					OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.TightRemark".Localized());
				else
					OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.NiceRemark".Localized());

			OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.SlowHipRocking".Localized());

			OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.AskFasterReply".Localized());

			OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.TensionBuildsFaster".Localized(
				JojoCumQ() >= 750 ? ", filling your bowels" + (JojoCumQ() >= 1000 ? " and distending your belly" : "") : ""
			));

			if (Player.HasCock())
				OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.PleasureTwitchSoak".Localized());

			if (Player.HasVagina())
				OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.SpontaneousJuiceSpill".Localized((Player.AverageVaginalWetness() >= 4 ? "Common.Text.Sprays".Localized() : "Common.Text.Leaks".Localized())));

			if (Player.Gender == Gender.Genderless)
				OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.BodyRocksEcstasy".Localized());

			OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.SlidesOutCumDrip".Localized((CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) >= 4 ? "You give Jojo a lingering kiss." : "")));

			OutputText("Scenes.Jojoscene.GetAnallyFuckedByMouseYouSlut.ReturnCampAfterRest".Localized(
				Player.ClothedOrNaked("the two of you get redressed and", "Jojo gets redressed and the two of you")
			));

			Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, -1f));

			CoC.Instance.Flags.Set(GameFlagIds.JojoAnalCatchCounter, CoC.Instance.Flags.Get(GameFlagIds.JojoAnalCatchCounter) + 1);

			CoC.Instance.Flags.Set(GameFlagIds.JojoSexCounter, CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) + 1);

			Player.Orgasm("Anal");

			Player.SlimeFeed();

			DoNext(() => ReturnToCamp(1));
		}

		public void GetVagFuckedByMouse()
		{
			ClearOutput();

			var isVirgin = ((Player.Vaginas.Count == 0 ? 0 : Player.Vaginas[0].Looseness) == 0);

			JojoSprite();

			OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.YearnForMouseCock".Localized(Player.ClothedOrNaked("You remove your [armor] and "), (CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) < 4 ? "hesitantly " : "")));

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoVaginalCatchCounter) == 0)
			{
				OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.SpreadLipsReassure".Localized());

				OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.HesitateThenJam".Localized());

				Player.CuntChange(CombatController.Instance!.Context.Enemy.BiggestCockArea(), true);

				OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.ClenchTrapDick".Localized());

				OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.RelaxIntoRhythm".Localized(
					JojoCumQ() >= 750 ? "His orgasm seems to never end and he continues to stuff your womb. " + (JojoCumQ() >= 900 ? "Excessive mouse-spunk spills out of your [pussy]. Gods, that was intense!" : "") : ""
				));

				OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.MutualOrgasmSpray".Localized((Player.HasCock() ? "; [eachCock] let loose a cum shower, drenching your belly and thighs" : "")));
			}
			else
				if (CoC.Instance.Flags.Get(GameFlagIds.JojoVaginalCatchCounter) == 1)
				{
					OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.SootheLingeringWorry".Localized());

					OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.EmbarrassedForeplayAsk".Localized());

					Player.CuntChange(CombatController.Instance!.Context.Enemy.BiggestCockArea(), true);

					OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.SlowGyratingStart".Localized(
						JojoCumQ() >= 750 ? "His orgasm seems to never end and he continues to stuff your womb. " + (JojoCumQ() >= 900 ? "Excessive mouse-spunk spills out of your [pussy]. Gods, that was intense!" : "") : ""
					));

					OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.MutualOrgasmSprayAgain".Localized((Player.HasCock() ? "; [eachCock] let loose a cum shower, drenching your belly and thighs" : "")));
				}
				else
					if (CoC.Instance.Flags.Get(GameFlagIds.JojoVaginalCatchCounter) >= 2)
					{
						OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.AlreadyErectEntry".Localized());

						Player.CuntChange(CombatController.Instance!.Context.Enemy.BiggestCockArea(), true);

						OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.SlowGyratingStartAgain".Localized());

						OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.MutualOrgasmSprayThird".Localized((Player.HasCock() ? "; [eachCock] let loose a cum shower, drenching your belly and thighs" : ""), (CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) >= 4 ? "You give Jojo a lingering kiss." : "")));
					}

			OutputText("Scenes.Jojoscene.GetVagFuckedByMouse.ReturnAfterRecovery".Localized(
				Player.ClothedOrNaked("the two of you get redressed and", "Jojo gets redressed and the two of you")
			));

			Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, -1f));

			CoC.Instance.Flags.Set(GameFlagIds.JojoVaginalCatchCounter, CoC.Instance.Flags.Get(GameFlagIds.JojoVaginalCatchCounter) + 1);

			CoC.Instance.Flags.Set(GameFlagIds.JojoSexCounter, CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) + 1);

			Player.KnockUp(PregnancyIds.Jojo, PregnancyIds.MouseIncubation + 82, JojoCumQ() < 2000 ? (int)(100 - (JojoCumQ() / 50)) : 60);

			Player.Orgasm("Vaginal");

			Player.SlimeFeed();

			DoNext(() => ReturnToCamp(1));
		}

		public void GiveBirthToPureJojoBabies()
		{

			OutputText("Scenes.Jojoscene.GiveBirthToPureJojoBabies.CervixPainCry".Localized((noFur ? "" : ", and fur the same color as your own hair starts to cover its body")));

			OutputText("Scenes.Jojoscene.GiveBirthToPureJojoBabies.RecoveredLitterNearby".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
			{
				if (CoC.Instance.Flags.Get(GameFlagIds.JojoLittersAmilyReactionCounter) == 0)
				{
					OutputText("Scenes.Jojoscene.GiveBirthToPureJojoBabies.AmilyRushesIn".Localized());

					OutputText("Scenes.Jojoscene.GiveBirthToPureJojoBabies.SlumberGlimpseAmily".Localized());
				}
				else
				{
					OutputText("Scenes.Jojoscene.GiveBirthToPureJojoBabies.RecoveredLitterNearbyAgain".Localized());

					OutputText("Scenes.Jojoscene.GiveBirthToPureJojoBabies.KidsTakeLitter".Localized());
				}

				CoC.Instance.Flags.Set(GameFlagIds.JojoLittersAmilyReactionCounter, CoC.Instance.Flags.Get(GameFlagIds.JojoLittersAmilyReactionCounter) + 1);
			}
			else
				OutputText("Scenes.Jojoscene.GiveBirthToPureJojoBabies.JojoThanksSincerely".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.JojoLitters, CoC.Instance.Flags.Get(GameFlagIds.JojoLitters) + 1);
		}

		public void SuckJojosCock()
		{
			ClearOutput();

			JojoSprite();

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoBlowjobXp) <= 0)
				OutputText("Scenes.Jojoscene.SuckJojosCock.HisTurnPleasure".Localized(
					Player.ClothedOrNaked("You remove your [armor] and ")
				));
			else
				OutputText("Scenes.Jojoscene.SuckJojosCock.TasteHisDick".Localized(
					Player.ClothedOrNaked("You remove your [armor] and ")
				));

			OutputText("Scenes.Jojoscene.SuckJojosCock.PositionTeasePlan".Localized((Player.Gender > 0 ? ", " : ""), (Player.HasCock() ? "your [cocks] leaking pre" : ""), (Player.Gender == Gender.Hermaphrodite ? " and " : ""), (Player.HasVagina() ? "your [pussy] leaking juices" : "")));

			OutputText("Scenes.Jojoscene.SuckJojosCock.LickLipsThanks".Localized());

			OutputText("Scenes.Jojoscene.SuckJojosCock.MoaningBallsPrecum".Localized());

			OutputText("Scenes.Jojoscene.SuckJojosCock.DeepThroatRhythm".Localized((JojoCumQ() >= 600 ? "Common.Text.Distending".Localized() : "Common.Text.Stuffing".Localized()), (JojoCumQ() >= 900 ? "His orgasm seems to never end and he continues to cum for a good while." : "")));

			if (CoC.Instance.Flags.Get(GameFlagIds.JojoBlowjobXp) <= 0)
				OutputText("Scenes.Jojoscene.SuckJojosCock.CollapseLegsNumb".Localized());
			else
				OutputText("Scenes.Jojoscene.SuckJojosCock.CollapseDeliciousPraise".Localized());

			OutputText("Scenes.Jojoscene.SuckJojosCock.RestThenReturnCamp".Localized(
				Player.ClothedOrNaked("the two of you get redressed and", "Jojo gets redressed and the two of you")
			));

			if (JojoCumQ() < 2500)
				Player.RefillHunger(JojoCumQ() / 25);
			else
				Player.RefillHunger(100);

			CoC.Instance.Flags.Set(GameFlagIds.JojoBlowjobXp, CoC.Instance.Flags.Get(GameFlagIds.JojoBlowjobXp) + 1);

			CoC.Instance.Flags.Set(GameFlagIds.JojoSexCounter, CoC.Instance.Flags.Get(GameFlagIds.JojoSexCounter) + 1);

			Player.ApplyStats(new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Corruption, -1f));

			Player.SlimeFeed();

			DoNext(() => ReturnToCamp(1));
		}

		public void AmilyDiscoversTentacleJojo()
		{
			ClearOutput();

			CoC.Instance.Flags.Set(GameFlagIds.AmilyDiscoveredTentatleJojo,

				CoC.Instance.Flags.Get(GameFlagIds.AmilyDiscoveredTentatleJojo) + 1);

			OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.SettleDownFuck".Localized());

			OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.WhatDidYouDo".Localized());

			OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.AmilyScreams".Localized());

			if (!Player.IsPureEnough(66))
			{
				OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.FlipHerBird".Localized());

				OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.RanBackRuins".Localized());

				CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

				CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

				Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

				ReturnToCamp(1);

				return;
			}

			OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.ThisIsBad".Localized());

			AddButton(0, "Common.Button.Explain".Localized(), () =>
			{
				OutputText("Scenes.Followerinteractions.AerisDies.ListensImpassively".Localized());

				CoC.Instance.Flags.Set(GameFlagIds.AmilyWaitForPcFixJojo, 1);

				ReturnToCamp(1);
			});

			AddButton(1, "Common.Button.FlipBird".Localized(), () =>
			{
				OutputText("Scenes.Followerinteractions.AmilyLeavesBirdJojoTentacles.FlipHerBird".Localized());

				CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

				CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

				Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

				ReturnToCamp(1);
			});
		}

		public void AmilyIsPissedAtYou()
		{
			ClearOutput();

			CoC.Instance.Flags.Set(GameFlagIds.AmilyPissedPcCorrupedJojo,

				CoC.Instance.Flags.Get(GameFlagIds.AmilyPissedPcCorrupedJojo) + 1);

			OutputText("Scenes.Followerinteractions.AmilyIsPissedAtYouForRuiningJojo.PersonalFuckPet".Localized(Player.HasFur() ? "furry " : ""));

			OutputText("Scenes.Followerinteractions.AmilyIsPissedAtYouForRuiningJojo.WhatDidYouDo".Localized());

			OutputText("Scenes.Followerinteractions.AmilyIsPissedAtYouForRuiningJojo.AmilyScreams".Localized());

			OutputText("Scenes.Followerinteractions.AmilyIsPissedAtYouForRuiningJojo.AtCrossroads".Localized());

			AddButton(0, "Common.Button.FixHim".Localized(), () =>
			{
				OutputText("Scenes.Followerinteractions.AgreeToHelpAmilyFixJojo.YouSay".Localized());

				CoC.Instance.Flags.Set(GameFlagIds.AmilyWaitForPcFixJojo, 1);

				Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));

				ReturnToCamp(1);
			});

			AddButton(1, "Common.Button.FuckOff".Localized(), () =>
			{
				OutputText("Scenes.Followerinteractions.TellAmilyToFuckOffRuinedJojo.BetterFuck".Localized());

				OutputText("Scenes.Followerinteractions.TellAmilyToFuckOffRuinedJojo.SadnessPalpable".Localized());

				CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

				CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

				CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 0);

				CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageAccessible, 1);

				Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

				ReturnToCamp(1);
			});
		}

	}
}