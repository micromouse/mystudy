using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace DataStructureDemo {
    /// <summary>
    /// 双链表集合
    /// </summary>
    /// <typeparam name="T">双链表节点数据类型</typeparam>
    /// <remarks>
    /// 在.NET中，已经为我们提供了单链表和双链表的实现，它们分别是ListDictionary与LinkedList<T>。从名称可以看出，单链表的实现ListDictionary不是泛型实现
    /// ListDictionary位于System.Collection.Specialized下，它是基于键值对（Key/Value）的集合，微软给出的建议是：通常用于包含10个或10个以下项的集合。
    /// </remarks>
    public class DoubleLinkedList<T> {
        private int count;
        private DoubleLinkedNode<T> head;

        /// <summary>
        /// 总节点数
        /// </summary>
        public int Count => count;

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>索引位置节点值</returns>
        public T this[int index] {
            get => this.GetNodeByIndex(index).Item;
            set => this.GetNodeByIndex(index).Item = value;
        }

        /// <summary>
        /// 初始化双链表集合
        /// </summary>
        public DoubleLinkedList() {
            count = 0;
            head = null;
        }

        /// <summary>
        /// 在尾节点后插入新节点
        /// </summary>
        /// <param name="item">数据域</param>
        public void AddAfter(T item) {
            var newNode = new DoubleLinkedNode<T>(item);

            if (head == null) {
                head = newNode;
            } else {
                var lastNode = this.GetNodeByIndex(count - 1);
                lastNode.Next = newNode;
                newNode.Prev = lastNode;
            }

            count++;
        }

        /// <summary>
        /// 在尾节点前插入新节点
        /// </summary>
        /// <param name="item">数据域</param>
        public void AddBefore(T item) {
            var newNode = new DoubleLinkedNode<T>(item);

            if (head == null) {
                head = newNode;
            } else {
                var lastNode = this.GetNodeByIndex(count - 1);
                var prevNode = lastNode.Prev;

                //调整倒数第二个阶段与新节点的关系
                if (prevNode != null) {
                    prevNode.Next = newNode;
                    newNode.Prev = prevNode;
                }

                //调整尾节点与新节点的关系
                lastNode.Prev = newNode;
                newNode.Next = lastNode;

                //新节点是头节点
                if (newNode.Prev == null) head = newNode;
            }

            count++;
        }

        /// <summary>
        /// 在指定位置后插入节点
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="item">数据域</param>
        public void InsertAfter(int index, T item) {
            var newNode = new DoubleLinkedNode<T>(item);
            var prevNode = this.GetNodeByIndex(index);

            if (prevNode == null) {
                //是空节点
                head = newNode;
            } else {
                var nextNode = prevNode.Next;

                //调整新节点与前驱节点的关系
                prevNode.Next = newNode;
                newNode.Prev = prevNode;

                //调整新节点与后继节点的关系
                if (nextNode != null) {
                    newNode.Next = nextNode;
                    nextNode.Prev = newNode;
                }
            }

            count++;
        }

        /// <summary>
        /// 在指定位置前插入新节点
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="item">数据域</param>
        public void InsertBefore(int index,T item) {
            var newNode = new DoubleLinkedNode<T>(item);
            var nextNode = this.GetNodeByIndex(index);

            if (nextNode == null) {
                head = newNode;
            } else {
                var prevNode = nextNode.Prev;

                //调整新节点与后继节点的关系
                newNode.Next = nextNode;
                nextNode.Prev = newNode;

                //调整新节点与前驱节点的关系
                if (prevNode != null) {
                    prevNode.Next = newNode;
                    newNode.Prev = prevNode;
                }

                //如果新节点是头节点
                if (newNode.Prev == null) head = newNode;
            }

            count++;
        }

        /// <summary>
        /// 获得索引位置节点
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>索引位置节点</returns>
        private DoubleLinkedNode<T> GetNodeByIndex(int index) {
            if (index < 0 || index >= count) {
                throw new ArgumentOutOfRangeException(nameof(index), "索引超出范围");
            }

            var node = head;
            for (int i = 0; i < index; i++) {
                node = node.Next;
            }

            return node;
        }

    }

    /// <summary>
    /// 双链表节点
    /// </summary>
    /// <typeparam name="T">双链表节点数据类型</typeparam>
    public class DoubleLinkedNode<T> {
        /// <summary>
        /// 数据域
        /// </summary>
        public T Item { get; set; }
        /// <summary>
        /// 前一节点
        /// </summary>
        public DoubleLinkedNode<T> Prev { get; set; }
        /// <summary>
        /// 下一节点
        /// </summary>
        public DoubleLinkedNode<T> Next { get; set; }

        /// <summary>
        /// 初始化双链表节点
        /// </summary>
        /// <param name="item">数据域</param>
        public DoubleLinkedNode(T item) {
            this.Item = item;
        }
    }
}
