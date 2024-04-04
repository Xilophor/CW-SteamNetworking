using System;

namespace SteamNetworking.Messaging;

[Flags]
public enum ESendFlags
{
    unreliable = 0,
    noNangle = 1,
    noDelay = 4,
    reliable = 8
}

