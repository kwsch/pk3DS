using System;
using System.Linq;
using System.Text;

namespace pk3DS.Core;

public class EncounterTable
{
    public int MinLevel;
    public int MaxLevel;
    public readonly int[] Rates;
    public readonly Encounter7[][] Encounter7s;
    public readonly Encounter7[] AdditionalSOS;

    public readonly byte[] Data;

    public EncounterTable(byte[] t)
    {
        Rates = new int[10];
        Encounter7s = new Encounter7[9][];
        AdditionalSOS = new Encounter7[6];
        Data = (byte[])t.Clone();
        Reset();
    }

    public void Reset()
    {
        var t = Data;
        MinLevel = t[0];
        MaxLevel = t[1];

        for (int i = 0; i < Rates.Length; i++)
            Rates[i] = t[2 + i];

        for (int i = 0; i < Encounter7s.Length - 1; i++)
        {
            Encounter7s[i] = new Encounter7[10];
            var ofs = 0xC + (i * 4 * Encounter7s[i].Length);
            for (int j = 0; j < Encounter7s[i].Length; j++)
                Encounter7s[i][j] = new Encounter7(BitConverter.ToUInt32(t, ofs + (4 * j)));
        }

        for (var i = 0; i < AdditionalSOS.Length; i++)
            AdditionalSOS[i] = new Encounter7(BitConverter.ToUInt32(t, 0x14C + (4 * i)));

        Encounter7s[8] = AdditionalSOS;
    }

    /// <summary>
    /// Copies the first set of slots to all others, including weather if specified.
    /// </summary>
    /// <param name="PlusWeather">Copy first set to weather slots (only first few get copied as Weather has less slots)</param>
    public void CopySlotsToSOS(bool PlusWeather = false)
    {
        int length = Encounter7s.Length - 2; // except weather
        if (PlusWeather)
            length++; // reinclude weather slots
        var toBeCopiedTo = Encounter7s.Skip(1).Take(length);

        var first = Encounter7s[0];
        foreach (var slots in toBeCopiedTo)
        {
            for (int s = 0; s < slots.Length; s++)
            {
                slots[s].Species = first[s].Species;
                slots[s].Forme = first[s].Forme;
            }
        }
    }

    public void Write()
    {
        Data[0] = (byte)MinLevel;
        Data[1] = (byte)MaxLevel;
        for (int i = 0; i < Rates.Length; i++)
            Data[2 + i] = (byte)Rates[i];

        for (int i = 0; i < Encounter7s.Length - 1; i++)
        {
            var ofs = 0xC + (i * 4 * Encounter7s[i].Length);
            for (int j = 0; j < Encounter7s[i].Length; j++)
                BitConverter.GetBytes(Encounter7s[i][j].RawValue).CopyTo(Data, ofs + (4 * j));
        }

        for (int i = 0; i < AdditionalSOS.Length; i++)
            BitConverter.GetBytes(AdditionalSOS[i].RawValue).CopyTo(Data, 0x14C + (4 * i));
    }

    public string GetAllies(int slotIndex, ReadOnlySpan<string> speciesList)
    {
        string result = "";
        for (int i = 1; i < Encounter7s.Length; i++)
        {
            var ally = Encounter7s[i][slotIndex];
            if (ally?.Species is null or 0)
                continue;
            var name = speciesList[(int)ally.Species];
            if (result.Contains(name))
                continue;
            result += result.Length == 0 ? name : ", " + name;
        }

        if (result.Length == 0)
            return "(None)";
        return result;
    }

    public string GetSummary(string[] speciesList)
    {
        var sb = new StringBuilder();
        sb.Append("Encounters").Append(" (Levels ").Append(MinLevel).Append('-').Append(MaxLevel).Append("): ");
        sb.AppendLine();
        AddSlotSetSummary(sb, speciesList);

        sb.Append("Additional SOS Weather encounters: ");
        sb.AppendJoin(", ", AdditionalSOS
            .Select(e => e.RawValue).Distinct().Select(e => new Encounter7(e))
            .Select(e => e.GetSummary(speciesList))).AppendLine();

        return sb.ToString();
    }

    private void AddSlotSetSummary(StringBuilder sb, string[] speciesList)
    {
        var baseSlots = Encounter7s[0];
        for (int i = 0; i < baseSlots.Length; i++)
        {
            var species = baseSlots[i].Species;
            if (species == 0)
                continue;

            var rate = Rates[i];
            sb.Append($"\t({rate}%)\t{speciesList[species]}");
            var allies = GetAllies(i, speciesList);
            if (allies.Length != 0)
                sb.Append(" w/ ").Append(allies);
            sb.AppendLine();
        }
    }

    public void Reset(byte[] data)
    {
        Buffer.BlockCopy(data, 0, Data, 0, 0x164);
        Reset();
    }
}