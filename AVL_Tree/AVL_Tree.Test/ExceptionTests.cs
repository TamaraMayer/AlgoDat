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
            //View model zum testen anlegen mit einem kleinen baum

            this.vm = new AVL_VM();

            this.vm.root = new Node(7);
            this.vm.root.Right = new Node(8);
            this.vm.root.Right.Right = new Node(9);
            this.vm.root.Left = new Node(2);
            this.vm.root.Left.Left = new Node(1);
            this.vm.root.Left.Right = new Node(5);
            this.vm.root.Left.Right.Left = new Node(4);
            this.vm.root.Left.Right.Right = new Node(6);
        }


        [TestMethod]
        public void ShouldGetArgumentException_RecursiveInsert()
        {
            vm.InputField = 4;

            Assert.ThrowsException<ArgumentException>(()=>vm.RecursiveInsert(vm.root), "4 could not be inserted, it is already in the tree. No duplicates allowed!");
        }

        [TestMethod]
        public void ShouldGetArgumentException_Find()
        {
            Assert.ThrowsException<ArgumentException>(() => vm.Find(vm.root,34), "The given number is not inside the Tree!");
        }

        [TestMethod]
        public void ShouldGetArgumentException_FindParent()
        {
            Assert.ThrowsException<ArgumentException>(() => vm.FindParent(vm.root, 34), "The given number is not inside the Tree!");
        }

        [TestMethod]
        public void ShouldGetArgumentOutOfRangeException_FindParent()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => vm.FindParent(null, 34), "The specified parameter may not be null!");
        }

        [TestMethod]
        public void ShouldGetArgumentOutOfRangeException_GetLeftetstNode()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => vm.GetLeftestLeafNode(null), "The specified parameter may not be null!");
        }
        [TestMethod]
        public void ShouldGetArgumentOutOfRangeException_Remove()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => vm.Remove(null, 5), "The specified parameter may not be null!");
        }
        [TestMethod]
        public void ShouldGetArgumentOutOfRangeException_GetInorderSuccessor()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => vm.GetInOrderSuccessor(vm.root.Right.Right), "The specified parameter may not be null!");
        }
    }
}
