using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IO_projekt
{

    public partial class Form1 : Form
    {
        public static uint OPERATIONS;

        Player p;
        //Zmiana - Artur
        Timer MainTimer;
        Random Seed;
        Star[] StarArray;
        int StarCount;
        static int score = 0;

        static bool Pause;

        /*
        static void Main()
        {
            Application.Run(new Form1());
        }
        */
        public Form1()
        {
            InitializeComponent();
            //Adam - pełny ekran i schowanie kursora myszy
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Cursor.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OPERATIONS = 0;
            Pause = false;

            Console.WriteLine(OPERATIONS++ + "> " + "Loading form...", OPERATIONS);
            p = new Player(this, this.Width / 2, this.Height - 100);

            //Zmiana - Artur
            MainTimer = new Timer();
            MainTimer.Interval = 10;
            MainTimer.Start();
            MainTimer.Tick += new System.EventHandler(MainTimer_Tick);

            Seed = new Random();
            StarCount = 50;
            StarArray = new Star[StarCount];
            for (int i = 0; i < StarCount; i++)
            {
                StarArray[i] = new Star(this);
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
                if (e.KeyCode == Keys.Right)
                {
                if (!Pause)
                {
                    p.MoveRight();
                }
                }
                if (e.KeyCode == Keys.Left)
                {
                    if (!Pause)
                    {
                    p.MoveLeft();
                    }
                }
                if (e.KeyCode == Keys.Up)
                {
                    if (!Pause)
                    {
                        p.MoveUp();
                    }
                }
                if (e.KeyCode == Keys.Down)
                {
                    if (!Pause)
                    {
                        p.MoveDown();
                    }
                }
                if (e.KeyCode == Keys.Space)
                {
                    if (!Pause)
                    {
                        p.Shoot();
                    }
                }
                //Adam - wyjście z gry przez naciśnięcie klawisza esc
                if(e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                p.MoveRightStop();
            }
            if (e.KeyCode == Keys.Left)
            {
                p.MoveLeftStop();
            }
            if (e.KeyCode == Keys.Up)
            {
                p.MoveUpStop();
            }
            if (e.KeyCode == Keys.Down)
            {
                p.MoveDownStop();
            }

            if (e.KeyCode == Keys.Space)
            {
                p.ShootStop();
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (Pause)
                {
                    pauseLabel.Location = new Point(this.Width/2-147, 109);
                    MainTimer.Start();
                    pauseLabel.Visible = false;
                    Pause = false;
                }
                else
                {
                    pauseLabel.Visible = true;
                    MainTimer.Stop();
                    Pause = true;
                }
            }


        }

        //Zmiana - Artur
        public void MainTimer_Tick(object sender, EventArgs e)
        {

            for (int i = 0; i < StarCount; i++)
            {
                StarArray[i].Move();
                if (StarArray[i].Sprite.Top > this.Height)
                {
                    StarArray[i].Dispose();
                    StarArray[i] = new Star(this);
                }
            }
        }

        
    }
}