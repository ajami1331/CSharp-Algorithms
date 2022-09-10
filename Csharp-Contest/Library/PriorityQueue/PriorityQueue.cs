// PriorityQueue.cs
// Author: Araf Al Jami
// Last Updated: 10-09-2565 20:57

namespace CLown1331.Library.PriorityQueue
{
    using System;
    using System.Collections.Generic;

    public class PriorityQueue<T>
    {
        private readonly List<T> data;
        private readonly Comparison<T> compare;

        public PriorityQueue(Comparison<T> compare)
        {
            this.compare = compare;
            this.data = new List<T> {default(T)};
        }

        public int Count
        {
            get { return this.data.Count - 1; }
        }

        public T Peek() => this.data[1];

        public void Clear()
        {
            this.data.Clear();
            this.data.Add(default(T));
        }

        public void Enqueue(T item)
        {
            this.data.Add(item);
            int curPlace = this.Count;
            while (curPlace > 1 && this.compare(item, this.data[curPlace / 2]) < 0)
            {
                this.data[curPlace] = this.data[curPlace / 2];
                this.data[curPlace / 2] = item;
                curPlace /= 2;
            }
        }

        public T Dequeue()
        {
            T ret = this.data[1];
            this.data[1] = this.data[this.Count];
            this.data.RemoveAt(this.Count);
            var curPlace = 1;
            while (true)
            {
                int max = curPlace;
                if (this.Count >= curPlace * 2 && this.compare(this.data[max], this.data[2 * curPlace]) > 0)
                {
                    max = 2 * curPlace;
                }

                if (this.Count >= curPlace * 2 + 1 && this.compare(this.data[max], this.data[2 * curPlace + 1]) > 0)
                {
                    max = 2 * curPlace + 1;
                }

                if (max == curPlace)
                {
                    break;
                }

                T item = this.data[max];
                this.data[max] = this.data[curPlace];
                this.data[curPlace] = item;
                curPlace = max;
            }

            return ret;
        }

        public bool Empty() => this.Count == 0;
    }
}
