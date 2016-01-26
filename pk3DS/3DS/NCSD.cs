using System;
using System.Collections.Generic;
using System.Linq;

namespace CTR
{
    public class NCSD
    {
        public Header header;
        public CardInfoHeader cardinfoheader;
        public List<NCCH> NCCH_Array;
        public bool Card2;
        public byte[] Data;

        public class Header
        {
            public byte[] Signature; //Size 0x100;
            public uint Magic;
            public uint MediaSize;
            public ulong TitleId;
            //public byte[] padding; //Size: 0x10
            public NCCH_Meta[] OffsetSizeTable; //Size: 8
            //public byte[] padding; //Size: 0x28
            public byte[] flags; //Size: 0x8
            public ulong[] NCCHIdTable; //Size: 0x8;
            //public byte[] Padding2; //Size: 0x30;
        }
        public class CardInfoHeader
        {
            public uint WritableAddress;
            public uint CardInfoBitmask;
            public CardInfoNotes CIN;
            public ulong NCCH0TitleId;
            public ulong Reserved0;
            public byte[] InitialData; // Size: 0x30
            public byte[] Reserved1; // Size: 0xC0
            public byte[] NCCH0Header; // Size: 0x100

            public class CardInfoNotes
            {
                public byte[] Reserved0; // Size: 0xF8;
                public ulong MediaSizeUsed;
                public ulong Reserved1;
                public uint Unknown;
                public byte[] Reserved2; //Size: 0xC;
                public ulong CVerTitleId;
                public ushort CVerTitleVersion;
                public byte[] Reserved3; //Size: 0xCD6;
            }
        }
        public class NCCH_Meta
        {
            public uint Offset;
            public uint Size;
        }

        public ulong GetWritableAddress()
        {
            const ulong MEDIA_UNIT_SIZE = 0x200;
            return Card2
                ? Align(header.OffsetSizeTable[NCCH_Array.Count - 1].Offset * NCCH.MEDIA_UNIT_SIZE
                        + header.OffsetSizeTable[NCCH_Array.Count - 1].Size * NCCH.MEDIA_UNIT_SIZE + 0x1000, 0x10000) / MEDIA_UNIT_SIZE
                : 0x00000000FFFFFFFF;
        }
        public void BuildHeader()
        {
            Data = new byte[0x4000];
            Array.Copy(header.Signature, Data, 0x100);
            Array.Copy(BitConverter.GetBytes(header.Magic), 0, Data, 0x100, 4);
            Array.Copy(BitConverter.GetBytes(header.MediaSize), 0, Data, 0x104, 4);
            Array.Copy(BitConverter.GetBytes(header.TitleId), 0, Data, 0x108, 8);
            for (int i = 0; i < header.OffsetSizeTable.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(header.OffsetSizeTable[i].Offset), 0, Data, 0x120 + 8 * i, 4);
                Array.Copy(BitConverter.GetBytes(header.OffsetSizeTable[i].Size), 0, Data, 0x124 + 8 * i, 4);
            }
            Array.Copy(header.flags, 0, Data, 0x188, header.flags.Length);
            for (int i = 0; i < header.NCCHIdTable.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(header.NCCHIdTable[i]), 0, Data, 0x190 + 8 * i, 8);
            }
            //CardInfoHeader
            Array.Copy(BitConverter.GetBytes(cardinfoheader.WritableAddress), 0, Data, 0x200, 4);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CardInfoBitmask), 0, Data, 0x204, 4);
            Array.Copy(cardinfoheader.CIN.Reserved0, 0, Data, 0x208, cardinfoheader.CIN.Reserved0.Length);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CIN.MediaSizeUsed), 0, Data, 0x300, 8);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CIN.Reserved1), 0, Data, 0x308, 8);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CIN.Unknown), 0, Data, 0x310, 4);
            Array.Copy(cardinfoheader.CIN.Reserved2, 0, Data, 0x314, cardinfoheader.CIN.Reserved2.Length);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CIN.CVerTitleId), 0, Data, 0x320, 8);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CIN.CVerTitleVersion), 0, Data, 0x328, 2);
            Array.Copy(cardinfoheader.CIN.Reserved3, 0, Data, 0x32A, cardinfoheader.CIN.Reserved3.Length);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.NCCH0TitleId), 0, Data, 0x1000, 8);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.Reserved0), 0, Data, 0x1008, 8);
            Array.Copy(cardinfoheader.InitialData, 0, Data, 0x1010, cardinfoheader.InitialData.Length);
            Array.Copy(cardinfoheader.Reserved1, 0, Data, 0x1040, cardinfoheader.Reserved1.Length);
            Array.Copy(cardinfoheader.NCCH0Header, 0, Data, 0x1100, cardinfoheader.NCCH0Header.Length);
            Array.Copy(Enumerable.Repeat((byte)0xFF, 0x2E00).ToArray(), 0, Data, 0x1200, 0x2E00);
        }
        internal static ulong Align(ulong input, ulong alignsize)
        {
            ulong output = input;
            if (output % alignsize != 0)
            {
                output += alignsize - output % alignsize;
            }
            return output;
        }
    }
}
