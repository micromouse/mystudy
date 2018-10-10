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
        /// 顺序添加节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Addnode_and_sequareoutput_correct() {
            var list = this.GetSingleLinkedList();

            var output = "";
            for (int i = 0; i < list.Count; i++) {
                output += list[i];
            }

            Assert.Equal("2345", output);
        }

        /// <summary>
        /// 在索引0位置插入节点,顺序出书正确
        /// </summary>
        [Fact]
        public void Insertnode_0_and_sequareoutput_correct() {
            var list = this.GetSingleLinkedList();

            list.Insert(0, 1);

            var output = "";
            for (int i = 0; i < list.Count; i++) {
                output += list[i];
            }
            Assert.Equal("12345", output);
        }

        /// <summary>
        /// 在索引位置2插入节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Insertnode_2_sequareoutput_correct() {
            var list = this.GetSingleLinkedList();

            list.Insert(2, 9);

            var output = "";
            for (int i = 0; i < list.Count; i++) {
                output += list[i];
            }
            Assert.Equal("23945", output);
        }

        /// <summary>
        /// 在末尾插入节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Insertnode_4_sequareoutput_correct() {
            var list = this.GetSingleLinkedList();

            list.Insert(4, 6);

            var output = "";
            for (int i = 0; i < list.Count; i++) {
                output += list[i];
            }
            Assert.Equal("23456", output);
        }

        /// <summary>
        /// 获得单链表集合
        /// </summary>
        /// <returns>单链表集合</retur`ns>
        private SingleLinkedList<int> GetSingleLinkedList() {
            var list = new SingleLinkedList<int>();

            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);

            return list;
        }
    }
}
