

namespace ImaginationServer.Common.Packets
{
    public abstract class OutgoingPacket
    {
        public abstract void Send(string clientAddress);
    }
}