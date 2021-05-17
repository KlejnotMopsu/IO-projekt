using System.Windows.Forms;

namespace IO_projekt
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pauseLabel = new System.Windows.Forms.Label();
            this.Replaybtn = new System.Windows.Forms.Button();
            this.Exitbtn = new System.Windows.Forms.Button();
            this.Scorelbl = new System.Windows.Forms.Label();
            this.Pointslbl = new System.Windows.Forms.Label();
            this.Lifelbl = new System.Windows.Forms.Label();
            this.LifePointslbl = new System.Windows.Forms.Label();
            this.ScoreView = new System.Windows.Forms.ListView();
            this.Scorebtn = new System.Windows.Forms.Button();
            this.ClScorebtn = new System.Windows.Forms.Button();
            this.Playbtn = new System.Windows.Forms.Button();
            this.scoreMultiplierTimeLabel = new System.Windows.Forms.Label();
            this.doubleShootTimeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pauseLabel
            // 
            this.pauseLabel.AutoSize = true;
            this.pauseLabel.BackColor = System.Drawing.Color.Transparent;
            this.pauseLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pauseLabel.Font = new System.Drawing.Font("Stencil", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pauseLabel.ForeColor = System.Drawing.Color.Red;
            this.pauseLabel.Location = new System.Drawing.Point(441, 50);
            this.pauseLabel.Name = "pauseLabel";
            this.pauseLabel.Size = new System.Drawing.Size(433, 142);
            this.pauseLabel.TabIndex = 0;
            this.pauseLabel.Text = "Pause";
            this.pauseLabel.Visible = false;
            // 
            // Replaybtn
            // 
            this.Replaybtn.BackColor = System.Drawing.Color.MintCream;
            this.Replaybtn.Font = new System.Drawing.Font("Stencil", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Replaybtn.Location = new System.Drawing.Point(445, 226);
            this.Replaybtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Replaybtn.Name = "Replaybtn";
            this.Replaybtn.Size = new System.Drawing.Size(221, 82);
            this.Replaybtn.TabIndex = 1;
            this.Replaybtn.Text = "replay";
            this.Replaybtn.UseVisualStyleBackColor = false;
            this.Replaybtn.Visible = false;
            this.Replaybtn.Click += new System.EventHandler(this.Replaybtn_Click);
            // 
            // Exitbtn
            // 
            this.Exitbtn.BackColor = System.Drawing.Color.MintCream;
            this.Exitbtn.Font = new System.Drawing.Font("Stencil", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Exitbtn.Location = new System.Drawing.Point(677, 226);
            this.Exitbtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Exitbtn.Name = "Exitbtn";
            this.Exitbtn.Size = new System.Drawing.Size(221, 82);
            this.Exitbtn.TabIndex = 2;
            this.Exitbtn.Text = "exit";
            this.Exitbtn.UseVisualStyleBackColor = false;
            this.Exitbtn.Visible = false;
            this.Exitbtn.Click += new System.EventHandler(this.Exitbtn_Click);
            // 
            // Scorelbl
            // 
            this.Scorelbl.BackColor = System.Drawing.Color.Transparent;
            this.Scorelbl.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Scorelbl.ForeColor = System.Drawing.Color.Gainsboro;
            this.Scorelbl.Location = new System.Drawing.Point(13, 14);
            this.Scorelbl.Name = "Scorelbl";
            this.Scorelbl.Size = new System.Drawing.Size(125, 34);
            this.Scorelbl.TabIndex = 3;
            this.Scorelbl.Text = "Score:";
            // 
            // Pointslbl
            // 
            this.Pointslbl.BackColor = System.Drawing.Color.Transparent;
            this.Pointslbl.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pointslbl.ForeColor = System.Drawing.Color.Gainsboro;
            this.Pointslbl.Location = new System.Drawing.Point(132, 14);
            this.Pointslbl.Name = "Pointslbl";
            this.Pointslbl.Size = new System.Drawing.Size(247, 34);
            this.Pointslbl.TabIndex = 4;
            this.Pointslbl.Text = "0";
            // 
            // Lifelbl
            // 
            this.Lifelbl.BackColor = System.Drawing.Color.Transparent;
            this.Lifelbl.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lifelbl.ForeColor = System.Drawing.Color.Gainsboro;
            this.Lifelbl.Location = new System.Drawing.Point(904, 14);
            this.Lifelbl.Name = "Lifelbl";
            this.Lifelbl.Size = new System.Drawing.Size(109, 34);
            this.Lifelbl.TabIndex = 5;
            this.Lifelbl.Text = "Life:";
            // 
            // LifePointslbl
            // 
            this.LifePointslbl.BackColor = System.Drawing.Color.Transparent;
            this.LifePointslbl.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LifePointslbl.ForeColor = System.Drawing.Color.Gainsboro;
            this.LifePointslbl.Location = new System.Drawing.Point(981, 14);
            this.LifePointslbl.Name = "LifePointslbl";
            this.LifePointslbl.Size = new System.Drawing.Size(71, 34);
            this.LifePointslbl.TabIndex = 6;
            this.LifePointslbl.Text = "100";
            // 
            // ScoreView
            // 
            this.ScoreView.BackColor = System.Drawing.Color.DarkOrchid;
            this.ScoreView.Font = new System.Drawing.Font("Stencil", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScoreView.ForeColor = System.Drawing.Color.Yellow;
            this.ScoreView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ScoreView.HideSelection = false;
            this.ScoreView.LabelWrap = false;
            this.ScoreView.Location = new System.Drawing.Point(20, 388);
            this.ScoreView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ScoreView.Name = "ScoreView";
            this.ScoreView.Size = new System.Drawing.Size(311, 366);
            this.ScoreView.TabIndex = 7;
            this.ScoreView.UseCompatibleStateImageBehavior = false;
            this.ScoreView.View = System.Windows.Forms.View.Details;
            this.ScoreView.Visible = false;
            // 
            // Scorebtn
            // 
            this.Scorebtn.BackColor = System.Drawing.Color.MintCream;
            this.Scorebtn.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Scorebtn.Location = new System.Drawing.Point(445, 324);
            this.Scorebtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Scorebtn.Name = "Scorebtn";
            this.Scorebtn.Size = new System.Drawing.Size(221, 82);
            this.Scorebtn.TabIndex = 8;
            this.Scorebtn.Text = "Best score";
            this.Scorebtn.UseVisualStyleBackColor = false;
            this.Scorebtn.Visible = false;
            this.Scorebtn.Click += new System.EventHandler(this.Scorebtn_Click);
            // 
            // ClScorebtn
            // 
            this.ClScorebtn.BackColor = System.Drawing.Color.MintCream;
            this.ClScorebtn.Font = new System.Drawing.Font("Stencil", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClScorebtn.Location = new System.Drawing.Point(679, 324);
            this.ClScorebtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ClScorebtn.Name = "ClScorebtn";
            this.ClScorebtn.Size = new System.Drawing.Size(221, 82);
            this.ClScorebtn.TabIndex = 9;
            this.ClScorebtn.Text = "close";
            this.ClScorebtn.UseVisualStyleBackColor = false;
            this.ClScorebtn.Visible = false;
            this.ClScorebtn.Click += new System.EventHandler(this.ClScorebtn_Click);
            // 
            // Playbtn
            // 
            this.Playbtn.BackColor = System.Drawing.Color.MintCream;
            this.Playbtn.Font = new System.Drawing.Font("Stencil", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Playbtn.Location = new System.Drawing.Point(445, 425);
            this.Playbtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Playbtn.Name = "Playbtn";
            this.Playbtn.Size = new System.Drawing.Size(221, 82);
            this.Playbtn.TabIndex = 10;
            this.Playbtn.Text = "play";
            this.Playbtn.UseVisualStyleBackColor = false;
            this.Playbtn.Visible = false;
            this.Playbtn.Click += new System.EventHandler(this.Playbtn_Click);
            // 
            // scoreMultiplierTimeLabel
            // 
            this.scoreMultiplierTimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.scoreMultiplierTimeLabel.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreMultiplierTimeLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.scoreMultiplierTimeLabel.Location = new System.Drawing.Point(13, 48);
            this.scoreMultiplierTimeLabel.Name = "scoreMultiplierTimeLabel";
            this.scoreMultiplierTimeLabel.Size = new System.Drawing.Size(365, 34);
            this.scoreMultiplierTimeLabel.TabIndex = 11;
            this.scoreMultiplierTimeLabel.Text = "Double Points: 0s";
            this.scoreMultiplierTimeLabel.Visible = false;
            // 
            // doubleShootTimeLabel
            // 
            this.doubleShootTimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.doubleShootTimeLabel.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doubleShootTimeLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.doubleShootTimeLabel.Location = new System.Drawing.Point(13, 82);
            this.doubleShootTimeLabel.Name = "doubleShootTimeLabel";
            this.doubleShootTimeLabel.Size = new System.Drawing.Size(365, 34);
            this.doubleShootTimeLabel.TabIndex = 12;
            this.doubleShootTimeLabel.Text = "Double Shoot: 0s";
            this.doubleShootTimeLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1316, 768);
            this.Controls.Add(this.doubleShootTimeLabel);
            this.Controls.Add(this.scoreMultiplierTimeLabel);
            this.Controls.Add(this.Playbtn);
            this.Controls.Add(this.ClScorebtn);
            this.Controls.Add(this.Scorebtn);
            this.Controls.Add(this.ScoreView);
            this.Controls.Add(this.LifePointslbl);
            this.Controls.Add(this.Lifelbl);
            this.Controls.Add(this.Pointslbl);
            this.Controls.Add(this.Scorelbl);
            this.Controls.Add(this.Exitbtn);
            this.Controls.Add(this.Replaybtn);
            this.Controls.Add(this.pauseLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pauseLabel;
        private System.Windows.Forms.Button Replaybtn;
        private System.Windows.Forms.Button Exitbtn;
        private System.Windows.Forms.Label Scorelbl;
        private System.Windows.Forms.Label Pointslbl;
        private Label Lifelbl;
        private Label LifePointslbl;
        private ListView ScoreView;
        private Button Scorebtn;
        private Button ClScorebtn;
        private Button Playbtn;
        public Label scoreMultiplierTimeLabel;
        public Label doubleShootTimeLabel;
    }
}

