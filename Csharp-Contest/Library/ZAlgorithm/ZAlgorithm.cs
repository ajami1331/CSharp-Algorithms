// ZAlgorithm.cs
// Author: Araf Al Jami
// Last Updated: 11-09-2565 20:09

namespace CLown1331.Library.ZAlgorithm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ZAlgorithm<T>
    {
        private readonly T[] s;
        private readonly int n;

        public int[] Z { get; }

        public int[] Occurance { get; }

        public int MaxZ { get; private set; }

        public ZAlgorithm(IEnumerable<T> a, IEnumerable<T> b, T outOf)
        {
            this.s = a.Concat(new[] {outOf}).Concat(b).ToArray();
            this.n = this.s.Length;
            this.Z = new int[this.n];
            this.Occurance = new int[this.n];
            this.Compute(outOf);
        }

        private void Compute(T outOf)
        {
            var l = 0;
            var r = 0;
            for (var i = 1; i < this.n; i++)
            {
                if (i > r)
                {
                    l = r = i;
                    while (r < this.n && this.s[r - l].Equals(this.s[r]))
                    {
                        r++;
                    }

                    this.Z[i] = r - l;
                    r--;
                }
                else
                {
                    int k = i - l;
                    if (this.Z[k] < r - i + 1)
                    {
                        this.Z[i] = this.Z[k];
                    }
                    else
                    {
                        l = i;
                        while (r < this.n && this.s[r - l].Equals(this.s[r]))
                        {
                            r++;
                        }

                        this.Z[i] = r - l;
                        r--;
                    }
                }
            }

            int iter = 0;
            while (iter < this.n && !this.s[iter].Equals(outOf))
            {
                iter++;
            }
            
            for (int i = iter + 1; i < this.n; i++)
            {
                this.MaxZ = Math.Max(this.MaxZ, this.Z[i]);
                this.Occurance[this.Z[i]]++;
            }
        }
    }
}
