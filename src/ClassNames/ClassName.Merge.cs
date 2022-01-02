namespace ClassNames;

public partial class ClassName
{
    /// <summary>
    /// Merge classNames to one string
    /// Ignores null and empty values
    /// </summary>
    /// <param name="names">string classNames</param>
    /// <returns>merged className</returns>
    public static string Merge(params string?[]? names)
    {
        if (names == null) return string.Empty;
        return ExtractFromStringEnumerable(names!);
    }

    /// <summary>
    /// Merge classNames to one string
    /// Ignores null and empty values
    /// </summary>
    /// <param name="names">enumerable of string classNames</param>
    /// <returns>merged className</returns>
    public static string Merge(IEnumerable<string?>? names)
    {
        if (names == null) return string.Empty;
        return ExtractFromStringEnumerable(names);
    }

    /// <summary>
    /// Merge classNames to one string, using different strategy for different object types
    /// Supports string, IEnumerable<string>, IEnumerable<(string, bool)>, object, ClassName
    ///   string: string value or string.Empty if value is null, "class" -> "class"
    ///   IEnumerable<string?>: enumerbale items merged together, ["class1", "class2"] => "class1 class2"
    ///   IEnumerable<(string?, bool)>: First items of tuples whose second items is true, [("class1", true), ("class2", false)] => "class1"
    ///   object: all object property names whose value is true or bool.Parse parses to true, new { class1 = "true", class2 = false, class3 = true } => "class1 class3"
    ///   ClassName: output of ClassName.new().(....).compose() method, see also documentation of that method.
    /// Objects are resolved according to their type and after that merged together.
    /// </summary>
    /// <param name="objs">objects</param>
    /// <returns>merged className</returns>
    public static string Merge(params object?[]? objs)
    {
        if (objs == null) return string.Empty;

        var names = objs
            .Select(value =>
            {
                if (value is string strValue)
                    return strValue;

                if (value is IEnumerable<string?> enumerableValue)
                    return ExtractFromStringEnumerable(enumerableValue);

                if (value is IEnumerable<(string, bool)> tupleEnumerableValueBool)
                    return ExtractFromTupleEnumerable(tupleEnumerableValueBool);

                if (value is ClassName cn)
                    return cn.Compile();

                return ExtractFromObject(value);
            })
            .Where(x => !string.IsNullOrWhiteSpace(x));

        return Merge(names);
    }

    /// <summary>
    /// Optimalized version of Merge(params object?[]? objs) for all ClassName[] type arguments
    /// </summary>
    /// <param name="objs">objects</param>
    /// <returns>merged className</returns>
    public static string Merge(params ClassName[] cns)
    {
        if (cns == null) return string.Empty;

        var names = cns
            .Select(value => value.Compile())
            .Where(x => !string.IsNullOrWhiteSpace(x));

        return Merge(names);
    }

    /// <summary>
    /// Optimalized version of Merge(params object?[]? objs) for all (string?, bool)[] type arguments
    /// </summary>
    /// <param name="objs">objects</param>
    /// <returns>merged className</returns>
    public static string Merge(params (string, bool)[] tuples)
    {
        if (tuples == null) return string.Empty;
        return ExtractFromTupleEnumerable(tuples);
    }

    /// <summary>
    /// Optimalized version of Merge(params object?[]? objs) for first string type and all other ClassName[] type arguments
    /// </summary>
    /// <param name="objs">objects</param>
    /// <returns>merged className</returns>
    public static string Merge(string className, params ClassName[] cns)
    {
        if (cns == null) return string.IsNullOrWhiteSpace(className) ? string.Empty : className;

        var names = cns
            .Select(value => value.Compile())
            .Where(x => !string.IsNullOrWhiteSpace(x));

        return Merge(className, Merge(names));
    }

    /// <summary>
    /// Optimalized version of Merge(params object?[]? objs)  for first string type and all other (string?, bool)[] type arguments 
    /// </summary>
    /// <param name="objs">objects</param>
    /// <returns>merged className</returns>
    public static string Merge(string className, params (string, bool)[] tuples)
    {
        if (tuples == null) return string.IsNullOrWhiteSpace(className) ? string.Empty : className;
        return Merge(className, ExtractFromTupleEnumerable(tuples));
    }

    private static string ExtractFromStringEnumerable(IEnumerable<string?> names)
    {
        return string.Join(' ', names.Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    private static string ExtractFromObject(object? obj)
    {
        if (obj == null) return string.Empty;

        var type = obj.GetType();
        var names = type.GetProperties()
            .Select(x => (value: x.GetValue(obj), prop: x))
            .Where(x => GetBooleanFromObject(x.value))
            .Select(x => x.prop.Name);

        return ExtractFromStringEnumerable(names);
    }

    private static string ExtractFromTupleEnumerable(IEnumerable<(string className, bool when)> names)
    {
        var classNames = names
            .Where(x => !string.IsNullOrWhiteSpace(x.className) && x.when)
            .Select(x => x!.className);

        return ExtractFromStringEnumerable(classNames);
    }

    private static bool GetBooleanFromObject(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is bool boolValue)
        {
            return boolValue;
        }

        if (obj is string stringValue)
        {
            return bool.TryParse(stringValue, out var boolean) && boolean;
        }

        return false;
    }
}