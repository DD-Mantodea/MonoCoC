using MonoCoC.Registry;

namespace MonoCoC.Contents.Achievements
{

    public class Achievement : IIdentifiable
    {
        public Identifier Id { get; set; } = new("coc", "none");

        public string TitleKey = "";

        public string DescriptionKey = "";
    }
}