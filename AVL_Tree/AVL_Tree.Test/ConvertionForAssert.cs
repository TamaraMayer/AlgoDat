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
        public static string ConvertNodeListWithNullValuesToString(List<Node> nodes)
        {
            string s = "";

            foreach (Node n in nodes)
            {
                s += n.Value;
            }

            return s;
        }

        public static bool CompareNodesToRender(List<NodeToRender> expected,List<NodeToRender> actual)
        {
            if(expected.Count != actual.Count)
            {
                return false;
            }

            for (int i = 0; i < expected.Count; i++)
            {
                if(expected[i] == null && actual[i] == null)
                {
                    continue;
                }

                if(expected[i].ActualHeight != actual[i].ActualHeight)
                {
                    return false;
                }

                if (expected[i].Value != actual[i].Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
