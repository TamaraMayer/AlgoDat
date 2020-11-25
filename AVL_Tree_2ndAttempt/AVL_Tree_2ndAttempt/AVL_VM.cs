using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AVL_Tree_2ndAttempt
{
    class AVL_VM : INotifyPropertyChanged
    {
        // is the root node of the AVL tree
        public Node root { get; set; }

        //is the height of the root, will be calculated and saved with every rendering
        public int rootHeight { get; set; }

        //is the list of nodes in the traversed order
        public List<Node> traversedList { get; private set; }

        //is the list of nodes for rendering, in inOrderTraverse, with nulls to show empty spots in the visualization
        //is called and set from code behind
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

        //when triggered the tree changed in some way and has to be rendered new
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

        //called by code behind for visualization purposes only
        internal void SetListToDraw()
        {
            toDraw.Clear();
            this.rootHeight = CalculateHeight(root);

            TraverseInOrderForVisialisation(this.root, 0);
        }

        //actual height has the topdown view on height, root has the lowest with 0, leafnotes have the highest
        private void TraverseInOrderForVisialisation(Node current, int actualHeight)
        {
            //basically like traverse in order but with nulls so that "all" elements in the tree are contained

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
                        //trycatch in finally reset input field to zero, in catch block messagebox 

                        if (root == null)
                        {
                            root = new Node(this.InputField);
                        }
                        else
                        {
                            RecurviseInsert(root);
                        }
                        // rebalance is in recursive insert, every node from the one where it is inserted to the root is checked
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
                        //TODO trycatch, exception catch block message box with exception message
                        Node toRemoveParent = FindParent(this.root, this.InputField);
                        Remove(toRemoveParent, this.InputField);
                        Rebalance(toRemoveParent);
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
                        //traverse (can be any of the traverses) count that to get the number of elements in the tree

                        TraverseInOrder(root);
                        int i = traversedList.Count;

                        MessageBox.Show($"There are {i} elements in the tree!", "Count All", MessageBoxButton.OK);
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
                        //try catch if workes dann number exists, also occured one time; catch heißt es ist nicht im baum
                        try
                        {
                            Node node = Find(root);

                            MessageBox.Show($"The number {inputField} occurs once in the tree!", "Count Specific", MessageBoxButton.OK);
                        }
                        catch
                        {
                            MessageBox.Show($"The number {inputField} is not in the tree!", "Count Specific", MessageBoxButton.OK);
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
                        //try catch if workes dann number exists; catch heißt es ist nicht im baum
                        try
                        {
                            Node node = Find(root);

                            MessageBox.Show($"The number {inputField} is in the tree!", "Contains", MessageBoxButton.OK);
                        }
                        catch
                        {
                            MessageBox.Show($"The number {inputField} is not in the tree!", "Contains", MessageBoxButton.OK);
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
                        //clear the list, traverse starting at the root node, then show the message

                        traversedList.Clear();
                        TraverseInOrder(this.root);

                        ShowTraverseMessage("Traversed in Order");
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
                        //clear the list, traverse starting at the root node, then show the message

                        traversedList.Clear();
                        TraversePreOrder(this.root);

                        ShowTraverseMessage("Traversed pre Order");
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
                        //clear the list, traverse starting at the root node, then show the message

                        traversedList.Clear();
                        TraversePostOrder(this.root);

                        ShowTraverseMessage("Traversed post Order");
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
                        //set root node to null

                        this.root = null;
                        this.FireTreeChangedEvent();
                    });
            }
        }

        private void ShowTraverseMessage(string message)
        {
            string s = "";

            //goes through the traversed list and saves the value into the string, and adds a semicolon 
            //aslong as it is not the last element in the list.
            for (int i = 0; i < traversedList.Count; i++)
            {
                s += traversedList[i].Value;

                if (i != traversedList.Count - 1)
                {
                    s += ", ";
                }
            }

            MessageBox.Show(s, message, MessageBoxButton.OK);
        }

        public int CalculateHeight(Node current)
        {
            //recursivly calculates the height of all nodes, starting with the one in the parameter

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
            //recursivly goes through the tree until it either finds a node with the value and throws an exception
            //or a place to insert is found, after inserting all nodes that were went through will be checked if
            //this node is unbalanced and needs to be rotated

            if (currentNode.Value == InputField)
            {
                throw new ArgumentException($"{this.InputField} could not be inserted, it is already in the tree. No duplicates allowed!");
            }
            else
            {
                if (currentNode.Value > this.InputField)
                {
                    if (currentNode.Left == null)
                    {
                        currentNode.Left = new Node(inputField);

                        MessageBox.Show($"{this.InputField} was inserted!", "Insert", MessageBoxButton.OK);
                    }
                    else
                    {
                        RecurviseInsert(currentNode.Left);
                    }

                    Rebalance(currentNode);
                }
                else
                {
                    if (currentNode.Right == null)
                    {
                        currentNode.Right = new Node(inputField);

                        MessageBox.Show($"{this.InputField} was inserted!", "Insert", MessageBoxButton.OK);
                    }
                    else
                    {
                        RecurviseInsert(currentNode.Right);
                    }

                    Rebalance(currentNode);
                }
            }
        }

        private Node Find(Node current)
        {
            //recursivly goes through the tree until it either finds a node with the value and returns it
            //or throws an exception when the last node has no children

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
        }

        private Node FindParent(Node current, int target)
        {
            //recursivly goes through the tree until it either finds a node with the value and returns its parent
            //or throws an exception when the last node has no children

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
            //checks if the node to be removed is on the right or the left side of the prent node and sets the node that needs to be removed
            //there are 3 possibilities to remove the node
            //has no children, has 2 children, has only one child

            bool left;
            Node toRemove;

            left = IsLeft(toRemoveParent, target);

            if (left)
            {
                toRemove = toRemoveParent.Left;
            }
            else
            {
                toRemove = toRemoveParent.Right;
            }

            #region KEINE KINDER
            //wenn das zu entfernende Blatt keine Kinder hat, einfach das blatt entfernen
            //sprich beim parent die vorher herausgefundene seite auf null setzen
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
            //wenn das blatt zwei kinder hat, wird der nachfolger vom inorder traverse gesucht, also das linkeste blatt im rechten subbaum
            //dann den parent zu diesem nachfolger
            //dann wird der wert des inOrderSuccessor in den toRemoveNode gespeichtert und
            //der inOrderSuccessor bei seinem parent gelöscht
            if (toRemove.Left != null && toRemove.Right != null)
            {
                Node inOrderSuccessor = GetLeftestLeafNode(toRemove.Right);
                Node tempParent;

                if (inOrderSuccessor.Value == toRemove.Right.Value)
                {
                    tempParent = toRemove;
                }
                else
                {
                    tempParent = FindParent(toRemove.Right, inOrderSuccessor.Value);
                }

                toRemove.Value = inOrderSuccessor.Value;
                Remove(tempParent, inOrderSuccessor.Value);

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
            //else
            //if (current.Right != null)
            //{
            //    return GetLeftestLeafNode(current.Right);
            //}
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



        private void Rebalance(Node current)
        {
            //checks for null are "unnesseccary", balance factor would return 0

            int b_factor = CalculateBalanceFactor(current);

            if (b_factor > 1)
            {
                if (current.Left != null)
                {
                    if (CalculateBalanceFactor(current.Left) > 0)
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
                    if (CalculateBalanceFactor(current.Right) > 0)
                    {
                        RotateRL(current);
                    }
                    else
                    {
                        RotateLeft(current);
                    }
                }
            }

            this.FireTreeChangedEvent();
        }

        private int CalculateBalanceFactor(Node current)
        {
            int l = CalculateHeight(current.Left);
            int r = CalculateHeight(current.Right);
            int b_factor = l - r;
            return b_factor;
        }

        private bool IsLeft(Node parent, int target)
        {
            if (parent.Left != null)
            {
                if (parent.Left.Value == target)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void RotateLeft(Node current)
        {
            Node parent;
            bool left = false;

            try
            {
                parent = FindParent(root, current.Value);
                left = IsLeft(parent, current.Value);
            }
            catch
            {
                parent = null;
            }





            Node temp = current.Right;
            current.Right = temp.Left;
            temp.Left = current;

            if (parent != null)
            {
                if (left)
                {
                    parent.Left = temp;

                }
                else
                {
                    parent.Right = temp;
                }
            }

            if (current == root)
            {
                root = temp;
            }

        }

        private void RotateRL(Node current)
        {
            //current.Right.Left.Right = current.Right;
            //current.Right = current.Right.Left;
            //current.Right.Parent = current;
            //current.Right.Right.Left = null;
            //current.Right.Right.Parent = current.Right;

            //// RotateRight(current.Right);
            //this.FireTreeChangedEvent();
            //RotateLeft(current);

            Node temp = current.Right;
            RotateRight(temp);
            RotateLeft(current);
        }

        private void RotateLR(Node current)
        {

            //current.Left.Right.Left = current.Left;
            //current.Left = current.Left.Right;
            //current.Left.Parent = current;
            //current.Left.Left.Right = null;
            //current.Left.Left.Parent = current.Left;


            //// RotateLeft(current.Left);
            //this.FireTreeChangedEvent();
            //RotateRight(current);

            Node temp = current.Left;
            RotateLeft(temp);
            RotateRight(current);
        }

        private void RotateRight(Node current)
        {
            Node parent;
            bool left = false;

            try
            {
                parent = FindParent(root, current.Value);
                left = IsLeft(parent, current.Value);
            }
            catch
            {
                parent = null;
            }

            Node temp = current.Left;
            current.Left = temp.Right;
            temp.Right = current;

            if (parent != null)
            {
                if (left)
                {
                    parent.Left = temp;
                }
                else
                {
                    parent.Right = temp;
                }
            }

            if (current == root)
            {
                root = temp;
            }
        }
    }
}
