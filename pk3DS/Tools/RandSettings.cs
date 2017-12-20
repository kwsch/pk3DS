using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS
{
    public static class RandSettings
    {
        public const string FileName = "randsettings.txt";
        private static readonly Dictionary<string, List<NameValue>> Settings = new Dictionary<string, List<NameValue>>();
        public static void Load(string[] lines)
        {
            Settings.Clear();
            int ctr = 0;
            while (ctr < lines.Length)
            {
                string formname = lines[0];
                int end = Array.FindIndex(lines, ctr, string.IsNullOrWhiteSpace);
                var list = GetList(lines, ctr + 1, end - 1);
                Settings.Add(formname, list);
                ctr = end + 1;
            }
        }
        public static string[] Save()
        {
            var result = new List<string>();
            foreach (var list in Settings)
            {
                result.Add(list.Key);
                result.AddRange(list.Value.Select(val => val.Write()));
                result.Add(string.Empty);
            }
            return result.ToArray();
        }

        public static void LoadFormSettings(Form form, Control.ControlCollection controls)
        {
            if (!Settings.TryGetValue(form.Name, out var list))
                return;

            foreach (Control ctrl in controls)
            {
                LoadFormSettings(form, ctrl.Controls);
                var pair = list.FirstOrDefault(z => ctrl.Name == z.Name);
                if (pair == null)
                    continue;

                TryGetValue(ctrl, pair.Value);
            }
        }
        public static void SaveFormSettings(Form form, Control.ControlCollection controls)
        {
            if (!Settings.TryGetValue(form.Name, out var list))
                return;

            foreach (Control ctrl in controls)
            {
                SaveFormSettings(form, ctrl.Controls);
                var pair = list.FirstOrDefault(z => ctrl.Name == z.Name) ?? new NameValue(ctrl.Name);
                TrySetValue(ctrl, pair);
            }
        }
        private static void TryGetValue(Control ctrl, string s)
        {
            switch (ctrl)
            {
                case NumericUpDown nud:
                    if (decimal.TryParse(s, out var n))
                        nud.Value = n;
                    break;
                case ComboBox cb:
                    if (int.TryParse(s, out var c))
                        cb.SelectedIndex = c;
                    break;
                case CheckBox ck:
                    if (bool.TryParse(s, out var b))
                        ck.Checked = b;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine($"{ctrl.Name}: unknown control type.");
                    break;
            }
        }
        private static void TrySetValue(Control ctrl, NameValue v)
        {
            switch (ctrl)
            {
                case NumericUpDown nud:
                    v.Value = nud.Value.ToString(CultureInfo.InvariantCulture);
                    break;
                case ComboBox cb:
                    v.Value = cb.SelectedIndex.ToString();
                    break;
                case CheckBox ck:
                    v.Value = ck.Checked.ToString();
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine($"{ctrl.Name}: unknown control type.");
                    break;
            }
        }

        private static List<NameValue> GetList(IList<string> lines, int start, int end)
        {
            var list = new List<NameValue>();
            for (int i = start; i <= end; i++)
            {
                var val = new NameValue(lines[i]);
                if (val.Name != null)
                    list.Add(val);
            }
            return list;
        }

        private class NameValue
        {
            public readonly string Name;
            public string Value;

            private const char Separator = '\t';
            public string Write() => Name + Separator + Value;

            public NameValue(string s)
            {
                var split = s.Split(Separator);
                Name = split[0];
                if (split.Length < 2)
                    return;
                Value = split[1];
            }
        }
    }
}
