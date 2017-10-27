using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImaginationServer.Common;

namespace ImaginationServer.Auth.Packets.Auth
{
    public class LoginResponse
    {
        // Get these in constructor
        public byte SuccessCode { get; } // This is the Connection ID
        public string UserKey { get; } // Generated User Key

        // These are all from previous LUNI C++ data
        public string TalkLikeAPirate { get; } = "Talk_Like_A_Pirate"; // Always is "Talk_Like_A_Pirate", yet 33 bytes long
        public string UnknownString { get; } = ""; // Have NO idea what this is... Is it relevant?
        // TODO: Does sending proper ClientVersion matter eventually...?
        public ushort ClientVersion1 { get; } = 1; // For some reason, the client version is split up over 3 unsigned shorts
        public ushort ClientVersion2 { get; } = 10;
        public ushort ClientVersion3 { get; } = 64;
        public string Unknown { get; } = "_";
        // TODO: These IPs probably need to be configurable eventually
        public string RedirectIp { get; } = "localhost"; // Redirect IP Address
        public string ChatIp { get; } = "localhost"; // Chat IP Address
        public ushort RedirectPort { get; } = 2006; // Redirect Port
        public ushort ChatPort { get; } = 2003; // Chat Port
        public string AnotherIp { get; } = "localhost"; // Another IP Address? 33 bytes long
        // Does this Guid ever have to change? Could it serve a purpose?
        public string PossibleGuid = "00000000-0000-0000-0000-000000000000"; // In the form of a UUID (maybe a GUID?)
        public ushort ZeroShort { get; } = 0; // Always 0 for some reason...
        // Does Localization ever matter? Might need to be configurable eventually to return localized text...
        public byte[] Localization { get; } = { 0x02, 0x00, 0x55, 0x53, 0x00 };
        //                                       ?    Empty   U     S   Empty
        public byte FirstLoginSubscription { get; } = 0; // Check - Is this the first login after subscribing? (Probably obsolete)
        public byte Subscribed { get; } = 0; // 0 if subscribed, 1 if not subscribed (Probably can always be 0)
        public ulong ZeroLong { get; } = 0; // This is all zeros...
        // TODO: Custom errors eventually?
        public ushort ErrorMsgLength { get; } = 0; // This is the length of the error msg
        public string ErrorMsg { get; } = "T"; // This is the error message displayed if connection ID is 0x05. Can be X bytes long (I believe)
        public ushort ExtraBytesLength { get; } = 324; // This is the number of bytes left (number of each chunk of extra data = 16 bytes * x chunks + these 4 bytes)

        public LoginResponse(byte success, string userKey)
        {
            SuccessCode = success;
            UserKey = userKey;
        }

        public void Send(string clientAddress)
        {
            using (var bitStream = new WBitStream())
            {
                bitStream.WriteHeader(PacketEnums.RemoteConnection.Client, (uint) PacketEnums.WorldServerPacketId.MsgClientLoginResponse);

                bitStream.Write(SuccessCode);
                bitStream.WriteString(TalkLikeAPirate, 33);
                for (var i = 0; i < 7; i++)
                {
                    bitStream.WriteString(Unknown, 0, 33);
                }

                // Client Version
                bitStream.Write(ClientVersion1);
                bitStream.Write(ClientVersion2);
                bitStream.Write(ClientVersion3);

                bitStream.WriteString(UserKey, 66, 66);
                bitStream.WriteString(RedirectIp, 33);
                bitStream.WriteString(ChatIp, 33);
                bitStream.Write(RedirectPort);
                bitStream.Write(ChatPort);
                bitStream.WriteString(AnotherIp, 33);
                bitStream.WriteString(PossibleGuid, 37);
                bitStream.Write(ZeroShort);

                // Localization
                for (var i = 0; i < 5; i++)
                {
                    bitStream.Write(Localization[i]);
                }

                bitStream.Write(FirstLoginSubscription);
                bitStream.Write(Subscribed);
                bitStream.Write(ZeroLong);
                bitStream.Write(ErrorMsgLength);
                // TODO: Probably need to write the proper length of chars here eventually
                bitStream.WriteString(ErrorMsg, 0, 0);

                bitStream.Write(ExtraBytesLength);
                CreateExtraPacketData(0, 0, 2803442767, bitStream);
                CreateExtraPacketData(7, 37381, 2803442767, bitStream);
                CreateExtraPacketData(8, 6, 2803442767, bitStream);
                CreateExtraPacketData(9, 0, 2803442767, bitStream);
                CreateExtraPacketData(10, 0, 2803442767, bitStream);
                CreateExtraPacketData(11, 1, 2803442767, bitStream);
                CreateExtraPacketData(14, 1, 2803442767, bitStream);
                CreateExtraPacketData(15, 0, 2803442767, bitStream);
                CreateExtraPacketData(17, 1, 2803442767, bitStream);
                CreateExtraPacketData(5, 0, 2803442767, bitStream);
                CreateExtraPacketData(6, 1, 2803442767, bitStream);
                CreateExtraPacketData(20, 1, 2803442767, bitStream);
                CreateExtraPacketData(19, 30854, 2803442767, bitStream);
                CreateExtraPacketData(21, 0, 2803442767, bitStream);
                CreateExtraPacketData(22, 0, 2803442767, bitStream);
                CreateExtraPacketData(23, 4114, 2803442767, bitStream);
                CreateExtraPacketData(27, 4114, 2803442767, bitStream);
                CreateExtraPacketData(28, 1, 2803442767, bitStream);
                CreateExtraPacketData(29, 0, 2803442767, bitStream);
                CreateExtraPacketData(30, 30854, 2803442767, bitStream);

                File.WriteAllBytes("Temp/loginresponse.bin", bitStream.GetBytes());

                AuthServer.Server.Send(bitStream, WPacketPriority.SystemPriority,
                    WPacketReliability.ReliableOrdered, 0, clientAddress, false); // Send the packet.
            }
        }

        private static void CreateExtraPacketData(uint stampId, int bracketNum, uint afterNum, WBitStream bitStream)
        {
            const uint zeroPacket = 0;

            bitStream.Write(stampId);
            bitStream.Write(bracketNum);
            bitStream.Write(afterNum);
            bitStream.Write(zeroPacket);
        }
    }
}
