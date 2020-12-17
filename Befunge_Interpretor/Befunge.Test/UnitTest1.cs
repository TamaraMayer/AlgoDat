using Microsoft.VisualStudio.TestTools.UnitTesting;
using Befunge_Interpretor;
using System;

namespace Befunge.Test
{
    [TestClass]
    public class UnitTest1
    {
        private string actual;
        private IInputVisitor visitor;

        [TestInitialize]
        public void Setup()
        {
            actual = "";
            this.visitor = new InputVisitor();
        }

        [DataTestMethod]
        [DataRow ("> 25 *\"!dlrow ,olleH\":v\r\n                   v:,_@\r\n                   >  ^", "Hello, world!\n")]
        [DataRow (">               v\r\nv  ,,,,, \"Hello\"<\r\n> 48*,          v\r\nv,,,,,, \"World!\"<\r\n> 25*,@", "Hello World!\n")]
        public void ShouldRunCode(string input, string expected)
        {
            Interpretor interpretor = new Interpretor(input,this.visitor);
            interpretor.OnNewOutput += ReactOnNewOutput;
            interpretor.Run();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldThrowArgumentNullException_BecauseInputIsNull()
        {
            string exceptionMessageActual="";

            try
            {
                Interpretor interpretor = new Interpretor(null,this.visitor);
            }
            catch (ArgumentNullException e)
            {
                exceptionMessageActual = e.Message;
            }

            Assert.AreEqual("The specified paramter must not be null, empty or only whitespace! (Parameter 'inputString')", exceptionMessageActual);
        }

        [TestMethod]
        public void ShouldThrowArgumentNullException_BecauseVisitorIsNull()
        {
            string exceptionMessageActual = "";

            try
            {
                Interpretor interpretor = new Interpretor("test", null);
            }
            catch (ArgumentNullException e)
            {
                exceptionMessageActual = e.Message;
            }

            Assert.AreEqual("The specified paramter must not be null! (Parameter 'inputVisitor')", exceptionMessageActual);
        }

        [DataTestMethod]
        [DataRow(">52+.@       ","7 ")]
        [DataRow(">52*.@       ", "10 ")]
        [DataRow(">52/.@       ", "2 ")]
        [DataRow(">52%.@       ", "7 ")]
        public void TestCalculationMethods(string input, string expectedOutput)
        {
            Interpretor interpretor = new Interpretor(input, this.visitor);
            interpretor.OnNewOutput += ReactOnNewOutput;
            interpretor.Run();

            Assert.AreEqual(expectedOutput, actual);
        }

        public void ReactOnNewOutput(object sender, OnOutpuEventArgs e)
        {
            actual += e.Output;
        }
    }
}
