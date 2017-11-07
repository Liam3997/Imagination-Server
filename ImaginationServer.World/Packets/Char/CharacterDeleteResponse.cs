using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImaginationServer.Common;
using ImaginationServer.Common.Packets;
using static ImaginationServer.Enums.PacketEnums.WorldServerPacketId;
using static WPacketPriority;
using static WPacketReliability;
using ImaginationServer.Enums;

namespace ImaginationServer.World.Packets.Char
{
    public class CharacterDeleteResponse : OutgoingPacket
    {
        public byte ResponseCode { get; }

        public CharacterDeleteResponse(byte responseCode)
        {
            ResponseCode = responseCode;
        }

        public override void Send(string clientAddress)
        {
            using (var bitStream = new WBitStream()) // Create the new bitstream
            {
                // Always write the packet header!
                bitStream.WriteHeader(PacketEnums.RemoteConnection.Client, (uint)MsgClientDeleteCharacterResponse);
                bitStream.Write(ResponseCode);

                // Send the packet
                WorldServer.Server.Send(bitStream, SystemPriority, ReliableOrdered, 0, clientAddress, false);
            }
        }
    }
}
