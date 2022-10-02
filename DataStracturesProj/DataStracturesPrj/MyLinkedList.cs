using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStracturesPrj
{
    public class MyLinkedList<T> : IEnumerable<T>
    {
        private Node head;
        private Node tail;
        private int count = 0;
        public bool IsEmpty() => (count == 0);
        public Node ReturnLast() => tail;
        public Node ReturnFirst() => head;
        public void AddFirst(T value)
        {
            Node node = new Node(value);
            if (count == 0)
            {
                head = node;
                tail = node;
            }
            else
            {
                head.Prev = node;
                node.Next = head;
                head = node;
            }
            count++;
        }
        public void AddFirstNode(Node n)
        {
            if (count == 0)
            {
                head = n;
                tail = n;
                n.Next = null;
                n.Prev = null;
            }
            else
            {
                head.Prev = n;
                n.Next = head;
                head = n;
                n.Prev = null;
            }
            count++;
        }

        public void AddLast(T value)
        {
            Node node = new Node(value);
            if (count == 0)
            {
                tail = node;
                head = node;
            }
            else
            {
                tail.Next = node;
                node.Prev = node;
                tail = node;
            }
            count++;
        }

        public bool RemoveFirst(out T value)
        {
            if (count == 0)
            {
                value = default;
                return false;
            }
            if (count == 1)
            {
                tail = null;
            }

            value = head.Value;

            head = head.Next;
            head.Prev = null;

            count--;
            return true;
        }
        public void RemoveFirst()
        {
            if (head == null) return; //if the list is empty do nothing
            head = head.Next; //else -> advence list start to the next object in list
            if(head != null)head.Prev = null; //new first priviuos is looking at null
            count--;
            if (head == null) tail = null; //if after deleting: all the list is clean then last is also null
        }
        public bool RemoveLast(out T value)
        {
            if (count == 0)
            {
                value = default;
                return false;
            }
            if (count == 1)
            {
                head = null;
            }

            value = tail.Value;

            tail = tail.Prev;
            tail.Next = null;

            count--;
            return true;
        }
        public void RemoveLast()
        {
            if (tail == null) return; //if there is no last (meaning there is no first also) do nothing
            tail = tail.Prev; //else -> back list last to the previous object of current last
            count--;
            if (tail == null)
            {
                head = null; //if after deleting: all the list is clean then first is also null
                return;
            }
            tail.Next = null; //new last next is looking at null
        }
        public bool GetAt(int index, out T value)
        {
            if (index < 0 || index >= count)
            {
                value = default;
                return false;
            }

            Node node = head;
            for (int i = 0; i < index; i++)
            {
                node = node.Next;
            }

            value = node.Value;
            return true;
        }
        public bool AddAt(int index, T value)
        {
            if (index < 0 || index > count)
            {
                value = default;
                return false;
            }

            if (index == 0)//want to add at index 0 - the first index
            {
                AddFirst(value);
            }
            else if (index == count)//want to add at index count - one after the largest index
            {
                AddLast(value);
            }
            else
            {
                Node node = head;
                for (int i = 0; i < index - 1; i++)
                {
                    node = node.Next;
                }

                Node nodeToAdd = new Node(value);

                node.Next.Prev = nodeToAdd;
                nodeToAdd.Next = node.Next;

                node.Next = nodeToAdd;
                nodeToAdd.Prev = node;
                count++;
            }

            return true;
        }
        public void RellocateToStart(Node n)
        {
            bool result = RemoveNode(n);
            if (!result) return;
            AddFirstNode(n);
        }
        public bool RemoveNode(Node n)
        {
            if (count == 0) return false;
            else if (n == head)
            {
                RemoveFirst();
                return true;
            }
            else if (n == tail)
            {
                RemoveLast();
                return true;
            }
            count--;
            n.Prev.Next = n.Next;
            n.Next.Prev = n.Prev;
            return true;

        }

        public override string ToString()
        {
            string s = "";
            Node temp = head;
            for (int i = 0; i < count; i++)
            {
                s += temp.Value + " -> ";
                temp = temp.Next;
            }
            s += "null";

            return s;
        }

        public IEnumerator<T> GetEnumerator() // FOREACH BACKWARD
        {
            if (tail == null) yield break;
            var node = tail;
            while (node != null)
            {
                yield return node.Value;
                node = node.Prev;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Node
        {
            public Node Next { get; set; }
            public T Value { get; set; }
            public Node Prev { get; set; }

            public Node(T value)
            {
                Value = value;
            }
        }
    }
}
