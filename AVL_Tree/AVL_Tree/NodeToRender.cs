using System;
using System.Collections.Generic;
using System.Text;

namespace AVL_Tree
{
    public class NodeToRender
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
