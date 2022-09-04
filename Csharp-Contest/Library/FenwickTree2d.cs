// FenwickTree2d.cs
// Author: Araf Al Jami
// Last Updated: 05-09-2565 01:48

namespace Library.FenwickTree2d
{
    using System;

    public class FenwickTree2d<T>
    {
        private readonly T[,] tree;
        private readonly int size;
        private readonly Func<T, T, T> merge;
        private readonly int logn;
        private Func<T, T, int> comp;

        public FenwickTree2d(int size, Func<T, T, T> merge)
            : this(size, merge, (arg1, arg2) =>
            {
                throw new NotImplementedException();
            })
        {
        }

        public FenwickTree2d(int size, Func<T, T, T> merge, Func<T, T, int> comp)
        {
            this.size = size;
            this.tree = new T[size, size];
            this.merge = merge;
            this.logn = (int)Math.Floor(Math.Log(this.size, 2));
            this.comp = comp;
        }

        public void UpdateY(int xIndex, int yIndex, T value)
        {
            for (; yIndex < this.size; yIndex += yIndex & -yIndex)
            {
                this.tree[xIndex, yIndex] = this.merge(this.tree[xIndex, yIndex], value);
            }
        }

        public void Update(int xIndex, int yIndex, T value)
        {
            for (; xIndex < this.size; xIndex += xIndex & -xIndex)
            {
                this.UpdateY(xIndex, yIndex, value);
            }
        }

        public T QueryY(int xIndex, int yIndex)
        {
            var ret = default(T);
            for (; yIndex > 0; yIndex -= yIndex & -yIndex)
            {
                ret = this.merge(ret, this.tree[xIndex, yIndex]);
            }

            return ret;
        }

        public T Query(int xIndex, int yIndex)
        {
            var ret = default(T);
            for (; xIndex > 0; xIndex -= xIndex & -xIndex)
            {
                ret = this.merge(ret, this.QueryY(xIndex, yIndex));
            }

            return ret;
        }
    }
}
