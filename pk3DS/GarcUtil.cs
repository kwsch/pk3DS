using pk3DS.Core.CTR;
using System;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace pk3DS
{
    /// <summary>
    /// Windows-forms friendly wrapper for GARC functions
    /// </summary>
    public static class GarcUtil
    {
        private static ProgressBar pBar1;
        private static Label label;

        private static void GARC_FileCountDetermined(object sender, GARC.FileCountDeterminedEventArgs e)
        {
            if (pBar1 == null) pBar1 = new ProgressBar();
            if (pBar1.InvokeRequired)
                pBar1.Invoke((MethodInvoker)delegate { pBar1.Minimum = 0; pBar1.Step = 1; pBar1.Value = 0; pBar1.Maximum = e.Total; });
            else { pBar1.Minimum = 0; pBar1.Step = 1; pBar1.Value = 0; pBar1.Maximum = e.Total; }
            if (label == null) label = new Label();
            if (label.InvokeRequired)
                label.Invoke((MethodInvoker)delegate { label.Visible = true; });
        }

        private static void GARC_PackProgressed(object sender, GARC.PackProgressedEventArgs e)
        {
            if (pBar1.InvokeRequired)
                pBar1.Invoke((MethodInvoker)(() => pBar1.PerformStep()));
            else { pBar1.PerformStep(); }
            string update = $"{(float)e.Current / (float)e.Total:P2} - {e.Current}/{e.Total} - {e.CurrentFile}";
            if (label.InvokeRequired)
                label.Invoke((MethodInvoker)delegate { label.Text = update; });
            else { label.Text = update; }
        }

        private static void GARC_UnpackProgressed(object sender, GARC.UnpackProgressedEventArgs e)
        {
            #region Step
            if (pBar1.InvokeRequired) pBar1.Invoke((MethodInvoker)(() => pBar1.PerformStep()));
            else pBar1.PerformStep();

            string update = $"{e.Current / e.Total:P2} - {e.Current}/{e.Total}";
            if (label.InvokeRequired)
                label.Invoke((MethodInvoker)delegate { label.Text = update; });
            else { label.Text = update; }
            #endregion
        }

        public static bool garcPackMS(string folderPath, string garcPath, int version, int bytesPadding, ProgressBar pBar1 = null, Label label = null, bool supress = false)
        {
            GARC.FileCountDetermined += GARC_FileCountDetermined;
            GARC.PackProgressed += GARC_PackProgressed;
            try
            {              
                var filectr = GARC.garcPackMS(folderPath, garcPath, version, bytesPadding);
                if (filectr > 0)
                {
                    // We're done.
                    SystemSounds.Exclamation.Play();
                    if (!supress) WinFormsUtil.Alert("Pack Successful!", filectr + " files packed to the GARC!");
                }

                if (label.InvokeRequired)
                    label.Invoke((MethodInvoker)delegate { label.Visible = false; });
                else { label.Visible = false; }

                return true;
            }
            catch (DirectoryNotFoundException)
            {
                if (!supress) WinFormsUtil.Error("Folder does not exist.");
            }
            catch (Exception e)
            {
                WinFormsUtil.Error("Packing failed", e.ToString());
            }
            finally
            {
                GARC.FileCountDetermined -= GARC_FileCountDetermined;
                GARC.PackProgressed -= GARC_PackProgressed;
            }
            return false;
        }

        public static bool garcUnpack(string garcPath, string outPath, bool skipDecompression, ProgressBar pBar1 = null, Label label = null, bool supress = false, bool bypassExt = false)
        {
            GARC.FileCountDetermined += GARC_FileCountDetermined;
            GARC.UnpackProgressed += GARC_UnpackProgressed;
            try
            {
                var fileCount = GARC.garcUnpack(garcPath, outPath, skipDecompression);
                if (fileCount > 0)
                {
                    SystemSounds.Exclamation.Play();
                    if (!supress) WinFormsUtil.Alert("Unpack Successful!", fileCount + " files unpacked from the GARC!");
                }

                if (label == null)
                    return true;
                if (label.InvokeRequired)
                    label.Invoke((MethodInvoker)delegate { label.Visible = false; });
                else
                    label.Visible = false;
                return true;
            }
            catch (FileNotFoundException)
            {
                WinFormsUtil.Alert("File does not exist");
            }
            finally
            {
                GARC.FileCountDetermined -= GARC_FileCountDetermined;
                GARC.UnpackProgressed -= GARC_UnpackProgressed;
            }
            return false;
        }
    }
}
