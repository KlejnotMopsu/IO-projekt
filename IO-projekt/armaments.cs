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
        int SmokeInterval = 40;
        int SmokeCooldown;

        public Rocket(Form1 fh)
        {
            SmokeCooldown = SmokeInterval;

            Console.WriteLine("Creating Rocket...");
            formHandle = fh;
            BulletSpeed = 7;
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
            SmokeCooldown -= 10;
            if (SmokeCooldown <= 0)
            {
                CreateSmokeCloud(new Point(Sprite.Location.X, Sprite.Location.Y+Sprite.Height));
                SmokeCooldown = SmokeInterval;
            }

            if (this.Sprite.Top > 0)
            {
                foreach (Form1.Enemy en in Conf.enemies)
                {
                    if (this.Sprite.Bounds.IntersectsWith(en.Sprite.Bounds))
                    {
                        Conf.BulletsToRemove.Add(this);
                        this.Sprite.Dispose();
                        this.Explode();

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

        private void Explode()
        {
            Rectangle TempRectangle = new Rectangle()
            {
                Width = 200,
                Height = 200
            };
            TempRectangle.Location = new Point(
                this.Sprite.Location.X - TempRectangle.Width/2,
                this.Sprite.Location.Y - TempRectangle.Height/2
                );
            
            foreach (Form1.Enemy e in Conf.enemies)
            {
                if (e.Sprite.Bounds.IntersectsWith(TempRectangle))
                {
                    e.GetHit();
                }
            }
        }

        private void CreateSmokeCloud(Point loc)
        {
            Thread th = new Thread(async () =>
                {
                    PictureBox TempPb = new PictureBox()
                    {
                        Width = 20,
                        Height = 20,
                        BackColor = Color.Transparent,
                        //Image = Properties.Resources.SmokePic,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Location = loc
                    };
                    formHandle.xGamePanel.Invoke(new MethodInvoker(delegate { formHandle.Controls.Add(TempPb); }));
                    TempPb.BringToFront();


                    int alpha = 90;
                    while (alpha > 0)
                    {                        
                        TempPb.Image = SetAlpha((Bitmap)Properties.Resources.SmokePic, alpha);
                        alpha -= 5;
                        TempPb.Width += 1;
                        TempPb.Height += 1;
                        await Task.Delay(100).ConfigureAwait(false);
                    }

                    Console.WriteLine("Reached here");
                    formHandle.xGamePanel.Invoke(new MethodInvoker(delegate { formHandle.Controls.Remove(TempPb); }));
                    TempPb.Dispose();
                }
            );

            th.Start();

            Bitmap SetAlpha(Bitmap bmpIn, int alpha)
            {
                Bitmap bmpOut = new Bitmap(bmpIn.Width, bmpIn.Height);
                float a = alpha / 255f;
                Rectangle r = new Rectangle(0, 0, bmpIn.Width, bmpIn.Height);

                float[][] matrixItems = {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, a, 0},
                new float[] {0, 0, 0, 0, 1}};

                System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(matrixItems);

                System.Drawing.Imaging.ImageAttributes imageAtt = new System.Drawing.Imaging.ImageAttributes();
                imageAtt.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

                using (Graphics g = Graphics.FromImage(bmpOut))
                    g.DrawImage(bmpIn, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);

                return bmpOut;
            }
        }
    }
}
