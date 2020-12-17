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
            this.visitor = new Befunge.Test.InputVisitor();
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
        [DataRow(">50/.@       ", "0 ")]
        [DataRow(">52%.@       ", "1 ")]
        [DataRow(">52-.@       ", "3 ")]
        public void TestCalculationMethods(string input, string expectedOutput)
        {
            Interpretor interpretor = new Interpretor(input, this.visitor);
            interpretor.OnNewOutput += ReactOnNewOutput;
            interpretor.Run();

            Assert.AreEqual(expectedOutput, actual);
        }


        [DataTestMethod]
        [DataRow(">25`.@       ", "0 ")]
        [DataRow(">52`.@       ", "1 ")]
        [DataRow(">7!.@       ", "0 ")]
        [DataRow(">0!.@       ", "1 ")]
        [DataRow(">0    v\r\n @.\"@\"_\"@\",@  ", "@")]
        [DataRow(">1    v\r\n @.\"@\"_\"@\",@  ", "64 ")]
        [DataRow("v  > \"~\".@\r\n>0 |\r\n   > \"~\",@", "~")]
        [DataRow("v  > \"~\".@\r\n>1 |\r\n   > \"~\",@", "126 ")]
        [DataRow(">0\"@\"\\ ^\r\n     @._,@", "@")]
        [DataRow(">12$\"@\"\\ v\r\n       @._,@", "64 ")]
        [DataRow("v      @._,@\r\n>1#2\"@\"\\ v", "64 ")]
        [DataRow("<@,.:&","113 q")]
        [DataRow("> ~:.,.@", "84 T0 ")]
        [DataRow(">100g .@","62 ")]
        [DataRow(">\"eca\"81g\"pS\" ,,,,,, @","Sp ace")]
        [DataRow(">\"eca\"88*0g\"pS\" ,,,,,, @", "Sp ace")]
        [DataRow(">99*58*4+88+0p       @","Q")]
        [DataRow(">\"?gniog I ma erehW\" v\r\n @ ,,,,,,,,,,,,,,,,, ? ,,,,,,,,,,,,,,,,,@\r\n                     ,\r\n" +
            "                     ,\r\n                     ,\r\n                     ,\r\n                     ,\r\n" +
            "                     ,\r\n                     ,\r\n                     ,\r\n                     ,\r\n" +
            "                     ,\r\n                     ,\r\n                     ,\r\n                     ,\r\n" +
            "                     ,\r\n                     ,\r\n                     ,\r\n                     ,\r\n                     @", "Where am I going?")]
        public void TestVariousCodes(string input, string expectedOutput)
        {
            Interpretor interpretor = new Interpretor(input, this.visitor);
            interpretor.OnNewOutput += ReactOnNewOutput;
            interpretor.Run();

            Assert.AreEqual(expectedOutput, actual);
        }

        [TestMethod]
        public void TestInvalidCode()
        {
            string exceptionMessageActual = "";

            try
            {
                Interpretor interpretor = new Interpretor(">25`A.@ ", this.visitor);
                interpretor.OnNewOutput += ReactOnNewOutput;
                interpretor.Run();
            }
            catch (Exception e)
            {
                exceptionMessageActual = e.Message;
            }

            Assert.AreEqual("Came accross some invalid code! In line 0 at position 4", exceptionMessageActual);


        }

        public void ReactOnNewOutput(object sender, OnOutpuEventArgs e)
        {
            actual += e.Output;
        }
    }
}
