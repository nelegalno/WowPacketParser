using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class AccountDataHandler
    {
        [Parser(Opcode.SMSG_ACCOUNT_DATA_TIMES)]
        public static void HandleAccountDataTimes(Packet packet)
        {
            packet.ReadBit("byte20");

            for (var i = 0; i < 8; ++i)
                packet.ReadTime("[" + (AccountDataType)i + "]" + " Time");

            packet.ReadUInt32("dword16");
            packet.ReadTime("Server Time"); //24*4
        }

        [Parser(Opcode.SMSG_LOGOUT_CANCEL_ACK)]
        public static void HandleLogoutCancelAck(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_LOGOUT_COMPLETE)]
        public static void HandleLogoutCompletek(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_LOGOUT_CANCEL)]
        [Parser(Opcode.CMSG_LOGOUT_REQUEST)]
        public static void HandleAccountNull(Packet packet)
        {
        }
    }
}
