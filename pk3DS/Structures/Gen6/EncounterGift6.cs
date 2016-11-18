using System.IO;

namespace pk3DS
{
    public class EncounterGift6
    {
        // All
        public byte[] Data;
        public bool ORAS;

        public ushort Species;
        public ushort u2;
        public byte Form;
        public byte Level;
        public sbyte Ability;
        public sbyte Nature;
        public byte Shiny, u9, uA, uB;
        public int HeldItem;
        public sbyte Gender;
        // ORAS
        public byte u11;
        public short MetLocation;
        public ushort Move;
        // All
        public sbyte[] IVs = new sbyte[6];
        // ORAS
        public byte[] ContestStats;
        public byte u22;
        // All
        public byte uLast;

        public EncounterGift6(byte[] data, bool oras)
        {
            Data = data;
            ORAS = oras;
            using (BinaryReader br = new BinaryReader(new MemoryStream(Data)))
            {
                Species = br.ReadUInt16();
                u2 = br.ReadUInt16();
                Form = br.ReadByte();
                Level = br.ReadByte();
                Shiny = br.ReadByte();
                Ability = br.ReadSByte();
                Nature = br.ReadSByte();
                u9 = br.ReadByte();
                uA = br.ReadByte();
                uB = br.ReadByte();
                HeldItem = br.ReadInt32();
                Gender = br.ReadSByte();

                if (ORAS)
                {
                    u11 = br.ReadByte();
                    MetLocation = br.ReadInt16();
                    Move = br.ReadUInt16();
                }

                for (int i = 0; i < 6; i++)
                    IVs[i] = br.ReadSByte();

                if (ORAS)
                {
                    ContestStats = br.ReadBytes(6);
                    u22 = br.ReadByte();
                }

                uLast = br.ReadByte();
            }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Species);
                bw.Write(u2);
                bw.Write(Form);
                bw.Write(Level);
                bw.Write(Shiny);
                bw.Write(Ability);
                bw.Write(Nature);
                bw.Write(u9);
                bw.Write(uA);
                bw.Write(uB);
                bw.Write(HeldItem);
                bw.Write(Gender);

                if (ORAS)
                {
                    bw.Write(u11);
                    bw.Write(MetLocation);
                    bw.Write(Move);
                }

                for (int i = 0; i < 6; i++)
                    bw.Write(IVs[i]);

                if (ORAS)
                {
                    bw.Write(ContestStats);
                    bw.Write(u22);
                }

                bw.Write(uLast);

                return ms.ToArray();
            }
        }
    }
}
