using System.Collections.Generic;

namespace MonoCoC.Contents.Tags
{
    public interface ITaggable
    {
        IReadOnlyList<string> Tags { get; }
    }
}