# Benchmark

Run benchmarks by running command:

```bash
./benchmark.ps1
```

## Results 


|            Method |       Mean |    Error |   StdDev |  Gen 0 | Allocated |
|------------------ |-----------:|---------:|---------:|-------:|----------:|
|      StringsMerge |   167.5 ns |  3.23 ns |  4.53 ns | 0.0401 |     168 B |
|   StringListMerge |   175.7 ns |  3.52 ns |  3.12 ns | 0.0401 |     168 B |
|    StringsBuilder |   245.0 ns |  4.31 ns |  5.76 ns | 0.0553 |     232 B |
| StringListBuilder |   299.3 ns |  5.87 ns |  7.42 ns | 0.0839 |     352 B |
|      ObjectsMerge | 3,036.9 ns | 29.48 ns | 26.14 ns | 0.4807 |   2,024 B |
|      ObjectsBuild | 3,005.3 ns | 54.59 ns | 76.52 ns | 0.4539 |   1,904 B |
|   ObjectsAddBuild |   512.5 ns |  9.87 ns | 15.08 ns | 0.1202 |     504 B |
