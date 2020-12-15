using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class Edge
    {
        private string v;
        private string u;
        private int w;
        private int flow;

        public Edge(string v, string u, int w)
        {
            this.V = v;
            this.U = u;
            this.W = w;
            this.Flow = 0;
        }

        public string V { get => v; set => v = value; }
        public string U { get => u; set => u = value; }
        public int W { get => w; set => w = value; }
        public int Flow { get => flow; set => flow = value; }
    }
}
