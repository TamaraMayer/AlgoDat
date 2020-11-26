using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("AVL_Tree.Test")]

namespace AVL_Tree
{
    public class AVL_VM : INotifyPropertyChanged
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

        //Property the input field in the view is bound to, thats the reason for the notify
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
            //die 2 listen instanzieren

            traversedList = new List<Node>();
            toDraw = new List<NodeToRender>();

            //this.root = new Node(7);
            //this.root.Right = new Node(8);
            //this.root.Right.Right = new Node(9);
            //this.root.Left = new Node(2);
            //this.root.Left.Left = new Node(1);
            //this.root.Left.Right = new Node(5);
            //this.root.Left.Right.Left = new Node(4);
            //this.root.Left.Right.Right = new Node(6);
        }

        private void Notify([CallerMemberName] string property = null)
        {
            //fires event that a property has changed

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected virtual void FireTreeChangedEvent()
        {
            //fires the event that the tree has changed

            this.TreeChangedEvent?.Invoke(this, null);
        }

        public ICommand InsertCommand
        {
            get
            {
                return new Command
                (
                    obj =>
                    {
                        //if root is null add the value as the root node, otherwise call recursive insert with the root
                        //after inserting the tree changed event is called/fired
                        //recursive insert can throw an exception when the value is already in the list
                        //when this exception is caught shows the message to the user
                        //finally resets the inputfield to zero

                        try
                        {
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
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Insert", MessageBoxButton.OK);
                        }
                        finally
                        {
                            this.InputField = 0;
                        }
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
                        //Checks if the value to be removed is the value of the root node, if yes, calls the remove root method, otherwise
                        //trys to find the parent of the node with the to remove value, can throw an exception
                        //after finding the parent calls the remove method
                        //and in both cases rebalances the tree and fires the tree change event
                        //when the exception from FindParent is caught shows the message to the user
                        //finally resets the inputfield to zero

                        Node toRemoveParent;

                        if (this.root == null)
                        {
                            MessageBox.Show("There is no tree, nothing can be removed", "Remove", MessageBoxButton.OK);
                        }

                        try
                        {
                            if (root.Value == this.InputField)
                            {
                                RemoveRootValue();

                                if (this.root != null)
                                {
                                    Rebalance(this.root);
                                }
                            }
                            else
                            {
                                toRemoveParent = FindParent(this.root, this.InputField);
                                Remove(toRemoveParent, this.InputField);

                                Rebalance(toRemoveParent);
                            }

                            this.FireTreeChangedEvent();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Remove", MessageBoxButton.OK);
                        }
                        finally
                        {
                            this.InputField = 0;
                        }
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
                        //tries to find the number in the input field starting at the root, can throw an exception
                        //if no exception thrown and caught the number occurs exactly once since in this tree no suplicates are allowed, otherwise it is not in the tree
                        //finally sets the inputfield to zero

                        try
                        {
                            Node node = Find(root, this.InputField);

                            MessageBox.Show($"The number {inputField} occurs once in the tree!", "Count Specific", MessageBoxButton.OK);
                        }
                        catch
                        {
                            MessageBox.Show($"The number {inputField} is not in the tree!", "Count Specific", MessageBoxButton.OK);
                        }
                        finally
                        {
                            this.InputField = 0;
                        }
                    });
            }
        }
        public ICommand ContainsCommand
        {
            get
            {
                return new Command
                (
                    obj =>
                    {
                        //tries to find the number in the input field starting at the root, can throw an exception
                        //if no exception thrown and caught the number occurs, otherwise not
                        //finally sets the inputfield to zero

                        try
                        {
                            Node node = Find(root, this.InputField);

                            MessageBox.Show($"The number {inputField} is in the tree!", "Contains", MessageBoxButton.OK);
                        }
                        catch
                        {
                            MessageBox.Show($"The number {inputField} is not in the tree!", "Contains", MessageBoxButton.OK);
                        }
                        finally
                        {
                            this.InputField = 0;
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
                        //set root node to null and fire tree changed event

                        this.root = null;
                        this.FireTreeChangedEvent();
                    });
            }
        }

        internal void ShowTraverseMessage(string message)
        {
            //goes through the traversed list and saves the value into the string, and adds a semicolon 
            //aslong as it is not the last element in the list.

            string s = "";

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
        internal void RecurviseInsert(Node currentNode)
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
                        currentNode.Right = new Node(inputField);
                    }
                    else
                    {
                        RecurviseInsert(currentNode.Right);
                    }
                }

                Rebalance(currentNode);
            }
        }

        internal Node Find(Node current, int lookFor)
        {
            //recursivly goes through the tree until it either finds a node with the value and returns it
            //or throws an exception when the last node has no children

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
                        return Find(current.Left, lookFor);
                    }
                }

                if (current.Right != null)
                {
                    return Find(current.Right, lookFor);
                }
            }

            throw new ArgumentException("The given number is not inside the Tree!");
        }

        internal Node FindParent(Node current, int target)
        {
            //just a precausion, should never be thrown within this whole code
            if (current == null)
            {
                throw new ArgumentOutOfRangeException(nameof(current), "The specified parameter may not be null!");
            }

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

            if (current.Value > target)
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
        }

        internal void Remove(Node toRemoveParent, int target)
        {
            //just a precausion, should never be thrown within this whole code
            if (toRemoveParent == null)
            {
                throw new ArgumentOutOfRangeException(nameof(toRemoveParent), "The specified parameter may not be null!");
            }

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

            //könnt eine eigene Methode sein weil es hier und im RemoveRoot genau gleich ist
            #region ZWEI KINDER
            //wenn das blatt zwei kinder hat, wird der nachfolger vom inorder traverse gesucht, also das linkeste blatt im rechten subbaum
            //dann den parent zu diesem nachfolger
            //dann wird der wert des inOrderSuccessor in den toRemoveNode gespeichtert und
            //dann rekursiv der inOrderSuccessor removed
            if (toRemove.Left != null && toRemove.Right != null)
            {
                Node inOrderSuccessor = GetInOrderSuccessor(toRemove.Right);
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
            //checks on which side the toRemove node has the child, and sets this to the spot of the toRemove node beim toRemoveParent node
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

        internal void RemoveRootValue()
        {
            //sets root node as node toRemove
            //there are 3 possibilities to remove the node
            //has no children, has 2 children, has only one child

            Node toRemove = this.root;

            #region KEINE KINDER
            //wenn das zu entfernende Blatt keine Kinder hat, einfach das blatt entfernen
            //weil es die root ist, diese auf null setzen
            if (toRemove.Left == null && toRemove.Right == null)
            {
                this.root = null;
            }
            #endregion KEINE KINDER

            //könnt eine eigene Methode sein weil es hier und im Remove genau gleich ist
            #region ZWEI KINDER
            //wenn das blatt zwei kinder hat, wird der nachfolger vom inorder traverse gesucht, also das linkeste blatt im rechten subbaum
            //dann den parent zu diesem nachfolger
            //dann wird der wert des inOrderSuccessor in den toRemoveNode gespeichtert und
            //dann rekursiv der inOrderSuccessor removed
            if (toRemove.Left != null && toRemove.Right != null)
            {
                Node inOrderSuccessor = GetInOrderSuccessor(toRemove.Right);
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
            //checks on which side the toRemove node has the child, and sets this to the spot of the toRemove node beim toRemoveParent node
            if (toRemove.Left != null)
            {
                this.root = toRemove.Left;
                return;
            }

            if (toRemove.Right != null)
            {
                this.root = toRemove.Right;
                return;
            }
            #endregion ONLY ONE CHILD
        }

        internal Node GetInOrderSuccessor(Node current)
        {
            //checks if the given node has a left child,
            //if yes, calls itself with the left child
            //if no returns itself

            if (current.Right.Left != null)
            {
                return GetInOrderSuccessor(current.Right.Left);
            }
            else
            {
                return current.Right;
            }
        }
        internal void TraverseInOrder(Node current)
        {
            //if the node is not null, checks if there is a left child
            //if yes, calls itself with the left child
            //then adds the current node to the list
            //then checks if there is a right child, if yes calls itself with the right child

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
        internal void TraversePreOrder(Node current)
        {
            //if the node is not null
            //adds the current node to the list
            //then checks if there is a left child, if yes calls itself with the left child
            //then checks if there is a right child, if yes calls itself with the right child

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
        internal void TraversePostOrder(Node current)
        {
            //if the node is not null
            //checks if there is a left child, if yes calls itself with the left child
            //then checks if there is a right child, if yes calls itself with the right child
            //then adds the current node to the list

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
            }
        }

        internal void SetListToDraw()
        {
            //called by code behind for visualization purposes only
            //first clears the list, then calculates the height of the root
            //then traverses the tree for visualization

            toDraw.Clear();
            this.rootHeight = CalculateHeight(root);

            TraverseInOrderForVisialisation(this.root, 0);
        }

        //actual height has the topdown view on height, root has the lowest with 0, leafnotes have the highest
        internal void TraverseInOrderForVisialisation(Node current, int actualHeight)
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

        internal void Rebalance(Node current)
        {
            //checks for null are "unnesseccary", balance factor would return 0

            //calculates balance factor for the current node,
            //if its grater than 1, checks the left side of the node
            //if the balance factor of the left side is bigger than zero it calls the rotate right method, otherwise the rotate left right method
            //if the balance factor of the current node is smaller than or equal to zero, checks if the balance factor is smaller than minus 1
            //if it is, checks the right side of the node
            //if the balance factor of the left side is bigger than zero it calls the rotate right left method, otherwise the rotate left method

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
        }

        internal int CalculateBalanceFactor(Node current)
        {
            //calculates height of the left and right subtree
            //subtracts right height from left and return it

            int l = CalculateHeight(current.Left);
            int r = CalculateHeight(current.Right);
            int b_factor = l - r;
            return b_factor;
        }

        internal bool IsLeft(Node parent, int target)
        {
            //checks if the target value is on the left side of the given node
            //return true if yes,
            //return false if not on left side, or left side is null

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

        internal void RotateLeft(Node current)
        {
            Node parent;
            bool left = false;

            //tries to find the parent of the node to rotate, if there is one checks on which side it is
            //if there is none (which means the current node is the root), set parent to null

            try
            {
                parent = FindParent(root, current.Value);
                left = IsLeft(parent, current.Value);
            }
            catch
            {
                parent = null;
            }

            //sets a temporary node that gets the value and children of the right child of the current node,
            //the right child becomes the value and children of the left child of the temporary node
            //and the temporary nodes left child is set to the current node

            Node temp = current.Right;
            current.Right = temp.Left;
            temp.Left = current;

            //if there is a parent set the according child to be the temporary node
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

            //if the node current node was the root, the root is now the temporary node
            if (current == root)
            {
                this.root = temp;
            }

        }

        internal void RotateRL(Node current)
        {
            //Sets a temporary node to the right child of the current node
            //then rotates the temporary node right
            //and then rotates the current node left

            Node temp = current.Right;
            RotateRight(temp);
            RotateLeft(current);
        }

        internal void RotateLR(Node current)
        {
            //Sets a temporary node to the left child of the current node
            //then rotates the temporary node left
            //and then rotates the current node right

            Node temp = current.Left;
            RotateLeft(temp);
            RotateRight(current);
        }

        internal void RotateRight(Node current)
        {
            Node parent;
            bool left = false;

            //tries to find the parent of the node to rotate, if there is one checks on which side it is
            //if there is none (which means the current node is the root), set parent to null

            try
            {
                parent = FindParent(root, current.Value);
                left = IsLeft(parent, current.Value);
            }
            catch
            {
                parent = null;
            }

            //sets a temporary node that gets the value and children of the left child of the current node,
            //the left child becomes the value and children of the right child of the temporary node
            //and the temporary nodes right child is set to the current node

            Node temp = current.Left;
            current.Left = temp.Right;
            temp.Right = current;

            //if there is a parent set the according child to be the temporary node
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

            //if the node current node was the root, the root is now the temporary node
            if (current == root)
            {
                root = temp;
            }
        }
    }
}
