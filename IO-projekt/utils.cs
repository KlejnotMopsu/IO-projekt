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


        async void NextLevel()
        {
            MainTimer.Stop();
            CurrentLevel++;

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

            Conf.ClearAll();
            xGamePanel.Visible = false;
            switch (CurrentLevel)
            {
                case 2:
                    {
                        xGamePanel = new SecondArea(this, p);
                        this.Controls.Add(xGamePanel);
                    }
                    break;
            }
            Console.WriteLine("Reached here...");
            while (BlackScreen.Left < BlackScreen.Width)
            {
                BlackScreen.Left += 50;
                await Task.Delay(10);
            }

            BlackScreen.Dispose();
            MainTimer.Start();
        }
    }
}
