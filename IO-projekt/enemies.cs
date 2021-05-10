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
    public partial class Form1 : Form
    {
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
            public bool alreadyShot;

            public abstract void TICK();
            public abstract void GetHit();
        }

        public class Bonus : Enemy
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

            public bool bonusDoubleShoot = false;
            public bool BonusDoubleShoot
            {
                get { return bonusDoubleShoot; }
            }

            public bool bonusMultiplier = false;
            public bool BonusMultiplier
            {
                get { return bonusMultiplier; }
            }

            public Bonus(Form1 f, Player p)
            {
                EnemySpeed = 5;
                DistanceTravelled = 0;
                MaxDistanceTravel = f.Height + 200;
                this.p = p;
                formHandle = f;

                Sprite = new PictureBox();

                int enemyType = rand.Next(4);
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
                else if(enemyType == 2)
                {
                    Sprite.Image = Properties.Resources.bonusDoubleShoot;
                    Sprite.Width = Sprite.Height = 30;
                    Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
                    bonusDoubleShoot = true;
                }
                else if(enemyType == 3)
                {
                    Sprite.Image = Properties.Resources.bonusMultiplier;
                    Sprite.Width = Sprite.Height = 30;
                    Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
                    bonusMultiplier = true;                                        
                }

                Sprite.Location = new Point(rand.Next(100, f.Width - 100), -100);

                f.xGamePanel.Controls.Add(this.Sprite);
            }

            public override void TICK()
            {
                if (this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
                {
                    this.Sprite.Dispose();
                    Conf.BonusesToRemove.Add(this);
                    DistanceTravelled = MaxDistanceTravel;
                    this.formHandle.bonusMedia.controls.play();

                    if (bonusHP)
                    {
                        hp += 10;
                        this.formHandle.LifePointslbl.Text = Convert.ToString(hp);                        
                    }
                    else if (bonusShield)
                    {
                        p.shielded = true;
                        p.Sprite.Image = Properties.Resources.player1shield;
                    }
                    else if(bonusDoubleShoot)
                    {
                        if(p.DoubleShootTime <= 0)
                        {
                            formHandle.doubleShootTimeLabel.Top = labelTopOffset;
                            if (formHandle.scoreMultiplierTimeLabel.Visible)
                            {
                                formHandle.doubleShootTimeLabel.Top += formHandle.scoreMultiplierTimeLabel.Height;
                            }
                            formHandle.doubleShootTimeLabel.Visible = true;
                        }
                        p.DoubleShootTime = 10000;                        
                    }
                    else if(bonusMultiplier)
                    {
                        if(scoreMultiplierTime <= 0)
                        {
                            formHandle.scoreMultiplierTimeLabel.Top = labelTopOffset;
                            if (formHandle.doubleShootTimeLabel.Visible)
                            {
                                formHandle.scoreMultiplierTimeLabel.Top += formHandle.doubleShootTimeLabel.Height;
                            }
                            formHandle.scoreMultiplierTimeLabel.Visible = true;
                        }
                        scoreMultiplier = 2;
                        scoreMultiplierTime = 10000;                        
                    }
                }
                if (DistanceTravelled < MaxDistanceTravel)
                {
                    DistanceTravelled += EnemySpeed;
                    this.Sprite.Top += this.EnemySpeed;
                }
                if (this.Sprite.Top >= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
                {
                    this.Sprite.Dispose();
                    Conf.BonusesToRemove.Add(this);
                    DistanceTravelled = MaxDistanceTravel;
                }
            }

            public override void GetHit()
            {
                this.Sprite.Dispose();
                Conf.BonusesToRemove.Add(this);
            }            
        }

        public class EnemyStandard : Enemy
        {
            public EnemyStandard(Form1 f, Player p)
            {
                EnemySpeed = 4;
                DistanceTravelled = 0;
                MaxDistanceTravel = f.Height + 200;
                this.p = p;
                formHandle = f;
                alreadyShot = false;

                Sprite = new PictureBox();
                Sprite.Image = Properties.Resources.enemy1;
                Sprite.Width = Sprite.Height = 50;
                Sprite.BackColor = Color.Transparent;
                Sprite.SizeMode = PictureBoxSizeMode.Zoom;
                Sprite.Location = new Point(rand.Next(100, f.Width - 100), -100);

                f.xGamePanel.Controls.Add(this.Sprite);
            }

            public override void TICK()
            {
                if (this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
                {
                    this.Sprite.Dispose();
                    Conf.EnemiesToRemove.Add(this);
                    DistanceTravelled = MaxDistanceTravel;

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
                if (this.Sprite.Top >= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
                {
                    this.Sprite.Dispose();
                    Conf.EnemiesToRemove.Add(this);
                    DistanceTravelled = MaxDistanceTravel;
                    hp -= 10;
                    this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                    p.HPCheck();
                }
            }

            public override void GetHit()
            {
                if (!alreadyShot)
                {
                    this.Sprite.Dispose();
                    Conf.EnemiesToRemove.Add(this);
                    score += scoreMultiplier;
                    Console.WriteLine("score = " + score);
                    this.formHandle.Pointslbl.Text = Convert.ToString(score);
                    alreadyShot = true;
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
                alreadyShot = false;

                Sprite = new PictureBox();

                Sprite.Image = Properties.Resources.enemy3;
                Sprite.Width = Sprite.Height = 50;
                Sprite.BackColor = Color.Transparent;
                Sprite.SizeMode = PictureBoxSizeMode.Zoom;

                Sprite.Location = new Point(rand.Next(100, f.Width - 100), -100);

                EnemyShootTimer = new System.Windows.Forms.Timer();
                EnemyShootTimer.Tick += new System.EventHandler(EnemyShootTimer_Tick);
                EnemyShootTimer.Interval = 2500;
                EnemyShootTimer.Start();

                f.xGamePanel.Controls.Add(this.Sprite);
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
                if(!alreadyShot)
                {
                    EnemyShootTimer.Stop();
                    this.Sprite.Dispose();
                    Conf.EnemiesToRemove.Add(this);
                    score += scoreMultiplier;
                    Console.WriteLine("score = " + score);
                    this.formHandle.Pointslbl.Text = Convert.ToString(score);
                    alreadyShot = true;
                }                
            }

            private void EnemyShootTimer_Tick(object sender, EventArgs e)
            {
                if (this.Sprite.Top > 0 && !Pause)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Enemy bullet shot.");
                    EnemyBullet b = new EnemyBullet(formHandle, p, this, "side", this.Sprite.Location.X + this.Sprite.Width / 2, this.Sprite.Location.Y + this.Sprite.Height);                    

                    using (SoundPlayer sp = new SoundPlayer(Properties.Resources.laser))
                    {
                        sp.Play();
                    }

                    Conf.enemyBullets.Add(b);
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

            public EnemyBullet(Form1 f, Player p, Enemy e, String type, int x, int y)
            {
                formHandle = f;
                this.p = p;

                BulletSpeed = 20;
                DistanceTravelled = 0;
                MaxDistanceTravelled = f.Height + 2000;

                if(type == "straight")
                {
                    BulletSpeedHorizontal = 0;
                }
                else if(type == "left")
                {
                    BulletSpeedHorizontal = -10;
                }
                else if(type == "right")
                {
                    BulletSpeedHorizontal = 10;
                }
                else
                {
                    Random Seed = new Random();
                    int direction = Seed.Next(2);
                    if (direction == 0)
                    {
                        BulletSpeedHorizontal = 10;
                    }
                    else
                    {
                        BulletSpeedHorizontal = -10;
                    }
                }                
                
                Sprite = new PictureBox();
                Sprite.Width = Sprite.Height = 10;
                Sprite.BackColor = Color.Red;
                Sprite.Location = new Point(x - this.Sprite.Width / 2, y);

                f.xGamePanel.Controls.Add(this.Sprite);
            }

            public void TICK()
            {
                if (this.Sprite.Top > 0)
                {
                    if (this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
                    {
                        DistanceTravelled = MaxDistanceTravelled;
                        this.Sprite.Top = -1000;
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
                    if (this.Sprite.Left >= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width || this.Sprite.Left <= 0)
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

        public class EnemyBoss : Enemy
        {
            int HitPoints;
            int MaxHitPoints = 50;
            int phase;
            System.Windows.Forms.Timer BossShootTimer;
            PictureBox hpBar;

            public EnemyBoss(Form1 f, Player p)
            {
                HitPoints = MaxHitPoints;
                EnemySpeed = 5;
                DistanceTravelled = 0;
                MaxDistanceTravel = 300;
                phase = 1;

                this.p = p;
                formHandle = f;

                Sprite = new PictureBox();
                Sprite.Image = Properties.Resources.boss;
                Sprite.Width = 168;
                Sprite.Height = 251;
                Sprite.BackColor = Color.Transparent;
                Sprite.SizeMode = PictureBoxSizeMode.Zoom;
                Sprite.Location = new Point(f.Width / 2 - Sprite.Width / 2, -251);

                hpBar = new PictureBox();
                hpBar.Width = Sprite.Width;
                hpBar.Height = 10;
                hpBar.BackColor = Color.Red;
                hpBar.BorderStyle = BorderStyle.Fixed3D;
                hpBar.Location = new Point(Sprite.Left, Sprite.Top-(hpBar.Height+5));

                BossShootTimer = new System.Windows.Forms.Timer();
                BossShootTimer.Tick += new System.EventHandler(BossShootTimer_Tick);
                BossShootTimer.Interval = 1000;
                BossShootTimer.Start();

                f.xGamePanel.Controls.Add(this.Sprite);
                f.xGamePanel.Controls.Add(this.hpBar);
            }

            private void BossShootTimer_Tick(object sender, EventArgs e)
            {
                if(Sprite.Top > 0)
                {
                    if (!Pause && phase >= 1)
                    {
                        EnemyBullet b = new EnemyBullet(formHandle, p, this, "straight", this.Sprite.Location.X + this.Sprite.Width / 2, this.Sprite.Location.Y + this.Sprite.Height);
                        Conf.enemyBullets.Add(b);
                    }
                    if (!Pause && phase == 2)
                    {
                        EnemyBullet b1 = new EnemyBullet(formHandle, p, this, "left", this.Sprite.Location.X + this.Sprite.Width / 4, this.Sprite.Location.Y + this.Sprite.Height);
                        Conf.enemyBullets.Add(b1);
                        EnemyBullet b2 = new EnemyBullet(formHandle, p, this, "right", this.Sprite.Location.X + this.Sprite.Width / 2 + this.Sprite.Width / 4, this.Sprite.Location.Y + this.Sprite.Height);
                        Conf.enemyBullets.Add(b2);
                    }

                    if (!Pause)
                    {
                        using (SoundPlayer sp = new SoundPlayer(Properties.Resources.laser))
                        {
                            sp.Play();
                        }
                    }
                }                
            }

            public override void TICK()
            {
                if (this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
                {
                    Conf.EnemiesToRemove.Add(this);
                    BossShootTimer.Stop();

                    hp = 0;

                    Console.WriteLine("hp = " + hp);
                    this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                    p.HPCheck();
                }

                if (DistanceTravelled < MaxDistanceTravel)
                {
                    DistanceTravelled += EnemySpeed;
                    this.Sprite.Top += this.EnemySpeed;
                    this.hpBar.Top += this.EnemySpeed;
                }
                else
                {
                    Sprite.Left += EnemySpeed;
                    hpBar.Left += EnemySpeed;
                    if (Sprite.Left > formHandle.Width - Sprite.Width)
                    {
                        Sprite.Left = formHandle.Width - Sprite.Width;
                        hpBar.Left = Sprite.Left + (Sprite.Width - hpBar.Width) / 2;
                        EnemySpeed = -EnemySpeed;
                    }
                    else if(Sprite.Left < 0)
                    {
                        Sprite.Left = 0;
                        hpBar.Left = Sprite.Left + (Sprite.Width - hpBar.Width) / 2;
                        EnemySpeed = -EnemySpeed;
                    }                    
                }
            }

            public override void GetHit()
            {
                HitPoints--;
                hpBar.Width = Sprite.Width * HitPoints / MaxHitPoints;
                hpBar.Left = Sprite.Left + (Sprite.Width - hpBar.Width) / 2;
                formHandle.xGamePanel.Controls.Remove(this.hpBar);
                formHandle.xGamePanel.Controls.Add(this.hpBar);                

                if (HitPoints <= 0)
                {
                    this.Sprite.Dispose();
                    this.hpBar.Dispose();
                    Conf.EnemiesToRemove.Add(this);
                    score += 50 * scoreMultiplier;
                    Console.WriteLine("score = " + score);
                    formHandle.Pointslbl.Text = Convert.ToString(score);

                    //formHandle.showGameOver("You win");
                    formHandle.NextLevel();
                    BossShootTimer.Stop();
                }
                else if(HitPoints <= MaxHitPoints/2 && phase == 1)
                {
                    phase = 2;
                    DistanceTravelled -= 250;
                    EnemySpeed = 5;
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
                alreadyShot = false;

                Sprite = new PictureBox();

                HitPoints = 4;


                Sprite.Image = Properties.Resources.enemy2;
                Sprite.Width = Sprite.Height = 50;
                Sprite.BackColor = Color.Transparent;
                Sprite.SizeMode = PictureBoxSizeMode.Zoom;

                Sprite.Location = new Point(rand.Next(100, f.Width - 100),
                    -100);


                f.xGamePanel.Controls.Add(this.Sprite);
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
                        hp -= 10;
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
                    hp -= 10;

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
                    if(!alreadyShot)
                    {
                        this.Sprite.Dispose();
                        Conf.EnemiesToRemove.Add(this);
                        score += scoreMultiplier;
                        Console.WriteLine("score = " + score);
                        this.formHandle.Pointslbl.Text = Convert.ToString(score);
                        alreadyShot = true;
                    }                    
                }
                else if (this.HitPoints == 3)
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
    }

}
