using System;

namespace ImaginationServer.World.Replica.Components
{
    public class ControllablePhysicsComponent : ReplicaComponent
    {
        public bool Flag3 { get; set; }
        public Data3 DataThree { get; set; } = new Data3();
        public bool Flag4 { get; set; }
        public Data4 DataFour { get; set; } = new Data4();
        public bool Flag5 { get; set; }
        public Data5 DataFive { get; set; } = new Data5();
        public bool Flag6 { get; set; }
        public Data6 DataSix { get; set; } = new Data6();

        #region Serialization only

        public bool Flag7 { get; set; }

        #endregion

        public override void WriteToPacket(WBitStream bitStream, ReplicaPacketType type)
        {
            if (type == ReplicaPacketType.Construction)
            {
                bitStream.Write(Flag1);
                if (Flag1)
                {
                    bitStream.Write(DataOne.D1);
                    bitStream.Write(DataOne.D2);
                    bitStream.Write(DataOne.D3);
                }

                bitStream.Write(Flag2);
                if (Flag2)
                {
                    bitStream.Write(DataTwo.D1);
                    bitStream.Write(DataTwo.D2);
                    bitStream.Write(DataTwo.D3);
                    bitStream.Write(DataTwo.D4);
                    bitStream.Write(DataTwo.D5);
                    bitStream.Write(DataTwo.D6);
                    bitStream.Write(DataTwo.D7);
                }
            }

            bitStream.Write(Flag3);
            if (Flag3)
            {
                bitStream.Write(DataThree.D1);
                bitStream.Write(DataThree.D2);
            }

            bitStream.Write(Flag4);
            if (Flag4)
            {
                bitStream.Write(DataFour.D1);
                bitStream.Write(DataFour.D2);
            }

            bitStream.Write(Flag5);
            if (Flag5)
            {
                bitStream.Write(DataFive.Flag);
                if (DataFive.Flag)
                {
                    bitStream.Write(DataFive.D1);
                    bitStream.Write(DataFive.D2);
                }
            }

            bitStream.Write(Flag6);
            if (Flag6)
            {
                bitStream.Write(DataSix.PosX);
                bitStream.Write(DataSix.PosY);
                bitStream.Write(DataSix.PosZ);

                bitStream.Write(DataSix.RotationX);
                bitStream.Write(DataSix.RotationY);
                bitStream.Write(DataSix.RotationZ);
                bitStream.Write(DataSix.RotationW);

                bitStream.Write(DataSix.IsOnGround);
                bitStream.Write(DataSix.Unknown1);

                bitStream.Write(DataSix.VelocityFlag);
                if (DataSix.VelocityFlag)
                {
                    bitStream.Write(DataSix.VelocityX);
                    bitStream.Write(DataSix.VelocityY);
                    bitStream.Write(DataSix.VelocityZ);
                }

                bitStream.Write(DataSix.AngularVelocityFlag);
                if (DataSix.AngularVelocityFlag)
                {
                    bitStream.Write(DataSix.AngularVelocityX);
                    bitStream.Write(DataSix.AngularVelocityY);
                    bitStream.Write(DataSix.AngularVelocityZ);
                }

                bitStream.Write(DataSix.MovingPlatformFlag);
                if (DataSix.MovingPlatformFlag)
                {
                    bitStream.Write(DataSix.MpUnknown1);
                    bitStream.Write(DataSix.MpUnknown2);
                    bitStream.Write(DataSix.MpUnknown3);
                    bitStream.Write(DataSix.MpUnknown4);
                    bitStream.Write(DataSix.MpFlag1);
                    if (DataSix.MpFlag1)
                    {
                        bitStream.Write(DataSix.MpUnknownD1);
                        bitStream.Write(DataSix.MpUnknownD2);
                        bitStream.Write(DataSix.MpUnknownD3);
                    }
                }
            }

            if (type == ReplicaPacketType.Serialization)
            {
                bitStream.Write(Flag7);
            }
        }

        public override uint GetComponentId()
        {
            return 1;
        }

        public void SetPosition(float x, float y, float z)
        {
            DataSix.PosX = x;
            DataSix.PosY = y;
            DataSix.PosZ = z;
            Flag6 = true;
        }

        #region Creation only

        public bool Flag1 { get; set; }
        public Data1 DataOne { get; set; }
        public bool Flag2 { get; set; }
        public Data2 DataTwo { get; set; }

        #endregion

        public class Data1
        {
            public uint D1 { get; set; }
            public bool D2 { get; set; }
            public bool D3 { get; set; }
        }

        public class Data2
        {
            public uint D1 { get; set; }
            public uint D2 { get; set; }
            public uint D3 { get; set; }
            public uint D4 { get; set; }
            public uint D5 { get; set; }
            public uint D6 { get; set; }
            public uint D7 { get; set; }
        }

        public class Data3
        {
            public float D1 { get; set; }
            public float D2 { get; set; }
        }

        public class Data4
        {
            public uint D1 { get; set; }
            public bool D2 { get; set; }
        }

        public class Data5
        {
            public bool Flag { get; set; }
            public uint D1 { get; set; }
            public bool D2 { get; set; }
        }

        public class Data6
        {
            public float PosX { get; set; }
            public float PosY { get; set; }
            public float PosZ { get; set; }
            public float RotationX { get; set; }
            public float RotationY { get; set; }
            public float RotationZ { get; set; }
            public float RotationW { get; set; }
            public bool IsOnGround { get; set; }
            public bool Unknown1 { get; set; }
            public bool VelocityFlag { get; set; }

            #region Velocity

            public float VelocityX { get; set; }
            public float VelocityY { get; set; }
            public float VelocityZ { get; set; }

            #endregion

            public bool AngularVelocityFlag { get; set; }

            #region Angular Velocity

            public float AngularVelocityX { get; set; }
            public float AngularVelocityY { get; set; }
            public float AngularVelocityZ { get; set; }

            #endregion

            public bool MovingPlatformFlag { get; set; }

            #region Moving Platform

            public long MpUnknown1 { get; set; }
            public float MpUnknown2 { get; set; }
            public float MpUnknown3 { get; set; }
            public float MpUnknown4 { get; set; }
            public bool MpFlag1 { get; set; }

            #region Flag data

            public float MpUnknownD1 { get; set; }
            public float MpUnknownD2 { get; set; }
            public float MpUnknownD3 { get; set; }

            #endregion

            #endregion
        }
    }
}