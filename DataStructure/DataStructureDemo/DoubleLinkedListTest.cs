using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace DataStructureDemo {
    /// <summary>
    /// 双链表测试
    /// </summary>
    public class DoubleLinkedListTest {
        private ITestOutputHelper _output;

        /// <summary>
        /// 初始化双链表测试
        /// </summary>
        /// <param name="output">测试输出帮助器</param>
        public DoubleLinkedListTest(ITestOutputHelper output) {
            _output = output;
        }

        /// <summary>
        /// 末尾添加节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Addafternode_and_sequareoutput_correct() {
            var list = this.GetDoubleLinkedList();
            Assert.Equal("1234", this.GetSequareOutput(list));
        }

        /// <summary>
        /// 尾节点前添加新节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Addbefore_and_sequareoutput_correct() {
            var list = this.GetDoubleLinkedList(2);

            list.AddBefore(9);
            list.AddBefore(8);
            Assert.Equal("1982", this.GetSequareOutput(list));
        }

        /// <summary>
        /// 在首节点后插入新节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Insertafter_0_and_sequareoutput_correct() {
            var list = this.GetDoubleLinkedList();

            list.InsertAfter(0, 0);
            Assert.Equal("10234", this.GetSequareOutput(list));
        }

        /// <summary>
        /// 在尾节点后插入新节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Insertafter_3_and_sequareoutput_correct() {
            var list = this.GetDoubleLinkedList();

            list.InsertAfter(3, 5);
            Assert.Equal("12345", this.GetSequareOutput(list));
        }

        /// <summary>
        /// 在第3个节点后插入新节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Insertafter_2_and_sequareoutput_correct() {
            var list = this.GetDoubleLinkedList();

            list.InsertAfter(2, 5);
            Assert.Equal("12354", this.GetSequareOutput(list));
        }

        /// <summary>
        /// 在首节点前插入新节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Insertbefore_0_sequareoutput_correct() {
            var list = this.GetDoubleLinkedList();

            list.InsertBefore(0, 0);
            Assert.Equal("01234", this.GetSequareOutput(list));
        }

        /// <summary>
        /// 在第3个节点前插入新节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Insertbefore_2_sequareoutput_correct() {
            var list = this.GetDoubleLinkedList();

            list.InsertBefore(2, 5);
            Assert.Equal("12534", this.GetSequareOutput(list));
        }

        /// <summary>
        /// 在尾节点前插入新节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Insertbefore_3_sequareoutput_correct() {
            var list = this.GetDoubleLinkedList();

            list.InsertBefore(3, 5);
            Assert.Equal("12354", this.GetSequareOutput(list));
        }

        /// <summary>
        /// 删除首节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Removenode_0_sequareoutput_correct() {
            var list = this.GetDoubleLinkedList();

            list.RemoveAt(0);
            Assert.Equal("234", this.GetSequareOutput(list));
        }

        /// <summary>
        /// 删除第3个节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Removenode_2_sequareoutput_correct() {
            var list = this.GetDoubleLinkedList();

            list.RemoveAt(2);
            Assert.Equal("124", this.GetSequareOutput(list));
        }

        /// <summary>
        /// 删除尾节点,顺序输出正确
        /// </summary>
        [Fact]
        public void Removenode_3_sequareoutput_correct() {
            var list = this.GetDoubleLinkedList();

            list.RemoveAt(3);
            Assert.Equal("123", this.GetSequareOutput(list));
        }

        /// <summary>
        /// 获得双链表顺序输出字符串
        /// </summary>
        /// <param name="list">双链表</param>
        /// <returns>顺序输出字符串</returns>
        private string GetSequareOutput(DoubleLinkedList<int> list) {
            var output = "";

            for (int i = 0; i < list.Count; i++) {
                output += list[i];
            }

            _output.WriteLine($"this sequare output:{output}");
            return output;
        }

        /// <summary>
        /// 获得双链表集合
        /// </summary>
        /// <param name="count">链表数</param>
        /// <returns>双链表集合</returns>
        private DoubleLinkedList<int> GetDoubleLinkedList(int count = 4) {
            var list = new DoubleLinkedList<int>();

            for (int i = 0; i < count; i++) {
                list.AddAfter(i+1);
            }

            return list;
        }
    }
}
