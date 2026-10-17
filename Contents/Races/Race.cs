using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Creatures;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Races
{
    public delegate float RaceScoreFormula(Player player, Race race);

    public class Race : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string NameKey = "";

        public RaceScoreFormula? Formula;

        public float Score(Player player) => (Formula ?? RaceManager.DefaultFormula)(player, this);
    }
}