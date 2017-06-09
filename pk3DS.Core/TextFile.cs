﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pk3DS.Core
{
    public class TextFile
    {
        // Text Formatting Config
        private const ushort KEY_BASE = 0x7C89;
        private const ushort KEY_ADVANCE = 0x2983;
        private const ushort KEY_VARIABLE = 0x0010;
        private const ushort KEY_TERMINATOR = 0x0000;
        private const ushort KEY_TEXTRETURN = 0xBE00;
        private const ushort KEY_TEXTCLEAR = 0xBE01;
        private const ushort KEY_TEXTWAIT = 0xBE02;
        private const ushort KEY_TEXTNULL = 0xBDFF;
        private const bool SETEMPTYTEXT = false;
        private static readonly byte[] emptyTextFile = { 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00 };

        public TextFile(GameConfig config, byte[] data = null)
        {
            Data = (byte[])(data ?? emptyTextFile).Clone();

            if (InitialKey != 0)
                throw new Exception("Invalid initial key! Not 0?");
            if (SectionDataOffset + TotalLength != Data.Length || TextSections != 1)
                throw new Exception("Invalid Text File");
            if (SectionLength != TotalLength)
                throw new Exception("Section size and overall size do not match.");

            Config = config;
        }
        private GameConfig Config { get; set; }
        private ushort TextSections { get { return BitConverter.ToUInt16(Data, 0x0); } set { BitConverter.GetBytes(value).CopyTo(Data, 0x0); } } // Always 0x0001
        private ushort LineCount { get { return BitConverter.ToUInt16(Data, 0x2); } set { BitConverter.GetBytes(value).CopyTo(Data, 0x2); } }
        private uint TotalLength { get { return BitConverter.ToUInt32(Data, 0x4); } set { BitConverter.GetBytes(value).CopyTo(Data, 0x4); } }
        private uint InitialKey { get { return BitConverter.ToUInt32(Data, 0x8); } set { BitConverter.GetBytes(value).CopyTo(Data, 0x8); } } // Always 0x00000000
        private uint SectionDataOffset { get { return BitConverter.ToUInt32(Data, 0xC); } set { BitConverter.GetBytes(value).CopyTo(Data, 0xC); } } // Always 0x0010
        private uint SectionLength { get { return BitConverter.ToUInt32(Data, (int)SectionDataOffset); } set { BitConverter.GetBytes(value).CopyTo(Data, SectionDataOffset); } }
        private LineInfo[] LineOffsets
        {
            get
            {
                LineInfo[] result = new LineInfo[LineCount];
                int sdo = (int)SectionDataOffset;
                for (int i = 0; i < result.Length; i++)
                    result[i] = new LineInfo
                    {
                        Offset = BitConverter.ToInt32(Data, i * 8 + sdo + 4) + sdo,
                        Length = BitConverter.ToInt16(Data, i * 8 + sdo + 8)
                    };
                return result;
            }
            set
            {
                if (value == null)
                    return;
                int sdo = (int)SectionDataOffset;
                for (int i = 0; i < value.Length; i++)
                {
                    BitConverter.GetBytes(value[i].Offset).CopyTo(Data, i * 8 + sdo + 4);
                    BitConverter.GetBytes(value[i].Length).CopyTo(Data, i * 8 + sdo + 8);
                }
            }
        }
        private class LineInfo
        {
            public int Offset, Length;
        }

        public string[] Lines
        {
            get
            {
                ushort key = KEY_BASE;
                string[] result = new string[LineCount];
                LineInfo[] lines = LineOffsets;
                for (int i = 0; i < lines.Length; i++)
                {
                    byte[] EncryptedLineData = new byte[lines[i].Length * 2];
                    Array.Copy(Data, lines[i].Offset, EncryptedLineData, 0, EncryptedLineData.Length);
                    byte[] DecryptedLineData = cryptLineData(EncryptedLineData, key);
                    result[i] = getLineString(Config, DecryptedLineData);
                    key += KEY_ADVANCE;
                }
                return result;
            }
            set
            {
                if (value == null)
                    value = new string[0];

                ushort key = KEY_BASE;
                LineInfo[] lines = new LineInfo[value.Length];

                // Get Line Data
                byte[][] lineData = new byte[lines.Length][];
                int sdo = (int)SectionDataOffset;
                int bytesUsed = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    string text = (value[i] ?? "").Trim();
                    if (text.Length == 0 && SETEMPTYTEXT)
                        text = $"[~ {i}]";
                    byte[] DecryptedLineData;
                    try
                    {
                        DecryptedLineData = getLineData(Config, text);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"{ex.GetType()} at line {i}: {ex.Message}");
                    }
                    lineData[i] = cryptLineData(DecryptedLineData, key);
                    if (lineData[i].Length % 4 == 2)
                        Array.Resize(ref lineData[i], lineData[i].Length + 2);
                    key += KEY_ADVANCE;
                    lines[i] = new LineInfo { Offset = 4 + 8 * value.Length + bytesUsed, Length = DecryptedLineData.Length / 2 };
                    bytesUsed += lineData[i].Length;
                }

                // Apply Line Data
                Array.Resize(ref Data, sdo + 4 + 8 * value.Length + bytesUsed);
                LineOffsets = lines; // Handled by LineInfo[] set {}
                lineData.SelectMany(i => i).ToArray().CopyTo(Data, Data.Length - bytesUsed);
                TotalLength = SectionLength = (uint)(Data.Length - sdo);
                LineCount = (ushort)value.Length;
            }
        }
        public byte[] Data;

        private byte[] cryptLineData(byte[] data, ushort key)
        {
            byte[] result = new byte[data.Length];
            for (int i = 0; i < result.Length; i += 2)
            {
                BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(data, i) ^ key)).CopyTo(result, i);
                key = (ushort)(key << 3 | key >> 13);
            }
            return result;
        }
        private byte[] getLineData(GameConfig config, string line)
        {
            if (line == null)
                return new byte[2];

            MemoryStream ms = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                int i = 0;
                while (i < line.Length)
                {
                    ushort val = line[i++];

                    if (val == 0x202F) val = 0xE07F; // nbsp
                    else if (val == 0x2026) val = 0xE08D; // …
                    else if (val == 0x2642) val = 0xE08E; // ♂
                    else if (val == 0x2640) val = 0xE08F; // ♀

                    if (val == '[') // Variable
                    {
                        // grab the string
                        int bracket = line.IndexOf("]", i, StringComparison.Ordinal);
                        if (bracket < 0)
                            throw new ArgumentException("Variable text is not capped properly.");
                        string varText = line.Substring(i, bracket - i);
                        var varValues = getVariableValues(config, varText);
                        foreach (ushort v in varValues) bw.Write(v);
                        i += 1 + varText.Length;
                    }
                    else if (val == '\\') // Escaped Formatting
                    {
                        var escapeValues = getEscapeValues(line[i++]);
                        foreach (ushort v in escapeValues) bw.Write(v);
                    }
                    else
                        bw.Write(val);
                }
                bw.Write(KEY_TERMINATOR); // cap the line off
                return ms.ToArray();
            }
        }
        private string getLineString(GameConfig config, byte[] data)
        {
            if (data == null)
                return null;

            string s = "";
            int i = 0;
            while (i < data.Length)
            {
                ushort val = BitConverter.ToUInt16(data, i);
                if (val == KEY_TERMINATOR) break;
                i += 2;

                switch (val)
                {
                    case KEY_TERMINATOR: return s;
                    case KEY_VARIABLE: s += getVariableString(config, data, ref i); break;
                    case '\n': s += @"\n"; break;
                    case '\\': s += @"\\"; break;
                    case '[': s += @"\["; break;
                    case 0xE07F: s += (char)0x202F; break; // nbsp
                    case 0xE08D: s += (char)0x2026; break; // …
                    case 0xE08E: s += (char)0x2642; break; // ♂
                    case 0xE08F: s += (char)0x2640; break; // ♀
                    default: s += (char)val; break;
                }
            }
            return s; // Shouldn't get hit if the string is properly terminated.
        }
        private string getVariableString(GameConfig config, byte[] data, ref int i)
        {
            string s = "";
            ushort count = BitConverter.ToUInt16(data, i); i += 2;
            ushort variable = BitConverter.ToUInt16(data, i); i += 2;

            switch (variable)
            {
                case KEY_TEXTRETURN: // "Waitbutton then scroll text; \r"
                    return "\\r";
                case KEY_TEXTCLEAR: // "Waitbutton then clear text;; \c"
                    return "\\c";
                case KEY_TEXTWAIT: // Dramatic pause for a text line. New!
                    ushort time = BitConverter.ToUInt16(data, i); i += 2;
                    return $"[WAIT {time}]";
                case KEY_TEXTNULL: // Empty Text line? Includes linenum so maybe for betatest finding used-unused lines?
                    ushort line = BitConverter.ToUInt16(data, i); i += 2;
                    return $"[~ {line}]";
            }

            string varName = getVariableString(config, variable);

            s += "[VAR" + " " + varName;
            if (count > 1)
            {
                s += '(';
                while (count > 1)
                {
                    ushort arg = BitConverter.ToUInt16(data, i); i += 2;
                    s += arg.ToString("X4");
                    if (--count == 1) break;
                    s += ",";
                }
                s += ')';
            }
            s += "]";
            return s;
        }
        private IEnumerable<ushort> getEscapeValues(char esc)
        {
            var vals = new List<ushort>();
            switch (esc)
            {
                case 'n': vals.Add('\n'); return vals;
                case '\\': vals.Add('\\'); return vals;
                case 'r': vals.AddRange(new ushort[] { KEY_VARIABLE, 1, KEY_TEXTRETURN }); return vals;
                case 'c': vals.AddRange(new ushort[] { KEY_VARIABLE, 1, KEY_TEXTCLEAR }); return vals;
                default: throw new Exception("Invalid terminated line: \"\\" + esc + "\"");
            }
        }
        private IEnumerable<ushort> getVariableValues(GameConfig config, string variable)
        {
            string[] split = variable.Split(' ');
            if (split.Length < 2)
                throw new ArgumentException("Incorrectly formatted variable text!");

            var vals = new List<ushort> { KEY_VARIABLE };
            switch (split[0])
            {
                case "~": // Blank Text Line Variable (No text set - debug/quality testing variable?)
                    vals.Add(1);
                    vals.Add(KEY_TEXTNULL);
                    vals.Add(Convert.ToUInt16(split[1]));
                    break;
                case "WAIT": // Event pause Variable.
                    vals.Add(1);
                    vals.Add(KEY_TEXTWAIT);
                    vals.Add(Convert.ToUInt16(split[1]));
                    break;
                case "VAR": // Text Variable
                    vals.AddRange(getVariableParameters(config, split[1]));
                    break;
                default: throw new Exception("Unknown variable method type!");
            }
            return vals;
        }
        private IEnumerable<ushort> getVariableParameters(GameConfig config, string text)
        {
            var vals = new List<ushort>();
            int bracket = text.IndexOf('(');
            bool noArgs = bracket < 0;
            string variable = noArgs ? text : text.Substring(0, bracket);
            ushort varVal = getVariableNumber(config, variable);

            if (!noArgs)
            {
                string strArgs = text.Substring(bracket + 1, text.Length - bracket - 2);
                string[] args = strArgs.Split(',');
                vals.Add((ushort)(1 + args.Length));
                vals.Add(varVal);
                try
                {
                    vals.AddRange(args.Select(t => Convert.ToUInt16(t, 16)));
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.GetType()} in args '{strArgs}'");
                }
            }
            else
            {
                vals.Add(1);
                vals.Add(varVal);
            }
            return vals;
        }

        private ushort getVariableNumber(GameConfig config, string variable)
        {
            var v = config.getVariableCode(variable);
            if (v != null)
                return (ushort)v.Code;

            try
            {
                return Convert.ToUInt16(variable, 16);
            }
            catch { throw new ArgumentException($"Variable \"{variable}\" parse error."); }
        }
        private string getVariableString(GameConfig config, ushort variable)
        {
            var v = config.getVariableName(variable);
            return v == null ? variable.ToString("X4") : v.Name;
        }

        // Exposed Methods
        public static string[] getStrings(GameConfig config, byte[] data)
        {
            TextFile t;
            try { t = new TextFile(config, data); } catch { return null; }
            return t.Lines;
        }
        public static byte[] getBytes(GameConfig config, string[] lines)
        {
            return new TextFile(config) { Lines = lines }.Data;
        }
    }
}
