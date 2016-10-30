using System.IO;

namespace pk3DS
{
    public class GARCReference
    {
        public readonly int FileNumber;
        public readonly string Name;
        private int A => (FileNumber / 100) % 10;
        private int B => (FileNumber / 10) % 10;
        private int C => (FileNumber / 1) % 10;
        public readonly bool LanguageVariant;
        public string Reference => Path.Combine("a", A.ToString(), B.ToString(), C.ToString());

        public GARCReference(int file, string name, bool lv = false)
        {
            Name = name;
            FileNumber = file;
            LanguageVariant = lv;
        }
        public GARCReference getRelativeGARC(int offset, string name = "")
        {
            return new GARCReference(FileNumber + offset, name);
        }
    }
}
