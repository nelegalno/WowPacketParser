using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using WowPacketParserModule.V5_4_8_18414.Enums;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class TalentHandler
    {
        [Parser(Opcode.CMSG_LEARN_TALENT)]
        public static void HandleLearnTalent(Packet packet)
        {
            var talentCount = packet.ReadBits("Talent Count", 23);

            for (int i = 0; i < talentCount; i++)
                packet.ReadUInt16("Talent Id", i);
        }

        [Parser(Opcode.CMSG_SET_PRIMARY_TALENT_TREE)]
        public static void HandleSetPrimaryTalentTreeSpec(Packet packet)
        {
            packet.ReadUInt32("Spec Tab Id");
        }

        [Parser(Opcode.SMSG_TALENTS_INFO)]
        public static void ReadTalentInfo(Packet packet)
        {
            packet.ReadToEnd();
        }
    }
}
