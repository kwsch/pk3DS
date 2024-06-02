using System.Windows.Forms;

namespace pk3DS.WinForms;

public static class FormUtil
{
    // Utility (Shared)
    internal static void SetForms(int species, ComboBox cb, string[][] AltForms)
    {
        cb.Items.Clear();
        string[] forms = AltForms[species];
        if (forms.Length < 2)
        {
            cb.Items.Add("");
            cb.Enabled = false;
        }
        else
        {
            cb.Items.AddRange(forms);
            cb.Enabled = true;
        }
        cb.SelectedIndex = 0;
    }
}