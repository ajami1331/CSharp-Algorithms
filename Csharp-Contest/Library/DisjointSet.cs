namespace Csharp_Contest
{
    public class DisjointSet
    {
        private int size;
        private int[] parent;

        public int NumberOfComponent { get; private set; }

        public int[] Count { get; }

        public int[] RealParent { get; }

        public int this[int u] => this.FindParent(u);

        public DisjointSet(int size)
        {
            this.size = size;
            this.NumberOfComponent = size;
            this.parent = new int[size];
            this.RealParent = new int[size];
            this.Count = new int[size];
            this.Reset();
        }

        public void Reset()
        {
            for (int i = 0; i < this.size; i++)
            {
                this.Count[i] = 1;
                this.parent[i] = i;
                this.RealParent[i] = i;
            }

            this.NumberOfComponent = this.size;
        }

        public int FindParent(int u)
        {
            if (this.parent[u] == u)
            {
                return u;
            }

            return this.parent[u] = this.FindParent(this.parent[u]);
        }

        public bool IsSameSet(int u, int v)
        {
            return this.FindParent(u) == this.FindParent(v);
        }

        public void MergeSet(int u, int v)
        {
            if (this.IsSameSet(u, v))
            {
                return;
            }

            u = this.FindParent(u);
            v = this.FindParent(v);
            if (this.Count[u] < this.Count[v])
            {
                (u, v) = (v, u);
            }

            this.parent[u] = this.parent[v];
            this.RealParent[u] = this.RealParent[v];
            this.Count[v] += this.Count[u];
            this.NumberOfComponent--;
        }
    }
}
