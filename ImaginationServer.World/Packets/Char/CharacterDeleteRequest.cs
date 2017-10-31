using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImaginationServer.Common.Packets;

namespace ImaginationServer.World.Packets.Char
{
    public class CharacterDeleteRequest : IncomingPacket
    {
        public long ObjectId { get; }

        public CharacterDeleteRequest(BinaryReader binaryReader) : base(binaryReader)
        {
            ObjectId = binaryReader.ReadInt64(); // The object id of the character
            Console.WriteLine(ObjectId);
        }
    }
}
