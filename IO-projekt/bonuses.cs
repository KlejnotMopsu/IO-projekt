using System;

namespace IO_projekt
{
    public abstract class Bonus2
    {
        public System.Windows.Forms.PictureBox Sprite;
        public Form1 FormHandle;
        bool Collected;

        public Bonus2(Form1 fh)
        {
            FormHandle = fh;
            Sprite = new System.Windows.Forms.PictureBox();
            Sprite.Width = Sprite.Height = 35;
            Sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            Sprite.Top = -Sprite.Height;
            Sprite.Left = new Random().Next(0, FormHandle.Width - Sprite.Width);

            FormHandle.xGamePanel.Controls.Add(Sprite);

            Collected = false;
        }

        public void TICK()
        {
            Sprite.Top += 2;
            if (Sprite.Bounds.IntersectsWith(FormHandle.p.Sprite.Bounds) && !Collected)
            {
                Collected = true;
                PlayerCollected();
            }
        }
        public abstract void PlayerCollected();
    }


    public class Credit : Bonus2
    {
        int Amount = 100;        

        public Credit(Form1 fh, Form1.Enemy hEnemy = null, int AmountModifier = 0) : base(fh)
        {
            Sprite.Image = Properties.Resources.CreditPic;
            Amount += AmountModifier;

            if (AmountModifier != 0)
            {
                Sprite.Width += (int)(AmountModifier / 2);
                Sprite.Height += (int)(AmountModifier / 2);
            }
            
            if (hEnemy != null)
            {
                Sprite.Top = hEnemy.Sprite.Top - Sprite.Height / 2;
                Sprite.Left = hEnemy.Sprite.Left - Sprite.Width / 2;
            }
        }

        public override void PlayerCollected()
        {
            FormHandle.p.Credits += Amount;            
            this.FormHandle.bonusMedia.controls.play();
            Sprite.Dispose();
        }
    }
}
