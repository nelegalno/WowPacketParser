using System;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using WowPacketParserModule.V5_4_8_18414.Enums;
using Guid = WowPacketParser.Misc.Guid;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class QueryHandler
    {
        [Parser(Opcode.CMSG_CREATURE_QUERY)]
        public static void HandleCreatureQuery(Packet packet)
        {
            packet.ReadUInt32("Entry");
        }

        [Parser(Opcode.CMSG_NAME_QUERY)]
        public static void HandleNameQuery(Packet packet)
        {
            var guid = new byte[8];
            guid[4] = packet.ReadBit();
            var byte20 = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var byte28 = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ParseBitStream(guid, 7, 5, 1, 2, 6, 3, 0, 4);
            packet.WriteGuid("Guid", guid);

            if (byte20)
                packet.ReadInt32("int16");

            if (byte28)
                packet.ReadInt32("int24");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_CREATURE_QUERY_RESPONSE)]
        public static void HandleCreatureQueryResponse(Packet packet)
        {
            var entry = packet.ReadEntry("Entry");

            var hasData = packet.ReadBit("hasData");

            if (!hasData)
                return; // nothing to do

            var creature = new UnitTemplate();

            uint lengthSubname = packet.ReadBits("Subname length", 11);
            uint qItemCount = packet.ReadBits("itemCount", 22);

            packet.ReadBits(11); // Unk String length. Needs reading somewhere?
			
            var lengthName = new int[4][];
            for (var i = 0; i < 4; i++)
            {
                lengthName[i] = new int[2];
                lengthName[i][0] = (int)packet.ReadBits("Name length female", 11);
                lengthName[i][1] = (int)packet.ReadBits("Name length male", 11);
            }

            creature.RacialLeader = packet.ReadBit("Racial Leader");

            uint lengthIconName = packet.ReadBits(6) ^ 1;

            creature.KillCredits = new uint[2];
            creature.KillCredits[0] = packet.ReadUInt32("Kill Credit 1");
			
            creature.DisplayIds = new uint[4];
            creature.DisplayIds[3] = packet.ReadUInt32("Display Id 4");
            creature.DisplayIds[1] = packet.ReadUInt32("Display Id 2");
			
            creature.Expansion = packet.ReadEnum<ClientType>("Expansion", TypeCode.UInt32);
			
            creature.Type = packet.ReadEnum<CreatureType>("Type", TypeCode.Int32);
			
            creature.Modifier2 = packet.ReadSingle("Modifier Health");
			
            creature.TypeFlags2 = packet.ReadUInt32("Creature Type Flags 2");
			
            creature.TypeFlags = packet.ReadEnum<CreatureTypeFlag>("Type Flags", TypeCode.UInt32);
			
            creature.Rank = packet.ReadEnum<CreatureRank>("Rank", TypeCode.Int32);
            creature.MovementId = packet.ReadUInt32("Movement Id");

            var name = new string[8];
            for (var i = 0; i < 4; ++i)
            {
                if (lengthName[i][1] > 1)
                    packet.ReadCString("Male Name", i);

                if (lengthName[i][0] > 1)
                    name[i] = packet.ReadCString("Female name", i);
            }
            creature.Name = name[0];
			
            if (lengthSubname > 1)
                creature.SubName = packet.ReadCString("Sub Name");
				
			
            creature.DisplayIds[0] = packet.ReadUInt32("Display Id 1");	
				
            creature.DisplayIds[2] = packet.ReadUInt32("Display Id 3");
			
            if (lengthIconName > 1)
                creature.IconName = packet.ReadCString("Icon Name");
			
            creature.QuestItems = new uint[qItemCount];
            for (var i = 0; i < qItemCount; ++i)
                creature.QuestItems[i] = (uint)packet.ReadEntryWithName<UInt32>(StoreNameType.Item, "Quest Item", i);			
			
            creature.KillCredits[1] = packet.ReadUInt32("Kill Credit 2");

            creature.Modifier1 = packet.ReadSingle("Modifier Mana");

            creature.Family = packet.ReadEnum<CreatureFamily>("Family", TypeCode.Int32);

            packet.AddSniffData(StoreNameType.Unit, entry.Key, "QUERY_RESPONSE");

            Storage.UnitTemplates.Add((uint)entry.Key, creature, packet.TimeSpan);

            var objectName = new ObjectName
            {
                ObjectType = ObjectType.Unit,
                Name = creature.Name,
            };
            Storage.ObjectNames.Add((uint)entry.Key, objectName, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_NAME_QUERY_RESPONSE)]
        public static void HandleNameQueryResponse(Packet packet)
        {
            var guid = packet.StartBitStream(3, 6, 7, 2, 5, 4, 0, 1);
            packet.ParseBitStream(guid, 5, 4, 7, 6, 1, 2);

            var nameData = !packet.ReadBoolean("not nameData");
            if (nameData)
            {
                packet.ReadInt32("unk108");
                packet.ReadInt32("unk36");
                packet.ReadEnum<Class>("Class", TypeCode.Byte);
                packet.ReadEnum<Race>("Race", TypeCode.Byte);
                packet.ReadByte("Level");
                packet.ReadEnum<Gender>("Gender", TypeCode.Byte);
            }
            packet.ParseBitStream(guid, 0, 3);

            packet.WriteGuid("Guid", guid);

            if (!nameData)
                return;

            var guid2 = new byte[8];
            var guid3 = new byte[8];

            guid2[2] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid3[7] = packet.ReadBit();
            guid3[2] = packet.ReadBit();
            guid3[0] = packet.ReadBit();
            var unk32 = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid3[5] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid2[0] = packet.ReadBit();

            var len = new uint[5];
            for (var i = 5; i > 0; i--)
                len[i - 1] = packet.ReadBits(7);

            guid3[6] = packet.ReadBit();
            guid3[3] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid3[1] = packet.ReadBit();
            guid3[4] = packet.ReadBit();

            var len56 = packet.ReadBits(6);

            guid2[6] = packet.ReadBit();

            packet.ReadXORByte(guid3, 6);
            packet.ReadXORByte(guid3, 0);

            var name = packet.ReadWoWString("Name", len56);
            var playerGuid = new Guid(BitConverter.ToUInt64(guid, 0));
            StoreGetters.AddName(playerGuid, name);

            packet.ReadXORByte(guid2, 5);
            packet.ReadXORByte(guid2, 2);
            packet.ReadXORByte(guid3, 3);
            packet.ReadXORByte(guid2, 4);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid3, 4);
            packet.ReadXORByte(guid3, 2);
            packet.ReadXORByte(guid2, 7);

            for (var i = 5; i > 0; i--)
                packet.ReadWoWString("str", len[i - 1], i);

            packet.ReadXORByte(guid2, 6);
            packet.ReadXORByte(guid3, 7);
            packet.ReadXORByte(guid3, 1);
            packet.ReadXORByte(guid2, 1);
            packet.ReadXORByte(guid3, 5);
            packet.ReadXORByte(guid2, 0);

            packet.WriteGuid("Guid2", guid2);
            packet.WriteGuid("Guid3", guid3);

            var objectName = new ObjectName
            {
                ObjectType = ObjectType.Player,
                Name = name,
            };
            Storage.ObjectNames.Add((uint)playerGuid.GetLow(), objectName, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_NPC_TEXT_UPDATE)]
        public static void HandleNpcTextUpdate(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_QUERY_TIME_RESPONSE)]
        public static void HandleQueryTimeResponse(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_REALM_NAME_QUERY_RESPONSE)]
        public static void HandleQueryRealmNameResponse(Packet packet)
        {
            packet.ReadToEnd();
        }
    }
}
