using BenchmarkDotNet.Attributes;

namespace ClassNames.Benchmark;

[MemoryDiagnoser]
public class Bench
{
    [Benchmark]
    public void StringsMerge()
    {
        ClassName.Merge("c1", "c2", "", "c3", null, "c4");
    }

    [Benchmark]
    public void StringListMerge()
    {
        ClassName.Merge(new[] { "c1", "c2", "", "c3", null, "c4" });
    }

    [Benchmark]
    public void StringsBuilder()
    {
        ClassName.New()
            .Add("c1")
            .Add("c2")
            .Add("")
            .Add("c3")
            .Add((string)null!)
            .Add("c4")
            .Compile();
    }

    [Benchmark]
    public void StringListBuilder()
    {
        ClassName.New()
            .Add("c1", "c2", "", "c3", null, "c4")
            .Compile();
    }

    [Benchmark]
    public void ObjectsMerge()
    {
        ClassName.Merge(new
        {
            c1 = true,
            c2 = false,
            c3 = true,
            c4 = true
        }, new
        {
            c5 = true,
            c6 = false,
            c7 = true,
            c8 = true
        }, new
        {
            c9 = true,
            c10 = false,
            c11 = true,
            c12 = true
        });
    }

    [Benchmark]
    public void ObjectsBuild()
    {
        ClassName.New()
            .Add(new
            {
                c1 = true,
                c2 = false,
                c3 = true,
                c4 = true
            })
            .Add(new
            {
                c5 = true,
                c6 = false,
                c7 = true,
                c8 = true
            })
            .Add(new
            {
                c9 = true,
                c10 = false,
                c11 = true,
                c12 = true
            })
            .Compile();
    }

    [Benchmark]
    public void ObjectsAddBuild()
    {
        ClassName.New()
            .Add("c1", true)
            .Add("c2", false)
            .Add("c3", true)
            .Add("c4", true)
            .Add("c5", true)
            .Add("c6", false)
            .Add("c7", true)
            .Add("c8", true)
            .Add("c9", true)
            .Add("c10", false)
            .Add("c11", true)
            .Add("c12", true)
            .Compile();
    }
}
