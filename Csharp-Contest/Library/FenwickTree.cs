// FenwickTree.cs
// Authors: Araf Al-Jami
// Created: 21-08-2020 2:52 PM
// Updated: 08-07-2021 3:44 PM

namespace Library.FenwickTree
{
    using System;

    public class FenwickTree<T>
    {
        private readonly T[] tree;
        private readonly int size;
        private readonly Func<T, T, T> merge;
        private readonly int logn;
        private Func<T, T, int> comp;

        public FenwickTree(int size, Func<T, T, T> merge)
            : this(size, merge, (arg1, arg2) => throw new NotImplementedException()) { }

        public FenwickTree(int size, Func<T, T, T> merge, Func<T, T, int> comp)
        {
            this.size = size;
            this.tree = new T[size];
            this.merge = merge;
            this.logn = (int)Math.Floor(Math.Log(this.size, 2));
            this.comp = comp;
        }

        public T this[int node]
        {
            get => this.Query(node);
            set => this.Update(node, value);
        }

        public void Update(int index, T value)
        {
            for (; index < this.size; index += index & -index)
            {
                this.tree[index] = this.merge(this.tree[index], value);
            }
        }

        public T Query(int index)
        {
            T ret = default(T);
            for (; index > 0; index -= index & -index)
            {
                ret = this.merge(ret, this.tree[index]);
            }

            return ret;
        }

        public int LowerBound(T value)
        {
            T sum = default(T);
            T newSum = default(T);
            int pos = 0;
            int newPos = 0;
            for (int i = logn; i >= 0; i--)
            {
                newPos = pos + (1 << i);
                if (newPos >= this.size)
                {
                    continue;
                }

                newSum = this.merge(sum, this.tree[pos + (1 << i)]);

                if (this.comp(newSum, value) >= 0)
                {
                    continue;
                }

                sum = newSum;
                pos = newPos;
            }

            return pos + 1;
        }
    }
}
