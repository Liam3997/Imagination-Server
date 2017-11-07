﻿using System;
using System.Collections.Generic;
using System.IO;
using static ImaginationServer.Enums.PacketEnums;
using static ImaginationServer.Enums.PacketEnums.WorldServerPacketId;

namespace ImaginationServer.Common
{
    public static class ExtensionMethods
    {
        public static long ToCppTime(this DateTime time)
        {
            return (long) time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime ToDateTime(this long cppTime)
        {
            return new DateTime(1970, 1, 1).AddSeconds(cppTime);
        }

        public static string ReadWString(this BinaryReader reader, int maxLength)
        {
            var valueChars = new List<char>();
            while (true)
            {
                var c = reader.ReadChar();
                if (c == '\0' || valueChars.Count >= maxLength) break;
                valueChars.Add(c);
            }
            return new string(valueChars.ToArray());
        }

        public static void WriteHeader(this WBitStream bitStream, RemoteConnection remoteConnection, uint packetCode)
        {
            bitStream.Write((byte) 83);
            bitStream.Write((ushort) remoteConnection);
            bitStream.Write(packetCode);
            bitStream.Write((byte) 0);
        }

        public static void InitGameMsg(this WBitStream bitStream, long objId, ushort msgId)
        {
            bitStream.WriteHeader(RemoteConnection.Client, (uint) MsgClientGameMsg);
            bitStream.Write(objId);
            bitStream.Write(msgId);
        }
    }
}