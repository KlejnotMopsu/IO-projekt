using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using System.Drawing;

namespace IO_projekt
{
    public partial class Form1 : Form
    {
        public class Player
        {
            Form1 formHandle;

            public PictureBox Sprite;
            public Point Position;

            Image sprite_neutral = Image.FromFile("src\\graphics\\player1.png");
            Image sprite_ltilt = Image.FromFile("src\\graphics\\player1-ltilt.png");
            Image sprite_rtilt = Image.FromFile("src\\graphics\\player1-rtilt.png");

            int MovementSpeed;

            Timer MoveRightTimer;
            Timer MoveLeftTimer;
            Timer MoveUpTimer;
            Timer MoveDownTimer;
            Timer ShootTimer;
            Timer EnemyTimer;
            System.Diagnostics.Stopwatch stopwatch;

            static class Conf
            {
                public static List<Enemy> enemies = new List<Enemy>();
            }

            public Player(Form1 f, int x = 0, int y = 0)
            {
                Console.WriteLine("Player() - f.Width: " + f.Width + "  f.Height: " + f.Height);

                formHandle = f;
                Sprite = new PictureBox();
                Sprite.Height = Sprite.Width = 50;
                Sprite.Image = sprite_neutral;
                Sprite.BackColor = Color.Transparent;
                Position = new Point(x, y);
                Sprite.Location = Position;
                Console.WriteLine("Sprite.Location = " + "(" + Sprite.Location.X + "," + Sprite.Location.Y + ")");
                f.Controls.Add(this.Sprite);

                MovementSpeed = 10;

                MoveRightTimer = new Timer();
                MoveRightTimer.Tick += new System.EventHandler(MoveRightTimer_Tick);
                MoveRightTimer.Interval = 10;

                MoveLeftTimer = new Timer();
                MoveLeftTimer.Tick += new System.EventHandler(MoveLeftTimer_Tick);
                MoveLeftTimer.Interval = 10;

                MoveUpTimer = new Timer();
                MoveUpTimer.Tick += new System.EventHandler(MoveUpTimer_Tick);
                MoveUpTimer.Interval = 10;

                MoveDownTimer = new Timer();
                MoveDownTimer.Tick += new System.EventHandler(MoveDownTimer_Tick);
                MoveDownTimer.Interval = 10;

                ShootTimer = new Timer();
                ShootTimer.Tick += new System.EventHandler(ShootTimer_Tick);
                ShootTimer.Interval = 200;

                EnemyTimer = new Timer();
                EnemyTimer.Tick += new System.EventHandler(EnemyTimer_Tick);
                EnemyTimer.Interval = 2000;
                EnemyTimer.Start();

                stopwatch = new System.Diagnostics.Stopwatch();
            }

            public class Enemy
            {
                public int DistanceTravelled;
                int EnemySpeed;
                public int MaxDistanceTravel;
                Timer EnemyTimer;
                public PictureBox Sprite;
                Random rand = new Random();
                Player p;

                public Enemy(Form1 f, Player p)
                {
                    EnemySpeed = 4;
                    DistanceTravelled = 0;
                    MaxDistanceTravel = f.Height + 200;
                    this.p = p;


                    Sprite = new PictureBox();
                    Sprite.Width = Sprite.Height = 50;
                    Sprite.BackColor = Color.Blue;

                    Sprite.Location = new Point(rand.Next(100, f.Width - 100),
                        -100);


                    EnemyTimer = new Timer();
                    EnemyTimer.Interval = 20;
                    EnemyTimer.Tick += new System.EventHandler(EnemyTimer_Tick);

                    f.Controls.Add(this.Sprite);

                    EnemyTimer.Start();
                }

                public void EnemyTimer_Tick(Object sender, EventArgs e)
                {
                    if (!Pause)
                    {
                        if ((this.Sprite.Top >= p.Sprite.Top - 50 && this.Sprite.Top <= p.Sprite.Top + 50) && (this.Sprite.Left >= p.Sprite.Left - 50 && this.Sprite.Left <= p.Sprite.Left + 50))
                        {
                            this.Sprite.Top = -1000;
                            DistanceTravelled = MaxDistanceTravel;
                            Conf.enemies.Remove(this);
                        }
                        else if (DistanceTravelled < MaxDistanceTravel)
                        {
                            DistanceTravelled += EnemySpeed;
                            this.Sprite.Top += this.EnemySpeed;
                        }
                    }
                }
            }

            public class Bullet
            {
                int DistanceTravelled;
                int MaxDistanceTravelled;
                int BulletSpeed;
                Timer BulletTimer;
                PictureBox Sprite;
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

                    BulletTimer = new Timer();
                    BulletTimer.Interval = 20;
                    BulletTimer.Tick += new System.EventHandler(BulletTimer_Tick);

                    f.Controls.Add(this.Sprite);

                    BulletTimer.Start();

                }
                public void BulletTimer_Tick(Object sender, EventArgs e)
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
                            }
                        }
                    }

                    //zmiana - Artur
                    if (this.DistanceTravelled > this.MaxDistanceTravelled)
                    {
                        formHandle.Controls.Remove(this.Sprite);
                    }
                    //koniec zmiany

                    if (tmp != null)
                    {
                        //zmiana - Mikołaj
                        score = score + 1;
                        Console.WriteLine("score = " + score);
                        this.formHandle.Pointslbl.Text = Convert.ToString(score);
                        // Pointslbl.Text = (score < 10) ? "0" + score.ToString() : score.ToString();
                        //---------------------------------------------------
                        Conf.enemies.Remove(tmp);
                    }
                    if (DistanceTravelled < MaxDistanceTravelled)
                    {
                        DistanceTravelled += BulletSpeed;
                        this.Sprite.Top -= this.BulletSpeed;
                    }
                }
            }

            public void MoveRight()
            {
                Sprite.Image = sprite_rtilt;
                MoveRightTimer.Start();
            }
            public void MoveRightStop()
            {
                Sprite.Image = sprite_neutral;
                MoveRightTimer.Stop();
            }
            public void MoveLeft()
            {
                Sprite.Image = sprite_ltilt;
                MoveLeftTimer.Start();
            }
            public void MoveLeftStop()
            {
                Sprite.Image = sprite_neutral;
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
            }

            private void EnemyTimer_Tick(object sender, EventArgs e)
            {
                if (!Pause)
                {
                    Console.WriteLine(OPERATIONS++ + "> " + "Enemy spawned.");
                    Enemy enemy = new Enemy(formHandle, this);
                    Conf.enemies.Add(enemy);
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