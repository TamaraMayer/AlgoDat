using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace AVL_Tree
{
    class Tree_ViewModel : INotifyPropertyChanged
    {
        public List<Node> traversedList { get; private set; }
        public List<Node> toDraw { get; private set; }

        private int inputField;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler TreeChanged;

        public int InputField
        {
            get
            {
                return this.inputField;
            }
            set
            {
                this.inputField = value;
                this.Notify();
            }
        }

        internal void SetListToDraw()
        {
            toDraw.Clear();

            TraverseInOrderForVisialisation(this.root, 0);
        }

        //actual height has the topdown view on height, root has the lowest with 0, leafnotes have the highest
        private void TraverseInOrderForVisialisation(Node current, int actualHeight)
        {
            if (actualHeight == root.Height)
            {
                return;
            }

            if (current != null)
            {

                if (current.Left != null)
                {
                    TraverseInOrderForVisialisation(current.Left, actualHeight + 1);
                }
                else
                {
                    TraverseInOrderForVisialisation(null, actualHeight + 1);
                }

                this.toDraw.Add(new Node(current.Value, actualHeight));

                if (current.Right != null)
                {
                    TraverseInOrderForVisialisation(current.Right, actualHeight + 1);
                }
                else
                {
                    TraverseInOrderForVisialisation(null, actualHeight + 1);
                }
            }
            else
            {
                TraverseInOrderForVisialisation(null, actualHeight + 1);
                this.toDraw.Add(null);
                TraverseInOrderForVisialisation(null, actualHeight + 1);
            }
        }

        public Node root { get; set; }

        public Tree_ViewModel()
        {
            toDraw = new List<Node>();
            traversedList = new List<Node>();
        }

        public ICommand InsertCommand
        {
            get
            {
                return new Command
                (
                    obj =>
                    {
                        //trycatch

                        if (root == null)
                        {
                            root = new Node(this.InputField, null);
                            root.RebalanceEvent += Rebalance;
                        }
                        else
                        {
                            RecurviseInsert(root);
                        }

                        CalculateHeight(root);
                        this.FireTreeChangedEvent();
                    });
            }
        }

        private void Rebalance(object sender, EventArgs e)
        {
            Node current = sender as Node;

            int b_factor = current.BalanceFactor;
            if (b_factor > 1)
            {
                if (current.Left != null)
                {
                    if (current.Left.BalanceFactor > 0)
                    {
                        RotateRight(current);
                    }
                    else
                    {
                        RotateLR(current);
                    }
                }
            }
            else if (b_factor < -1)
            {
                if (current.Right != null)
                {
                    if (current.Right.BalanceFactor > 0)
                    {
                        RotateRL(current);
                    }
                    else
                    {
                        RotateLeft(current);
                    }
                }
            }

//            CalculateHeight(current);
            this.FireTreeChangedEvent();
        }

        private void BalanceToTop(Node current)
        {
            int b_factor = CalculateHeight(current.Left) - CalculateHeight(current.Right);

            current.BalanceFactor = b_factor;
        }

        private int CalculateHeight(Node current)
        {
            int height = 0;
            if (current != null)
            {
                int l = CalculateHeight(current.Left);
                int r = CalculateHeight(current.Right);
                int m = Math.Max(l, r);
                height = m + 1;
              //  current.Height = height;
            }
            return height;
        }

        private void RotateLeft(Node current)
        {
            Node temp = current.Right;
            current.Right = temp.Left;
            temp.Left = current;
            temp.Parent = current.Parent;
            temp.Left.Parent = temp;

            if(current.Right != null)
            {
                current.Right.Parent = current;
            }

            if (temp.Parent != null)
            {
                temp.Parent.Right = temp;
            }

            if (current == root)
            {
                root = temp;
            }

            //current.CalculateHeight();
            //temp.CalculateHeight();
        }

        private void RotateRL(Node current)
        {
            current.Right.Left.Right = current.Right;
            current.Right = current.Right.Left;
            current.Right.Parent = current;
            current.Right.Right.Left = null;
            current.Right.Right.Parent = current.Right;

           // RotateRight(current.Right);
            this.FireTreeChangedEvent();
            RotateLeft(current);
        }

        private void RotateLR(Node current)
        {

            current.Left.Right.Left = current.Left;
            current.Left = current.Left.Right;
            current.Left.Parent = current;
            current.Left.Left.Right = null;
            current.Left.Left.Parent = current.Left;


            // RotateLeft(current.Left);
            this.FireTreeChangedEvent();
            RotateRight(current);
        }

        private void RotateRight(Node current)
        {
            Node temp = current.Left;
            current.Left = temp.Right;
            temp.Right = current;
            temp.Parent = current.Parent;
            temp.Right.Parent = temp;

            if (current.Left != null)
            {
                current.Left.Parent = current;
            }

            if (temp.Parent != null)
            {
                temp.Parent.Right = temp;
            }

            if (current == root)
            {
                root = temp;
            }

            //current.CalculateHeight();
            //temp.CalculateHeight();
        }

        private void RecurviseInsert(Node currentNode)
        {
            if (currentNode.Value == inputField)
            {
                this.InputField = 0;
                //Message box, could not be inserted, but eigentlich als Exception
            }
            else
            {
                if (currentNode.Value > this.inputField)
                {
                    if (currentNode.Left == null)
                    {
                        //messagebox, was inserted
                        currentNode.Left = new Node(inputField, currentNode);
                        currentNode.Left.RebalanceEvent += Rebalance;
                        //TODO balance to top
                        BalanceToTop(currentNode.Left);
                    }
                    else
                    {
                        RecurviseInsert(currentNode.Left);
                    }
                }
                else
                {
                    if (currentNode.Right == null)
                    {
                        currentNode.Right = new Node(inputField, currentNode);
                        currentNode.Right.RebalanceEvent += Rebalance;
                        //TODO balance to top
                        BalanceToTop(currentNode.Right);
                    }
                    else
                    {
                        RecurviseInsert(currentNode.Right);
                    }
                }
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                return new Command
                (
                    obj =>
                    {
                        //trycatch, exception
                        Node toRemove = Find(this.root);
                        Remove(toRemove);

                        this.CalculateHeight(root);

                        this.FireTreeChangedEvent();
                    });
            }
        }

        public ICommand CountAllCommand
        {
            get
            {
                return new Command
                (
                    obj =>
                    {
                        //one traverse method, in ne liste speichern(passiert in traverse?!) und dann zählen
                    });
            }
        }

        public ICommand CountValueOccurenceCommand
        {
            get
            {
                return new Command
                (
                    obj =>
                    {
                        //find, if true return 1 as count, if false return 0 as count

                        Node node = Find(root);
                        if (node != null)
                        {

                        }
                        else
                        {

                        }
                    });
            }
        }

        public ICommand ContainsComand
        {
            get
            {
                return new Command
                (
                    obj =>
                    {
                        //try catch, catch = is not in the avl tree, otherwise is in, but check again
                        Node node = Find(root);
                        if (node != null)
                        {

                        }
                    });
            }
        }

        public ICommand TraverseInOrderCommand
        {
            get
            {
                return new Command
                    (
                    obj =>
                    {
                        TraverseInOrder(this.root);
                    });
            }
        }

        private void TraverseInOrder(Node current)
        {
            if (current != null)
            {
                if (current.Left != null)
                {
                    TraverseInOrder(current.Left);
                }

                this.traversedList.Add(current);

                if (current.Right != null)
                {
                    TraverseInOrder(current.Right);
                }
            }
        }

        public ICommand TraversePreOrderCommand
        {
            get
            {
                return new Command
                    (
                    obj =>
                    {
                        TraversePreOrder(this.root);
                    });
            }
        }

        private void TraversePreOrder(Node current)
        {
            if (current != null)
            {
                traversedList.Add(current);

                if (current.Left != null)
                {
                    TraversePreOrder(current.Left);
                }
                if (current.Right != null)
                {
                    TraversePreOrder(current.Right);
                }
            }
        }

        private void TraversePostOrder(Node current)
        {
            if (current != null)
            {
                if (current.Left != null)
                {
                    TraversePostOrder(current.Left);
                }
                if (current.Right != null)
                {
                    TraversePostOrder(current.Right);
                }

                traversedList.Add(current);
                //write bzw add to some list
            }
        }

        public ICommand TraversePostOrderCommand
        {
            get
            {
                return new Command
                    (
                    obj =>
                    {
                        TraversePostOrder(this.root);
                    });
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                return new Command
                (
                    obj =>
                    {
                        //set root to null?!, will depend on how view will work
                        this.root = null;
                        this.FireTreeChangedEvent();
                    });
            }
        }

        private void Remove(Node toRemove)
        {
            if (toRemove.Left == null && toRemove.Right == null)
            {
                if(toRemove.Parent.Left != null && toRemove.Parent.Left.Value == toRemove.Value)
                { 
                    toRemove.Parent.Left = null;
                }
                else
                {
                    toRemove.Parent.Right = null;
                }
                return;
            }

            if (toRemove.Left != null && toRemove.Right != null)
            {
                Node temp = GetLeftestLeafNode(toRemove.Left);

                toRemove.Value = temp.Value;
                Remove(temp);
                toRemove.CalculateHeight();
                return;
            }

            if (toRemove.Left != null)
            {
                toRemove.Value = toRemove.Left.Value;
                toRemove.Left = null;
                toRemove.CalculateHeight();
                return;
            }

            if (toRemove.Right != null)
            {
                toRemove.Value = toRemove.Right.Value;
                toRemove.Right = null;
                toRemove.CalculateHeight();
                return;
            }
        }

        private Node GetLeftestLeafNode(Node current)
        {
            if (current.Left != null)
            {
                return GetLeftestLeafNode(current.Left);
            }
            else if (current.Right != null)
            {
                return GetLeftestLeafNode(current.Right);
            }
            else
            {
                return current;
            }
        }

        private Node Find(Node current)
        {
            int lookFor = this.InputField;

            if (current.Value == lookFor)
            {
                return current;
            }
            else
            {
                if (current.Left != null)
                {
                    if (current.Value >= lookFor)
                    {
                        return Find(current.Left);
                    }
                }

                if (current.Right != null)
                {
                    return Find(current.Right);
                }
            }

            throw new ArgumentException("The given number is not inside the Tree!");
            //check right+left for null and throw exception
        }

        private void Notify([CallerMemberName] string property = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected virtual void FireTreeChangedEvent()
        {
            this.TreeChanged?.Invoke(this,null);
        }
    }
}
