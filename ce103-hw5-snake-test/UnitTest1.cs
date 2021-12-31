using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ce103_hw5_snake_dll;

namespace ce103_hw5_snake_test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetScoreTestMethod1()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int expected = instance.getScore(25, 0);
            Assert.AreEqual(16, expected);
        }

        [TestMethod]
        public void GetScoreTestMethod2()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int expected = instance.getScore(26, 0);
            Assert.AreEqual(8, expected);
        }

        [TestMethod]
        public void GetScoreTestMethod3()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int expected = instance.getScore(5, 10);
            Assert.AreEqual(26, expected);
        }

        [TestMethod]
        public void IsCollidedWithselfOrBaitTestMethod1()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int[,] arr = new int[2, 100];
            arr[0, 0] = 10;
            arr[1, 0] = 20;
            instance.isCollidedWithselfOrBait(10, 20, arr, 4, 0);
            Assert.AreEqual(true, instance.isCollidedWithselfOrBait(10, 20, arr, 4, 0));
        }

        [TestMethod]
        public void IsCollidedWithselfOrBaitTestMethod2()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int[,] arr = new int[2, 100];
            arr[0, 0] = 10;
            arr[1, 0] = 20;
            instance.isCollidedWithselfOrBait(10, 20, arr, 4, 0);
            Assert.AreEqual(false, instance.isCollidedWithselfOrBait(10, 20, arr, 4, 1));
        }

        [TestMethod]
        public void IsCollidedWithselfOrBaitTestMethod3()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int[,] arr = new int[2, 100];
            arr[0, 0] = 10;
            arr[1, 0] = 19;
            instance.isCollidedWithselfOrBait(10, 20, arr, 4, 0);
            Assert.AreEqual(false, instance.isCollidedWithselfOrBait(10, 20, arr, 4, 0));
        }

        [TestMethod]
        public void CollisionTestMethod1()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int[,] arr = new int[2, 100];
            arr[0, 0] = 2;
            arr[1, 0] = 100;
            Assert.AreEqual(true, instance.collision(arr, 5));
        }

        [TestMethod]
        public void CollisionTestMethod2()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int[,] arr = new int[2, 100];
            arr[0, 0] = 3;
            arr[1, 0] = 21;
            Assert.AreEqual(true, instance.collision(arr, 2));
        }

        [TestMethod]
        public void CollisionTestMethod3()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int[,] arr = new int[2, 100];
            arr[0, 0] = 4;
            arr[1, 0] = 70;
            arr[0, 1] = 3;
            arr[1, 1] = 70;
            Assert.AreEqual(false, instance.collision(arr, 2));
        }

        [TestMethod]
        public void EatBaitTestMethod1()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int[,] arr = new int[2, 100];
            arr[0, 0] = 2;
            arr[1, 0] = 100;
            int[] arr2 = new int[2];
            arr2[0] = 2;
            arr2[1] = 100;
            Assert.AreEqual(true, instance.eatBait(arr, arr2));
        }

        [TestMethod]
        public void EatBaitTestMethod2()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int[,] arr = new int[2, 100];
            arr[0, 0] = 2;
            arr[1, 0] = 100;
            int[] arr2 = new int[2];
            arr2[0] = 2;
            arr2[1] = 99;
            Assert.AreEqual(false, instance.eatBait(arr, arr2));
        }

        [TestMethod]
        public void EatBaitTestMethod3()
        {
            SnakeGameFunctions instance = new SnakeGameFunctions();
            int[,] arr = new int[2, 100];
            arr[0, 0] = 2;
            arr[1, 0] = 100;
            int[] arr2 = new int[2];
            arr2[0] = 1;
            arr2[1] = 100;
            Assert.AreEqual(false, instance.eatBait(arr, arr2));
        }
    }
}
