using System;
using System.Collections.Generic;
using System.Linq;

namespace ImaginationServer.World.Replica.Components
{
    public class DestructibleComponent : ReplicaComponent
    {
        public bool Flag1 { get; set; }
        public List<Data1> DataOneList { get; set; } = new List<Data1>();
        public bool Flag2 { get; set; }
        public List<Data2> DataTwoList { get; set; } = new List<Data2>();
        public bool Flag3 { get; set; }
        public Data3 DataThree { get; set; } = new Data3();
        public bool Flag4 { get; set; }
        public Data4 DataFour { get; set; } = new Data4();
        public List<uint> Data4_1 { get; set; } = new List<uint>();
        public bool Trigger { get; set; }
        public bool Data4_2 { get; set; }
        public bool Data4_3 { get; set; }
        public bool Data4_4_1 { get; set; }
        public bool Flag4_4_2 { get; set; }
        public uint Data4_4_2 { get; set; }
        public bool Flag5 { get; set; }
        public bool Data5 { get; set; }

        public override void WriteToPacket(WBitStream bitStream, ReplicaPacketType type)
        {
            if (type == ReplicaPacketType.Construction)
            {
                bitStream.Write(Flag1);
                if (Flag1)
                {
                    uint data1C = (uint) DataOneList.Count;
                    bitStream.Write(data1C);
                    for (var k = 0; k < data1C; k++)
                    {
                        Data1 currentData = DataOneList[k];
                        bitStream.Write(currentData.D1);
                        bitStream.Write(currentData.D2Flag);
                        if (currentData.D2Flag)
                        {
                            bitStream.Write(currentData.D2);
                        }
                        bitStream.Write(currentData.D3);
                        bitStream.Write(currentData.D4);
                        bitStream.Write(currentData.D5);
                        bitStream.Write(currentData.D6);
                        bitStream.Write(currentData.D7);
                        bitStream.Write(currentData.D8);
                        bitStream.Write(currentData.D9);
                        bitStream.Write(currentData.D10);
                        bool t = currentData.D11;
                        bitStream.Write(t);
                        bitStream.Write(currentData.D12);
                        if (t)
                        {
                            bitStream.Write(currentData.D13);
                        }
                        bitStream.Write(currentData.D14);
                    }
                }
                bitStream.Write(Flag2);
                if (Flag2)
                {
                    uint data2C = (uint) DataTwoList.Count;
                    bitStream.Write(data2C);
                    for (var k = 0; k < data2C; k++)
                    {
                        Data2 currentData = DataTwoList[k];
                        bitStream.Write(currentData.D1);
                        bitStream.Write(currentData.D2Flag);
                        if (currentData.D2Flag)
                        {
                            bitStream.Write(currentData.D2);
                        }
                        bitStream.Write(currentData.D3);
                        bitStream.Write(currentData.D4);
                        bitStream.Write(currentData.D5);
                        bitStream.Write(currentData.D6);
                        bitStream.Write(currentData.D7);
                        bitStream.Write(currentData.D8);
                        bitStream.Write(currentData.D9);
                        bitStream.Write(currentData.D10);
                        bool t = currentData.D11;
                        bitStream.Write(t);
                        bitStream.Write(currentData.D12);
                        if (t)
                        {
                            bitStream.Write(currentData.D13);
                        }
                        bitStream.Write(currentData.D14);
                    }
                }
            }
            //Index 23
            if (type == ReplicaPacketType.Construction)
            {
                bitStream.Write(Flag3);
                if (Flag3)
                {
                    bitStream.Write(DataThree.D1);
                    bitStream.Write(DataThree.D2);
                    bitStream.Write(DataThree.D3);
                    bitStream.Write(DataThree.D4);
                    bitStream.Write(DataThree.D5);
                    bitStream.Write(DataThree.D6);
                    bitStream.Write(DataThree.D7);
                    bitStream.Write(DataThree.D8);
                    bitStream.Write(DataThree.D9);
                    //flag3 = false;
                }
            }

            bitStream.Write(Flag4);
            if (Flag4)
            {
                bitStream.Write(DataFour.Health);
                bitStream.Write(DataFour.MaxHealthN);
                bitStream.Write(DataFour.Armor);
                bitStream.Write(DataFour.MaxArmorN);
                bitStream.Write(DataFour.Imagination);
                bitStream.Write(DataFour.MaxImaginationN);
                bitStream.Write(DataFour.D7);
                bitStream.Write(DataFour.D8);
                bitStream.Write(DataFour.D9);
                bitStream.Write(DataFour.D10);
                bitStream.Write(DataFour.MaxHealth);
                bitStream.Write(DataFour.MaxArmor);
                bitStream.Write(DataFour.MaxImagination);
                int t = Data4_1.Count;
                bitStream.Write(t);
                for (var k = 0; k < t; k++)
                {
                    bitStream.Write(Data4_1[k]);
                }
                bitStream.Write(Trigger);
                if (type == ReplicaPacketType.Construction)
                {
                    bitStream.Write(Data4_2);
                    bitStream.Write(Data4_3);
                    if (Trigger)
                    {
                        bitStream.Write(Data4_4_1);
                        bitStream.Write(Flag4_4_2);
                        if (Flag4_4_2)
                        {
                            bitStream.Write(Data4_4_2);
                        }
                    }
                }
                //flag4 = false;
            }

            bitStream.Write(Flag5);
            if (Flag5)
            {
                bitStream.Write(Data5);
                //flag5 = false;
            }
        }

        public override uint GetComponentId()
        {
            return 7;
        }

        public class Data1
        {
            public uint D1 { get; set; } = 0;
            public bool D2Flag { get; set; } = false;
            public uint D2 { get; set; } = 0;
            public bool D3 { get; set; } = false;
            public bool D4 { get; set; } = false;
            public bool D5 { get; set; } = false;
            public bool D6 { get; set; } = false;
            public bool D7 { get; set; } = false;
            public bool D8 { get; set; } = false;
            public bool D9 { get; set; } = false;
            public bool D10 { get; set; } = false;
            public bool D11 { get; set; } = false; // Flag for D13
            public bool D12 { get; set; } = false;
            public ulong D13 { get; set; } = 0;
            public uint D14 { get; set; } = 0;
        }

        public class Data2
        {
            // Dupe of Data1?
            public uint D1 { get; set; } = 0;
            public bool D2Flag { get; set; } = false;
            public uint D2 { get; set; } = 0;
            public bool D3 { get; set; } = false;
            public bool D4 { get; set; } = false;
            public bool D5 { get; set; } = false;
            public bool D6 { get; set; } = false;
            public bool D7 { get; set; } = false;
            public bool D8 { get; set; } = false;
            public bool D9 { get; set; } = false;
            public bool D10 { get; set; } = false;
            public bool D11 { get; set; } = false; // Flag for D13
            public bool D12 { get; set; } = false;
            public ulong D13 { get; set; } = 0;
            public uint D14 { get; set; } = 0;
        }

        public class Data3
        {
            public uint D1 { get; set; } = 0;
            public uint D2 { get; set; } = 0;
            public uint D3 { get; set; } = 0;
            public uint D4 { get; set; } = 0;
            public uint D5 { get; set; } = 0;
            public uint D6 { get; set; } = 0;
            public uint D7 { get; set; } = 0;
            public uint D8 { get; set; } = 0;
            public uint D9 { get; set; } = 0;
        }

        public class Data4
        {
            public uint Health { get; set; } = 4;
            public float MaxHealth { get; set; } = 4.0f;
            public float MaxHealthN { get; set; } = 4.0f;
            public uint Armor { get; set; } = 0;
            public float MaxArmor { get; set; } = 0.0f;
            public float MaxArmorN { get; set; } = 0.0f;
            public uint Imagination { get; set; } = 0;
            public float MaxImagination { get; set; } = 0.0f;
            public float MaxImaginationN { get; set; } = 0.0f;
            public uint D7 { get; set; } = 0;
            public bool D8 { get; set; } = false;
            public bool D9 { get; set; } = false;
            public bool D10 { get; set; } = false;
        }
    } 
}