using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Creatures;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Races
{
    public static class RaceRegistry
    {
        public static Registry<Race> Races => ManagerInstance<RaceManager>.Instance.Registry;

        public static void Register(Race race) => Races.Register(race.Id, () => race);

        public static float Score(Identifier id, Player player) =>

            Races.TryGet(id, out var race) ? race.Score(player) : 0f;

        public static float Score(string path, Player player) => Score(new Identifier("coc", path), player);
    }
}