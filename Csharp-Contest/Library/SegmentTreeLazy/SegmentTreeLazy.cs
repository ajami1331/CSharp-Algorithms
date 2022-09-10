// SegmentTreeLazy.cs
// Author: Araf Al Jami
// Last Updated: 10-09-2565 20:57

namespace CLown1331.Library.SegmentTreeLazy
{
    using System;
    using System.Linq;

    public class Node<T>
    {
        public Node()
        {
        }

        public T Value { get; set; } = default(T);

        public T Prop { get; set; } = default(T);
    }

    public class SegmentTreeLazy<T>
    {
        private Node<T>[] tree;
        private int length;
        private Func<T, T, T> merge;
        private T outOfBound;
        private Func<Node<T>, T, bool> setValue;
        private Func<int, int, int, SegmentTreeLazy<T>, bool> propagate;

        public Node<T> this[int node]
        {
            get { return this.tree[node]; }
            set { this.tree[node] = value; }
        }

        public SegmentTreeLazy(int length, Func<T, T, T> merge, Func<int, int, int, SegmentTreeLazy<T>, bool> propagate, Func<Node<T>, T, bool> setValue, T outOfBound)
        {
            this.length = length;
            this.tree = Enumerable.Repeat(0, length << 2).Select(_ => new Node<T>()).ToArray();
            this.merge = merge;
            this.propagate = propagate;
            this.setValue = setValue;
            this.outOfBound = outOfBound;
            this.Build(1, 0, length - 1);
        }

        public void Build(int node, int b, int e)
        {
            if (b == e)
            {
                return;
            }

            int mid = b + e >> 1;
            int left = node << 1;
            int right = left | 1;
            this.Build(left, b, mid);
            this.Build(right, mid + 1, e);
            this[node].Value = this.merge(this[left].Value, this[right].Value);
        }

        public T Query(int node, int b, int e, int l, int r)
        {
            this.propagate(node, b, e, this);
            if (r < b || e < l)
            {
                return this.outOfBound;
            }

            if (b >= l && e <= r)
            {
                return this[node].Value;
            }

            int left = node << 1;
            int right = left | 1;
            int mid = b + e >> 1;
            T x = this.Query(left, b, mid, l, r);
            T y = this.Query(right, mid + 1, e, l, r);
            return this.merge(x, y);
        }

        public void Update(int node, int b, int e, int l, int r, T value)
        {
            this.propagate(node, b, e, this);
            if (r < b || e < l)
            {
                return;
            }

            if (b >= l && e <= r)
            {
                this.setValue(this[node], value);
                this.propagate(node, b, e, this);
                return;
            }

            int left = node << 1;
            int right = left | 1;
            int mid = b + e >> 1;

            this.Update(left, b, mid, l, r, value);
            this.Update(right, mid + 1, e, l, r, value);

            this[node].Value = this.merge(this[left].Value, this[right].Value);
        }
    }
}
