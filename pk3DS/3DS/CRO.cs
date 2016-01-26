using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace CTR
{
    class CRO
    {
        // Utility
        internal static void updateTB(RichTextBox RTB, string progress)
        {
            try
            {
                if (RTB.InvokeRequired)
                    RTB.Invoke((MethodInvoker)delegate
                    {
                        RTB.AppendText(Environment.NewLine + progress);
                        RTB.SelectionStart = RTB.Text.Length;
                        RTB.ScrollToCaret();
                    });
                else
                {
                    RTB.SelectionStart = RTB.Text.Length;
                    RTB.ScrollToCaret();
                    RTB.AppendText(progress + Environment.NewLine);
                }
            }
            catch { }
        }
        internal static int IndexOfBytes(byte[] array, byte[] pattern, int startIndex, int count)
        {
            int i = startIndex;
            int endIndex = count > 0 ? startIndex + count : array.Length;
            int fidx = 0;

            while (i++ != endIndex - 1)
            {
                if (array[i] != pattern[fidx]) i -= fidx;
                fidx = array[i] == pattern[fidx] ? ++fidx : 0;
                if (fidx == pattern.Length)
                    return i - fidx + 1;
            }
            return -1;
        }
        internal static string getHexString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "");
        }
        internal static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        // Checking
        internal static string[] verifyCRR(string PATH_CRR, string PATH_CRO)
        {
            // Get CRO files
            string[] CROFiles = Directory.GetFiles(PATH_CRO);

            // Weed out anything that isn't a .cro
            List<string> cros = new List<string>();
            for (int i = 0; i < CROFiles.Length; i++)
                if (Path.GetExtension(cros[i]) == ".cro")
                    cros.Add(cros[i]);
            CROFiles = cros.ToArray();

            // Store Hashes as Strings (hacky way to sort byte[]'s against eachother
            string[] hashes = new string[CROFiles.Length];
            for (int i = 0; i < hashes.Length; i++)
            {
                byte[] data = File.ReadAllBytes(CROFiles[i]);
                byte[] hash = hashCRO(ref data);
                hashes[i] = getHexString(hash).ToUpper();
            }
            Array.Sort(hashes, string.Compare);
            // Convert Hash Strings to Bytes
            byte[][] hashData = new byte[hashes.Length][];
            for (int i = 0; i < hashes.Length; i++)
                hashData[i] = StringToByteArray(hashes[i]);

            // Open the CRR
            byte[] CRR = File.ReadAllBytes(PATH_CRR);
            int hashTableOffset = BitConverter.ToInt32(CRR, 0x350);
            int hashCount = BitConverter.ToInt32(CRR, 0x354);

            // A little validation...
            if (hashCount != hashData.Length)
                throw new Exception(
                    $"Amount of input file-hashes does not equal the hash count in CRR. Expected {hashCount}, got {hashData.Length}.");

            string[] results = new string[hashData.Length];
            // Store Hashes in CRR
            for (int i = 0; i < hashData.Length; i++)
            {
                byte[] crrEntryHash = new byte[0x20];
                Array.Copy(CRR, i * 0x20 + hashTableOffset, crrEntryHash, 0, 0x20);
                results[i] = "Hash @ {0} is " + (crrEntryHash.SequenceEqual(hashData[i]) ? "valid." : "invalid.");
                Array.Copy(hashData, 0, CRR, hashTableOffset + 0x20 * i, 0x20);
            }
            return results;
        }
        internal static bool rehashCRR(string PATH_CRR, string PATH_CRO, bool saveCRO = true, bool saveCRR = true, RichTextBox TB_Progress = null, ProgressBar PB_Show = null)
        {
            // Get CRO files
            string[] CROFiles = Directory.GetFiles(PATH_CRO);

            // Weed out anything that isn't a .cro
            CROFiles = CROFiles.Where(t => Path.GetExtension(t) == ".cro").ToArray();
            // Open the CRR
            byte[] CRR = File.ReadAllBytes(PATH_CRR);
            int hashTableOffset = BitConverter.ToInt32(CRR, 0x350);
            int hashCount = BitConverter.ToInt32(CRR, 0x354);

            // A little validation...
            if (hashCount != CROFiles.Length)
            {
                updateTB(TB_Progress,
                    $"Amount of input file-hashes does not equal the hash count in CRR. Expected {hashCount}, got {CROFiles.Length}.");
                updateTB(TB_Progress, "Did not modify files. Aborting.");
                return false;
            }

            // Initialize Update Display
            if (TB_Progress == null) TB_Progress = new RichTextBox();
            if (PB_Show == null) PB_Show = new ProgressBar();
            if (PB_Show.InvokeRequired)
                PB_Show.Invoke((MethodInvoker)delegate { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = CROFiles.Length; });
            else { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = CROFiles.Length; }
            updateTB(TB_Progress, "");
            updateTB(TB_Progress, "Computing hashes for " + CROFiles.Length + " CRO files.");

            // Store Hashes as Strings (hacky way to sort byte[]'s against eachother
            string[] hashes = new string[CROFiles.Length];
            for (int i = 0; i < hashes.Length; i++)
            {
                byte[] data = File.ReadAllBytes(CROFiles[i]);
                byte[] hash = hashCRO(ref data);
                hashes[i] = getHexString(hash).ToUpper();
                if (saveCRO)
                    File.WriteAllBytes(CROFiles[i], data);

                if (PB_Show.InvokeRequired)
                    PB_Show.Invoke((MethodInvoker)(() => PB_Show.PerformStep()));
                else { PB_Show.PerformStep(); }
            }
            updateTB(TB_Progress, "Hashes computed, now sorting."); // Don't need to fiddle the ProgressBar because this should be quite quick.
            string[] hashCopy = (string[])hashes.Clone(); // Store an unsorted list for later.
            Array.Sort(hashes, string.Compare);
            // Convert Hash Strings to Bytes
            byte[][] hashData = new byte[hashes.Length][];
            for (int i = 0; i < hashes.Length; i++)
                hashData[i] = StringToByteArray(hashes[i]);

            updateTB(TB_Progress, "Hashes sorted, writing hashes to CRR.");

            // Loop to check which CROs have to be updated. Do this separate from overwriting so we don't overwrite hashes for other CROs (yet).
            int updatedCTR = 0;
            for (int i = 0; i < hashData.Length; i++)
            {
                // Check to see if the hash is currently in the table already.
                int index = IndexOfBytes(CRR, hashData[i], 0, CRR.Length);
                if (index < 0)
                {
                    // CRO was updated.
                    string file = CROFiles[Array.IndexOf(hashCopy, hashes[i])];
                    updateTB(TB_Progress, $"{Path.GetFileName(file)} hash has been updated.");
                    updatedCTR++;
                }
            }
            // Store Hashes in CRR
            for (int i = 0; i < hashData.Length; i++)
                Array.Copy(hashData[i], 0, CRR, hashTableOffset + 0x20 * i, 0x20);

            updateTB(TB_Progress,
                updatedCTR > 0
                    ? $"{updatedCTR} hashes have been updated."
                    : "CRR is fine. No modifications are necessary.");

            // Save File
            if (saveCRR && updatedCTR > 0)
            {
                File.WriteAllBytes(PATH_CRR, CRR);
                updateTB(TB_Progress, "Wrote CRR.");
            }
            else
            {
                updateTB(TB_Progress, "CRR has not been updated.");
            }
            return true;
        }
        internal static byte[] hashCRO(ref byte[] CRO)
        {
            // Allocate new byte array to store modified CRO
            SHA256 mySHA = SHA256.Create();

            // Compute the hashes
            byte[] hashH = mySHA.ComputeHash(CRO, 0x80, 0x100);
            byte[] hash0 = mySHA.ComputeHash(CRO, BitConverter.ToInt32(CRO, 0xB0), BitConverter.ToInt32(CRO, 0xB4));
            byte[] hash1 = mySHA.ComputeHash(CRO, BitConverter.ToInt32(CRO, 0xC0), BitConverter.ToInt32(CRO, 0xB8) - BitConverter.ToInt32(CRO, 0xC0));
            byte[] hash2 = mySHA.ComputeHash(CRO, BitConverter.ToInt32(CRO, 0xB8), BitConverter.ToInt32(CRO, 0xBC));

            // Set the hashes
            Array.Copy(hashH, 0, CRO, 0x00, 0x20);
            Array.Copy(hash0, 0, CRO, 0x20, 0x20);
            Array.Copy(hash1, 0, CRO, 0x40, 0x20);
            Array.Copy(hash2, 0, CRO, 0x60, 0x20);

            // Return the fixed overall hash
            return mySHA.ComputeHash(CRO, 0, 0x80);
        }
    }
}
