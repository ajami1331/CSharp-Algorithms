// DisjointSet.cs
// Author: Araf Al Jami
// Last Updated: 23-08-2565 21:39

namespace Library.DisjointSet
{
    public class DisjointSet
    {
        private int size;
        private int[] parent;
        private int[] count;

        public int NumberOfComponent { get; private set; }

        public int[] RealParent => this.parent;

        public int this[int u] => this.GetParent(u);

        public DisjointSet(int size)
        {
            this.size = size;
            this.NumberOfComponent = size;
            this.parent = new int[size];
            this.count = new int[size];
            this.Reset();
        }

        public void Reset()
        {
            for (var i = 0; i < this.size; i++)
            {
                this.count[i] = 1;
                this.parent[i] = i;
            }

            this.NumberOfComponent = this.size;
        }

        public int GetParent(int u)
        {
            if (this.parent[u] == u)
            {
                return u;
            }

            return this.parent[u] = this.GetParent(this.parent[u]);
        }

        public bool IsSameSet(int u, int v) => this.GetParent(u) == this.GetParent(v);

        public void MergeSet(int u, int v)
        {
            if (this.IsSameSet(u, v))
            {
                return;
            }

            u = this.GetParent(u);
            v = this.GetParent(v);
            if (this.count[u] < this.count[v])
            {
                (u, v) = (v, u);
            }

            this.parent[u] = this.parent[v];
            this.count[v] += this.count[u];
            this.count[u] = this.count[v];
            this.NumberOfComponent--;
        }

        public int GetComponentSize(int u) => this.count[this[u]];
    }
}
