using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

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

        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootMedia;
        WindowsMediaPlayer bonusMedia;

        static int score = 0;
        static int hp = 100;


        static bool Pause;
        static bool GameOver;

        public Form1()
        {
            InitializeComponent();
            Lifelbl.Location = new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 120, 13);
            LifePointslbl.Location = new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 60, 13);
            LifePointslbl.Text = hp.ToString();

            gameMedia = new WindowsMediaPlayer();
            shootMedia = new WindowsMediaPlayer();
            bonusMedia = new WindowsMediaPlayer();
            gameMedia.URL = @"sounds\\space_music.wav";
            shootMedia.URL = @"sounds\\laser.wav";
            bonusMedia.URL = @"sounds\\bonus.wav";

            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 7;
            shootMedia.settings.volume = 4;
            bonusMedia.settings.volume = 4;

            gameMedia.controls.play();
            //Adam - pełny ekran i schowanie kursora myszy
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Cursor.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OPERATIONS = 0;
            Pause = false;
            GameOver = false;

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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
                Console.WriteLine("cos kliknelo...");
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
                if (!GameOver)
                {
                    if (Pause)
                    {
                        MainTimer.Start();
                        Cursor.Hide();
                        pauseLabel.Visible = false;
                        Exitbtn.Visible = false;
                        Pause = false;
                    }
                    else
                    {
                        pauseLabel.Text = "Pause";
                        pauseLabel.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 170, 109);
                        Exitbtn.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 85, 270);
                        Cursor.Show();
                        pauseLabel.Visible = true;
                        Exitbtn.Visible = true;
                        MainTimer.Stop();
                        Pause = true;
                    }
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

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Adam - przycisk replay
        private void Replaybtn_Click(object sender, EventArgs e)
        {
            score = 0;
            Pointslbl.Text = Convert.ToString(score);
            hp = 100;
            LifePointslbl.Text = Convert.ToString(hp);
            Pause = false;
            GameOver = false;
            p.ResetGame();
            MainTimer.Start();
            Cursor.Hide();
            pauseLabel.Visible = false;
            Exitbtn.Visible = false;
            Replaybtn.Visible = false;
            this.ActiveControl = null;
            p.MoveRightStop();
            p.MoveLeftStop();
            p.MoveUpStop();
            p.MoveDownStop();
            p.ShootStop();
        }
    }
}