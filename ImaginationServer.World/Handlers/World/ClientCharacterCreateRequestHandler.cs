using System;
using System.Collections.Generic;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.CharacterData;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;
using ImaginationServerWorldPackets;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.WorldServerPacketId;
using static WPacketPriority;
using static WPacketReliability;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientCharacterCreateRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            using (var database = new DbUtils())
            {
                if (!client.Authenticated)
                    return; // You need to have an account and be signed into it to make a character!

                var name = reader.ReadWString(66); // Read the name of the new character
                reader.BaseStream.Position = 74; // Set the position to right after the username
                var ftpName1 = reader.ReadUInt32(); // Read
                var ftpName2 = reader.ReadUInt32(); // FTP
                var ftpName3 = reader.ReadUInt32(); // Names

                // FTP Temp names
                var ftpName = FtpNames.FirstNames[ftpName1] + FtpNames.MiddleNames[ftpName2] +
                              FtpNames.LastNames[ftpName3];

                // TODO: Have it automatically delete the ftpName if the server owner wants names to be auto-approved

                reader.ReadBytes(9); // Read 9 ... unknown bytes?

                var shirtColor = reader.ReadUInt32(); // Read their choices in appearance
                var shirtStyle = reader.ReadUInt32();
                var pantsColor = reader.ReadUInt32();
                var hairStyle = reader.ReadUInt32();
                var hairColor = reader.ReadUInt32();
                var lh = reader.ReadUInt32();
                var rh = reader.ReadUInt32();
                var eyebrows = reader.ReadUInt32();
                var eyes = reader.ReadUInt32();
                var mouth = reader.ReadUInt32();

                var responseId =
                    (byte) (database.CharacterExists(name) ? 0x04 : 0x00);
                // Generate the respond ID

                var account = database.GetAccount(client.Username);

                if (account.Characters.Count >= 4) // Don't want any cheaters getting more than 4!
                {
                    responseId = 0x04;
                }

                if (responseId == 0x00) // Make sure to actually make it, if the character does not exist.
                {
                    // Create the new character
                    var character = new Character
                    {
                        Owner = client.Username,
                        GmLevel = 0,
                        Reputation = 100,
                        Name = name,
                        FtpName = ftpName,
                        NameRejected = false,
                        FreeToPlay = false,
                        ShirtColor = shirtColor,
                        ShirtStyle = shirtStyle,
                        PantsColor = pantsColor,
                        HairStyle = hairStyle,
                        HairColor = hairColor,
                        Lh = lh,
                        Rh = rh,
                        Eyebrows = eyebrows,
                        Eyes = eyes,
                        Mouth = mouth,
                        // Initialize the other character data
                        Items = new List<BackpackItem>(),
                        Level = 0,
                        Missions = new List<string>(),
                        LastZoneId = (ushort) ZoneId.VentureExplorer,
                        MapInstance = 0,
                        MapClone = 0,
                        Position = ZonePositions.VentureExplorer,
                    };

                    // TODO: Initial items? Pants?
                    character.Items.Add(
                        new BackpackItem
                        {
                            Lot = WorldPackets.FindCharShirtID(shirtColor, shirtStyle),
                            Linked = false,
                            Count = 1,
                            Slot = 0
                        });
                    //character.AddItem(WorldPackets.);

                    database.AddCharacter(character); // Add the character to the database.

                    account.Characters.Add(character.Name); // Add the character to the account
                    database.UpdateAccount(account); // Update the account
                }

                // Output the code
                Console.WriteLine($"Got character create request from {client.Username}. Response Code: {responseId}");

                // Create the response
                using (var bitStream = new WBitStream())
                {
                    bitStream.WriteHeader(RemoteConnection.Client, (uint) MsgClientCharacterCreateResponse);
                    // Always write the packet header.
                    bitStream.Write((responseId)); // Write the response code.
                    WorldServer.Server.Send(bitStream, SystemPriority,
                        ReliableOrdered, 0, client.Address, false); // Send the response.
                }

                if (responseId == 0x00)
                    WorldPackets.SendCharacterListResponse(client.Address, database.GetAccount(client.Username),
                        WorldServer.Server);
                // Send the updated character list.
            }
        }
    }
}