using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Media;


namespace IO_projekt
{
    public abstract class Gun
    {
        public Form1.Player PlayerHandle;
        public int ShotsInterval;

        public Gun(Form1.Player ph)
        {
            PlayerHandle = ph;
        }

        public abstract void Shoot();
    }

    public class BasicGun : Gun
    {
        public BasicGun(Form1.Player ph) : base(ph)
        {
            ShotsInterval = 100;
        }

        public override void Shoot()
        {
            
            using (SoundPlayer sp = new SoundPlayer(Properties.Resources.laser))
            {
                sp.Play();
            }

            Conf.bullets.Add(new Form1.Player.Bullet(PlayerHandle.formHandle, PlayerHandle));
        }
    }

    public class ScatterGun : Gun
    {
        public ScatterGun(Form1.Player ph) : base(ph)
        {
            ShotsInterval = 300;
        }

        public override void Shoot()
        {
            using (SoundPlayer sp = new SoundPlayer(Properties.Resources.laser))
            {
                sp.Play();
            }

            Conf.bullets.Add(new ScatterGunBullet(PlayerHandle.formHandle, -3));
            Conf.bullets.Add(new Form1.Player.Bullet(PlayerHandle.formHandle, PlayerHandle));
            Conf.bullets.Add(new ScatterGunBullet(PlayerHandle.formHandle, 3));
        }

        private class ScatterGunBullet : Form1.Player.Bullet
        {
            int Angle;
            public ScatterGunBullet(Form1 fh, int a)
            {
                Angle = a;

                formHandle = fh;
                BulletSpeed = 15;
                BulletSpeedHorizontal = 0;
                DistanceTravelled = 0;
                MaxDistanceTravelled = fh.Height + 200;
                Sprite = new PictureBox();
                Sprite.Width = Sprite.Height = 10;
                Sprite.BackColor = Color.Transparent;
                Sprite.Image = Properties.Resources.BulletPic;
                Sprite.Location = new Point(
                    formHandle.p.Sprite.Location.X + formHandle.p.Sprite.Width/2 - this.Sprite.Width/2,
                    formHandle.p.Sprite.Location.Y
                    );

                fh.xGamePanel.Controls.Add(this.Sprite);
            }

            public override void TICK()
            {
                if (this.Sprite.Top > 0)
                {
                    foreach (Form1.Enemy en in Conf.enemies)
                    {
                        if (this.Sprite.Bounds.IntersectsWith(en.Sprite.Bounds))
                        {
                            Conf.BulletsToRemove.Add(this);
                            this.Sprite.Dispose();

                            en.GetHit();
                        }
                    }

                    foreach (Form1.Bonus b in Conf.bonuses)
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

                    this.Sprite.Left += Angle;
                }
                else if (this.DistanceTravelled >= this.MaxDistanceTravelled)
                {
                    formHandle.Controls.Remove(this.Sprite);
                    Conf.BulletsToRemove.Add(this);
                }
            }
        }
    }

    public class Rocket : Form1.Player.Bullet
    {
        public Rocket(Form1 fh)
        {
            Console.WriteLine("Creating Rocket...");
            formHandle = fh;
            BulletSpeed = 4;
            DistanceTravelled = 0;
            MaxDistanceTravelled = fh.Height + 200;
            Sprite = new PictureBox();
            Sprite.Width = 20;
            Sprite.Height = 40;
            Sprite.SizeMode = PictureBoxSizeMode.Zoom;
            Sprite.BackColor = Color.Transparent;
            Sprite.Image = Properties.Resources.rocket;
            Sprite.Location = new Point(
                formHandle.p.Sprite.Location.X + formHandle.p.Sprite.Width / 2 - this.Sprite.Width / 2,
                formHandle.p.Sprite.Location.Y
                );

            fh.xGamePanel.Controls.Add(this.Sprite);
        }

        public override void TICK()
        {
            if (this.Sprite.Top > 0)
            {
                foreach (Form1.Enemy en in Conf.enemies)
                {
                    if (this.Sprite.Bounds.IntersectsWith(en.Sprite.Bounds))
                    {
                        Conf.BulletsToRemove.Add(this);
                        this.Sprite.Dispose();

                        en.GetHit();
                    }
                }

                foreach (Form1.Bonus b in Conf.bonuses)
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
                Conf.BulletsToRemove.Add(this);
            }
        }

        private async void CreateSmokeCloud()
        {
            Thread th = new Thread(() =>
                {
                    PictureBox TempPb = new PictureBox()
                    {
                        Width = 20,
                        Height = 20,
                        BackColor = Color.White
                    };
                    //this.formHandle.Invoke(formHandle.Controls.Add(TempPb));
                }
            );

            th.Start();
        }
    }
}
