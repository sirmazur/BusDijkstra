using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    
        public class RedBlackTree<TKey, TValue> where TKey : IComparable<TKey>
        {
            private enum NodeColor
            {
                Red,
                Black
            }

            private class RedBlackTreeNode
            {
                public TKey Key { get; set; }
                public TValue Value { get; set; }
                public RedBlackTreeNode Left { get; set; }
                public RedBlackTreeNode Right { get; set; }
                public NodeColor Color { get; set; }
                public int Size { get; set; }

                public RedBlackTreeNode(TKey key, TValue value, NodeColor color, int size)
                {
                    Key = key;
                    Value = value;
                    Color = color;
                    Size = size;
                }
            }

            private RedBlackTreeNode root;

            public bool IsEmpty
            {
                get { return root == null; }
            }

            public TValue Get(TKey key)
            {
                RedBlackTreeNode node = Get(root, key);
                if (node == null)
                {
                    throw new KeyNotFoundException();
                }
                return node.Value;
            }

            private RedBlackTreeNode Get(RedBlackTreeNode node, TKey key)
            {
                if (node == null)
                {
                    return null;
                }

                int cmp = key.CompareTo(node.Key);
                if (cmp < 0)
                {
                    return Get(node.Left, key);
                }
                else if (cmp > 0)
                {
                    return Get(node.Right, key);
                }
                else
                {
                    return node;
                }
            }

            public bool Contains(TKey key)
            {
                RedBlackTreeNode node = Get(root, key);
                return node != null;
            }

            public void Put(TKey key, TValue value)
            {
                root = Put(root, key, value);
                root.Color = NodeColor.Black;
            }

            private RedBlackTreeNode Put(RedBlackTreeNode node, TKey key, TValue value)
            {
                if (node == null)
                {
                    return new RedBlackTreeNode(key, value, NodeColor.Red, 1);
                }

                int cmp = key.CompareTo(node.Key);
                if (cmp < 0)
                {
                    node.Left = Put(node.Left, key, value);
                }
                else if (cmp > 0)
                {
                    node.Right = Put(node.Right, key, value);
                }
                else
                {
                    node.Value = value;
                }

                if (IsRed(node.Right) && !IsRed(node.Left))
                {
                    node = RotateLeft(node);
                }
                if (IsRed(node.Left) && IsRed(node.Left.Left))
                {
                    node = RotateRight(node);
                }
                if (IsRed(node.Left) && IsRed(node.Right))
                {
                    FlipColors(node);
                }
                node.Size = 1 + Size(node.Left) + Size(node.Right);

                return node;
            }
            private RedBlackTreeNode DeleteMin(RedBlackTreeNode node)
            {
                if (node.Left == null)
                {
                    return null;
                }

                if (!IsRed(node.Left) && !IsRed(node.Left.Left))
                {
                    node = MoveRedLeft(node);
                }

                node.Left = DeleteMin(node.Left);
                return Balance(node);
            }
            private RedBlackTreeNode DeleteMax(RedBlackTreeNode node)
            {
                if (IsRed(node.Left))
                {
                    node = RotateRight(node);
                }

                if (node.Right == null)
                {
                    return null;
                }

                if (!IsRed(node.Right) && !IsRed(node.Right.Left))
                {
                    node = MoveRedRight(node);
                }

                node.Right = DeleteMax(node.Right);
                return Balance(node);
            }

            public void Delete(TKey key)
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException();
                }

                if (!Contains(key))
                {
                    return;
                }

                if (!IsRed(root.Left) && !IsRed(root.Right))
                {
                    root.Color = NodeColor.Red;
                }

                root = Delete(root, key);
                if (!IsEmpty)
                {
                    root.Color = NodeColor.Black;
                }
            }

            private RedBlackTreeNode Delete(RedBlackTreeNode node, TKey key)
            {
                if (key.CompareTo(node.Key) < 0)
                {
                    if (!IsRed(node.Left) && !IsRed(node.Left.Left))
                    {
                        node = MoveRedLeft(node);
                    }
                    node.Left = Delete(node.Left, key);
                }
                else
                {
                    if (IsRed(node.Left))
                    {
                        node = RotateRight(node);
                    }
                    if (key.CompareTo(node.Key) == 0 && (node.Right == null))
                    {
                        return null;
                    }
                    if (!IsRed(node.Right) && !IsRed(node.Right.Left))
                    {
                        node = MoveRedRight(node);
                    }
                    if (key.CompareTo(node.Key) == 0)
                    {
                        RedBlackTreeNode x = Min(node.Right);
                        node.Key = x.Key;
                        node.Value = x.Value;
                        node.Right = DeleteMin(node.Right);
                    }
                    else
                    {
                        node.Right = Delete(node.Right, key);
                    }
                }
                return Balance(node);
            }

            public TKey MinKey
            {
                get
                {
                    if (IsEmpty)
                    {
                        throw new InvalidOperationException();
                    }
                    return Min(root).Key;
                }
            }

            private RedBlackTreeNode Min(RedBlackTreeNode node)
            {
                if (node.Left == null)
                {
                    return node;
                }
                return Min(node.Left);
            }

            public TKey MaxKey
            {
                get
                {
                    if (IsEmpty)
                    {
                        throw new InvalidOperationException();
                    }
                    return Max(root).Key;
                }
            }

            private RedBlackTreeNode Max(RedBlackTreeNode node)
            {
                if (node.Right == null)
                {
                    return node;
                }
                return Max(node.Right);
            }

            public IEnumerable<TKey> Keys()
            {
                if (IsEmpty)
                {
                    yield break;
                }

                Stack<RedBlackTreeNode> stack = new Stack<RedBlackTreeNode>();
                RedBlackTreeNode node = root;
                while (stack.Count > 0 || node != null)
                {
                    if (node != null)
                    {
                        stack.Push(node);
                        node = node.Left;
                    }
                    else
                    {
                        node = stack.Pop();
                        yield return node.Key;
                        node = node.Right;
                    }
                }
            }

            private bool IsRed(RedBlackTreeNode node)
            {
                if (node == null)
                {
                    return false;
                }
                return node.Color == NodeColor.Red;
            }

            private RedBlackTreeNode RotateLeft(RedBlackTreeNode node)
            {
                RedBlackTreeNode x = node.Right;
                node.Right = x.Left;
                x.Left = node;
                x.Color = node.Color;
                node.Color = NodeColor.Red;
                x.Size = node.Size;
                node.Size = 1 + Size(node.Left) + Size(node.Right);
                return x;
            }

            private RedBlackTreeNode RotateRight(RedBlackTreeNode node)
            {
                RedBlackTreeNode x = node.Left;
                node.Left = x.Right;
                x.Right = node; x.Color = node.Color;
                node.Color = NodeColor.Red;
                x.Size = node.Size;
                node.Size = 1 + Size(node.Left) + Size(node.Right);
                return x;
            }

            private void FlipColors(RedBlackTreeNode node)
            {
                node.Color = NodeColor.Red;
                node.Left.Color = NodeColor.Black;
                node.Right.Color = NodeColor.Black;
            }

            private RedBlackTreeNode MoveRedLeft(RedBlackTreeNode node)
            {
                FlipColors(node);
                if (IsRed(node.Right.Left))
                {
                    node.Right = RotateRight(node.Right);
                    node = RotateLeft(node);
                    FlipColors(node);
                }
                return node;
            }

            private RedBlackTreeNode MoveRedRight(RedBlackTreeNode node)
            {
                FlipColors(node);
                if (IsRed(node.Left.Left))
                {
                    node = RotateRight(node);
                    FlipColors(node);
                }
                return node;
            }

            private RedBlackTreeNode Balance(RedBlackTreeNode node)
            {
                if (IsRed(node.Right))
                {
                    node = RotateLeft(node);
                }
                if (IsRed(node.Left) && IsRed(node.Left.Left))
                {
                    node = RotateRight(node);
                }
                if (IsRed(node.Left) && IsRed(node.Right))
                {
                    FlipColors(node);
                }

                node.Size = 1 + Size(node.Left) + Size(node.Right);
                return node;
            }

            private int Size(RedBlackTreeNode node)
            {
                return node == null ? 0 : node.Size;
            }
        }
    
}

