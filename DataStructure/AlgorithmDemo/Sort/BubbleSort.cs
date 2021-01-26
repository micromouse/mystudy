using System;

namespace AlgorithmDemo.Sort {
    /// <summary>
    /// 冒泡排序
    /// </summary>
    public static class BubbleSort {
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">排序数据类型</typeparam>
        /// <param name="arr">排序数组</param>
        /// <param name="comparison">比较委托</param>
        public static void Sort<T>(T[] arr, Comparison<T> comparison) {
            for (int i = 1; i < arr.Length; i++) {
                var exchanged = false;

                for (int j = 0; j < arr.Length - i; j++) {
                    if (comparison(arr[j], arr[j + 1]) > 0) {
                        var temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        exchanged = true;
                    }
                }

                if (!exchanged) {
                    return;
                }
            }
        }
    }
}
