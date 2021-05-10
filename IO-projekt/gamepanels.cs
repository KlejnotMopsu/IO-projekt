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
        }
    }
}
