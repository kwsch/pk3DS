using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using pk3DS.Properties;

namespace CTR
{
    public class Exheader
    {
        public byte[] Data;
        public byte[] AccessDescriptor;
        public ulong TitleID;

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
                    char lc = RecognizedGames[titleid].ToArray()[0].ToCharArray()[3];
                    char lc2 = vars[1].ToCharArray()[3];
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

        public bool isPokemon()
        {
            return isORAS() || isXY();
        }
        public bool isORAS()
        {
            return ((TitleID & 0xFFFFFFFF) >> 8 == 0x11C5) || ((TitleID & 0xFFFFFFFF) >> 8 == 0x11C4);
        }
        public bool isXY()
        {
            return ((TitleID & 0xFFFFFFFF) >> 8 == 0x55D) || ((TitleID & 0xFFFFFFFF) >> 8 == 0x55E);
        }
        public string GetPokemonSerial()
        {
            if (!isPokemon())
                return "CTR-P-XXXX";
            string name;
            switch ((TitleID & 0xFFFFFFFF) >> 8)
            {
                case 0x11C5: //Alpha Sapphire
                    name = "ECLA";
                    break;
                case 0x11C4: //Omega Ruby
                    name = "ECRA";
                    break;
                case 0x55D: //X
                    name = "EKJA";
                    break;
                case 0x55E: //Y
                    name = "EK2A";
                    break;
                default:
                    name = "XXXX";
                    break;
            }
            return "CTR-P-" + name;
        }
    }
}
