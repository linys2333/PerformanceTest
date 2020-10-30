using BenchmarkDotNet.Attributes;
using PerformanceTest.Models;
using SqlSugar;
using System;
using System.ComponentModel;

namespace PerformanceTest
{
    [Description("SqlSugar")]
    public class SqlSugarBenchmarksBase : BenchmarkBase
    {
        public override int StartIndex { get; set; } = 4;

        private SqlSugarClient _sqlSugar;

        [GlobalSetup]
        public void Setup()
        {
            GlobalSetup();

            _sqlSugar = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = TestConfig.Instance.ConnectionString,
                DbType = DbType.MySql,
                IsAutoCloseConnection = false,
                IsShardSameThread = true,
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings
                {
                    MySqlDisableNarvchar = true
                }
            });
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _sqlSugar.Close();
            _sqlSugar.Dispose();

            GlobalCleanup();
        }

        [Benchmark(Description = "Insert")]
        public int Insert()
        {
            Step();

            return _sqlSugar.Insertable(new TestData
            {
                Id = DataIndex,
                Text = $"Insert{DataIndex}",
                CreationDate = DateTime.Now,
                LastChangeDate = DateTime.Now
            }).ExecuteCommand();
        }

        [Benchmark(Description = "Query")]
        public TestData Query()
        {
            Step();

            return _sqlSugar.Queryable<TestData>().Where(post => post.Id == DataIndex).First();
        }

        [Benchmark(Description = "Update")]
        public int Update()
        {
            Step();

            return _sqlSugar.Updateable<TestData>()
                .SetColumns(a => new TestData { Counter1 = DataIndex })
                .Where(a => a.Id == DataIndex).ExecuteCommand();
        }

        [Benchmark(Description = "Delete")]
        public int Delete()
        {
            Step();

            return _sqlSugar.Deleteable<TestData>().In(DataIndex).ExecuteCommand();
        }
    }
}
