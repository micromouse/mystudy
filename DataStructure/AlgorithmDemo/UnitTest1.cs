using Xunit;

namespace AlgorithmDemo {
    public class UnitTest1 {
        [Fact]
        public void Test1() {
            var arr = new int[] { 6, 7, 5, 8, 4, 9, 3, 1, 2 };

            for (int i = 1; i < arr.Length; i++) {
                var curr = arr[i];

                var j = i - 1;
                while (j > -1 && curr < arr[j]) {
                    arr[j + 1] = arr[j];
                    j--;
                }

                arr[j + 1] = curr;
            }

            Assert.Equal("1,2,3,4,5,6,7,8,9", string.Join(',', arr));
        }
    }
}
