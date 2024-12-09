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

        if (clientSession.MyPlayer == null)
            return;
        if(clientSession.MyPlayer.Room == null) 
            return;

        //TODO : 검증

        //서버에서 좌표 이동
        //다른 플레이어한테 알려준다
        S_Move resMovePacket = new S_Move();
        resMovePacket.PlayerID = clientSession.MyPlayer.Info.PlayerID;
        resMovePacket.PosInfo = movePacket.PosInfo;

        clientSession.MyPlayer.Room.Broadcast(resMovePacket);
    }

    public static void C_AimHandler(PacketSession session, IMessage packet)
    {
        C_Aim AimPacket = packet as C_Aim;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.MyPlayer == null)
            return;
        if (clientSession.MyPlayer.Room == null)
            return;

        S_Aim resAimPacket = new S_Aim();
        resAimPacket.PlayerID = clientSession.MyPlayer.Info.PlayerID;
        resAimPacket.IsAim = AimPacket.IsAim;
        resAimPacket.Pos = AimPacket.Pos;

        clientSession.MyPlayer.Room.Broadcast(resAimPacket);
    }

    public static void C_WeaponchangeHandler(PacketSession session, IMessage packet)
    {
        C_Weaponchange WeaponchangePacket = packet as C_Weaponchange;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.MyPlayer == null)
            return;
        if (clientSession.MyPlayer.Room == null)
            return;

        S_Weaponchange redWeaponchangePacket = new S_Weaponchange();
        redWeaponchangePacket.PlayerID = clientSession.MyPlayer.Info.PlayerID;
        redWeaponchangePacket.GunType = WeaponchangePacket.GunType;

        clientSession.MyPlayer.Room.Broadcast(redWeaponchangePacket);
    }

    public static void C_ReloadHandler(PacketSession session, IMessage packet)
    {
        C_Reload reloadPacket = packet as C_Reload;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.MyPlayer == null)
            return;
        if (clientSession.MyPlayer.Room == null)
            return;

        S_Reload redReloadPacket = new S_Reload();
        redReloadPacket.PlayerID = clientSession.MyPlayer.Info.PlayerID;
        redReloadPacket.IsReload = reloadPacket.IsReload;

        clientSession.MyPlayer.Room.Broadcast(redReloadPacket);
    }
}
