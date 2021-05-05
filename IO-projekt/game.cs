﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WMPLib;
using System.Drawing;
using System.Threading;
using System.Media;

namespace IO_projekt
{
    public static class Conf
    {
        public static List<Form1.Enemy> enemies = new List<Form1.Enemy>();
        public static List<Form1.Player.Bullet> bullets = new List<Form1.Player.Bullet>();
        public static List<Form1.EnemyBullet> enemyBullets = new List<Form1.EnemyBullet>();
        public static List<Form1.Bonus> bonuses = new List<Form1.Bonus>();

        public static void ClearAll()
        {
            enemies.Clear();
            bullets.Clear();
            enemyBullets.Clear();
            bonuses.Clear();
        }

        public static List<Form1.Enemy> EnemiesToRemove = new List<Form1.Enemy>();
        public static void CollectEnemies()
        {
            foreach (Form1.Enemy en in EnemiesToRemove)
            {
                enemies.Remove(en);
            }
        }

        public static List<Form1.Player.Bullet> BulletsToRemove = new List<Form1.Player.Bullet>();
        public static void CollectBullets()
        {
            foreach (Form1.Player.Bullet bl in BulletsToRemove)
            {
                bullets.Remove(bl);
            }
        }

        public static List<Form1.EnemyBullet> EnemyBulletsToRemove = new List<Form1.EnemyBullet>();
        public static void CollectEnemyBullets()
        {
            foreach (Form1.EnemyBullet bl in EnemyBulletsToRemove)
            {
                enemyBullets.Remove(bl);
            }
        }

        public static List<Form1.Bonus> BonusesToRemove = new List<Form1.Bonus>();
        public static void CollectBonuses()
        {
            foreach (Form1.Bonus bl in BonusesToRemove)
            {
                bonuses.Remove(bl);
            }
        }
    }

    public partial class Form1 : Form
    {
        public class Player
        {
            Form1 formHandle;

            public PictureBox Sprite;
            public Point Position;

            int MovementSpeed;

            bool IsGunLockOpen;
            int NeededGunCooldown = 500;
            int GunCooldown;

            public bool shielded = false;
            public bool Shielded
            {
                get { return shielded; }
            }

            System.Windows.Forms.Timer MoveRightTimer;
            System.Windows.Forms.Timer MoveLeftTimer;
            System.Windows.Forms.Timer MoveUpTimer;
            System.Windows.Forms.Timer MoveDownTimer;          

            public Player(Form1 f)
            {
                Console.WriteLine("Player() - f.Width: " + f.Width + "  f.Height: " + f.Height);

                formHandle = f;
                Sprite = new PictureBox();
                Sprite.Height = Sprite.Width = 50;
                Sprite.Image = Properties.Resources.player1;
                Sprite.BackColor = Color.Transparent;
                Position = new Point(formHandle.Width/2, formHandle.Height-this.Sprite.Height-50);
                Sprite.Location = Position;
                Console.WriteLine("Sprite.Location = " + "(" + Sprite.Location.X + "," + Sprite.Location.Y + ")");
                //f.GamePanel.Controls.Add(this.Sprite);

                MovementSpeed = 20;

                IsGunLockOpen = false;
                GunCooldown = 0;

                MoveRightTimer = new System.Windows.Forms.Timer();
                MoveRightTimer.Tick += new System.EventHandler(MoveRightTimer_Tick);
                MoveRightTimer.Interval = 10;

                MoveLeftTimer = new System.Windows.Forms.Timer();
                MoveLeftTimer.Tick += new System.EventHandler(MoveLeftTimer_Tick);
                MoveLeftTimer.Interval = 10;

                MoveUpTimer = new System.Windows.Forms.Timer();
                MoveUpTimer.Tick += new System.EventHandler(MoveUpTimer_Tick);
                MoveUpTimer.Interval = 10;

                MoveDownTimer = new System.Windows.Forms.Timer();
                MoveDownTimer.Tick += new System.EventHandler(MoveDownTimer_Tick);
                MoveDownTimer.Interval = 10;
            }

            public void TICK()
            {
                if (GunCooldown > 0)
                    GunCooldown -= 20;
                if (IsGunLockOpen)
                    this.Shoot();
            }

            public void OpenGunLock()
            {
                IsGunLockOpen = true;
            }
            public void CloseGunLock()
            {
                IsGunLockOpen = false;
            }

            public void xMoveLeft()
            {
                if (this.Sprite.Left > this.Sprite.Width)
                    this.Sprite.Left -= MovementSpeed;
            }
            public void xMoveRight()
            {
                if (this.Sprite.Left < this.formHandle.Width - this.Sprite.Width)
                    this.Sprite.Left += MovementSpeed;
            }
            public void xMoveUp()
            {
                if (this.Sprite.Top < this.Sprite.Height)
                    this.Sprite.Top -= MovementSpeed;
            }
            public void xMoveDown()
            {
                if (this.Sprite.Top > this.formHandle.Height - this.Sprite.Height)
                    this.Sprite.Top += MovementSpeed;
            }

            public void ResetGame()
            {                
                Sprite.Location = new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width/2, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 100);
                Sprite.Image = Properties.Resources.player1;
                shielded = false;

                foreach (Enemy en in Conf.enemies)
                {
                    en.Sprite.Top = -1000;
                    en.DistanceTravelled = en.MaxDistanceTravel;
                    formHandle.Controls.Remove(en.Sprite);                    
                }
                Conf.enemies.Clear();

                foreach (Bullet b in Conf.bullets)
                {
                    b.Sprite.Top = -1000;
                    b.DistanceTravelled = b.MaxDistanceTravelled;
                    formHandle.Controls.Remove(b.Sprite);
                }
                Conf.bullets.Clear();

                foreach (EnemyBullet b in Conf.enemyBullets)
                {
                    b.Sprite.Top = -1000;
                    b.DistanceTravelled = b.MaxDistanceTravelled;
                    formHandle.Controls.Remove(b.Sprite);
                }
                Conf.enemyBullets.Clear();

                foreach (Bonus b in Conf.bonuses)
                {
                    b.Sprite.Top = -1000;
                    b.DistanceTravelled = b.MaxDistanceTravel;
                    formHandle.Controls.Remove(b.Sprite);
                }
                Conf.bonuses.Clear();
            }            

            public class Bullet
            {
                public int DistanceTravelled;
                public int MaxDistanceTravelled;
                int BulletSpeed;
                System.Windows.Forms.Timer BulletTimer;
                public PictureBox Sprite;
                
                //zmiana - Artur
                Form1 formHandle;

                public Bullet(Form1 f, Player p)
                {
                    formHandle = f;
                    BulletSpeed = 10;
                    DistanceTravelled = 0;
                    MaxDistanceTravelled = f.Height + 200;
                    Sprite = new PictureBox();
                    Sprite.Width = Sprite.Height = 10;
                    Sprite.BackColor = Color.Yellow;
                    Sprite.Location = new Point(p.Sprite.Location.X + p.Sprite.Width / 2 - this.Sprite.Width / 2,
                        p.Sprite.Location.Y);

                    BulletTimer = new System.Windows.Forms.Timer();
                    BulletTimer.Interval = 20;
                    BulletTimer.Tick += new System.EventHandler(BulletTimer_Tick);

                    f.xGamePanel.Controls.Add(this.Sprite);

                    //BulletTimer.Start();

                }

                public void TICK()
                {
                    if (this.Sprite.Top > 0)
                    {
                        foreach (Enemy en in Conf.enemies)
                        {
                            if (this.Sprite.Bounds.IntersectsWith(en.Sprite.Bounds))
                            {
                                Conf.BulletsToRemove.Add(this);
                                this.Sprite.Dispose();

                                en.GetHit();
                            }
                        }

                        foreach (Bonus b in Conf.bonuses)
                        {
                            if (this.Sprite.Bounds.IntersectsWith(b.Sprite.Bounds))
                            {
                                Conf.BulletsToRemove.Add(this);
                                this.Sprite.Dispose();

                                b.GetHit();
                            }
                        }
                    }

                    if (DistanceTravelled < MaxDistanceTravelled)
                    {
                        DistanceTravelled += BulletSpeed;
                        this.Sprite.Top -= this.BulletSpeed;
                    }
                    else if (this.DistanceTravelled >= this.MaxDistanceTravelled)
                    {
                        formHandle.Controls.Remove(this.Sprite);
                        //Conf.bullets.Remove(this);
                        Conf.BulletsToRemove.Add(this);
                    }
                }
                
                public void BulletTimer_Tick(Object sender, EventArgs e)
                {
                    if (!Pause)
                    {
                        Enemy tmp = null;
                        if (this.Sprite.Top > 0)
                        {
                            foreach (Enemy en in Conf.enemies)
                            {
                                if (this.Sprite.Bounds.IntersectsWith(en.Sprite.Bounds))
                                {
                                    en.Sprite.Top = -1000;
                                    this.Sprite.Top = -1000;
                                    en.DistanceTravelled = en.MaxDistanceTravel;
                                    tmp = en;
                                    //Conf.bullets.Remove(this);
                                    Conf.BulletsToRemove.Add(this);
                                }
                            }
                        }

                        if (tmp != null)
                        {/*
                            if (!tmp.BonusHP && !tmp.BonusShield) // za zestrzelenie bonusów nie ma punktów
                            {
                                score = score + 1;
                                Console.WriteLine("score = " + score);
                                this.formHandle.Pointslbl.Text = Convert.ToString(score);
                            } */                           
                            Conf.enemies.Remove(tmp);
                        }

                        if (DistanceTravelled < MaxDistanceTravelled)
                        {
                            DistanceTravelled += BulletSpeed;
                            this.Sprite.Top -= this.BulletSpeed;
                        }
                        else if (this.DistanceTravelled >= this.MaxDistanceTravelled)
                        {
                            formHandle.Controls.Remove(this.Sprite);
                            //Conf.bullets.Remove(this);
                            Conf.BulletsToRemove.Add(this);
                        }                                             
                    }                    
                }                
            }

            public void HPCheck()
            {
                if(hp <= 0)
                {
                    Sprite.Top = -1000;

                    formHandle.showGameOver("Game Over");                    
                }
            }

            

            public void MoveRight()
            {
                if (shielded)
                {
                    Sprite.Image = Properties.Resources.player1_rtiltshield;
                }
                else
                {
                    Sprite.Image = Properties.Resources.player1_rtilt;
                }                
                MoveRightTimer.Start();
            }
            public void MoveRightStop()
            {
                if (shielded)
                {
                    Sprite.Image = Properties.Resources.player1shield;
                }
                else
                {
                    Sprite.Image = Properties.Resources.player1;
                }                    
                MoveRightTimer.Stop();
            }
            public void MoveLeft()
            {
                if(shielded)
                {
                    Sprite.Image = Properties.Resources.player1_ltiltshield;
                }
                else
                {
                    Sprite.Image = Properties.Resources.player1_ltilt;
                }                
                MoveLeftTimer.Start();
            }
            public void MoveLeftStop()
            {
                if(shielded)
                {
                    Sprite.Image = Properties.Resources.player1shield;
                }
                else
                {
                    Sprite.Image = Properties.Resources.player1;
                }                
                MoveLeftTimer.Stop();
            }
            public void MoveUp()
            {
                MoveUpTimer.Start();
            }
            public void MoveUpStop()
            {
                MoveUpTimer.Stop();
            }
            public void MoveDown()
            {
                MoveDownTimer.Start();
            }
            public void MoveDownStop()
            {
                MoveDownTimer.Stop();
            }
            public void Shoot()
            {

                if (IsGunLockOpen)
                {
                    if (GunCooldown <= 0)
                    {
                        GunCooldown = NeededGunCooldown;

                        using (SoundPlayer sp = new SoundPlayer(Properties.Resources.laser))
                        {
                            sp.Play();
                        }

                        Conf.bullets.Add(new Bullet(formHandle, this));
                    }
                }
            }

            private void MoveRightTimer_Tick(object sender, EventArgs e)
            {

                if (Sprite.Left < formHandle.Width - 80)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Ticking MoveRightTimer");
                    Sprite.Left += MovementSpeed;
                }
            }
            private void MoveLeftTimer_Tick(object sender, EventArgs e)
            {
                if (Sprite.Left > 15)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Ticking MoveLeftTimer");
                    Sprite.Left -= MovementSpeed;
                }
            }
            private void MoveUpTimer_Tick(object sender, EventArgs e)
            {
                if (Sprite.Top > 15)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Ticking MoveUpTimer");
                    Sprite.Top -= MovementSpeed;
                }
            }
            private void MoveDownTimer_Tick(object sender, EventArgs e)
            {
                if (Sprite.Top < formHandle.Height - 100)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Ticking MoveDownTimer");
                    Sprite.Top += MovementSpeed;
                }
            }           
        }



        //Zmiana - Artur
        public class Star : IDisposable
        {
            public PictureBox Sprite;
            int speed;
            Form1 formHandle;

            public Star(Form1 f)
            {
                Random Seed = new Random();
                this.formHandle = f;
                this.speed = Seed.Next(1, 7);


                this.Sprite = new PictureBox();
                this.Sprite.SizeMode = PictureBoxSizeMode.Zoom;

                this.Sprite.BackColor = Color.White;
                this.Sprite.Left = Seed.Next(0, this.formHandle.Width);
                this.Sprite.Width = this.Sprite.Height = Seed.Next(4, 10);
                this.Sprite.Top = -10;
                this.formHandle.xGamePanel.Controls.Add(this.Sprite);
            }

            public void Move()
            {
                this.Sprite.Top += this.speed;
            }

            public void Dispose()
            {
                this.formHandle.Controls.Remove(this.Sprite);
            }
        }


        public void SrvField()
        {
            StreamReader sr;
            StreamWriter sw;
            string path = @"ScoreFile.txt";

            ScoreView.Columns.Add("Positioncl");
            ScoreView.Columns.Add("Scorecl");
            ScoreView.Columns[0].Width = 155;
            ScoreView.Columns[1].Width = 155;

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
                Console.WriteLine("plik został utworzony");
                sw.Close();
            }

            if (Int32.Parse(Pointslbl.Text) > lowest)
            {

                scoreList[0] = Int32.Parse(Pointslbl.Text);

                for (int b = 0; b < 11; b++)
                {
                    for (int c = b + 1; c < 11; c++)
                    {
                        if (scoreList[c] == scoreList[b]) scoreList[c] = 0;
                    }
                }

                Array.Sort(scoreList);

                sw = new StreamWriter(path, false);
                Console.WriteLine("plik został otwarty");

                int i = 10;
                while (scoreList[i] != 0 && i > 0)
                {
                    sw.WriteLine(scoreList[i].ToString());
                    Console.WriteLine("dodano: " + scoreList[i].ToString());
                    i--;
                }
                sw.Close();
                Console.WriteLine("plik został zamkniety");

            }

            var fi = new FileInfo(path);

            sr = File.OpenText(path);
            if (fi.Length != 0)
            {
                for (int c = 0; c < 11; c++)
                {
                    scoreList[c] = 0;
                }
                lowest = 10000;
                string s;
                int i = 0;
                while ((s = sr.ReadLine()) != null && i < 10)
                {
                    scoreList[i] = Int32.Parse(s);
                    i++;
                }
            }
            sr.Close();

            Array.Reverse(scoreList);


            for (int i = 10; i > 0; i--)
            {
                if (scoreList[i] == 0) lowest = 0;
            }

            int k = 0;
            for (int n = 10; n > 0; n--)
            {

                if ((scoreList[n] < lowest && scoreList[n] != 0) && lowest != 0) lowest = scoreList[n];
                if (scoreList[n] > 0)
                {
                    ScoreView.Items.Add(new ListViewItem(new string[] { "               " + (k + 1).ToString() + ".", "      " + scoreList[n].ToString() })); Console.WriteLine("tab dod: " + scoreList[n].ToString());
                }
                else ScoreView.Items.Add(new ListViewItem(new string[] { "               " + (k + 1).ToString() + "." }));
                k++;
            }

        }
       
    }
}