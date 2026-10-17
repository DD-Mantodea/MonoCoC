using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat
{

    public class DamageTypeDefinition : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string NameKey = "";

        public Identifier Handler;
    }
}