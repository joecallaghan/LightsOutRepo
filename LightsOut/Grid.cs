using System;
using System.Collections.Generic;

namespace LightsOut
{
    ///<summary>Implements the behaviour of the LightsOut game</summary>
    public class Grid
    {
        /// <summary>Gets the number of rows in the game grid</summary>
        public int Rows { get; }

        /// <summary>Gets the number of columns in the game grid</summary>
        public int Columns { get; }

        /// <summary>Gets the number of positions within the game grid which are currently lit</summary>
        public int CountLit
        {
            get
            {
                int ct = 0;
                foreach (var b in _positions)
                    ct += (b ? 1 : 0);
                          
                return ct;
            }
        }

        /// <summary>Indicates that the game has been completed (ie that the count of lit positions is zero)</summary>
        public bool Complete
        {
            get
            {
                return CountLit == 0;
            }
        }

        /// <summary>
        /// An indexer to access grid game positions
        /// </summary>
        /// <param name="r">The index of the row (zero=top row)</param>
        /// <param name="c">The index of the column (zero=leftmost column></param>
        /// <returns>the status of the position (true=lit, false=unlit)</returns>
        public bool this[int r, int c]
        {
            get {
                if (r < 0 || r > (Rows - 1))
                    throw new ArgumentOutOfRangeException("row");

                if (c < 0 || c > (Columns - 1))
                    throw new ArgumentOutOfRangeException("column");

                return _positions[Offset(r,c)];
            }
        }

        private List<bool> _positions;
        /* a list of booleans which maintains the state (lit = true, unlit = false) of each position
         * within the row/column matrix. Its a single List (because its easier to iterate), so wherever 
         * a row/column pair is provided,this gets converted to an offset into this list */

        /// <summary>
        /// Constructor to  create an instance of Grid with the specified dimensions and with no 
        /// initially lit positions
        /// </summary>
        /// <remarks>Note that this constructor should not normally be called to instantiate a valid 
        /// game since it effectively creates a resolved game. It is supplied for testing purposes.</remarks>
       public Grid(int rows, int columns)
       {
            if (rows < 5 || rows > 20)
                throw new ArgumentOutOfRangeException("rows");

            if (columns < 5 || columns > 20)
                throw new ArgumentOutOfRangeException("columns");
            /* eacy dimension of the grid must be in the range 5-20 */

            Rows = rows;
            Columns = columns;

            _positions = new List<bool>();
            for (var i = 0; i < Rows * Columns; i++)
                _positions.Add(false);
            /* initialise all the positions to a false (or un-set) state */
        }

        /// <summary>
        /// Constructor to create a Grid instance with the specified dimensions and with the specified 
        /// number of randomly selected positions initially lit
        /// </summary>
        /// <param name="rows">The number of rows in the grid</param>
        /// <param name="columns">The number of columns in the grid</param>
        /// <param name="initialCount">The number of randomly selected positions which will be initially lit</param>
        public Grid(int rows, int columns, int initialCount):this(rows, columns)
        {
            if (initialCount < 1 || initialCount > (rows * columns) - 1)
                throw new ArgumentOutOfRangeException("initialCount");
            /* Where a number of randomly lit initial positions is supplied, it must be greater then
             * zero (since no initially lit positions would mean that the game was already solved) and
             * nor must it fill the grid */
   
            Random positionRndmzr = new Random();
            while (CountLit < initialCount)
            {
                var rnd = positionRndmzr.Next(0, _positions.Count);
                if (!_positions[rnd])
                    _positions[rnd] = true;
            }
            /* and then set grid positions randomly until the specified initialCount is met */
        }
        
        /// <summary>
        /// Toggle the state of the indicated row/column and any horizontally or vertically adjacent positions
        /// </summary>
        /// <param name="row">The row index of the position to toggle (zero = top row)</param>
        /// <param name="col">The column index of the position to toggle (zero = leftmost column)</param>
        public void ActivatePosition(int row, int col)
        {
            if (row < 0 || row > (Rows - 1))
                throw new ArgumentOutOfRangeException("row");

            if (col < 0 || col > (Columns - 1))
                throw new ArgumentOutOfRangeException("column");

            Toggle(row, col);

            /* toggle the state of the indicated row/column and then attempt to toggle the state of 
             * any vertically or horizontally adjacent position. There could be up to 4, but this will
             * depend on whether the indicated row/column is in an 'edge' column or row */

            if (col > 0)
                Toggle(row, col - 1);

            if (col < Columns-1)
                Toggle(row, col + 1);

            if (row > 0)
                Toggle(row - 1, col);

            if (row < Rows-1)
                Toggle(row + 1, col);
        }

        private void Toggle(int row, int col)
        /* invert the state of a row/column position. It's private, because its not a valid operation
         * within the terms of the game! */
        {
            var index = Offset(row, col);
            _positions[index] = !_positions[index];
        }

        private int Offset(int r, int c)
        /* turn a row/column pairing into an offset into the List*/
        {
            return (r * Columns) + c;
        }
    }
}
