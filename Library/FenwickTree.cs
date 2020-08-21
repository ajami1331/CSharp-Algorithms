namespace Csharp_Contest
{
    using System;
    public class FenwickTree<T>
    {
        private readonly T[] tree;
        private readonly int size;
        private readonly Func<T, T, T> func;
        private readonly int logn;
        private Func<T, T, int> comp;

        public FenwickTree(int size, Func<T, T, T> func, Func<T, T, int> comp)
        {
            this.size = size;
            this.tree = new T[size];
            this.func = func;
            this.logn = (int)Math.Floor(Math.Log(this.size, 2));
            this.comp = comp;
        }

        public void Update(int index, T value)
        {
            for (; index < size; index += index & -index)
            {
                this.tree[index] = this.func(this.tree[index], value);
            }
        }

        public T Query(int index)
        {
            T ret = default(T);
            for (; index > 0; index -= index & -index)
            {
                ret = this.func(ret, this.tree[index]);
            }

            return ret;
        }

        public int LowerBound(T value)
        {
            int l = 1;
            int r = this.size - 1;
            while (r - l > 4)
            {
                int mid = l + (r - l) / 2;
                T sum = this.Query(mid);
                if (this.comp(sum, value) < 0)
                {
                    l = mid;
                }
                else
                {
                    r = mid;
                }
            }

            for (; l <= r; l++)
            {
                T sum = this.Query(l);
                if (this.comp(sum, value) == 0)
                {
                    return l;
                }
            }

            return -1;
        }
    }
}
