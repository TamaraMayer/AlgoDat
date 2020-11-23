using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL_Tree_2ndAttempt
{
    class NodeToRender
    {
        public int Value { get; set; }
        public int ActualHeight { get; set; }


        public NodeToRender(int value, int actualHeight)
        {
            this.Value = value;
            this.ActualHeight = actualHeight;
        }
    }
}
