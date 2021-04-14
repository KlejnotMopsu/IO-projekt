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
                public static List<Bullet> bullets = new List<Bullet>();
            }

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

            //Adam - rozpoczęcie nowej gry po GameOver
            public void ResetGame()
            {                
                Sprite.Location = new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width/2, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 100);

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
                Form1 formHandle;

                public Enemy(Form1 f, Player p)
                {
                    EnemySpeed = 4;
                    DistanceTravelled = 0;
                    MaxDistanceTravel = f.Height + 200;
                    this.p = p;
                    formHandle = f;

                    Sprite = new PictureBox();
                    Sprite.Width = Sprite.Height = 50;
                    Sprite.BackColor = Color.Transparent;
                    Sprite.Image = Properties.Resources.enemy1;
                    Sprite.SizeMode = PictureBoxSizeMode.Zoom;

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
                        if (this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
                        {
                            this.Sprite.Top = -1000;
                            DistanceTravelled = MaxDistanceTravel;
                            formHandle.Controls.Remove(this.Sprite);
                            Conf.enemies.Remove(this);
                            //Adam - po kontakcie z przeciwnikiem gracz traci 10hp
                            hp -= 10;
                            Console.WriteLine("hp = " + hp);
                            this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                            this.EnemyTimer.Stop();
                            p.HPCheck();
                        }
                        if (DistanceTravelled < MaxDistanceTravel)
                        {
                            DistanceTravelled += EnemySpeed;
                            this.Sprite.Top += this.EnemySpeed;
                        }
                        if (this.Sprite.Top == System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
                        {
                            //Adam - gdy przeciwnik przeleci niezestrzelony tracimy 10hp
                            hp -= 10;
                            Console.WriteLine("hp = " + hp);
                            this.formHandle.LifePointslbl.Text = Convert.ToString(hp);
                            this.EnemyTimer.Stop();
                            p.HPCheck();
                        }                        
                    }
                }
            }

            public class Bullet
            {
                public int DistanceTravelled;
                public int MaxDistanceTravelled;
                int BulletSpeed;
                Timer BulletTimer;
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

                    BulletTimer = new Timer();
                    BulletTimer.Interval = 20;
                    BulletTimer.Tick += new System.EventHandler(BulletTimer_Tick);

                    f.Controls.Add(this.Sprite);

                    BulletTimer.Start();

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
                                    Conf.bullets.Remove(this);
                                }
                            }
                        }

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
                        else if (this.DistanceTravelled >= this.MaxDistanceTravelled)
                        {
                            formHandle.Controls.Remove(this.Sprite);
                            Conf.bullets.Remove(this);
                        }                                             
                    }                    
                }                
            }

            //Adam - po zmianie hp sprawdzmy, czy spadło do 0
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
                Sprite.Image = Properties.Resources.player1_rtilt;
                MoveRightTimer.Start();
            }
            public void MoveRightStop()
            {
                Sprite.Image = Properties.Resources.player1;
                MoveRightTimer.Stop();
            }
            public void MoveLeft()
            {
                Sprite.Image = Properties.Resources.player1_ltilt;
                MoveLeftTimer.Start();
            }
            public void MoveLeftStop()
            {
                Sprite.Image = Properties.Resources.player1;
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
                Conf.bullets.Add(b);
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