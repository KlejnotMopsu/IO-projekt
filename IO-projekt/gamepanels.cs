﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace IO_projekt
{  

    public abstract class GamePanel : Panel
    {
        public Form1 FormHandle;
        public Form1.Player PlayerHandle;

        public abstract void TICK();

        public async void SpawnPlayer()
        {
            PlayerHandle.Sprite.Left = this.FormHandle.Width / 2 - PlayerHandle.Sprite.Width / 2;
            PlayerHandle.Sprite.Top = this.FormHandle.Height + PlayerHandle.Sprite.Height;

            //PlayerHandle.Sprite.Left = 1000;
            //PlayerHandle.Sprite.Top = 1000;

            while (PlayerHandle.Sprite.Top > this.Height - PlayerHandle.Sprite.Height - 50)
            {
                PlayerHandle.Sprite.Top -= 10;
                await Task.Delay(30);
                Console.WriteLine("awaiting");
            }
        }
    }

    public class FirstArea : GamePanel
    {
        Random Seed;
        public FirstArea(Form1 fh, Form1.Player ph)
        {
            FormHandle = fh;
            PlayerHandle = ph;

            Seed = new Random();

            Conf.ClearAll();

            this.BackColor = Color.Black;

            this.Controls.Add(PlayerHandle.Sprite);

            this.Width = FormHandle.Width;
            this.Height = FormHandle.Height;

            this.SpawnPlayer();
        }

        public override void TICK()
        {

            int roll = Seed.Next(150);
            if ((roll == 0 || roll == 1) && Form1.level >= 1 && !Form1.BossLevel)
            {
                Form1.EnemyStandard en = new Form1.EnemyStandard(FormHandle, FormHandle.p);
                Conf.enemies.Add(en);
            }
            else if (roll == 2 && Form1.level >= 2 && !Form1.BossLevel)
            {
                Form1.EnemyDreadnought en = new Form1.EnemyDreadnought(FormHandle, FormHandle.p);
                Conf.enemies.Add(en);
            }
            else if (roll == 3 && Form1.level >= 3 && !Form1.BossLevel)
            {
                Form1.EnemyRifleman er = new Form1.EnemyRifleman(FormHandle, FormHandle.p);
                Conf.enemies.Add(er);
            }
            else if (Form1.level >= 4 && !Form1.BossLevel)
            {
                Form1.BossLevel = true;
                Form1.level = 0;
                Form1.EnemyBoss eb = new Form1.EnemyBoss(FormHandle, FormHandle.p);
                Conf.enemies.Add(eb);
            }

            roll = Seed.Next(750);
            if (roll == 0 && !Form1.BossLevel)
            {
                Form1.Bonus b = new Form1.Bonus(FormHandle, FormHandle.p);
                Conf.bonuses.Add(b);
            }

            if (Form1.scoreMultiplierTime > 0)
            {
                FormHandle.scoreMultiplierTimeLabel.Text = "Double Points: " + (Form1.scoreMultiplierTime / 1000.0).ToString() + "s";
                Form1.scoreMultiplierTime -= FormHandle.MainTimer.Interval;
            }
            else
            {
                Form1.scoreMultiplier = 1;
                FormHandle.scoreMultiplierTimeLabel.Visible = false;
                if (FormHandle.doubleShootTimeLabel.Visible)
                {
                    FormHandle.doubleShootTimeLabel.Top = Form1.labelTopOffset;
                }
            }

            if (FormHandle.p.DoubleShootTime > 0)
            {
                FormHandle.doubleShootTimeLabel.Text = "Double Shoot: " + (FormHandle.p.DoubleShootTime / 1000.0).ToString() + "s";
            }
            else
            {
                FormHandle.doubleShootTimeLabel.Visible = false;
                if (FormHandle.scoreMultiplierTimeLabel.Visible)
                {
                    FormHandle.scoreMultiplierTimeLabel.Top = Form1.labelTopOffset;
                }
            }
        }
    }

    public class SecondArea : GamePanel
    {
        Random Seed;

        public SecondArea(Form1 fh, Form1.Player ph)
        {
            FormHandle = fh;
            PlayerHandle = ph;

            Seed = new Random();

            Conf.ClearAll();

            this.BackColor = ColorTranslator.FromHtml("#033806");

            this.Controls.Add(PlayerHandle.Sprite);

            this.Width = FormHandle.Width;
            this.Height = FormHandle.Height;

            this.SpawnPlayer();
        }

        public override void TICK()
        {
            int roll = Seed.Next(150);
            if (roll > 135)
            {
                //Conf.enemies.Add(new Form1.EnemyRifleman(FormHandle, FormHandle.p));
            }
            if (roll > 145)
            {
                //Conf.enemies.Add(new Meteorite(this));
                Conf.enemies.Add(new Form1.EnemyLocust(FormHandle, FormHandle.p));
            }
            if (roll > 148)
            {
                Conf.enemies.Add(new Meteorite(this));
            }
        }

        private class Meteorite : Form1.Enemy
        {
            GamePanel GPHandle;          

            public Meteorite(GamePanel gp)
            {
                GPHandle = gp;
                Sprite = new PictureBox();

                Sprite.Width = Sprite.Height = 50;
                Sprite.Image = Properties.Resources.MeteoritePic;
                Sprite.BackColor = Color.Transparent;
                Sprite.SizeMode = PictureBoxSizeMode.Zoom;

                Random r = new Random();
                
                Sprite.Top = -50;
                Sprite.Left = r.Next(0, GPHandle.Width - this.Sprite.Width);

                GPHandle.Controls.Add(Sprite);
            }

            public override void TICK()
            {
 
                Sprite.Top += 3;
                if (Sprite.Top > GPHandle.Height)
                {
                    this.Sprite.Dispose();
                    Conf.EnemiesToRemove.Add(this);
                }
            }

            public override void GetHit()
            {
                return;
            }
        }
    }
}