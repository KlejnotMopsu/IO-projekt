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

        static int lowest = 0;
        static int[] scoreList = new int[11];
        

        Panel GamePanel;
        Panel PausePanel;

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

            this.PausePanel = new Panel();
            this.PausePanel.Width = this.Width;
            this.PausePanel.Height = this.Height;
            this.PausePanel.BackColor = Color.Red;
            this.Controls.Add(this.PausePanel);
            this.PausePanel.Visible = false;

            this.GamePanel = new Panel();
            this.Controls.Add(this.GamePanel);

           
            
            gameMedia = new WindowsMediaPlayer();
            shootMedia = new WindowsMediaPlayer();
            bonusMedia = new WindowsMediaPlayer();

            Directory.CreateDirectory("sound");

            File.WriteAllBytes(@"sound\space_music.wav", StreamToByteArr(Properties.Resources.space_music));
            File.WriteAllBytes(@"sound\laser.wav", StreamToByteArr(Properties.Resources.laser));
            File.WriteAllBytes(@"sound\bonus.wav", StreamToByteArr(Properties.Resources.bonus));

            gameMedia.URL = @"sound\space_music.wav";
            //shootMedia.URL = @"sound\laser.wav";
            bonusMedia.URL = @"sound\bonus.wav";


            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 7;
            shootMedia.settings.volume = 4;
            bonusMedia.settings.volume = 4;

            gameMedia.controls.play();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Cursor.Hide();
            this.Shown += Form1_shown;

            SrvField();
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
        
        private void Form1_shown(object sender, EventArgs e)
        {
            this.GamePanel.Width = this.Width;
            this.GamePanel.Height = this.Height;
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
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Form1 formHandle;
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
                        this.PausePanel.Visible = false;
                        //this.GamePanel.Visible = true;

                        MainTimer.Start();
                        Cursor.Hide();
                        pauseLabel.Visible = false;
                        Exitbtn.Visible = false;
                        //Playbtn.Visible = false;
                        //Scorebtn.Visible = false;
                        Pause = false;
                    }
                    else
                    {
                        //this.GamePanel.Visible = false;
                        this.PausePanel.Visible = true;

                        pauseLabel.Text = "Pause";
                        pauseLabel.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 170, 109);
                        //ScoreView.Visible = true;
                        //Playbtn.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 85, 240);
                        Exitbtn.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 85, 320);
                        //Scorebtn.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 85, 400);
                        Cursor.Show();
                        pauseLabel.Visible = true;
                        //Playbtn.Visible = true;
                        Exitbtn.Visible = true;
                        //Scorebtn.Visible = true;
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

            foreach (Enemy en in Conf.enemies)
            {
                en.TICK();
            }
            Conf.CollectEnemies();

            foreach(Player.Bullet bl in Conf.bullets)
            {
                bl.TICK();
            }
            Conf.CollectBullets();

            foreach (EnemyBullet bl in Conf.enemyBullets)
            {
                bl.TICK();
            }
            Conf.CollectEnemyBullets();

            int roll = Seed.Next(1, 101);
            //if (roll % 50 == 0)
            if(roll == 1)
            {
                EnemyStandard en = new EnemyStandard(this, this.p);
                Conf.enemies.Add(en);
            }
            if (roll == 2)
            {
                EnemyRifleman er = new EnemyRifleman(this, this.p);
                Conf.enemies.Add(er);
            }
        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
            Scorebtn.Visible = false;
            Replaybtn.Visible = false;
            this.ActiveControl = null;
            p.MoveRightStop();
            p.MoveLeftStop();
            p.MoveUpStop();
            p.MoveDownStop();
            p.ShootStop();
        }

        public static byte[] StreamToByteArr(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private void Scorebtn_Click(object sender, EventArgs e)
        {
            ScoreView.Size = new Size(327, 334);
            ScoreView.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 163, 250);
            ClScorebtn.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 85, 590);
            Scorebtn.Visible = false;
            Replaybtn.Visible = false;
            Exitbtn.Visible = false;
            //Playbtn.Visible = false;
            ScoreView.Visible = true;
            ClScorebtn.Visible = true;
        }

        private void ClScorebtn_Click(object sender, EventArgs e)
        {
            ScoreView.Visible = false;
            ClScorebtn.Visible = false;
            Replaybtn.Visible = true;
            //Playbtn.Visible = true;
            Scorebtn.Visible = true;
            Exitbtn.Visible = true;
        }

        private void Playbtn_Click(object sender, EventArgs e)
        {
            MainTimer.Start();
            Cursor.Hide();
            pauseLabel.Visible = false;
            Exitbtn.Visible = false;
            Scorebtn.Visible = false;
            Pause = false;
            Playbtn.Visible = false;
        }
    }
}