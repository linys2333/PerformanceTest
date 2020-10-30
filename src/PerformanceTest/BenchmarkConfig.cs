using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using PerformanceTest.Colums;

namespace PerformanceTest
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            AddLogger(ConsoleLogger.Default);

            AddExporter(HtmlExporter.Default);

            AddDiagnoser(MemoryDiagnoser.Default);

            AddColumn(new TargetColum(),
                TargetMethodColumn.Method,
                new ReturnColum(),
                StatisticColumn.P50,
                StatisticColumn.P90,
                StatisticColumn.P95,
                StatisticColumn.Mean,
                StatisticColumn.Min,
                StatisticColumn.Max,
                StatisticColumn.OperationsPerSecond,
                StatisticColumn.Iterations);

            AddJob(Job.ShortRun
                .WithLaunchCount(1)
                .WithWarmupCount(2)  // 预热执行数
                .WithIterationCount(TestConfig.Instance.IterationCount)  // 迭代数，1个迭代内会循环M次，1个循环内会连续执行N次基准方法
                .WithInvocationCount(TestConfig.Instance.InvokeCount)  // 1个迭代内基准方法总执行次数，其中M=InvocationCount/N，N=UnrollFactor
                .WithUnrollFactor(10)  // 1个循环中要连续执行的次数
            );

            /*
                Benchmark执行逻辑
                
                GlobalSetup();

                for (long i = 0; i < iterationCount; i++)
                {         
                    IterationSetup();

                    for (long j = 0; j < invokeCount / unrollFactor; j++)
                    {
                        method(); // 1st call
                        method(); // 2nd call

                        method(); // (unrollFactor - 1)'th call
                        method(); // unrollFactor'th call
                    }
                    
                    IterationCleanup();
                }

                GlobalCleanup();
             */

            WithOrderer(new DefaultOrderer(SummaryOrderPolicy.Declared));
        }
    }
}