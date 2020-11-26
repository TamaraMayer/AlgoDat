using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AVL_Tree.Test
{
    [TestClass]
   public class AVL_Test
    {
        public AVL_VM vm;

        [TestInitialize]
        public void Setup()
        {
            //sets the vm to test, and also a small tree to test things on

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
        public void ShouldRemoveValue()
        {
            string expected = "1246789";

            vm.InputField = 5;

            vm.RemoveCommand.Execute(null);

            vm.TraverseInOrder(vm.root);

            string actual = ConvertionForAssert.ConvertNodeListToString(vm.traversedList);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldRemoveRootValue()
        {
            string expected = "1245689";

            vm.InputField = 7;

            vm.RemoveCommand.Execute(null);

            vm.TraverseInOrder(vm.root);

            string actual = ConvertionForAssert.ConvertNodeListToString(vm.traversedList);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        public void ShouldFindValue()
        {
            Node expected = vm.root;

            Node actual = vm.Find(vm.root,7);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldFindParentOfValue()
        {
            Node expected = vm.root.Left.Right;

            Node actual = vm.FindParent(vm.root, 6);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldFindParentOfRootEqualsNull()
        {
            Node expected = vm.root.Left.Right;

            Node actual = vm.FindParent(vm.root, 6);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldGetInOrderSuccessor()
        {
            Node expected = vm.root.Right;

            Node actual = vm.GetInOrderSuccessor(vm.root);

            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}
