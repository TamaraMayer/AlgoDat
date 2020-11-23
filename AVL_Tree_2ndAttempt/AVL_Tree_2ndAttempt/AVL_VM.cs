using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AVL_Tree_2ndAttempt
{
    class AVL_VM : INotifyPropertyChanged
    {
       public Node root { get; set; }
        public List<Node> traversedList { get; private set; }
        public List<Node> toDraw { get; private set; }

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

        private void Notify([CallerMemberName] string property = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected virtual void FireTreeChangedEvent()
        {
            this.TreeChangedEvent?.Invoke(this, null);
        }
    }
}
