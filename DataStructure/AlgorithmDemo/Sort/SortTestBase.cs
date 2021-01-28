using Xunit.Abstractions;

namespace AlgorithmDemo.Sort {
    /// <summary>
    /// 排序测试基类
    /// </summary>
    public abstract class SortTestBase {
        protected readonly ITestOutputHelper _helper;

        /// <summary>
        /// 初始化排序测试基类
        /// </summary>
        /// <param name="helper"><see cref="ITestOutputHelper"/></param>
        protected SortTestBase(ITestOutputHelper helper) {
            _helper = helper;
        }

        /// <summary>
        /// 获得排序数据
        /// </summary>
        /// <returns>排序数据</returns>
        protected int[] GetSortDatas() {
            var arr = new int[] { 5, 4, 6, 3, 2, 1, 9, 7, 8 };
            _helper.WriteLine($"排序前序列:[{string.Join(',', arr)}]");

            return arr;
        }
    }
}
