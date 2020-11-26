using Microsoft.VisualStudio.TestTools.UnitTesting;
using AVL_Tree;

namespace AVL_Tree_Test
{
    [TestClass]
    public class UnitTest1
    {
        public AVL_Tree.AVL_VM vm;

        [TestInitialize]
        public void Setup()
        {
            this.vm = new AVL_Tree.AVL_VM();
        }

        [TestMethod]
        public void Should_InsertSomeValues_usingInsertCommand()
        {

        }
    }
}
