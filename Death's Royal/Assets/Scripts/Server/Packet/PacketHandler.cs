using DummyClient;
using ServerCore;
using UnityEngine;

class PacketHandler
{
    public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEnterGame Packet = packet as S_BroadcastEnterGame;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.EnterGame(Packet);
    }

    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastLeaveGame Packet = packet as S_BroadcastLeaveGame;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.LeaveGame(Packet);
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        S_PlayerList Packet = packet as S_PlayerList;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Add(Packet);
    }

    public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMove Packet = packet as S_BroadcastMove;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Move(Packet);
    }
}