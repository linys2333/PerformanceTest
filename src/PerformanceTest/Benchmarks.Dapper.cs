using BenchmarkDotNet.Attributes;
using Dapper;
using MySql.Data.MySqlClient;
using PerformanceTest.Models;
using System.ComponentModel;
using System.Linq;

namespace PerformanceTest
{
    [Description("Dapper")]
    public class DapperBenchmarksBase : BenchmarkBase
    {
        private MySqlConnection _connection;

        public override int StartIndex { get; set; } = 1;

        [GlobalSetup]
        public void Setup()
        {
            GlobalSetup();

            _connection = new MySqlConnection(TestConfig.Instance.ConnectionString);
            _connection.Open();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _connection.Close();
            _connection.Dispose();

            GlobalCleanup();
        }

        [Benchmark(Description = "Insert")]
        public long Insert()
        {
            Step();

            return _connection.Execute($@"INSERT INTO TestDatas (Id, Text, CreationDate, LastChangeDate ) VALUES ({DataIndex}, 'Insert{DataIndex}', now(), now());");
        }

        [Benchmark(Description = "Query")]
        public TestData Query()
        {
            Step();

            return _connection.Query<TestData>("SELECT * FROM TestDatas WHERE Id = @Id", new { Id = DataIndex }).First();
        }

        [Benchmark(Description = "Update")]
        public long Update()
        {
            Step();

            return _connection.Execute($@"UPDATE TestDatas SET Counter1 = {DataIndex} WHERE Id =@Id", new { Id = DataIndex });
        }

        [Benchmark(Description = "Delete")]
        public long Delete()
        {
            Step();

            return _connection.Execute($@"DELETE FROM TestDatas WHERE Id =@Id", new { Id = DataIndex });
        }
    }
}
