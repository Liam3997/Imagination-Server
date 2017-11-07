// Respond to requests to delete characters

using System;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;
using ImaginationServer.World.Packets.Char;
using ImaginationServer.SQL_DB;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientCharacterDeleteRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            using (var database = new DbUtils())
            {
                if (!client.Authenticated) return;

                CharacterDeleteRequest request = new CharacterDeleteRequest(reader);

                var character = database.GetCharacter(request.ObjectId); // Retrieve the character from the database

                Console.WriteLine($"{client.Username} requested to delete their character {character.Name}.");

                CharacterDeleteResponse response;
                if (!string.Equals(character.Owner, client.Username,
                    StringComparison.CurrentCultureIgnoreCase)) // You can't delete someone else's character!
                {
                    response = new CharacterDeleteResponse(0x02); // Maybe that's the fail code?
                    Console.WriteLine("Failed: Can't delete someone else's character!");
                }
                else // Good to go, that's their character, they can delete it if they want.
                {
                    database.DeleteCharacter(character); // Remove the character from the Redis database
                    response = new CharacterDeleteResponse(0x01); // Success code
                    Console.WriteLine("Successfully deleted character.");
                }

                // Send the packet
                response.Send(client.Address);
            }
        }
    }
}