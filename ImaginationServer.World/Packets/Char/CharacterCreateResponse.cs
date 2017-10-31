using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Packets;
using static ImaginationServer.Common.PacketEnums.WorldServerPacketId;
using static WPacketPriority;
using static WPacketReliability;

namespace ImaginationServer.World.Packets.Char
{
    public class CharacterCreateResponse : OutgoingPacket
    {
        public Account ClientAccount { get; }
        public byte ResponseId { get; }

        public CharacterCreateResponse(Account clientAccount, byte responseId)
        {
            ClientAccount = clientAccount;
            ResponseId = responseId;
        }

        public override void Send(string clientAddress)
        {
            // Create the response
            using (var bitStream = new WBitStream())
            {
                bitStream.WriteHeader(PacketEnums.RemoteConnection.Client, (uint) MsgClientCharacterCreateResponse);
                // Always write the packet header.
                bitStream.Write(ResponseId); // Write the response code.
                WorldServer.Server.Send(bitStream, SystemPriority,
                    ReliableOrdered, 0, clientAddress, false); // Send the response.
            }

            if (ResponseId == 0x00)
            {
                // Send the updated character list.
                CharacterListResponse characterListResponse = new CharacterListResponse(ClientAccount);
                characterListResponse.Send(clientAddress);
            }
        }
    }
}
