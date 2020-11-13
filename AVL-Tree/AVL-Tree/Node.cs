using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL_Tree
{
    public class Node
    {
        private int height;
        private int balanceFactor;

        //as leafnode is 0, only 0,1,or -1 is an aceptable value, otherwise turn tree
        public int BalanceFactor { 
            get
            {
                return this.balanceFactor;
            }
            set
            {
                if(value >1 || value < -1)
                {
                    this.Rebalance();
                }
                //TODO: eventuell nicht immer speichern sonder erst wenn rebalance sicher passt 
                this.balanceFactor = value;
            }
        }

        private void Rebalance()
        {
            throw new NotImplementedException();
        }

        public int Value { get; set; }
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
                CalculateBalanceFactor(this);
            }
        }

        public Node Parent { get; set; }
        public Node Left { get; set; }

        public Node Right { get; set; }


        public Node(int value, Node parent)
        {
            this.Value = value;
            this.Height = 1;
            this.Parent = parent;
            CalculateBalanceFactor(this.Parent);
            CalculateHeight(this.Parent);
        }

        private void CalculateBalanceFactor(Node node)
        {
            //TODO:überprüfen ob left und right nicht null?!
            int l = this.Left.Height;
            int r = this.Right.Height;
            int b_factor = l - r;
            this.BalanceFactor = b_factor;
        }

        private void CalculateHeight(Node node)
        {
            int height = 0;
            //TODO:überprüfen ob left und right nicht null?!

            int l = this.Left.Height;
            int r = this.Right.Height;
            int m = Math.Max(l, r);
            height = m + 1;
            this.Height = height;
        }


    }
}
