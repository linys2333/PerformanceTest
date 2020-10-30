using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using PerformanceTest.Models;
using System;
using System.ComponentModel;
using System.Linq;

namespace PerformanceTest
{
    [Description("EF Core")]
    public class EfCoreBenchmarksBase : BenchmarkBase
    {
        public override int StartIndex { get; set; } = 2;

        private EFCoreContext _dbContext;

        [GlobalSetup]
        public void Setup()
        {
            GlobalSetup();

            _dbContext = new EFCoreContext(TestConfig.Instance.ConnectionString);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _dbContext.Dispose();

            GlobalCleanup();
        }

        [Benchmark(Description = "Insert")]
        public int Insert()
        {
            Step();

            _dbContext.Add(new TestData
            {
                Id = DataIndex,
                Text = $"Insert{DataIndex}",
                CreationDate = DateTime.Now,
                LastChangeDate = DateTime.Now
            });
            return _dbContext.SaveChanges();
        }

        [Benchmark(Description = "Query")]
        public TestData Query()
        {
            Step();

            return _dbContext.TestDatas.FirstOrDefault(p => p.Id == DataIndex);
        }

        [Benchmark(Description = "Query.NoTracking")]
        public TestData QueryNoTracking()
        {
            Step();

            return _dbContext.TestDatas.AsNoTracking().FirstOrDefault(p => p.Id == DataIndex);
        }

        [Benchmark(Description = "Update")]
        public int Update()
        {
            Step();

            var data = _dbContext.TestDatas.First(p => p.Id == DataIndex);
            data.Counter1 = DataIndex;
            return _dbContext.SaveChanges();
        }

        [Benchmark(Description = "Delete")]
        public int Delete()
        {
            Step();

            var data = _dbContext.TestDatas.First(p => p.Id == DataIndex);
            _dbContext.TestDatas.Remove(data);
            return _dbContext.SaveChanges();
        }
    }
}
