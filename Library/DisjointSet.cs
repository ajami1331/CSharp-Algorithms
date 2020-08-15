namespace Csharp_Contest.Library
{
    class DisjointSet
    {
        private int size;
        private int[] parent;
        private int[] count;

        private int numberOfComponent;

        public int NumberOfComponent { get { return numberOfComponent; } }
        public DisjointSet(int size)
        {
            this.size = size;
            this.numberOfComponent = size;
            this.parent = new int[size];
            this.count = new int[size];
            Reset();
        }
        public void Reset()
        {
            for (int i = 0; i < size; i++)
            {
                count[i] = 1;
                parent[i] = i;
            }
            numberOfComponent = size;
        }
        public int FindParent(int u)
        {
            if (parent[u] == u)
            {
                return u;
            }
            return parent[u] = FindParent(parent[u]);
        }

        public bool IsSameSet(int u, int v)
        {
            return FindParent(u) == FindParent(v);
        }
        public void MergeSet(int u, int v)
        {
            if (IsSameSet(u, v))
            {
                return;
            }
            u = FindParent(u);
            v = FindParent(v);
            parent[u] = parent[v];
            count[v] += count[u];
            numberOfComponent--;
        }
    }
}
