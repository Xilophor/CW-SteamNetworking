using System;
using System.Runtime.InteropServices;
using Steamworks;
using UnityEngine;

namespace SteamNetworking.Messaging;

internal class SteamMessageHandler : MonoBehaviour
{
    private readonly IntPtr m_sendBuffer = Marshal.AllocHGlobal(ushort.MaxValue);
    private readonly IntPtr[] m_receiveBuffers = new IntPtr[16];

    public void SendMessage(byte[] dataStream, ref SteamNetworkingIdentity identity, int channel)
    {
        var buffer = this.CreateBuffer(dataStream);
        var result = SteamNetworkingMessages.SendMessageToUser(ref identity, buffer, (uint)dataStream.Length,
            (int)ESendFlags.reliable, channel);

        if (result == EResult.k_EResultOK) return;
        SteamNetworking.Logger.LogError($"Failed to send message to user {identity.GetSteamID64()}!");
    }

    public void ReceiveMessage(ref SteamNetworkingIdentity identity, int channel)
    {
        int messageCount = SteamNetworkingMessages.ReceiveMessagesOnChannel(channel, this.m_receiveBuffers,
                this.m_receiveBuffers.Length);

        for (int i = 0; i < messageCount; i++)
        {
            try
            {
                SteamNetworkingMessage_t structure = Marshal.PtrToStructure<SteamNetworkingMessage_t>(this.m_receiveBuffers[i]);
                byte[] dataStream = new byte[structure.m_cbSize];
                Marshal.Copy(structure.m_pData, dataStream, 0, dataStream.Length);
                this.OnMessageReceived(dataStream);
            }
            finally
            {
                Marshal.DestroyStructure<SteamNetworkingMessage_t>(this.m_receiveBuffers[i]);
            }
        }
    }

    private void SendMessage<T>(byte[] dataStream)
    {

    }

    private void OnMessageReceived(byte[] dataStream)
    {

    }

    private IntPtr CreateBuffer(byte[] dataStream)
    {
        try
        {
            Marshal.Copy(dataStream, 0, this.m_sendBuffer, dataStream.Length);
            return this.m_sendBuffer;
        }
        catch
        {
            Marshal.FreeHGlobal(this.m_sendBuffer);
        }
        return IntPtr.Zero;
    }
}
