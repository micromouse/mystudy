﻿using Xunit;
using Xunit.Abstractions;

namespace AlgorithmDemo.Sort {
    /// <summary>
    /// 冒泡排序测试
    /// </summary>
    public class BubbleSortTest : SortTestBase {
        /// <summary>
        /// 初始化冒泡排序测试
        /// </summary>
        /// <param name="helper"><see cref="ITestOutputHelper"/></param>
        public BubbleSortTest(ITestOutputHelper helper) : base(helper) {
        }

        /// <summary>
        /// 使用冒泡排序按升序排列成功
        /// </summary>
        [Fact]
        public void UseBubbleSort_ByAsc_Correct() {
            var arr = this.GetSortDatas();
            BubbleSort.Sort(arr, (t1, t2) => t1 - t2);

            _helper.WriteLine($"排序后序列:[{string.Join(",", arr)}]");
            Assert.Equal("1,2,3,4,5,6,7,8,9", string.Join(',', arr));
        }

        /// <summary>
        /// 使用冒泡排序按降序排列成功
        /// </summary>
        [Fact]
        public void UseBubbleSort_ByDesc_Correct() {
            var arr = this.GetSortDatas();
            BubbleSort.Sort(arr, (t1, t2) => t2 - t1);

            _helper.WriteLine($"排序后序列:[{string.Join(",", arr)}]");
            Assert.Equal("9,8,7,6,5,4,3,2,1", string.Join(',', arr));
        }

    }
}
