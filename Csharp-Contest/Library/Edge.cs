// Edge.cs
// Authors: Araf Al-Jami
// Created: 30-08-2020 5:46 PM
// Updated: 08-07-2021 3:44 PM

namespace Library.Edge
{
    public class Edge
    {
        public int from;
        public int to;
        public long capacity;
        public long flow;
        public int index;

        public Edge()
        {

        }

        public Edge(int from, int to, long capacity, long flow, int index)
        {
            this.from = from;
            this.to = to;
            this.capacity = capacity;
            this.flow = flow;
            this.index = index;
        }
    }

}
