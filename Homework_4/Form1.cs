using System;

namespace Homework_4
{

    public partial class Form1 : Form
    {
        bool firstRun = true;
        public Bitmap b;
        public Graphics g;
        public Random random = new Random();
        public Pen penGreen = new Pen(Color.Green, 1);
        public Pen penRed = new Pen(Color.Red, 1);
        public Pen penPurple = new Pen(Color.Purple, 1);
        public double successPr=0.5;

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.successPr = (double)this.numericUpDown3.Value;
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
            this.b = new Bitmap(this.pictureBox1.Width, this.PreferredSize.Height);
            this.g = Graphics.FromImage(b);


            int trialsCount = (int)this.numericUpDown1.Value;
            int numerOfTrajectories = (int)this.numericUpDown2.Value;



            for (int i = 1; i <= numerOfTrajectories; i++)
            {
                List<Point> punti1 = new List<Point>();
                List<Point> punti2 = new List<Point>();
                List<Point> punti3 = new List<Point>();

                double Y = 0;
                for (int X = 1; X <= trialsCount; X++)
                {
                    double Uniform = random.NextDouble();
                    if (Uniform < successPr)
                        Y = Y + 1;
                    int xDevice = fromXrealToXVirtual(X, 0, trialsCount, pictureBox1.Width);
                    int yDevice = fromYrealToYVirtual(Y, 0, trialsCount, pictureBox1.Height);

                    int x2Device = fromXrealToXVirtual(X, 0, trialsCount, pictureBox1.Width);
                    int y2Device = fromYrealToYVirtual(Y * trialsCount / (X + 1), 0, trialsCount, pictureBox1.Height);

                    int x3Device = fromXrealToXVirtual(X, 0, trialsCount, pictureBox1.Width);
                    int y3Device = fromYrealToYVirtual(Y * (Math.Sqrt(trialsCount)) / (Math.Sqrt(X + 1)), 0, trialsCount * successPr, pictureBox1.Height);

                    punti1.Add(new Point(xDevice, yDevice));
                    punti2.Add(new Point(x2Device, y2Device));
                    punti3.Add(new Point(x3Device, y3Device));
                }
                Point p1 = punti1[trialsCount - 1];
                Point p2 = punti2[trialsCount - 1];
                Point p3 = punti3[trialsCount - 1];

                this.richTextBox1.AppendText(p1.Y.ToString() + "\t\t|"+ p2.Y.ToString() + "\t\t|" + p3.Y.ToString() + "\n");


                this.g.DrawLines(penGreen, punti1.ToArray());
                this.g.DrawLines(penRed, punti2.ToArray());
                this.g.DrawLines(penPurple, punti3.ToArray());

            }
            this.pictureBox1.Image = b;
           
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.successPr = (double)this.numericUpDown3.Value;
        }
    }

}