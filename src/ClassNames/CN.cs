namespace ClassNames;

/// <summary>
/// ClassNames builder class
/// Supports: (isDisabled = true, isFocused = false, clickable = true)
///  1) object oriented builder pattern 
///     CN.New("class1 class2").Add("class3", isDisabled).Add("class4", isFocused).Add("class5", clickable).Build();
///     -> "class1 class2 class3 class5"
///  2) params ready merge pattern           
///     CN.Merge("class1 class2", new[] {("class3", isDisabled), ("class4", isFocused)}, new[] {("class4", clickable)})
///     -> "class1 class2 class3 class5"
/// </summary>
public partial class CN
{
    private readonly List<string> classNames;

    private CN()
    {
        classNames = new List<string>();
    }

    /// <summary>
    /// Creates new instance of CN
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static CN New(string? className = null)
    {
        var cn = new CN();
        cn.Add(className);

        return cn;
    }

    /// <summary>
    /// Adds className if it is not null or whitespace, ("class1") => "class1"
    /// </summary>
    /// <returns>CN instance to chain</returns>
    public CN Add(string? className)
    {
        Add(className, true);

        return this;
    }

    /// <summary>
    /// Adds className if it is not null or whitespace and value is true, ("class1", true) => "class1"
    /// </summary>
    /// <returns>CN instance to chain</returns>
    public CN Add(string? name, bool value)
    {
        if (!string.IsNullOrWhiteSpace(name) && value)
            classNames.Add(name);

        return this;
    }

    /// <summary>
    /// Add class composed from classNames in array, ["class1", "class2", null, '', '  '] => "class1 class2"
    /// </summary>
    /// <returns>CN instance to chain</returns>
    public CN Add(params string?[]? names)
    {
        var merged = Merge(names);
        if (!string.IsNullOrWhiteSpace(merged))
            classNames.Add(merged);

        return this;
    }

    /// <summary>
    /// Add class composed from first items of tuples whose second items is true, [("class1", true), ("class2", false)] => "class1"
    /// </summary>
    /// <returns>CN instance to chain</returns>
    public CN Add(IEnumerable<(string?, bool)> names)
    {
        var merged = Merge(names);
        if (!string.IsNullOrWhiteSpace(merged))
            classNames.Add(merged);

        return this;
    }

    /// <summary>
    /// Add class composed using all object property names whose value is true or bool.Parse parses to true, new { class1 = "true", class2 = false, class3 = true } => "class1 class3"
    /// </summary>
    /// <returns>CN instance to chain</returns>
    public CN Add(object? obj)
    {
        var merged = ExtractFromObject(obj);
        if (!string.IsNullOrWhiteSpace(merged))
            classNames.Add(merged);

        return this;
    }

    /// <summary>
    /// Add class composed using output of C.new().(....).compose() method, see also documentation of that method
    /// </summary>
    /// <returns>CN instance to chain</returns>
    public CN Add(CN cn)
    {
        var merged = cn.Compile();
        if (!string.IsNullOrWhiteSpace(merged))
            classNames.Add(merged);

        return this;
    }

    /// <summary>
    /// Compiles classNames together
    /// If nullIfWhiteSpace is set to true, returns null insted of whitespace if classNames composes to whitespace
    /// </summary>
    /// <returns>CN instance to chain</returns>
    public string? Compile(bool nullIfWhiteSpace = false)
    {
        var merged = Merge(classNames);
        return nullIfWhiteSpace && string.IsNullOrWhiteSpace(merged) ? null : merged;
    }
}