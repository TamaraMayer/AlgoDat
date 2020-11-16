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

            TraverseInOrderForVisialisation(this.root,0);
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

                this.toDraw.Add(new Node(current.Value,actualHeight));

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
                        }
                        else
                        {
                            RecurviseInsert(root);
                        }
                    });
            }
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
                    }
                    else
                    {
                        RecurviseInsert(currentNode.Right);
                    }
                }
               

                    currentNode.CalculateBalanceFactor();
                    currentNode.CalculateHeight();
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
                    });
            }
        }

        private void Remove(Node toRemove)
        {
            if (toRemove.Left == null && toRemove.Right == null)
            {
                if (toRemove.Parent.Left.Value == toRemove.Value)
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
                Node temp = GetLeftestLeafNode(toRemove.Right);

                toRemove.Value = temp.Value;
                temp.Parent.Left = null;
               temp.CalculateHeight();
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
                if (current.Left.Value > lookFor)
                {
                    return Find(current.Left);
                }
                else
                {
                    return Find(current.Right);
                }
            }
            //check right+left for null and throw exception
        }

        private void Notify([CallerMemberName] string property = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
