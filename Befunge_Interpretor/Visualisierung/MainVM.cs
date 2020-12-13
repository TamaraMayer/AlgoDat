using System;
using System.Collections.Generic;
using System.Text;
using Befunge_Interpretor;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Visualisierung
{
    public class MainVM :INotifyPropertyChanged
    {
        public bool CanInputCode
        {
            get{
                return this.canInputCode;
            }
            set
            {
                this.canInputCode = value;
                Notify();
            }
        }

        public string Code { get; set; }

        public string Output
        {
            get
            {
                return this.output;
            }
            set
            {
                this.output = value;
                this.Notify();
            }
        }

        private Interpretor interpretor;

        private InputVisitor visitor;
        private string output;
        private bool canInputCode;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RunCodeCommand
        {
            get
            {
                return new Command(obj =>
                {
                    this.Output = "";

                    interpretor = new Interpretor(Code, visitor);
                    interpretor.OnNewOutput += Interpretor_OnNewOutput;
                    interpretor.Run();
                });
            }
        }

        private void Interpretor_OnNewOutput(object sender, OnOutpuEventArgs e)
        {
            this.Output += e.Output;
        }

        public MainVM()
        {
            this.visitor = new InputVisitor();
            this.CanInputCode = true;
        }

        private void Notify([CallerMemberName]string property = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
