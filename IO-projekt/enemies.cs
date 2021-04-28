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

                f.GamePanel.Controls.Add(this.Sprite);

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
                if (direction == 0)
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

                f.GamePanel.Controls.Add(this.Sprite);
            }

            public void TICK()
            {
                if (this.Sprite.Top > 0)
                {
                    if (this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
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

                f.GamePanel.Controls.Add(this.Sprite);
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
                if (this.Sprite.Top > 0)
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


                f.GamePanel.Controls.Add(this.Sprite);
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
