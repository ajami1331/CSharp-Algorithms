﻿// Dinic.cs
// Author: Araf Al Jami
// Last Updated: 10-09-2565 20:57

namespace CLown1331.Library.Dinic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Dinic
    {
        private int nodes;
        private List<Edge> edges;
        private List<int>[] adj;
        private int[] level;
        private int[] ptr;

        public Dinic(int nodes)
        {
            this.nodes = nodes;
            this.edges = new List<Edge>();
            this.adj = Enumerable.Repeat(0, nodes).Select(_ => new List<int>()).ToArray();
            this.level = new int[nodes];
            this.ptr = new int[nodes];
        }

        public void ClearFlow()
        {
            foreach (Edge edge in this.edges)
            {
                edge.flow = 0;
            }
        }

        public void AddEdge(int from, int to, long capacity)
        {
            this.edges.Add(new Edge(from, to, capacity, 0, this.edges.Count + 1));
            this.edges.Add(new Edge(to, from, 0, 0, this.edges.Count - 1));
            this.adj[from].Add(this.edges.Count - 2);
            this.adj[to].Add(this.edges.Count - 1);
        }

        public long MaxFlow(int src, int sink)
        {
            long maxFlow = 0;
            while (this.Bfs(src, sink))
            {
                for (var i = 0; i < this.nodes; i++)
                {
                    this.ptr[i] = 0;
                }

                long flow;
                while ((flow = this.Dfs(src, sink, long.MaxValue)) > 0)
                {
                    maxFlow += flow;
                }
            }

            return maxFlow;
        }

        public List<int> this[int i]
        {
            get { return this.adj[i]; }
        }

        private bool Bfs(int src, int sink)
        {

            for (var i = 0; i < this.nodes; i++)
            {
                this.level[i] = -1;
            }

            var q = new Queue<int>();
            this.level[src] = 0;
            q.Enqueue(src);

            while (q.Count > 0)
            {
                int u = q.Dequeue();
                foreach (int v in this.adj[u])
                {
                    if (this.edges[v].capacity - this.edges[v].flow < 1)
                    {
                        continue;
                    }

                    if (this.level[this.edges[v].to] != -1)
                    {
                        continue;
                    }

                    this.level[this.edges[v].to] = this.level[u] + 1;
                    q.Enqueue(this.edges[v].to);
                }

            }

            return this.level[sink] != -1;
        }

        private long Dfs(int u, int t, long pushed)
        {
            if (pushed == 0)
            {
                return 0;
            }

            if (u == t)
            {
                return pushed;
            }

            for (; this.ptr[u] < this.adj[u].Count; this.ptr[u]++)
            {
                int index = this.adj[u][this.ptr[u]];
                int v = this.edges[index].to;
                long candidate = this.edges[index].capacity - this.edges[index].flow;
                if (this.level[u] + 1 != this.level[v] || candidate < 1)
                {
                    continue;
                }

                long flow = this.Dfs(v, t, Math.Min(pushed, candidate));
                if (flow == 0)
                {
                    continue;
                }

                this.edges[index].flow += flow;
                this.edges[index ^ 1].flow -= flow;
                return flow;
            }

            return 0;
        }

        public Edge GetEdge(int id) => this.edges[id];
    }
}
