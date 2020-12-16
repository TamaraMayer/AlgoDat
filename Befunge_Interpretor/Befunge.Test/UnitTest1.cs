using Microsoft.VisualStudio.TestTools.UnitTesting;
using Befunge_Interpretor;

namespace Befunge.Test
{
    [TestClass]
    public class UnitTest1
    {
        private string output;
        private IInputVisitor visitor;

        [TestInitialize]
        public void Setup()
        {
            output = "";
            this.visitor = new InputVisitor();
        }

        [TestMethod]
        public void TestMethod1()
        {
            Interpretor interpretor = new Interpretor(">25*\"!dlrow,< olleH\":v\r\n                  v:, _@\r\n                  > ^",this.visitor);
        }

        public void ReactOnNewOutput(object sender, OnOutpuEventArgs e)
        {
            output += e;
        }
    }
}
