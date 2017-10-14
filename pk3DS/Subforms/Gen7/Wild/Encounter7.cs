using System.Text;

namespace pk3DS
{
    public class Encounter7
    {
        public uint Species;
        public uint Forme;
        public uint RawValue => Species | (Forme << 11);

        public Encounter7(uint val)
        {
            Species = val & 0x7FF;
            Forme = (val >> 11) & 0x1F;
        }

        public string GetSummary(string[] speciesList)
        {
            var sb = new StringBuilder();
            sb.Append(speciesList[Species]);
            if (Forme != 0)
                sb.Append($" (Forme {Forme})");
            return sb.ToString();
        }
    }
}
