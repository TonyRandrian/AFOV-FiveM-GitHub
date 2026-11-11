using System;

namespace metabolism.Shared
{
    [Flags]
    public enum PlayerActivity
    {
        None = 0,
        Underwater = 1,
        Sprinting = 2
    }
}