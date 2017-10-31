using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImaginationServer.Common;
using ImaginationServer.Common.Packets;

namespace ImaginationServer.World.Packets.Char
{
    public class CharacterRenameRequest : IncomingPacket
    {
        public long ObjectId { get; }
        public string NewName { get; }

        public CharacterRenameRequest(BinaryReader binaryReader) : base(binaryReader)
        {
            // Read packet
            ObjectId = binaryReader.ReadInt64();
            NewName = binaryReader.ReadWString(66);
        }
    }
}
