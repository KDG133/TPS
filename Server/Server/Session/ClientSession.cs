using ServerCore;
using System.Net;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;

namespace Server
{
    public class ClientSession : PacketSession
	{
		public Player MyPlayer { get; set; }
		public int SessionId { get; set; }

		public void Send(IMessage packet)
		{
			string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
            MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);
			
            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

            Send(new ArraySegment<byte>(sendBuffer));
        }

		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}");

			// PROTO Test
			MyPlayer = PlayerManager.Instance.Add();
			{
				MyPlayer.Info.Name = $"Player_{MyPlayer.Info.PlayerID}";
				MyPlayer.Info.IsAim = false;
                MyPlayer.Info.PosInfo.Pos.X = -41.0f;
                MyPlayer.Info.PosInfo.Pos.Y = 0.0f;
                MyPlayer.Info.PosInfo.Pos.Z = 28.0f;
				MyPlayer.Info.PosInfo.MoveDir = -90.0f;
				MyPlayer.Info.PosInfo.MoveSpeed = 0f;
                Console.WriteLine($"PlayerID : {MyPlayer.Info.PlayerID}");

                MyPlayer.Session = this;
            }

			RoomManager.Instance.Find(1).EnterGame(MyPlayer);
        }

		public override void OnRecvPacket(ArraySegment<byte> buffer)
		{
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
            RoomManager.Instance.Find(1).LeaveGame(MyPlayer.Info.PlayerID);

            SessionManager.Instance.Remove(this);

			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes)
		{
			//Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}
