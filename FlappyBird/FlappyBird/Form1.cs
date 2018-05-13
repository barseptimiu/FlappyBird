using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace FlappyBird
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
        }

        List<int> Pipe1 = new List<int>();
        List<int> Pipe2 = new List<int>();

        int PipeWidth = 55;
        int PipeDiffY = 160;
        int PipeDiffX = 280;

        bool start = true;
        bool running;

        int step = 3;
        int originalX, originalY;

        bool resetPipes = false;

        int points;

        bool inPipe = false;

        int score;


        private void Die()
        {
            running = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            button1.Visible = true;
            button1.Enabled = true;
            button1.BackgroundImage = FlappyBird.Properties.Resources.try_again;

            label1.Visible = false;
            pictureBox1.Visible = false;

            this.BackgroundImage = FlappyBird.Properties.Resources.game_over;

            ReadAndShowScore();

            points = 0;
            pictureBox1.Location = new Point(originalX, originalY);
            resetPipes = true;
            Pipe1.Clear();


        }


        private void ReadAndShowScore()
        {

            pictureBox2.Visible = true;
            pictureBox3.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            using (StreamReader reader = new StreamReader("Score.ini"))
            {
                score = int.Parse(reader.ReadToEnd());
                reader.Close();
            }

            if (score < int.Parse(label1.Text))
            {
                label2.Text = label1.Text;
                label3.Text = label1.Text;
                pictureBox4.Visible = true;
                MessageBox.Show("You died. :(");
                using (StreamWriter writer = new StreamWriter("Score.ini"))
                {
                    writer.Write(label1.Text);
                    writer.Close();
                }
            }

            if (score >= int.Parse(label1.Text))
            {
                label2.Text = label1.Text;
                label3.Text = score.ToString();
                MessageBox.Show("You died. :(");
            }

        }

        private void StartGame()
        {
            label1.Visible = true;
            pictureBox1.Visible = true;

            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            label2.Visible = false;
            label3.Visible = false;

            this.BackgroundImage = FlappyBird.Properties.Resources.flappy_bird_04_700x393;

            resetPipes = false;
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;

            Random random = new Random();

            int num = random.Next(40, this.Height - this.PipeDiffY);
            int num1 = num + this.PipeDiffY;

            Pipe1.Clear();
            Pipe1.Add(this.Width);
            Pipe1.Add(num);
            Pipe1.Add(this.Width);
            Pipe1.Add(num1);

            num = random.Next(40, this.Height - this.PipeDiffY);
            num1 = num + PipeDiffY;

            Pipe2.Clear();
            Pipe2.Add(this.Width + PipeDiffX);
            Pipe2.Add(num);
            Pipe2.Add(this.Width + PipeDiffX);
            Pipe2.Add(num1);

            button1.Visible = false;
            button1.Enabled = false;

            running = true;
            Focus();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Pipe1[0] + PipeWidth <= 0 | start == true)
            {
                Random rnd = new Random();
                int px = this.Width;
                int py = rnd.Next(40, this.Height - PipeDiffY);
                var p2x = px;
                var p2y = py + PipeDiffY;

                Pipe1.Clear();
                Pipe1.Add(px);
                Pipe1.Add(py);
                Pipe1.Add(p2x);
                Pipe1.Add(p2y);
            }
            else
            {
                Pipe1[0] = Pipe1[0] - 2;
                Pipe1[2] = Pipe1[2] - 2;
            }

            if (Pipe2[0] + PipeWidth <= 0)
            {
                Random rnd = new Random();
                int px = this.Width;
                int py = rnd.Next(40, this.Height - PipeDiffY);
                var p2x = px;
                var p2y = py + PipeDiffY;

                int[] p1 = { px, py, p2x, p2y };

                Pipe2.Clear();
                Pipe2.Add(px);
                Pipe2.Add(py);
                Pipe2.Add(p2x);
                Pipe2.Add(p2y);
            }
            else
            {
                Pipe2[0] = Pipe2[0] - 2;
                Pipe2[2] = Pipe2[2] - 2;
            }

            if (start == true)
            {
                start = false;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!resetPipes && Pipe1.Any() && Pipe2.Any())
            {
                //first top
                e.Graphics.FillRectangle(Brushes.DarkGreen, new Rectangle(Pipe1[0], 0, PipeWidth, Pipe1[1]));
                e.Graphics.FillRectangle(Brushes.DarkGreen, new Rectangle(Pipe1[0] - 10, Pipe1[3] - PipeDiffY, 75, 15));
                //first bottom
                e.Graphics.FillRectangle(Brushes.DarkGreen, new Rectangle(Pipe1[2], Pipe1[3], PipeWidth, this.Height - Pipe1[3]));
                e.Graphics.FillRectangle(Brushes.DarkGreen, new Rectangle(Pipe1[2] - 10, Pipe1[3], 75, 15));
                //second top
                e.Graphics.FillRectangle(Brushes.DarkGreen, new Rectangle(Pipe2[0], 0, PipeWidth, Pipe2[1]));
                e.Graphics.FillRectangle(Brushes.DarkGreen, new Rectangle(Pipe2[0] - 10, Pipe2[3] - PipeDiffY, 75, 15));
                //second bottom
                e.Graphics.FillRectangle(Brushes.DarkGreen, new Rectangle(Pipe2[2], Pipe2[3], PipeWidth, this.Height - Pipe2[3]));
                e.Graphics.FillRectangle(Brushes.DarkGreen, new Rectangle(Pipe2[2] - 10, Pipe2[3], 75, 15));
            }
        }

        private void CheckForPoint()
        {
            Rectangle bird = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(Pipe1[2] + 20, Pipe1[3] - PipeDiffY, 15, PipeDiffY);
            Rectangle rec2 = new Rectangle(Pipe2[2] + 20, Pipe2[3] - PipeDiffY, 15, PipeDiffY);
            Rectangle intersect1 = Rectangle.Intersect(bird, rec1);
            Rectangle intersect2 = Rectangle.Intersect(bird, rec2);

            if (!resetPipes | start)
            {
                if (intersect1 != Rectangle.Empty | intersect2 != Rectangle.Empty)
                {
                    if (!inPipe)
                    {
                        points++;
                        SoundPlayer sp = new SoundPlayer(FlappyBird.Properties.Resources.point);
                        sp.Play();
                        inPipe = true;
                    }
                }
                else
                {
                    inPipe = false;
                }
            }
        }

        private void CheckForCollision()
        {
            Rectangle bird = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(Pipe1[0], 0, PipeWidth, Pipe1[1]);
            Rectangle rec2 = new Rectangle(Pipe1[2], Pipe1[3], PipeWidth, this.Height - Pipe1[3]);
            Rectangle rec3 = new Rectangle(Pipe2[0], 0, PipeWidth, Pipe2[1]);
            Rectangle rec4 = new Rectangle(Pipe2[2], Pipe2[3], PipeWidth, this.Height - Pipe2[3]);
            Rectangle intersect1 = Rectangle.Intersect(bird, rec1);
            Rectangle intersect2 = Rectangle.Intersect(bird, rec2);
            Rectangle intersect3 = Rectangle.Intersect(bird, rec3);
            Rectangle intersect4 = Rectangle.Intersect(bird, rec4);

            if (!resetPipes | start)
            {
                if (intersect1 != Rectangle.Empty | intersect2 != Rectangle.Empty | intersect3 != Rectangle.Empty | intersect4 != Rectangle.Empty)
                {
                    SoundPlayer sp = new SoundPlayer(FlappyBird.Properties.Resources.collision);
                    sp.Play();
                    Die();
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Space:
                    step = -5;
                    pictureBox1.Image = FlappyBird.Properties.Resources.bird_straight;
                    break;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + step);
            if(pictureBox1.Location.Y < 0)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, 0);
            }

            if(pictureBox1.Location.Y + pictureBox1.Height > this.ClientSize.Height)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, this.ClientSize.Height - pictureBox1.Height);
            }
            CheckForCollision();
            if(running)
            {
                CheckForPoint();
            }

            label1.Text = Convert.ToString(points);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Space:
                    step = 3;
                    pictureBox1.Image = FlappyBird.Properties.Resources.bird_down;
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            originalX = pictureBox1.Location.X;
            originalY = pictureBox1.Location.Y;

            if (!File.Exists("Score.ini"))
            {
                File.Create("Score.ini").Dispose();
            }
        }
    }
}
