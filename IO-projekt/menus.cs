using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WMPLib;

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

            

            this.ColumnCount = 3;
            this.RowCount = 3;

            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));

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

            this.SetSelection();
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

    public class MainMenuPanel : TableLayoutPanel
    {
        string[] Selections = { "start", "view score", "exit" };
        List<Label> SelectionList;
        Form1 FormHandle;
        int CurrentSelection;
        int MaxSelection = 2;
        public WindowsMediaPlayer menuMedia;

        public MainMenuPanel(Form1 fh)
        {
            FormHandle = fh;
            SelectionList = new List<Label>();
            CurrentSelection = 0;

            menuMedia = new WindowsMediaPlayer();

            File.WriteAllBytes(@"sound\menu_music.wav", Form1.StreamToByteArr(Properties.Resources.menu_music));
            menuMedia.URL = @"sound\menu_music.wav";
            menuMedia.settings.setMode("loop", true);
            menuMedia.settings.volume = 7;

            this.ColumnCount = 3;
            this.RowCount = 1;

            //this.BackColor = Color.DarkRed;

            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));

            foreach (string s in Selections)
            {
                this.AddSelection(s);
            }

            this.SetSelection();

            FormHandle.Controls.Add(this);

            this.Visible = true;
            this.BringToFront();
            this.Focus();

            this.Reposition();

            this.KeyDown += MainMenu_KeyDown;
        }

        private void SetSelection()
        {
            for (int i = 0; i < this.RowCount - 1; i++)
            {
                ((PictureBox)this.GetControlFromPosition(0, i)).Image = null;
                ((PictureBox)this.GetControlFromPosition(2, i)).Image = null;
            }

            ((PictureBox)this.GetControlFromPosition(0, CurrentSelection)).Image = Properties.Resources.LeftSelectionMarker;
            ((PictureBox)this.GetControlFromPosition(2, CurrentSelection)).Image = Properties.Resources.RightSelectionMarker;
        }

        public void Reposition()
        {
            this.Width = this.FormHandle.Width / 2;
            this.Height = this.FormHandle.Height / 2;

            this.Top = this.FormHandle.Height / 2 - this.Height / 2;
            this.Left = this.FormHandle.Width / 2 - this.Width / 2;
        }

        private void AddSelection(string text)
        {
            this.Controls.Add(new Label() { Text=Selections[this.RowCount-1], Font=MenusConfig.DefaultFont, ForeColor=Color.White, AutoSize=true, Anchor=AnchorStyles.None }, 1, this.RowCount-1);
            this.Controls.Add(new PictureBox() { Width = 25, Height = 25, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.None }, 0, this.RowCount - 1);
            this.Controls.Add(new PictureBox() { Width = 25, Height = 25, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.None }, 2, this.RowCount - 1);

            this.RowCount++;
        }

        async private void ConfirmSelection()
        {
            switch (Selections[CurrentSelection])
            {
                case "start":
                    menuMedia.controls.stop();
                    FormHandle.MainTimer.Start();
                    this.Dispose();
                    FormHandle.Focus();

                    FormHandle.p.Sprite.Left = this.FormHandle.Width / 2 - FormHandle.p.Sprite.Width / 2;
                    FormHandle.p.Sprite.Top = this.FormHandle.Height + FormHandle.p.Sprite.Height;
                    while (FormHandle.p.Sprite.Top > this.FormHandle.Height - FormHandle.p.Sprite.Height - 50)
                    {
                        FormHandle.p.Sprite.Top -= 10;
                        await System.Threading.Tasks.Task.Delay(30);
                    }

                    FormHandle.gameMedia.controls.play();
                    break;

                case "view score":
                    ScoreTable st = new ScoreTable(FormHandle, this);
                    break;

                case "exit":
                    FormHandle.Close();
                    break;
            }
        }

        private void MainMenu_KeyDown(object sender, KeyEventArgs e)
        {
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

    public class ScoreTable : TableLayoutPanel
    {
        Form1 FormHandle;
        Panel parent;
        string[] FileContent;
        private class ScorePosition
        {
            public int Score;
            public string Name;

            public ScorePosition(int s, string n)
            {
                Score = s;
                Name = n;
            }
        }
        List<ScorePosition> Scores;

        public ScoreTable(Form1 fh, Panel p)
        {
            StreamWriter sw;
            string path = @"Scores.txt";
            FormHandle = fh;
            parent = p;
            Scores = new List<ScorePosition>();

            this.ColumnCount = 2;
            this.RowCount = 2;
            //this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;

            this.RowStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            this.BackColor = Color.Black;

            this.Controls.Add(new Label() {Text = "High", Font=MenusConfig.DefaultFont, ForeColor=Color.White, AutoSize=true, Anchor=AnchorStyles.Right }, 0, 0);
            this.Controls.Add(new Label() {Text = "scores", Font = MenusConfig.DefaultFont, ForeColor = Color.White, AutoSize = true, Anchor = AnchorStyles.Left }, 1, 0);

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
                Console.WriteLine("plik został utworzony");
                sw.Close();
            }

            FileContent = File.ReadAllLines(@"scores.txt");
            foreach (string s in FileContent)
            {
                Console.WriteLine("LINE: " + s);

                string[] line = s.Split('-');
                Scores.Add(new ScorePosition(Convert.ToInt32(line[0]), line[1]));
            }

            List<ScorePosition> SortedScores = Scores.OrderByDescending(o => o.Score).ToList();
            foreach (ScorePosition se in SortedScores)
            {
                this.AddEntry(se.Score, se.Name);
            }

            FormHandle.Controls.Add(this);
            this.Visible = true;
            this.BringToFront();
            this.Focus();
            this.Reposition();

            this.KeyPress += ScoreTable_KeyPress;
        }

        private void AddEntry(int score, string name)
        {
            this.Controls.Add(new Label() { Text = Convert.ToString(score), Font = new Font("Stencil", 26, FontStyle.Bold), ForeColor=Color.White, AutoSize=true, Anchor=AnchorStyles.Right }, 0, this.RowCount - 1);
            this.Controls.Add(new Label() { Text = name,Font = new Font("Stencil", 26, FontStyle.Bold), ForeColor = Color.DodgerBlue, AutoSize=true, Anchor=AnchorStyles.Left }, 1, this.RowCount - 1);

            this.RowCount++;
        }

        public void Reposition()
        {
            this.Width = this.FormHandle.Width;
            this.Height = this.FormHandle.Height;
            this.Top = 0;
            this.Left = 0;
        }

        private void ScoreTable_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                parent.Focus();
                this.Dispose();
            }
        }
    }

    public class ScoreEntry : FlowLayoutPanel
    {
        Form1 FormHandle;
        string PlayerName = "";
        Label NameLabel;

        public ScoreEntry(Form1 fh)
        {
            FormHandle = fh;

            FormHandle.Controls.Add(this);

            this.BackColor = Color.DarkRed;
            this.AutoSize = true;
            this.BorderStyle = BorderStyle.Fixed3D;

            this.FlowDirection = FlowDirection.TopDown;
            this.Controls.Add(new Label() { Text = "Your score:", Font = MenusConfig.DefaultFont, ForeColor = Color.White, AutoSize=true, Anchor = AnchorStyles.None });
            this.Controls.Add(new Label() { Text = Convert.ToString(Form1.score), Font = MenusConfig.DefaultFont, ForeColor = Color.White, AutoSize = true, Anchor= AnchorStyles.None});
            this.Controls.Add(new Label() { Text = "Enter your name:", Font = MenusConfig.DefaultFont, ForeColor = Color.White, AutoSize = true, Anchor = AnchorStyles.None });

            NameLabel = new Label();
            NameLabel.Font = MenusConfig.DefaultFont;
            NameLabel.ForeColor = Color.White;
            NameLabel.AutoSize = true;
            NameLabel.Anchor = AnchorStyles.None;
            this.Controls.Add(NameLabel);
            

            this.KeyPress += ScoreEntry_KeyDown;
            this.Focus();
            this.Reposition();
            this.Visible = true;
            this.BringToFront();
        }

        public void Reposition()
        {
            this.Top = this.FormHandle.Height / 2 - this.Height / 2;
            this.Left = this.FormHandle.Width / 2 - this.Width / 2;
        }

        private void ScoreEntry_KeyDown(object sender, KeyPressEventArgs e)
        {         
            if (e.KeyChar == (char)Keys.Enter)
            {
                File.AppendAllText(@"scores.txt", Convert.ToString(Form1.score) + '-' + PlayerName + '\n');

                this.Dispose();
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Dispose();
            }

             PlayerName += e.KeyChar;
             NameLabel.Text = PlayerName;
        }
    }
}
