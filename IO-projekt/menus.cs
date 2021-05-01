using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace IO_projekt
{
    public static class MenusConfig
    {
        public static Font DefaultFont = new Font("Stencil", 24, FontStyle.Bold);
    }

    public class PauseMenuPanel : TableLayoutPanel
    {
        Form1 FormHandle;
        Label ExitLabel;
        Label ContinueLabel;

        string[] Selections = { "continue", "exit" };
        int MaxSelection = 1;
        int CurrentSelection = 0;

        public PauseMenuPanel(Form1 fh)
        {
            FormHandle = fh;
            //Grid = new TableLayoutPanel();
            

            this.ColumnCount = 3;
            this.RowCount = 3;

            //this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));

            //this.RowStyles.Clear();
            //this.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));

            ContinueLabel = new Label();
            this.Controls.Add(ContinueLabel, 1, 0);
            this.Controls.Add(new PictureBox() { Width = 25, Height = 25, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.None }, 0, 0);
            this.Controls.Add(new PictureBox() { Width = 25, Height = 25, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.None }, 2, 0);
            ContinueLabel.Font = MenusConfig.DefaultFont;
            ContinueLabel.Text = "CONTINUE";
            ContinueLabel.ForeColor = Color.White;
            ContinueLabel.AutoSize = true;
            ContinueLabel.Anchor = AnchorStyles.None;

            ExitLabel = new Label();
            this.Controls.Add(ExitLabel, 1, 1);
            this.Controls.Add(new PictureBox() { Image = Properties.Resources.LeftSelectionMarker, Width = 25, Height = 25, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.None }, 0, 1);
            this.Controls.Add(new PictureBox() { Image = Properties.Resources.RightSelectionMarker, Width = 25, Height = 25, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.None }, 2, 1);
            ExitLabel.Font = MenusConfig.DefaultFont;
            ExitLabel.Text = "EXIT";
            ExitLabel.ForeColor = Color.White;
            ExitLabel.AutoSize = true;
            ExitLabel.Anchor = AnchorStyles.None;

            //this.Grid.Controls.Add(ExitButton, 1, 0);
            //this.Grid.Controls.Add(new PictureBox() {BackColor = Color.Green }, 0, 0);

            this.FormHandle.Controls.Add(this);
            

            this.BackColor = Color.Transparent;
            this.Visible = false;

            this.KeyDown += PauseMenu_KeyDown;
        }

        public void BringUp()
        {
            this.Visible = true;
            this.Focus();
            this.BringToFront();
        }
        public void HideMenu()
        {
            this.Visible = false;
            this.FormHandle.Focus();
            this.FormHandle.BringToFront();
        }

        private void SetSelection()
        {
            for (int i=0; i<this.RowCount-1; i++)
            {
                ((PictureBox)this.GetControlFromPosition(0, i)).Image = null;
                ((PictureBox)this.GetControlFromPosition(2, i)).Image = null;
            }

            ((PictureBox)this.GetControlFromPosition(0, CurrentSelection)).Image = Properties.Resources.LeftSelectionMarker;
            ((PictureBox)this.GetControlFromPosition(2, CurrentSelection)).Image = Properties.Resources.RightSelectionMarker;
        }

        public void Reposition()
        {
            this.Width = this.FormHandle.Width / 2 - 200;
            this.Height = this.FormHandle.Height / 2;

            this.Top = this.FormHandle.Height / 2 - this.Height / 2;
            this.Left = this.FormHandle.Width / 2 - this.Width / 2;
        }

        private void ConfirmSelection()
        {
            switch (Selections[CurrentSelection])
            {
                case "continue":
                    this.HideMenu();
                    this.FormHandle.MainTimer.Start();
                    this.FormHandle.Focus();
                    break;

                case "exit":
                    this.FormHandle.Close();
                    break;
            }
        }

        private void PauseMenu_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Getting KeyDown in PauseMenuPanel.");
            if (e.KeyCode == Keys.Escape)
            {
                this.HideMenu();
            }

            if (e.KeyCode == Keys.Down)
            {
                CurrentSelection++;
                if (CurrentSelection > MaxSelection)
                    CurrentSelection = 0;
                SetSelection();
            }
            if (e.KeyCode == Keys.Up)
            {
                CurrentSelection--;
                if (CurrentSelection < 0)
                    CurrentSelection = MaxSelection;
                SetSelection();
            }

            if (e.KeyCode == Keys.Enter)
            {
                ConfirmSelection();
            }
        }
    }
}
