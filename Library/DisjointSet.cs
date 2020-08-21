namespace Csharp_Contest
{
    public class DisjointSet
    {
        private int size;
        private int[] parent;
        private int[] count;

        private int numberOfComponent;

        public int NumberOfComponent { get { return this.numberOfComponent; } }

        public DisjointSet(int size)
        {
            this.size = size;
            this.numberOfComponent = size;
            this.parent = new int[size];
            this.count = new int[size];
            this.Reset();
        }

        public void Reset()
        {
            for (int i = 0; i < this.size; i++)
            {
                this.count[i] = 1;
                this.parent[i] = i;
            }

            this.numberOfComponent = this.size;
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
            this.parent[u] = this.parent[v];
            this.count[v] += this.count[u];
            this.numberOfComponent--;
        }
    }
}
