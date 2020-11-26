using Microsoft.VisualStudio.TestTools.UnitTesting;
using AVL_Tree;

namespace AVL_Tree_Test
{
    [TestClass]
    public class UnitTest1
    {
        public AVL_VM vm;

        [TestInitialize]
        public void Setup()
        {
            this.vm = new AVL_VM();
        }

        [TestMethod]
        public void Should_InsertSomeValues_usingInsertCommand()
        {
            int[] values = new int[] { 1, 2, 6, 7, 8, 9, 4, 5 };

            for(int i=0; i < values.Length; i++)
            {
                vm.InputField = i;
                vm.InsertCommand.Execute(null);
            }

            vm.TraverseInOrder();

        }
    }
}
