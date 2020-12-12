using System;
using System.Collections.Generic;
using System.Text;

namespace Befunge_Interpretor
{
    public interface IInputVisitor
    {
        public string GetUserInput(string messageToUser);
    }
}
