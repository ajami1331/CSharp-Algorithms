namespace Csharp_Contest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /*
     *  #import_Edge.cs
     */
    public class PushRelabel
    {
        private int nodes;
        private List<int>[] adj;
        private List<Edge> edges;
        private bool[] active;
        private long[] excess;
        private int[] dist;
        private int[] count;
        private List<int>[] B;
        private int b;

        public PushRelabel(int nodes)
        {
            this.nodes = nodes;
            this.adj = Enumerable.Repeat(0, nodes).Select(_ => new List<int>()).ToArray();
            this.edges = new List<Edge>();
            this.active = new bool[nodes];
            this.excess = new long[nodes];
            this.count = new int[nodes + 1];
            this.dist = new int[nodes];
            this.B = Enumerable.Repeat(0, nodes).Select(_ => new List<int>()).ToArray();
        }

        public void AddEdge(int from, int to, long capacity)
        {
            this.edges.Add(new Edge(from, to, capacity, 0, this.edges.Count + 1));
            this.edges.Add(new Edge(to, from, 0, 0, this.edges.Count - 1));
            this.adj[from].Add(this.edges.Count - 2);
            this.adj[to].Add(this.edges.Count - 1);
        }

        public long MaxFlow(int s, int t)
        {
            foreach (int e in this.adj[s])
            {
                this.excess[s] += this.edges[e].capacity;
            }

            this.count[0] = this.nodes;
            this.Enqueue(s);
            this.active[t] = true;

            while (this.b >= 0)
            {
                if (this.B[this.b].Count > 0)
                {
                    int v = this.B[this.b].Last();
                    this.B[this.b].RemoveAt(this.B[this.b].Count - 1);
                    this.active[v] = false;
                    this.Discharge(v);
                }
                else
                {
                    this.b--;
                }
            }

            return this.excess[t];
        }

        public List<int> this[int i] => this.adj[i];

        private void Enqueue(int v)
        {
            if (!this.active[v] && this.excess[v] > 0 && this.dist[v] < this.nodes)
            {
                this.active[v] = true;
                this.B[this.dist[v]].Add(v);
                this.b = Math.Max(this.b, this.dist[v]);
            }
        }

        private void Push(Edge e)
        {
            long amt = Math.Min(this.excess[e.from], e.capacity - e.flow);
            if (this.dist[e.from] == this.dist[e.to] + 1 && amt > 0)
            {
                e.flow += amt;
                this.edges[e.index].flow -= amt;
                this.excess[e.to] += amt;
                this.excess[e.from] -= amt;
                this.Enqueue(e.to);
            }
        }

        private void Gap(int k)
        {
            for (int v = 0; v < this.nodes; v++)
            {
                if (this.dist[v] >= k)
                {
                    this.count[this.dist[v]]--;
                    this.dist[v] = Math.Max(this.dist[v], this.nodes);
                    this.count[this.dist[v]]++;
                    this.Enqueue(v);
                }
            }
        }

        private void Relabel(int v)
        {
            this.count[this.dist[v]]--;
            this.dist[v] = this.nodes;
            foreach (int e in this.adj[v])
            {
                if (this.edges[e].capacity - this.edges[e].flow > 0)
                {
                    this.dist[v] = Math.Min(this.dist[v], this.dist[this.edges[e].to] + 1);
                }
            }

            this.count[this.dist[v]]++;
            this.Enqueue(v);
        }

        private void Discharge(int v)
        {
            for (int i = 0; i < this.adj[v].Count; i++)
            {
                if (this.excess[v] > 0)
                {
                    this.Push(this.edges[this.adj[v][i]]);
                }
                else
                {
                    break;
                }
            }

            if (this.excess[v] > 0)
            {
                if (this.count[this.dist[v]] == 1)
                {
                    this.Gap(this.dist[v]);
                }
                else
                {
                    this.Relabel(v);
                }
            }
        }
        public Edge GetEdge(int id)
        {
            return this.edges[id];
        }
    }
}