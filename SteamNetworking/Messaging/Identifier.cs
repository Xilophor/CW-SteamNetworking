using System.Reflection;
using Sirenix.Serialization;

namespace SteamNetworking.Messaging;

public readonly struct RpcIdentifier(uint? gameObjectId, ushort? networkBehaviourId, MethodInfo rpcMethod)
    : IIdentifier, IBehaviourIdentifier
{
    [OdinSerialize] public readonly uint? gameObjectId = gameObjectId;
    [OdinSerialize] public readonly ushort? networkBehaviourId = networkBehaviourId;
    [OdinSerialize] public readonly MethodInfo rpcMethod = rpcMethod;
}

public interface IBehaviourIdentifier
{
}

public interface IIdentifier
{
    public static readonly string? Identifier;
}
