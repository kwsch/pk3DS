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
        private static readonly Dictionary<string, List<NameValue>> Settings = new();

        public static void Load(string[] lines)
        {
            Settings.Clear();
            int ctr = 0;
            while (ctr < lines.Length)
            {
                string formname = lines[ctr];
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

        public static void GetFormSettings(Form form, Control.ControlCollection controls)
        {
            if (!Settings.TryGetValue(form.Name, out var list))
                return;

            foreach (Control ctrl in controls)
            {
                GetFormSettings(form, ctrl.Controls);
                if (string.IsNullOrWhiteSpace(ctrl.Name))
                    continue;
                var pair = list.Find(z => ctrl.Name == z.Name);
                if (pair == null)
                    continue;

                TryGetValue(ctrl, pair.Value);
            }
        }

        public static void SetFormSettings(Form form, Control.ControlCollection controls)
        {
            if (!Settings.TryGetValue(form.Name, out var list))
            {
                list = new List<NameValue>();
                Settings.Add(form.Name, list);
            }

            foreach (Control ctrl in controls)
            {
                SetFormSettings(form, ctrl.Controls);
                if (string.IsNullOrWhiteSpace(ctrl.Name))
                    continue;
                var pair = list.Find(z => ctrl.Name == z.Name) ?? new NameValue(ctrl.Name);
                if (TrySetValue(ctrl, pair) && !list.Contains(pair))
                    list.Add(pair);
            }
        }

        private static void TryGetValue(Control ctrl, string s)
        {
            switch (ctrl)
            {
                case NumericUpDown nud:
                    if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var n))
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

        private static bool TrySetValue(Control ctrl, NameValue v)
        {
            switch (ctrl)
            {
                case NumericUpDown nud:
                    v.Value = nud.Value.ToString(CultureInfo.InvariantCulture);
                    return true;
                case ComboBox cb:
                    v.Value = cb.SelectedIndex.ToString();
                    return true;
                case CheckBox ck:
                    v.Value = ck.Checked.ToString();
                    return true;
                default:
                    System.Diagnostics.Debug.WriteLine($"{ctrl.Name}: unknown control type.");
                    return false;
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
