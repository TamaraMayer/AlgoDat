using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AVL_Tree.Test
{
    [TestClass]
    public class InsertTest
    {
        public AVL_VM vm;

        [TestInitialize]
        public void Setup()
        {
            this.vm = new AVL_VM();
            //this.vm.root = null;
        }

        [TestMethod]
        public void Should_InsertSomeValues_usingInsertCommand()
        {
            //inserts values into the tree, sometimes needs to rotate aswell
            //traverses in order starting at root, gets a string out of this tree and this string is compared with the expected Result

            string expectedResult = "12456789";
            int[] values = new int[] { 1, 2, 6, 7, 8, 9, 4, 5 };

            for (int i = 0; i < values.Length; i++)
            {
                vm.InputField = values[i];
                vm.InsertCommand.Execute(null);
            }

            vm.TraverseInOrder(vm.root);

            string actual = ConvertionForAssert.ConvertNodeListToString(vm.traversedList);

            Assert.AreEqual(expectedResult, actual);
        }
    }
}
