start ../../PacketGenerator/bin/Debug/PacketGenerator.exe ../../PacketGenerator/PDL.xml
xcopy /y GenPackets.cs "../../DummyClient/Packet"
xcopy /y GenPackets.cs "../../../Death's Royal/Assets/Scripts/Server/Packet"
xcopy /y GenPackets.cs "../../Server/Packet"
xcopy /y ClientPacketManager.cs "../../DummyClient/Packet"
xcopy /y ClientPacketManager.cs "../../../Death's Royal/Assets/Scripts/Server/Packet"
xcopy /y ServerPacketManager.cs "../../Server/Packet"