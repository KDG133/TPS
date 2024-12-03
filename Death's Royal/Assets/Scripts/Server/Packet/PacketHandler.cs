using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler
{
	public static void S_ChatHandler(PacketSession session, IMessage packet)
	{
		S_Chat chatPacket = packet as S_Chat;
		ServerSession serverSession = session as ServerSession;

		//Debug.Log(chatPacket.Context);
	}

	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
		S_EnterGame enterGamePacket = packet as S_EnterGame;
        Managers.Object.Add(enterGamePacket.Player, myPlayer : true);
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leaveGamePacket = packet as S_LeaveGame;
        Managers.Object.RemoveMyPlayer();
    }

    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;

        foreach(PlayerInfo player in spawnPacket.Players)
        {
            Managers.Object.Add(player, myPlayer: false);
        }
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawnPacket = packet as S_Despawn;

        foreach (int id in despawnPacket.PlayerIds)
        {
            Managers.Object.Remove(id);
        }
    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move movePacket = packet as S_Move;
        ServerSession serverSession = session as ServerSession;

        GameObject go = Managers.Object.FindById(movePacket.PlayerID);
        if (go == null)
            return;

        ThirdPersonController tpc = go.GetComponent<ThirdPersonController>();
        if (tpc == null)
            return;

        tpc.playerSpeed = movePacket.PosInfo.MoveSpeed;
        tpc.transform.position = new Vector3(movePacket.PosInfo.Pos.X,
            movePacket.PosInfo.Pos.Y, movePacket.PosInfo.Pos.Z);
        tpc.transform.rotation = Quaternion.Euler(new Vector3(0f, movePacket.PosInfo.MoveDir, 0f));
    }
}
