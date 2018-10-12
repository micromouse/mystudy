using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace DataStructureDemo {
    /// <summary>
    /// 与瑟夫问题测试
    /// </summary>
    public class JosephusProblemTest {
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// 初始化约瑟夫问题你测试
        /// </summary>
        /// <param name="output">测试输出帮助器</param>
        public JosephusProblemTest(ITestOutputHelper output) {
            _output = output;
        }

        /// <summary>
        /// 使用LinkedList测试,输出出对正确
        /// </summary>
        [Fact]
        public void UseLinkedList_and_outputoutqueue_correct() {
            var linkedList = new JosephusProblemLinkedList();

            var number = 4;
            var personList = linkedList.GetPersonList(10);
            var output = linkedList.Execute(personList, number);

            _output.WriteLine($"总人数={10}");
            _output.WriteLine($"出对号={number}");
            _output.WriteLine($"出对顺序={output}");
            Assert.Equal("4,8,2,7,3,10,9,1,6,5", output);
        }
    }
}
