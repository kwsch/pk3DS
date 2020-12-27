using System.Text;

namespace pk3DS.Core
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
                sb.Append(" (Forme ").Append(Forme).Append(')');
            return sb.ToString();
        }

        public uint Dump(EncounterTable t) => RawValue | (uint)(t.MinLevel << 16) | (uint)(t.MaxLevel << 24);
    }
}
