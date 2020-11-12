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
        private int inputField;

        private string messageForUser;
        private SolidColorBrush messageColor;

        public event PropertyChangedEventHandler PropertyChanged;

        public string MessageForUser
        {
            get
            {
                return this.messageForUser;
            }
            set
            {
                this.messageForUser = value;
                this.Notify();
            }
        }
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

        public SolidColorBrush MessageColor
        {
            get
            {
                return this.messageColor;
            }
            set
            {
                this.messageColor = value;
                this.Notify();
            }
        }

        public Node root { get; set; }

        public ObservableCollection<Node> Tree { get; set; }

        public Tree_ViewModel()
        {
            Tree = new ObservableCollection<Node>();
        }

        public ICommand InsertCommand
        {
            get
            {
                return new Command
                (
                    obj =>
                    {

                        if (root == null)
                        {
                            root = new Node(this.InputField,null);
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
                this.MessageForUser = "This number is already in the tree. No duplicate allowed!";
                this.MessageColor = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else
            {
                if (currentNode.Value > this.inputField)
                {
                    if(currentNode.Left == null)
                    {
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
            }

        }

        private void Notify([CallerMemberName] string property = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
