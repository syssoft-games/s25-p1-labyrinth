using System;
using System.Collections.Generic;

public static class IListExtensions
{
    #region INDEX_CHECK

    public static bool IsInRange<T>(this IList<T> list, int index)
    {
        return index >= 0 && index < list.Count;
    }

    public static bool IsOutOfRange<T>(this IList<T> list, int index)
    {
        return index < 0 || index >= list.Count;
    }

    #endregion

    #region INDEX_SINGLE

    /// <summary>
    /// Versucht, ein Element am angegebenen Index zu ermitteln.
    /// </summary>
    public static bool TryGetAtIndex<T>(this IList<T> list, int index, out T item)
    {
        if (list.IsOutOfRange(index))
        {
            item = default;
            return false;
        }

        item = list[index];
        return true;
    }

    /// <summary>
    /// Versucht, das auf ein gegebenes Element folgende Element zu ermitteln.
    /// </summary>
    public static bool TryGetNext<T>(this IList<T> list, int index, out T item)
    {
        var nextIndex = index + 1;
        return TryGetAtIndex(list, nextIndex, out item);
    }

    /// <summary>
    /// Versucht, das einem gegebenen Element vorhergehende Element zu ermitteln.
    /// </summary>
    public static bool TryGetPrevious<T>(this IList<T> list, int index, out T item)
    {
        var previousIndex = index - 1;
        return TryGetAtIndex(list, previousIndex, out item);
    }

    /// <summary>
    /// Versucht, das erste Element der Liste zu ermitteln.
    /// </summary>
    public static bool TryGetFirst<T>(this IList<T> list, out T item)
    {
        if (list.Count > 0)
        {
            item = list[0];
            return true;
        }

        item = default;
        return false;
    }

    /// <summary>
    /// Versucht, das letzte Element der Liste zu ermitteln.
    /// </summary>
    public static bool TryGetLast<T>(this IList<T> list, out T item)
    {
        if (list.Count > 0)
        {
            item = list[^1];
            return true;
        }

        item = default;
        return false;
    }

    #endregion

    #region INDEX_ALL

    /// <summary>
    /// Ermittelt die Indizes aller Elemente, die ein Prädikat erfüllen.
    /// </summary>
    public static List<int> GetIndices<T>(this IList<T> list, Predicate<T> predicate)
    {
        var indices = new List<int>();
        if (list == null || predicate == null) return indices;
        for (int i = 0; i < list.Count; i++)
        {
            if (predicate(list[i]))
                indices.Add(i);
        }

        return indices;
    }

    /// <summary>
    /// Versucht, Elemente an bestimmten Indizes zu ermitteln.
    /// </summary>
    public static bool TryGetAtIndices<T>(this IList<T> list, int[] indices, out List<T> items)
    {
        items = new List<T>();
        if (list == null || indices == null) return false;
        foreach (var idx in indices)
        {
            if (idx >= 0 && idx < list.Count)
                items.Add(list[idx]);
        }

        return items.Count > 0;
    }

    #endregion

    #region FILTER_SINGLE

    /// <summary>
    /// Versucht, das erste Element zu ermitteln, das ein Prädikat erfüllt.
    /// </summary>
    public static bool TryFindFirst<T>(this IList<T> source, Predicate<T> predicate, out T item)
    {
        foreach (var elem in source)
        {
            if (predicate(elem))
            {
                item = elem;
                return true;
            }
        }

        item = default;
        return false;
    }

    /// <summary>
    /// Versucht, das letzte Element zu ermitteln, das ein Prädikat erfüllt.
    /// </summary>
    public static bool TryFindLast<T>(this IList<T> source, Predicate<T> predicate, out T item)
    {
        item = default;
        bool found = false;
        foreach (var elem in source)
        {
            if (predicate(elem))
            {
                item = elem;
                found = true;
            }
        }

        return found;
    }

    /// <summary>
    /// Sucht wahlweise das erste oder das letzte Element nach einem Prädikat.
    /// </summary>
    public static bool TryFind<T>(this IList<T> source, Predicate<T> predicate, bool first, out T item)
    {
        if (first)
            return source.TryFindFirst(predicate, out item);
        else
            return source.TryFindLast(predicate, out item);
    }

    public static bool TryGetTypeOf<T, TSpecific>(this IList<T> source, out TSpecific item)
        where TSpecific : T
    {
        foreach (var elem in source)
        {
            if (elem is TSpecific specificElem)
            {
                item = specificElem;
                return true;
            }
        }

        item = default;
        return false;
    }

    #endregion

    #region FILTER_ALL

    /// <summary>
    /// Gibt eine Liste aller Elemente zurück, die das angegebene Prädikat erfüllen.
    /// </summary>
    public static List<T> GetWhere<T>(this IList<T> source, Predicate<T> predicate)
    {
        var result = new List<T>();
        foreach (var elem in source)
        {
            if (predicate(elem))
                result.Add(elem);
        }

        return result;
    }

    /// <summary>
    /// Versucht, alle Elemente zu ermitteln, die ein Prädikat erfüllen.
    /// </summary>
    public static bool TryGetWhere<T>(this IList<T> source, Predicate<T> predicate, out List<T> items)
    {
        items = GetWhere(source, predicate);
        return items.Count > 0;
    }

    #endregion

    #region COLLECTIONS

    public static bool Enqueue<T>(this IList<T> list, T item, bool allowDuplicates = false)
    {
        if (!allowDuplicates && list.Contains(item))
            return false;

        list.Add(item);
        return true;
    }

    public static bool Dequeue<T>(this IList<T> list, out T result)
    {
        if (list.Count == 0)
        {
            result = default;
            return false;
        }

        result = list[0];
        list.RemoveAt(0);
        return true;
    }

    public static bool Push<T>(this IList<T> list, T item, bool allowDuplicates = false)
    {
        if (!allowDuplicates && list.Contains(item))
            return false;

        list.Add(item);
        return true;
    }

    public static bool Pop<T>(this IList<T> list, out T result)
    {
        if (list.Count == 0)
        {
            result = default;
            return false;
        }

        result = list[^1];
        list.RemoveAt(list.Count - 1);
        return true;
    }

    #endregion

    public static bool Any<T>(this IList<T> list, Predicate<T> predicate)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (predicate(list[i]))
                return true;
        }
        return false;
    }
    public static bool All<T>(this IList<T> list, Predicate<T> predicate)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!predicate(list[i]))
                return false;
        }
        return true;
    }
    public static bool None<T>(this IList<T> list, Predicate<T> predicate)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (predicate(list[i]))
                return false;
        }
        return true;
    }
}