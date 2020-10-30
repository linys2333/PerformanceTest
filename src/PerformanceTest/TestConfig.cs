using Microsoft.Extensions.Configuration;

namespace PerformanceTest
{
    /// <summary>
    /// 程序测试配置
    /// </summary>
    public class TestConfig
    {
        public static readonly TestConfig Instance = new TestConfig();

        /// <summary>
        /// 数据库连接字符串，使用的mysql
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 测试数据行数
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Benchmark迭代数
        /// </summary>
        public int IterationCount { get; set; }

        /// <summary>
        /// 1个迭代内基准方法总执行次数
        /// </summary>
        public int InvokeCount { get; set; }

        /// <summary>
        /// 初始化配置
        /// </summary>
        public void Init()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            ConnectionString = config.GetConnectionString("Default");
            RowCount = config.GetValue<int>(nameof(RowCount));
            IterationCount = config.GetValue<int>(nameof(IterationCount));
            InvokeCount = config.GetValue<int>(nameof(InvokeCount));
        }
    }
}