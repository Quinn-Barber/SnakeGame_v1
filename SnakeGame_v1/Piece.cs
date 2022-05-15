using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame_v1
{
    internal class Piece : Label
    {
        public Piece(int x, int y)
        {
            Location = new System.Drawing.Point(x, y);
            Size = new System.Drawing.Size(20, 20);
            BackColor = System.Drawing.Color.Orange;
            Enabled = false;
        }
    }
}
