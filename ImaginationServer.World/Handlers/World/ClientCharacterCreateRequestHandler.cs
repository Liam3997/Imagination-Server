using System;
using System.Collections.Generic;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.CharacterData;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;
using ImaginationServer.World.Packets.Char;
using ImaginationServer.SQL_DB;

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

                var request = new CharacterCreateRequest(reader);
                request.CreatedCharacter.Owner = client.Username;

                var responseId =
                    (byte) (database.CharacterExists(request.CreatedCharacter.Name) ? 0x04 : 0x00);
                // Generate the respond ID

                var account = database.GetAccount(client.Username);

                if (account.Characters.Count >= 4) // Don't want any cheaters getting more than 4!
                {
                    responseId = 0x04;
                }

                if (responseId == 0x00) // Make sure to actually make it, if the character does not exist.
                {
                    // Get the new character
                    var character = request.CreatedCharacter;

                    // Initial Shirt/Pants Items
                    character.Items.Add(
                        new BackpackItem
                        {
                            Lot = FindCharShirtId(character.ShirtColor, character.ShirtStyle),
                            Linked = true,
                            Count = 1,
                            Slot = 0
                        });
                    character.Items.Add(
                        new BackpackItem
                        {
                            Lot = FindCharPantsId(character.PantsColor),
                            Linked = true,
                            Count = 1,
                            Slot = 1
                        });

                    database.AddCharacter(character); // Add the character to the database.

                    account.Characters.Add(character.Name); // Add the character to the account
                    database.UpdateAccount(account); // Update the account
                }

                // Output the code
                Console.WriteLine($"Got character create request from {client.Username}. Response Code: {responseId}");

                // Create the response
                CharacterCreateResponse characterCreateResponse = new CharacterCreateResponse(database.GetAccount(client.Username), responseId);
                characterCreateResponse.Send(client.Address);
            }
        }

        // Struct that puts names to each pants ID (for easy use)
        private enum CharCreatePantsColor : uint
        {
            PantsBrightRed = 2508,
            PantsBrightOrange = 2509,
            PantsBrickYellow = 2511,
            PantsMediumBlue = 2513,
            PantsSandGreen = 2514,
            PantsDarkGreen = 2515,
            PantsEarthGreen = 2516,
            PantsEarthBlue = 2517,
            PantsBrightBlue = 2519,
            PantsSandBlue = 2520,
            PantsDarkStoneGray = 2521,
            PantsMediumStoneGray = 2522,
            PantsWhite = 0x9DB,
            PantsBlack = 2524,
            PantsReddishBrown = 2526,
            PantsDarkRed = 2527
        }

        // Struct that puts names to each base shirt ID (for easy use)
        private enum CharCreateShirtColor : uint
        {
            ShirtBrightRed = 4049,
            ShirtBrightBlue = 4083,
            // TODO: Does this need to be used? It didn't get used in LUNI...
            ShirtBrightYellow = 4117,
            ShirtDarkGreen = 4151,
            ShirtBrightOrange = 4185,
            ShirtBlack = 4219,
            ShirtDarkStoneGray = 4253,
            ShirtMediumStoneGray = 4287,
            ShirtReddishBrown = 4321,
            ShirtWhite = 4355,
            ShirtMediumBlue = 4389,
            ShirtDarkRed = 4423,
            ShirtEarthBlue = 4457,
            ShirtEarthGreen = 4491,
            ShirtBrickYellow = 4525,
            ShirtSandBlue = 4559,
            ShirtSandGreen = 4593
        }

        private uint FindCharShirtId(uint shirtColor, uint shirtStyle)
        {
            uint shirtId = 0;

            // This is a switch case to determine the base shirt color (IDs from CDClient.xml)
            switch (shirtColor)
            {
                case 0:
                {
                    shirtId = shirtStyle >= 35 ? 5730 : (uint)CharCreateShirtColor.ShirtBrightRed;
                    break;
                }

                case 1:
                {
                    shirtId = shirtStyle >= 35 ? 5736 : (uint)CharCreateShirtColor.ShirtBrightBlue;
                    break;
                }

                case 3:
                {
                    shirtId = shirtStyle >= 35 ? 5808 : (uint)CharCreateShirtColor.ShirtDarkGreen;
                    break;
                }

                case 5:
                {
                    shirtId = shirtStyle >= 35 ? 5754 : (uint)CharCreateShirtColor.ShirtBrightOrange;
                    break;
                }

                case 6:
                {
                    shirtId = shirtStyle >= 35 ? 5760 : (uint)CharCreateShirtColor.ShirtBlack;
                    break;
                }

                case 7:
                {
                    shirtId = shirtStyle >= 35 ? 5766 : (uint)CharCreateShirtColor.ShirtDarkStoneGray;
                    break;
                }

                case 8:
                {
                    shirtId = shirtStyle >= 35 ? 5772 : (uint)CharCreateShirtColor.ShirtMediumStoneGray;
                    break;
                }

                case 9:
                {
                    shirtId = shirtStyle >= 35 ? 5778 : (uint)CharCreateShirtColor.ShirtReddishBrown;
                    break;
                }

                case 10:
                {
                    shirtId = shirtStyle >= 35 ? 5784 : (uint)CharCreateShirtColor.ShirtWhite;
                    break;
                }

                case 11:
                {
                    shirtId = shirtStyle >= 35 ? 5802 : (uint)CharCreateShirtColor.ShirtMediumBlue;
                    break;
                }

                case 13:
                {
                    shirtId = shirtStyle >= 35 ? 5796 : (uint)CharCreateShirtColor.ShirtDarkRed;
                    break;
                }

                case 14:
                {
                    shirtId = shirtStyle >= 35 ? 5802 : (uint)CharCreateShirtColor.ShirtEarthBlue;
                    break;
                }

                case 15:
                {
                    shirtId = shirtStyle >= 35 ? 5808 : (uint)CharCreateShirtColor.ShirtEarthGreen;
                    break;
                }

                case 16:
                {
                    shirtId = shirtStyle >= 35 ? 5814 : (uint)CharCreateShirtColor.ShirtBrickYellow;
                    break;
                }

                case 84:
                {
                    shirtId = shirtStyle >= 35 ? 5820 : (uint)CharCreateShirtColor.ShirtSandBlue;
                    break;
                }

                case 96:
                {
                    shirtId = shirtStyle >= 35 ? 5826 : (uint)CharCreateShirtColor.ShirtSandGreen;
                    shirtColor = 16;
                    break;
                }
            }

            // Initialize another variable for the shirt color
            uint editedShirtColor = shirtId;

            // This will be the final shirt ID
            uint shirtIdFinal;

            // For some reason, if the shirt color is 35 - 40,
            // The ID is different than the original... Was this because
            // these shirts were added later?
            if (shirtStyle >= 35)
            {
                shirtIdFinal = editedShirtColor += shirtStyle - 35;
            }
            else
            {
                // Get the final ID of the shirt by adding the shirt
                // style to the editedShirtColor
                shirtIdFinal = editedShirtColor += shirtStyle - 1;
            }

            return shirtIdFinal;
        }

        private uint FindCharPantsId(uint pantsColor)
        {
            uint pantsId = 2508;

            // This is a switch case to determine 
            // the pants color (IDs from CDClient.xml)
            switch (pantsColor)
            {
                case 0:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsBrightRed;
                    break;
                }

                case 1:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsBrightBlue;
                    break;
                }

                case 3:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsDarkGreen;
                    break;
                }

                case 5:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsBrightOrange;
                    break;
                }

                case 6:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsBlack;
                    break;
                }

                case 7:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsDarkStoneGray;
                    break;
                }

                case 8:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsMediumStoneGray;
                    break;
                }

                case 9:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsReddishBrown;
                    break;
                }

                case 10:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsWhite;
                    break;
                }

                case 11:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsMediumBlue;
                    break;
                }

                case 13:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsDarkRed;
                    break;
                }

                case 14:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsEarthBlue;
                    break;
                }

                case 15:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsEarthGreen;
                    break;
                }

                case 16:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsBrickYellow;
                    break;
                }

                case 84:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsSandBlue;
                    break;
                }

                case 96:
                {
                    pantsId = (uint)CharCreatePantsColor.PantsSandGreen;
                    break;
                }
            }

            return pantsId;
        }
    }
}