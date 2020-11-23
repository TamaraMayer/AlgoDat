using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AVL_Tree_2ndAttempt
{
    class AVL_VM : INotifyPropertyChanged
    {
        public Node root { get; set; }
        public int rootHeight { get; set; }
        public List<Node> traversedList { get; private set; }
        public List<NodeToRender> toDraw { get; private set; }

        private int inputField;
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

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler TreeChangedEvent;

        public AVL_VM()
        {
            traversedList = new List<Node>();
            toDraw = new List<NodeToRender>();
        }

        private void Notify([CallerMemberName] string property = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected virtual void FireTreeChangedEvent()
        {
            this.TreeChangedEvent?.Invoke(this, null);
        }

        internal void SetListToDraw()
        {
            toDraw.Clear();
            this.rootHeight = CalculateHeight(root);

            TraverseInOrderForVisialisation(this.root, 0);
        }

        //actual height has the topdown view on height, root has the lowest with 0, leafnotes have the highest
        private void TraverseInOrderForVisialisation(Node current, int actualHeight)
        {
            if (actualHeight == this.rootHeight)
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

                this.toDraw.Add(new NodeToRender(current.Value, actualHeight));

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
                            root = new Node(this.InputField);
                        }
                        else
                        {
                            RecurviseInsert(root);
                        }

                        this.FireTreeChangedEvent();
                    });
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
                        Node toRemove = FindParent(this.root, this.InputField);
                        Remove(toRemove, this.InputField);

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

        public int CalculateHeight(Node current)
        {
            int height = 0;
            if (current != null)
            {
                int l = CalculateHeight(current.Left);
                int r = CalculateHeight(current.Right);
                int m = Math.Max(l, r);
                height = m + 1;
            }
            return height;
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
                        //TODO messagebox, was inserted
                        currentNode.Left = new Node(inputField);
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
                        //TODO messagebox, was inserted
                        currentNode.Right = new Node(inputField);
                    }
                    else
                    {
                        RecurviseInsert(currentNode.Right);
                    }
                }
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

        private Node FindParent(Node current, int target)
        {
            if (current.Left != null)
            {
                if (current.Left.Value == target)
                {
                    return current;
                }
            }

            if (current.Right != null)
            {
                if (current.Right.Value == target)
                {
                    return current;
                }
            }

            if (current.Value >= inputField)
            {
                if (current.Left != null)
                {
                    return FindParent(current.Left, target);
                }
            }
            else
            {
                if (current.Right != null)
                {
                    return FindParent(current.Right, target);
                }
            }

            throw new ArgumentException("The given number is not inside the Tree!");
            //check right+left for null and throw exception
        }

        private void Remove(Node toRemoveParent, int target)
        {
            bool left;
            Node toRemove;

            if (toRemoveParent.Left != null)
            {
                if (toRemoveParent.Left.Value == target)
                {
                    left = true;
                    toRemove = toRemoveParent.Left;
                }
                else
                {
                    left = false;
                    toRemove = toRemoveParent.Right;
                }
            }
            else
            {
                left = false;
                toRemove = toRemoveParent.Right;
            }

            #region KEINE KINDER
            //  wenn das zu entfernende Blatt keine Kinder hat
            if (toRemove.Left == null && toRemove.Right == null)
            {
                if (left)
                {
                    toRemoveParent.Left = null;
                }
                else
                {
                    toRemoveParent.Right = null;
                }
                return;
            }
            #endregion KEINE KINDER

            #region ZWEI KINDER
            if (toRemove.Left != null && toRemove.Right != null)
            {
                Node inOrderSuccessor = GetLeftestLeafNode(toRemove.Right);
                Node tempParent = FindParent(toRemove.Right, inOrderSuccessor.Value);

                if(tempParent.Left!= null)
                {
                    toRemove.Value = inOrderSuccessor.Value;
                    tempParent.Left = null;
                }
                else
                {
                    if (left)
                    {
                        toRemoveParent.Left = inOrderSuccessor;
                    }
                    else
                    {
                        toRemoveParent.Right = inOrderSuccessor;
                    }
                }

                return;
            }
            #endregion ZWEI KINDER

            #region ONLY ONE CHILD
            if (toRemove.Left != null)
            {
                toRemoveParent.Left = toRemove.Left;
                return;
            }

            if (toRemove.Right != null)
            {
                toRemoveParent.Right = toRemove.Right;
                return;
            }
            #endregion ONLY ONE CHILD
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

    }
}
