using System.IO;

namespace ImaginationServer.Common.Packets
{
    public abstract class IncomingPacket
    {
        protected IncomingPacket(BinaryReader binaryReader)
        {
        }
    }
}
