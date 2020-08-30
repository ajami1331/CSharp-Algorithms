namespace Csharp_Contest
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
