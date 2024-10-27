namespace VeganLife.Helpers.Extensions
{
    internal static class CollectionsExtension
    {
        public static IEnumerable<T> Add<T>(this IEnumerable<T> enumerable, T value)
        {
            return enumerable.Concat([value]);
        }

        public static IEnumerable<T> Insert<T>(this IEnumerable<T> enumerable, int index, T value)
        {
            return enumerable.SelectMany((x, i) => index == i ? [value, x] : new T[] { x });
        }

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> enumerable, int index, T value)
        {
            return enumerable.Select((x, i) => index == i ? value : x);
        }

        public static IEnumerable<T> Remove<T>(this IEnumerable<T> enumerable, int index)
        {
            return enumerable.Where((x, i) => index != i);
        }
    }
}
