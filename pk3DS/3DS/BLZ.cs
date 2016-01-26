/*----------------------------------------------------------------------------*/
/*--  blz.c - Bottom LZ coding for Nintendo GBA/DS                          --*/
/*--  Copyright (C) 2011 CUE                                                --*/
/*--                                                                        --*/
/*--  Ported to C# by Andi Badra, tweaks by Kaphotics                       --*/
/*--                                                                        --*/
/*--  This program is free software: you can redistribute it and/or modify  --*/
/*--  it under the terms of the GNU General Public License as published by  --*/
/*--  the Free Software Foundation, either version 3 of the License, or     --*/
/*--  (at your option) any later version.                                   --*/
/*--                                                                        --*/
/*--  This program is distributed in the hope that it will be useful,       --*/
/*--  but WITHOUT ANY WARRANTY; without even the implied warranty of        --*/
/*--  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the          --*/
/*--  GNU General Public License for more details.                          --*/
/*--                                                                        --*/
/*--  You should have received a copy of the GNU General Public License     --*/
/*--  along with this program. If not, see <http://www.gnu.org/licenses/>.  --*/
/*----------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CTR
{
    public class BLZCoder
    {
        public static void main(string[] args)
        {
            new BLZCoder(args);
        }

        private const int CMD_DECODE = 0;
        private const int CMD_ENCODE = 1;

        private const int BLZ_NORMAL = 0;
        private const int BLZ_BEST = 1;
        private const int BLZ_SHIFT = 1;
        private const int BLZ_MASK = 0x80;
        private const int BLZ_THRESHOLD = 2;
        private const int BLZ_N = 0x1002;
        private const int BLZ_F = 0x12;

        private const int BLZ_MAXIM = 0x01400000;
        private const int RAW_MAXIM = 0x00FFFFFF;
        readonly bool arm9;
        int new_len;
        static void EXIT(string text)
        {
            Console.Write(text);
        }
        private readonly ProgressBar pBar1;
        private void initpBar(int max)
        {
            if (pBar1.InvokeRequired)
                pBar1.Invoke((MethodInvoker)delegate { pBar1.Minimum = 0; pBar1.Step = 1; pBar1.Value = 0; pBar1.Maximum = max; });
            else { pBar1.Minimum = 0; pBar1.Step = 1; pBar1.Value = 0; pBar1.Maximum = max; }
        }
        private void setpBarPos(int pos)
        {
            if (pBar1.InvokeRequired)
                pBar1.Invoke((MethodInvoker)delegate { pBar1.Value = pos; });
            else { pBar1.Value = pos; }
        }
        public BLZCoder(string[] args, ProgressBar pBar = null)
        {
            int cmd, mode = 0;
            if (pBar == null) pBar1 = new ProgressBar();
            pBar1 = pBar;

            // Title();

            if (args == null || (args.Length != 2))
                throw new Exception("No arguments supplied to BLZ");

            if (args[0].Equals("-d"))
                cmd = CMD_DECODE;

            else if (args[0].Equals("-en") || args[0].Equals("-en9"))
            {
                cmd = CMD_ENCODE;
                mode = BLZ_NORMAL;
            }
            else if (args[0].Equals("-eo") || args[0].Equals("-eo9"))
            {
                cmd = CMD_ENCODE;
                mode = BLZ_BEST;
            }
            else { Console.Write("Command not supported" + Environment.NewLine); return; }

            if (args.Length < 2)
                Console.Write("Filename not specified" + Environment.NewLine);
            else
            {
                int arg;
                switch (cmd)
                {
                    case 0:
                        for (arg = 1; arg < args.Length; arg++)
                            BLZ_Decode(args[arg]);
                        break;
                    case 1:
                        arm9 = args[0].Length > 3 && args[0][3] == '9';
                        for (arg = 1; arg < args.Length; arg++)
                            BLZ_Encode(args[arg], mode);
                        break;
                }
            }

            Console.Write(Environment.NewLine + "Done" + Environment.NewLine);
        }
        private void Save(string filename, byte[] buffer, int length)
        {
            Array.Resize(ref buffer, length);
            try
            { File.WriteAllBytes(filename, buffer); }
            catch (IOException e)
            {
                Console.Write(Environment.NewLine + "File write error" + Environment.NewLine + e + Environment.NewLine);
                File.WriteAllBytes("blz.bin", buffer);
                Console.Write(Environment.NewLine + "Wrote to 'blz.bin' instead." + Environment.NewLine);
            }
        }
        private void BLZ_Decode(string filename)
        {
            try
            {
                Console.Write("- decoding '%s'", filename);
                long startTime = DateTime.Now.Millisecond;
                byte[] buf = File.ReadAllBytes(filename);
                BLZResult result = BLZ_Decode(buf);
                if (result != null)
                    Save(filename, result.buffer, result.length);
                Console.Write(" - done, time="
                        + (DateTime.Now.Millisecond - startTime) + "ms");
                Console.Write(Environment.NewLine + "");
            }
            catch (IOException e)
            { Console.Write(Environment.NewLine + "File read error" + Environment.NewLine + e); }
        }
        private BLZResult BLZ_Decode(byte[] data)
        {
            int raw_len, len;
            int enc_len, dec_len;
            int flags = 0;

            byte[] pak_buffer = prepareData(data);
            int pak_len = pak_buffer.Length - 3;

            int inc_len = BitConverter.ToInt32(pak_buffer, pak_len - 4);
            if (inc_len < 1)
            {
                Console.Write(", WARNING: not coded file!");
                enc_len = 0;
                dec_len = pak_len;
                pak_len = 0;
                raw_len = dec_len;
            }
            else
            {
                if (pak_len < 8)
                {
                    Console.Write(Environment.NewLine + "File has a bad header" + Environment.NewLine);
                    return null;
                }
                int hdr_len = pak_buffer[pak_len - 5];
                if (hdr_len < 8 || hdr_len > 0xB)
                {
                    Console.Write(Environment.NewLine + "Bad header length" + Environment.NewLine);
                    return null;
                }
                if (pak_len <= hdr_len)
                {
                    Console.Write(Environment.NewLine + "Bad length" + Environment.NewLine);
                    return null;
                }
                enc_len = (int)(BitConverter.ToUInt32(pak_buffer, pak_len - 8) & 0x00FFFFFF);
                dec_len = pak_len - enc_len;
                pak_len = enc_len - hdr_len;
                raw_len = dec_len + enc_len + inc_len;
                if (raw_len > RAW_MAXIM)
                {
                    Console.Write(Environment.NewLine + "Bad decoded length" + Environment.NewLine);
                    return null;
                }
            }

            byte[] raw_buffer = new byte[raw_len];

            int pak = 0;
            int raw = 0;
            int pak_end = dec_len + pak_len;
            int raw_end = raw_len;

            for (len = 0; len < dec_len; len++)
                raw_buffer[raw++] = pak_buffer[pak++];

            BLZ_Invert(pak_buffer, dec_len, pak_len);

            int mask = 0;

            while (raw < raw_end)
            {
                if ((mask = (int)((uint)mask >> BLZ_SHIFT)) == 0)
                {
                    if (pak == pak_end)
                        break;

                    flags = pak_buffer[pak++];
                    mask = BLZ_MASK;
                }

                if ((flags & mask) == 0)
                {
                    if (pak == pak_end)
                        break;

                    raw_buffer[raw++] = pak_buffer[pak++];
                }
                else
                {
                    if (pak + 1 >= pak_end)
                        break;

                    int pos = pak_buffer[pak++] << 8;
                    pos |= pak_buffer[pak++];
                    len = (int)((uint)pos >> 12) + BLZ_THRESHOLD + 1;
                    if (raw + len > raw_end)
                    {
                        Console.Write(", WARNING: wrong decoded length!");
                        len = raw_end - raw;
                    }
                    pos = (pos & 0xFFF) + 3;
                    while (len-- > 0)
                    {
                        int charHere = raw_buffer[raw - pos];
                        raw_buffer[raw++] = (byte)charHere;
                    }
                }
            }

            BLZ_Invert(raw_buffer, dec_len, raw_len - dec_len);

            raw_len = raw;

            if (raw != raw_end)
                Console.Write(", WARNING: unexpected end of encoded file!");

            return new BLZResult(raw_buffer, raw_len);
        }
        private BLZResult BLZ_Encode(byte[] data, int mode)
        {
            new_len = 0;

            byte[] raw_buffer = prepareData(data);
            int raw_len = raw_buffer.Length - 3;

            byte[] pak_buffer = null;
            int pak_len = BLZ_MAXIM + 1;

            byte[] new_buffer = BLZ_Code(raw_buffer, raw_len, mode);

            if (new_len < pak_len)
            {
                pak_buffer = new_buffer;
                pak_len = new_len;
            }
            return new BLZResult(pak_buffer, pak_len);
        }
        private byte[] prepareData(byte[] data)
        {
            int fs = data.Length;
            byte[] fb = new byte[fs + 3];
            for (int i = 0; i < fs; i++)
                fb[i] = data[i];

            return fb;
        }
        private void writeUnsigned(byte[] buffer, int offset, int value)
        {
            buffer[offset] = (byte)(value & 0xFF);
            buffer[offset + 1] = (byte)((value >> 8) & 0xFF);
            buffer[offset + 2] = (byte)((value >> 16) & 0xFF);
            buffer[offset + 3] = (byte)((value >> 24) & 0x7F);
        }
        private void BLZ_Encode(string filename, int mode)
        {
            try
            {
                Console.Write("Now encoding {0}", filename);
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                byte[] buf = File.ReadAllBytes(filename);
                BLZResult result = BLZ_Encode(buf, mode);
                if (result != null)
                    Save(filename, result.buffer, result.length);

                stopwatch.Stop();
                Console.Write(Environment.NewLine + "Done, time elapsed = " + stopwatch.ElapsedMilliseconds + "ms" + Environment.NewLine);
            }
            catch (IOException e)
            { Console.Write(Environment.NewLine + "File read error" + Environment.NewLine + e + Environment.NewLine); }
        }
        private byte[] BLZ_Code(byte[] raw_buffer, int raw_len, int best)
        {
            int flg = 0;
            int pos_best = 0;
            int pos_next = 0;
            int pos_post = 0;

            int pak_tmp = 0;
            int raw_tmp = raw_len;

            int pak_len = raw_len + (raw_len + 7) / 8 + 11;
            byte[] pak_buffer = new byte[pak_len];

            int raw_new = raw_len;

            // We don't do any of the checks here
            // Presume that we actually are using an arm9
            if (arm9)
                raw_new -= 0x4000;

            BLZ_Invert(raw_buffer, 0, raw_len);

            int pak = 0;
            int raw = 0;
            int raw_end = raw_new;

            int mask = 0;
            initpBar(raw_end);
            while (raw < raw_end)
            {
                setpBarPos(raw);
                if ((mask = (int)((uint)mask >> BLZ_SHIFT)) == 0)
                {
                    pak_buffer[flg = pak++] = 0;
                    mask = BLZ_MASK;
                }

                SearchPair sl1 = SEARCH(pos_best, raw_buffer, raw, raw_end);
                int len_best = sl1.l;
                pos_best = sl1.p;

                // LZ-CUE optimization start
                if (best == BLZ_BEST)
                {
                    if (len_best > BLZ_THRESHOLD)
                    {
                        if (raw + len_best < raw_end)
                        {
                            raw += len_best;
                            SearchPair sl2 = SEARCH(pos_next, raw_buffer, raw,
                                    raw_end);
                            int len_next = sl2.l;
                            pos_next = sl2.p;
                            raw -= len_best - 1;
                            SearchPair sl3 = SEARCH(pos_post, raw_buffer, raw,
                                    raw_end);
                            int len_post = sl3.l;
                            pos_post = sl3.p;
                            raw--;

                            if (len_next <= BLZ_THRESHOLD)
                                len_next = 1;
                            if (len_post <= BLZ_THRESHOLD)
                                len_post = 1;
                            if (len_best + len_next <= 1 + len_post)
                                len_best = 1;
                        }
                    }
                }
                // LZ-CUE optimization end
                pak_buffer[flg] = (byte)(pak_buffer[flg] << 1);
                if (len_best > BLZ_THRESHOLD)
                {
                    raw += len_best;
                    pak_buffer[flg] |= 1;
                    pak_buffer[pak++] = (byte)((byte)((len_best - (BLZ_THRESHOLD + 1)) << 4) | ((uint)(pos_best - 3) >> 8));
                    pak_buffer[pak++] = (byte)(pos_best - 3);
                }
                else
                    pak_buffer[pak++] = raw_buffer[raw++];

                if (pak + raw_len - raw < pak_tmp + raw_tmp)
                {
                    pak_tmp = pak;
                    raw_tmp = raw_len - raw;
                }
            }

            while ((mask > 0) && (mask != 1))
            {
                mask = (int)((uint)mask >> BLZ_SHIFT);
                pak_buffer[flg] = (byte)(pak_buffer[flg] << 1);
            }

            pak_len = pak;

            BLZ_Invert(raw_buffer, 0, raw_len);
            BLZ_Invert(pak_buffer, 0, pak_len);

            if (pak_tmp == 0 || (raw_len + 4 < ((pak_tmp + raw_tmp + 3) & 0xFFFFFFFC) + 8))
            {
                pak = 0;
                raw = 0;
                raw_end = raw_len;

                while (raw < raw_end)
                    pak_buffer[pak] = raw_buffer[raw];

                while ((pak & 3) > 0)
                    pak_buffer[pak++] = 0;

                pak_buffer[pak++] = 0;
                pak_buffer[pak++] = 0;
                pak_buffer[pak++] = 0;
                pak_buffer[pak++] = 0;
            }
            else
            {
                byte[] tmp = new byte[raw_tmp + pak_tmp + 11];

                int len;
                for (len = 0; len < raw_tmp; len++)
                    tmp[len] = raw_buffer[len];

                for (len = 0; len < pak_tmp; len++)
                    tmp[raw_tmp + len] = pak_buffer[len + pak_len - pak_tmp];

                pak = 0;
                pak_buffer = tmp;

                pak = raw_tmp + pak_tmp;

                int enc_len = pak_tmp;
                int hdr_len = 8;
                int inc_len = raw_len - pak_tmp - raw_tmp;

                while ((pak & 3) > 0)
                {
                    pak_buffer[pak++] = 0xFF;
                    hdr_len++;
                }

                writeUnsigned(pak_buffer, pak, enc_len + hdr_len);
                pak += 3;
                pak_buffer[pak++] = (byte)hdr_len;
                writeUnsigned(pak_buffer, pak, inc_len - hdr_len);
                pak += 4;
            }
            new_len = pak;
            return pak_buffer;
        }

        private class SearchPair
        {
            public readonly int l;
            public readonly int p;

            public SearchPair(int l, int p)
            {
                this.l = l;
                this.p = p;
            }
        }
        private SearchPair SEARCH(int p, IList<byte> raw_buffer, int raw, int raw_end)
        {
            int l = BLZ_THRESHOLD;
            int max = raw >= BLZ_N ? BLZ_N : raw;
            for (int pos = 3; pos <= max; pos++)
            {
                int len;
                for (len = 0; len < BLZ_F; len++)
                {
                    if (raw + len == raw_end)
                        break;

                    if (len >= pos)
                        break;

                    if (raw_buffer[raw + len] != raw_buffer[raw + len - pos])
                        break;
                }

                if (len <= l) continue;
                p = pos;
                if ((l = len) == BLZ_F)
                    break;
            }
            return new SearchPair(l, p);
        }

        private class BLZResult
        {
            public BLZResult(byte[] raw_buffer, int raw_len)
            {
                buffer = raw_buffer;
                length = raw_len;
            }

            public readonly byte[] buffer;
            public readonly int length;
        }
        private void BLZ_Invert(byte[] buffer, int offset, int length)
        {
            int bottom = offset + length - 1;

            while (offset < bottom)
            {
                int ch = buffer[offset];
                buffer[offset++] = buffer[bottom];
                buffer[bottom--] = (byte)ch;
            }
        }
    }
}