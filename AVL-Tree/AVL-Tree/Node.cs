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

        //as leafnode is 0; only 0,1,or -1 is an aceptable value, otherwise turn tree
        public int BalanceFactor
        {
            get
            {
                return this.balanceFactor;
            }
            set
            {
                if (value > 1 || value < -1)
                {
                    this.balanceFactor = value;
                  this.FireRebalanceEvent();
                  //  this.Rebalance();
                }
                else
                {
                    if (Parent != null)
                    {
                      Parent.CalculateHeight();
                    }
                }
                //TODO: eventuell nicht immer speichern sonder erst wenn rebalance sicher passt 
                this.balanceFactor = value;
            }
        }

        public event EventHandler RebalanceEvent;

        protected virtual void FireRebalanceEvent()
        {
            RebalanceEvent?.Invoke(this, null);
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
                CalculateBalanceFactor();
            }
        }

        public Node Parent { get; set; }
        public Node Left { get; set; }

        public Node Right { get; set; }


        public Node( int value, int actualheight)
        {
            this.Height = actualheight;
            this.Value = value;
        }

        public Node(int value, Node parent)
        {
            this.Value = value;
            this.Height = 1;
            this.Parent = parent;
        }

        public void CalculateBalanceFactor()
        {
            int l;
            int r;

            if (this.Left != null)
            {
                 l = this.Left.Height;
            }
            else
            {
                l = 0;
            }

            if (this.Right != null)
            {
                r = this.Right.Height;
            }
            else
            {
                r = 0;
            }

            int b_factor = l - r;
            this.BalanceFactor = b_factor;
        }

        public void CalculateHeight()
        {
            int height = 0;
            int l;
            int r;

            if (this.Left != null)
            {
                l = this.Left.Height;
            }
            else
            {
                l = 0;
            }

            if (this.Right != null)
            {
                r = this.Right.Height;
            }
            else
            {
                r = 0;
            }

            int m = Math.Max(l, r);
            height = m + 1;
            this.Height= height;
        }


    }
}
