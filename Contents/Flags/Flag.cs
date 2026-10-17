using MonoCoC.Registry;

namespace MonoCoC.Contents.Flags
{
    public readonly struct Flag<T>(Identifier id)
    {
        public readonly Identifier Id = Register(id);

        public Flag(string path) : this(new Identifier("coc", path))
        {
        }

        public Flag(string space, string path) : this(new Identifier(space, path))
        {
        }

        public static Identifier Register(Identifier identifier)
        {
            GameFlags.Register(identifier, GameFlags.TypeOf<T>());

            return identifier;
        }
    }
}