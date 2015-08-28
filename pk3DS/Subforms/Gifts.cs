using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public partial class Gifts : Form
    {
        public Gifts()
        {
            specieslist[0] = "---";
            Array.Resize(ref specieslist, 722);
            if (!File.Exists(FieldPath))
            {
                Util.Error("CRO does not exist! Closing.", FieldPath);
                Close();
            }
            InitializeComponent();

            specieslist[0] = "---";
            abilitylist[0] = itemlist[0] = movelist[0] = "(None)"; // blank == -1

            CB_Species.Items.Clear();
            foreach (string s in specieslist)
                CB_Species.Items.Add(s);
            CB_HeldItem.Items.Clear();
            foreach (string s in itemlist)
                CB_HeldItem.Items.Add(s);

            loadData();
        }
        internal static string FieldPath = Path.Combine(Main.RomFSPath, "DllField.cro");
        private byte[] FieldData;
        private int fieldOffset = Main.oras ? 0xF906C : 0xF805C;
        private int fieldSize = Main.oras ? 0x24 : 0x18;
        private int count = Main.oras ? 0x25 : 0x13;
        private Gift[] GiftData;
        private string[] abilitylist = Main.getText((Main.oras) ? 37 : 34);
        private string[] movelist = Main.getText((Main.oras) ? 14 : 13);
        private string[] itemlist = Main.getText((Main.oras) ? 114 : 96);
        private string[] specieslist = Main.getText((Main.oras) ? 98 : 80);
        private void B_Save_Click(object sender, EventArgs e)
        {
            saveEntry();
            saveData();
            Close();
        }
        private void B_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void loadData()
        {
            FieldData = File.ReadAllBytes(FieldPath);
            GiftData = new Gift[count];
            LB_Gifts.Items.Clear();
            for (int i = 0; i < GiftData.Length; i++)
            {
                GiftData[i] = new Gift(FieldData.Skip(fieldOffset + i * fieldSize).Take(fieldSize).ToArray(), Main.oras);
                LB_Gifts.Items.Add(String.Format("{0} - {1}", i.ToString("00"), specieslist[GiftData[i].Species]));
            }
            loaded = true;
            LB_Gifts.SelectedIndex = 0;
        }
        private void saveData()
        {
            // Check to see if a starter has been modified right before we write data.
            bool starters = false;
            int[] entries = Main.oras
                ? new[]
                {
                    0, 1, 2, // Gen 3
                    28, 29, 30, // Gen 2
                    31, 32, 33, // Gen 4
                    34, 35, 36 // Gen 5
                }
                : new[]
                {
                    0, 1, 2, // Gen 6
                    3, 4, 5, // Gen 1
                };

            for (int i = 0; i < GiftData.Length; i++)
            {
                int offset = fieldOffset + i*fieldSize;

                // Check too see if starters got modified
                if (Array.IndexOf(entries, i) > - 1 && BitConverter.ToUInt16(FieldData, offset) != GiftData[i].Species)
                    starters = true;
                
                // Write new data
                Array.Copy(GiftData[i].Write(), 0, FieldData, offset, fieldSize);
            }

            if (starters) // are modified
                Util.Alert("Starters have been modified.", 
                    "Be sure to update the Starters in DllPoke3Select.cro by updating via the Starter Editor.");

            File.WriteAllBytes(FieldPath, FieldData);
        }

        private int entry = -1;
        private bool loaded;
        private void changeIndex(object sender, EventArgs e)
        {
            if (entry != -1) 
                saveEntry();
            if (!loaded)
                return;
            entry = LB_Gifts.SelectedIndex;
            loadEntry();
        }
        private void loadEntry()
        {
            CB_Species.SelectedIndex = GiftData[entry].Species;
            CB_HeldItem.SelectedIndex = GiftData[entry].HeldItem;
            NUD_Level.Value = GiftData[entry].Level;
            NUD_Form.Value = GiftData[entry].Form;

            NUD_IV0.Value = GiftData[entry].IVs[0];
            NUD_IV1.Value = GiftData[entry].IVs[1];
            NUD_IV2.Value = GiftData[entry].IVs[2];
            NUD_IV3.Value = GiftData[entry].IVs[3];
            NUD_IV4.Value = GiftData[entry].IVs[4];
            NUD_IV5.Value = GiftData[entry].IVs[5];
        }
        private void saveEntry()
        {
            GiftData[entry].Species = (ushort)CB_Species.SelectedIndex;
            GiftData[entry].HeldItem = CB_HeldItem.SelectedIndex;
            GiftData[entry].Level = (byte)NUD_Level.Value;
            GiftData[entry].Form = (byte)NUD_Form.Value;

            GiftData[entry].IVs[0] = (sbyte)NUD_IV0.Value;
            GiftData[entry].IVs[1] = (sbyte)NUD_IV1.Value;
            GiftData[entry].IVs[2] = (sbyte)NUD_IV2.Value;
            GiftData[entry].IVs[3] = (sbyte)NUD_IV3.Value;
            GiftData[entry].IVs[4] = (sbyte)NUD_IV4.Value;
            GiftData[entry].IVs[5] = (sbyte)NUD_IV5.Value;
        }

        public class Gift
        {
            // All
            public byte[] Data;
            public bool ORAS;

            public ushort Species;
            public ushort u2;
            public byte Form;
            public byte Level;
            public byte Gender;
            public short Nature;
            public byte u9, uA, uB;
            public int HeldItem;
            public byte Ability;
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

            public Gift(byte[] data, bool oras)
            {
                Data = data;
                ORAS = oras;
                using (BinaryReader br = new BinaryReader(new MemoryStream(Data)))
                {
                    Species = br.ReadUInt16();
                    u2 = br.ReadUInt16();
                    Form = br.ReadByte();
                    Level = br.ReadByte();
                    Gender = br.ReadByte();
                    Nature = br.ReadInt16();
                    u9 = br.ReadByte();
                    uA = br.ReadByte();
                    uB = br.ReadByte();
                    HeldItem = br.ReadInt32();
                    Ability = br.ReadByte();

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
                    bw.Write(Gender);
                    bw.Write(Nature);
                    bw.Write(u9);
                    bw.Write(uA);
                    bw.Write(uB);
                    bw.Write(HeldItem);
                    bw.Write(Ability);

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
}