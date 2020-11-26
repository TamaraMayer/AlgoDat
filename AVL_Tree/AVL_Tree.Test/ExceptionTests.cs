using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AVL_Tree.Test
{
    [TestClass]
   public class ExceptionTests
    {
        public AVL_VM vm;

        [TestInitialize]
        public void Setup()
        {
            this.vm = new AVL_VM();
            //this.vm.root = null;
        }


        [TestMethod]
        public void ShouldGetArgumentException_RecursiveInsert()
        {
            
        }
    }
}
