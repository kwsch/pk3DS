using System.IO;

namespace pk3DS
{
    public class MegaEvolutions
    {
        public ushort[] Form, Method, Argument, u6;
        public MegaEvolutions(byte[] data)
        {
            if (data.Length < 0x10 || data.Length % 8 != 0) return;
            Form = new ushort[data.Length / 8];
            Method = new ushort[data.Length / 8];
            Argument = new ushort[data.Length / 8];
            u6 = new ushort[data.Length / 8];
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
                for (int i = 0; i < Form.Length; i++)
                {
                    Form[i] = br.ReadUInt16();
                    Method[i] = br.ReadUInt16();
                    Argument[i] = br.ReadUInt16();
                    u6[i] = br.ReadUInt16();
                }
        }
        public byte[] Write()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                for (int i = 0; i < Form.Length; i++)
                {
                    if (Method[i] == 0)
                    { Form[i] = Argument[i] = 0; } // No method to evolve, clear information.
                    bw.Write(Form[i]);
                    bw.Write(Method[i]);
                    bw.Write(Argument[i]);
                    bw.Write(u6[i]);
                }
                return ms.ToArray();
            }
        }
    }
}
