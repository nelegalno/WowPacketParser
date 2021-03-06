using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class InstanceHandler
    {
        [Parser(Opcode.CMSG_RESET_INSTANCES)]
        [Parser(Opcode.SMSG_UPDATE_DUNGEON_ENCOUNTER_FOR_LOOT)]
        public static void HandleInstanceNull(Packet packet)
        {
        }

        [Parser(Opcode.MSG_SET_RAID_DIFFICULTY)]
        [Parser(Opcode.SMSG_INSTANCE_RESET)]
        [Parser(Opcode.SMSG_SET_DUNGEON_DIFFICULTY)]
        [Parser(Opcode.SMSG_UPDATE_LAST_INSTANCE)]
        public static void HandleSetDifficulty(Packet packet)
        {
            packet.ReadToEnd();
        }
    }
}
