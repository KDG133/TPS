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

        Console.WriteLine($"C_Move ({movePacket.PosInfo.Pos.X}, {movePacket.PosInfo.Pos.Y}, {movePacket.PosInfo.Pos.Z})");

        if (clientSession.MyPlayer == null)
            return;
        if(clientSession.MyPlayer.Room == null) 
            return;

        //TODO : 검증

        //서버에서 좌표 이동
        PlayerInfo info = clientSession.MyPlayer.Info;
        info.PosInfo = movePacket.PosInfo;

        //다른 플레이어한테 알려준다
        S_Move resMovePacket = new S_Move();
        resMovePacket.PlayerID = clientSession.MyPlayer.Info.PlayerID;
        resMovePacket.PosInfo = movePacket.PosInfo;

        clientSession.MyPlayer.Room.Broadcast(resMovePacket);
    }
}
