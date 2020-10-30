using BenchmarkDotNet.Attributes;
using PerformanceTest.Models;
using System;
using System.ComponentModel;

namespace PerformanceTest
{
    [Description("FreeSql")]
    public class FreeSqlBenchmarksBase : BenchmarkBase
    {
        public override int StartIndex { get; set; } = 3;

        private IFreeSql _freeSql;

        [GlobalSetup]
        public void Setup()
        {
            GlobalSetup();

            _freeSql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.MySql, TestConfig.Instance.ConnectionString)
                .UseAutoSyncStructure(false)
                .Build();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _freeSql.Dispose();

            GlobalCleanup();
        }

        [Benchmark(Description = "Insert")]
        public int Insert()
        {
            Step();

            return _freeSql.Insert(new TestData
            {
                Id = DataIndex,
                Text = $"Insert{DataIndex}",
                CreationDate = DateTime.Now,
                LastChangeDate = DateTime.Now
            }).ExecuteAffrows();
        }

        [Benchmark(Description = "Query")]
        public TestData Query()
        {
            Step();

            return _freeSql.Select<TestData>().Where(post => post.Id == DataIndex).First();
        }

        [Benchmark(Description = "Update")]
        public int Update()
        {
            Step();

            return _freeSql.Update<TestData>(DataIndex)
                .Set(a => a.Counter1, DataIndex)
                .ExecuteAffrows();
        }

        [Benchmark(Description = "Delete")]
        public int Delete()
        {
            Step();

            return _freeSql.Delete<TestData>(new { id = DataIndex }).ExecuteAffrows();
        }
    }
}
