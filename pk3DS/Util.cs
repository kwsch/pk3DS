using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace pk3DS
{
    class Util
    { // Image Layering/Blending Utility
        internal static Bitmap LayerImage(Image baseLayer, Image overLayer, int x, int y, double trans)
        {
            Bitmap img = new Bitmap(baseLayer.Width, baseLayer.Height);
            using (Graphics gr = Graphics.FromImage(img))
            {
                gr.DrawImage(baseLayer, new Point(0, 0));
                Bitmap o = ChangeOpacity(overLayer, trans);
                gr.DrawImage(o, new Rectangle(x, y, overLayer.Width, overLayer.Height));
            }
            return img;
        }
        internal static Bitmap ChangeOpacity(Image img, double trans)
        {
            if (img == null)
                return null;
            if (img.PixelFormat.HasFlag(PixelFormat.Indexed))
                return (Bitmap)img;

            Bitmap bmp = (Bitmap)img.Clone();
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr ptr = bmpData.Scan0;

            int len = bmp.Width * bmp.Height * 4;
            byte[] data = new byte[len];

            Marshal.Copy(ptr, data, 0, len);

            for (int i = 0; i < data.Length; i += 4)
                data[i + 3] = (byte)(data[i + 3] * trans);

            Marshal.Copy(data, 0, ptr, len);
            bmp.UnlockBits(bmpData);

            return bmp;
        }
        internal static Bitmap getSprite(int species, int form, int gender, int item)
        {
            string file;
            if (species == 0)
            { return (Bitmap)Properties.Resources.ResourceManager.GetObject("_0"); }
            {
                file = "_" + species;
                if (form > 0) // Alt Form Handling
                    file = file + "_" + form;
                else if (gender == 1 && (species == 592 || species == 593)) // Frillish & Jellicent
                    file = file + "_" + gender;
                else if (gender == 1 && (species == 521 || species == 668)) // Unfezant & Pyroar
                    file = "_" + species + "f";
            }

            // Redrawing logic
            Bitmap baseImage = (Bitmap)Properties.Resources.ResourceManager.GetObject(file);
            if (baseImage == null)
            {
                if (species < 722)
                {
                    baseImage = LayerImage(
                        (Image)Properties.Resources.ResourceManager.GetObject("_" + species),
                        Properties.Resources.unknown,
                        0, 0, .5);
                }
                else
                    baseImage = Properties.Resources.unknown;
            }
            if (item > 0)
            {
                Bitmap itemimg = (Bitmap)Properties.Resources.ResourceManager.GetObject("item_" + item) ?? Properties.Resources.helditem;
                // Redraw
                baseImage = LayerImage(baseImage, itemimg, 22 + (15 - itemimg.Width) / 2, 15 + (15 - itemimg.Height), 1);
            }
            return baseImage;
        }
        internal static Bitmap scaleImage(Bitmap rawImg, int s)
        {
            Bitmap bigImg = new Bitmap(rawImg.Width * s, rawImg.Height * s);
            for (int x = 0; x < bigImg.Width; x++)
                for (int y = 0; y < bigImg.Height; y++)
                    bigImg.SetPixel(x, y, rawImg.GetPixel(x / s, y / s));
            return bigImg;
        }

        // Strings and Paths
        internal static FileInfo GetNewestFile(DirectoryInfo directory)
        {
            return directory.GetFiles()
                .Union(directory.GetDirectories().Select(GetNewestFile))
                .OrderByDescending(f => f?.LastWriteTime ?? DateTime.MinValue)
                .FirstOrDefault();
        }
        internal static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
               .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        internal static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
        internal static string TrimFromZero(string input)
        {
            int index = input.IndexOf('\0');
            if (index < 0)
                return input;

            return input.Substring(0, index);
        }
        internal static string[] getStringList(string f, string l)
        {
            object txt = Properties.Resources.ResourceManager.GetObject("text_" + f + "_" + l); // Fetch File, \n to list.
            List<string> rawlist = ((string)txt).Split('\n').ToList();

            string[] stringdata = new string[rawlist.Count];
            for (int i = 0; i < rawlist.Count; i++)
                stringdata[i] = rawlist[i].Trim();

            return stringdata;
        }
        internal static string[] getSimpleStringList(string f)
        {
            object txt = Properties.Resources.ResourceManager.GetObject(f); // Fetch File, \n to list.
            List<string> rawlist = ((string)txt).Split('\n').ToList();

            string[] stringdata = new string[rawlist.Count];
            for (int i = 0; i < rawlist.Count; i++)
                stringdata[i] = rawlist[i].Trim();

            return stringdata;
        }
        // Randomization
        internal static Random rand = new Random();
        internal static uint rnd32()
        {
            return (uint)(rand.Next(1 << 30)) << 2 | (uint)(rand.Next(1 << 2));
        }

        // Data Retrieval
        internal static int ToInt32(TextBox tb)
        {
            string value = tb.Text;
            return ToInt32(value);
        }
        internal static uint ToUInt32(TextBox tb)
        {
            string value = tb.Text;
            return ToUInt32(value);
        }
        internal static int ToInt32(MaskedTextBox tb)
        {
            string value = tb.Text;
            return ToInt32(value);
        }
        internal static uint ToUInt32(MaskedTextBox tb)
        {
            string value = tb.Text;
            return ToUInt32(value);
        }
        internal static int ToInt32(string value)
        {
            string val = value?.Replace(" ", "").Replace("_", "").Trim();
            return string.IsNullOrWhiteSpace(val) ? 0 : int.Parse(val);
        }
        internal static uint ToUInt32(string value)
        {
            string val = value?.Replace(" ", "").Replace("_", "").Trim();
            return string.IsNullOrWhiteSpace(val) ? 0 : uint.Parse(val);
        }
        internal static uint getHEXval(TextBox tb)
        {
            if (tb.Text == null)
                return 0;
            string str = getOnlyHex(tb.Text);
            return uint.Parse(str, NumberStyles.HexNumber);
        }
        internal static int getIndex(ComboBox cb)
        {
            int val;
            if (cb.SelectedValue == null)
                return 0;

            try
            { val = (int)cb.SelectedValue; }
            catch
            { val = cb.SelectedIndex; if (val < 0) val = 0; }
            return val;
        }
        internal static string getOnlyHex(string str)
        {
            if (str == null) return "0";

            string s = "";

            foreach (char t in str)
            {
                var c = t;
                // filter for hex
                if ((c < 0x0047 && c > 0x002F) || (c < 0x0067 && c > 0x0060))
                    s += c;
                else
                    System.Media.SystemSounds.Beep.Play();
            }
            if (s.Length == 0)
                s = "0";
            return s;
        }

        // Data Manipulation
        internal static void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(rand.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        // Form Translation
        internal static void TranslateInterface(Control form, string lang)
        {
            // Check to see if a the translation file exists in the same folder as the executable
            string externalLangPath = "lang_" + lang + ".txt";
            string[] rawlist;
            if (File.Exists(externalLangPath))
                rawlist = File.ReadAllLines(externalLangPath);
            else
            {
                object txt = Properties.Resources.ResourceManager.GetObject("lang_" + lang);
                if (txt == null) return; // Translation file does not exist as a resource; abort this function and don't translate UI.
                rawlist = ((string)txt).Split(new[] { "\n" }, StringSplitOptions.None);
                rawlist = rawlist.Select(i => i.Trim()).ToArray(); // Remove trailing spaces
            }

            string[] stringdata = new string[rawlist.Length];
            int itemsToRename = 0;
            for (int i = 0; i < rawlist.Length; i++)
            {
                // Find our starting point
                if (!rawlist[i].Contains("! " + form.Name)) continue;

                // Allow renaming of the Window Title
                string[] WindowName = rawlist[i].Split(new[] { " = " }, StringSplitOptions.None);
                if (WindowName.Length > 1) form.Text = WindowName[1];
                // Copy our Control Names and Text to a new array for later processing.
                for (int j = i + 1; j < rawlist.Length; j++)
                {
                    if (rawlist[j].Length == 0) continue; // Skip Over Empty Lines, errhandled
                    if (rawlist[j][0].ToString() == "-") continue; // Keep translating if line is a comment line
                    if (rawlist[j][0].ToString() == "!") // Stop if we have reached the end of translation
                        goto rename;
                    stringdata[itemsToRename] = rawlist[j]; // Add the entry to process later.
                    itemsToRename++;
                }
            }
            return; // Not Found

            // Now that we have our items to rename in: Control = Text format, let's execute the changes!
            rename:
            for (int i = 0; i < itemsToRename; i++)
            {
                string[] SplitString = stringdata[i].Split(new[] { " = " }, StringSplitOptions.None);
                if (SplitString.Length < 2)
                    continue; // Error in Input, errhandled
                string ctrl = SplitString[0]; // Control to change the text of...
                string text = SplitString[1]; // Text to set Control.Text to...
                Control[] controllist = form.Controls.Find(ctrl, true);
                if (controllist.Length != 0) // If Control is found
                { controllist[0].Text = text; goto next; }

                // Check MenuStrips
                foreach (MenuStrip menu in form.Controls.OfType<MenuStrip>())
                {
                    // Menu Items aren't in the Form's Control array. Find within the menu's Control array.
                    ToolStripItem[] TSI = menu.Items.Find(ctrl, true);
                    if (TSI.Length <= 0) continue;

                    TSI[0].Text = text; goto next;
                }
                // Check ContextMenuStrips
                foreach (ContextMenuStrip cs in FindContextMenuStrips(form.Controls.OfType<Control>()).Distinct())
                {
                    ToolStripItem[] TSI = cs.Items.Find(ctrl, true);
                    if (TSI.Length <= 0) continue;

                    TSI[0].Text = text; goto next;
                }

                next:;
            }
        }
        internal static List<ContextMenuStrip> FindContextMenuStrips(IEnumerable<Control> c)
        {
            List<ContextMenuStrip> cs = new List<ContextMenuStrip>();
            foreach (Control control in c)
            {
                if (control.ContextMenuStrip != null)
                    cs.Add(control.ContextMenuStrip);

                else if (control.Controls.Count > 0)
                    cs.AddRange(FindContextMenuStrips(control.Controls.OfType<Control>()));
            }
            return cs;
        }

        // Message Displays
        internal static DialogResult Error(params string[] lines)
        {
            System.Media.SystemSounds.Exclamation.Play();
            string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        internal static DialogResult Alert(params string[] lines)
        {
            System.Media.SystemSounds.Asterisk.Play();
            string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        internal static DialogResult Prompt(MessageBoxButtons btn, params string[] lines)
        {
            System.Media.SystemSounds.Question.Play();
            string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Prompt", btn, MessageBoxIcon.Asterisk);
        }

        // DataSource Providing
        public class cbItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
        }
        internal static List<cbItem> getCBList(string textfile, string lang)
        {
            // Set up
            string[] inputCSV = getSimpleStringList(textfile);

            // Get Language we're fetching for
            int index = Array.IndexOf(new[] { "ja", "en", "fr", "de", "it", "es", "ko", "zh", }, lang);

            // Set up our Temporary Storage
            string[] unsortedList = new string[inputCSV.Length - 1];
            int[] indexes = new int[inputCSV.Length - 1];

            // Gather our data from the input file
            for (int i = 1; i < inputCSV.Length; i++)
            {
                string[] countryData = inputCSV[i].Split(',');
                indexes[i - 1] = Convert.ToInt32(countryData[0]);
                unsortedList[i - 1] = countryData[index + 1];
            }

            // Sort our input data
            string[] sortedList = new string[inputCSV.Length - 1];
            Array.Copy(unsortedList, sortedList, unsortedList.Length);
            Array.Sort(sortedList);

            // Arrange the input data based on original number
            return sortedList.Select(t => new cbItem
            {
                Text = t, Value = indexes[Array.IndexOf(unsortedList, t)]
            }).ToList();
        }
        internal static List<cbItem> getCBList(string[] inStrings, params int[][] allowed)
        {
            List<cbItem> cbList = new List<cbItem>();
            if (allowed == null)
                allowed = new[] { Enumerable.Range(0, inStrings.Length).ToArray() };

            foreach (int[] list in allowed)
            {
                // Sort the Rest based on String Name
                string[] unsortedChoices = new string[list.Length];
                for (int i = 0; i < list.Length; i++)
                    unsortedChoices[i] = inStrings[list[i]];

                string[] sortedChoices = new string[unsortedChoices.Length];
                Array.Copy(unsortedChoices, sortedChoices, unsortedChoices.Length);
                Array.Sort(sortedChoices);

                // Add the rest of the items
                cbList.AddRange(sortedChoices.Select(t => new cbItem
                {
                    Text = t, Value = list[Array.IndexOf(unsortedChoices, t)]
                }));
            }
            return cbList;
        }
        internal static List<cbItem> getOffsetCBList(List<cbItem> cbList, string[] inStrings, int offset, int[] allowed)
        {
            if (allowed == null)
                allowed = Enumerable.Range(0, inStrings.Length).ToArray();

            int[] list = (int[])allowed.Clone();
            for (int i = 0; i < list.Length; i++)
                list[i] -= offset;

            {
                // Sort the Rest based on String Name
                string[] unsortedChoices = new string[allowed.Length];
                for (int i = 0; i < allowed.Length; i++)
                    unsortedChoices[i] = inStrings[list[i]];

                string[] sortedChoices = new string[unsortedChoices.Length];
                Array.Copy(unsortedChoices, sortedChoices, unsortedChoices.Length);
                Array.Sort(sortedChoices);

                // Add the rest of the items
                cbList.AddRange(sortedChoices.Select(t => new cbItem
                {
                    Text = t, Value = allowed[Array.IndexOf(unsortedChoices, t)]
                }));
            }
            return cbList;
        }
        internal static List<cbItem> getVariedCBList(List<cbItem> cbList, string[] inStrings, int[] stringNum, int[] stringVal)
        {
            // Set up
            List<cbItem> newlist = new List<cbItem>();

            for (int i = 4; i > 1; i--) // add 4,3,2
            {
                // First 3 Balls are always first
                cbItem ncbi = new cbItem
                {
                    Text = inStrings[i],
                    Value = i
                };
                newlist.Add(ncbi);
            }

            // Sort the Rest based on String Name
            string[] ballnames = new string[stringNum.Length];
            for (int i = 0; i < stringNum.Length; i++)
                ballnames[i] = inStrings[stringNum[i]];

            string[] sortedballs = new string[stringNum.Length];
            Array.Copy(ballnames, sortedballs, ballnames.Length);
            Array.Sort(sortedballs);

            // Add the rest of the balls
            newlist.AddRange(sortedballs.Select(t => new cbItem
            {
                Text = t, Value = stringVal[Array.IndexOf(ballnames, t)]
            }));
            return newlist;
        }
        internal static List<cbItem> getUnsortedCBList(string textfile)
        {
            // Set up
            List<cbItem> cbList = new List<cbItem>();
            string[] inputCSV = getSimpleStringList(textfile);

            // Gather our data from the input file
            for (int i = 1; i < inputCSV.Length; i++)
            {
                string[] inputData = inputCSV[i].Split(',');
                cbItem ncbi = new cbItem
                {
                    Value = Convert.ToInt32(inputData[0]),
                    Text = inputData[1]
                };
                cbList.Add(ncbi);
            }
            return cbList;
        }

        // GARCTool Utility
        internal static string GuessExtension(BinaryReader br, string defaultExt, bool bypass)
        {
            try
            {
                string ext = "";
                long position = br.BaseStream.Position;
                byte[] magic = System.Text.Encoding.ASCII.GetBytes(br.ReadChars(4));

                // check for extensions
                {
                    ushort count = BitConverter.ToUInt16(magic, 2);

                    // check for 2char container extensions
                    try
                    {
                        br.BaseStream.Position = position + 4 + 4 * count;
                        if (br.ReadUInt32() == br.BaseStream.Length)
                        {
                            ext += (char)magic[0] + (char)magic[1];
                            goto end;
                        }
                    }
                    catch { }
                    if (bypass) return defaultExt;

                    // check for darc
                    try
                    {
                        count = BitConverter.ToUInt16(magic, 0);
                        br.BaseStream.Position = position + 4 + 0x40 * count;
                        uint tableval = br.ReadUInt32();
                        br.BaseStream.Position += 0x20 * tableval;
                        while (br.PeekChar() == 0) // seek forward
                            br.ReadByte();
                        if (br.ReadUInt32() == 0x63726164)
                            return "darc";
                    }
                    catch { }

                    // check for bclim
                    try
                    {
                        br.BaseStream.Position = br.BaseStream.Length - 0x28;
                        if (br.ReadUInt32() == 0x4D494C43)
                        {
                            br.BaseStream.Position = br.BaseStream.Length - 0x4;
                            if (br.ReadUInt32() == br.BaseStream.Length - 0x28)
                                return "bclim";
                        }
                    }
                    catch { }

                    // generic check
                    {
                        if (magic[0] < 0x41)
                            return defaultExt;
                        for (int i = 0; i < magic.Length && i < 4; i++)
                        {
                            if ((magic[i] >= 'a' && magic[i] <= 'z') || (magic[i] >= 'A' && magic[i] <= 'Z')
                                || char.IsDigit((char)magic[i]))
                            {
                                ext += (char)magic[i];
                            }
                            else
                                break;
                        }
                    }
                }
            end:
                {
                    // Return BaseStream position to the start.
                    br.BaseStream.Position = position;
                    if (ext.Length <= 1)
                        return defaultExt;
                    return ext;
                }
            }
            catch { return defaultExt; }
        }
        internal static string GuessExtension(string path, bool bypass)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(path)))
                return GuessExtension(br, "bin", bypass);
        }
        internal static uint Reverse(uint x)
        {
            uint y = 0;
            for (int i = 0; i < 32; ++i)
            {
                y <<= 1;
                y |= (x & 1);
                x >>= 1;
            }
            return y;
        }
        internal static char[] Reverse(char[] charArray)
        {
            Array.Reverse(charArray);
            return charArray;
        }

        // Find Code off of Reference
        internal static int IndexOfBytes(byte[] array, byte[] pattern, int startIndex, int count)
        {
            int i = startIndex;
            int endIndex = count > 0 ? startIndex + count : array.Length;
            int fidx = 0;

            while (i++ != endIndex - 1)
            {
                if (array[i] != pattern[fidx]) i -= fidx;
                fidx = (array[i] == pattern[fidx]) ? ++fidx : 0;
                if (fidx == pattern.Length)
                    return i - fidx + 1;
            }
            return -1;
        }
        
        // Misc
        internal static string getHexString(byte[] data)
        {
            return BitConverter.ToString(data).Replace('-', ' ');
        }
        internal static void resizeJagged(ref byte[][] array, int size, int lowLen)
        {
            int oldSize = array?.Length ?? 0;
            Array.Resize(ref array, size);

            // Zero fill new data
            for (int i = oldSize; i < size - oldSize; i++)
            {
                array[i] = new byte[lowLen];
            }
        }
        internal static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        internal static int highlightText(RichTextBox RTB, string word, Color hlColor)
        {
            int ctr = 0;
            int s_start = RTB.SelectionStart, startIndex = 0, index;

            while ((index = RTB.Text.IndexOf(word, startIndex, StringComparison.Ordinal)) != -1)
            {
                RTB.Select(index, word.Length);
                RTB.SelectionColor = hlColor;

                startIndex = index + word.Length;
                ctr++;
            }

            RTB.SelectionStart = s_start;
            RTB.SelectionLength = 0;
            RTB.SelectionColor = Color.Black;
            return ctr;
        }
        // http://stackoverflow.com/questions/4820212/automatically-trim-a-bitmap-to-minimum-size
        internal static Bitmap TrimBitmap(Bitmap source)
        {
            Rectangle srcRect;
            BitmapData data = null;
            try
            {
                data = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] buffer = new byte[data.Height * data.Stride];
                Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

                int xMin = int.MaxValue,
                    xMax = int.MinValue,
                    yMin = int.MaxValue,
                    yMax = int.MinValue;

                bool foundPixel = false;

                // Find xMin
                for (int x = 0; x < data.Width; x++)
                {
                    bool stop = false;
                    for (int y = 0; y < data.Height; y++)
                    {
                        byte alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha != 0)
                        {
                            xMin = x;
                            stop = true;
                            foundPixel = true;
                            break;
                        }
                    }
                    if (stop)
                        break;
                }

                // Image is empty...
                if (!foundPixel)
                    return null;

                // Find yMin
                for (int y = 0; y < data.Height; y++)
                {
                    bool stop = false;
                    for (int x = xMin; x < data.Width; x++)
                    {
                        byte alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha != 0)
                        {
                            yMin = y;
                            stop = true;
                            break;
                        }
                    }
                    if (stop)
                        break;
                }

                // Find xMax
                for (int x = data.Width - 1; x >= xMin; x--)
                {
                    bool stop = false;
                    for (int y = yMin; y < data.Height; y++)
                    {
                        byte alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha != 0)
                        {
                            xMax = x;
                            stop = true;
                            break;
                        }
                    }
                    if (stop)
                        break;
                }

                // Find yMax
                for (int y = data.Height - 1; y >= yMin; y--)
                {
                    bool stop = false;
                    for (int x = xMin; x <= xMax; x++)
                    {
                        byte alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha != 0)
                        {
                            yMax = y;
                            stop = true;
                            break;
                        }
                    }
                    if (stop)
                        break;
                }

                srcRect = Rectangle.FromLTRB(xMin, yMin, xMax + 1, yMax + 1); // fixed; was cropping 1px too much on the max end
            }
            finally
            {
                if (data != null)
                    source.UnlockBits(data);
            }

            Bitmap dest = new Bitmap(srcRect.Width, srcRect.Height);
            Rectangle destRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            using (Graphics graphics = Graphics.FromImage(dest))
            {
                graphics.DrawImage(source, destRect, srcRect, GraphicsUnit.Pixel);
            }
            return dest;
        }
    }
}
