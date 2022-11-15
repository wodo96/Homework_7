using System;

namespace Homework_4
{

    public partial class Form1 : Form
    {
        bool firstRun = true;
        public Bitmap b1;
        public Bitmap b2;
        public Graphics g1;
        public Graphics g2;
        public Random random = new Random();
        public Pen penGreen = new Pen(Color.Green, 1);
        public Pen penRed = new Pen(Color.Red, 1);
        public Pen penPurple = new Pen(Color.Purple, 1);
        public Pen penOrange = new Pen(Color.Orange, 1);
        public int lambda=1;
        public int successPr = 50;

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.lambda = (int)this.numericUpDown3.Value;
        }


        private int fromYrealToYVirtual(double y, double minY, double maxY, int h)
        {
            if ((maxY - minY) == 0)
                return 0;

            return Convert.ToInt32(h - ((y - minY) * h) / (maxY - minY));
        }
        private int fromXrealToXVirtual(double x, double minX, double maxX, int w)
        {
            if ((maxX - minX) == 0)
            {
                return 0;
            }
            return Convert.ToInt32(((x - minX) * w) / (maxX - minX));
        }

        void enableBlocks()
        {
            this.textBox3.Visible = true;
            this.richTextBox1.BackColor = Color.White;
            this.richTextBox1.Visible = true;
            this.pictureBox1.BackColor = Color.LightBlue;
            this.textBox4.Visible = true;
            this.textBox5.Visible = true;
            this.textBox6.Visible = true;
            this.textBox7.Visible = true;
            this.pictureBox2.Visible = true;
            this.pictureBox3.Visible = true;
            this.pictureBox4.Visible = true;
            this.firstRun = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (firstRun)
            {
                enableBlocks();
            }

            this.richTextBox1.Text = "absolute\t\t|relative\t\t|normalized relative\n";
            this.b1 = new Bitmap(this.pictureBox1.Width, this.PreferredSize.Height);
            this.g1 = Graphics.FromImage(b1);

            this.b2 = new Bitmap(this.pictureBox5.Width, this.PreferredSize.Height);
            this.g2 = Graphics.FromImage(b2);

            int trialsCount = (int)this.numericUpDown1.Value;
            int numerOfTrajectories = (int)this.numericUpDown2.Value;

            this.lambda = (int)this.numericUpDown3.Value;

            this.successPr = (int)this.lambda / trialsCount;

            Dictionary<int, int> interrarivalDistribution = new Dictionary<int, int>();
            Dictionary<int, int> finals1 = new Dictionary<int, int>();
            Dictionary<int, int> finals2 = new Dictionary<int, int>();
            Dictionary<int, int> finals3 = new Dictionary<int, int>();

            int interrarival = 0;

            for (int i = 1; i <= numerOfTrajectories; i++)
            {
                List<Point> punti1 = new List<Point>();
                List<Point> punti2 = new List<Point>();
                List<Point> punti3 = new List<Point>();

                double Y = 0;
                for (int X = 1; X <= trialsCount; X++)
                {
                    int Uniform = random.Next(100);
                    if (Uniform <= successPr)
                    {
                        Y++;
                        if (interrarival != 0)
                        {
                            if (!interrarivalDistribution.ContainsKey(interrarival))
                            {
                                interrarivalDistribution.Add(interrarival, 1);
                            }
                            else
                            {
                                interrarivalDistribution[interrarival]++;
                            }
                            interrarival = 0;
                        }
                    }
                    else
                    {
                        interrarival++;
                    }

                    int xDevice = fromXrealToXVirtual(X, 0, trialsCount, pictureBox1.Width);
                    int yDevice = fromYrealToYVirtual(Y, 0, trialsCount, pictureBox1.Height);

                    int x2Device = fromXrealToXVirtual(X, 0, trialsCount, pictureBox1.Width);
                    int y2Device = fromYrealToYVirtual(Y * trialsCount / (X + 1), 0, trialsCount, pictureBox1.Height);

                    int x3Device = fromXrealToXVirtual(X, 0, trialsCount, pictureBox1.Width);
                    int y3Device = fromYrealToYVirtual(Y * (Math.Sqrt(trialsCount)) / (Math.Sqrt(X + 1)), 0, trialsCount * lambda, pictureBox1.Height);

                    punti1.Add(new Point(xDevice, yDevice));
                    punti2.Add(new Point(x2Device, y2Device));
                    punti3.Add(new Point(x3Device, y3Device));
                }
                Point p1 = punti1[trialsCount - 1];
                Point p2 = punti2[trialsCount - 1];
                Point p3 = punti3[trialsCount - 1];

                if (finals1.ContainsKey(p1.Y))
                {
                    finals1[p1.Y] += 1;
                }
                else
                {
                    finals1.Add(p1.Y, 1);
                }

                if (finals2.ContainsKey(p2.Y))
                {
                    finals2[p2.Y] += 1;
                }
                else
                {
                    finals2.Add(p2.Y, 1);
                }

                if (finals3.ContainsKey(p3.Y))
                {
                    finals3[p3.Y] += 1;
                }
                else
                {
                    finals3.Add(p3.Y, 1);
                }


                this.richTextBox1.AppendText(p1.Y.ToString() + "\t\t|"+ p2.Y.ToString() + "\t\t|" + p3.Y.ToString() + "\n");


                this.g1.DrawLines(penGreen, punti1.ToArray());
                this.g1.DrawLines(penRed, punti2.ToArray());
                this.g1.DrawLines(penPurple, punti3.ToArray());

            }

            lambdaHistogram(b2, g2, this.pictureBox5, interrarivalDistribution, trialsCount);
            this.pictureBox5.Image = b2;

            this.pictureBox1.Image = b1;
           
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.lambda = (int)this.numericUpDown3.Value;
        }

        private void lambdaHistogram(Bitmap bitmap, Graphics g, PictureBox pictureBox, Dictionary<int, int> distribution, int numElement)
        {
            int j = 0;
            int step = pictureBox.Width / distribution.Count;


            foreach (var item in distribution)
            {
                double virtualX = fromXrealToXVirtual(item.Value * 20, 0, numElement, pictureBox.Height);
                g.DrawRectangle(penOrange, j + 1, pictureBox.Height - (int)virtualX, step, (int)virtualX);
                j += step;
            }

            pictureBox.Image = bitmap;
        }
    }

}