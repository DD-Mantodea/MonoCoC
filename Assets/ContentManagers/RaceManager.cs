using Hjson;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Races;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class RaceManager : ContentManager<Race>
    {
        public static RaceScoreFormula DefaultFormula = PlayerExt.DefaultScore;

        private static readonly Dictionary<Identifier, RaceScoreFormula> Overrides = [];

        private static bool _loaded;

        static RaceManager()
        {
            RegisterScore("coc:human", PlayerExt.HumanScore);

            RegisterScore("coc:demon", PlayerExt.DemonScore);

            RegisterScore("coc:mino", PlayerExt.MinoScore);

            RegisterScore("coc:cow", PlayerExt.CowScore);

            RegisterScore("coc:sand_trap", PlayerExt.SandTrapScore);

            RegisterScore("coc:insect", PlayerExt.InsectScore);

            RegisterScore("coc:bee", PlayerExt.BeeScore);

            RegisterScore("coc:ferret", PlayerExt.FerretScore);

            RegisterScore("coc:wolf", PlayerExt.WolfScore);

            RegisterScore("coc:dog", PlayerExt.DogScore);

            RegisterScore("coc:mouse", PlayerExt.MouseScore);

            RegisterScore("coc:raccoon", PlayerExt.RaccoonScore);

            RegisterScore("coc:fox", PlayerExt.FoxScore);

            RegisterScore("coc:cat", PlayerExt.CatScore);

            RegisterScore("coc:lizard", PlayerExt.LizardScore);

            RegisterScore("coc:spider", PlayerExt.SpiderScore);

            RegisterScore("coc:horse", PlayerExt.HorseScore);

            RegisterScore("coc:kitsune", PlayerExt.KitsuneScore);

            RegisterScore("coc:dragon", PlayerExt.DragonScore);

            RegisterScore("coc:goblin", PlayerExt.GoblinScore);

            RegisterScore("coc:gnoll", PlayerExt.GnollScore);

            RegisterScore("coc:goo", PlayerExt.GooScore);

            RegisterScore("coc:naga", PlayerExt.NagaScore);

            RegisterScore("coc:mermaid", PlayerExt.MermaidScore);

            RegisterScore("coc:bat", PlayerExt.BatScore);

            RegisterScore("coc:bunny", PlayerExt.BunnyScore);

            RegisterScore("coc:harpy", PlayerExt.HarpyScore);

            RegisterScore("coc:kanga", PlayerExt.KangaScore);

            RegisterScore("coc:sheep", PlayerExt.SheepScore);

            RegisterScore("coc:shark", PlayerExt.SharkScore);

            RegisterScore("coc:mutant", PlayerExt.MutantScore);

            RegisterScore("coc:salamander", PlayerExt.SalamanderScore);

            RegisterScore("coc:dryad", PlayerExt.DryadScore);

            RegisterScore("coc:siren", PlayerExt.SirenScore);

            RegisterScore("coc:pig", PlayerExt.PigScore);

            RegisterScore("coc:satyr", PlayerExt.SatyrScore);

            RegisterScore("coc:rhino", PlayerExt.RhinoScore);

            RegisterScore("coc:echidna", PlayerExt.EchidnaScore);

            RegisterScore("coc:deer", PlayerExt.DeerScore);

            RegisterScore("coc:cockatrice", PlayerExt.CockatriceScore);

            RegisterScore("coc:imp", PlayerExt.ImpScore);

            RegisterScore("coc:red_panda", PlayerExt.RedPandaScore);
        }

        public override string Folder => "Data/Races";

        public override string Pattern => "*.hjson";

        public static void RegisterScore(Identifier id, RaceScoreFormula formula)
        {
            Overrides[id] = formula;

            if (_loaded && RaceRegistry.Races.TryGet(id, out var race))
                race.Formula = formula;
        }

        public static void SetDefaultFormula(RaceScoreFormula formula) => DefaultFormula = formula;

        public override void Load()
        {
            base.Load();

            _loaded = true;
        }

        protected override void Store(string key, Race race)
        {
            base.Store(key, race);

            race.Formula = Overrides.TryGetValue(race.Id, out var formula) ? formula : null;
        }

        public override void FromText(string path, string text, IDictionary<string, Race> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            var id = root.Str("id");

            if (!Identifier.TryParse(id, out var identifier))
                throw new FormatException($"Invalid race id '{id}' in '{path}'");

            entries[identifier.ToString()] = new Race
            {
                Id = identifier,

                NameKey = root.Str("nameKey")
            };
        }
    }
}