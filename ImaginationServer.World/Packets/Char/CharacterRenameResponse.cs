using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImaginationServer.Common;
using ImaginationServer.Common.Packets;
using static ImaginationServer.Enums.PacketEnums.WorldServerPacketId;
using ImaginationServer.Enums;

namespace ImaginationServer.World.Packets.Char
{
    public class CharacterRenameResponse : OutgoingPacket
    {
        public byte ResponseCode { get; }

        public CharacterRenameResponse(byte responseCode)
        {
            ResponseCode = responseCode;
        }

        public override void Send(string clientAddress)
        {
            using (var bitStream = new WBitStream()) // Create packet
            {
                // Always write packet header
                bitStream.WriteHeader(PacketEnums.RemoteConnection.Client, (uint)MsgClientCharacterRenameResponse);
                bitStream.Write(ResponseCode);

                // Send the packet
                WorldServer.Server.Send(bitStream, WPacketPriority.SystemPriority,
                    WPacketReliability.ReliableOrdered, 0, clientAddress, false);
            }
        }
    }
}
