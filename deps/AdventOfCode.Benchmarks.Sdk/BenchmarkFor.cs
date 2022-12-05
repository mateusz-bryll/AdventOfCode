using System.Text;
using AdventOfCode.Tasks.Sdk;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode.Benchmarks.Sdk;

public abstract class BenchmarkFor<TTasks> : BenchmarkFor<TTasks, int>
    where TTasks : ITasks<int>
{
    protected BenchmarkFor(TTasks tasks) : base(tasks)
    {
    }
}

public abstract class BenchmarkFor<TTasks, TTasksResult> : BenchmarkFor<TTasks, TTasksResult, TTasksResult>
    where TTasks : ITasks<TTasksResult>
{
    protected BenchmarkFor(TTasks tasks) : base(tasks)
    {
    }
}

[MemoryDiagnoser]
public abstract class BenchmarkFor<TTasks, TFirstTaskResult, TSecondTaskResult> : IBenchmark
    where TTasks : ITasks<TFirstTaskResult, TSecondTaskResult>
{
    public int DayNumber => tasks.DayNumber;
    private readonly TTasks tasks;

    protected BenchmarkFor(TTasks tasks)
    {
        this.tasks = tasks;
    }
    
    [ParamsSource(nameof(Data))]
    public IEnumerable<string> BenchmarkInputData { get; set; } = default!;
    
    [Benchmark]
    public TFirstTaskResult Basic()
    {
        return tasks.GetBasicTaskResult(BenchmarkInputData);
    }

    [Benchmark]
    public TSecondTaskResult Advanced()
    {
        return tasks.GetAdvancedTaskResult(BenchmarkInputData);
    }
    
    public IEnumerable<IEnumerable<string>> Data => new[]
    {
        File.ReadLines(Path.Combine(AppContext.BaseDirectory, $"day-{tasks.DayNumber:D2}.inputdata"), Encoding.UTF8).ToArray(),
        File.ReadLines(Path.Combine(AppContext.BaseDirectory, $"day-{tasks.DayNumber:D2}.testdata"), Encoding.UTF8).ToArray()
    };
}
