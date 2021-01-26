using System;

namespace AlgorithmDemo.Sort {
    /// <summary>
    /// 直接插入法排序
    /// </summary>
    public static class StraightInsertSort {
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">排序数据类型</typeparam>
        /// <param name="arr">排序数组</param>
        /// <param name="comparison">比较委托</param>
        public static void Sort<T>(T[] arr, Comparison<T> comparison) {
            for (int i = 1; i < arr.Length; i++) {
                var curr = arr[i];

                var j = i - 1;
                while (j > -1 && comparison(curr, arr[j]) < 0) {
                    arr[j + 1] = arr[j];
                    j--;
                }

                arr[j + 1] = curr;
            }
        }
    }
}
