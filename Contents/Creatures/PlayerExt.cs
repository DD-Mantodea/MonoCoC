using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Races;
using System;
using System.Linq;

namespace MonoCoC.Contents.Creatures
{
    public static class PlayerExt
    {
        public static int CockCount(Player player, string type) => player.Cocks.Count(cock => cock.Id.Path == type);

        public static int RowCount(Player player) => player.BreastRows.Count;

        public static float BiggestTit(Player player) => player.BiggestBreastSize();

        public static bool Matches(string typeId, string token) =>

            typeId == token ||

            typeId.StartsWith(token, StringComparison.Ordinal) ||

            typeId.Contains("_" + token, StringComparison.Ordinal);

        public static float DefaultScore(this Player player, Race race)
        {
            var token = race.Id.Path;

            var score = 0f;

            if (Matches(player.PartId<FacePart>(), token))
                score += 1;

            if (Matches(player.PartId<EarsPart>(), token))
                score += 1;

            if (Matches(player.PartId<TailPart>(), token))
                score += 1;

            if (Matches(player.PartId<LowerBodyPart>(), token))
                score += 1;

            if (Matches(player.PartId<ArmsPart>(), token))
                score += 1;

            if (Matches(player.PartId<TonguePart>(), token))
                score += 1;

            if (Matches(player.PartId<EyesPart>(), token))
                score += 1;

            if (Matches(player.PartId<HornsPart>(), token))
                score += 1;

            if (Matches(player.PartId<WingsPart>(), token))
                score += 1;

            if (Matches(player.PartId<NeckPart>(), token))
                score += 1;

            if (Matches(player.PartId<RearBodyPart>(), token))
                score += 1;

            if (Matches(player.PartId<UnderBodyPart>(), token))
                score += 1;

            if (Matches(player.PartId<GillsPart>(), token))
                score += 1;

            if (Matches(player.PartId<AntennaePart>(), token))
                score += 1;

            if (Matches(player.PartId<HairPart>(), token))
                score += 1;

            if (Matches(player.PartId<SkinPart>(), token))
                score += 1;

            if (player.Cocks.Any(cock => Matches(cock.Id.Path, token)))
                score += 1;

            return score;
        }

        public static float HumanScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("human"))
                counter += 1;

            if (player.IsPart<SkinPart>("plain"))
                counter += 1;

            if (player.IsPart<HornsPart>("none"))
                counter += 1;

            if (player.IsPart<TailPart>("none"))
                counter += 1;

            if (player.IsPart<WingsPart>("none"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("human"))
                counter += 1;

            if (CockCount(player, "human") == 1 && player.Cocks.Count == 1)
                counter += 1;

            if (RowCount(player) == 1 && player.IsPart<SkinPart>("plain"))
                counter += 1;

            return counter;
        }

        public static float DemonScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<HornsPart>("demon") && player.Body.Get<HornsPart>().Value > 0)
                counter += 1;

            if (player.IsPart<HornsPart>("demon") && player.Body.Get<HornsPart>().Value > 4)
                counter += 1;

            if (player.IsPart<TailPart>("demonic"))
                counter += 1;

            if (player.HasBatLikeWings())
                counter += 1;

            if (player.HasPlainSkin() && player.Corruption > 50)
                counter += 1;

            if (player.IsPart<FacePart>("human") && player.Corruption > 50)
                counter += 1;

            if (player.IsPart<LowerBodyPart>("demonic_high_heels", "demonic_claws"))
                counter += 1;

            if (CockCount(player, "demon") > 0)
                counter += 1;

            return counter;
        }

        public static float MinoScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("cow_minotaur"))
                counter += 1;

            if (player.IsPart<EarsPart>("cow"))
                counter += 1;

            if (player.IsPart<TailPart>("cow"))
                counter += 1;

            if (player.IsPart<HornsPart>("cow_minotaur"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("hoofed") && counter > 0)
                counter += 1;

            if (player.Tallness > 80 && counter > 0)
                counter += 1;

            if (player.Cocks.Count > 0 && counter > 0 && CockCount(player, "horse") > 0)
                counter += 1;

            if (player.Vaginas.Count > 0)
                counter -= 1;

            return counter;
        }

        public static float CowScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<EarsPart>("cow"))
                counter += 1;

            if (player.IsPart<TailPart>("cow"))
                counter += 1;

            if (player.IsPart<HornsPart>("cow_minotaur"))
                counter += 1;

            if (player.IsPart<FacePart>("human") && counter > 0)
                counter += 1;

            if (player.IsPart<FacePart>("cow_minotaur"))
                counter -= 1;

            if (player.IsPart<LowerBodyPart>("hoofed") && counter > 0)
                counter += 1;

            if (player.Tallness >= 73 && counter > 0)
                counter += 1;

            if (player.Vaginas.Count > 0 && counter > 0)
                counter += 1;

            if (BiggestTit(player) > 4 && counter > 0)
                counter += 1;

            if (player.BiggestLactation() > 2 && counter > 0)
                counter += 1;

            return counter;
        }

        public static float SandTrapScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.HasStatusEffect(StatusEffectIds.BlackNipples))
                counter += 1;

            if (player.HasStatusEffect(StatusEffectIds.Uniball))
                counter += 1;

            if (player.Vaginas.Count > 0 && player.Vaginas[0].Id.Path == "black_sand_trap")
                counter += 1;

            if (player.IsPart<EyesPart>("black_eyes_sand_trap"))
                counter += 1;

            if (player.IsPart<WingsPart>("giant_dragonfly"))
                counter += 1;

            if (player.HasStatusEffect(StatusEffectIds.Uniball))
                counter += 1;

            return counter;
        }

        public static float InsectScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<AntennaePart>("bee"))
                counter += 1;

            if (player.IsPart<ArmsPart>("spider", "bee"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("bee", "chitinous_spider_legs", "drider"))
                counter += 1;

            if (player.IsPart<TailPart>("bee_abdomen", "spider_abdomen", "scorpion"))
                counter += 1;

            if (player.IsPart<WingsPart>("giant_dragonfly", "bee_like_small", "bee_like_large", "faerie_large", "faerie_small"))
                counter += 1;

            if (player.IsPart<FacePart>("spider_fangs"))
                counter += 1;

            return counter;
        }

        public static float BeeScore(this Player player, Race race = null)
        {
            var counter = 0f;

            var hair = player.Body.Get<HairPart>().Color;

            if (hair == "shiny black")
                counter += 1;

            if (hair == "black and yellow")
                counter += 2;

            if (player.IsPart<AntennaePart>("bee"))
            {
                counter += 1;

                if (player.IsPart<FacePart>("human"))
                    counter += 1;
            }

            if (player.IsPart<ArmsPart>("bee"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("bee"))
            {
                counter += 1;

                if (player.Vaginas.Count == 1)
                    counter += 1;
            }

            if (player.IsPart<TailPart>("bee_abdomen"))
                counter += 1;

            if (player.IsPart<WingsPart>("bee_like_small"))
                counter += 1;

            if (player.IsPart<WingsPart>("bee_like_large"))
                counter += 1;

            return counter;
        }

        public static float FerretScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("ferret_mask"))
                counter += 1;

            if (player.IsPart<FacePart>("ferret"))
                counter += 2;

            if (player.IsPart<EarsPart>("ferret"))
                counter += 1;

            if (player.IsPart<TailPart>("ferret"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("ferret"))
                counter += 1;

            if (player.IsPart<ArmsPart>("ferret"))
                counter += 1;

            if (counter >= 2 && player.HasFur())
                counter += 2;

            return counter;
        }

        public static float WolfScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("wolf"))
                counter += 1;

            if (CockCount(player, "wolf") > 0)
                counter += 1;

            if (player.IsPart<EarsPart>("wolf"))
                counter += 1;

            if (player.IsPart<TailPart>("wolf"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("wolf"))
                counter += 1;

            if (player.IsPart<EyesPart>("wolf"))
                counter += 2;

            if (player.HasFur() && counter > 0)
                counter += 1;

            if (counter >= 2)
            {
                if (RowCount(player) > 1)
                    counter += 1;

                if (RowCount(player) == 4)
                    counter += 1;

                if (RowCount(player) > 4)
                    counter -= 1;
            }

            return counter;
        }

        public static float DogScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("dog"))
                counter += 1;

            if (player.IsPart<EarsPart>("dog"))
                counter += 1;

            if (player.IsPart<TailPart>("dog"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("dog"))
                counter += 1;

            if (player.IsPart<ArmsPart>("dog"))
                counter += 1;

            if (CockCount(player, "dog") > 0)
                counter += 1;

            if (player.HasFur() && counter > 0)
                counter += 1;

            if (counter >= 2)
            {
                if (RowCount(player) > 1)
                    counter += 1;

                if (RowCount(player) == 3)
                    counter += 1;

                if (RowCount(player) > 3)
                    counter -= 1;
            }

            return counter;
        }

        public static float MouseScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<EarsPart>("mouse"))
                counter += 1;

            if (player.IsPart<TailPart>("mouse"))
                counter += 1;

            if (player.IsPart<FacePart>("buckteeth"))
                counter += 1;

            if (player.IsPart<FacePart>("mouse"))
                counter += 2;

            if (player.HasFur() && counter > 0)
                counter += 1;

            if (player.Tallness < 55 && counter > 0)
                counter += 1;

            if (player.Tallness < 45 && counter > 0)
                counter += 1;

            return counter;
        }

        public static float RaccoonScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("raccoon_mask"))
                counter += 1;

            if (player.IsPart<FacePart>("raccoon"))
                counter += 2;

            if (player.IsPart<EarsPart>("raccoon"))
                counter += 1;

            if (player.IsPart<TailPart>("raccoon"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("raccoon"))
                counter += 1;

            if (counter > 0 && player.Balls > 0)
                counter += 1;

            if (player.HasFur() && counter > 0)
                counter += 1;

            return counter;
        }

        public static float FoxScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("fox"))
                counter += 1;

            if (player.IsPart<EarsPart>("fox"))
                counter += 1;

            if (player.IsPart<TailPart>("fox"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("fox"))
                counter += 1;

            if (player.IsPart<ArmsPart>("fox"))
                counter += 1;

            if (CockCount(player, "dog") > 0 && counter > 0)
                counter += 1;

            if (RowCount(player) > 1 && counter > 0)
                counter += 1;

            if (RowCount(player) == 3 && counter > 0)
                counter += 1;

            if (RowCount(player) == 4 && counter > 0)
                counter += 1;

            if (player.HasFur() && counter > 0)
                counter += 1;

            return counter;
        }

        public static float CatScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.HasCatFace())
                counter += 1;

            if (player.IsPart<TonguePart>("cat"))
                counter += 1;

            if (player.IsPart<EarsPart>("cat"))
                counter += 1;

            if (player.IsPart<TailPart>("cat"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("cat"))
                counter += 1;

            if (player.IsPart<ArmsPart>("cat"))
                counter += 1;

            if (CockCount(player, "cat") > 0)
                counter += 1;

            if (RowCount(player) > 1 && counter > 0)
                counter += 1;

            if (RowCount(player) == 3 && counter > 0)
                counter += 1;

            if (RowCount(player) > 3)
                counter -= 2;

            if (player.HasFur() && counter > 0)
                counter += 1;

            return counter;
        }

        public static float LizardScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("lizard"))
                counter += 1;

            if (player.IsPart<EarsPart>("lizard"))
                counter += 1;

            if (player.IsPart<TailPart>("lizard"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("lizard"))
                counter += 1;

            if (player.HasDragonHorns())
                counter += 1;

            if (player.HasDragonHorns(true))
                counter += 1;

            if (player.IsPart<ArmsPart>("lizard"))
                counter += 1;

            if (counter > 2)
            {
                if (player.IsPart<TonguePart>("lizard", "snake"))
                    counter += 1;

                if (CockCount(player, "lizard") > 0)
                    counter += 1;

                if (player.IsPart<EyesPart>("lizard", "basilisk"))
                    counter += 1;

                if (player.HasReptileScales())
                    counter += 1;
            }

            return counter;
        }

        public static float SpiderScore(this Player player, Race race = null)
        {
            var counter = 0f;

            var spiderEyes = player.IsPart<EyesPart>("spider", "four_spider_eyes");

            if (spiderEyes && player.Body.Get<EyesPart>().Count == 4)
                counter += 2;
            else if (spiderEyes)
                counter += 1;

            if (player.IsPart<FacePart>("spider_fangs"))
                counter += 1;

            if (player.IsPart<ArmsPart>("spider"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("chitinous_spider_legs", "drider"))
                counter += 2;
            else if (counter > 0)
                counter -= 1;

            if (player.IsPart<TailPart>("spider_abdomen"))
                counter += 2;

            if (!player.HasPlainSkin() && counter > 0)
                counter -= 1;

            return counter;
        }

        public static float HorseScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("horse"))
                counter += 1;

            if (player.IsPart<EarsPart>("horse"))
                counter += 1;

            if (player.IsPart<TailPart>("horse"))
                counter += 1;

            if (CockCount(player, "horse") > 0)
                counter += 1;

            if (player.IsPart<LowerBodyPart>("hoofed"))
                counter += 1;

            if (player.HasFur() && counter > 0)
                counter += 1;

            return counter;
        }

        public static float KitsuneScore(this Player player, Race race = null)
        {
            var counter = 0f;

            var colors = player.HairOrFurColor();

            if (player.IsPart<EarsPart>("fox"))
                counter += 1;

            if (player.IsPart<TailPart>("fox"))
                counter += 1;

            if (player.IsPart<TailPart>("fox") && player.Body.Get<TailPart>().Venom >= 2)
                counter += 2;

            if (player.VaginalCapacity() >= 8000)
                counter += 1;

            if (counter > 0 && player.IsPart<FacePart>("human", "fox"))
                counter += 1;

            if (counter > 0 && (RaceColorRegistry.Has("basic_kitsune_hair", colors) || RaceColorRegistry.Has("elder_kitsune", colors)))
                counter += 1;

            if (counter > 0 && player.Femininity >= 40)
                counter += 1;

            if (player.HasFur() && !RaceColorRegistry.Has("basic_kitsune_outer_fur", colors) && !RaceColorRegistry.Has("elder_kitsune", colors))
                counter -= 1;

            if (player.HasScales())
                counter -= 2;

            if (player.HasGooSkin())
                counter -= 3;

            if (!player.IsPart<LowerBodyPart>("human", "fox"))
                counter -= 1;

            if (!player.IsPart<FacePart>("human", "fox"))
                counter -= 1;

            if (!player.IsPart<EarsPart>("fox"))
                counter -= 1;

            if (!player.IsPart<TailPart>("fox"))
                counter -= 1;

            return counter;
        }

        public static float DragonScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("dragon"))
                counter += 1;

            if (player.IsPart<EarsPart>("dragon"))
                counter += 1;

            if (player.IsPart<TailPart>("draconic"))
                counter += 1;

            if (player.IsPart<TonguePart>("draconic"))
                counter += 1;

            if (CockCount(player, "dragon") > 0)
                counter += 1;

            if (player.HasDragonWings())
                counter += 1;

            if (player.IsPart<LowerBodyPart>("dragon"))
                counter += 1;

            if (player.HasDragonScales() && counter > 0)
                counter += 1;

            if (player.HasDragonHorns())
                counter += 1;

            if (player.IsPart<HornsPart>("draconic_x4_12_inch_long"))
                counter += 1;

            if (player.HasDragonfire())
                counter += 1;

            if (player.IsPart<ArmsPart>("dragon"))
                counter += 1;

            if (player.IsPart<EyesPart>("dragon"))
                counter += 1;

            if (player.HasDragonNeck())
                counter += 1;

            if (player.HasDragonRearBody())
                counter += 1;

            return counter;
        }

        public static float GoblinScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (RaceColorRegistry.Has("goblin_skin", player.SkinTone))
                counter += 2;

            if (counter > 0)
            {
                if (player.IsPart<FacePart>("human"))
                    counter += 0.5f;

                if (player.IsPart<LowerBodyPart>("human"))
                    counter += 0.5f;

                if (player.IsPart<EarsPart>("elfin"))
                    counter += 1;

                if (player.Tallness < 48)
                    counter += 1;

                if (player.HasVagina())
                    counter += 1;
            }

            return counter;
        }

        public static float GnollScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("coc:gnoll"))
                counter += 1;

            if (player.IsPart<EarsPart>("coc:gnoll"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("coc:gnoll"))
                counter += 1;

            if (player.IsPart<ArmsPart>("coc:gnoll"))
                counter += 1;

            if (player.IsPart<TailPart>("coc:gnoll"))
                counter += 1;

            if (CockCount(player, "coc:gnoll") > 0)
                counter += 1;

            if (player.HasFur() && counter > 3)
                counter += 1;

            return counter;
        }

        public static float GooScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.HairType() == "goo")
                counter += 1;

            if (player.SkinAdj == "slimy")
                counter += 1;

            if (player.IsPart<LowerBodyPart>("goo"))
                counter += 1;

            if (player.VaginalCapacity() > 9000)
                counter += 1;

            if (player.HasStatusEffect(StatusEffectIds.SlimeCraving))
                counter += 1;

            return counter;
        }

        public static float NagaScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<LowerBodyPart>("coc:naga"))
                counter += 2;

            if (player.IsPart<FacePart>("snake_fangs"))
                counter += 1;

            if (player.IsPart<TonguePart>("snake"))
                counter += 1;

            if (counter > 0 && player.IsPart<AntennaePart>("none"))
                counter += 1;

            if (counter > 0 && player.IsPart<WingsPart>("none"))
                counter += 1;

            return counter;
        }

        public static float MermaidScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<LowerBodyPart>("mermaid"))
                counter += 2;

            if (player.IsPart<GillsPart>("mermaid"))
                counter += 1;

            if (counter > 0 && player.IsPart<AntennaePart>("none"))
                counter += 1;

            if (counter > 0 && player.IsPart<WingsPart>("none"))
                counter += 1;

            return counter;
        }

        public static float BatScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<LowerBodyPart>("bat"))
                counter += 2;

            if (player.IsPart<FacePart>("bat"))
                counter += 1;

            if (player.IsPart<WingsPart>("bat_like_large"))
                counter += 2;

            if (player.IsPart<EarsPart>("bat"))
                counter += 2;

            return counter;
        }

        public static float BunnyScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("bunny"))
                counter += 1;

            if (player.IsPart<TailPart>("rabbit"))
                counter += 1;

            if (player.IsPart<EarsPart>("bunny"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("bunny"))
                counter += 1;

            if (player.Balls > 2 && counter > 0)
                counter -= 1;

            if (player.HasPlainSkin() && counter > 1)
                counter += 1;

            if (counter > 0 && player.IsPart<AntennaePart>("none"))
                counter += 1;

            if (counter > 0 && player.IsPart<WingsPart>("none"))
                counter += 1;

            return counter;
        }

        public static float HarpyScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<ArmsPart>("harpy"))
                counter += 1;

            if (player.HairType() == "feather")
                counter += 1;

            if (player.IsPart<WingsPart>("feathered_large"))
                counter += 1;

            if (player.IsPart<TailPart>("harpy"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("harpy"))
                counter += 1;

            if (counter >= 2 && player.IsPart<FacePart>("human"))
                counter += 1;

            if (counter >= 2 && player.IsPart<EarsPart>("human", "elfin"))
                counter += 1;

            return counter;
        }

        public static float KangaScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (CockCount(player, "kangaroo") > 0)
                counter += 1;

            if (player.IsPart<EarsPart>("kangaroo"))
                counter += 1;

            if (player.IsPart<TailPart>("kangaroo"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("kangaroo"))
                counter += 1;

            if (player.IsPart<FacePart>("kangaroo"))
                counter += 1;

            if (counter >= 2 && player.HasFur())
                counter += 1;

            return counter;
        }

        public static float SheepScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<EarsPart>("sheep"))
                counter += 1;

            if (player.IsPart<HornsPart>("sheep"))
                counter += 1;

            if (player.IsPart<HornsPart>("ram"))
                counter += 1;

            if (player.IsPart<TailPart>("sheep"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("cloven_hoofed") && player.Body.Get<LowerBodyPart>().LegCount == 2)
                counter += 1;

            if (player.HairType() == "wool")
                counter += 1;

            if (player.HasWool())
                counter += 1;

            return counter;
        }

        public static float SharkScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("shark_teeth"))
                counter += 1;

            if (player.IsPart<GillsPart>("fish"))
                counter += 1;

            if (player.IsPart<RearBodyPart>("shark_fin"))
                counter += 1;

            if (player.IsPart<TailPart>("shark"))
                counter += 1;

            if (player.HasPlainSkin() && counter > 0)
                counter += 1;

            return counter;
        }

        public static float MutantScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (!player.IsPart<FacePart>("human"))
                counter += 1;

            if (!player.IsPart<TailPart>("none"))
                counter += 1;

            if (player.Cocks.Count > 1)
                counter += 1;

            if (player.HasCock() && player.HasVagina())
                counter += 1;

            if (player.HasFuckableNipples())
                counter += 1;

            if (RowCount(player) > 1)
                counter += 1;

            if (counter > 1 && player.HasPlainSkin())
                counter += 1;

            if (player.IsPart<FacePart>("horse"))
            {
                if (player.HasFur())
                    counter -= 1;

                if (player.IsPart<TailPart>("horse"))
                    counter -= 1;
            }

            if (player.IsPart<FacePart>("dog"))
            {
                if (player.HasFur())
                    counter -= 1;

                if (player.IsPart<TailPart>("dog"))
                    counter -= 1;
            }

            return counter;
        }

        public static float SalamanderScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<ArmsPart>("salamander"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("salamander"))
                counter += 1;

            if (player.IsPart<TailPart>("salamander"))
                counter += 1;

            if (player.HasPerk(PerkIds.Lustserker))
                counter += 1;

            if (counter >= 2)
            {
                if (CockCount(player, "lizard") > 0)
                    counter += 1;

                if (player.IsPart<FacePart>("human"))
                    counter += 1;

                if (player.IsPart<EarsPart>("human"))
                    counter += 1;
            }

            return counter;
        }

        public static float DryadScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.HairType() == "vine")
            {
                counter += 1;

                var hair = player.Body.Get<HairPart>();

                if (hair.Adj == "leafy" || hair.FlowerColor != "")
                    counter += 1;
            }

            if (player.IsPart<SkinPart>("stalk"))
                counter += 1;

            if (player.IsPart<SkinPart>("wooden"))
                counter += 2;

            if (player.IsPart<RearBodyPart>("bark"))
                counter += 1;

            if (player.IsPart<HornsPart>("wooden"))
                counter += 1;

            if (player.IsPart<WingsPart>("wooden"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("root_legs"))
                counter += 1;

            if (counter >= 1 && player.IsPart<EarsPart>("elfin"))
                counter += 1;

            return counter;
        }

        public static float SirenScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<FacePart>("shark_teeth") &&
                player.IsPart<TailPart>("shark") &&

                player.IsPart<WingsPart>("feathered_large") &&

                player.IsPart<ArmsPart>("harpy"))

                counter += 4;

            if (counter > 0 && player.HasVagina())
                counter += 1;

            return counter;
        }

        public static float PigScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<EarsPart>("pig"))
                counter += 1;

            if (player.IsPart<TailPart>("pig"))
                counter += 1;

            if (player.IsPart<FacePart>("pig", "boar"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("cloven_hoofed"))
                counter += 2;

            if (CockCount(player, "pig") > 0)
                counter += 1;

            return counter;
        }

        public static float SatyrScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<LowerBodyPart>("cloven_hoofed"))
                counter += 1;

            if (player.IsPart<TailPart>("goat"))
                counter += 1;

            if (counter >= 2)
            {
                if (player.IsPart<EarsPart>("elfin"))
                    counter += 1;

                if (player.IsPart<FacePart>("human"))
                    counter += 1;

                if (CockCount(player, "human") > 0)
                    counter += 1;

                if (player.Balls > 0 && player.BallSize >= 3)
                    counter += 1;
            }

            return counter;
        }

        public static float RhinoScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<EarsPart>("rhino"))
                counter += 1;

            if (player.IsPart<TailPart>("rhino"))
                counter += 1;

            if (player.IsPart<FacePart>("rhino"))
                counter += 1;

            if (player.IsPart<HornsPart>("rhino"))
                counter += 1;

            if (counter >= 2 && player.SkinTone == "gray")
                counter += 1;

            if (counter >= 2 && player.HasCock() && CockCount(player, "rhino") > 0)
                counter += 1;

            return counter;
        }

        public static float EchidnaScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<EarsPart>("echidna"))
                counter += 1;

            if (player.IsPart<TailPart>("echidna"))
                counter += 1;

            if (player.IsPart<FacePart>("echidna"))
                counter += 1;

            if (player.IsPart<TonguePart>("echidna"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("echidna"))
                counter += 1;

            if (counter >= 2 && player.HasFur())
                counter += 1;

            if (counter >= 2 && CockCount(player, "echidna") > 0)
                counter += 1;

            return counter;
        }

        public static float DeerScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<EarsPart>("deer"))
                counter += 1;

            if (player.IsPart<TailPart>("deer"))
                counter += 1;

            if (player.IsPart<FacePart>("deer"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("cloven_hoofed"))
                counter += 1;

            if (player.IsPart<HornsPart>("antlers") && player.Body.Get<HornsPart>().Value >= 4)
                counter += 1;

            if (counter >= 2 && player.HasFur())
                counter += 1;

            if (counter >= 3 && CockCount(player, "horse") > 0)
                counter += 1;

            return counter;
        }

        public static float CockatriceScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<EarsPart>("cockatrice"))
                counter += 1;

            if (player.IsPart<TailPart>("cockatrice"))
                counter += 1;

            if (player.IsPart<LowerBodyPart>("cockatrice"))
                counter += 1;

            if (player.IsPart<FacePart>("cockatrice"))
                counter += 1;

            if (player.IsPart<EyesPart>("cockatrice"))
                counter += 1;

            if (player.IsPart<ArmsPart>("cockatrice"))
                counter += 1;

            if (player.IsPart<AntennaePart>("cockatrice"))
                counter += 1;

            if (player.IsPart<NeckPart>("cockatrice"))
                counter += 1;

            if (counter > 2)
            {
                if (player.IsPart<TonguePart>("lizard"))
                    counter += 1;

                if (player.IsPart<WingsPart>("feathered_large"))
                    counter += 1;

                if (player.IsPart<SkinPart>("lizard_scales"))
                    counter += 1;

                if (player.IsPart<UnderBodyPart>("cockatrice"))
                    counter += 1;

                if (CockCount(player, "lizard") > 0)
                    counter += 1;
            }

            return counter;
        }

        public static float ImpScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<EarsPart>("coc:imp"))
                counter += 1;

            if (player.IsPart<TailPart>("coc:imp"))
                counter += 1;

            if (player.IsPart<WingsPart>("coc:imp"))
                counter += 1;

            if (player.IsPart<WingsPart>("imp_large"))
                counter += 2;

            if (player.IsPart<LowerBodyPart>("coc:imp"))
                counter += 1;

            if (player.HasPlainSkin() && player.SkinTone is "red" or "orange")
                counter += 1;

            if (player.IsPart<HornsPart>("coc:imp"))
                counter += 1;

            if (player.IsPart<ArmsPart>("predator") && player.Body.Get<ArmsPart>().Claws.Id.Path == "coc:imp")
                counter += 1;

            if (player.Tallness <= 42)
                counter += 1;

            if (player.Tallness > 42)
                counter -= 1;

            if (BiggestTit(player) >= 1)
                counter -= 1;

            if (RowCount(player) == 2)
                counter -= 1;

            if (RowCount(player) == 3)
                counter -= 2;

            if (RowCount(player) == 4)
                counter -= 3;

            return counter;
        }

        public static float RedPandaScore(this Player player, Race race = null)
        {
            var counter = 0f;

            if (player.IsPart<EarsPart>("red_panda"))
                counter += 1;

            if (player.IsPart<TailPart>("red_panda"))
                counter += 1;

            if (player.IsPart<ArmsPart>("red_panda"))
                counter += 1;

            if (player.IsPart<FacePart>("red_panda"))
                counter += 2;

            if (player.IsPart<LowerBodyPart>("red_panda"))
                counter += 1;

            if (counter >= 2)
            {
                if (player.HasFur())
                    counter += 1;

                if (player.HasFurryUnderBody())
                    counter += 1;
            }

            return counter;
        }
    }
}