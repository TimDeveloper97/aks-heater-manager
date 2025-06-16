namespace AquilaService;

/// <summary>
/// list extension
/// </summary>
public static class ListHelper
{
    /// <summary>
    /// Contains key
    /// </summary>
    /// <param name="lst">lst</param>
    /// <param name="key">key</param>
    /// <returns>bool</returns>
    public static bool ContainsKey(this List<(string, object)>? lst, string key)
    {
        if (lst is null || lst.Count == 0)
            return false;

        return lst.Any(x => x.Item1 == key);
    }

    /// <summary>
    /// Contains value
    /// </summary>
    /// <param name="lst">lst</param>
    /// <param name="value">value</param>
    /// <returns>bool</returns>
    public static bool ContainsValue(this List<(string, object)>? lst, object value)
    {
        if (lst is null || lst.Count == 0)
            return false;

        return lst.Any(x => x.Item2?.ToString() == value.ToString());
    }

    /// <summary>
    /// First of default list by key
    /// </summary>
    /// <param name="lst">lst</param>
    /// <param name="key">key</param>
    /// <returns>first data by key</returns>
    public static (string, object)? FirstOrDefault(this List<(string, object)>? lst, string key)
    {
        if (lst is null || lst.Count == 0)
            return null;

        var first = lst.FirstOrDefault(x => x.Item1.Trim() == key);
        return first;
    }

    /// <summary>
    /// First or default data
    /// </summary>
    /// <param name="lst">lst</param>
    /// <returns>first or default data</returns>
    public static (string, object)? FirstOrDefault(this List<(string, object)>? lst)
    {
        if (lst is null || lst.Count == 0)
            return null;

        var first = lst.FirstOrDefault();
        return first;
    }

    /// <summary>
    /// get list first element in list tuple
    /// </summary>
    /// <param name="lst">lst</param>
    /// <returns>list string</returns>
    public static List<string>? GetKeys(this List<(string, object)>? lst)
    {
        if (lst is null || lst.Count == 0)
            return null;

        var result = lst.Select(x => x.Item1).ToList();
        return result;
    }

    /// <summary>
    /// get list second element in list tuple
    /// </summary>
    /// <param name="lst">lst</param>
    /// <returns>list object</returns>
    public static List<object>? GetValues(this List<(string, object)>? lst)
    {
        if (lst is null || lst.Count == 0)
            return null;

        var result = lst.Select(x => x.Item2).ToList();
        return result;
    }

    /// <summary>
    /// where function
    /// </summary>
    /// <param name="lst">list data</param>
    /// <param name="key">key</param>
    /// <returns>new list with filter by key</returns>
    public static IEnumerable<(string, object)>? Where(this List<(string, object)>? lst, string key)
    {
        if (lst is null || lst.Count == 0)
            return null;

        var where = lst.Where(x => x.Item1 == key);
        return where;
    }

    /// <summary>
    /// get value by key
    /// </summary>
    /// <param name="lst">list data</param>
    /// <param name="key">key</param>
    /// <returns>an object filter by key</returns>
    public static object? GetValue(this List<(string, object)>? lst, string key)
    {
        if (lst is null || lst.Count == 0)
            return null;

        var first = lst.FirstOrDefault(x => x.Item1 == key);
        return first.Item2;
    }

    /// <summary>
    /// set value
    /// </summary>
    /// <param name="lst">list data</param>
    /// <param name="key">key</param>
    /// <param name="obj">object</param>
    public static void SetValue(this List<(string, object)>? lst, string key, object obj)
    {
        if (lst is null || lst.Count == 0)
            return;

        var first = lst.FirstOrDefault(x => x.Item1 == key);
        var index = lst.IndexOf(first);

        lst[index] = (key, obj);
    }

    public static List<List<T>> CartesianProduct<T>(this List<List<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> result = new[] { Enumerable.Empty<T>() };

        foreach (var sequence in sequences)
        {
            result = from seq in result
                     from item in sequence
                     select seq.Concat(new[] { item });
        }

        return result.Select(r => r.ToList()).ToList();
    }
}
