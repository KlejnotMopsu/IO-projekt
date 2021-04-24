using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    }

    public partial class Form1 : Form
    {
        public class Player
        {
            Form1 formHandle;

            public PictureBox Sprite;
            public Point Position;

            int MovementSpeed;

            public bool shielded = false;
            public bool Shielded
            {
                get { return shielded; }
            }

            System.Windows.Forms.Timer MoveRightTimer;
            System.Windows.Forms.Timer MoveLeftTimer;
            System.Windows.Forms.Timer MoveUpTimer;
            System.Windows.Forms.Timer MoveDownTimer;
            System.Windows.Forms.Timer ShootTimer;
            System.Windows.Forms.Timer EnemyTimer;            
            //System.Diagnostics.Stopwatch stopwatch;

            

            public Player(Form1 f, int x = 0, int y = 0)
            {
                Console.WriteLine("Player() - f.Width: " + f.Width + "  f.Height: " + f.Height);

                formHandle = f;
                Sprite = new PictureBox();
                Sprite.Height = Sprite.Width = 50;
                Sprite.Image = Properties.Resources.player1;
                Sprite.BackColor = Color.Transparent;
                Position = new Point(x, y);
                Sprite.Location = Position;
                Console.WriteLine("Sprite.Location = " + "(" + Sprite.Location.X + "," + Sprite.Location.Y + ")");
                f.Controls.Add(this.Sprite);

                MovementSpeed = 20;

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

                ShootTimer = new System.Windows.Forms.Timer();
                ShootTimer.Tick += new System.EventHandler(ShootTimer_Tick);
                ShootTimer.Interval = 200;

                EnemyTimer = new System.Windows.Forms.Timer();
                EnemyTimer.Tick += new System.EventHandler(EnemyTimer_Tick);
                EnemyTimer.Interval = 5000;
                EnemyTimer.Start();

                //stopwatch = new System.Diagnostics.Stopwatch();
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

                    f.Controls.Add(this.Sprite);

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
                    GameOver = true;
                    formHandle.pauseLabel.Text = "Game Over";
                    formHandle.pauseLabel.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 285, 100);
                    formHandle.Exitbtn.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 85, 270);
                    formHandle.Replaybtn.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 85, 370);
                    Cursor.Show();
                    formHandle.pauseLabel.Visible = true;
                    formHandle.Exitbtn.Visible = true;
                    formHandle.Replaybtn.Visible = true;
                    formHandle.MainTimer.Stop();
                    Pause = true;
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
                ShootTimer.Start();
            }
            public void ShootStop()
            {
                ShootTimer.Stop();
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
            private void ShootTimer_Tick(object sender, EventArgs e)
            {
                Console.WriteLine(OPERATIONS++ + "> " + "Bullet shot.");
                Bullet b = new Bullet(formHandle, this);
               // this.formHandle.shootMedia.controls.play();

                //Thread - aplikacja wywala po kilkunastu strzalach
                /*
                Thread th = new Thread(() => {
                    using (WindowsMediaPlayer laserWMP = new WindowsMediaPlayer())
                    {
                        laserWMP.URL = @"sound\laser.wav";
                        laserWMP.controls.play();
                    }
                });
                th.Start();
                */

                //Task<TResult> - dziala, ale buguje sie co kilkanascie strzalow
                /*
                var t = Task<int>.Run(() => {
                    WindowsMediaPlayer laserWMP = new WindowsMediaPlayer();
                    laserWMP.URL = @"sound\laser.wav";
                    laserWMP.controls.play();
                    return 0;
                });
                */

                //SoundPlayer - dziala, nie ma regulacji glosnosci
                
                using (SoundPlayer sp = new SoundPlayer(Properties.Resources.laser))
                {
                    sp.Play();
                }
                
                Conf.bullets.Add(b);
            }            

            private void EnemyTimer_Tick(object sender, EventArgs e)
            {
                if (!Pause)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Enemy spawned.");
                    Enemy enemy = new EnemyDreadnought(formHandle, this);
                    Conf.enemies.Add(enemy);
                }
            }
        }

        public abstract class Enemy
        {
            public int DistanceTravelled;
            public int EnemySpeed;
            public int MaxDistanceTravel;
            public System.Windows.Forms.Timer EnemyTimer;
            public PictureBox Sprite;
            public Random rand = new Random();
            public Player p;
            public Form1 formHandle;           

            public abstract void TICK();
            public abstract void GetHit();
        }

        public class EnemyStandard : Enemy
        {
            public bool bonusHP = false;
            public bool BonusHP
            {
                get { return bonusHP; }
            }

            public bool bonusShield = false;
            public bool BonusShield
            {
                get { return bonusShield; }
            }

            public EnemyStandard(Form1 f, Player p)
            {
                EnemySpeed = 4;
                DistanceTravelled = 0;
                MaxDistanceTravel = f.Height + 200;
                this.p = p;
                formHandle = f;

                Sprite = new PictureBox();

                int enemyType = rand.Next(10);
                if (enemyType == 0)
                {
                    Sprite.Image = Properties.Resources.bonusHP;
                    Sprite.Width = Sprite.Height = 30;
                    Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
                    bonusHP = true;
                }
                else if (enemyType == 1)
                {
                    Sprite.Image = Properties.Resources.bonusShield;
                    Sprite.Width = Sprite.Height = 30;
                    Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
                    bonusShield = true;
                }
                else
                {
                    Sprite.Image = Properties.Resources.enemy1;
                    Sprite.Width = Sprite.Height = 50;
                    Sprite.BackColor = Color.Transparent;
                    Sprite.SizeMode = PictureBoxSizeMode.Zoom;
                }

                Sprite.Location = new Point(rand.Next(100, f.Width - 100),
                    -100);

                EnemyTimer = new System.Windows.Forms.Timer();
                EnemyTimer.Interval = 20;
                EnemyTimer.Tick += new System.EventHandler(EnemyTimer_Tick);

                f.Controls.Add(this.Sprite);

                //EnemyTimer.Start();
            }

            public override void TICK()
            {
                if (this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
                {
                    this.Sprite.Dispose();
                    Conf.EnemiesToRemove.Add(this);
                    
                    //this.Sprite.Top = -1000;
                    DistanceTravelled = MaxDistanceTravel;

                    //Conf.enemies.Remove(this);
                    if (bonusHP)
                    {
                        hp += 10;
                        this.formHandle.bonusMedia.controls.play();
                    }
                    else if (bonusShield)
                    {
                        formHandle.p.shielded = true;
                        p.Sprite.Image = Properties.Resources.player1shield;
                        this.formHandle.bonusMedia.controls.play();
                    }
                    else
                    {
                        if (p.shielded)
                        {
                            p.shielded = false;
                            p.Sprite.Image = Properties.Resources.player1;
                        }
                        else
                        {
                            hp -= 10;
                        }
                    }
                    Console.WriteLine("hp = " + hp);
                    this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                    p.HPCheck();
                    this.EnemyTimer.Stop();
                }
                if (DistanceTravelled < MaxDistanceTravel)
                {
                    DistanceTravelled += EnemySpeed;
                    this.Sprite.Top += this.EnemySpeed;
                }
                if (this.Sprite.Top == System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
                {
                    if (!bonusHP && !bonusShield) 
                    {
                        hp -= 10;
                    }
                    Console.WriteLine("hp = " + hp);
                    this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                    this.EnemyTimer.Stop();
                    p.HPCheck();
                }
            }

            public override void GetHit()
            {
                this.Sprite.Dispose();
                Conf.EnemiesToRemove.Add(this);                
                if (!BonusHP && !BonusShield) 
                {
                    score++;
                    Console.WriteLine("score = " + score);
                    this.formHandle.Pointslbl.Text = Convert.ToString(score);
                } 
            }

            public void EnemyTimer_Tick(Object sender, EventArgs e)
            {
                if (!Pause)
                {
                    if (this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
                    {
                        this.Sprite.Top = -1000;
                        DistanceTravelled = MaxDistanceTravel;
                        formHandle.Controls.Remove(this.Sprite);
                        Conf.enemies.Remove(this);
                        if (bonusHP)
                        {
                            hp += 10;
                            this.formHandle.bonusMedia.controls.play();
                        }
                        else if (bonusShield)
                        {
                            p.shielded = true;
                            p.Sprite.Image = Properties.Resources.player1shield;
                            this.formHandle.bonusMedia.controls.play();
                        }
                        else
                        {
                            if (p.shielded)
                            {
                                p.shielded = false;
                                p.Sprite.Image = Properties.Resources.player1;
                            }
                            else
                            {
                                hp -= 10;
                            }
                        }
                        Console.WriteLine("hp = " + hp);
                        this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                        p.HPCheck();
                        this.EnemyTimer.Stop();
                    }
                    if (DistanceTravelled < MaxDistanceTravel)
                    {
                        DistanceTravelled += EnemySpeed;
                        this.Sprite.Top += this.EnemySpeed;
                    }
                    if (this.Sprite.Top == System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
                    {
                        if (!bonusHP && !bonusShield)
                        {
                            hp -= 10;
                        }
                        Console.WriteLine("hp = " + hp);
                        this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                        this.EnemyTimer.Stop();
                        p.HPCheck();
                    }
                }
            }
        }

        public class EnemyBullet
        {
            public int DistanceTravelled;
            public int MaxDistanceTravelled;
            int BulletSpeed;
            int BulletSpeedHorizontal;
            public PictureBox Sprite;            
            Form1 formHandle;
            Player p;

            public EnemyBullet(Form1 f, Player p, EnemyRifleman e)
            {                
                formHandle = f;
                this.p = p;
                BulletSpeed = 10;
                Random Seed = new Random();
                int direction = Seed.Next(2);
                if(direction == 0)
                {
                    BulletSpeedHorizontal = 10;
                }
                else
                {
                    BulletSpeedHorizontal = -10;
                }
                
                DistanceTravelled = 0;
                MaxDistanceTravelled = f.Height + 2000;
                Sprite = new PictureBox();
                Sprite.Width = Sprite.Height = 10;
                Sprite.BackColor = Color.Red;
                Sprite.Location = new Point(e.Sprite.Location.X + e.Sprite.Width / 2 - this.Sprite.Width / 2, e.Sprite.Location.Y);

                f.Controls.Add(this.Sprite);
            }

            public void TICK()
            {
                if (this.Sprite.Top > 0)
                {
                    if(this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
                    {
                        DistanceTravelled = MaxDistanceTravelled;
                        formHandle.Controls.Remove(this.Sprite);
                        Conf.EnemyBulletsToRemove.Add(this);
                        if (p.shielded)
                        {
                            p.shielded = false;
                            p.Sprite.Image = Properties.Resources.player1;
                        }
                        else
                        {
                            hp -= 10;
                        }
                        Console.WriteLine("hp = " + hp);
                        this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                        p.HPCheck();
                    }
                }

                if (DistanceTravelled < MaxDistanceTravelled)
                {
                    DistanceTravelled += BulletSpeed;
                    this.Sprite.Top += this.BulletSpeed;
                    this.Sprite.Left += this.BulletSpeedHorizontal;
                    if(this.Sprite.Left >= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width || this.Sprite.Left <= 0)
                    {
                        BulletSpeedHorizontal = -BulletSpeedHorizontal;
                    }
                }
                else if (this.DistanceTravelled >= this.MaxDistanceTravelled)
                {
                    formHandle.Controls.Remove(this.Sprite);
                    Conf.EnemyBulletsToRemove.Add(this);
                }
            }
        }

        public class EnemyRifleman : Enemy
        {
            System.Windows.Forms.Timer EnemyShootTimer;
            public EnemyRifleman(Form1 f, Player p)
            {
                EnemySpeed = 2;
                DistanceTravelled = 0;
                MaxDistanceTravel = f.Height + 200;
                this.p = p;
                formHandle = f;

                Sprite = new PictureBox();

                Sprite.Image = Properties.Resources.enemy3;
                Sprite.Width = Sprite.Height = 50;
                Sprite.BackColor = Color.Transparent;
                Sprite.SizeMode = PictureBoxSizeMode.Zoom;

                Sprite.Location = new Point(rand.Next(100, f.Width - 100), -100);
                
                EnemyShootTimer = new System.Windows.Forms.Timer();
                EnemyShootTimer.Tick += new System.EventHandler(EnemyShootTimer_Tick);
                EnemyShootTimer.Interval = 5000;
                EnemyShootTimer.Start();

                f.Controls.Add(this.Sprite);
            }

            public override void TICK()
            {
                if (this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
                {
                    this.Sprite.Top = -1000;
                    DistanceTravelled = MaxDistanceTravel;
                    formHandle.Controls.Remove(this.Sprite);
                    Conf.EnemiesToRemove.Add(this);
                    EnemyShootTimer.Stop();

                    if (p.shielded)
                    {
                        p.shielded = false;
                        p.Sprite.Image = Properties.Resources.player1;
                    }
                    else
                    {
                        hp -= 10;
                    }

                    Console.WriteLine("hp = " + hp);
                    this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                    p.HPCheck();
                }

                if (DistanceTravelled < MaxDistanceTravel)
                {
                    DistanceTravelled += EnemySpeed;
                    this.Sprite.Top += this.EnemySpeed;
                }

                if (this.Sprite.Top == System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
                {
                    hp -= 10;

                    Console.WriteLine("hp = " + hp);
                    this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                    p.HPCheck();
                    EnemyShootTimer.Stop();
                }
            }

            public override void GetHit()
            {
                EnemyShootTimer.Stop();
                this.Sprite.Dispose();
                Conf.EnemiesToRemove.Add(this);
                score++;
                Console.WriteLine("score = " + score);
                this.formHandle.Pointslbl.Text = Convert.ToString(score);
            }

            private void EnemyShootTimer_Tick(object sender, EventArgs e)
            {
                if(this.Sprite.Top > 0)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Enemy bullet shot.");
                    EnemyBullet b = new EnemyBullet(formHandle, p, this);

                    using (SoundPlayer sp = new SoundPlayer(Properties.Resources.laser))
                    {
                        sp.Play();
                    }

                    Conf.enemyBullets.Add(b);
                }                
            }
        }

        public class EnemyDreadnought : Enemy
        {
            public int HitPoints;

            public EnemyDreadnought(Form1 f, Player p)
            {
                EnemySpeed = 1;
                DistanceTravelled = 0;
                MaxDistanceTravel = f.Height + 200;
                this.p = p;
                formHandle = f;

                Sprite = new PictureBox();

                HitPoints = 4;

                
               Sprite.Image = Properties.Resources.enemy2;
               Sprite.Width = Sprite.Height = 50;
               Sprite.BackColor = Color.Transparent;
               Sprite.SizeMode = PictureBoxSizeMode.Zoom;
                
                Sprite.Location = new Point(rand.Next(100, f.Width - 100),
                    -100);


                f.Controls.Add(this.Sprite);
            }

            public override void TICK()
            {
                
                if (this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
                {
                    this.Sprite.Top = -1000;
                    DistanceTravelled = MaxDistanceTravel;
                    formHandle.Controls.Remove(this.Sprite);
                    Conf.EnemiesToRemove.Add(this);
                    //Conf.enemies.Remove(this);
                    
                    if (p.shielded)
                    {
                        p.shielded = false;
                        p.Sprite.Image = Properties.Resources.player1;
                    }
                    else
                    {
                        hp -= 30;
                    }
                    
                    Console.WriteLine("hp = " + hp);
                    this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                    p.HPCheck();
                    //this.EnemyTimer.Stop();
                }
                if (DistanceTravelled < MaxDistanceTravel)
                {
                    DistanceTravelled += EnemySpeed;
                    this.Sprite.Top += this.EnemySpeed;
                }
                if (this.Sprite.Top == System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
                {                  
                    hp -= 30;
                    
                    Console.WriteLine("hp = " + hp);
                    this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                    //this.EnemyTimer.Stop();
                    p.HPCheck();
                }
            }

            public override void GetHit()
            {
                this.HitPoints--;
                if (this.HitPoints <= 0)
                {
                    this.Sprite.Dispose();
                    Conf.EnemiesToRemove.Add(this);
                    score += 4;
                    Console.WriteLine("score = " + score);
                    this.formHandle.Pointslbl.Text = Convert.ToString(score);
                }               
                else if(this.HitPoints == 3)
                {
                    this.Sprite.Image = Properties.Resources.enemy2dmg1;
                }
                else if (this.HitPoints == 2)
                {
                    this.Sprite.Image = Properties.Resources.enemy2dmg2;
                }
                else if (this.HitPoints == 1)
                {
                    this.Sprite.Image = Properties.Resources.enemy2dmg3;
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
                this.formHandle.Controls.Add(this.Sprite);
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