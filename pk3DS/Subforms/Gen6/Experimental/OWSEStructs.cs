using pk3DS.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace pk3DS
{
    #region Game Related Classes
    public class MapMatrix
    {
        public ushort u0;
        public ushort u1;
        public ushort uL;
        public ushort Width, Height;
        private readonly int Area;
        public ushort[] EntryList;
        public Entry[] Entries;
        public Unknown[] Unknowns;

        public byte[] UnkData;

        public MapMatrix(byte[][] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data[0])))
            {
                u0 = br.ReadUInt16();
                u1 = br.ReadUInt16();
                Width = br.ReadUInt16();
                Height = br.ReadUInt16();

                Area = Width*Height;
                Entries = new Entry[Area];
                EntryList = new ushort[Area];
                for (int i = 0; i < Area; i++)
                    EntryList[i] = br.ReadUInt16();

                if (br.BaseStream.Position != br.BaseStream.Length)
                    uL = br.ReadUInt16();
            }
            if (data.Length > 1)
                ParseUnk(UnkData = data[1]);
        }

        public byte[] Write()
        {
            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(u0);
            bw.Write(Width);
            bw.Write(Height);
            foreach (ushort Entry in EntryList)
            {
                bw.Write(Entry);
            }

            bw.Write(uL);
            return ms.ToArray();
        }

        public Bitmap Preview(int Scale, int ColorShift)
        {
            // Require the entries to be defined in order to continue.
            if (Entries.Any(entry => entry == null))
            {
                // Do nothing; images are instead created with the standard dimensions and returned.
            }

            // Fetch Singular Images first
            Bitmap[] EntryImages = new Bitmap[Area];
            for (int i = 0; i < Area; i++)
            {
                EntryImages[i] = Entries[i] == null
                    ? new Bitmap(40 * Scale, 40 * Scale)
                    : Entries[i].Preview(Scale, ColorShift);
            }

            // Combine all images into one.
            Bitmap img = new Bitmap(EntryImages[0].Width * Width, EntryImages[0].Height * Height);

            using Graphics g = Graphics.FromImage(img);
            for (int i = 0; i < Area; i++)
                g.DrawImage(EntryImages[i],                         new Point(i * EntryImages[0].Width % img.Width, EntryImages[0].Height * (i / Width)));

            return img;
        }

        public class Entry
        {
            public Collision coll;

            public ushort Width, Height;
            private readonly int Area;
            public uint[] Tiles; // Certain bits?

            public Entry(byte[] data)
            {
                using BinaryReader br = new BinaryReader(new MemoryStream(data));
                Width = br.ReadUInt16();
                Height = br.ReadUInt16();
                Area = Width*Height;
                Tiles = new uint[Area];
                for (int i = 0; i < Area; i++)
                {
                    Tiles[i] = br.ReadUInt32();
                }
            }

            public byte[] Write()
            {
                using MemoryStream ms = new MemoryStream();
                using BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(Width);
                bw.Write(Height);
                foreach (uint Tile in Tiles)
                {
                    bw.Write(Tile);
                }

                return ms.ToArray();
            }

            public Bitmap Preview(int s, int ColorShift)
            {
                byte[] bmpData = BytePreview(s, ColorShift);
                Bitmap b = new Bitmap(Width * s, Height * s, PixelFormat.Format32bppArgb);
                BitmapData bData = b.LockBits(new Rectangle(0, 0, Width * s, Height * s), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                System.Runtime.InteropServices.Marshal.Copy(bmpData, 0, bData.Scan0, bmpData.Length);
                b.UnlockBits(bData);

                return b;
            }

            public byte[] BytePreview(int s, int ColorShift)
            {
                byte[] bmpData = new byte[4 * Width * Height * s * s];
                for (int i = 0; i < Area; i++)
                {
                    int X = i % 40;
                    int Y = i / 40;
                    uint colorValue = Tiles[i] == 0x01000021
                        ? 0xFF000000
                        : LCRNG32.Advance(Tiles[i], ColorShift) | 0xFF000000;

                    byte[] pixel = BitConverter.GetBytes(colorValue);
                    for (int x = 0; x < s * s; x++)
                    {
                        pixel.CopyTo(bmpData, 4 * ((((Y * s) + (x / s)) * Width * s) + (X * s) + (x % s)));
                    }
                }
                return bmpData;
            }
        }

        public class Collision
        {
            public string Magic;
            public int termOffset;
            public int U5D8;
            public byte[] UnknownBytes;
            public CollisionObject[] Map40;
            public int[] MapInts;
            public CollisionObject[] MapMisc;
            public string termMagic;
            public byte[] termData;

            public Collision(byte[] data)
            {
                using BinaryReader br = new BinaryReader(new MemoryStream(data));
                Magic = new string(br.ReadChars(4)); // Magic
                if (Magic != "coll")
                {
                    return; // all other properties are null
                }

                termOffset = br.ReadInt32();
                U5D8 = br.ReadInt32();
                UnknownBytes = br.ReadBytes(0x14);

                // Read 40 collision rectangles
                Map40 = new CollisionObject[40];
                for (int i = 0; i < Map40.Length; i++)
                {
                    Map40[i] = new CollisionObject(br.ReadBytes(0x10));
                }

                // Read 32 Int32s
                MapInts = new int[0x20];
                for (int i = 0; i < MapInts.Length; i++)
                {
                    MapInts[i] = br.ReadInt32();
                }

                // Read misc collision rectangles
                int ct = termOffset - (int)br.BaseStream.Position + 0x10;
                MapMisc = new CollisionObject[ct/0x10];
                for (int i = 0; i < MapMisc.Length; i++)
                {
                    MapMisc[i] = new CollisionObject(br.ReadBytes(0x10));
                }

                // Read Term
                termMagic = new string(br.ReadChars(4));
                if (termMagic != "term")
                {
                    return; // all other properties are null
                }

                // Read the rest of the data....
                termData = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
            }

            public class CollisionObject
            {
                private readonly float _0;
                private readonly float _1;
                private readonly float _2;
                private readonly float _3; // rarely used

                // I don't even know...
                public float F1 => _0 / 2;
                public float F2 => _1 * 80;
                public float F3 => _2 / 2;
                public float F4 => _3 / 1;

                public CollisionObject(byte[] data)
                {
                    _0 = BitConverter.ToSingle(data, 0x0);
                    _1 = BitConverter.ToSingle(data, 0x4);
                    _2 = BitConverter.ToSingle(data, 0x8);
                    _3 = BitConverter.ToSingle(data, 0xC);
                }

                public override string ToString()
                {
                    return string.Join(", ", F1.ToString(), F2.ToString(), F3.ToString(), F4.ToString());
                }
            }
        }

        public string Unk2String()
        {
            return Unknowns.Aggregate("", (current, l) => current + $"{l.Direction}: {l.P1,3} {l.P2,3} {l.P3,3} {l.P4,3}{Environment.NewLine,3}");
        }

        private void ParseUnk(byte[] data)
        {
            List<Unknown> unk = new List<Unknown>();
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            do
            {
                var obj = new Unknown
                {
                    Direction = br.ReadUInt32(),
                    _1 = br.ReadSingle(),
                    _2 = br.ReadSingle(),
                    _3 = br.ReadSingle(),
                    _4 = br.ReadSingle(),
                };
                unk.Add(obj);
            } while (unk.Last().Direction != 0);

            unk.RemoveAt(unk.Count-1);
            Unknowns = unk.ToArray();
        }

        public class Unknown
        {
            public uint Direction;
            public float _1;
            public float _2;
            public float _3;
            public float _4;

            public int P1 => (int)_1 / 18;
            public int P2 => (int)_2 / 18;
            public int P3 => (int)_3 / 18;
            public int P4 => (int)_4 / 18;
        }
    }

    public class ZoneData
    {
        internal const int Size = 0x38;
        public readonly byte[] Data;

        public byte MapType { get => Data[0]; set => Data[0] = value; }
        public byte MapMove { get => Data[1]; set => Data[1] = value; }
        public int MapArea { get => BitConverter.ToUInt16(Data, 0x02); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x02); }
        public int MapMatrix { get => BitConverter.ToUInt16(Data, 0x04); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x04); }
        public int TextFile { get => BitConverter.ToUInt16(Data, 0x06); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x06); }

        public uint BGMSpring { get => BitConverter.ToUInt32(Data, 0x08); set => BitConverter.GetBytes(value).CopyTo(Data, 0x08); }
        public uint BGMSummer { get => BitConverter.ToUInt32(Data, 0x0C); set => BitConverter.GetBytes(value).CopyTo(Data, 0x0C); }
        public uint BGMAutumn { get => BitConverter.ToUInt32(Data, 0x10); set => BitConverter.GetBytes(value).CopyTo(Data, 0x10); }
        public uint BGMWinter { get => BitConverter.ToUInt32(Data, 0x14); set => BitConverter.GetBytes(value).CopyTo(Data, 0x14); }

        public int ScriptFile { get => BitConverter.ToUInt16(Data, 0x18); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x18); }
        public int TownMapGroup { get => BitConverter.ToUInt16(Data, 0x1A); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x1A); }

        public int ParentMap // 0x1C - low 10 bits
        {
            get => BitConverter.ToUInt16(Data, 0x1C) & 0x3FF;
            set => BitConverter.GetBytes((ushort)(value | (BitConverter.ToUInt16(Data, 0x1C) & ~0x3FF))).CopyTo(Data, 0x1C);
        }

        public int OLValue // 0x1C - high 6 bits
        {
            get => BitConverter.ToUInt16(Data, 0x1C) >> 10;
            set => BitConverter.GetBytes((ushort)((value << 10) | ParentMap)).CopyTo(Data, 0x1C);
        }

        // 5,1,1,7
        public int Weather // 0x1E, low 5
        {
            get => BitConverter.ToUInt16(Data, 0x1E) & 0x1F;
            set => BitConverter.GetBytes((ushort)(value | (BitConverter.ToUInt16(Data, 0x1E) & ~0x1F))).CopyTo(Data, 0x1E);
        }

        public bool IsSkyBoxEnable { get => ((Data[0x1E] >> 5) & 1) == 1; set => Data[0x1E] = (byte)((Data[0x1E] & ~0x20) | (value ? 1 << 5 : 0)); }
        public bool IsRollerSkateEnable { get => ((Data[0x1E] >> 6) & 1) == 1; set => Data[0x1E] = (byte)((Data[0x1E] & ~0x40) | (value ? 1 << 6 : 0)); }

        public int BattleBackground // 7-13
        {
            get => (BitConverter.ToUInt16(Data, 0x1E) >> 7) & 0x7F;
            set => BitConverter.GetBytes((ushort)(((value & 0x7F) << 7) | (BitConverter.ToUInt16(Data, 0x1E) & ~0x3F80))).CopyTo(Data, 0x1E);
        }

        private uint Unk_20 { get => BitConverter.ToUInt32(Data, 0x20); set => BitConverter.GetBytes(value).CopyTo(Data, 0x20); }
        public uint MapChange { get => Unk_20 & 0x1Fu; set => Unk_20 = (Unk_20 & ~0x1Fu) | value; }
        // ??? 5
        public bool IsBicycleEnable { get => ((Unk_20 >> 10) & 1) == 1; set => Unk_20 = (Unk_20 & ~(1u << 10)) | (value ? 1u << 10 : 0); }
        public bool IsRunEnable { get => ((Unk_20 >> 11) & 1) == 1; set => Unk_20 = (Unk_20 & ~(1u << 11)) | (value ? 1u << 11 : 0); }
        public bool IsEscapeRopeEnable { get => ((Unk_20 >> 12) & 1) == 1; set => Unk_20 = (Unk_20 & ~(1u << 12)) | (value ? 1u << 12 : 0); }
        public bool IsFlyEnable { get => ((Unk_20 >> 13) & 1) == 1; set => Unk_20 = (Unk_20 & ~(1u << 13)) | (value ? 1u << 13 : 0); }
        public bool IsBGM { get => ((Unk_20 >> 14) & 1) == 1; set => Unk_20 = (Unk_20 & ~(1u << 14)) | (value ? 1u << 14 : 0); }
        public bool IsUnk { get => ((Unk_20 >> 15) & 1) == 1; set => Unk_20 = (Unk_20 & ~(1u << 15)) | (value ? 1u << 15 : 0); }

        // unused - camera
        public ushort Camera1 { get => BitConverter.ToUInt16(Data, 0x22); set => BitConverter.GetBytes(value).CopyTo(Data, 0x22); }

        public ushort Camera2 { get => BitConverter.ToUInt16(Data, 0x24); set => BitConverter.GetBytes(value).CopyTo(Data, 0x24); }
        public uint CameraFlags { get => BitConverter.ToUInt32(Data, 0x26); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x26); }

        private short X { get => BitConverter.ToInt16(Data, 0x2C); set => BitConverter.GetBytes(value).CopyTo(Data, 0x2C); }
        public short Z { get => BitConverter.ToInt16(Data, 0x2E); set => BitConverter.GetBytes(value).CopyTo(Data, 0x2E); }
        private short Y { get => BitConverter.ToInt16(Data, 0x30); set => BitConverter.GetBytes(value).CopyTo(Data, 0x30); }
        private short X2 { get => BitConverter.ToInt16(Data, 0x32); set => BitConverter.GetBytes(value).CopyTo(Data, 0x32); }
        public short Z2 { get => BitConverter.ToInt16(Data, 0x34); set => BitConverter.GetBytes(value).CopyTo(Data, 0x34); }
        private short Y2 { get => BitConverter.ToInt16(Data, 0x36); set => BitConverter.GetBytes(value).CopyTo(Data, 0x36); }

        public float PX { get => (float)X / 18; set => X = (short)(18 * value); }
        public float PY { get => (float)Y / 18; set => Y = (short)(18 * value); }
        public float PX2 { get => (float)X2 / 18; set => X2 = (short)(18 * value); }
        public float PY2 { get => (float)Y2 / 18; set => Y2 = (short)(18 * value); }

        public ZoneData(byte[] data)
        {
            if (data.Length != Size)
            {
                return;
            }

            Data = data;
        }

        public byte[] Write()
        {
            return Data;
        }
    }

    public class Zone
    {
        public readonly ZoneData ZD;
        public ZoneEntities Entities;
        public readonly ZoneScript MapScript;
        public readonly ZoneEncounters Encounters;
        public readonly ZoneUnknown File5;

        public Zone(byte[][] Zone)
        {
            // A ZO is comprised of 4-5 files.

            // Array 0 is [Map Info]
            ZD = new ZoneData(Zone[0]);
            // Array 1 is [Overworld Entities & their Scripts]
            Entities = new ZoneEntities(Zone[1]);
            // Array 2 is [Map Script]
            MapScript = new ZoneScript(Zone[2]);
            // Array 3 is [Wild Encounters]
            Encounters = new ZoneEncounters(Zone[3]);
            // Array 4 is [???] - May not be present in all.
            if (Zone.Length <= 4)
            {
                return;
            }

            File5 = new ZoneUnknown(Zone[4]);
        }

        public byte[][] Write()
        {
            byte[][] Zone = new byte[File5 != null ? 5 : 4][];
            Zone[0] = ZD.Data;
            Zone[1] = Entities.Write();
            Zone[2] = MapScript.Write();
            Zone[3] = Encounters.Write();
            if (Zone.Length <= 4)
            {
                return Zone;
            }

            Zone[4] = File5.Write();
            return Zone;
        }

        public class ZoneEntities
        {
            public byte[] Data;

            public int Length;
            public int FurnitureCount, NPCCount, WarpCount, TriggerCount, UnknownCount;
            public EntityFurniture[] Furniture;
            public EntityNPC[] NPCs;
            public EntityWarp[] Warps;
            public EntityTrigger1[] Triggers1;
            public EntityTrigger2[] Triggers2;

            public Script Script;

            public ZoneEntities(byte[] data)
            {
                Data = data;

                using BinaryReader br = new BinaryReader(new MemoryStream(data));
                // Load Header
                Length = br.ReadInt32();
                Furniture = new EntityFurniture[FurnitureCount = br.ReadByte()];
                NPCs = new EntityNPC[NPCCount = br.ReadByte()];
                Warps = new EntityWarp[WarpCount = br.ReadByte()];
                Triggers1 = new EntityTrigger1[TriggerCount = br.ReadByte()];
                Triggers2 = new EntityTrigger2[UnknownCount = br.ReadInt32()]; // not sure if there's other types or if the remaining 3 bytes are padding.

                // Load Entitites
                for (int i = 0; i < FurnitureCount; i++)
                {
                    Furniture[i] = new EntityFurniture(br.ReadBytes(EntityFurniture.Size));
                }

                for (int i = 0; i < NPCCount; i++)
                {
                    NPCs[i] = new EntityNPC(br.ReadBytes(EntityNPC.Size));
                }

                for (int i = 0; i < WarpCount; i++)
                {
                    Warps[i] = new EntityWarp(br.ReadBytes(EntityWarp.Size));
                }

                for (int i = 0; i < TriggerCount; i++)
                {
                    Triggers1[i] = new EntityTrigger1(br.ReadBytes(EntityTrigger1.Size));
                }

                for (int i = 0; i < UnknownCount; i++)
                {
                    Triggers2[i] = new EntityTrigger2(br.ReadBytes(EntityTrigger2.Size));
                }

                // Load Script Data
                int len = br.ReadInt32();
                br.BaseStream.Position -= 4;
                Script = new Script(br.ReadBytes(len));
            }

            public byte[] Write()
            {
                byte[] F = new byte[Furniture.Length * EntityFurniture.Size];
                for (int i = 0; i < Furniture.Length; i++)
                {
                    Furniture[i].Write().CopyTo(F, i * EntityFurniture.Size);
                }

                byte[] N = new byte[NPCs.Length * EntityNPC.Size];
                for (int i = 0; i < NPCs.Length; i++)
                {
                    NPCs[i].Write().CopyTo(N, i * EntityNPC.Size);
                }

                byte[] W = new byte[Warps.Length * EntityWarp.Size];
                for (int i = 0; i < Warps.Length; i++)
                {
                    Warps[i].Write().CopyTo(W, i * EntityWarp.Size);
                }

                byte[] T = new byte[Triggers1.Length * EntityTrigger1.Size];
                for (int i = 0; i < Triggers1.Length; i++)
                {
                    Triggers1[i].Write().CopyTo(T, i * EntityTrigger1.Size);
                }

                byte[] U = new byte[Triggers2.Length * EntityTrigger2.Size];
                for (int i = 0; i < Triggers2.Length; i++)
                {
                    Triggers2[i].Write().CopyTo(U, i * EntityTrigger2.Size);
                }

                // Assemble entity information
                byte[] OWEntities = F.Concat(N).Concat(W).Concat(T).Concat(U).ToArray();
                byte[] EntityLength = BitConverter.GetBytes(8 + OWEntities.Length);
                byte[] EntityCounts = {(byte)Furniture.Length, (byte)NPCs.Length, (byte)Warps.Length, (byte)Triggers1.Length, (byte)Triggers2.Length, 0, 0, 0 };

                // Reassemble NPC portion
                byte[] OWEntityData = EntityLength.Concat(EntityCounts).Concat(OWEntities).ToArray();

                // Reassemble Script portion
                byte[] OWScriptData = Script.Write();

                byte[] finalData = OWEntityData.Concat(OWScriptData).ToArray();

                // Add padding zeroes if required (yield size % 4 == 0)
                if (finalData.Length % 4 != 0)
                {
                    Array.Resize(ref finalData, finalData.Length + 4 - (finalData.Length % 4));
                }

                return finalData;
            }

            // Entity Classes
            public class EntityFurniture
            {
                // Usable Attributes
                public int Script { get => BitConverter.ToUInt16(Raw, 0x00); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x00); }

                public int U2 { get => BitConverter.ToUInt16(Raw, 0x02); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x02); }
                public int U4 { get => BitConverter.ToUInt16(Raw, 0x04); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x04); }
                public int U6 { get => BitConverter.ToUInt16(Raw, 0x06); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x06); }

                // Coordinates have some upper-bit usage it seems...
                public int X { get => BitConverter.ToUInt16(Raw, 0x08); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x08); }
                public int Y { get => BitConverter.ToUInt16(Raw, 0x0A); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0A); }
                // Next two bytes should be dealing with furniture width?
                public int WX { get => BitConverter.ToInt16(Raw, 0x0C); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x0C); }
                public int WY { get => BitConverter.ToInt16(Raw, 0x0E); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x0E); }

                public int U10 { get => BitConverter.ToInt32(Raw, 0x10); set => BitConverter.GetBytes(value).CopyTo(Raw, 0x10); }

                public byte[] Raw;
                public byte[] OriginalData;
                internal const byte Size = 0x14;

                public EntityFurniture(byte[] data = null)
                {
                    Raw = data ?? new byte[Size];
                    OriginalData = (byte[])Raw.Clone();
                }

                public byte[] Write()
                {
                    return Raw;
                }
            }

            public class EntityNPC
            {
                // Usable Attributes
                public int ID { get => BitConverter.ToUInt16(Raw, 0x00); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x00); }
                public int Model { get => BitConverter.ToUInt16(Raw, 0x02); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x02); }
                public int MovePermissions { get => BitConverter.ToUInt16(Raw, 0x04); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x04); }
                public int MovePermissions2 { get => BitConverter.ToUInt16(Raw, 0x06); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x06); }
                public int SpawnFlag { get => BitConverter.ToUInt16(Raw, 0x08); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x08); }
                public int Script { get => BitConverter.ToUInt16(Raw, 0x0A); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0A); }
                public int FaceDirection { get => BitConverter.ToUInt16(Raw, 0x0C); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0C); }
                public int SightRange { get => BitConverter.ToUInt16(Raw, 0x0E); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0E); }

                // XY Only
                public int U10 { get => BitConverter.ToUInt16(Raw, 0x10); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x10); }
                public int U12 { get => BitConverter.ToUInt16(Raw, 0x12); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x12); }

                // Shorts
                public int U14 { get => BitConverter.ToInt16(Raw, 0x14); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x14); }
                public int U16 { get => BitConverter.ToInt16(Raw, 0x16); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x16); }
                // Negative only in X/Y... seeing behind them? Might be projection of an interaction area.
                public int U18 { get => BitConverter.ToInt16(Raw, 0x18); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x18); }
                public int U1A { get => BitConverter.ToInt16(Raw, 0x1A); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x1A); }

                // WalkArea Leashes (?): If these are for NPCs that walk in an area, I'm not sure if there's a direction specified.
                // Set L# to -1 to turn off.
                public int L1 { get => BitConverter.ToInt16(Raw, 0x1C); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x1C); }
                public int L2 { get => BitConverter.ToInt16(Raw, 0x1E); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x1E); }
                public int L3 { get => BitConverter.ToInt16(Raw, 0x20); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x20); }
                // Leash Direction? Only used when an area is specified.
                public int LDir { get => BitConverter.ToUInt16(Raw, 0x22); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x22); }

                // 0x24-0x25 is Unused in OR/AS, rarely 1 in XY
                public int U24 { get => BitConverter.ToUInt16(Raw, 0x24); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x24); }
                // 0x26-0x27 is Unused in OR/AS

                // Highest bits for X/Y seem to be fractions of a coordinate?
                public int X { get => BitConverter.ToUInt16(Raw, 0x28); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x28); }
                public int Y { get => BitConverter.ToUInt16(Raw, 0x2A); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x2A); }

                // -360, 360 ????
                public float Degrees { get => BitConverter.ToSingle(Raw, 0x2C); set => BitConverter.GetBytes(value).CopyTo(Raw, 0x2C); }
                public float Deg18 => Degrees/18;

                public byte[] Raw;
                public byte[] OriginalData;
                internal const byte Size = 0x30;

                public EntityNPC(byte[] data = null)
                {
                    Raw = data ?? new byte[Size];
                    OriginalData = (byte[])Raw.Clone();
                }

                public byte[] Write()
                {
                    return Raw;
                }
            }

            public class EntityWarp
            {
                // Usable Attributes
                public int DestinationMap { get => BitConverter.ToUInt16(Raw, 0x00); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x00); }
                public int DestinationTileIndex { get => BitConverter.ToUInt16(Raw, 0x02); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x02); }

                // Not sure if these are widths or face direction
                public byte FaceDirection { get => Raw[0x04]; set => Raw[0x4] = value; }
                public byte SceneTransitionEffect { get => Raw[0x05]; set => Raw[0x5] = value; }

                // Either 0 or 1, only in X/Y
                // 0: use x,z,y
                // 1: use x,y,{unused}
                public ushort CoordinateType { get => BitConverter.ToUInt16(Raw, 0x06); set => BitConverter.GetBytes(value).CopyTo(Raw, 0x06); }
                // Coordinates have some upper-bit usage it seems...
                public short X { get => BitConverter.ToInt16(Raw, 0x08); set => BitConverter.GetBytes(value).CopyTo(Raw, 0x08); }
                public short Z { get => BitConverter.ToInt16(Raw, 0x0A); set => BitConverter.GetBytes(value).CopyTo(Raw, 0x0A); }
                public short Y { get => BitConverter.ToInt16(Raw, 0x0C); set => BitConverter.GetBytes(value).CopyTo(Raw, 0x0C); }

                public decimal PX => (decimal)X / 18;
                public decimal PY => (decimal)Y / 18;

                // Stretches RIGHT
                public short Width { get => BitConverter.ToInt16(Raw, 0x0E); set => BitConverter.GetBytes(value).CopyTo(Raw, 0x0E); }
                // Stretches DOWN
                public short Height { get => BitConverter.ToInt16(Raw, 0x10); set => BitConverter.GetBytes(value).CopyTo(Raw, 0x10); }
                // 0-bidirectional
                // 1-send only
                // 2-receive only
                public ushort Directionality { get => BitConverter.ToUInt16(Raw, 0x12); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x12); }

                // 0x14-0x15 Unused
                // 0x16-0x17 Unused

                public byte[] Raw;
                public byte[] OriginalData;
                internal const byte Size = 0x18;

                public EntityWarp(byte[] data = null)
                {
                    Raw = data ?? new byte[Size];
                    OriginalData = (byte[])Raw.Clone();
                }

                public byte[] Write()
                {
                    return Raw;
                }
            }

            public class EntityTrigger1
            {
                // Usable Attributes
                public int Script { get => BitConverter.ToUInt16(Raw, 0x00); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x00); }
                public int U2 { get => BitConverter.ToUInt16(Raw, 0x02); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x02); }
                public int Constant { get => BitConverter.ToUInt16(Raw, 0x04); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x04); }

                // 0 or 1 for type2, 0/5-8 for type1
                public int U6 { get => BitConverter.ToUInt16(Raw, 0x06); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x06); }
                // 0 or 1, always 0 in ORAS
                public int U8 { get => BitConverter.ToUInt16(Raw, 0x08); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x08); }

                // 0x0A-0x0B unused

                public int X { get => BitConverter.ToUInt16(Raw, 0x0C); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0C); }
                public int Y { get => BitConverter.ToUInt16(Raw, 0x0E); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0E); }

                public int Width { get => BitConverter.ToInt16(Raw, 0x10); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x10); }
                public int Height { get => BitConverter.ToInt16(Raw, 0x12); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x12); }
                public int U14 { get => BitConverter.ToInt16(Raw, 0x14); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x14); }
                public int U16 { get => BitConverter.ToInt16(Raw, 0x16); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x16); }

                public byte[] Raw;
                public byte[] OriginalData;
                internal const byte Size = 0x18;

                public EntityTrigger1(byte[] data = null)
                {
                    Raw = data ?? new byte[Size];
                    OriginalData = (byte[])Raw.Clone();
                }

                public byte[] Write()
                {
                    return Raw;
                }
            }

            public class EntityTrigger2
            {
                // Usable Attributes
                public int Script { get => BitConverter.ToUInt16(Raw, 0x00); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x00); }
                public int U2 { get => BitConverter.ToUInt16(Raw, 0x02); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x02); }
                public int Constant { get => BitConverter.ToUInt16(Raw, 0x04); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x04); }

                // 0 or 1 for type2, 0/5-8 for type1
                public int U6 { get => BitConverter.ToUInt16(Raw, 0x06); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x06); }
                // 0 or 1, always 0 in ORAS
                public int U8 { get => BitConverter.ToUInt16(Raw, 0x08); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x08); }

                // 0x0A-0x0B unused

                public int X { get => BitConverter.ToUInt16(Raw, 0x0C); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0C); }
                public int Y { get => BitConverter.ToUInt16(Raw, 0x0E); set => BitConverter.GetBytes((ushort)value).CopyTo(Raw, 0x0E); }

                public int Width { get => BitConverter.ToInt16(Raw, 0x10); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x10); }
                public int Height { get => BitConverter.ToInt16(Raw, 0x12); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x12); }
                public int U14 { get => BitConverter.ToInt16(Raw, 0x14); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x14); }
                public int U16 { get => BitConverter.ToInt16(Raw, 0x16); set => BitConverter.GetBytes((short)value).CopyTo(Raw, 0x16); }

                public byte[] Raw;
                public byte[] OriginalData;
                internal const byte Size = 0x18;

                public EntityTrigger2(byte[] data = null)
                {
                    Raw = data ?? new byte[Size];
                    OriginalData = (byte[])Raw.Clone();
                }

                public byte[] Write() => Raw;
            }
        }

        public class ZoneScript
        {
            public byte[] Data; // File details unknown.
            public Script Script;

            public ZoneScript(byte[] data)
            {
                Data = data;
                Script = new Script(data);
            }

            public byte[] Write()
            {
                Data = Script.Write();
                return Data;
            }
        }

        public class ZoneEncounters
        {
            public byte[] Data; // File details unknown.
            public byte[] Header;
            public EncounterSet[] Encounters;

            public ZoneEncounters(byte[] data)
            {
                Data = data;

                using BinaryReader br = new BinaryReader(new MemoryStream(data));
                Header = br.ReadBytes(0x10);
                Encounters = new EncounterSet[(int)(br.BaseStream.Length - br.BaseStream.Position)/4];
                for (int i = 0; i < Encounters.Length; i++)
                {
                    Encounters[i] = new EncounterSet(br.ReadBytes(4));
                }
            }

            public byte[] Write()
            {
                byte[] data = Header; // Start with the header data, then concat every encounter in afterwards.
                return Encounters.Aggregate(data, (current, t) => current.Concat(t.Write()).ToArray());
            }

            public class EncounterSet
            {
                public int Species;
                public int Form;
                public byte LevelMin, LevelMax;

                public EncounterSet(byte[] data)
                {
                    using BinaryReader br = new BinaryReader(new MemoryStream(data));
                    ushort SpecForm = br.ReadUInt16();
                    Species = SpecForm & 0x7FF;
                    Form = SpecForm >> 11;
                    LevelMin = br.ReadByte();
                    LevelMax = br.ReadByte();
                }

                public byte[] Write()
                {
                    using MemoryStream ms = new MemoryStream();
                    using BinaryWriter bw = new BinaryWriter(ms);
                    bw.Write((ushort)(Species | (Form << 11)));
                    bw.Write(LevelMin);
                    bw.Write(LevelMax);
                    return ms.ToArray();
                }
            }
        }

        public class ZoneUnknown
        {
            public byte[] FileData; // File details unknown.

            public ZoneUnknown(byte[] data)
            {
                FileData = data;
            }

            public byte[] Write()
            {
                return FileData;
            }
        }
    }

    #endregion
}
