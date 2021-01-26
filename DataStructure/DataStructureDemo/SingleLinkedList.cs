using System;

namespace DataStructureDemo {
    /// <summary>
    /// 单链表集合
    /// </summary>
    /// <typeparam name="T">链表数据类型</typeparam>
    public class SingleLinkedList<T> {
        private SingleLinkedNode<T> head;       //字段，当前链表的头节点

        /// <summary>
        /// 当前链表节点个数
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>节点</returns>
        public T this[int index] {
            get => GetNodeByIndex(index).Item;
            set => GetNodeByIndex(index).Item = value;
        }

        /// <summary>
        /// 初始化单链表集合
        /// </summary>
        public SingleLinkedList() {
            Count = 0;
            head = null;
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="item">数据域</param>
        public void Add(T item) {
            var newNode = new SingleLinkedNode<T>(item);

            if (head == null) {
                head = newNode;
            } else {
                var lastNode = GetNodeByIndex(Count - 1);
                lastNode.Next = newNode;
            }

            Count++;
        }

        /// <summary>
        /// 在指定位置插入节点
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="item">数据域</param>
        public void Insert(int index, T item) {
            var newNode = new SingleLinkedNode<T>(item);

            if (index < 0 || index >= Count) {
                throw new ArgumentOutOfRangeException(nameof(index), "索引超出范围");
            } else if (index == 0) {
                //在头节点插入新节点
                if (head == null) {
                    head = newNode;
                } else {
                    newNode.Next = head;
                    head = newNode;
                }
            } else {
                var prevNode = GetNodeByIndex(index - 1);
                newNode.Next = prevNode.Next;
                prevNode.Next = newNode;
            }

            Count++;
        }

        /// <summary>
        /// 删除指定位置节点
        /// </summary>
        /// <param name="index">索引</param>
        public void RemoveAt(int index) {
            if (index < 0 || index > Count) {
                throw new ArgumentOutOfRangeException(nameof(index), "索引超出范围");
            } else if (index == 0) {
                var deleteNode = head;
                head = head.Next;
                deleteNode = null;
            } else {
                var prevNode = GetNodeByIndex(index - 1);
                if (prevNode.Next == null) {
                    throw new ArgumentOutOfRangeException(nameof(index), "索引超出范围");
                }

                var deleteNode = prevNode.Next;
                prevNode.Next = deleteNode.Next;
                deleteNode = null;
            }

            Count--;
        }

        /// <summary>
        /// 获得索引位置节点
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>节点</returns>
        private SingleLinkedNode<T> GetNodeByIndex(int index) {
            if (index < 0 || index >= Count) {
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
    /// 单链表节点
    /// </summary>
    /// <typeparam name="T">节点数据类型</typeparam>
    public class SingleLinkedNode<T> {
        /// <summary>
        /// 数据域
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// 指针域
        /// </summary>
        public SingleLinkedNode<T> Next { get; set; }

        /// <summary>
        /// 初始化单链表节点
        /// </summary>
        /// <param name="item">数据域</param>
        public SingleLinkedNode(T item) {
            this.Item = item;
        }
    }
}
