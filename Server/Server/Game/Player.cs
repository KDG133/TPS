using Google.Protobuf.Protocol;

namespace Server
{
    public class Player
    {
        public PlayerInfo Info { get; set; } = new PlayerInfo() { PosInfo = new PositionInfo() { Pos = new PVector3() } };
        public GameRoom Room { get; set; }
        public ClientSession Session { get; set; }
    }
}