using System;
using System.Collections.Generic;
using System.Text;

namespace AVL_Tree.Test
{
    class ConvertionForAssert
    {
        public static string ConvertNodeListToString(List<Node> nodes)
        {
            string s = "";

            foreach (Node n in nodes)
            {
                s += n.Value;
            }

            return s;
        }
    }
}
