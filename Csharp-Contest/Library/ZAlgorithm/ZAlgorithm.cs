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
        private int[] z;

        public ZAlgorithm(IEnumerable<T> a, IEnumerable<T> b, T outOf)
        {
            this.s = a.Concat(new[] {outOf}).Concat(b).ToArray();
            this.n = this.s.Length;
            this.z = new int[this.n];
            this.Compute();
        }

        public int MaxZ(out int res)
        {
            var maxZ = 0;
            res = 0;
            for (var i = 1; i < this.n; i++)
            {
                if (this.z[i] == this.n - i && maxZ >= this.n - i)
                {
                    res = this.n - i;
                    break;
                }

                maxZ = Math.Max(maxZ, this.z[i]);
            }

            return maxZ;
        }

        private void Compute()
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

                    this.z[i] = r - l;
                    r--;
                }
                else
                {
                    int k = i - l;
                    if (this.z[k] < r - i + 1)
                    {
                        this.z[i] = this.z[k];
                    }
                    else
                    {
                        l = i;
                        while (r < this.n && this.s[r - l].Equals(this.s[r]))
                        {
                            r++;
                        }

                        this.z[i] = r - l;
                        r--;
                    }
                }
            }
        }
    }
}
