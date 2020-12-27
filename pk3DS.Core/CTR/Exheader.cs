using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using pk3DS.Core.Properties;

namespace pk3DS.Core.CTR
{
    public class Exheader
    {
        public readonly byte[] Data;
        public readonly byte[] AccessDescriptor;
        public readonly ulong TitleID;

        public Exheader(string EXHEADER_PATH)
        {
            Data = File.ReadAllBytes(EXHEADER_PATH);
            AccessDescriptor = Data.Skip(0x400).Take(0x400).ToArray();
            Data = Data.Take(0x400).ToArray();
            TitleID = BitConverter.ToUInt64(Data, 0x200);
        }

        public byte[] GetSuperBlockHash()
        {
            SHA256Managed sha = new SHA256Managed();
            return sha.ComputeHash(Data, 0, 0x400);
        }

        public string GetSerial()
        {
            const string output = "CTR-P-";

            var RecognizedGames = new Dictionary<ulong, string[]>();
            string[] lines = Resources.ResourceManager.GetString("_3dsgames").Split('\n').ToArray();
            foreach (string l in lines)
            {
                string[] vars = l.Split('\t').ToArray();
                ulong titleid = Convert.ToUInt64(vars[0], 16);
                if (RecognizedGames.ContainsKey(titleid))
                {
                    char lc = RecognizedGames[titleid].ToArray()[0][3];
                    char lc2 = vars[1][3];
                    if (lc2 == 'A' || lc2 == 'E' || (lc2 == 'P' && lc == 'J')) //Prefer games in order US, PAL, JP
                    {
                        RecognizedGames[titleid] = vars.Skip(1).Take(2).ToArray();
                    }
                }
                else
                {
                    RecognizedGames.Add(titleid, vars.Skip(1).Take(2).ToArray());
                }
            }
            return output + RecognizedGames[TitleID][0];
        }

        public bool IsSupported()
        {
            return IsORAS() || IsXY() || IsUSUM() || IsSM();
        }

        public bool IsUSUM()
        {
            return (TitleID & 0xFFFFFFFF) >> 8 == 0x1B50 || (TitleID & 0xFFFFFFFF) >> 8 == 0x1B51;
        }

        public bool IsSM()
        {
            return (TitleID & 0xFFFFFFFF) >> 8 == 0x1648 || (TitleID & 0xFFFFFFFF) >> 8 == 0x175E;
        }

        public bool IsORAS()
        {
            return (TitleID & 0xFFFFFFFF) >> 8 == 0x11C5 || (TitleID & 0xFFFFFFFF) >> 8 == 0x11C4;
        }

        public bool IsXY()
        {
            return (TitleID & 0xFFFFFFFF) >> 8 == 0x55D || (TitleID & 0xFFFFFFFF) >> 8 == 0x55E;
        }

        public string GetPokemonSerial()
        {
            if (!IsSupported())
                return "CTR-P-XXXX";
            string name = ((TitleID & 0xFFFFFFFF) >> 8) switch
            {
                0x1B51 => "A2BA", // Ultra Moon
                0x1B50 => "A2AA", // Ultra Sun
                0x175E => "BNEA", // Moon
                0x1648 => "BNDA", // Sun
                0x11C5 => "ECLA", // Alpha Sapphire
                0x11C4 => "ECRA", // Omega Ruby
                0x055D => "EKJA", // X
                0x055E => "EK2A", // Y
                _ => "XXXX"
            };
            return "CTR-P-" + name;
        }
    }
}
