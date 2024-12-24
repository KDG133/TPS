using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom
    {
        object _lock = new object();
        public int RoomId { get; set; }

        List<Player> _players = new List<Player>();

        public void EnterGame(Player newPlayer)
        {
            if(newPlayer == null)
                return;

            lock (_lock)
            {             
                _players.Add(newPlayer);
                newPlayer.Room = this;

                //본인에게 정보 전송
                {
                    S_EnterGame enterPacket = new S_EnterGame();
                    enterPacket.Player = newPlayer.Info;
                    newPlayer.Session.Send(enterPacket);

                    S_Spawn spawnPacket = new S_Spawn();
                    foreach (Player p in _players)
                    {
                        if (newPlayer != p)
                            spawnPacket.Players.Add(p.Info);
                    }
                    newPlayer.Session.Send(spawnPacket);
                }

                //타인에게 정보 전송
                {
                    S_Spawn spawnPacket = new S_Spawn();
                    spawnPacket.Players.Add(newPlayer.Info);
                    foreach(Player p in _players)
                    {
                        if (newPlayer != p)
                            p.Session.Send(spawnPacket);
                    }
                }
            }
        }

        public void LeaveGame(int playerId)
        {
            lock (_lock)
            {
                Player player = _players.Find(p => p.Info.PlayerID == playerId);
                if (player == null)
                    return;

                _players.Remove(player);
                player.Room = null;

                //본인에게 정보 전송
                {
                    S_LeaveGame leaveGame = new S_LeaveGame();
                    player.Session.Send(leaveGame);
                }

                //타인에게 정보 전송
                {
                    S_Despawn despawnPacket = new S_Despawn();
                    despawnPacket.PlayerIds.Add(player.Info.PlayerID);
                    foreach (Player p in _players)
                    {
                        if(player != p)
                            p.Session.Send(despawnPacket);
                    }
                }
            }
        }

        #region Handle
        public void HandleMove(Player player, C_Move movePacket)
        {
            if (player == null)
                return;

            lock (_lock)
            {
                //TODO : 검증

                //서버에서 좌표 이동
                PlayerInfo info = player.Info;
                info.PosInfo = movePacket.PosInfo;

                //다른 플레이어한테 알려준다
                S_Move resMovePacket = new S_Move();
                resMovePacket.PlayerID = player.Info.PlayerID;
                resMovePacket.PosInfo = movePacket.PosInfo;

                Broadcast(resMovePacket);
            }
        }

        public void HandleAim(Player player, C_Aim aimPacket)
        {
            if (player == null)
                return;

            lock (_lock)
            {
                S_Aim resAimPacket = new S_Aim();
                resAimPacket.PlayerID = player.Info.PlayerID;
                resAimPacket.IsAim = aimPacket.IsAim;
                resAimPacket.Pos = aimPacket.Pos;

                Broadcast(resAimPacket);
            }
        }

        public void HandleWeaponchange(Player player, C_Weaponchange weaponchangePacket)
        {
            if (player == null)
                return;

            lock (_lock)
            {
                S_Weaponchange redWeaponchangePacket = new S_Weaponchange();
                redWeaponchangePacket.PlayerID = player.Info.PlayerID;
                redWeaponchangePacket.GunType = weaponchangePacket.GunType;

                Broadcast(redWeaponchangePacket);
            }
        }

        public void HandleReload(Player player, C_Reload reloadPacket)
        {
            if (player == null)
                return;

            lock (_lock)
            {
                S_Reload redReloadPacket = new S_Reload();
                redReloadPacket.PlayerID = player.Info.PlayerID;
                redReloadPacket.IsReload = reloadPacket.IsReload;

                Broadcast(redReloadPacket);
            }
        }

        public void HandleShot(Player player, C_Shot shotPacket)
        {
            if (player == null)
                return;

            lock (_lock)
            {
                S_Shot resShotPacket = new S_Shot();
                resShotPacket.PlayerID = player.Info.PlayerID;
                resShotPacket.IsShot = shotPacket.IsShot;

                Broadcast(resShotPacket);
            }
        }
        #endregion

        public void Broadcast(IMessage packet)
        {
            lock ( _lock)
            {
                foreach (Player p in _players)
                {
                    p.Session.Send(packet);
                }
            }
        }
    }
}
