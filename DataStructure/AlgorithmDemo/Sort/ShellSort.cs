using System;

namespace AlgorithmDemo.Sort {
    /// <summary>
    /// 希尔排序
    /// </summary>
    public static class ShellSort {
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">排序数据类型</typeparam>
        /// <param name="arr">排序数组</param>
        /// <param name="comparison">比较委托</param>
        public static void Sort<T>(T[] arr, Comparison<T> comparison) {
            for (int gap = arr.Length / 2; gap >= 1; gap /= 2) {
                for (int i = gap; i < arr.Length; i++) {
                    var j = i - gap;

                    var curr = arr[i];
                    while (j >= 0 && comparison(curr, arr[j]) < 0) {
                        Swap(arr, j, j + gap);
                        j -= gap;
                    }
                }
            }
        }

        /// <summary>
        /// 交换数组元素值
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="arr">要交换元素的数组</param>
        /// <param name="index1">索引1</param>
        /// <param name="index2">索引2</param>
        private static void Swap<T>(T[] arr, int index1, int index2) {
            var temp = arr[index2];
            arr[index2] = arr[index1];
            arr[index1] = temp;
        }
    }
}
