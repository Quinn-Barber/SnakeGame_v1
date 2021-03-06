using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame_v1
{
    public partial class Background : Form
    {
        int cols = 27, rows = 27, score = 0, dx = 0, dy = 0, front = 0, back = 0;
        Piece[] snake = new Piece[1250];
        List<int> available = new List<int>();
        bool[,] visit;

        Random rand = new Random();

        Timer timer = new Timer();

        KeyEventArgs prev;

        public Background()
        {
            InitializeComponent();
            initial();
            launchTimer();
        }

        private void launchTimer()
        {
            timer.Interval = 75;
            timer.Tick += move;
            timer.Start();

        }

        private void move(object sender, EventArgs e)
        {
            int x = snake[front].Location.X, y = snake[front].Location.Y;
            if( dx == 0 && dy == 0)
            {
                return;
            }
            if( game_over(x + dx, y + dy))
            {
                timer.Stop();
                MessageBox.Show("Game Over");
                return;
            }
            if( collisionFood(x+dx, y + dy))
            {
                score += 1;
                Score.Text = "Score: " + score.ToString();
                if( hits( (y+dy)/20, (x+dx)/20) )
                {
                    return;
                }
                Piece head = new Piece(x + dx, y + dy);
                front = (front - 1 + 1250) % 1250;
                snake[front] = head;
                visit[head.Location.Y / 20, head.Location.X / 20] = true;
                Controls.Add(head);
                randomFood();
            }
            else
            {
                if( hits( (y+dy)/20, (x+dx)/20 ) )
                {
                    return;
                }
                visit[snake[back].Location.Y / 20, snake[back].Location.X / 20] = false;
                front = (front - 1 + 1250) % 1250;
                snake[front] = snake[back];
                snake[front].Location = new Point(x + dx, y + dy);
                back = (back - 1 + 1250) % 1250;
                visit[(y + dy) / 20, (x + dx) / 20] = true;
            }
        }

        private void randomFood()
        {
            available.Clear();
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    if (!visit[i, j])
                    {
                        available.Add(i * cols + j);
                    }
                }
            }
            int idx = rand.Next(available.Count) % available.Count;
            int numLeft = ((available[idx] * 20) % Width);
            int numTop = ((available[idx] * 20) / Width * 20);

            if (numLeft < 20) numLeft += 20;
            else if (numLeft > 500) numLeft -= 20;
            else if (numTop < 20) numTop += 20;
            else if (numTop > 500) numTop -= 20;

            Food.Left = numLeft;
            Food.Top = numTop;

        }

        private bool hits(int x, int y)
        {
            if (visit[x, y])
            {
                timer.Stop();
                MessageBox.Show("Oops! You hit your own body!");
                return true;
            }
            return false;
        }

        private void Background_KeyDown(object sender, KeyEventArgs e)
        {
            if (prev == null) prev = e;
            Boolean change = false;
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (prev.KeyCode == Keys.Left) break;
                    dx = dy = 0;
                    dx = 20;
                    change = true;
                    break;
                case Keys.Left:
                    if (prev.KeyCode == Keys.Right) break;
                    dx = dy = 0;
                    dx = -20;
                    change = true;
                    break;
                case Keys.Up:
                    if (prev.KeyCode == Keys.Down) break;
                    dx = dy = 0;
                    dy = -20;
                    change = true;
                    break;
                case Keys.Down:
                    if (prev.KeyCode == Keys.Up) break;
                    dx = dy = 0;
                    dy = 20;
                    change = true;
                    break;

            }
            if (change) prev = e;
        }

        private bool collisionFood(int x, int y)
        {
            return x == Food.Location.X && y == Food.Location.Y;
        }

        private bool game_over(int x, int y)
        {
            return x < 20 || y < 20 || x > 500 || y > 500;
        }

        private void initial()
        {
            visit = new bool[rows, cols];
            Piece head 
                = new Piece(260, 260);
            Food.Location
                = new Point(260, 200);
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    visit[i, j] = false;
                    available.Add(i * cols + j);
                }
            }

            visit[head.Location.Y / 20, head.Location.X / 20] = true;
            available.Remove(head.Location.Y / 20 * cols + head.Location.X / 20);
            Controls.Add(head);
            snake[front] = head;
        }
    }
}
