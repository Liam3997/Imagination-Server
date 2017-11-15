using System.Collections.Generic;
using ImaginationServer.Common.CharacterData;
using ImaginationServer.Common.Data;

namespace ImaginationServer.World.Replica.Components
{
    public class InventoryComponent : ReplicaComponent
    {
        public List<EquipmentData> Items { get; set; } = new List<EquipmentData>();
        public uint Data2 { get; set; } = 0; // Some sort of count?

        public override void WriteToPacket(WBitStream bitStream, ReplicaPacketType type)
        {
            uint size = (uint) Items.Count;
            bool hasEquipment = size > 0;
            bitStream.Write(hasEquipment);
            if (hasEquipment)
            {
                bitStream.Write(size);
                for (var k = 0; k < size; k++)
                {
                    EquipmentData item = Items[k];
                    bitStream.Write(item.Id);
                    bitStream.Write(item.Lot);
                    bitStream.Write(item.D3 > 0);
                    if (item.D3 > 0)
                    {
                        bitStream.Write(item.D3);
                    }
                    bitStream.Write(item.D4 > 0);
                    if (item.D4 > 0)
                    {
                        bitStream.Write(item.D4);
                    }
                    bitStream.Write(item.Slot > 0);
                    if (item.Slot > 0)
                    {
                        bitStream.Write(item.Slot);
                    }
                    bitStream.Write(item.D6 > 0);
                    if (item.D6 > 0)
                    {
                        bitStream.Write(item.D6);
                    }
                    uint s = (uint) item.D7.Count;
                    bool s0 = s > 0;
                    bitStream.Write(s0);
                    if (s0)
                    {
                        bitStream.Write(s);
                        for (var i = 0; i < s; i++)
                        {
                            bitStream.Write(item.D7[i]);
                        }
                    }
                    bitStream.Write(item.D8);
                }
            }
            // TODO: LUNI makes this a >= comparison, but it will always be true since Data2 can't be negative...is this a bug?
            bool f2 = Data2 >= 0;
            bitStream.Write(f2);
            if (f2)
            {
                bitStream.Write(Data2);
            }
        }

        public override uint GetComponentId()
        {
            return 17;
        }

        public class EquipmentData
        {
            public long Id { get; set; } = 0;
            public uint Lot { get; set; } = 0;
            public ulong D3 { get; set; } = 0;
            public uint D4 { get; set; } = 1; // Always 1?
            public ushort Slot { get; set; } = 0;
            public uint D6 { get; set; } = 4; // Always 4
            public List<byte> D7 { get; set; } = new List<byte>();
            public bool D8 { get; set; }
        }
    }
}