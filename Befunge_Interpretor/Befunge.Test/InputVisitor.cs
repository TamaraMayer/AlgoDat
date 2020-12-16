using Befunge_Interpretor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Befunge.Test
{
    public class InputVisitor : IInputVisitor
    {
        public string GetUserInput(string messageToUser)
        {
            if(messageToUser == "Please enter a number!")
            {
                return "13";
            }
            else
            {
                return "ñ";
            }
            
        }
    }
}
