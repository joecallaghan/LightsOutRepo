using LightsOut;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace LightsOutGame
{
    internal struct Position
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public partial class Form1 : Form
    {
        private int _Rows = 5;
        private int _Columns = 5;
        private int _InitialCount = 5;
        private List<CheckBox> _Checkboxes;

        private Grid _Grid;

        public Form1()
        {
            InitializeComponent();
            _Checkboxes = new List<CheckBox>();

            if (ConfigurationManager.AppSettings["Rows"] != null)
                int.TryParse(ConfigurationManager.AppSettings["Rows"], out _Rows);

            if (ConfigurationManager.AppSettings["Columns"] != null)
                int.TryParse(ConfigurationManager.AppSettings["Columns"], out _Columns);

            if (ConfigurationManager.AppSettings["InitialCount"] != null)
                int.TryParse(ConfigurationManager.AppSettings["InitialCount"], out _InitialCount);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            /* dynamically create checkboxes in a grid of the configured dimensions. Store the row 
             * and column index for each checkbox in the arbitrary data of the tag object and bind the 
             * checkbox to a click handler. */

            for (int r = 0; r < _Rows; r++)
            {
                for (int c = 0; c < _Columns; c++)
                {
                    var cb = new CheckBox();
                    cb.Top = 30 + (r * 30);
                    cb.Left = 5 + (c * 30);
                    cb.Width = 25;
                    cb.Height = 25;
                    cb.Appearance = Appearance.Button;
                    cb.Tag = new Position { Row = r, Column = c };
                
                    cb.Click += new EventHandler(checkBox_Click);
                    this.Controls.Add(cb);

                    _Checkboxes.Add(cb);
                    /* store the checkbox references in a List */
                }
            }
            this.Size = new Size(10 + (_Columns * 32), 60 + (_Rows * 32) );
            /*size the form based on the checkboxes plus appropriate margins */

            newGame();
            /* a game will be started automatically */
        }

        private void RefreshDisplayedGrid()
        {
            /* iterate each row/column position in the visible grid and get the current 
             * state of the corresponding position in the Grid object */

            for (var r =0; r<_Rows; r++)
            {
                for (var c = 0; c< _Columns; c++)
                {
                    var i = (r * _Columns) + c;
                     _Checkboxes[i].Checked = _Grid[r, c];
                    /* convert the row/column position into an offset which is used to
                     * get a reference to the checkbox so we can set its checked status */
                }
            }
        }

        private void checkBox_Click(object sender, EventArgs e)
        {
            /* a checkbox has been clicked....extract the row/column position from the
             * Tag object and call the method to update the Grid object state.
             * Refresh the visual grid and check to see if the last move completed the game */

            var pos = ((Position)((CheckBox)sender).Tag);
            _Grid.ActivatePosition(pos.Row, pos.Column);
            RefreshDisplayedGrid();
            if (_Grid.Complete)
            {
                foreach (var cb in _Checkboxes)
                    cb.Enabled = false;
              
                var dlgRes = MessageBox.Show(
                "You have completed the game. Click Menu->New Game to play again",
                "Game over",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            newGame();
        }

        private void newGame()
        {
            foreach (var cb in _Checkboxes)
                cb.Enabled = true;

            try
            {
                _Grid = new Grid(_Rows, _Columns, _InitialCount);
            }
            catch (ArgumentOutOfRangeException e)
            {
                var dlgRes = MessageBox.Show(
                String.Format("Review the value of '{0}' in the app.config file", e.ParamName),
                "Argument out of range",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

                Application.Exit();
                return;
            }
            RefreshDisplayedGrid();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
