﻿using Xunit;
using Xunit.Abstractions;

namespace AlgorithmDemo.Sort {
    /// <summary>
    /// 直接插入法排序测试
    /// </summary>
    public class StraightInsertSortTest : SortTestBase {
        /// <summary>
        /// 初始化直接插入法排序测试
        /// </summary>
        /// <param name="helper"><see cref="ITestOutputHelper"/></param>
        public StraightInsertSortTest(ITestOutputHelper helper) : base(helper) {
        }

        /// <summary>
        /// 使用直接插入法按升序排列正确
        /// </summary>
        [Fact]
        public void Use_StraightInsertSort_ByAsc_Correct() {
            var arr = this.GetSortDatas();

            StraightInsertSort.Sort(arr, (t1, t2) => t1 - t2);
            _helper.WriteLine($"排序后序列:[{string.Join(',', arr)}]");
            Assert.Equal("1,2,3,4,5,6,7,8,9", string.Join(',', arr));
        }

        /// <summary>
        /// 使用直接插入法按降序排列正确
        /// </summary>
        [Fact]
        public void Use_StraightInsertSort_ByDesc_Correct() {
            var arr = this.GetSortDatas();

            StraightInsertSort.Sort(arr, (t1, t2) => t2 - t1);
            _helper.WriteLine($"排序后序列:[{string.Join(',', arr)}]");
            Assert.Equal("9,8,7,6,5,4,3,2,1", string.Join(',', arr));
        }
    }
}
