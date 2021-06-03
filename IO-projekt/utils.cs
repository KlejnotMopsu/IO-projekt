using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace IO_projekt
{
    public partial class Form1 : Form
    {
        System.Threading.Thread FpsThread;

        public void StartFpsThread()
        {
            Label FpsLabel = new Label();
            FpsLabel.Font = new Font("Terminal", 20);
            FpsLabel.Text = "FPS: ";
            FpsLabel.ForeColor = Color.White;
            FpsLabel.AutoSize = true;
            FpsLabel.Top = this.Height - FpsLabel.Height-10;

            this.Controls.Add(FpsLabel);
            FpsLabel.BringToFront();

            Console.WriteLine("Initializing StartFpsThread()...");
            FpsThread = new System.Threading.Thread(() =>
            {
                Console.WriteLine("Declaring FpsThread...");

                System.Timers.Timer FpsTimer = new System.Timers.Timer(1000);
                FpsTimer.Enabled = true;
                FpsTimer.Elapsed += new System.Timers.ElapsedEventHandler(FpsTimer_tick);

                Console.WriteLine("Starting FpsTimer...");
                FpsTimer.Start();

                void FpsTimer_tick(object sender, System.Timers.ElapsedEventArgs e)
                {
                    FpsLabel.Invoke(new MethodInvoker(delegate { FpsLabel.Text = $"FPS: {FramesInCurrentSecond}"; }));
                    FramesInCurrentSecond = 0;
                }
            }
            );

            Console.WriteLine("Starting FpsThread...");
            FpsThread.Start();
        }

        public void StartNewGame()
        {
            Conf.ClearAndDisposeAll();
            p = new Player(this);
            if (xGamePanel != null)
                xGamePanel.Dispose();
            xGamePanel = new FirstArea(this, p);
            this.Controls.Add(this.xGamePanel);
            MainTimer.Start();
            this.Focus();
            GameOver = false;
            Pause = false;
            pauseLabel.Visible = false;
            hp = 100;
            UpdateHpLabel();

            if (Properties.Settings.Default.ShowFPS)
                StartFpsThread();
        }

        public async void NextLevel()
        {
            MainTimer.Stop();
            CurrentLevel++;
            BossLevel = false;

            UpdateHpLabel();
            p.MoveRightStop();
            p.MoveLeftStop();
            p.MoveUpStop();
            p.MoveDownStop();
            p.CloseGunLock();

            Panel BlackScreen = new Panel();
            BlackScreen.BackColor = Color.Black;
            BlackScreen.Width = this.Width+50;
            BlackScreen.Height = this.Height;
            BlackScreen.Top = 0;
            BlackScreen.Left = -BlackScreen.Width;

            this.Controls.Add(BlackScreen);
            BlackScreen.BringToFront();

            BlackScreen.Controls.Add(new Label() {
                Text = "Next level",
                Font = new Font("Stencil", 50, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            });

            while (BlackScreen.Left < -50)
            {
                BlackScreen.Left += 50;
                await Task.Delay(10);
            }

            await Task.Delay(500);

            Conf.ClearAndDisposeAll();
            new ShopPanel(this);
            return;
            xGamePanel.Visible = false;

            switch (CurrentLevel)
            {
                case 1:
                    {
                        xGamePanel = new FirstArea(this, p);
                        this.Controls.Add(xGamePanel);
                    }
                    break;

                case 2:
                    {
                        xGamePanel = new SecondArea(this, p);
                        this.Controls.Add(xGamePanel);
                    }
                    break;

                case 3:
                    {
                        xGamePanel = new ThirdArea(this, p);
                        this.Controls.Add(xGamePanel);
                    }
                    break;
            }

            while (BlackScreen.Left < BlackScreen.Width)
            {
                BlackScreen.Left += 50;
                await Task.Delay(10);
            }

            BlackScreen.Dispose();
            MainTimer.Start();
        }

        public void BringUpShop()
        {
            new ShopPanel(this);
        }
    }
}
