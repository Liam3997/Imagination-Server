﻿using System.Collections.Generic;

namespace ImaginationServer.Enums
{
    public class ZoneChecksums
    {
        public static readonly byte[] VentureExplorer = { 0x7c, 0x08, 0xb8, 0x20 };

        public static Dictionary<ZoneId, byte[]> Checksums { get; } = new Dictionary<ZoneId, byte[]>
        { {ZoneId.VentureExplorer, VentureExplorer} };
    }
}