using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreObjects = WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class ActionBarHandler
    {
        [Parser(Opcode.SMSG_ACTION_BUTTONS)]
        public static void HandleActionButtons(Packet packet)
        {
            packet.ReadToEnd();
        }
    }
}
