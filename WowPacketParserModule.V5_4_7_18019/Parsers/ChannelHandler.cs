using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_7_18019.Parsers
{
    public static class ChannelHandler
    {
        [Parser(Opcode.CMSG_CHANNEL_SILENCE_VOICE)]
        [Parser(Opcode.CMSG_CHANNEL_UNSILENCE_VOICE)]
        [Parser(Opcode.CMSG_CHANNEL_SILENCE_ALL)]
        [Parser(Opcode.CMSG_CHANNEL_UNSILENCE_ALL)]
        public static void HandleChannelSilencing(Packet packet)
        {
            packet.ReadCString("Channel Name");
            packet.ReadCString("Player Name");
        }

        [Parser(Opcode.CMSG_CHANNEL_LIST)]
        [Parser(Opcode.CMSG_CHANNEL_OWNER)]
        [Parser(Opcode.CMSG_CHANNEL_ANNOUNCEMENTS)]
        [Parser(Opcode.CMSG_CHANNEL_VOICE_ON)]
        [Parser(Opcode.CMSG_CHANNEL_VOICE_OFF)]
        [Parser(Opcode.CMSG_SET_CHANNEL_WATCH)]
        [Parser(Opcode.CMSG_DECLINE_CHANNEL_INVITE)]
        [Parser(Opcode.CMSG_CHANNEL_DISPLAY_LIST)]
        public static void HandleChannelMisc(Packet packet)
        {
            packet.ReadCString("Channel Name");
        }

        [Parser(Opcode.SMSG_CHANNEL_LIST)]
        public static void HandleChannelSendList(Packet packet)
        {
            packet.AsHex();
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_CHANNEL_MEMBER_COUNT)]
        public static void HandleChannelMemberCount(Packet packet)
        {
            packet.ReadCString("Channel Name");
            packet.ReadEnum<ChannelFlag>("Flags", TypeCode.Byte);
            packet.ReadInt32("Unk int32");
        }

        [Parser(Opcode.SMSG_CHANNEL_NOTIFY)]
        public static void HandleChannelNotify(Packet packet)
        {
            var type = packet.ReadEnum<ChatNotificationType>("Notification Type", TypeCode.Byte);

            if (type == ChatNotificationType.InvalidName) // hack, because of some silly reason this type
                packet.ReadBytes(3);                      // has 3 null bytes before the invalid channel name

            packet.ReadCString("Channel Name");

            switch(type)
            {
                case ChatNotificationType.PlayerAlreadyMember:
                case ChatNotificationType.Invite:
                case ChatNotificationType.ModerationOn:
                case ChatNotificationType.ModerationOff:
                case ChatNotificationType.AnnouncementsOn:
                case ChatNotificationType.AnnouncementsOff:
                case ChatNotificationType.PasswordChanged:
                case ChatNotificationType.OwnerChanged:
                case ChatNotificationType.Joined:
                case ChatNotificationType.Left:
                case ChatNotificationType.VoiceOn:
                case ChatNotificationType.VoiceOff:
                {
                    packet.ReadGuid("GUID");
                    packet.ReadInt32("unk");
                    break;
                }
                case ChatNotificationType.YouJoined:
                {
                    packet.ReadEnum<ChannelFlag>("Flags", TypeCode.Byte);
                    packet.ReadInt32("Channel Id");
                    packet.ReadInt32("Unk");
                    break;
                }
                case ChatNotificationType.YouLeft:
                {
                    packet.ReadInt32("Channel Id");
                    packet.ReadBoolean("Unk");
                    break;
                }
                case ChatNotificationType.PlayerNotFound:
                case ChatNotificationType.ChannelOwner:
                case ChatNotificationType.PlayerNotBanned:
                case ChatNotificationType.PlayerInvited:
                case ChatNotificationType.PlayerInviteBanned:
                {
                    packet.ReadCString("Player Name");
                    break;
                }
                case ChatNotificationType.ModeChange:
                {
                    packet.ReadGuid("GUID");
                    packet.ReadEnum<ChannelMemberFlag>("Old Flags", TypeCode.Byte);
                    packet.ReadEnum<ChannelMemberFlag>("New Flags", TypeCode.Byte);
                    break;
                }
                case ChatNotificationType.PlayerKicked:
                case ChatNotificationType.PlayerBanned:
                case ChatNotificationType.PlayerUnbanned:
                {
                    packet.ReadGuid("Bad");
                    packet.ReadGuid("Good");
                    break;
                }
                case ChatNotificationType.Unknown1:
                {
                    packet.ReadGuid("GUID");
                    break;
                }
                case ChatNotificationType.WrongPassword:
                case ChatNotificationType.NotMember:
                case ChatNotificationType.NotModerator:
                case ChatNotificationType.NotOwner:
                case ChatNotificationType.Muted:
                case ChatNotificationType.Banned:
                case ChatNotificationType.InviteWrongFaction:
                case ChatNotificationType.WrongFaction:
                case ChatNotificationType.InvalidName:
                case ChatNotificationType.NotModerated:
                case ChatNotificationType.Throttled:
                case ChatNotificationType.NotInArea:
                case ChatNotificationType.NotInLfg:
                    break;
            }
        }

        [Parser(Opcode.CMSG_CHANNEL_BAN)] // 4.3.4
        public static void HandleChannelBan(Packet packet)
        {
            var channelLength = packet.ReadBits(8);
            var nameLength = packet.ReadBits(7);
            packet.ReadWoWString("Channel", channelLength);
            packet.ReadWoWString("Player to ban", nameLength);
        }

        [Parser(Opcode.CMSG_JOIN_CHANNEL)]
        public static void HandleChannelJoin(Packet packet)
        {
            packet.ReadInt32("Channel Id");
            var passwordLength = packet.ReadBits("PassLength", 7);
            packet.ReadBit("unk1"); //Has Voice");
            var channelLength = packet.ReadBits("ChannelLength", 7);
            packet.ReadBit("unk2"); //Joined by zone update");
            packet.ReadWoWString("Channel Pass", passwordLength);
            packet.ReadWoWString("Channel Name", channelLength);
        }

        [Parser(Opcode.CMSG_LEAVE_CHANNEL)]
        public static void HandleChannelLeave(Packet packet)
        {
            packet.ReadInt32("Channel Id");
            packet.ReadCString("Channel Name");
        }

        [Parser(Opcode.SMSG_USERLIST_REMOVE)]
        public static void HandleChannelUserListRemove(Packet packet)
        {
            packet.ReadGuid("GUID");
            packet.ReadEnum<ChannelFlag>("Flags", TypeCode.Byte);
            packet.ReadInt32("Counter");
            packet.ReadCString("Channel Name");
        }

        [Parser(Opcode.SMSG_USERLIST_ADD)]
        [Parser(Opcode.SMSG_USERLIST_UPDATE)]
        public static void HandleChannelUserListAdd(Packet packet)
        {
            packet.ReadGuid("GUID");
            packet.ReadEnum<ChannelMemberFlag>("Member Flags", TypeCode.Byte);
            packet.ReadEnum<ChannelFlag>("Flags", TypeCode.Byte);
            packet.ReadInt32("Counter");
            packet.ReadCString("Channel Name");
        }

        [Parser(Opcode.CMSG_CHANNEL_PASSWORD, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleChannelPassword(Packet packet)
        {
            packet.ReadCString("Channel Name");
            packet.ReadCString("Password");
        }

        [Parser(Opcode.CMSG_CHANNEL_PASSWORD, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleChannelPassword434(Packet packet)
        {
            var channelLen = packet.ReadBits(8);
            var passLen = packet.ReadBits(7);

            packet.ReadWoWString("Channel Name", channelLen);
            packet.ReadWoWString("Password", passLen);
        }

        [Parser(Opcode.CMSG_CHANNEL_INVITE, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleChannelInvite(Packet packet)
        {
            packet.ReadCString("Channel Name");
            packet.ReadCString("Name");
        }

        [Parser(Opcode.CMSG_CHANNEL_INVITE, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleChannelInvite434(Packet packet)
        {
            var nameLen = packet.ReadBits(7);
            var channelLen = packet.ReadBits(8);

            packet.ReadWoWString("Name", nameLen);
            packet.ReadWoWString("Channel Name", channelLen);
        }
    }
}
