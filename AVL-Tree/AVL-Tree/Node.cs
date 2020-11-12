using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL_Tree
{
    public class Node
    {
        //as leafnode is 0, only 0,1,or -1 is an aceptable value, otherwise turn tree
        public int BalanceFactor { get; set; }
        public int Height { get; set; }
        public int Value { get; set; }

        public Node Parent { get; set; }
        public Node Left { get; set; }

        public Node Right { get; set; }


        public Node(int value, Node parent)
        {
            this.Value = value;
            this.Height = 1;
            this.Parent = parent;
        }

    }
}
