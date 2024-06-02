using System.Text;

namespace pk3DS.Core;

public class Encounter7(uint val)
{
    public uint Species = val & 0x7FF;
    public uint Forme = (val >> 11) & 0x1F;
    public uint RawValue => Species | (Forme << 11);

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