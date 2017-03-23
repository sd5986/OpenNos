﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenNos.Core;

namespace OpenNos.GameObject.Packets.ServerPackets
{
    [PacketHeader("treq")]
    public class TreqPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public int X { get; set; }

        [PacketIndex(1)]
        public int Y { get; set; }

        [PacketIndex(2)]
        public byte? StartPress { get; set; }

        [PacketIndex(3)]
        public byte? RecordPress { get; set; }

        #endregion
    }
}