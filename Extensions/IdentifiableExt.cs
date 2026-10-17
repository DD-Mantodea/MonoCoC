using MonoCoC.Registry;

namespace MonoCoC.Extensions
{
    public static class IdentifiableExt
    {

        extension(IIdentifiable self)
        {
            public bool Is(Identifier id) => self.Id == id;
        }
    }
}
