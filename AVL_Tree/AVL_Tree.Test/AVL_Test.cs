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
        public void ShouldRemoveRootValue_WhenOnlyRootInTree()
        {
            vm.root = new Node(7);
            vm.InputField = 7;

            vm.RemoveCommand.Execute(null);

            Assert.IsNull(vm.root);
        }

        [TestMethod]
        public void ShouldRemoveRootValue_FromBiggerTree()
        {
            int[] values = new int[] { 3, 10, 11, 12, 13 };

            for (int i = 0; i < values.Length; i++)
            {
                vm.InputField = values[i];
                vm.RecurviseInsert(vm.root);
            }

            vm.InputField = 7;

            vm.RemoveCommand.Execute(null);

            Assert.IsNull(vm.root);
        }

        [DataTestMethod]
        [DataRow(7,10)]
        [DataRow(7,3)]
        public void ShouldRemoveRootValue_WhenRootHasOnlyOneChild(int root, int child)
        {
            vm.root = new Node(root);
            vm.InputField = child;
            vm.InsertCommand.Execute(null);

            vm.InputField = root;
            vm.RemoveCommand.Execute(null);

            //TODO not sure if are equal funkt
            Assert.AreEqual(new Node(child),vm.root);
        }

        [DataTestMethod]
        [DataRow (7, "1245689")]
        [DataRow(2,"1456789")]
        [DataRow(1,"2456789")]
        public void ShouldRemoveValue(int value, string expected)
        {
            vm.InputField = value;

            vm.RemoveCommand.Execute(null);

            vm.TraverseInOrder(vm.root);

            string actual = ConvertionForAssert.ConvertNodeListToString(vm.traversedList);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldFindRootValue()
        {
            Node expected = vm.root;

            Node actual = vm.Find(vm.root, 7);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldFindValueLeftOfRoot()
        {
            Node expected = vm.root.Left.Right.Left;

            Node actual = vm.Find(vm.root, 4);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldFindValueRightOfRoot()
        {
            Node expected = vm.root.Right.Right;

            Node actual = vm.Find(vm.root, 9);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldFindParentOfValue_LeftOfRoot()
        {
            Node expected = vm.root.Left.Right;

            Node actual = vm.FindParent(vm.root, 6);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldFindParentOfValue_RightOfRoot()
        {
            Node expected = vm.root.Right;

            Node actual = vm.FindParent(vm.root, 9);
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
        public void ShouldGetInOrderSuccessor_WithoutLeftChild()
        {
            Node expected = vm.root.Right;

            Node actual = vm.GetInOrderSuccessor(vm.root);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldGetInOrderSuccessor_WithTwoLeftChildren()
        {
            int[] values = new int[] { 3,10,11,12,13 };

            for (int i = 0; i < values.Length; i++)
            {
                vm.InputField = values[i];
                vm.RecurviseInsert(vm.root);
            }

            Node expected = vm.root.Right.Left.Left;

            Node actual = vm.GetInOrderSuccessor(vm.root);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChecksIfValueIsLeftOfParent_ShouldReturn_false()
        {
            bool expected = false;

            bool actual = vm.IsLeft(vm.root.Right, 9);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldClearTree()
        {
            vm.ClearCommand.Execute(null);

            Assert.AreEqual(null, vm.root);
        }

        [TestMethod]
        public void ShouldTraverseTreeInOrder()
        {
            string expected = "12456789";

            vm.TraverseInOrderCommand.Execute(null);

            string actual = ConvertionForAssert.ConvertNodeListToString(vm.traversedList);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldTraverseTreePreOrder()
        {
            string expected = "72154689";

            vm.TraversePreOrderCommand.Execute(null);

            string actual = ConvertionForAssert.ConvertNodeListToString(vm.traversedList);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldTraverseTreePostOrder()
        {
            string expected = "14652987";

            vm.TraversePostOrderCommand.Execute(null);

            string actual = ConvertionForAssert.ConvertNodeListToString(vm.traversedList);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldSetListForRendering()
        {
            List<NodeToRender> expected = new List<NodeToRender> { null,
           new NodeToRender(1,2), null,
           new NodeToRender(2,1), new NodeToRender(4,3),
           new NodeToRender(5,2), new NodeToRender(6,3),
           new NodeToRender(7,0), null,
           null,null,
           new NodeToRender(8,1), null,
           new NodeToRender(9,2),null};

            vm.SetListToDraw();

            bool actual = ConvertionForAssert.CompareNodesToRender(expected, vm.toDraw);

            Assert.IsTrue(actual);
        }
    }
}
