using IO_projekt.Properties;
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

        string[] Selections = { "continue", "exit to menu", "exit to desktop" };
        int MaxSelection = 1;
        int CurrentSelection = 0;
        
        public PauseMenuPanel(Form1 fh)
        {        
            FormHandle = fh;            

            MaxSelection = Selections.Length - 1;

            this.ColumnCount = 3;
            this.RowCount = 3;

            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));

            this.RowCount = 1;
            foreach (string s in Selections)
            {
                AddSelection(s);
            }

            this.FormHandle.Controls.Add(this);
            
            this.BackColor = Color.Transparent;

            this.SetSelection();

            this.KeyDown += PauseMenu_KeyDown;

            this.Reposition();
            this.BringToFront();
            this.Focus();
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

        private void AddSelection(string s)
        {
            this.RowCount++;
            this.Controls.Add(new Label() {
                        Font = MenusConfig.DefaultFont,
                        Text = s,
                        ForeColor = Color.White,
                        AutoSize = true,
                        Anchor = AnchorStyles.None,
                }, 1, this.RowCount-2);
            this.Controls.Add(new PictureBox() { Width = 25, Height = 25, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.None }, 0, this.RowCount - 2);
            this.Controls.Add(new PictureBox() { Width = 25, Height = 25, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.None }, 2, this.RowCount - 2);

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
                    this.Dispose();
                    this.FormHandle.MainTimer.Start();
                    Form1.Pause = false;
                    break;

                case "exit to menu":
                    this.Dispose();
                    FormHandle.MainMenu?.Dispose();
                    FormHandle.MainMenu = new MainMenuPanel(FormHandle);
                    break;

                case "exit to desktop":
                    this.FormHandle.Close();
                    break;
            }
        }

        private void PauseMenu_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Getting KeyDown in PauseMenuPanel.");
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
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }     
    }

    public class MainMenuPanel : TableLayoutPanel
    {
        string[] Selections = { "start", "options", "view score", "exit" };
        List<Label> SelectionList;
        Form1 FormHandle;
        int CurrentSelection;
        int MaxSelection = 1;
        public WindowsMediaPlayer menuMedia;

        public MainMenuPanel(Form1 fh)
        {
            FormHandle = fh;
            SelectionList = new List<Label>();
            CurrentSelection = 0;
            MaxSelection = Selections.Length - 1;
            FormHandle.pauseLabel.Visible = false;

            menuMedia = new WindowsMediaPlayer();
            FormHandle.gameMedia.controls.stop();

            
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            File.WriteAllBytes(@"sound\menu_music.wav", Form1.StreamToByteArr(Properties.Resources.menu_music));
            menuMedia.URL = @"sound\menu_music.wav";
            menuMedia.settings.setMode("loop", true);
            menuMedia.settings.volume = 7;

            this.ColumnCount = 3;
            this.RowCount = 2;

            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            

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

            this.RowStyles.Clear();
            this.RowStyles.Add(new RowStyle(SizeType.Absolute, 400));
            
            BackgroundImage = Properties.Resources.menubg2;
        }

        private void SetSelection()
        {
            for (int i = 0; i < this.RowCount - 2; i++)
            {
                ((PictureBox)this.GetControlFromPosition(0, i+1)).Image = null;
                ((PictureBox)this.GetControlFromPosition(2, i+1)).Image = null;
            }

            ((PictureBox)this.GetControlFromPosition(0, CurrentSelection+1)).Image = Properties.Resources.LeftSelectionMarker;
            ((PictureBox)this.GetControlFromPosition(2, CurrentSelection+1)).Image = Properties.Resources.RightSelectionMarker;
        }

        public void Reposition()
        {
            this.Width = this.FormHandle.Width;
            this.Height = this.FormHandle.Height;

            this.Top = 0;
            this.Left = 0;
        }

        private void AddSelection(string text)
        {
            this.Controls.Add(new Label() { Text=Selections[this.RowCount-2], Font=MenusConfig.DefaultFont, ForeColor=Color.White, AutoSize=true, Anchor=AnchorStyles.None, BackColor=Color.Transparent }, 1, this.RowCount-1);
            this.Controls.Add(new PictureBox() { Width = 25, Height = 25, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.Right, BackColor = Color.Transparent }, 0, this.RowCount - 1);
            this.Controls.Add(new PictureBox() { Width = 25, Height = 25, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.Left, BackColor = Color.Transparent }, 2, this.RowCount - 1);

            this.RowCount++;
        }

        async private void ConfirmSelection()
        {
            switch (Selections[CurrentSelection])
            {
                case "start":
                    menuMedia.controls.stop();
                    menuMedia.close();
                    FormHandle.MainTimer.Start();
                    this.Dispose();
                    FormHandle.Focus();
                    FormHandle.StartNewGame();

                    FormHandle.p.Sprite.Left = this.FormHandle.Width / 2 - FormHandle.p.Sprite.Width / 2;
                    FormHandle.p.Sprite.Top = this.FormHandle.Height + FormHandle.p.Sprite.Height;
                    while (FormHandle.p.Sprite.Top > this.FormHandle.Height - FormHandle.p.Sprite.Height - 50)
                    {
                        FormHandle.p.Sprite.Top -= 10;
                        await System.Threading.Tasks.Task.Delay(30);
                    }

                    FormHandle.gameMedia.controls.play();
                    break;

                case "options":
                    new OptionsPanel(FormHandle);
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
        int clrlvl = 0;
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

        Label EscLabel;
        Timer EscLabelTimer;

        public ScoreTable(Form1 fh, Panel p)
        {
            StreamWriter sw;
            string path = @"Scores.txt";
            FormHandle = fh;
            parent = p;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            Scores = new List<ScorePosition>();

            this.ColumnCount = 2;
            this.RowCount = 2;

            this.RowStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            this.BackColor = Color.Black;

            this.Controls.Add(new Label() {Text = "High", Font = new Font("Stencil", 60, FontStyle.Bold), BackColor = Color.Transparent, ForeColor =Color.Red, AutoSize=true, Anchor=AnchorStyles.Right }, 0, 0);
            this.Controls.Add(new Label() {Text = "scores", Font = new Font("Stencil", 60, FontStyle.Bold), BackColor = Color.Transparent, ForeColor = Color.Red, AutoSize = true, Anchor = AnchorStyles.Left }, 1, 0);

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

            EscLabel = new Label() { Text="Press esc to quit", Font = MenusConfig.DefaultFont, ForeColor=Color.White, AutoSize=true, Anchor=AnchorStyles.Left };
            this.FormHandle.Controls.Add(EscLabel);
            EscLabel.BringToFront();

            EscLabelTimer = new Timer() { Interval=600 };
            EscLabelTimer.Tick += new EventHandler((object sender, EventArgs e) =>
                {
                    if (EscLabel.Visible == true)
                        EscLabel.Visible = false;
                    else
                        EscLabel.Visible = true;
                }
            );
            EscLabelTimer.Start();
            this.BackgroundImage = Properties.Resources.scorebg;
        }

        private void AddEntry(int score, string name)
        {
            if(clrlvl == 1)
            {
                this.Controls.Add(new Label() { Text = Convert.ToString(score), Font = new Font("Stencil", 28, FontStyle.Bold), BackColor = Color.Transparent, ForeColor = Color.WhiteSmoke, AutoSize = true, Anchor = AnchorStyles.Right }, 0, this.RowCount - 1);
                this.Controls.Add(new Label() { Text = name, Font = new Font("Stencil", 28, FontStyle.Bold), BackColor = Color.Transparent, ForeColor = Color.WhiteSmoke, AutoSize = true, Anchor = AnchorStyles.Left }, 1, this.RowCount - 1);
                clrlvl = 0;
            }
            else
            {
                this.Controls.Add(new Label() { Text = Convert.ToString(score), Font = new Font("Stencil", 28, FontStyle.Bold), BackColor = Color.Transparent, ForeColor = Color.PowderBlue, AutoSize = true, Anchor = AnchorStyles.Right }, 0, this.RowCount - 1);
                this.Controls.Add(new Label() { Text = name, Font = new Font("Stencil", 28, FontStyle.Bold), BackColor = Color.Transparent, ForeColor = Color.PowderBlue, AutoSize = true, Anchor = AnchorStyles.Left }, 1, this.RowCount - 1);
                clrlvl = 1;
            }
            
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
                this.EscLabelTimer.Stop();
                this.EscLabelTimer.Dispose();
                this.EscLabel.Dispose();
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
            if (e.KeyChar == (char)Keys.Enter && PlayerName != "")
            {
                File.AppendAllText(@"scores.txt", Convert.ToString(Form1.score) + '-' + PlayerName.Replace("\n", "").Replace("\r", "") + '\n');

                this.Dispose();
                FormHandle.MainMenu = new MainMenuPanel(FormHandle);
                Conf.ClearAndDisposeAll();
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Dispose();
                FormHandle.MainMenu = new MainMenuPanel(FormHandle);
                Conf.ClearAndDisposeAll();
            }
            if (e.KeyChar == (char)Keys.Back && PlayerName != "")
            {
                PlayerName = PlayerName.Remove(PlayerName.Length - 1);
            }

             if(PlayerName.Length <= 10 && e.KeyChar != (char)Keys.Back) PlayerName += e.KeyChar;
             NameLabel.Text = PlayerName;
        }
    }

    public class ShopPanel : FlowLayoutPanel
    {
        Panel BlackScreen;

        Form1 FormHandle;
        TableLayoutPanel ShopTablePanel;
        TableLayoutPanel ShopTabTable;
        TableLayoutPanel BottomPanel;
        Label ExitLabel;
        Label ItemDescriptionLabel;
        Label CreditsLabel;
        PictureBox ShopSelectionMarker;

        string[] ShopTabs = { "Bonuses", "Upgrades" };
        ShopEntry[] BonusesSelections = { new ShopEntry("Shield", Properties.Resources.bonusShield, 200),
                                   new ShopEntry("Scatter Gun", Properties.Resources.scatterGun, 1000),
                                   new ShopEntry("Rocket", Properties.Resources.rocket, 250),
                                   new ShopEntry("10 HP", Properties.Resources.bonusHP, 100),
                                   new ShopEntry("Bullet Speed", Properties.Resources.bulletSpeed, 150) };

        ShopEntry[] UpgradesSelections = {new ShopEntry("Rate of Fire+", Properties.Resources.TempPic, 175),
                                   new ShopEntry("Bullet Speed", Properties.Resources.TempPic, 150)
            };

        ShopEntry[] CurrentSelections;

        int CurrentRowSelection = 0;
        int CurrentColumnSelection = 0;
        int MaxRowSelection;
        int MaxColumnSelection;

        private class ShopEntry
        {
            public string Name;
            public Image Img;
            public int Cost;
            public ShopEntry(string n, Image i, int c)
            {
                Name = n;
                Img = i;
                Cost = c;
            }
        }

        private void BringUp()
        {
            FormHandle.p.CloseGunLock();
            this.Left = 0;
        }

        private async void BlackScreenUp()
        {
            BlackScreen = new Panel();
            BlackScreen.BackColor = Color.Black;
            BlackScreen.Width = this.Width + 50;
            BlackScreen.Height = this.Height;
            BlackScreen.Top = 0;
            BlackScreen.Left = -BlackScreen.Width;

            this.FormHandle.Controls.Add(BlackScreen);
            BlackScreen.BringToFront();

            Label ShopLabel = new Label()
            {
                Text = "Shop",
                Font = new Font("Stencil", 50, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            BlackScreen.Controls.Add(ShopLabel);

            while (BlackScreen.Left < 0)
            {
                await System.Threading.Tasks.Task.Delay(10);
                BlackScreen.Left += 50;
            }
           
            ShopLabel.Dispose();
        }
        private async void BlackScreenDown()
        {
            while (BlackScreen.Left < BlackScreen.Width)
            {
                await System.Threading.Tasks.Task.Delay(10);
                BlackScreen.Left += 50;
            }
        }

        public ShopPanel(Form1 fh)
        {
            FormHandle = fh;

            //BlackScreenUp();

            this.FlowDirection = FlowDirection.TopDown;
            
            ShopTablePanel = new TableLayoutPanel();
                      
            ExitLabel = new Label() { Text = "Exit Shop", Font = MenusConfig.DefaultFont, ForeColor=Color.White, AutoSize = true, Anchor=AnchorStyles.None };
            ItemDescriptionLabel = new Label() { Text = "abc", Font = MenusConfig.DefaultFont, ForeColor = Color.White, AutoSize = true, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter };
            CreditsLabel = new Label() { Text = $"Credits:\n${FormHandle.p.Credits}", Font = MenusConfig.DefaultFont, ForeColor = Color.White, AutoSize = true, Anchor = AnchorStyles.None, TextAlign=ContentAlignment.MiddleCenter };

            ShopTabTable = new TableLayoutPanel();
            ShopTabTable.ColumnStyles.Clear();
            ShopTabTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            ShopTabTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            ShopTabTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            ShopTabTable.RowCount = 1;
            ShopTabTable.ColumnCount = 3;
            ShopTabTable.Controls.Add(new PictureBox() { Width = Height = 40, BackColor = Color.Transparent, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.None }, 0, 0);
            ShopTabTable.Controls.Add(new Label() { Text = "abc", Font = MenusConfig.DefaultFont, ForeColor = Color.White, AutoSize = true, Anchor = AnchorStyles.None }, 1, 0);
            ShopTabTable.Controls.Add(new PictureBox() { Width = Height = 40, BackColor = Color.Transparent, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.None }, 2, 0);

            BottomPanel = new TableLayoutPanel();
            BottomPanel.RowCount = 1;
            BottomPanel.ColumnCount = 3;
            BottomPanel.ColumnStyles.Clear();
            BottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            BottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            BottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            BottomPanel.Controls.Add(ExitLabel, 0, 0);
            BottomPanel.Controls.Add(CreditsLabel, 1, 0);
            BottomPanel.Controls.Add(ItemDescriptionLabel, 2, 0);
            ((PictureBox)ShopTabTable.GetControlFromPosition(2, 0)).Image = Properties.Resources.LeftSelectionMarker;

            this.Reposition();

            this.ShopTablePanel.ColumnCount = 4;
            this.ShopTablePanel.RowCount = 1;

            this.ShopTablePanel.ColumnStyles.Clear();
            this.ShopTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            this.ShopTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            this.ShopTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            this.ShopTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            this.ShopTablePanel.RowStyles.Clear();
            this.ShopTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, this.ShopTablePanel.Width / this.ShopTablePanel.ColumnCount));

            LoadBonusesTab();
            this.ShopTablePanel.RowCount++;

            FormHandle.Controls.Add(this);
            this.Controls.Add(ShopTabTable);
            this.Controls.Add(ShopTablePanel);
            this.Controls.Add(BottomPanel);
            this.BringToFront();

            this.KeyDown += ShopPanel_KeyPress;
            this.Focus();

            this.SetSelection();
            Console.WriteLine($"Max COL: {MaxColumnSelection}");
            Console.WriteLine($"Max ROW: {MaxRowSelection}");
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            BackColor = Color.Transparent;
            BackgroundImage = Resources.scorebg;

            this.BringUp();
            //BlackScreenDown();

        }
        public void Reposition()
        {
            this.Width = FormHandle.Width;
            this.Height = FormHandle.Height;

            ShopTabTable.Width = this.Width;
            BottomPanel.Width = this.Width;

            this.ShopTablePanel.Width = this.Width;
            this.ShopTablePanel.Height = this.Height - BottomPanel.Height - ShopTabTable.Height - 50;

            this.ShopTablePanel.RowStyles.Clear();
            for (int i=0; i<this.ShopTablePanel.RowCount; i++)
                this.ShopTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        }
        private void AddBonusesSelections()
        {
            this.ShopTablePanel.RowCount = 1;

            int ColumnIndex = 0;
            foreach (ShopEntry s in BonusesSelections)
            {
                Console.WriteLine($"Adding selection {s.Name}");

                this.ShopTablePanel.Controls.Add(new PictureBox()
                {
                    Image = s.Img,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                    Width = 150,
                    Height = 150,
                    Anchor = AnchorStyles.None
                },
                    ColumnIndex++, this.ShopTablePanel.RowCount - 1);

                if (ColumnIndex >= 4)
                {
                    ColumnIndex = 0;
                    this.ShopTablePanel.RowCount++;
                    this.ShopTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, this.ShopTablePanel.Width / this.ShopTablePanel.ColumnCount));
                }

                this.ShopTablePanel.RowStyles.Clear();
                for (int i=0; i<this.ShopTablePanel.RowCount; i++)
                    this.ShopTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, this.ShopTablePanel.Width / this.ShopTablePanel.ColumnCount));
            }

            MaxColumnSelection = this.ShopTablePanel.ColumnCount - 1;
            MaxRowSelection = this.ShopTablePanel.RowCount - 1;
        }
        private void AddUpgradesSelections()
        {
            this.ShopTablePanel.RowCount = 1;

            int ColumnIndex = 0;
            foreach (ShopEntry s in UpgradesSelections)
            {
                Console.WriteLine($"Adding selection {s.Name}");

                this.ShopTablePanel.Controls.Add(new PictureBox()
                {
                    Image = s.Img,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                    Width = 70,
                    Height = 70,
                    Anchor = AnchorStyles.None
                },
                    ColumnIndex++, this.ShopTablePanel.RowCount - 1);

                if (ColumnIndex >= 4)
                {
                    ColumnIndex = 0;
                    this.ShopTablePanel.RowCount++;
                    this.ShopTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, this.ShopTablePanel.Width / this.ShopTablePanel.ColumnCount));
                }

                this.ShopTablePanel.RowStyles.Clear();
                for (int i = 0; i < this.ShopTablePanel.RowCount; i++)
                    this.ShopTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, this.ShopTablePanel.Width / this.ShopTablePanel.ColumnCount));
            }

            MaxColumnSelection = this.ShopTablePanel.ColumnCount - 1;
            MaxRowSelection = this.ShopTablePanel.RowCount - 1;
        }

        private void SetSelection()
        {
            ShopSelectionMarker?.Dispose();
            ShopSelectionMarker = new PictureBox() { Image=Properties.Resources.ShopSelectionMarkerPic, BackColor=Color.Transparent, SizeMode=PictureBoxSizeMode.StretchImage };

            if (CurrentRowSelection <= -1)
                ShopSelectionMarker.Parent = ShopTabTable.GetControlFromPosition(1, 0);
            else if(CurrentRowSelection > MaxRowSelection)
                ShopSelectionMarker.Parent = BottomPanel.GetControlFromPosition(0, 0);
            else
                ShopSelectionMarker.Parent = ShopTablePanel.GetControlFromPosition(CurrentColumnSelection, CurrentRowSelection); ;

            ShopSelectionMarker.Width = ShopSelectionMarker.Parent.Width;
            ShopSelectionMarker.Height = ShopSelectionMarker.Parent.Height;

            if (CurrentRowSelection >= 0 && CurrentRowSelection <= MaxRowSelection)
            {
                ItemDescriptionLabel.Text = $"{CurrentSelections[CurrentColumnSelection + this.ShopTablePanel.ColumnCount * CurrentRowSelection].Name}\n${CurrentSelections[CurrentColumnSelection + this.ShopTablePanel.ColumnCount * CurrentRowSelection].Cost}";
            }
            else
                ItemDescriptionLabel.Text = "";
        }

        private void ConfirmSelection()
        {
            if (CurrentRowSelection < 0)
                return;

            if (CurrentRowSelection > MaxRowSelection)
            {
                FormHandle.NextLevel();
                this.Visible = false;
                this.DisposeTabs();
                this.Dispose();
                
                return;
            }

            ShopEntry SelectedItem = CurrentSelections[CurrentColumnSelection + this.ShopTablePanel.ColumnCount * CurrentRowSelection];

            if (FormHandle.p.Credits < SelectedItem.Cost)
            {
                return;
            }

            switch (SelectedItem.Name)
            {
                case "10 HP":
                    if (Form1.hp < 100)
                    {
                        Form1.hp += 10;
                        if (Form1.hp > 100)
                        {
                            Form1.hp = 100;
                        }
                    }
                    else
                        return;
                    break;

                case "Shield":
                    if (FormHandle.p.shielded)
                        return;
                    else
                    {
                        FormHandle.p.shielded = true;
                    }
                    break;

                case "Scatter Gun":
                    if (FormHandle.p.ScatterGun)
                        return;
                    else
                    {
                        FormHandle.p.ScatterGun = true;
                    }
                    break;

                case "Bullet Speed":
                    if (FormHandle.p.bulletSpeedIncreased)
                    {
                        FormHandle.p.bulletSpeed = 20;
                        FormHandle.p.bulletSpeedIncreased = true;
                    }
                    else
                        return;
                    break;
            }

            FormHandle.p.Credits -= SelectedItem.Cost;
            CreditsLabel.Text = $"Credits:\n${FormHandle.p.Credits}";
        }

        private void ChangeTab()
        {
            switch (((Label)ShopTabTable.GetControlFromPosition(1, 0)).Text)
            {
                case "Bonuses":
                    LoadUpgradesTab();
                    break;

                case "Upgrades":
                    LoadBonusesTab();
                    break;
            }
        }

        private void LoadBonusesTab()
        {
            CurrentSelections = BonusesSelections;

            ((Label)ShopTabTable.GetControlFromPosition(1, 0)).Text = "Bonuses";
            ((PictureBox)ShopTabTable.GetControlFromPosition(0, 0)).Image = null;
            ((PictureBox)ShopTabTable.GetControlFromPosition(2, 0)).Image = Properties.Resources.LeftSelectionMarker;

            while (ShopTablePanel.Controls.Count > 0)
                ShopTablePanel.Controls[0].Dispose();

            AddBonusesSelections();

            CurrentColumnSelection = 0;
            CurrentRowSelection = 0;
            SetSelection();
        }
        private void LoadUpgradesTab()
        {
            CurrentSelections = UpgradesSelections;

            ((Label)ShopTabTable.GetControlFromPosition(1, 0)).Text = "Upgrades";
            ((PictureBox)ShopTabTable.GetControlFromPosition(0, 0)).Image = Properties.Resources.RightSelectionMarker;
            ((PictureBox)ShopTabTable.GetControlFromPosition(2, 0)).Image = null;

            while (ShopTablePanel.Controls.Count > 0)
                ShopTablePanel.Controls[0].Dispose();

            AddUpgradesSelections();

            CurrentColumnSelection = 0;
            CurrentRowSelection = 0;
            SetSelection();
        }

        private void DisposeTabs()
        {
            ShopTabTable.Dispose();
            ShopTablePanel.Dispose();
            BottomPanel.Dispose();
        }

        private void ShopPanel_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ConfirmSelection();
            }

            if(e.KeyCode == Keys.Left && CurrentRowSelection <= MaxRowSelection)
            {
                if (CurrentRowSelection < 0)
                {
                    ChangeTab();
                    return;
                }

                if (CurrentColumnSelection > 0 && ShopTablePanel.GetControlFromPosition(CurrentColumnSelection - 1, CurrentRowSelection) != null)
                {
                    CurrentColumnSelection--;
                    SetSelection();
                }
            }
                

            if (e.KeyCode == Keys.Right && CurrentRowSelection <= MaxRowSelection)
            {
                if (CurrentRowSelection < 0)
                {
                    ChangeTab();
                    return;
                }

                if (CurrentColumnSelection < MaxColumnSelection && ShopTablePanel.GetControlFromPosition(CurrentColumnSelection + 1, CurrentRowSelection) != null)
                {
                    CurrentColumnSelection++;
                    SetSelection();
                }
            }
                

            if (e.KeyCode == Keys.Down)
            {
                if (CurrentRowSelection > MaxRowSelection)
                    return;

                if (CurrentRowSelection+1 > MaxRowSelection)
                {
                    CurrentRowSelection++;
                    SetSelection();
                    return;
                }

                if (CurrentRowSelection < MaxRowSelection + 1 && ShopTablePanel.GetControlFromPosition(CurrentColumnSelection, CurrentRowSelection + 1) != null)
                {
                    CurrentRowSelection++;
                    SetSelection();
                }
            }


            if (e.KeyCode == Keys.Up)
            {
                if (CurrentRowSelection <= -1)
                    return;

                if (CurrentRowSelection-1 < 0)
                {
                    CurrentRowSelection--;
                    SetSelection();
                    return;
                }


                if (CurrentRowSelection > 0 && ShopTablePanel.GetControlFromPosition(CurrentColumnSelection, CurrentRowSelection - 1) != null)
                {
                    CurrentRowSelection--;
                    SetSelection();
                }
            }

            Console.WriteLine($"COL: {CurrentColumnSelection}");
            Console.WriteLine($"ROW: {CurrentRowSelection}");
            
        }
    }

    public class OptionsPanel : FlowLayoutPanel
    {
        Form1 FormHandle;
        TableLayoutPanel TopPanel;
        TableLayoutPanel MainPanel;
        PictureBox TempMarker;

        string[] SoundSelections = { "master", "music", "sound" };
        string[] DisplaySelections = { "show fps", "ui scale" };
        int CurrentSelectionInd = -1;
        int MaxSelectionInd;
        string CurrentSelection;

        Dictionary<string, string[]> OptionsTabs;
        int CurrentTabInd;
        int MaxTabInd;

        int MaxRow = 0;

        public OptionsPanel(Form1 fh)
        {
            this.FlowDirection = FlowDirection.TopDown;

            FormHandle = fh;
            OptionsTabs = new Dictionary<string, string[]>();
            OptionsTabs.Add("sound", SoundSelections);
            OptionsTabs.Add("display", DisplaySelections);
            MaxTabInd = 1;

            CurrentTabInd = 0;
            SetupTables();
            FormHandle.Controls.Add(this);
            Reposition();
            ReloadTab();
            
            KeyDown += OptionsPanel_KeyDown;
            this.BringToFront();
            this.Focus();

            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BackColor = Color.Transparent;
            this.BackgroundImage = Resources.scorebg2;
        }

        private void SetSelection(int PrevInd = -2)
        {
            if (PrevInd >= 0)
            {
                ((PictureBox)MainPanel.GetControlFromPosition(0, PrevInd)).Image = null;
                ((PictureBox)MainPanel.GetControlFromPosition(2, PrevInd)).Image = null;
            }

            if (CurrentSelectionInd == -1)
            {
                TempMarker = new PictureBox() {
                    Parent = TopPanel.GetControlFromPosition(1, 0),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = Properties.Resources.ShopSelectionMarkerPic,
                    BackColor = Color.Transparent
                };
                TempMarker.Width = TempMarker.Parent.Width;
                TempMarker.Height = TempMarker.Parent.Height;

                CurrentSelection = "";
                return;
            }

            TempMarker?.Dispose();

            ((PictureBox)MainPanel.GetControlFromPosition(0, CurrentSelectionInd)).Image = Properties.Resources.LeftSelectionMarker;
            ((PictureBox)MainPanel.GetControlFromPosition(2, CurrentSelectionInd)).Image = Properties.Resources.RightSelectionMarker;
            CurrentSelection = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][CurrentSelectionInd];
            Console.WriteLine($"Selection set to {CurrentSelection}");
        }

        private void ReloadTab()
        {
            TempMarker?.Dispose();

            ((Label)TopPanel.GetControlFromPosition(1, 0)).Text = OptionsTabs.Keys.ToArray()[CurrentTabInd];

            if (CurrentTabInd == 0)
            {
                ((Label)TopPanel.GetControlFromPosition(0, 0)).Text = "";
                ((Label)TopPanel.GetControlFromPosition(2, 0)).Text = OptionsTabs.Keys.ToArray()[CurrentTabInd + 1];
            }
            else if (CurrentTabInd == MaxTabInd)
            {
                ((Label)TopPanel.GetControlFromPosition(0, 0)).Text = OptionsTabs.Keys.ToArray()[CurrentTabInd - 1];
                ((Label)TopPanel.GetControlFromPosition(2, 0)).Text = "";
            }

            for (int i=0; i<MainPanel.RowCount; i++)
            {
                ((Label)MainPanel.GetControlFromPosition(1, i)).Text = "";
            }

            MaxRow = 0;
            MainPanel.RowStyles.Clear();
            foreach (string s in OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]])
            {
                AddMainPanelEntry(s);
            }
            MaxSelectionInd = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]].Length-1;
            CurrentSelectionInd = -1;

            UpdateAllSelectionText();

            SetSelection();
            
        }

        private void UpdateCurrentSelectionText()
        {
            switch (OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][CurrentSelectionInd])
            {
                case "show fps":
                    if (Properties.Settings.Default.ShowFPS)
                        ((Label)MainPanel.GetControlFromPosition(1, CurrentSelectionInd)).Text = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][CurrentSelectionInd] + ": on";
                    else
                        ((Label)MainPanel.GetControlFromPosition(1, CurrentSelectionInd)).Text = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][CurrentSelectionInd] + ": off";
                    return;

                case "master":
                    ((Label)MainPanel.GetControlFromPosition(1, CurrentSelectionInd)).Text = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][CurrentSelectionInd] + ": " + Convert.ToString(Properties.Settings.Default.VolMaster);
                    break;

                case "music":
                    ((Label)MainPanel.GetControlFromPosition(1, CurrentSelectionInd)).Text = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][CurrentSelectionInd] + ": " + Convert.ToString(Properties.Settings.Default.VolMusic);
                    break;

                case "sound":
                    ((Label)MainPanel.GetControlFromPosition(1, CurrentSelectionInd)).Text = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][CurrentSelectionInd] + ": " + Convert.ToString(Properties.Settings.Default.VolSound);
                    break;
            }
        }

        private void UpdateAllSelectionText()
        {
            for (int i=0; i<MaxRow; i++)
            {
                switch (OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][i])
                {
                    case "show fps":
                        if (Properties.Settings.Default.ShowFPS)
                            ((Label)MainPanel.GetControlFromPosition(1, i)).Text = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][i] + ": on";
                        else
                            ((Label)MainPanel.GetControlFromPosition(1, i)).Text = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][i] + ": off";
                        break;

                    case "master":
                        ((Label)MainPanel.GetControlFromPosition(1, i)).Text = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][i] + ": " + Convert.ToString(Properties.Settings.Default.VolMaster);
                        break;

                    case "music":
                        ((Label)MainPanel.GetControlFromPosition(1, i)).Text = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][i] + ": " + Convert.ToString(Properties.Settings.Default.VolMusic);
                        break;

                    case "sound":
                        ((Label)MainPanel.GetControlFromPosition(1, i)).Text = OptionsTabs[OptionsTabs.Keys.ToArray()[CurrentTabInd]][i] + ": " + Convert.ToString(Properties.Settings.Default.VolSound);
                        break;
                }
            }
        }

        private void OptionsPanel_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Left)
            {
                if (CurrentSelectionInd < 0)
                {
                    if (CurrentTabInd > 0)
                    {
                        CurrentTabInd--;
                        ReloadTab();
                    }
                }
                else
                {
                    switch (CurrentSelection)
                    {
                        case "show fps":
                            Properties.Settings.Default.ShowFPS = !Properties.Settings.Default.ShowFPS;
                            Properties.Settings.Default.Save();
                            UpdateCurrentSelectionText();
                            break;

                        case "master":
                            if (Properties.Settings.Default.VolMaster > 0)
                            {
                                Properties.Settings.Default.VolMaster--;
                                Properties.Settings.Default.Save();
                                UpdateCurrentSelectionText();
                                UpdateAudioLevels();
                            }
                            break;

                        case "music":
                            if (Properties.Settings.Default.VolMusic > 0)
                            {
                                Properties.Settings.Default.VolMusic--;
                                Properties.Settings.Default.Save();
                                UpdateCurrentSelectionText();
                                UpdateAudioLevels();
                            }
                            break;

                        case "sound":
                            if (Properties.Settings.Default.VolSound > 0)
                            {
                                Properties.Settings.Default.VolSound--;
                                Properties.Settings.Default.Save();
                                UpdateCurrentSelectionText();
                                UpdateAudioLevels();
                            }
                            break;
                    }
                }
            }

            if (e.KeyCode == Keys.Right)
            {
                if (CurrentSelectionInd < 0)
                {
                    if (CurrentTabInd < MaxTabInd)
                    {
                        CurrentTabInd++;
                        ReloadTab();
                    }
                }
                else
                {
                    switch (CurrentSelection)
                    {
                        case "show fps":
                            Properties.Settings.Default.ShowFPS = !Properties.Settings.Default.ShowFPS;
                            Properties.Settings.Default.Save();
                            UpdateCurrentSelectionText();
                            break;

                        case "master":
                            if (Properties.Settings.Default.VolMaster < 10)
                            {
                                Properties.Settings.Default.VolMaster++;
                                Properties.Settings.Default.Save();
                                UpdateCurrentSelectionText();
                                UpdateAudioLevels();
                            }
                            break;

                        case "music":
                            if (Properties.Settings.Default.VolMusic < 10)
                            {
                                Properties.Settings.Default.VolMusic++;
                                Properties.Settings.Default.Save();
                                UpdateCurrentSelectionText();
                                UpdateAudioLevels();
                            }
                            break;

                        case "sound":
                            if (Properties.Settings.Default.VolSound < 10)
                            {
                                Properties.Settings.Default.VolSound++;
                                Properties.Settings.Default.Save();
                                UpdateCurrentSelectionText();
                                UpdateAudioLevels();
                            }
                            break;
                    }
                }
            }

            if (e.KeyCode == Keys.Up)
            {
                if (CurrentSelectionInd > -1)
                {
                    CurrentSelectionInd--;
                    SetSelection(CurrentSelectionInd+1);
                }
            }

            if (e.KeyCode == Keys.Down)
            {
                if (CurrentSelectionInd < MaxSelectionInd)
                {
                    CurrentSelectionInd++;
                    SetSelection(CurrentSelectionInd-1);
                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.FormHandle.MainMenu.Focus();
                this.Dispose();
            }
        }

        private void Reposition()
        {
            this.Width = FormHandle.Width;
            this.Height = FormHandle.Height;

            TopPanel.Width = this.Width;
            MainPanel.Width = this.Width;


            Console.WriteLine($"Current MainPanel.Height = {MainPanel.Height}");
        }

        private void AddMainPanelEntry(string s)
        {
            MaxRow++;
            Console.WriteLine($"Current MainPanel.RowCount = {MainPanel.RowCount}");


            ((Label)MainPanel.GetControlFromPosition(1, MaxRow - 1)).Text = s + ": ";


            if (s == "show fps")
            {
                if (Properties.Settings.Default.ShowFPS)
                    ((Label)MainPanel.GetControlFromPosition(1, MaxRow - 1)).Text += "on";
                else
                    ((Label)MainPanel.GetControlFromPosition(1, MaxRow - 1)).Text += "off";
            }
        }

        private void UpdateAudioLevels()
        {
            FormHandle.gameMedia.settings.volume = Properties.Settings.Default.VolMaster * Properties.Settings.Default.VolMusic;
            FormHandle.MainMenu.menuMedia.settings.volume = Properties.Settings.Default.VolMaster * Properties.Settings.Default.VolMusic;
            FormHandle.bonusMedia.settings.volume = Properties.Settings.Default.VolMaster * Properties.Settings.Default.VolSound;
            FormHandle.shootMedia.settings.volume = Properties.Settings.Default.VolMaster * Properties.Settings.Default.VolSound;
        }

        private void SetupTables()
        {
            TopPanel = new TableLayoutPanel();
            TopPanel.RowCount = 1;
            TopPanel.ColumnCount = 3;
            TopPanel.ColumnStyles.Clear();
            TopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            TopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            TopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            TopPanel.Controls.Add(new Label() { Text = "", Font = new Font("Stencil", 18, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Anchor = AnchorStyles.None }, 0, 0);
            TopPanel.Controls.Add(new Label() { Text = "", Font = new Font("Stencil", 36, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Anchor = AnchorStyles.None }, 1, 0);
            TopPanel.Controls.Add(new Label() { Text = "sad", Font = new Font("Stencil", 18, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Anchor = AnchorStyles.None }, 2, 0);
            this.Controls.Add(TopPanel);
            TopPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            TopPanel.BackColor = Color.Transparent;
            TopPanel.BackgroundImage = Properties.Resources.scorebg;

            SetupMainTable();

            SetSelection();
        }

        private void SetupMainTable()
        {
            MainPanel?.Dispose();
            MainPanel = new TableLayoutPanel();
            MainPanel.RowCount = 1;
            MainPanel.ColumnCount = 3;
            MainPanel.ColumnStyles.Clear();
            MainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            MainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            MainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            this.Controls.Add(MainPanel);


            MainPanel.RowCount = 4;
            for (int i = 0; i < MainPanel.RowCount; i++)
            { 
                MaxRow++;
                MainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
                MainPanel.Height += 70;
                MainPanel.Controls.Add(new PictureBox() { Width = 60, Height = 60, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.Right}, 0, i);
                MainPanel.Controls.Add(new Label() { Text = "", Font = MenusConfig.DefaultFont, ForeColor = Color.White, AutoSize = true, Anchor = AnchorStyles.None }, 1, i);
                MainPanel.Controls.Add(new PictureBox() { Width = 60, Height = 60, SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.Left }, 2, i);
            }
            
        }
    }
}
