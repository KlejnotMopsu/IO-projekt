using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using WMPLib;
using System.IO;

namespace IO_projekt
{  
    public partial class Form1: Form
    {
        public abstract class GamePanel : Panel
        {
            public Form1 FormHandle;
            public Form1.Player PlayerHandle;
            public Random Seed;

            public abstract void TICK();

            public GamePanel(Form1 fh, Player ph)
            {
                Conf.ClearAndDisposeAll();

                Seed = new Random();
                FormHandle = fh;
                PlayerHandle = ph;
            }

            public async void SpawnPlayer()
            {
                PlayerHandle.Sprite.Left = this.FormHandle.Width / 2 - PlayerHandle.Sprite.Width / 2;
                PlayerHandle.Sprite.Top = this.FormHandle.Height + PlayerHandle.Sprite.Height;

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
            public FirstArea(Form1 fh, Form1.Player ph) : base(fh, ph)
            {          
                this.BackColor = Color.Black;
                this.FormHandle.BackColor = Color.Black;

                this.Controls.Add(PlayerHandle.Sprite);

                this.Width = FormHandle.Width;
                this.Height = FormHandle.Height;

                this.SpawnPlayer();
            }

            public override void TICK()
            {
                int roll = this.Seed.Next(150);
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

                if(Conf.enemies.Count == 0)
                {
                    Form1.EnemyStandard en = new Form1.EnemyStandard(FormHandle, FormHandle.p);
                    Conf.enemies.Add(en);
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
            public int phase;
            public EnemySecondBoss leftBoss;
            public EnemySecondBoss rightBoss;
            int riflemanCount;
            WindowsMediaPlayer bossMedia;

            public SecondArea(Form1 fh, Form1.Player ph) : base(fh, ph)
            {
                this.BackColor = ColorTranslator.FromHtml("#033806");
                FormHandle.BackColor = ColorTranslator.FromHtml("#033806");

                this.Controls.Add(PlayerHandle.Sprite);

                this.Width = FormHandle.Width;
                this.Height = FormHandle.Height;

                this.SpawnPlayer();

                phase = 1;
            }

            public override void TICK()
            {
                if (phase == 1)
                {
                    int roll = Seed.Next(200);
                    riflemanCount = 0;

                    if (roll <= 3)
                    {
                        Conf.enemies.Add(new Meteorite(this, FormHandle.p));
                    }
                    else if (roll <= 5)
                    {
                        Conf.enemies.Add(new Form1.EnemyLocust(FormHandle, FormHandle.p));
                    }
                    else if (roll <= 7)
                    {                                                
                        Conf.enemies.Add(new Form1.EnemyRifleman(FormHandle, FormHandle.p));
                    }

                    foreach(Enemy e in Conf.enemies)
                    {
                        if(e is Form1.EnemyRifleman)
                        {
                            riflemanCount++;
                            break;
                        }
                    }
                    if(riflemanCount == 0)
                    {
                        Conf.enemies.Add(new Form1.EnemyRifleman(FormHandle, FormHandle.p));
                    }

                    roll = Seed.Next(600);
                    if (roll == 0)
                    {
                        Form1.Bonus b = new Form1.Bonus(FormHandle, FormHandle.p);
                        Conf.bonuses.Add(b);
                    }
                }
                else if(phase == 2)
                {
                    leftBoss = new EnemySecondBoss(FormHandle, FormHandle.p, "left");
                    rightBoss = new EnemySecondBoss(FormHandle, FormHandle.p, "right");
                    Conf.enemies.Add(leftBoss);
                    Conf.enemies.Add(rightBoss);
                    phase = 3;

                    bossMedia = new WindowsMediaPlayer();
                    File.WriteAllBytes(@"sound\boss2Inc.wav", StreamToByteArr(Properties.Resources.boss2Inc));
                    bossMedia.URL = @"sound\boss2Inc.wav";
                    bossMedia.settings.volume = 10;
                    bossMedia.controls.play();
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

                if (score >= 150 && phase == 1)
                {
                    phase = 2;
                }
            }

            private class Meteorite : Form1.Enemy
            {
                GamePanel GPHandle;

                public Meteorite(GamePanel gp, Form1.Player ph)
                {
                    GPHandle = gp;
                    p = ph;
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

                    if (this.Sprite.Bounds.IntersectsWith(p.Sprite.Bounds))
                    {
                        Form1.hp -= 45;
                        GPHandle.FormHandle.UpdateHpLabel();
                        this.Sprite.Dispose();
                        Conf.EnemiesToRemove.Add(this);
                        p.HPCheck();
                    }
                }

                public override void GetHit()
                {
                    return;
                }
            }
        }

        public class ThirdArea : GamePanel
        {
            public ThirdArea(Form1 fh, Form1.Player ph) : base(fh, ph)
            {
                this.BackColor = ColorTranslator.FromHtml("#2e040c");
                FormHandle.BackColor = ColorTranslator.FromHtml("#2e040c");

                this.Controls.Add(PlayerHandle.Sprite);

                this.Width = FormHandle.Width;
                this.Height = FormHandle.Height;

                this.SpawnPlayer();
            }

            public override void TICK()
            {
                int roll = Seed.Next(200);

                if (roll < 3)
                {
                    Form1.EnemyStandard er = new Form1.EnemyStandard(FormHandle, FormHandle.p);
                    Conf.enemies.Add(er);
                }

                if (roll > 197)
                {
                    Conf.enemies.Add(new Form1.EnemyExploder(FormHandle, FormHandle.p));
                }

                roll = Seed.Next(750);
                if (roll == 0)
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
    }    
}
