// Edge.cs
// Author: Araf Al Jami
// Last Updated: 07-09-2565 03:03

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
