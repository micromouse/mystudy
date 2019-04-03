using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructureDemo {
    /// <summary>
    /// 用LinkedList实现的约瑟夫问题
    /// </summary>
    public class JosephusProblemLinkedList {
        /// <summary>
        /// 获得双链表集合
        /// </summary>
        /// <param name="count">人数</param>
        /// <returns>双链表集合</returns>
        public LinkedList<Person> GetPersonList(int count) {
            var personList = new LinkedList<Person>();

            for (int i = 1; i <= count; i++) {
                personList.AddLast(new Person(i, $"Counter-{i}"));
            }

            return personList;
        }

        /// <summary>
        /// 执行约瑟夫问题计算出对人员
        /// </summary>
        /// <param name="personList">参与的人员集合</param>
        /// <param name="number">出对号</param>
        /// <returns>逗号分隔的出对人员集合</returns>
        public string Execute(LinkedList<Person> personList, int number) {
            var output = "";
            var startNode = personList.First;
            LinkedListNode<Person> removeNode;

            while (personList.Count >= 1) {
                for (int i = 1; i < number; i++) {
                    startNode = startNode == personList.Last ? personList.First : startNode.Next;
                }

                //记录删除节点,设置下一个开始节点
                removeNode = startNode;
                startNode = startNode == personList.Last ? personList.First : startNode.Next;

                //删除删除节点
                output += $",{removeNode.Value.Id}";
                personList.Remove(removeNode);
            }

            return output.Substring(1);
        }
    }

    /// <summary>
    /// 参与约瑟夫问题的人
    /// </summary>
    public class Person {
        /// <summary>
        /// 初始化参与约瑟夫问题的人
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="">名称</param>
        public Person(int id, string name) {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
