﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WMPLib;

namespace IO_projekt
{
    public partial class Form1 : Form
    {
        public static uint OPERATIONS;
        public volatile int FramesInCurrentSecond = 0;

        public Player p;
        public Timer MainTimer;
        Random Seed;
        Star[] StarArray;
        int StarCount;

        static int lowest = 0;
        static int[] scoreList = new int[11];
        
        public GamePanel xGamePanel;

        public MainMenuPanel MainMenu;
        ScoreEntry xScoreEntry;

        public WindowsMediaPlayer gameMedia;
        public WindowsMediaPlayer shootMedia;
        public WindowsMediaPlayer bonusMedia;

        public static int score = 0;
        public static int scoreMultiplier = 1;
        public static int scoreMultiplierTime;
        
        public static int hp = 100;
        public static int level = 1;
        public static int CurrentLevel = 1;

        public static bool Pause;
        static bool GameOver;
        public static bool BossLevel;

        public static int labelTopOffset;

        public Form1()
        {
            InitializeComponent();
            Lifelbl.Location = new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 120, 13);
            LifePointslbl.Location = new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 60, 13);
            LifePointslbl.Text = hp.ToString();

            xGamePanel = null;

            gameMedia = new WindowsMediaPlayer();
            shootMedia = new WindowsMediaPlayer();
            bonusMedia = new WindowsMediaPlayer();

            Directory.CreateDirectory("sound");

            File.WriteAllBytes(@"sound\game_music.wav", StreamToByteArr(Properties.Resources.game_music));
            File.WriteAllBytes(@"sound\laser.wav", StreamToByteArr(Properties.Resources.laser));
            File.WriteAllBytes(@"sound\bonus.wav", StreamToByteArr(Properties.Resources.bonus));

            gameMedia.URL = @"sound\game_music.wav";
            bonusMedia.URL = @"sound\bonus.wav";

            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 7;
            shootMedia.settings.volume = 4;
            bonusMedia.settings.volume = 4;

            gameMedia.controls.stop();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Cursor.Hide();
            this.Shown += Form1_shown;

            labelTopOffset = scoreMultiplierTimeLabel.Top;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OPERATIONS = 0;
            Pause = false;
            GameOver = false;
            BossLevel = false;

            scoreMultiplierTime = 0;

            Console.WriteLine(OPERATIONS++ + "> " + "Loading form...", OPERATIONS);
            
            MainTimer = new Timer();
            MainTimer.Interval = 10;
            MainTimer.Tick += new System.EventHandler(MainTimer_Tick);

            Seed = new Random();
            StarCount = 50;
            StarArray = new Star[StarCount];
            for (int i = 0; i < StarCount; i++)
            {
                StarArray[i] = null;
            }
        }      
        
        private void Form1_shown(object sender, EventArgs e)
        {
            this.MainMenu = new MainMenuPanel(this);          
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
                    p.OpenGunLock();
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
                p.CloseGunLock();
            }
            if (e.KeyCode == Keys.S)
            {
                xScoreEntry = new ScoreEntry(this);
            }
            if (e.KeyCode == Keys.I)
            {
                p.CurrentGun.ShotsInterval = 20;
            }
            if (e.KeyCode == Keys.O)
            {
                Conf.bonuses.Add(new Bonus(this, p));
            }
            if(e.KeyCode == Keys.R)
            {
                Conf.bullets.Add(new Rocket(this));
            }
            if (e.KeyCode == Keys.P)
            {
                this.NextLevel();
            }
            if (e.KeyCode == Keys.U)
            {
                Conf.enemies.Add(new EnemyDreadnought(this, this.p));
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (!GameOver)
                {
                    if (Pause)
                    {
                        MainTimer.Start();
                        pauseLabel.Visible = false;
                        Pause = false;                        
                    }
                    else
                    {
                        p.MoveRightStop();
                        p.MoveLeftStop();
                        p.MoveUpStop();
                        p.MoveDownStop();
                        p.CloseGunLock();
                        p.CloseGunLock();

                        new PauseMenuPanel(this);
                        MainTimer.Stop();
                        Pause = true;
                    }
                }                
            }
        }

        public void MainTimer_Tick(object sender, EventArgs e)
        {
            FramesInCurrentSecond++;
            p.TICK();

            for (int i = 0; i < StarCount; i++)
            {
                if (StarArray[i] == null)
                    StarArray[i] = new Star(this);

                if (StarArray[i].Sprite.Top > this.Height)
                {
                    StarArray[i].Dispose();
                    StarArray[i] = new Star(this);
                }

                StarArray[i].Move();
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

            foreach (Bonus bl in Conf.bonuses)
            {
                bl.TICK();
            }
            Conf.CollectBonuses();

            foreach (Bonus2 b in Conf.bonuses2)
            {
                b.TICK();
            }

            if (score > 50)
            {
                level = 4;
            }
            else if(score > 20)
            {
                level = 3;
            }
            else if(score > 10)
            {
                level = 2;
            }

            xGamePanel.TICK();

            Conf.DelayedAdd();
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
       
        public void showGameOver(string message)
        {
            GameOver = true;
            Pause = true;

            MainTimer.Stop();

            xScoreEntry = new ScoreEntry(this);

            if (Int32.Parse(Pointslbl.Text) > lowest)
            {
                ScoreView.Clear();
            }

            pauseLabel.Text = message;

            if(pauseLabel.Text == "You win")
            {
                pauseLabel.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 210, 100);
            }
            else
            {
                pauseLabel.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 285, 100);
            }            
                                         
            pauseLabel.Visible = true;

            p.MoveRightStop();
            p.MoveLeftStop();
            p.MoveUpStop();
            p.MoveDownStop();
            p.CloseGunLock();
        }               
    }
}