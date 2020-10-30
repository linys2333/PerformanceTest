namespace PerformanceTest
{
    public abstract class BenchmarkBase
    {
        /// <summary>
        /// 起始索引
        /// </summary>
        public abstract int StartIndex { get; set; }

        /// <summary>
        /// 数据索引
        /// </summary>
        public int DataIndex { get; set; }
        
        /// <summary>
        /// 全局执行
        /// </summary>
        protected void GlobalSetup()
        {
            // 由于每个测试用例运行环境隔离的，所以这里需要初始化
            TestConfig.Instance.Init();

            StartIndex *= TestConfig.Instance.RowCount;
            DataIndex = StartIndex;
        }

        /// <summary>
        /// 全局清理
        /// </summary>
        protected void GlobalCleanup()
        {
        }

        /// <summary>
        /// 单个方法执行
        /// </summary>
        protected void Step()
        {
            DataIndex++;
            if (DataIndex > StartIndex + TestConfig.Instance.RowCount)
            {
                DataIndex = StartIndex;
            }
        }
    }
}