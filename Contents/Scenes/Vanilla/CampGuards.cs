using System;
using MonoCoC.Contents.Followers;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    public static class CampGuards
    {
        public static bool IsGuard(string follower)
        {
            foreach (var guard in FollowerRegistry.Guards)
                if (string.Equals(guard.Name, follower, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }
    }
}