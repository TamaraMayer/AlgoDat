using System;
using System.Collections.Generic;
using System.Text;
using Befunge_Interpretor;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

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

        public Thread thread { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RunCodeCommand
        {
            get
            {
                return new Command(obj =>
                {
                    this.Output = "";

                    this.thread = new Thread(obj =>
                    {
                        interpretor = new Interpretor(Code, visitor);
                        interpretor.OnNewOutput += Interpretor_OnNewOutput;
                        interpretor.CodeEnd += OnCodeEnd;
                        interpretor.Run();
                    });
                    this.thread.IsBackground = true;
                    this.thread.Start();
                });
            }
        }

        public ICommand StopCodeCommand
        {
            get
            {
                return new Command(obj =>
                {
                    if (this.interpretor != null)
                    {
                        this.interpretor.end = true;
                    }
                });
            }
        }

        private void OnCodeEnd(object sender, EventArgs e)
        {
            MessageBox.Show("The Code reached its end!", "End", MessageBoxButton.OK);

            if (this.thread.IsAlive)
            {
                this.thread.Abort();
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
            //Application.Current.Dispatcher.Invoke(() =>
            //{
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

            //});

        }
    }
}
