using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace IO_projekt
{
    public static class Conf
    {
        public static List<Form1.Enemy> enemies = new List<Form1.Enemy>();
        public static List<Form1.Player.Bullet> bullets = new List<Form1.Player.Bullet>();
        public static List<Form1.EnemyBullet> enemyBullets = new List<Form1.EnemyBullet>();
        public static List<Form1.Bonus> bonuses = new List<Form1.Bonus>();
        public static List<Bonus2> bonuses2 = new List<Bonus2>();

        public static List<object> DelayedAddList = new List<object>();
        public static void ClearAll()
        {
            enemies.Clear();
            bullets.Clear();
            enemyBullets.Clear();
            bonuses.Clear();
        }
        public static void ClearAndDisposeAll()
        {
            foreach (Form1.Enemy e in enemies)
            {
                e.Sprite.Dispose();
                Form1.EnemyRifleman er = e as Form1.EnemyRifleman;
                if (er != null)
                {                
                    er.EnemyShootTimer.Stop();
                    er.EnemyShootTimer.Dispose();
                }                   
            }
            enemies.Clear();

            foreach (Form1.Player.Bullet b in bullets)
            {
                b.Sprite.Dispose();
            }
            bullets.Clear();

            foreach (Form1.EnemyBullet b in enemyBullets)
            {
                b.Sprite.Dispose();
            }
            enemyBullets.Clear();

            foreach (Form1.Bonus b in bonuses)
            {
                b.Sprite.Dispose();
            }
            bonuses.Clear();

            foreach (Bonus2 b in bonuses2)
            {
                b.Sprite.Dispose();
            }
            bonuses2.Clear();
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

        public static void DelayedAdd()
        {
            foreach (object o in DelayedAddList)
            {
                Form1.EnemyExploder.Fragment ef = o as Form1.EnemyExploder.Fragment;
                if (ef != null)
                {
                    enemies.Add(ef);
                }
            }
            DelayedAddList.Clear();
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
            public int Credits;

            bool IsGunLockOpen;
            public int NeededGunCooldown = 250;
            int GunCooldown;

            int doubleShootTime;
            public int DoubleShootTime
            {
                get { return doubleShootTime;  }
                set { doubleShootTime = value; }
            }

            public bool shielded = false;
            public bool Shielded
            {
                get { return shielded; }
            }

            bool scatterGun;
            public bool ScatterGun
            {
                get { return scatterGun; }
                set { scatterGun = value; }
            }

            public int bulletSpeed;
            public bool bulletSpeedIncreased;

            System.Windows.Forms.Timer MoveRightTimer;
            System.Windows.Forms.Timer MoveLeftTimer;
            System.Windows.Forms.Timer MoveUpTimer;
            System.Windows.Forms.Timer MoveDownTimer;          

            public Player(Form1 f)
            {
                Console.WriteLine("Player() - f.Width: " + f.Width + "  f.Height: " + f.Height);

                Credits = 0;
                formHandle = f;
                Sprite = new PictureBox();
                Sprite.Height = Sprite.Width = 50;
                Sprite.Image = Properties.Resources.player1;
                Sprite.BackColor = Color.Transparent;
                Console.WriteLine("Sprite.Location = " + "(" + Sprite.Location.X + "," + Sprite.Location.Y + ")");

                MovementSpeed = 20;
                bulletSpeed = 15;
                bulletSpeedIncreased = false;

                IsGunLockOpen = false;
                GunCooldown = 0;

                doubleShootTime = 0;

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

                scatterGun = false;
            }

            public void TICK()
            {
                if (GunCooldown > 0)
                    GunCooldown -= 10;
                if (doubleShootTime > 0)
                    doubleShootTime -= 10;
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

            public class Bullet
            {
                public int DistanceTravelled;
                public int MaxDistanceTravelled;
                int BulletSpeed;
                int BulletSpeedHorizontal;
                public PictureBox Sprite;
                
                Form1 formHandle;

                public Bullet(Form1 f, Player p, int x, int y, int bs)
                {
                    formHandle = f;
                    BulletSpeed = bs;
                    BulletSpeedHorizontal = 0;
                    DistanceTravelled = 0;
                    MaxDistanceTravelled = f.Height + 200;
                    Sprite = new PictureBox();
                    Sprite.Width = Sprite.Height = 10;
                    Sprite.BackColor = Color.Transparent;
                    Sprite.Image = Properties.Resources.BulletPic;
                    Sprite.Location = new Point(x - this.Sprite.Width / 2, y);

                    f.xGamePanel.Controls.Add(this.Sprite);
                }

                public Bullet(Form1 f, Player p, int x, int y, String type, int bs)
                {
                    formHandle = f;
                    BulletSpeed = bs;
                                        
                    DistanceTravelled = 0;
                    MaxDistanceTravelled = f.Height + 200;
                    Sprite = new PictureBox();
                    Sprite.Width = Sprite.Height = 10;
                    Sprite.BackColor = Color.Transparent;                    
                    Sprite.Location = new Point(x - this.Sprite.Width / 2, y);

                    if (type == "left")
                    {
                        BulletSpeedHorizontal = -5;
                        Sprite.Image = Properties.Resources.BulletLeftIMG;
                    }
                    else
                    {
                        BulletSpeedHorizontal = 5;
                        Sprite.Image = Properties.Resources.BulletRightIMG;
                    }

                    f.xGamePanel.Controls.Add(this.Sprite);
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
                        this.Sprite.Location = new Point(this.Sprite.Left + BulletSpeedHorizontal, this.Sprite.Top - BulletSpeed);
                    }
                    else if (this.DistanceTravelled >= this.MaxDistanceTravelled)
                    {
                        formHandle.Controls.Remove(this.Sprite);
                        Conf.BulletsToRemove.Add(this);
                    }
                }                              
            }

            public void HPCheck()
            {
                if(hp <= 0)
                {
                    Sprite.Top = -1000;

                    formHandle.LifePointslbl.Text = Convert.ToString(0);

                    if (CurrentLevel == 2)
                    {
                        SecondArea sa = formHandle.xGamePanel as SecondArea;
                        if (sa.phase == 3)
                        {
                            sa.leftBoss.BossShootTimer.Stop();
                            sa.rightBoss.BossShootTimer.Stop();
                        }
                    }

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

                if (!Pause && IsGunLockOpen)
                {
                    if (GunCooldown <= 0)
                    {
                        GunCooldown = NeededGunCooldown;

                        using (SoundPlayer sp = new SoundPlayer(Properties.Resources.laser))
                        {
                            sp.Play();
                        }

                        if(doubleShootTime > 0)
                        {
                            Conf.bullets.Add(new Bullet(formHandle, this, this.Sprite.Location.X + this.Sprite.Width / 4, this.Sprite.Location.Y, bulletSpeed));
                            Conf.bullets.Add(new Bullet(formHandle, this, this.Sprite.Location.X + this.Sprite.Width / 2 + this.Sprite.Width / 4, this.Sprite.Location.Y, bulletSpeed));
                        }
                        else
                        {
                            Conf.bullets.Add(new Bullet(formHandle, this, this.Sprite.Location.X + this.Sprite.Width / 2, this.Sprite.Location.Y, bulletSpeed));
                        }

                        if(scatterGun)
                        {
                            Conf.bullets.Add(new Bullet(formHandle, this, this.Sprite.Location.X + this.Sprite.Width / 2, this.Sprite.Location.Y, "left", bulletSpeed));
                            Conf.bullets.Add(new Bullet(formHandle, this, this.Sprite.Location.X + this.Sprite.Width / 2, this.Sprite.Location.Y, "right", bulletSpeed));
                        }
                    }
                }
            }

            private void MoveRightTimer_Tick(object sender, EventArgs e)
            {

                if (!Pause && Sprite.Left < formHandle.Width - 80)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Ticking MoveRightTimer");
                    Sprite.Left += MovementSpeed;
                }
            }
            private void MoveLeftTimer_Tick(object sender, EventArgs e)
            {
                if (!Pause && Sprite.Left > 15)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Ticking MoveLeftTimer");
                    Sprite.Left -= MovementSpeed;
                }
            }
            private void MoveUpTimer_Tick(object sender, EventArgs e)
            {
                if (!Pause && Sprite.Top > 15)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Ticking MoveUpTimer");
                    Sprite.Top -= MovementSpeed;
                }
            }
            private void MoveDownTimer_Tick(object sender, EventArgs e)
            {
                if (!Pause && Sprite.Top < formHandle.Height - 100)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Ticking MoveDownTimer");
                    Sprite.Top += MovementSpeed;
                }
            }           
        }

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
    }
}