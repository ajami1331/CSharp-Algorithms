// EdmondsKarp.cs
// Author: Araf Al Jami
// Last Updated: 10-09-2565 20:57

namespace CLown1331.Library.EdmondsKarp
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   <para>Implementation of Edmonds-Karp max flow algorithm. </para>
    ///   <para>Running time:<br />        O(|V|*|E|^2).</para>
    /// </summary>
    public class EdmondsKarp
    {
        private readonly int[] parents;
        private readonly int[,] graph;
        private readonly bool[] visited;

        /// <summary>Initializes a new instance of the <see cref="EdmondsKarp" /> class.</summary>
        /// <param name="nodes">The nodes.</param>
        public EdmondsKarp(int nodes)
        {
            this.Nodes = nodes;
            this.parents = new int[nodes];
            this.visited = new bool[nodes];
            this.graph = new int[nodes, nodes];
        }

        /// <summary>Gets the number of nodes.</summary>
        /// <value>The nodes.</value>
        public int Nodes { get; }

        /// <summary>Adds the edge.</summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bidirectional">if set to <c>true</c> [bidirectional].</param>
        public void AddEdge(int from, int to, int capacity, bool bidirectional)
        {
            this.graph[from, to] += capacity;
            if (bidirectional)
            {
                this.graph[to, from] += capacity;
            }
        }

        /// <summary>Sets the edge capacity.</summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="bidirectional">if set to <c>true</c> [bidirectional].</param>
        public void SetEdgeCap(int from, int to, int capacity, bool bidirectional)
        {
            this.graph[from, to] = capacity;
            if (bidirectional)
            {
                this.graph[to, from] = capacity;
            }
        }

        /// <summary>  Computes the maximum flow.</summary>
        /// <param name="src">The source.</param>
        /// <param name="sink">The sink.</param>
        /// <returns>The maximum flow.</returns>
        public int MaxFlow(int src, int sink)
        {
            var maxFlow = 0;
            while (this.Bfs(src, sink))
            {
                int minCap;
                this.AugmentPath(minCap = this.MinValue(sink), sink);
                maxFlow += minCap;
            }

            return maxFlow;
        }

        private bool Bfs(int src, int sink)
        {
            for (var i = 0; i < this.Nodes; i++)
            {
                this.visited[i] = false;
                this.parents[i] = -1;
            }

            this.visited[src] = true;
            var q = new Queue<int>();
            q.Enqueue(src);
            while (q.Count > 0)
            {
                int u = q.Dequeue();
                if (u == sink)
                {
                    return true;
                }

                for (var i = 0; i < this.Nodes; i++)
                {
                    if (this.graph[u, i] > 0 && !this.visited[i])
                    {
                        q.Enqueue(i);
                        this.visited[i] = true;
                        this.parents[i] = u;
                    }
                }
            }

            return this.parents[sink] != -1;
        }

        private int MinValue(int i)
        {
            var ret = int.MaxValue;
            for (; this.parents[i] != -1; i = this.parents[i])
            {
                ret = Math.Min(ret, this.graph[this.parents[i], i]);
            }

            return ret;
        }

        private void AugmentPath(int value, int i)
        {
            for (; this.parents[i] != -1; i = this.parents[i])
            {
                this.graph[this.parents[i], i] -= value;
                this.graph[i, this.parents[i]] += value;
            }
        }
    }
}
