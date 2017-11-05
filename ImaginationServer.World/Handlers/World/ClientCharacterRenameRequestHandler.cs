using System;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;
using ImaginationServer.World.Packets.Char;
using static ImaginationServer.Enums.PacketEnums;
using static ImaginationServer.Enums.PacketEnums.WorldServerPacketId;
using ImaginationServer.SQL_DB;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientCharacterRenameRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            using (var database = new DbUtils())
            {
                // Read packet
                CharacterRenameRequest request = new CharacterRenameRequest(reader);

                // Gather info
                var account = database.GetAccount(client.Username);
                var character = database.GetCharacter(request.ObjectId, true);

                Console.WriteLine(
                    $"Got character rename request from {client.Username}. Old name: {character.Name}. New name: {request.NewName}");

                CharacterRenameResponse response;
                // Make sure they own the account
                if (
                    !string.Equals(account.Username, character.Name,
                        StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("Failed to rename character: You can't rename someone else!");
                    response = new CharacterRenameResponse(0x01); // Fail code
                }
                else if (database.CharacterExists(request.NewName)) // Make sure nobody already has that name
                {
                    response = new CharacterRenameResponse(0x03); // Code for username taken
                    Console.WriteLine("Failed to rename character: Name already taken.");
                }
                else // Good to go!
                {
                    try
                    {
                        // TODO: Should this set UnapprovedName instead and mark the name as unapproved?
                        character.Name = request.NewName; // Set their new name
                        database.UpdateCharacter(character); // Update the character
                        response = new CharacterRenameResponse(0x00); // Success code, everything worked just fine.
                        Console.WriteLine("Successfully renamed character!");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"Error while trying to rename user - {exception}");
                        response = new CharacterRenameResponse(0x01); // Some error?
                    }

                    // Send the packet
                    response.Send(client.Address);
                }
            }
        }
    }
}