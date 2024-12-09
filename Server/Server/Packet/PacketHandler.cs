using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	public static void C_ChatHandler(PacketSession session, IMessage packet)
	{
		S_Chat chatPacket = packet as S_Chat;
		ClientSession serverSession = session as ClientSession;

		Console.WriteLine(chatPacket.Context);
	}

    public static void C_MoveHandler(PacketSession session, IMessage packet)
    {
        C_Move movePacket = packet as C_Move;
        ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;

        GameRoom room = player.Room;
        if(room == null) 
            return;

        room.HandleMove(player, movePacket);
    }

    public static void C_AimHandler(PacketSession session, IMessage packet)
    {
        C_Aim AimPacket = packet as C_Aim;
        ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;

        GameRoom room = player.Room;
        if (room == null)
            return;

        room.HandleAim(player, AimPacket);
    }

    public static void C_WeaponchangeHandler(PacketSession session, IMessage packet)
    {
        C_Weaponchange WeaponchangePacket = packet as C_Weaponchange;
        ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;

        GameRoom room = player.Room;
        if (room == null)
            return;

        room.HandleWeaponchange(player, WeaponchangePacket);
    }

    public static void C_ReloadHandler(PacketSession session, IMessage packet)
    {
        C_Reload reloadPacket = packet as C_Reload;
        ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;

        GameRoom room = player.Room;
        if (room == null)
            return;

        room.HandleReload(player, reloadPacket);
    }
}
