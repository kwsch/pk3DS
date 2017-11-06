using System;
using System.Collections.Generic;
using System.Linq;

namespace pk3DS.Core
{
    public static class TableUtil
    {
        private const string sep = "\t";

        public static string GetTable<T>(IEnumerable<T> arr, IList<string> names, string name = null) where T : new()
        {
            var list = GetTableRaw(arr).ToArray();

            // slap in name to column header
            list[0] = $"Index{sep}{name ?? typeof(T).Name}{sep}{list[0]}";

            // slap in row name to row
            for (int i = 1; i < list.Length; i++)
                list[i] = $"{i - 1}{sep}{names[i - 1]}{sep}{list[i]}";

            return string.Join(Environment.NewLine, list);
        }
        public static string GetTable<T>(IEnumerable<T> arr) where T : new()
        {
            return string.Join(Environment.NewLine, GetTableRaw(arr));
        }

        private static IEnumerable<string> GetTableRaw<T>(IEnumerable<T> arr) where T : new()
        {
            return Table(arr).Select(row => string.Join(sep, row));
        }
        private static IEnumerable<IEnumerable<string>> Table<T>(IEnumerable<T> arr) where T : new()
        {
            var type = typeof(T);
            yield return GetNames(type);
            foreach (var z in arr)
                yield return GetValues(z, type);
        }
        private static IEnumerable<string> GetNames(Type type)
        {
            foreach (var z in type.GetProperties())
                yield return z.Name;
            foreach (var z in type.GetFields())
                yield return z.Name;
        }
        private static IEnumerable<string> GetValues(object obj, Type type)
        {
            foreach (var z in type.GetProperties())
                yield return z.GetValue(obj, null)?.ToString() ?? "";
            foreach (var z in type.GetFields())
                yield return z.GetValue(obj)?.ToString() ?? "";
        }
    }
}
