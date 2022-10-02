using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStracturesPrj
{
    public class BST<T> : IEnumerable<T> where T : IComparable<T>
    {
        private Node root;

        public bool IsEmpty()
        {
            return root == null;
        }

        public void Add(T item) // O(logN)
        {
            if (root == null)
            {
                root = new Node(item);
                return;
            }

            Node tmp = root;
            while (true)
            {
                if (item.CompareTo(tmp.Value) < 0) // item < tmp.value - go left
                {
                    if (tmp.Left == null)
                    {
                        tmp.Left = new Node(item);
                        break;
                    }
                    else tmp = tmp.Left;
                }
                else // go right
                {
                    if (tmp.Right == null)
                    {
                        tmp.Right = new Node(item);
                        break;
                    }
                    else tmp = tmp.Right;
                }
            }

            // count++
            //+ notification
        }

        public bool Search(T item, out T foundItem)
        {
            foundItem = default;
            if (item == null || root == null) return false;
            Node tmp = root;
            while (tmp != null)
            {
                if (item.CompareTo(tmp.Value) < 0) tmp = tmp.Left;
                else if (item.CompareTo(tmp.Value) > 0) tmp = tmp.Right;
                else
                {
                    foundItem = tmp.Value;
                    return true;
                }
            }
            return false;
        }
        public bool FindBestMatch(T item, out T foundItem)
        {
            Node tmp = root;
            Node tmpHigher = tmp;
            bool flag = false;
            while (tmp != null)
            {
                if (item.CompareTo(tmp.Value) == 0)
                {
                    foundItem = tmp.Value;
                    return true;
                }
                if (item.CompareTo(tmp.Value) > 0) tmp = tmp.Right;
                else
                {
                    tmpHigher = tmp;
                    tmp = tmp.Left;
                    flag = true;
                }
            }
            if (flag)
            {
                foundItem = tmpHigher.Value;
                return true;
            }
            foundItem = default;
            return false;

        }
        public bool RemoveSingleItem(T key)
        {
            Node parent = root;
            Node current = root;
            bool leftChildren = false;
            while (current != null && current.Value.CompareTo(key) != 0)
            {
                parent = current;
                if (key.CompareTo(current.Value) < 0)
                {
                    current = current.Left;
                    leftChildren = true;
                }
                else
                {
                    current = current.Right;
                    leftChildren = false;
                }
            }
            if (current == null) return false;
            if (current.Left == null && current.Right == null)
            {
                if (current == root)
                    root = null;
                else if (leftChildren)
                    parent.Left = null;
                else
                    parent.Right = null;
            }
            else if (current.Right == null)
            {
                if (current == root)
                    root = current.Left;
                else if (leftChildren)
                    parent.Left = current.Left;
                else
                    parent.Right = current.Right;

            }
            else if (current.Left == null)
            {
                if (current == root)
                    root = current.Right;
                else if (leftChildren)
                    parent.Left = parent.Right;
                else
                    parent.Right = current.Right;
            }
            else
            {
                Node replacement = GetReplacement(current);

                if (current == root)
                    root = replacement;
                else if (leftChildren)
                    parent.Left = replacement;

                else
                {
                    parent.Right = replacement;
                    replacement.Left = current.Left;
                }
            }
            return true;
        }
        private Node GetReplacement(Node tmpNode)
        {
            Node replacementParent = tmpNode;
            Node replacement = tmpNode;
            Node tmp = tmpNode.Right;
            while (!(tmp == null))
            {
                replacementParent = tmp;
                replacement = tmp;
                tmp = tmp.Left;
            }
            if (!(replacement == tmpNode.Right))
            {
                replacementParent.Left = replacement.Right;
                replacement.Right = tmpNode.Right;
            }
            return replacement;
        }
        

        public void RemoveAllByValue(T item)
        {
            while (Search(item, out item)) RemoveSingleItem(item);
        }

        public int GetLevelsCnt()
        {
            return GetLevelsCnt(root);
        }


        int GetLevelsCnt(Node subTreeRoot)
        {
            if (subTreeRoot == null) return 0;

            int leftTreeDepth = GetLevelsCnt(subTreeRoot.Left);
            int rightTreeDepth = GetLevelsCnt(subTreeRoot.Right);

            return Math.Max(leftTreeDepth, rightTreeDepth) + 1;
        }

        public void ScanInOrder(Action<T> singleItemAction)  // Action<T> => void Func(T item)
        {
            ScanInOrder(root, singleItemAction);
        }

        private void ScanInOrder(Node subTreeRoot, Action<T> singleItemAction)
        {
            if (subTreeRoot == null) return;

            ScanInOrder(subTreeRoot.Left, singleItemAction);
            singleItemAction(subTreeRoot.Value); //invoke
            ScanInOrder(subTreeRoot.Right, singleItemAction);
        }
        public void DoOnThat(T value, Action<T> func)//Do the fucntion on the items higher than value
        {
            if (root == null) return;
            Node tmp = root;
            while (tmp != null)
            {
                if (tmp.Value.CompareTo(value) > 0) tmp = tmp.Right;
            }
            if (tmp.Value.CompareTo(value) <= 0 && tmp != null) ScanInOrder(tmp, func);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return root.GetEnumerator();    
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class Node : IEnumerable<T>
        {
            public T Value;
            public Node Left;
            public Node Right;

            public Node(T value)
            {
                this.Value = value;
                Left = Right = null;
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (Left != null)
                {
                    foreach (var v in Left)
                    {
                        yield return v;
                    }
                }

                yield return Value;

                if (Right != null)
                {
                    foreach (var v in Right)
                    {
                        yield return v;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
