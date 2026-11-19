using System;

namespace metabolism.Shared
{
    [Flags]
    public enum ActionRestriction
    {
        NONE = 0,
        NO_SPRINT = 1 << 0,
        NO_WALK = 1 << 1,
        NO_JUMP = 1 << 2,
        NO_AIM = 1 << 3,
        NO_FIRE = 1 << 4,
        NO_MELEE = 1 << 5,
        SLOW_MOVEMENT = 1 << 6,
        BLURRED_VISION = 1 << 7,
        CAMERA_SHAKE = 1 << 8
    }
}