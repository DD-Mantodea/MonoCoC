namespace MonoCoC.Contents.Achievements
{
    public sealed record AchievementEntry(
        string Section, string Title, string Id, string DescLocked, string DescUnlocked, bool Shadow);

    // 由原版 Achievements.hx 的 achievementsScreen() 逐条生成
    public static class AchievementTable
    {
        public static readonly AchievementEntry[] Entries =

        [
            new("Storyline", "Newcomer", "coc:story_newcomer", "Enter the realm of Mareth.", "", false),

            new("Storyline", "Marae's Savior", "coc:story_marae_savior", "Complete Marae's quest.", "", false),

            new("Storyline", "Revenge at Last", "coc:story_zetaz_revenge", "Defeat Zetaz and obtain the map.", "", false),

            new("Storyline", "Demon Slayer", "coc:story_finalboss", "Defeat Lethice.", "", false),

            new("Zones", "Explorer", "coc:zone_explorer", "Discover every zone.", "", false),

            new("Zones", "Sightseer", "coc:zone_sightseer", "Discover every place.", "", false),

            new("Zones", "Where am I?", "coc:zone_where_am_i", "Explore for the first time.", "", false),

            new("Zones", "Forest Ranger", "coc:zone_forest_ranger", "Explore the forest 100 times.", "", false),

            new("Zones", "Vacationer", "coc:zone_vacationer", "Explore the lake 100 times.", "", false),

            new("Zones", "Dehydrated", "coc:zone_dehydrated", "Explore the desert 100 times.", "", false),

            new("Zones", "Mountaineer", "coc:zone_mountaineer", "Explore the mountains 100 times.", "", false),

            new("Zones", "Rolling Hills", "coc:zone_rolling_hills", "Explore the plains 100 times.", "", false),

            new("Zones", "Wet All Over", "coc:zone_wet_all_over", "Explore the swamp 100 times.", "", false),

            new("Zones", "We Need to Go Deeper", "coc:zone_we_need_to_go_deeper", "Explore the deepwoods 100 times.", "", false),

            new("Zones", "Light-headed", "coc:zone_light_headed", "Explore the high mountains 100 times.", "", false),

            new("Zones", "All Murky", "coc:zone_all_murky", "Explore the bog 100 times.", "", false),

            new("Zones", "Frozen", "coc:zone_frozen", "Explore the glacial rift 100 times.", "", false),

            new("Zones", "Roasted", "coc:zone_roasted", "Explore the volcanic crag 100 times.", "", false),

            new("Zones", "Archaeologist", "coc:zone_archaeologist", "Explore the town ruins 15 times.", "", false),

            new("Zones", "Farmer", "coc:zone_farmer", "Visit Whitney's farm 30 times.", "", false),

            new("Zones", "Sea-Legs", "coc:zone_sea_legs", "Use the lake boat 15 times.", "", false),

            new("Levels", "Level up!", "coc:level_level_up", "Get to level 2.", "", false),

            new("Levels", "Novice", "coc:level_novice", "Get to level 5.", "", false),

            new("Levels", "Apprentice", "coc:level_apprentice", "Get to level 10.", "", false),

            new("Levels", "Journeyman", "coc:level_journeyman", "Get to level 15.", "", false),

            new("Levels", "Expert", "coc:level_expert", "Get to level 20.", "", false),

            new("Levels", "Master", "coc:level_master", "Get to level 30.", "", false),

            new("Levels", "Grandmaster", "coc:level_grandmaster", "Get to level 45.", "", false),

            new("Levels", "Illustrious", "coc:level_illustrious", "Get to level 60.", "", false),

            new("Levels", "Overlord", "coc:level_overlord", "Get to level 90.", "", false),

            new("Levels", "Are you a god?", "coc:level_are_you_a_god", "Get to level 120.", "Get to level 120.", true),

            new("Population", "My First Companion", "coc:population_first", "Have a camp population of 2.", "", false),

            new("Population", "Hamlet", "coc:population_hamlet", "Have a camp population of 5.", "", false),

            new("Population", "Village", "coc:population_village", "Have a camp population of 10.", "", false),

            new("Population", "Town", "coc:population_town", "Have a camp population of 25.", "", false),

            new("Population", "City", "coc:population_city", "Have a camp population of 100.", "", false),

            new("Population", "Metropolis", "coc:population_metropolis", "Have a camp population of 250.", "", false),

            new("Population", "Megalopolis", "coc:population_megalopolis", "Have a camp population of 500.", "", false),

            new("Population", "City-State", "coc:population_city_state", "Have a camp population of 1,000.", "", true),

            new("Population", "Kingdom", "coc:population_kingdom", "Have a camp population of 2,500.", "", true),

            new("Population", "Empire", "coc:population_empire", "Have a camp population of 5,000.", "", true),

            new("Time", "It's been a month", "coc:time_month", "Get to day 30.", "", false),

            new("Time", "Half-year", "coc:time_half_year", "Get to day 180.", "", false),

            new("Time", "Annual", "coc:time_annual", "Get to day 365. (1 year)", "", false),

            new("Time", "Biennial", "coc:time_biennial", "Get to day 730. (2 years)", "", false),

            new("Time", "Triennial", "coc:time_triennial", "Get to day 1,095. (3 years)", "", false),

            new("Time", "In for the long haul", "coc:time_long_haul", "Get to day 1,825. (5 years)", "", false),

            new("Time", "Decade", "coc:time_decade", "Get to day 3,650. (10 years)", "Get to day 3,650. (10 years | Okay, you can stop now.)", true),

            new("Time", "Century", "coc:time_century", "Get to day 36,500. (100 years)", "Get to day 36,500. (100 years | It's time to stop playing. Go outside.)", true),

            new("Time", "Time Traveler", "coc:time_traveler", "Get to day 36,500+ by tampering with save", "", true),

            new("Dungeons", "Delver", "coc:dungeon_delver", "Clear any dungeon.", "", false),

            new("Dungeons", "Delver Apprentice", "coc:dungeon_delver", "Clear 3 dungeons.", "", false),

            new("Dungeons", "Delver Master", "coc:dungeon_delver_master", "Clear every dungeon in the game.", "", false),

            new("Dungeons", "Shut Down Everything", "coc:dungeon_shut_down_everything", "Clear the Factory.", "", false),

            new("Dungeons", "You're in Deep", "coc:dungeon_youre_in_deep", "Fully clear the Deep Cave.", "", false),

            new("Dungeons", "End of Reign", "coc:dungeon_end_of_reign", "Fully clear the Lethice Stronghold.", "", false),

            new("Dungeons", "Friend of the Sand Witches", "coc:dungeon_sand_witch_friend", "Fully clear the Desert Cave.", "", false),

            new("Dungeons", "Fall of the Phoenix", "coc:dungeon_phoenix_fall", "Clear the Tower of the Phoenix.", "", false),

            new("Dungeons", "Accomplice", "coc:dungeon_accomplice", "Watch Helia kill the Harpy Queen.", "", true),

            new("Dungeons", "Extremely Celibate Delver", "coc:dungeon_extremely_chaste_delver", "Complete Phoenix Tower without ever orgasming from the beginning.", "", true),

            new("Dungeons", "A Little Hope", "coc:dungeon_a_little_hope", "Clear the accursed Manor.", "", false),

            new("Dungeons", "We Are the Flame", "coc:dungeon_we_are_the_flame", "Clear Tower of Deception.", "", false),

            new("Dungeons", "A Meeting of the Minds", "coc:dungeon_meeting_of_the_minds", "Climb the Pillar of Apotheosis, and best a wizard in a duel.", "", false),

            new("Fashion", "Wannabe Wizard", "coc:fashion_wannabe_wizard", "Equip wizard robes and magic staff.", "", false),

            new("Fashion", "Cosplayer", "coc:fashion_cosplayer", "Wear 10 different clothings/armors.", "", false),

            new("Fashion", "Dominatrix", "coc:fashion_dominatrix", "Wear any form of kinky clothing and wield any form of whip.", "", false),

            new("Fashion", "Going Commando", "coc:fashion_going_commando", "Wear no undergarments while wearing any clothes or armors.", "", false),

            new("Fashion", "Bling Bling", "coc:fashion_bling_bling", "Wear jewelry that is valued over 1,000 gems.", "", false),

            new("Wealth", "Rich", "coc:wealth_rich", "Have 1,000 gems.", "", false),

            new("Wealth", "Hoarder", "coc:wealth_hoarder", "Have 10,000 gems.", "", false),

            new("Wealth", "Gem Vault", "coc:wealth_gem_vault", "Have 100,000 gems.", "", false),

            new("Wealth", "Millionaire", "coc:wealth_millionaire", "Have 1,000,000 gems.", "Have 1,000,000 gems. What are you going to spend these gems on?", true),

            new("Wealth", "Item Vault", "coc:wealth_item_vault", "Fill up your inventory, chest, jewelry box, dresser, and all racks.", "", false),

            new("Combat", "Wizard", "coc:combat_wizard", "Learn all black and white spells from spell books.", "", false),

            new("Combat", "Cum Cannon", "coc:combat_cum_cannon", "Cum in the middle of battle.", "", false),

            new("Combat", "How Do I Shot Web?", "coc:combat_shot_web", "Fire your webbings at your opponent.", "", false),

            new("Combat", "Pain", "coc:combat_pain", "Deal 50 damage in one hit.", "", false),

            new("Combat", "Fractured Limbs", "coc:combat_fractured_limbs", "Deal 100 damage in one hit.", "", false),

            new("Combat", "Broken Bones", "coc:combat_broken_bones", "Deal 250 damage in one hit.", "", false),

            new("Combat", "Overkill", "coc:combat_overkill", "Deal 500 damage in one hit.", "", false),

            new("Combat", "Damage Sponge", "coc:combat_damage_sponge", "Take a total of 10,000 damage.", "", false),

            new("Combat", "Bloodletter", "coc:combat_blood_letter", "Deal a total of 50,000 damage.", "", false),

            new("Combat", "Revengeance", "coc:combat_revengeance", "Defeat an enemy with a katana counter.", "", false),

            new("Combat", "Night Sun", "coc:night_sun", "Defeat a higher level enemy with Supernova.", "", true),

            new("Seasonal Events", "Egg Hunter", "coc:holiday_egg_hunter", "Find 10 eggs as random drops during Easter event.", "", true),

            new("Seasonal Events", "Happy Birthday, Helia!", "coc:holiday_helia_birthday", "Participate into Helia's birthday event. (August)", "", true),

            new("Seasonal Events", "Thankslutting", "coc:holiday_thanksgiving_i", "Meet the Piggy-Slut (Thanksgiving)", "", true),

            new("Seasonal Events", "Gobble Gobble", "coc:holiday_thanksgiving_ii", "Meet the Cockgobbler (Thanksgiving)", "", true),

            new("Seasonal Events", "Pump-kin-kin-kin", "coc:holiday_halloween_i", "Find the pumpkin (Halloween)", "", true),

            new("Seasonal Events", "Fera's Wonderland", "coc:holiday_halloween_ii", "Free Fera/Visit her wonderland (Halloween)", "", true),

            new("Seasonal Events", "Naughty or Nice", "coc:holiday_christmas_i", "Meet the X-mas Elf (Christmas)", "", true),

            new("Seasonal Events", "A Christmas Carol", "coc:holiday_christmas_ii", "Complete Carol's mini-quest (Christmas)", "", true),

            new("Seasonal Events", "The Lovable Snowman", "coc:holiday_christmas_iii", "Have Nieve as lover (Christmas/Winter)", "", true),

            new("Seasonal Events", "Will You Be My Valentine?", "coc:holiday_valentine", "Visit the Wet Bitch during Valentine's day. (Valentine)", "", true),

            new("Survival/Realistic Mode", "Tastes Like Chicken", "coc:realistic_tastes_like_chicken", "Refill your hunger for the first time.", "", false),

            new("Survival/Realistic Mode", "Champion Needs Food Badly", "coc:realistic_champion_needs_food", "Instantly refill your hunger from 0 to 100 in one go.", "", false),

            new("Survival/Realistic Mode", "Gourmand", "coc:realistic_gourmand", "Refill hunger from 5 different sources.", "", false),

            new("Survival/Realistic Mode", "Glutton", "coc:realistic_glutton", "Eat while hunger is above 90.", "", false),

            new("Survival/Realistic Mode", "Fasting", "coc:realistic_fasting", "Keep hunger below 25 for a week but don't let it reach 0.", "", false),

            new("Challenges", "The Ultimate Noob", "coc:challenge_ultimate_noob", "Defeat Lethice at level 1.", "", false),

            new("Challenges", "The Mundane Champion", "coc:challenge_ultimate_mundane", "Defeat Lethice without having the knowledge of any spells.", "", false),

            new("Challenges", "The Celibate Hero", "coc:challenge_ultimate_celibate", "Finish the main story without ever having sex or masturbating.", "", false),

            new("Challenges", "Pacifist", "coc:challenge_pacifist", "Finish the main storyline without beating and killing anybody.", "Finish the main storyline without beating and killing anybody. Frisk would be so proud of you.", false),

            new("Challenges", "Speedrunner", "coc:challenge_speedrun", "Finish the main story within 30 days or less. Must not be on New Game+.", "", false),

            new("General", "Portal Defender", "coc:general_portal_defender", "Defeat 25 demons and sleep 10 times.", "", false),

            new("General", "Bad Ender", "coc:general_bad_ender", "Cause or witness 3 Bad Ends to various NPCs.", "", false),

            new("General", "Game Over!", "coc:general_game_over", "Get a Bad End.", "", false),

            new("General", "Urine Trouble", "coc:general_urine_trouble", "Urinate at least once in the realm of Mareth.", "", false),

            new("General", "Smashed", "coc:general_smashed", "Get so drunk that you end up urinating.", "", true),

            new("General", "What's Happening to Me?", "coc:general_whats_happening_to_me", "Transform for the first time.", "", false),

            new("General", "Transformer", "coc:general_transformer", "Transform 10 times.", "", false),

            new("General", "Shapeshifty", "coc:general_shapeshifty", "Transform 25 times.", "", false),

            new("General", "Fapfapfap", "coc:general_fapfapfap", "Masturbate for the first time.", "", false),

            new("General", "Faptastic", "coc:general_faptastic", "Masturbate 10 times.", "", false),

            new("General", "Master-bation", "coc:general_fapster", "Masturbate 100 times.", "", false),

            new("General", "Helspawn", "coc:general_helspawn", "Have Helia give birth to Helspawn and raise her until adulthood.", "", false),

            new("General", "Goo Armor", "coc:general_goo_armor", "Wear the goo armor.", "", false),

            new("General", "Urta's True Lover", "coc:general_urta_true_lover", "Complete Urta's infertility quest then have her give birth to a baby fox.", "", true),

            new("General", "Dress-tacular", "coc:general_dresstacular", "Give Rubi every outfit available.", "", false),

            new("General", "Godslayer", "coc:general_godslayer", "Defeat corrupted Marae.", "", true),

            new("General", "Follow the Leader", "coc:general_follow_the_leader", "Get every follower in the game.", "", false),

            new("General", "Gotta Love 'Em All", "coc:general_gotta_love_them_all", "Get every lover in the game. (Nieve optional)", "", false),

            new("General", "Meet Your {0}", "coc:general_meet_your_master", "Get every slave in the game. (Corrupt Jojo & Amily and Bimbo Sophie optional.)", "", false),

            new("General", "Slaver", "coc:general_meet_your_master_true", "Get every slave in the game, including corrupt Jojo and Amily, and Bimbo Sophie.", "", true),

            new("General", "All Your People are Belong to Me", "coc:general_all_ur_pplz_r_blng_2_me", "Obtain every follower, lover, and slave. (Excluding mutual exclusivity)", "", false),

            new("General", "Scholar", "coc:general_scholar", "Fill out all codex entries available in the game.", "", false),

            new("General", "Freeloader", "coc:general_freeloader", "Visit the kitsune's mansion 3 times.", "", false),

            new("General", "Schizophrenic", "coc:general_schizo", "Go between pure and corrupt 4 times. (Threshold of 20 and 80 corruption)", "", false),

            new("General", "Clean Slate", "coc:general_clean_slate", "Go from 100 corruption to zero for the first time.", "", false),

            new("General", "Perky", "coc:general_perky", "Have at least 20 perks.", "", false),

            new("General", "Super Perky", "coc:general_super_perky", "Have at least 35 perks.", "", false),

            new("General", "Ultra Perky", "coc:general_ultra_perky", "Have at least 50 perks.", "", false),

            new("General", "Jack of All Trades", "coc:general_stats_50", "Have at least 50 of each stat. (Libido, sensitivity, corruption optional)", "", false),

            new("General", "Incredible Stats", "coc:general_stats_100", "Have at least 100 of each stat. (Libido, sensitivity, corruption optional)", "", false),

            new("General", "Like Chuck Norris", "coc:general_like_chuck_norris", "Defeat the Frost Giant without any equipment.", "Defeat the Frost Giant without any equipment. Way to be a badass!", false),

            new("General", "Tentacle Beast Slayer", "coc:general_tentacle_beast_slayer", "Slay your first tentacle beast.", "", false),

            new("General", "Hammer Time", "coc:general_hammer_time", "Buy a total of 300 nails.", "", false),

            new("General", "Nail Scavenger", "coc:general_nail_scavenger", "Scavenge a total of 200 nails from the library wreckage", "", false),

            new("General", "I'm No Lumberjack", "coc:general_im_no_lumberjack", "Buy a total of 100 wood.", "", false),

            new("General", "Deforester", "coc:general_deforester", "Cut down 100 wood pieces.", "", false),

            new("General", "Yabba Dabba Doo", "coc:general_yabba_dabba_doo", "Buy a total of 100 stones.", "", false),

            new("General", "AntWorks", "coc:general_antworks", "Gather a total of 200 stones with Phylla help.", "", false),

            new("General", "Home Sweet Home", "coc:general_home_sweet_home", "Finish the cabin and complete it with furnishings.", "", false),

            new("General", "Getaway", "coc:general_getaway", "Spend the night outside your camp.", "", false),

            new("General", "My Tent's (not) Better Than Yours", "coc:general_my_tent_not_better", "Sleep in Arian's tent.", "", false),

            new("General", "Divine Intervention", "coc:general_minerva_purification", "Complete Minerva's purification process.", "", true),

            new("General", "Fencer", "coc:general_fencer", "Complete rapier training from Raphael.", "", true),

            new("General", "Now You're Fucking With Portals", "coc:general_fuck_with_portals", "Engage in portal sex with Ceraph.", "", true),

            new("General", "Getting Wood", "coc:general_getting_wood", "Punch a tree until wood falls out... Wait, what?", "", true),

            new("General", "Dick Banisher", "coc:general_dick_banisher", "Remove cocks from at least three dedickable NPCs. Don't you think they'll miss having their own cocks?", "", true),

            new("General", "You Bastard", "coc:general_you_bastard", "Perform something only someone who's evil would do. Like corrupting NPCs or removing dick from at least 7 dedickable NPCs.", "", true),

            new("General", "Up to Eleven", "coc:general_up_to_11", "Take your height up to 11 feet.", "", false),

            new("General", "Off With Her Head!", "coc:general_off_with_her_head", "You've managed to behead Lethice and show her head to the demons!", "", true),

            new("General", "NOOOOOOOOOOOO!", "coc:general_nooooooo", "You've managed to kill yourself before Lethice takes you as her slave.", "", true),

            new("General", "Make Mareth Great Again", "coc:general_make_mareth_great_again", "Build a wall around your camp to defend from those pesky imps.", "", false),

            new("General", "Terracotta Impy", "coc:general_terracotta_impy", "You've placed 100 imp statues around your camp wall. No, bassy. Bad bassy! Baaad Mr. Bassy!!!", "", true),

            new("General", "Kaizo Trap", "coc:general_kaizo_trap", "Fall victim to a Kaizo Trap.", "", true),

            new("General", "The Hunter Becomes the Hunted", "coc:general_hunter_is_hunted", "Turn the tables against the Erlking.", "", false),

            new("General", "Allahu Akbal", "coc:general_allahu_akbal", "Submit to Akbal until you receive the associated perks.", "", false),

            new("General", "Jojo's Bizarre Adventure", "coc:general_jojos_bizarre_adventure", "Have Jojo in your camp, through either methods.", "", false),

            new("General", "Parasite Queen", "coc:general_parasite_queen", "Host a massive amount of Eel Parasites.", "", false),

            new("General", "Nephila Queen", "coc:general_nephila_queen", "Host a massive amount of nephila parasites.", "", true),

            new("General", "Nephila Arch Queen", "coc:general_nephila_arch_queen", "Achieve nephila arch queendom.", "", true),

            new("General", "Lord Shoggoth Postulate", "coc:lord_british_postulate", "You just beat an eldritch abomination, proving that OCA has a very poor grasp of game balancing.", "", true),

            new("General", "Alexander the Great", "coc:alexander_the_great", "Achieve an impetuous style.", "Your style is impetuous!", false),
        ];
    }
}