using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImaginationServer.Common;
using ImaginationServer.Common.CharacterData;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Packets;

namespace ImaginationServer.World.Packets.Char
{
    public class CharacterCreateRequest : IncomingPacket
    {
        public Character CreatedCharacter { get; }

        public CharacterCreateRequest(BinaryReader binaryReader) : base(binaryReader)
        {
            var unapprovedName = binaryReader.ReadWString(66); // Read the name of the new character
            binaryReader.BaseStream.Position = 74; // Set the position to right after the username
            var ftpName1 = binaryReader.ReadUInt32(); // Read
            var ftpName2 = binaryReader.ReadUInt32(); // FTP
            var ftpName3 = binaryReader.ReadUInt32(); // Names

            // FTP Temp names
            var ftpName = FtpNames.FirstNames[ftpName1] + FtpNames.MiddleNames[ftpName2] +
                          FtpNames.LastNames[ftpName3];

            if (String.IsNullOrWhiteSpace(unapprovedName)) unapprovedName = null;

            // TODO: Have it automatically replace the ftpName with unapprovedName if the server owner wants names to be auto-approved
            var name = ftpName;

            binaryReader.ReadBytes(9); // Read 9 ... unknown bytes?

            var shirtColor = binaryReader.ReadUInt32(); // Read their choices in appearance
            var shirtStyle = binaryReader.ReadUInt32();
            var pantsColor = binaryReader.ReadUInt32();
            var hairStyle = binaryReader.ReadUInt32();
            var hairColor = binaryReader.ReadUInt32();
            var lh = binaryReader.ReadUInt32();
            var rh = binaryReader.ReadUInt32();
            var eyebrows = binaryReader.ReadUInt32();
            var eyes = binaryReader.ReadUInt32();
            var mouth = binaryReader.ReadUInt32();

            // Create the new character
            // Owner needs to be set in the Handler which has the Client
            CreatedCharacter = new Character
            {
                GmLevel = 0,
                Reputation = 100,
                Name = name,
                UnapprovedName = unapprovedName,
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
                LastZoneId = (ushort)ZoneId.VentureExplorer,
                MapInstance = 0,
                MapClone = 0,
                Position = ZonePositions.VentureExplorer,
            };
        }
    }
}
