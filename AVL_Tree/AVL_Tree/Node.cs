﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AVL_Tree
{
    public class Node
    {
        public Node Left { get; set; }
        public Node Right { get; set; }
        public int Value { get; set; }

        public Node(int value)
        {
            this.Value = value;
        }
    }
}
