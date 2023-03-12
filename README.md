# ClassNames

![CI](https://github.com/prixladi/class-names/workflows/CI/badge.svg)
[![NuGet](https://img.shields.io/nuget/dt/classnames.svg)](https://www.nuget.org/packages/ClassNames/) 
[![NuGet](https://img.shields.io/nuget/vpre/classnames.svg)](https://www.nuget.org/packages/ClassNames/)

ClassNames is a simple utility for composing **CSS** class names.

## API

ClassName can be composed using different objects. Each object type has a different way how to be converted to a class name. Different objects can be used mixed in one call / method chain.

- **string**: string value or string.Empty if value is null, `"class" -> "class"`
- **IEnumerable<string>**: items merged together, `["class1", null, '', "class2"] => "class1 class2"`
- **IEnumerable<(string, bool)>**: First items of tuples whose second items is true, `[("class1", true), ("class2", false)] => "class1"`
- **object**: all object property names whose value is true or **bool.Parse** returns true, `new { class1 = "true", class2 = false, class3 = true } => "class1 class3"`

Utility supports two different patterns:

- Object builder pattern. 

```csharp
var className = ClassName.New("pl-10 pr-10")
    .Add("opacity-50", isDisabled)
    .Add("underline", isFocused)
    .Add("click:animateScale", clickable)
    .Compile();
```

- Static Merge function, in some cases can be more compact than builder.

```csharp
var className = ClassName.Merge("pl-10 pr-10", 
    new[] {("opacity-50", isDisabled), ("underline", isFocused)}, 
    new[] {("click:animateScale", clickable)});
```

## Examples

In each example, you can see equivalent notations using the builder and merge patterns

### Strings and (string, bool) tuples

Probably most of the use cases out there. Every use case is written using just strings and (string, bool) tuples. Other objects just provide some convenience advantages in some cases.

1) Builder

```csharp
var className = ClassName.New("mt-10")
    .Add("underline", current.id == item.id)
    .Add("opacity-50", current.id != item.id)
    .Compile();
```

2) Merge

```csharp
var className = ClassName.Merge("mt-10", new [] 
{ 
    ("underline", current.id == item.id), 
    ("opacity-50", current.id != item.id)
});
```

### Objects

This would be the preferred way over tuples but there is no way to create an anonymous object with property containing dash, eg. `pt-10`. It is a real dealbreaker because it is a common naming style for **CSS** classes. ([Tailwind](https://tailwindcss.com/) for example)

1) Builder

```csharp
var className = ClassName.New("mt-10")
    .Add(new 
    { 
        roundedFull = isCircle,
        roundedOff = isSquare,
        hidden = isHiden
    })
    .Compile();
```

2) Merge

```csharp
var className = ClassName.Merge("mt-10", new 
{ 
    roundedFull = isCircle,
    roundedOff = isSquare,
    hidden = isHiden
});
```

### ClassName Combine

Can be used to combine existing ClassName instances.

1) Builder

```csharp
var main = ClassName.New("text-black")
    .Add("text-sm", showSmall)
    .Add("disabled", isDisabled);

var highlighted = ClassName.New("scale-110")
    .Add("bg-red", currentUser.id == item.user.id)
    .Add("bg-teal", currentUser.id != item.user.id);

var className = ClassName.New()
    .Add(main)
    .Add(highlighted, shouldHighlight)
    .Compile();
```

2) Merge

```csharp
var main = ClassName.New(ClassName.Merge(new [] 
    { 
        ("text-sm", showSmall), 
        ("disabled", isDisabled) 
    }));

var highlighted = ClassName.New(ClassName.Merge(
    "scale-110",
    new [] 
    { 
        ("bg-red", currentUser.id == item.user.id), 
        ("bg-teal", currentUser.id != item.user.id) 
    }));

var className = ClassName.Merge(
    main, new [] {(highlighted.Compile(), shouldHighlight)});

```

### Ternary Builder

This is a builder-only feature. It is jus a syntax sugar for ternary-like conditions for classes. It uses the first provided class if the predicate is true otherwise returns the second provided class.

1) Without ternary

```csharp
var className = ClassName.New()
    .Add("disbaled", isDisabled)
    .Add("bg-red", !isDisabled)
    .Compose();
```

2) With ternary

```csharp
var className = ClassName.New()
    .Ternary("disbaled", "bg-red", isDisabled)
    .Compose();
```

## Tests

Project with unit tests can be found in folder [**src/ClassNames.UnitTests**](./src/ClassNames.UnitTests).

Tests can be run directly in visual studio or from the command line with the following commands:

```bash
cd src/ClassNames.UnitTests
./test.ps1
```

After that coverage results can be found in folder [**src/ClassNames.UnitTests/TestsResults**](./src/ClassNames.UnitTests/TestsResults) ([**index.html**](src/ClassNames.UnitTests/TestsResults/index.html)).

## Benchmarks

Project with benchamarks can be found in folder [**src/ClassNames.Benchmark**](./src/ClassNames.Benchmark).
