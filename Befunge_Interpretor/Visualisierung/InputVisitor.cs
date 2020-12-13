using Befunge_Interpretor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Visualisierung
{
    public class InputVisitor : IInputVisitor
    {
        public string GetUserInput(string messageToUser)
        {
            InputPopUp instance = new InputPopUp();
            instance.MessageToUser.Text= messageToUser;

           if(instance.ShowDialog() == true)
            {
                string input = instance.MessageToUser.Text;
                instance.Close();
                return input;
            }
            return "";
        }
    }
}
