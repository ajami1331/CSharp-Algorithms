// SegmentTree.cs
// Author: Araf Al Jami
// Last Updated: 21-08-2565 01:43

namespace Library.SegmentTree
{
    using System;

    public class SegmentTree<T>
    {
        private T[] tree;
        private int length;
        private Func<T, T, T> merge;
        private T outOfBound;

        public T this[int node]
        {
            get => this.tree[node];
            set => this.tree[node] = value;
        }

        public SegmentTree(int length, Func<T, T, T> merge, T outOfBound)
        {
            this.length = length;
            this.tree = new T[length << 2];
            this.merge = merge;
            this.outOfBound = outOfBound;
        }

        public SegmentTree(T[] arr, Func<T, T, T> merge, T outOfBound)
            : this(arr.Length, merge, outOfBound)
        {
            this.Build(1, 0, arr.Length - 1, arr);
        }

        public void Build(int node, int b, int e, T[] arr)
        {
            if (b == e)
            {
                this[node] = arr[b];
                return;
            }

            int mid = b + e >> 1;
            int left = node << 1;
            int right = left | 1;
            this.Build(left, b, mid, arr);
            this.Build(right, mid + 1, e, arr);
            this[node] = this.merge(this[left], this[right]);
        }

        public T Query(int node, int b, int e, int l, int r)
        {
            if (r < b || e < l)
            {
                return this.outOfBound;
            }

            if (b >= l && e <= r)
            {
                return this[node];
            }

            int left = node << 1;
            int right = left | 1;
            int mid = b + e >> 1;
            T x = this.Query(left, b, mid, l, r);
            T y = this.Query(right, mid + 1, e, l, r);
            return this.merge(x, y);
        }

        public void Update(int node, int b, int e, int index, T value)
        {
            if (b == e)
            {
                this[node] = value;
                return;
            }

            int left = node << 1;
            int right = left | 1;
            int mid = b + e >> 1;
            if (index <= mid)
            {
                this.Update(left, b, mid, index, value);
            }
            else
            {
                this.Update(right, mid + 1, e, index, value);
            }

            this[node] = this.merge(this[left], this[right]);
        }
    }
}
