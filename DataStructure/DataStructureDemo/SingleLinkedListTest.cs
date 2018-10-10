using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DataStructureDemo {
    /// <summary>
    /// 单链表测试
    /// </summary>
    public class SingleLinkedListTest {
        /// <summary>
        /// 顺序添加节点,顺序输出相同
        /// </summary>
        [Fact]
        public void Addnode_and_sequareoutput_same() {
            var list = new SingleLinkedList<int>();

            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);

            var output = "";
            for (int i = 0; i < list.Count; i++) {
                output += list[i];
            }

            Assert.Equal("2345", output);
        }
    }
}
