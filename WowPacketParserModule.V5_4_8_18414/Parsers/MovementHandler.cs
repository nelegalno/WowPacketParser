using System;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using CoreParsers = WowPacketParser.Parsing.Parsers;
using Guid = WowPacketParser.Misc.Guid;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class MovementHandler
    {
        [Parser(Opcode.MSG_MOVE_FALL_LAND)]
        public static void HandleMoveFallLand(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var hasFallDircetion = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.Z = packet.ReadSingle(); // 44
            pos.X = packet.ReadSingle(); // 36
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            packet.ReadBit("bit 172"); // 172
            packet.ReadBit("bit 148"); // 148
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[7] = packet.ReadBit(); // 23
            packet.ReadBit("bit 149"); // 149
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            guid[5] = packet.ReadBit(); // 21
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            guid[2] = packet.ReadBit(); // 18
            guid[3] = packet.ReadBit(); // 19
            guid[0] = packet.ReadBit(); // 16
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[1] = packet.ReadBit(); // 17
            var hasTrans = packet.ReadBit("Has transport"); // 104
            guid[6] = packet.ReadBit(); // 22
            guid[4] = packet.ReadBit(); // 20
            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit(); // 56
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[6] = packet.ReadBit(); // 62
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
            }

            if (hasMovementFlags2)
                packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasFallData)
                hasFallDircetion = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags)
                packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24
            // flush bits
            packet.ParseBitStream(guid, 4, 3, 7, 0, 2, 5, 1, 6);

            if (Count > 0)
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Sin"); // 128
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 124
                }
                packet.ReadUInt32("Fall time"); // 116
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 4); // 60
                tpos.Y = packet.ReadSingle(); // 68
                tpos.O = packet.ReadSingle(); // 76
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.ReadXORByte(transportGuid, 6); // 62
                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 5); // 61
                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadUInt32("Transport Time"); // 84
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasSpline)
                packet.ReadInt32("Spline"); // 168

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.MSG_MOVE_HEARTBEAT)]
        public static void HandleMoveHeartbeat(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.X = packet.ReadSingle(); // 36
            pos.Z = packet.ReadSingle(); // 44
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            packet.ReadBit("bit 148"); // 148
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[3] = packet.ReadBit(); // 19
            guid[6] = packet.ReadBit(); // 22
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            packet.ReadBit("bit 149"); // 149
            packet.ReadBit("bit 172"); // 172
            guid[7] = packet.ReadBit(); // 23
            guid[2] = packet.ReadBit(); // 18
            guid[4] = packet.ReadBit(); // 20
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            var hasTrans = packet.ReadBit("Has transport"); // 104
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            guid[5] = packet.ReadBit(); // 21
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            guid[1] = packet.ReadBit(); // 17
            guid[0] = packet.ReadBit(); // 16

            if (hasTrans) // 104
            {
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[7] = packet.ReadBit(); // 63
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[4] = packet.ReadBit(); // 60
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
            }

            if (hasMovementFlags) // 24
                packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags2) // 28
                packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            // flush bits
            packet.ParseBitStream(guid, 2, 3, 6, 1, 4, 7);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 5, 0);

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                }
                packet.ReadSingle("Vertical Speed"); // 120
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 0); // 56
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 7); // 63
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 4); // 60
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 5); // 61
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadUInt32("Transport Time"); // 84
                tpos.O = packet.ReadSingle(); // 76
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.MSG_MOVE_JUMP)]
        public static void HandleMoveJump(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.X = packet.ReadSingle(); // 36
            pos.Z = packet.ReadSingle(); // 44
            guid[1] = packet.ReadBit(); // 17
            guid[7] = packet.ReadBit(); // 23
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            guid[5] = packet.ReadBit(); // 21
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[6] = packet.ReadBit(); // 22
            guid[4] = packet.ReadBit(); // 20
            packet.ReadBit("bit 149"); // 149
            var hasTrans = packet.ReadBit("Has transport"); // 104
            packet.ReadBit("bit 148"); // 148
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[3] = packet.ReadBit(); // 19
            packet.ReadBit("bit 172"); // 172
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            guid[2] = packet.ReadBit(); // 18
            guid[0] = packet.ReadBit(); // 16

            if (hasTrans) // 104
            {
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[4] = packet.ReadBit(); // 60
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[7] = packet.ReadBit(); // 63
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
            }

            if (hasMovementFlags) // 24
                packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

            if (hasMovementFlags2) // 28
                packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            // flush bits
            packet.ParseBitStream(guid, 7, 1, 0);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 2, 6, 3, 4, 5);

            if (hasFallData) // 140
            {
                packet.ReadSingle("Vertical Speed"); // 120
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                }
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 0); // 56
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 2); // 58
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadUInt32("Transport Time"); // 84
                tpos.X = packet.ReadSingle(); // 64
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 3); // 59
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                tpos.O = packet.ReadSingle(); // 76
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.MSG_MOVE_SET_FACING)]
        public static void HandleMoveSetFacing(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.X = packet.ReadSingle(); // 36
            pos.Z = packet.ReadSingle(); // 44
            guid[5] = packet.ReadBit(); // 21
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            guid[3] = packet.ReadBit(); // 19
            guid[2] = packet.ReadBit(); // 18
            var Count = packet.ReadBits("Counter", 22); // 152
            packet.ReadBit("bit 172"); // 172
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            guid[0] = packet.ReadBit(); // 16
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            packet.ReadBit("bit 148"); // 148
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[4] = packet.ReadBit(); // 20
            packet.ReadBit("bit 149"); // 149
            guid[1] = packet.ReadBit(); // 17
            guid[6] = packet.ReadBit(); // 22
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var hasTrans = packet.ReadBit("Has transport"); // 104
            guid[7] = packet.ReadBit(); // 23

            if (hasTrans) // 104
            {
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[0] = packet.ReadBit(); // 56
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[6] = packet.ReadBit(); // 62
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[4] = packet.ReadBit(); // 60
            }

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags2) // 28
                packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasMovementFlags) // 24
                packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

            // flush bits
            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 0, 6, 3, 1, 2, 7, 4, 5);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadXORByte(transportGuid, 2); // 58
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 7); // 63
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadUInt32("Transport Time"); // 84
                tpos.X = packet.ReadSingle(); // 64
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadSByte("Transport Seat"); // 80
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasFallData) // 140
            {
                packet.ReadUInt32("Fall time"); // 116
                packet.ReadSingle("Vertical Speed"); // 120
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Fall Sin"); // 124
                }
            }

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.MSG_MOVE_SET_PITCH)]
        public static void HandleMoveSetPitch(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle(); // 44
            pos.X = packet.ReadSingle(); // 36
            pos.Y = packet.ReadSingle(); // 40
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            packet.ReadBit("bit 149"); // 149
            guid[4] = packet.ReadBit(); // 20
            var hasTrans = packet.ReadBit("Has transport"); // 104
            guid[7] = packet.ReadBit(); // 23
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[0] = packet.ReadBit(); // 16
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            guid[5] = packet.ReadBit(); // 21
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            packet.ReadBit("bit 172"); // 172
            guid[2] = packet.ReadBit(); // 18
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            guid[6] = packet.ReadBit(); // 22
            guid[1] = packet.ReadBit(); // 17
            packet.ReadBit("bit 148"); // 148
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            guid[3] = packet.ReadBit(); // 19

            if (hasTrans) // 104
            {
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[6] = packet.ReadBit(); // 62
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[2] = packet.ReadBit(); // 58
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[0] = packet.ReadBit(); // 56
            }

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags) // 24
                packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

            if (hasMovementFlags2) // 28
                packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            // flush bits
            packet.ParseBitStream(guid, 2, 4, 5, 6, 0);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 3, 7, 1);

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.ReadXORByte(transportGuid, 2); // 58
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 5); // 61
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadSByte("Transport Seat"); // 80
                tpos.O = packet.ReadSingle(); // 76
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 7); // 63
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Fall Sin"); // 124
                }
                packet.ReadUInt32("Fall time"); // 116
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.MSG_MOVE_SET_RUN_MODE)]
        public static void HandleMoveSetRunMode(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle(); // 44
            pos.Y = packet.ReadSingle(); // 40
            pos.X = packet.ReadSingle(); // 36
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[1] = packet.ReadBit(); // 17
            guid[4] = packet.ReadBit(); // 20
            guid[0] = packet.ReadBit(); // 16
            guid[3] = packet.ReadBit(); // 19
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[6] = packet.ReadBit(); // 22
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[2] = packet.ReadBit(); // 18
            guid[5] = packet.ReadBit(); // 21
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            guid[7] = packet.ReadBit(); // 23
            packet.ReadBit("bit 148"); // 148
            packet.ReadBit("bit 172"); // 172
            packet.ReadBit("bit 149"); // 149
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            var hasTrans = packet.ReadBit("Has transport"); // 104

            if (hasTrans) // 104
            {
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[5] = packet.ReadBit(); // 61
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[3] = packet.ReadBit(); // 59
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[0] = packet.ReadBit(); // 56
            }

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags) // 24
                packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

            if (hasMovementFlags2) // 28
                packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            // flush bits
            packet.ParseBitStream(guid, 5, 6, 3, 7, 1, 0);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 4, 2);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 3); // 59
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 7); // 63
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadSByte("Transport Seat"); // 80
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadUInt32("Transport Time"); // 84
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 5); // 61
                tpos.Y = packet.ReadSingle(); // 68
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 128
                }
                packet.ReadUInt32("Fall time"); // 116
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.MSG_MOVE_SET_WALK_MODE)]
        public static void HandleMoveSetWalkMode(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.X = packet.ReadSingle(); // 36
            pos.Z = packet.ReadSingle(); // 44
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            packet.ReadBit("bit 172"); // 172
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[2] = packet.ReadBit(); // 18
            guid[4] = packet.ReadBit(); // 20
            guid[5] = packet.ReadBit(); // 21
            guid[1] = packet.ReadBit(); // 17
            guid[0] = packet.ReadBit(); // 16
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            packet.ReadBit("bit 148"); // 148
            guid[7] = packet.ReadBit(); // 23
            guid[3] = packet.ReadBit(); // 19
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[6] = packet.ReadBit(); // 22
            packet.ReadBit("bit 149"); // 149
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            var hasTrans = packet.ReadBit("Has transport"); // 104

            if (hasTrans) // 104
            {
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[0] = packet.ReadBit(); // 56
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[3] = packet.ReadBit(); // 59
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[2] = packet.ReadBit(); // 58
            }

            if (hasMovementFlags2) // 28
                packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasMovementFlags) // 24
                packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            // flush bits
            packet.ParseBitStream(guid, 4, 3, 7, 2, 1, 0, 6);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 5);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 6); // 62
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 7); // 63
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadXORByte(transportGuid, 2); // 58
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 3); // 59
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Fall Cos"); // 128
                }
                packet.ReadSingle("Vertical Speed"); // 120
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.MSG_MOVE_START_BACKWARD)]
        public static void HandleMoveStartBackWard(Packet packet)
        {
            if (packet.Direction == Direction.ClientToServer)
            {
                var guid = new byte[8];
                var transportGuid = new byte[8];
                var hasTransTime2 = false;
                var hasTransTime3 = false;
                var hasFallDirection = false;
                var pos = new Vector4();

                pos.Y = packet.ReadSingle(); // 40
                pos.Z = packet.ReadSingle(); // 44
                pos.X = packet.ReadSingle(); // 36
                var hasTime = !packet.ReadBit("Has no timestamp"); // 32
                var hasO = !packet.ReadBit("Has no Orient"); // 48
                guid[7] = packet.ReadBit(); // 23
                guid[2] = packet.ReadBit(); // 18
                var Count = packet.ReadBits("Counter", 22); // 152
                var hasFallData = packet.ReadBit("Has fall data"); // 140
                packet.ReadBit("bit 172"); // 172
                guid[5] = packet.ReadBit(); // 21
                guid[3] = packet.ReadBit(); // 19
                guid[6] = packet.ReadBit(); // 22
                var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
                guid[4] = packet.ReadBit(); // 20
                var hasTrans = packet.ReadBit("Has transport"); // 104
                guid[0] = packet.ReadBit(); // 16
                var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
                var hasPitch = !packet.ReadBit("Has no pitch"); // 112
                var hasSpline = !packet.ReadBit("has no Spline"); // 168
                var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
                packet.ReadBit("bit 148"); // 148
                guid[1] = packet.ReadBit(); // 17
                packet.ReadBit("bit 149"); // 149

                if (hasTrans) // 104
                {
                    transportGuid[1] = packet.ReadBit(); // 57
                    hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                    transportGuid[0] = packet.ReadBit(); // 56
                    transportGuid[7] = packet.ReadBit(); // 63
                    hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                    transportGuid[3] = packet.ReadBit(); // 59
                    transportGuid[5] = packet.ReadBit(); // 61
                    transportGuid[6] = packet.ReadBit(); // 62
                    transportGuid[2] = packet.ReadBit(); // 58
                    transportGuid[4] = packet.ReadBit(); // 60
                }

                if (hasMovementFlags2) // 28
                    packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

                if (hasMovementFlags) // 24
                    packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

                if (hasFallData) // 140
                    hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

                // flush bits
                if (Count > 0) // 152
                    for (var cnt = 0; cnt < Count; cnt++)
                    {
                        packet.ReadInt32("Dword 156", cnt); // 156
                    }

                packet.ParseBitStream(guid, 1, 3, 5, 2, 0, 4, 7, 6);

                if (hasSpline) // 168
                    packet.ReadInt32("Spline"); // 168

                if (hasTrans) // 104
                {
                    var tpos = new Vector4();
                    packet.ReadUInt32("Transport Time"); // 84
                    packet.ReadXORByte(transportGuid, 4); // 60
                    packet.ReadXORByte(transportGuid, 1); // 57
                    packet.ReadXORByte(transportGuid, 5); // 61
                    packet.ReadXORByte(transportGuid, 3); // 59
                    packet.ReadXORByte(transportGuid, 6); // 62
                    packet.ReadSByte("Transport Seat"); // 80
                    tpos.O = packet.ReadSingle(); // 76
                    tpos.X = packet.ReadSingle(); // 64
                    packet.ReadXORByte(transportGuid, 0); // 56
                    tpos.Y = packet.ReadSingle(); // 68
                    if (hasTransTime3) // 100
                        packet.ReadUInt32("Transport Time 3"); // 96
                    packet.ReadXORByte(transportGuid, 7); // 63
                    if (hasTransTime2) // 92
                        packet.ReadUInt32("Transport Time 2"); // 88
                    tpos.Z = packet.ReadSingle(); // 72
                    packet.ReadXORByte(transportGuid, 2); // 58
                    packet.WriteGuid("Transport Guid", transportGuid);
                    packet.WriteLine("Transport Position: {0}", tpos);
                }

                if (hasO)
                    pos.O = packet.ReadSingle(); // 48

                if (hasFallData) // 140
                {
                    packet.ReadUInt32("Fall time"); // 116
                    if (hasFallDirection) // 136
                    {
                        packet.ReadSingle("Fall Cos"); // 128
                        packet.ReadSingle("Fall Sin"); // 124
                        packet.ReadSingle("Horizontal Speed"); // 132
                    }
                    packet.ReadSingle("Vertical Speed"); // 120
                }

                if (hasPitch)
                    packet.ReadSingle("Pitch"); // 112

                if (hasTime)
                    packet.ReadUInt32("Timestamp"); // 32

                if (hasSplineElev)
                    packet.ReadSingle("Spline elevation"); // 144

                packet.WriteGuid("Guid", guid);
                packet.WriteLine("Position: {0}", pos);
            }
            else
            {
                packet.WriteLine("              : SMSG_SPELL_GO");
                //packet.AsHex();
                packet.ReadToEnd();
            }
        }

        [Parser(Opcode.MSG_MOVE_START_FORWARD)]
        public static void HandleMoveStartForward(Packet packet)
        {
            if (packet.Direction == Direction.ClientToServer)
            {
                var guid = new byte[8];
                var transportGuid = new byte[8];
                var hasTransTime2 = false;
                var hasTransTime3 = false;
                var hasFallDirection = false;
                var pos = new Vector4();

                pos.Z = packet.ReadSingle(); // 44
                pos.X = packet.ReadSingle(); // 36
                pos.Y = packet.ReadSingle(); // 40
                var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
                packet.ReadBit("bit 149"); // 149
                var hasSpline = !packet.ReadBit("has no Spline"); // 168
                packet.ReadBit("bit 148"); // 148
                guid[0] = packet.ReadBit(); // 16
                var hasO = !packet.ReadBit("Has no Orient"); // 48
                var hasFallData = packet.ReadBit("Has fall data"); // 140
                var Count = packet.ReadBits("Counter", 22); // 152
                guid[4] = packet.ReadBit(); // 20
                guid[1] = packet.ReadBit(); // 17
                var hasTime = !packet.ReadBit("Has no timestamp"); // 32
                guid[7] = packet.ReadBit(); // 23
                var hasPitch = !packet.ReadBit("Has no pitch"); // 112
                var hasTrans = packet.ReadBit("Has transport"); // 104
                guid[5] = packet.ReadBit(); // 21
                var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
                guid[3] = packet.ReadBit(); // 19
                var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
                guid[2] = packet.ReadBit(); // 18
                guid[6] = packet.ReadBit(); // 22
                packet.ReadBit("bit 172"); // 172

                if (hasTrans) // 104
                {
                    transportGuid[1] = packet.ReadBit(); // 57
                    hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                    transportGuid[3] = packet.ReadBit(); // 59
                    transportGuid[4] = packet.ReadBit(); // 60
                    transportGuid[2] = packet.ReadBit(); // 58
                    transportGuid[5] = packet.ReadBit(); // 61
                    transportGuid[0] = packet.ReadBit(); // 56
                    transportGuid[7] = packet.ReadBit(); // 63
                    transportGuid[6] = packet.ReadBit(); // 62
                    hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                }

                if (hasFallData) // 140
                    hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

                if (hasMovementFlags2) // 28
                    packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

                if (hasMovementFlags) // 24
                    packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

                // flush bits
                packet.ParseBitStream(guid, 1, 6, 7);

                if (Count > 0) // 152
                    for (var cnt = 0; cnt < Count; cnt++)
                    {
                        packet.ReadInt32("Dword 156", cnt); // 156
                    }

                packet.ParseBitStream(guid, 5, 0, 3, 2, 4);

                if (hasTrans) // 104
                {
                    var tpos = new Vector4();
                    packet.ReadXORByte(transportGuid, 3); // 59
                    packet.ReadXORByte(transportGuid, 1); // 57
                    packet.ReadXORByte(transportGuid, 6); // 62
                    tpos.Z = packet.ReadSingle(); // 72
                    packet.ReadXORByte(transportGuid, 4); // 60
                    if (hasTransTime3) // 100
                        packet.ReadUInt32("Transport Time 3"); // 96
                    packet.ReadSByte("Transport Seat"); // 80
                    packet.ReadXORByte(transportGuid, 7); // 63
                    tpos.O = packet.ReadSingle(); // 76
                    if (hasTransTime2) // 92
                        packet.ReadUInt32("Transport Time 2"); // 88
                    packet.ReadXORByte(transportGuid, 5); // 61
                    packet.ReadXORByte(transportGuid, 2); // 58
                    tpos.X = packet.ReadSingle(); // 64
                    packet.ReadXORByte(transportGuid, 0); // 56
                    tpos.Y = packet.ReadSingle(); // 68
                    packet.ReadUInt32("Transport Time"); // 84
                    packet.WriteGuid("Transport Guid", transportGuid);
                    packet.WriteLine("Transport Position: {0}", tpos);
                }

                if (hasFallData) // 140
                {
                    if (hasFallDirection) // 136
                    {
                        packet.ReadSingle("Fall Sin"); // 124
                        packet.ReadSingle("Fall Cos"); // 128
                        packet.ReadSingle("Horizontal Speed"); // 132
                    }
                    packet.ReadUInt32("Fall time"); // 116
                    packet.ReadSingle("Vertical Speed"); // 120
                }

                if (hasTime)
                    packet.ReadUInt32("Timestamp"); // 32

                if (hasPitch)
                    packet.ReadSingle("Pitch"); // 112

                if (hasSplineElev)
                    packet.ReadSingle("Spline elevation"); // 144

                if (hasO)
                    pos.O = packet.ReadSingle(); // 48

                if (hasSpline) // 168
                    packet.ReadInt32("Spline"); // 168

                packet.WriteGuid("Guid", guid);
                packet.WriteLine("Position: {0}", pos);
            }
            else
            {
                packet.WriteLine("              : SMSG_???");
                packet.AsHex();
                packet.ReadToEnd();
            }
        }

        [Parser(Opcode.MSG_MOVE_START_PITCH_UP)]
        public static void HandleMoveStartPitchUp(Packet packet)
        {
            if (packet.Direction == Direction.ClientToServer)
            {
                var guid = new byte[8];
                var transportGuid = new byte[8];
                var hasTransTime2 = false;
                var hasTransTime3 = false;
                var hasFallDirection = false;
                var pos = new Vector4();

                pos.Y = packet.ReadSingle(); // 40
                pos.Z = packet.ReadSingle(); // 44
                pos.X = packet.ReadSingle(); // 36
                guid[0] = packet.ReadBit(); // 16
                var hasPitch = !packet.ReadBit("Has no pitch"); // 112
                guid[3] = packet.ReadBit(); // 19
                packet.ReadBit("bit 148"); // 148
                var hasO = !packet.ReadBit("Has no Orient"); // 48
                guid[5] = packet.ReadBit(); // 21
                packet.ReadBit("bit 149"); // 149
                guid[2] = packet.ReadBit(); // 18
                guid[7] = packet.ReadBit(); // 23
                guid[1] = packet.ReadBit(); // 17
                var hasFallData = packet.ReadBit("Has fall data"); // 140
                var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
                var hasTrans = packet.ReadBit("Has transport"); // 104
                var hasSpline = !packet.ReadBit("has no Spline"); // 168
                var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
                guid[6] = packet.ReadBit(); // 22
                var Count = packet.ReadBits("Counter", 22); // 152
                var hasTime = !packet.ReadBit("Has no timestamp"); // 32
                guid[4] = packet.ReadBit(); // 20
                packet.ReadBit("bit 172"); // 172
                var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144

                if (hasTrans) // 104
                {
                    transportGuid[5] = packet.ReadBit(); // 61
                    transportGuid[1] = packet.ReadBit(); // 57
                    hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                    transportGuid[4] = packet.ReadBit(); // 60
                    transportGuid[2] = packet.ReadBit(); // 58
                    transportGuid[6] = packet.ReadBit(); // 62
                    transportGuid[0] = packet.ReadBit(); // 56
                    hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                    transportGuid[7] = packet.ReadBit(); // 63
                    transportGuid[3] = packet.ReadBit(); // 59
                }

                if (hasMovementFlags2) // 28
                    packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

                if (hasMovementFlags) // 24
                    packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

                if (hasFallData) // 140
                    hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

                packet.ResetBitReader(); // flush bits
                packet.ParseBitStream(guid, 6);

                if (Count > 0) // 152
                    for (var cnt = 0; cnt < Count; cnt++)
                    {
                        packet.ReadInt32("Dword 156", cnt); // 156
                    }

                packet.ParseBitStream(guid, 0, 5, 7, 1, 3, 4, 2);

                if (hasTrans) // 104
                {
                    var tpos = new Vector4();
                    packet.ReadSByte("Transport Seat"); // 80
                    packet.ReadXORByte(transportGuid, 3); // 59
                    if (hasTransTime3) // 100
                        packet.ReadUInt32("Transport Time 3"); // 96
                    packet.ReadXORByte(transportGuid, 2); // 58
                    packet.ReadXORByte(transportGuid, 7); // 63
                    packet.ReadXORByte(transportGuid, 0); // 56
                    tpos.Y = packet.ReadSingle(); // 68
                    packet.ReadXORByte(transportGuid, 4); // 60
                    packet.ReadXORByte(transportGuid, 5); // 61
                    if (hasTransTime2) // 92
                        packet.ReadUInt32("Transport Time 2"); // 88
                    packet.ReadXORByte(transportGuid, 1); // 57
                    tpos.X = packet.ReadSingle(); // 64
                    tpos.O = packet.ReadSingle(); // 76
                    packet.ReadXORByte(transportGuid, 6); // 62
                    packet.ReadUInt32("Transport Time"); // 84
                    tpos.Z = packet.ReadSingle(); // 72
                    packet.WriteGuid("Transport Guid", transportGuid);
                    packet.WriteLine("Transport Position: {0}", tpos);
                }

                if (hasPitch)
                    packet.ReadSingle("Pitch"); // 112

                if (hasO)
                    pos.O = packet.ReadSingle(); // 48

                if (hasSplineElev)
                    packet.ReadSingle("Spline elevation"); // 144

                if (hasFallData) // 140
                {
                    packet.ReadSingle("Vertical Speed"); // 120
                    if (hasFallDirection) // 136
                    {
                        packet.ReadSingle("Horizontal Speed"); // 132
                        packet.ReadSingle("Fall Cos"); // 128
                        packet.ReadSingle("Fall Sin"); // 124
                    }
                    packet.ReadUInt32("Fall time"); // 116
                }

                if (hasSpline) // 168
                    packet.ReadInt32("Spline"); // 168

                if (hasTime)
                    packet.ReadUInt32("Timestamp"); // 32

                packet.WriteGuid("Guid", guid);
                packet.WriteLine("Position: {0}", pos);
            }
            else
            {
                packet.WriteLine("              : SMSG_???");
                packet.AsHex();
                packet.ReadToEnd();
            }
        }

        [Parser(Opcode.MSG_MOVE_STOP)]
        public static void HandleMoveStop(Packet packet)
        {
            if (packet.Direction == Direction.ClientToServer)
            {
                var guid = new byte[8];
                var transportGuid = new byte[8];
                var hasTransTime2 = false;
                var hasTransTime3 = false;
                var hasFallDirection = false;
                var pos = new Vector4();

                pos.X = packet.ReadSingle(); // 36
                pos.Y = packet.ReadSingle(); // 40
                pos.Z = packet.ReadSingle(); // 44
                guid[5] = packet.ReadBit(); // 21
                guid[2] = packet.ReadBit(); // 18
                var hasFallData = packet.ReadBit("Has fall data"); // 140
                guid[0] = packet.ReadBit(); // 16
                packet.ReadBit("bit 172"); // 172
                packet.ReadBit("bit 148"); // 148
                var hasSpline = !packet.ReadBit("has no Spline"); // 168
                guid[1] = packet.ReadBit(); // 17
                var Count = packet.ReadBits("Counter", 22); // 152
                var hasPitch = !packet.ReadBit("Has no pitch"); // 112
                guid[3] = packet.ReadBit(); // 19
                guid[4] = packet.ReadBit(); // 20
                var hasTrans = packet.ReadBit("Has transport"); // 104
                packet.ReadBit("bit 149"); // 149
                guid[6] = packet.ReadBit(); // 22
                var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
                var hasTime = !packet.ReadBit("Has no timestamp"); // 32
                var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
                var hasO = !packet.ReadBit("Has no Orient"); // 48
                var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
                guid[7] = packet.ReadBit(); // 23

                if (hasTrans) // 104
                {
                    hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                    transportGuid[7] = packet.ReadBit(); // 63
                    transportGuid[4] = packet.ReadBit(); // 60
                    transportGuid[1] = packet.ReadBit(); // 57
                    transportGuid[0] = packet.ReadBit(); // 56
                    transportGuid[5] = packet.ReadBit(); // 61
                    transportGuid[2] = packet.ReadBit(); // 58
                    transportGuid[3] = packet.ReadBit(); // 59
                    hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                    transportGuid[6] = packet.ReadBit(); // 62
                }

                if (hasFallData) // 140
                    hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

                if (hasMovementFlags2) // 28
                    packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

                if (hasMovementFlags) // 24
                    packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

                packet.ResetBitReader();

                packet.ParseBitStream(guid, 0, 3);

                if (Count > 0) // 152
                    for (var cnt = 0; cnt < Count; cnt++)
                    {
                        packet.ReadInt32("Dword 156", cnt); // 156
                    }

                packet.ParseBitStream(guid, 6, 1, 4, 2, 5, 7);

                if (hasO)
                    pos.O = packet.ReadSingle(); // 48

                if (hasFallData) // 140
                {
                    packet.ReadSingle("Vertical Speed"); // 120
                    if (hasFallDirection) // 136
                    {
                        packet.ReadSingle("Horizontal Speed"); // 132
                        packet.ReadSingle("Fall Cos"); // 128
                        packet.ReadSingle("Fall Sin"); // 124
                    }
                    packet.ReadUInt32("Fall time"); // 116
                }

                if (hasSplineElev)
                    packet.ReadSingle("Spline elevation"); // 144

                if (hasTrans) // 104
                {
                    var tpos = new Vector4();
                    tpos.X = packet.ReadSingle(); // 64
                    packet.ReadUInt32("Transport Time"); // 84
                    packet.ReadXORByte(transportGuid, 3); // 59
                    tpos.O = packet.ReadSingle(); // 76
                    tpos.Y = packet.ReadSingle(); // 68
                    packet.ReadXORByte(transportGuid, 2); // 58
                    packet.ReadXORByte(transportGuid, 6); // 62
                    packet.ReadXORByte(transportGuid, 7); // 63
                    packet.ReadXORByte(transportGuid, 1); // 57
                    packet.ReadXORByte(transportGuid, 4); // 60
                    if (hasTransTime3) // 100
                        packet.ReadUInt32("Transport Time 3"); // 96
                    packet.ReadXORByte(transportGuid, 0); // 56
                    packet.ReadSByte("Transport Seat"); // 80
                    tpos.Z = packet.ReadSingle(); // 72
                    packet.ReadXORByte(transportGuid, 5); // 61
                    if (hasTransTime2) // 92
                        packet.ReadUInt32("Transport Time 2"); // 88
                    packet.WriteGuid("Transport Guid", transportGuid);
                    packet.WriteLine("Transport Position: {0}", tpos);
                }

                if (hasSpline) // 168
                    packet.ReadInt32("Spline"); // 168

                if (hasPitch)
                    packet.ReadSingle("Pitch"); // 112

                if (hasTime)
                    packet.ReadUInt32("Timestamp"); // 32

                packet.WriteGuid("Guid", guid);
                packet.WriteLine("Position: {0}", pos);
            }
            else
            {
                packet.WriteLine("              : SMSG_???");
                packet.ReadToEnd();
            }
        }

        [Parser(Opcode.MSG_MOVE_STOP_STRAFE)]
        public static void HandleMoveStopStrafe(Packet packet)
        {
            if (packet.Direction == Direction.ClientToServer)
            {
                var guid = new byte[8];
                var transportGuid = new byte[8];
                var hasTransTime2 = false;
                var hasTransTime3 = false;
                var hasFallDirection = false;
                var pos = new Vector4();

                pos.Z = packet.ReadSingle(); // 44
                pos.X = packet.ReadSingle(); // 36
                pos.Y = packet.ReadSingle(); // 40
                var hasFallData = packet.ReadBit("Has fall data"); // 140
                var hasO = !packet.ReadBit("Has no Orient"); // 48
                var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
                var hasTime = !packet.ReadBit("Has no timestamp"); // 32
                var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
                var hasSpline = !packet.ReadBit("has no Spline"); // 168
                guid[6] = packet.ReadBit(); // 22
                var hasTrans = packet.ReadBit("Has transport"); // 104
                packet.ReadBit("bit 172"); // 172
                var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
                guid[4] = packet.ReadBit(); // 20
                var hasPitch = !packet.ReadBit("Has no pitch"); // 112
                guid[5] = packet.ReadBit(); // 21
                guid[3] = packet.ReadBit(); // 19
                guid[2] = packet.ReadBit(); // 18
                var Count = packet.ReadBits("Counter", 22); // 152
                packet.ReadBit("bit 149"); // 149
                guid[7] = packet.ReadBit(); // 23
                guid[0] = packet.ReadBit(); // 16
                packet.ReadBit("bit 148"); // 148
                guid[1] = packet.ReadBit(); // 17

                if (hasTrans) // 104
                {
                    transportGuid[7] = packet.ReadBit(); // 63
                    hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                    transportGuid[3] = packet.ReadBit(); // 59
                    transportGuid[1] = packet.ReadBit(); // 57
                    transportGuid[6] = packet.ReadBit(); // 62
                    hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                    transportGuid[2] = packet.ReadBit(); // 58
                    transportGuid[5] = packet.ReadBit(); // 61
                    transportGuid[4] = packet.ReadBit(); // 60
                    transportGuid[0] = packet.ReadBit(); // 56
                }

                if (hasMovementFlags2) // 28
                    packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

                if (hasFallData) // 140
                    hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

                if (hasMovementFlags) // 24
                    packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

                packet.ResetBitReader();

                packet.ParseBitStream(guid, 5, 3);

                if (Count > 0) // 152
                    for (var cnt = 0; cnt < Count; cnt++)
                    {
                        packet.ReadInt32("Dword 156", cnt); // 156
                    }

                packet.ParseBitStream(guid, 2, 0, 1, 6, 4, 7);

                if (hasTrans) // 104
                {
                    var tpos = new Vector4();
                    packet.ReadXORByte(transportGuid, 0); // 56
                    if (hasTransTime3) // 100
                        packet.ReadUInt32("Transport Time 3"); // 96
                    packet.ReadXORByte(transportGuid, 1); // 57
                    packet.ReadXORByte(transportGuid, 6); // 62
                    packet.ReadUInt32("Transport Time"); // 84
                    tpos.Y = packet.ReadSingle(); // 68
                    tpos.Z = packet.ReadSingle(); // 72
                    packet.ReadXORByte(transportGuid, 4); // 60
                    if (hasTransTime2) // 92
                        packet.ReadUInt32("Transport Time 2"); // 88
                    packet.ReadXORByte(transportGuid, 3); // 59
                    packet.ReadSByte("Transport Seat"); // 80
                    tpos.X = packet.ReadSingle(); // 64
                    packet.ReadXORByte(transportGuid, 2); // 58
                    packet.ReadXORByte(transportGuid, 7); // 63
                    packet.ReadXORByte(transportGuid, 5); // 61
                    tpos.O = packet.ReadSingle(); // 76
                    packet.WriteGuid("Transport Guid", transportGuid);
                    packet.WriteLine("Transport Position: {0}", tpos);
                }

                if (hasO)
                    pos.O = packet.ReadSingle(); // 48

                if (hasSplineElev)
                    packet.ReadSingle("Spline elevation"); // 144

                if (hasTime)
                    packet.ReadUInt32("Timestamp"); // 32

                if (hasFallData) // 140
                {
                    if (hasFallDirection) // 136
                    {
                        packet.ReadSingle("Fall Sin"); // 124
                        packet.ReadSingle("Fall Cos"); // 128
                        packet.ReadSingle("Horizontal Speed"); // 132
                    }
                    packet.ReadUInt32("Fall time"); // 116
                    packet.ReadSingle("Vertical Speed"); // 120
                }

                if (hasPitch)
                    packet.ReadSingle("Pitch"); // 112

                if (hasSpline) // 168
                    packet.ReadInt32("Spline"); // 168

                packet.WriteGuid("Guid", guid);
                packet.WriteLine("Position: {0}", pos);
            }
            else
            {
                packet.WriteLine("              : SMSG_???");
                packet.ReadToEnd();
            }
        }

        [Parser(Opcode.CMSG_MOVE_TELEPORT_ACK)]
        public static void HandleMoveTeleportAck(Packet packet)
        {
            packet.ReadInt32("Time");
            packet.ReadInt32("Flags");

            var guid = packet.StartBitStream(0, 7, 3, 5, 4, 6, 1, 2);
            packet.ParseBitStream(guid, 4, 1, 6, 7, 0, 2, 5, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_UNK_00F2)]
        public static void HandleUnk00F2(Packet packet)
        {
            if (packet.Direction == Direction.ClientToServer)
            {
                var guid = new byte[8];
                var transportGuid = new byte[8];
                var hasTransTime2 = false;
                var hasTransTime3 = false;
                var hasFallDirection = false;
                var pos = new Vector4();

                packet.ReadInt32("MCounter"); // 176*4
                pos.X = packet.ReadSingle(); // 36
                pos.Y = packet.ReadSingle(); // 40
                pos.Z = packet.ReadSingle(); // 44
                guid[5] = packet.ReadBit(); // 21
                var hasSpline = !packet.ReadBit("has no Spline"); // 168
                var hasTime = !packet.ReadBit("Has no timestamp"); // 32
                var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
                packet.ReadBit("bit 172"); // 172
                var hasTrans = packet.ReadBit("Has transport"); // 104
                var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
                guid[3] = packet.ReadBit(); // 19
                var hasPitch = !packet.ReadBit("Has no pitch"); // 112
                var Count = packet.ReadBits("Counter", 22); // 152
                guid[1] = packet.ReadBit(); // 17
                var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
                guid[7] = packet.ReadBit(); // 23
                guid[6] = packet.ReadBit(); // 22
                guid[4] = packet.ReadBit(); // 20
                guid[0] = packet.ReadBit(); // 16
                var hasO = !packet.ReadBit("Has no Orient"); // 48
                guid[2] = packet.ReadBit(); // 18
                packet.ReadBit("bit 149"); // 149
                var hasFallData = packet.ReadBit("Has fall data"); // 140
                packet.ReadBit("bit 148"); // 148

                if (hasMovementFlags) // 24
                    packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

                if (hasTrans) // 104
                {
                    transportGuid[2] = packet.ReadBit(); // 58
                    hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                    transportGuid[3] = packet.ReadBit(); // 59
                    transportGuid[1] = packet.ReadBit(); // 57
                    transportGuid[4] = packet.ReadBit(); // 60
                    transportGuid[6] = packet.ReadBit(); // 62
                    transportGuid[0] = packet.ReadBit(); // 56
                    hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                    transportGuid[5] = packet.ReadBit(); // 61
                    transportGuid[7] = packet.ReadBit(); // 63
                }

                if (hasFallData) // 140
                    hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

                if (hasMovementFlags2) // 28
                    packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

                packet.ResetBitReader();

                packet.ParseBitStream(guid, 4, 1, 0, 2, 5, 3);

                if (Count > 0) // 152
                    for (var cnt = 0; cnt < Count; cnt++)
                    {
                        packet.ReadInt32("Dword 156", cnt); // 156
                    }

                packet.ParseBitStream(guid, 7, 6);

                if (hasTrans) // 104
                {
                    var tpos = new Vector4();
                    if (hasTransTime3) // 100
                        packet.ReadUInt32("Transport Time 3"); // 96
                    if (hasTransTime2) // 92
                        packet.ReadUInt32("Transport Time 2"); // 88
                    packet.ReadUInt32("Transport Time"); // 84
                    tpos.X = packet.ReadSingle(); // 64
                    tpos.Y = packet.ReadSingle(); // 68
                    packet.ReadXORByte(transportGuid, 7); // 63
                    packet.ReadSByte("Transport Seat"); // 80
                    packet.ReadXORByte(transportGuid, 1); // 57
                    tpos.Z = packet.ReadSingle(); // 72
                    tpos.O = packet.ReadSingle(); // 76
                    packet.ReadXORByte(transportGuid, 6); // 62
                    packet.ReadXORByte(transportGuid, 2); // 58
                    packet.ReadXORByte(transportGuid, 3); // 59
                    packet.ReadXORByte(transportGuid, 0); // 56
                    packet.ReadXORByte(transportGuid, 4); // 60
                    packet.ReadXORByte(transportGuid, 5); // 61
                    packet.WriteGuid("Transport Guid", transportGuid);
                    packet.WriteLine("Transport Position: {0}", tpos);
                }

                if (hasFallData) // 140
                {
                    if (hasFallDirection) // 136
                    {
                        packet.ReadSingle("Horizontal Speed"); // 132
                        packet.ReadSingle("Fall Cos"); // 128
                        packet.ReadSingle("Fall Sin"); // 124
                    }
                    packet.ReadSingle("Vertical Speed"); // 120
                    packet.ReadUInt32("Fall time"); // 116
                }

                if (hasSplineElev)
                    packet.ReadSingle("Spline elevation"); // 144

                if (hasPitch)
                    packet.ReadSingle("Pitch"); // 112

                if (hasTime)
                    packet.ReadUInt32("Timestamp"); // 32

                if (hasO)
                    pos.O = packet.ReadSingle(); // 48

                if (hasSpline) // 168
                    packet.ReadInt32("Spline"); // 168

                packet.WriteGuid("Guid", guid);
                packet.WriteLine("Position: {0}", pos);
            }
            else
            {
                packet.WriteLine("              : SMSG_???");
                packet.ReadToEnd();
            }
        }

        [Parser(Opcode.CMSG_UNK_09FB)] // sub_67B518
        public static void HandleUnk09FB(Packet packet)
        {
            if (packet.Direction == Direction.ClientToServer)
            {
                var guid = new byte[8];
                var transportGuid = new byte[8];
                var hasTransTime2 = false;
                var hasTransTime3 = false;
                var hasFallDirection = false;
                var pos = new Vector4();

                packet.ReadInt32();
                packet.ReadSingle();
                packet.ReadSingle("unk16");
                packet.ReadSingle();
                packet.ReadInt32("MCounter");
                packet.ReadToEnd();
            }
            else
            {
                packet.WriteLine("              : SMSG_SPELLHEALLOG");
                packet.ReadToEnd();
            }
        }

        [Parser(Opcode.CMSG_UNK_10F3)] // sub_6839FE
        public static void HandleUnk10F3(Packet packet)
        {
            if (packet.Direction == Direction.ClientToServer)
            {
                var guid = new byte[8];
                var transportGuid = new byte[8];
                var hasTransTime2 = false;
                var hasTransTime3 = false;
                var hasFallDirection = false;
                var pos = new Vector4();

                packet.ReadInt32("MCounter"); // 176*4
                pos.Y = packet.ReadSingle(); // 40
                pos.Z = packet.ReadSingle(); // 44
                pos.X = packet.ReadSingle(); // 36
                packet.ReadSingle("unk184");
                var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
                var hasSpline = !packet.ReadBit("has no Spline"); // 168
                guid[6] = packet.ReadBit(); // 22
                var hasTime = !packet.ReadBit("Has no timestamp"); // 32
                var hasFallData = packet.ReadBit("Has fall data"); // 140
                guid[5] = packet.ReadBit(); // 21
                var hasO = !packet.ReadBit("Has no Orient"); // 48
                packet.ReadBit("bit 149"); // 149
                guid[3] = packet.ReadBit(); // 19
                guid[2] = packet.ReadBit(); // 18
                guid[1] = packet.ReadBit(); // 17
                packet.ReadBit("bit 172"); // 172
                guid[4] = packet.ReadBit(); // 20
                var hasTrans = packet.ReadBit("Has transport"); // 104
                guid[7] = packet.ReadBit(); // 23
                guid[0] = packet.ReadBit(); // 16
                var Count = packet.ReadBits("Counter", 22); // 152
                var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
                var hasPitch = !packet.ReadBit("Has no pitch"); // 112
                var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
                packet.ReadBit("bit 148"); // 148

                if (hasTrans) // 104
                {
                    hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                    transportGuid[1] = packet.ReadBit(); // 57
                    transportGuid[7] = packet.ReadBit(); // 63
                    hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                    transportGuid[0] = packet.ReadBit(); // 56
                    transportGuid[6] = packet.ReadBit(); // 62
                    transportGuid[4] = packet.ReadBit(); // 60
                    transportGuid[3] = packet.ReadBit(); // 59
                    transportGuid[2] = packet.ReadBit(); // 58
                    transportGuid[5] = packet.ReadBit(); // 61
                }

                if (hasFallData) // 140
                    hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

                if (hasMovementFlags) // 24
                    packet.ReadEnum<MovementFlag>("Movement Flags", 30); // 24

                if (hasMovementFlags2) // 28
                    packet.ReadEnum<MovementFlagExtra>("Extra Movement Flags", 13); // 28

                packet.ResetBitReader();

                packet.ParseBitStream(guid, 0, 4, 2, 6, 1, 3, 7 ,5);

                if (Count > 0) // 152
                    for (var cnt = 0; cnt < Count; cnt++)
                    {
                        packet.ReadInt32("Dword 156", cnt); // 156
                    }

                if (hasSpline) // 168
                    packet.ReadInt32("Spline"); // 168

                if (hasSplineElev)
                    packet.ReadSingle("Spline elevation"); // 144

                if (hasTrans) // 104
                {
                    var tpos = new Vector4();
                    packet.ReadXORByte(transportGuid, 2); // 58
                    packet.ReadSByte("Transport Seat"); // 80
                    if (hasTransTime3) // 100
                        packet.ReadUInt32("Transport Time 3"); // 96
                    packet.ReadXORByte(transportGuid, 1); // 57
                    packet.ReadXORByte(transportGuid, 6); // 62
                    packet.ReadXORByte(transportGuid, 3); // 59
                    tpos.Z = packet.ReadSingle(); // 72
                    tpos.X = packet.ReadSingle(); // 64
                    packet.ReadXORByte(transportGuid, 7); // 63
                    tpos.O = packet.ReadSingle(); // 76
                    packet.ReadXORByte(transportGuid, 0); // 56
                    tpos.Y = packet.ReadSingle(); // 68
                    if (hasTransTime2) // 92
                        packet.ReadUInt32("Transport Time 2"); // 88
                    packet.ReadXORByte(transportGuid, 5); // 61
                    packet.ReadUInt32("Transport Time"); // 84
                    packet.ReadXORByte(transportGuid, 4); // 60
                    packet.WriteGuid("Transport Guid", transportGuid);
                    packet.WriteLine("Transport Position: {0}", tpos);
                }

                if (hasFallData) // 140
                {
                    if (hasFallDirection) // 136
                    {
                        packet.ReadSingle("Fall Cos"); // 128
                        packet.ReadSingle("Fall Sin"); // 124
                        packet.ReadSingle("Horizontal Speed"); // 132
                    }
                    packet.ReadSingle("Vertical Speed"); // 120
                    packet.ReadUInt32("Fall time"); // 116
                }

                if (hasO)
                    pos.O = packet.ReadSingle(); // 48

                if (hasTime)
                    packet.ReadUInt32("Timestamp"); // 32

                if (hasPitch)
                    packet.ReadSingle("Pitch"); // 112

                packet.WriteGuid("Guid", guid);
                packet.WriteLine("Position: {0}", pos);
            }
            else
            {
                packet.WriteLine("              : SMSG_???");
                packet.ReadToEnd();
            }
        }

        [Parser(Opcode.SMSG_BINDPOINTUPDATE)]
        public static void HandleBindPointUpdate(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_CLIENT_CONTROL_UPDATE)]
        public static void HandleClientControlUpdate(Packet packet)
        {
            var guid = new byte[8];
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("AllowMove");
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ParseBitStream(guid, 1, 5, 7, 4, 2, 6, 3, 0);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_ROOT)]
        public static void HandleMoveRoot(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_MOVE_SET_ACTIVE_MOVER)]
        public static void HandleMoveSetActiveMover(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_MOVE_SET_CAN_FLY)]
        public static void HandleSetCanFly(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_MOVE_SET_FLIGHT_SPEED)] // sub_C8A820
        public static void HandleMoveSetFlightSpeed(Packet packet)
        {
            packet.ReadSingle("Speed");
            packet.ReadInt32("MCounter");
            var guid = packet.StartBitStream(6, 5, 0, 4, 1, 7, 3, 2);
            packet.ParseBitStream(guid, 0, 7, 4, 5, 6, 2, 3, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_RUN_SPEED)]
        public static void HandleMoveSetRunSpeed(Packet packet)
        {
            var guid = packet.StartBitStream(1, 7, 4, 2, 5, 3, 6, 0);
            packet.ReadXORByte(guid, 1);
            packet.ReadInt32("MCounter");
            packet.ParseBitStream(guid, 7, 3, 0);
            packet.ReadSingle("Speed");
            packet.ParseBitStream(guid, 2, 4, 6, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_RUN_BACK_SPEED)] // sub_C8977A
        public static void HandleMoveSetRunBackSpeed(Packet packet)
        {
            var guid = packet.StartBitStream(7, 1, 0, 2, 4, 3, 6, 5);
            packet.ReadInt32("MCounter");
            packet.ParseBitStream(guid, 0, 3, 7, 5, 2, 4, 1);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_WALK_SPEED)] // sub_C8F849
        public static void HandleMoveSetWalkSpeed(Packet packet)
        {
            var guid = packet.StartBitStream(6, 7, 3, 1, 2, 0, 4, 5);
            packet.ParseBitStream(guid, 5, 6);
            packet.ReadInt32("MCounter");
            packet.ReadXORByte(guid, 4);
            packet.ReadSingle("Speed");
            packet.ParseBitStream(guid, 2, 3, 0, 1, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_TELEPORT)]
        public static void HandleMoveTeleport(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_MOVE_UNROOT)]
        public static void HandleMoveUnroot(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_MOVE_UNSET_CAN_FLY)]
        public static void HandleUnSetCanFly(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_PLAYER_MOVE)]
        public static void HandlePlayerMove(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_MONSTER_MOVE)]
        public static void HandleMonsterMove(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_SET_PHASE_SHIFT)]
        public static void HandlePhaseShift(Packet packet)
        {
            packet.ReadToEnd();
        }
    }
}
