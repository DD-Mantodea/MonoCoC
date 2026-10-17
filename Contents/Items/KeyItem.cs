using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Items
{
    public class KeyItem
    {
        public Identifier Id;

        public float[] Values = new float[4];

        public float this[int index]
        {
            get => index >= 0 && index < Values.Length ? Values[index] : 0;

            set
            {
                if (index >= 0 && index < Values.Length)
                    Values[index] = value;
            }
        }

        public string DisplayName => ("KeyItems." + Id.Path).LocalizedOr(Id.Path);
    }
}