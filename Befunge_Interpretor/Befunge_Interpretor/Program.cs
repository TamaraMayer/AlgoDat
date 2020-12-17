using System;

namespace Befunge_Interpretor
{
   public class Program
    {
       public static void Main(string[] args)
        {
            Interpretor i = new Interpretor(">               v\r\nv  ,,,,, \"Hello\"<\r\n> 48*,          v\r\nv,,,,,, \"World!\"<\r\n> 25*,@", new InputVisitor());
            i.Run();
        }
    }
}
