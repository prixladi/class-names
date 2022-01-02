namespace ClassNames;

/// <summary>
/// ClassNames builder class
/// Supports: (isDisabled = true, isFocused = false, clickable = true)
///  1) object oriented builder pattern 
///     ClassName.New("class1 class2").Add("class3", isDisabled).Add("class4", isFocused).Add("class5", clickable).Build();
///     -> "class1 class2 class3 class5"
///  2) params ready merge pattern           
///     ClassName.Merge("class1 class2", new[] {("class3", isDisabled), ("class4", isFocused)}, new[] {("class4", clickable)})
///     -> "class1 class2 class3 class5"
/// </summary>
public partial class ClassName
{
    private readonly List<string> classNames;

    private ClassName()
    {
        classNames = new List<string>();
    }

    /// <summary>
    /// Creates new instance of ClassName
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static ClassName New(string? className = null)
    {
        var cn = new ClassName();
        cn.Add(className);

        return cn;
    }

    /// <summary>
    /// Adds className if it is not null or whitespace and when is true (defaults to true), ("class1", true) => "class1"
    /// </summary>
    /// <returns>ClassName instance to chain</returns>
    public ClassName Add(string? name, bool when = true)
    {
        if (!string.IsNullOrWhiteSpace(name) && when)
            classNames.Add(name);

        return this;
    }

    /// <summary>
    /// Add class composed from classNames in array, ["class1", "class2", null, '', '  '] => "class1 class2"
    /// </summary>
    /// <returns>ClassName instance to chain</returns>
    public ClassName Add(params string?[]? names)
    {
        var merged = Merge(names);
        if (!string.IsNullOrWhiteSpace(merged))
            classNames.Add(merged);

        return this;
    }

    /// <summary>
    /// Add class composed from first items of tuples whose second items is true, [("class1", true), ("class2", false)] => "class1"
    /// </summary>
    /// <returns>ClassName instance to chain</returns>
    public ClassName Add(IEnumerable<(string, bool)> names)
    {
        var merged = Merge(names);
        if (!string.IsNullOrWhiteSpace(merged))
            classNames.Add(merged);

        return this;
    }

    /// <summary>
    /// Add class composed using all object property names whose value is true or bool.Parse parses to true, new { class1 = "true", class2 = false, class3 = true } => "class1 class3"
    /// </summary>
    /// <returns>ClassName instance to chain</returns>
    public ClassName Add(object? obj)
    {
        var merged = ExtractFromObject(obj);
        if (!string.IsNullOrWhiteSpace(merged))
            classNames.Add(merged);

        return this;
    }

    /// <summary>
    /// Add class composed using output of C.new().(....).compose() method, see also documentation of that method
    /// </summary>
    /// <returns>ClassName instance to chain</returns>
    public ClassName Add(ClassName cn, bool when = true)
    {
        var merged = cn.Compile();
        if (!string.IsNullOrWhiteSpace(merged) && when)
            classNames.Add(merged);

        return this;
    }

    /// <summary>
    /// Compiles classNames together
    /// If nullIfWhiteSpace is set to true, returns null insted of whitespace if classNames composes to whitespace
    /// </summary>
    /// <returns>ClassName instance to chain</returns>
    public string? Compile(bool nullIfWhiteSpace = false)
    {
        var merged = Merge(classNames);
        return nullIfWhiteSpace && string.IsNullOrWhiteSpace(merged) ? null : merged;
    }
}