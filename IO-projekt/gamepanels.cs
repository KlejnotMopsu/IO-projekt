using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace IO_projekt
{  

    public abstract class GamePanel : Panel
    {
        public Form1 FormHandle;
        public Form1.Player PlayerHandle;

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
        public FirstArea(Form1 fh, Form1.Player ph)
        {
            FormHandle = fh;
            PlayerHandle = ph;

            Conf.ClearAll();

            this.BackColor = Color.Black;

            this.Controls.Add(PlayerHandle.Sprite);

            this.Width = FormHandle.Width;
            this.Height = FormHandle.Height;

            this.SpawnPlayer();
        }


    }

    public class SecondArea : GamePanel
    {
        public SecondArea(Form1 fh, Form1.Player ph)
        {
            FormHandle = fh;
            PlayerHandle = ph;

            Conf.ClearAll();

            this.BackColor = ColorTranslator.FromHtml("#033806");

            this.Controls.Add(PlayerHandle.Sprite);

            this.Width = FormHandle.Width;
            this.Height = FormHandle.Height;

            this.SpawnPlayer();
        }
    }
}
