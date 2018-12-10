using LightsOut;
using NUnit.Framework;
using System;

namespace LightsOutTests
{
    [TestFixture]
    public class GridTests
    {
        #region ConstructorTests
        [Test]
        public void ConstructorRowRangeExceptionLow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Grid(-1, 5, 5));
        }

        [Test]
        public void ConstructorRowRangeExceptionHigh()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Grid(21, 5, 5));
        }

        [Test]
        public void ConstructorColumnRangeExceptionLow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Grid(-5, 0, 5));
        }

        [Test]
        public void ConstructorColumnRangeExceptionHigh()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Grid(5, 25, 5));
        }

        [Test]
        public void ConstructorInitialCountRangeExceptionLow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Grid(5, 5, 0));
        }

        [Test]
        public void ConstructorInitialCountRangeExceptionHigh()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Grid(5, 5, 25));
        }

        [Test]
        public void ConstructorOk()
        {
            var g = new Grid(5, 5, 5);
            Assert.IsNotNull(g);
            Assert.AreEqual(g.Rows, 5, "unexpected value for Rows property");
            Assert.AreEqual(g.Columns, 5, "unexpected value for Columns property");
            Assert.AreEqual(g.CountLit, 5, "unexpected value for CountSet property");
            Assert.AreEqual(g.Complete, false, "unexpected value for Complete property");
        }

        #endregion ConstructorTests

        #region ToggleRangeTests
        [Test] 
        public void ToggleOutofRangeColumnLow()
        {
            var g = new Grid(5, 5, 5);
            Assert.IsNotNull(g);

            Assert.Throws<ArgumentOutOfRangeException>(() => g.ActivatePosition(-1, 2));
        }

        [Test]
        public void ToggleOutofRangeColumnHigh()
        {
            var g = new Grid(5, 5, 5);
            Assert.IsNotNull(g);

            Assert.Throws<ArgumentOutOfRangeException>(() => g.ActivatePosition(6, 2));
        }

        [Test]
        public void ToggleOutofRangeRowLow()
        {
            var g = new Grid(5, 5, 5);
            Assert.IsNotNull(g);

            Assert.Throws<ArgumentOutOfRangeException>(() => g.ActivatePosition(2,-1));
        }

        [Test]
        public void ToggleOutofRangeRowHigh()
        {
            var g = new Grid(5, 5, 5);
            Assert.IsNotNull(g);

            Assert.Throws<ArgumentOutOfRangeException>(() => g.ActivatePosition(2,10));
        }

        #endregion ToggleRangeTests

        #region IndexerRangeTests
        [Test]
        public void IndexerOutofRangeColumnLow()
        {
            var g = new Grid(5, 5, 5);
            bool b;
            Assert.IsNotNull(g);

            Assert.Throws<ArgumentOutOfRangeException>(() => b=g[-1, 2]);
        }

        [Test]
        public void IndexerOutofRangeColumnHigh()
        {
            var g = new Grid(5, 5, 5);
            bool b;
            Assert.IsNotNull(g);

            Assert.Throws<ArgumentOutOfRangeException>(() => b=g[6, 2]);
        }

        [Test]
        public void IndexerOutofRangeRowLow()
        {
            var g = new Grid(5, 5, 5);
            bool b;
            Assert.IsNotNull(g);

            Assert.Throws<ArgumentOutOfRangeException>(() => b=g[2, -1]);
        }

        [Test]
        public void IndexerOutofRangeRowHigh()
        {
            var g = new Grid(5, 5, 5);
            bool b;
            Assert.IsNotNull(g);

            Assert.Throws<ArgumentOutOfRangeException>(() => b=g[2, 10]);
        }

        #endregion IndexerRangeTests

        #region ToggleTests
        [Test]
        public void ToggleCentral()
        /* create a grid of 5,5 dimensions with no initial random population and activate a position in the centre of the grid.
         * Test that
         * 1) The state of the activated position is true
         * 2) The state of the four adjacent positions is true
         * 3) That all other positions are false
         * 4) That the count of set positions is 5
         * */
        {
            var expectedPositions = new int[,] { { 0, 2 }, { 1, 2 }, { 2, 2 }, { 1, 1 }, { 1, 3 } };

            var g = new Grid(5, 5);
            Assert.IsNotNull(g, "grid object has not been instantiated");

            g.ActivatePosition(1, 2);
            Assert.AreEqual(g.CountLit, expectedPositions.GetLength(0), "Unexpected value of CountSet property");

            for (int r = 0; r < 5; r++)
                for (int c = 0; c < 5; c++)
                    if (isInSet(expectedPositions, r, c))
                        Assert.AreEqual(g[r, c], true, String.Format("Position {0},{1} is expected to be true", r, c));
                    else
                        Assert.AreEqual(g[r, c], false, String.Format("Position {0},{1} is expected to be false", r, c));
                                 
        }

        [Test]
        public void ToggleTop()
        /* create a grid of 5,5 dimensions with no initial random population and activate a position in the 
         * top row of the grid.
         * Test that
         * 1) The state of the activated position is true
         * 2) The state of the three adjacent positions is true
         * 3) That all other positions are false
         * 4) That the count of set positions is 4
         * */
        {
            var expectedPositions = new int[,] { { 0, 2 }, { 1, 2 }, {0, 1}, { 0, 3 } };

            var g = new Grid(5, 5);
            Assert.IsNotNull(g, "grid object has not been instantiated");

            g.ActivatePosition(0, 2);
            Assert.AreEqual(g.CountLit, expectedPositions.GetLength(0), "Unexpected value of CountSet property");

            for (int r = 0; r < 5; r++)
                for (int c = 0; c < 5; c++)
                    if (isInSet(expectedPositions, r, c))
                        Assert.AreEqual(g[r, c], true, String.Format("Position {0},{1} is expected to be true", r, c));
                    else
                        Assert.AreEqual(g[r, c], false, String.Format("Position {0},{1} is expected to be false", r, c));

        }

        [Test]
        public void ToggleBottom()
        /* create a grid of 5,5 dimensions with no initial random population and activate a position in the 
         * bottom row of the grid.
         * Test that
         * 1) The state of the activated position is true
         * 2) The state of the three adjacent positions is true
         * 3) That all other positions are false
         * 4) That the count of set positions is 4
         * */
        {
            var expectedPositions = new int[,] { { 4, 2 }, { 3, 2 }, { 4, 1}, { 4, 3} };

            var g = new Grid(5, 5);
            Assert.IsNotNull(g, "grid object has not been instantiated");

            g.ActivatePosition(4,2);
            Assert.AreEqual(g.CountLit, expectedPositions.GetLength(0), "Unexpected value of CountSet property");

            for (int r = 0; r < 5; r++)
                for (int c = 0; c < 5; c++)
                    if (isInSet(expectedPositions, r, c))
                        Assert.AreEqual(g[r, c], true, String.Format("Position {0},{1} is expected to be true", r, c));
                    else
                        Assert.AreEqual(g[r, c], false, String.Format("Position {0},{1} is expected to be false", r, c));

        }
        [Test]
        public void ToggleLeft()
        /* create a grid of 5,5 dimensions with no initial random population and activate a position in the 
         * leftmost columnof the grid.
         * Test that
         * 1) The state of the activated position is true
         * 2) The state of the three adjacent positions is true
         * 3) That all other positions are false
         * 4) That the count of set positions is 4
         * */
        {
            var expectedPositions = new int[,] { { 2,0 }, { 3,0}, { 1,0 }, { 2,1} };

            var g = new Grid(5, 5);
            Assert.IsNotNull(g, "grid object has not been instantiated");

            g.ActivatePosition(2,0);
            Assert.AreEqual(g.CountLit, expectedPositions.GetLength(0), "Unexpected value of CountSet property");

            for (int r = 0; r < 5; r++)
                for (int c = 0; c < 5; c++)
                    if (isInSet(expectedPositions, r, c))
                        Assert.AreEqual(g[r, c], true, String.Format("Position {0},{1} is expected to be true", r, c));
                    else
                        Assert.AreEqual(g[r, c], false, String.Format("Position {0},{1} is expected to be false", r, c));

        }
        [Test]
        public void ToggleRight()
        /* create a grid of 5,5 dimensions with no initial random population and activate a position in the 
         * rightmost column of the grid.
         * Test that
         * 1) The state of the activated position is true
         * 2) The state of the three adjacent positions is true
         * 3) That all other positions are false
         * 4) That the count of set positions is 4
         * */
        {
            var expectedPositions = new int[,] { { 2,4 }, { 1,4 }, { 3,4 }, { 2,3 } };

            var g = new Grid(5, 5);
            Assert.IsNotNull(g, "grid object has not been instantiated");

            g.ActivatePosition(2,4);
            Assert.AreEqual(g.CountLit, expectedPositions.GetLength(0), "Unexpected value of CountSet property");

            for (int r = 0; r < 5; r++)
                for (int c = 0; c < 5; c++)
                    if (isInSet(expectedPositions, r, c))
                        Assert.AreEqual(g[r, c], true, String.Format("Position {0},{1} is expected to be true", r, c));
                    else
                        Assert.AreEqual(g[r, c], false, String.Format("Position {0},{1} is expected to be false", r, c));

        }

        #endregion ToggleTests

        private static bool isInSet(int[,] a, int row, int column)
        {
            for (var r = 0; r < a.GetLength(0); r++)
                if (a[r, 0] == row && a[r, 1] == column)
                    return true;

            return false;
        }
    }
}
