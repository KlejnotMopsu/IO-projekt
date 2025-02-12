﻿using System.Windows.Forms;

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
            this.Scorelbl = new System.Windows.Forms.Label();
            this.Pointslbl = new System.Windows.Forms.Label();
            this.Lifelbl = new System.Windows.Forms.Label();
            this.LifePointslbl = new System.Windows.Forms.Label();
            this.ScoreView = new System.Windows.Forms.ListView();
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
            this.pauseLabel.Location = new System.Drawing.Point(331, 41);
            this.pauseLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pauseLabel.Name = "pauseLabel";
            this.pauseLabel.Size = new System.Drawing.Size(348, 114);
            this.pauseLabel.TabIndex = 0;
            this.pauseLabel.Text = "Pause";
            this.pauseLabel.Visible = false;
            // 
            // Scorelbl
            // 
            this.Scorelbl.BackColor = System.Drawing.Color.Transparent;
            this.Scorelbl.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Scorelbl.ForeColor = System.Drawing.Color.Gainsboro;
            this.Scorelbl.Location = new System.Drawing.Point(10, 11);
            this.Scorelbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Scorelbl.Name = "Scorelbl";
            this.Scorelbl.Size = new System.Drawing.Size(94, 28);
            this.Scorelbl.TabIndex = 3;
            this.Scorelbl.Text = "Score:";
            // 
            // Pointslbl
            // 
            this.Pointslbl.BackColor = System.Drawing.Color.Transparent;
            this.Pointslbl.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pointslbl.ForeColor = System.Drawing.Color.Gainsboro;
            this.Pointslbl.Location = new System.Drawing.Point(99, 11);
            this.Pointslbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Pointslbl.Name = "Pointslbl";
            this.Pointslbl.Size = new System.Drawing.Size(185, 28);
            this.Pointslbl.TabIndex = 4;
            this.Pointslbl.Text = "0";
            // 
            // Lifelbl
            // 
            this.Lifelbl.BackColor = System.Drawing.Color.Transparent;
            this.Lifelbl.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lifelbl.ForeColor = System.Drawing.Color.Gainsboro;
            this.Lifelbl.Location = new System.Drawing.Point(678, 11);
            this.Lifelbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lifelbl.Name = "Lifelbl";
            this.Lifelbl.Size = new System.Drawing.Size(82, 28);
            this.Lifelbl.TabIndex = 5;
            this.Lifelbl.Text = "Life:";
            // 
            // LifePointslbl
            // 
            this.LifePointslbl.BackColor = System.Drawing.Color.Transparent;
            this.LifePointslbl.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LifePointslbl.ForeColor = System.Drawing.Color.Gainsboro;
            this.LifePointslbl.Location = new System.Drawing.Point(736, 11);
            this.LifePointslbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LifePointslbl.Name = "LifePointslbl";
            this.LifePointslbl.Size = new System.Drawing.Size(53, 28);
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
            this.ScoreView.Location = new System.Drawing.Point(15, 315);
            this.ScoreView.Margin = new System.Windows.Forms.Padding(2);
            this.ScoreView.Name = "ScoreView";
            this.ScoreView.Size = new System.Drawing.Size(234, 298);
            this.ScoreView.TabIndex = 7;
            this.ScoreView.UseCompatibleStateImageBehavior = false;
            this.ScoreView.View = System.Windows.Forms.View.Details;
            this.ScoreView.Visible = false;
            // 
            // scoreMultiplierTimeLabel
            // 
            this.scoreMultiplierTimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.scoreMultiplierTimeLabel.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreMultiplierTimeLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.scoreMultiplierTimeLabel.Location = new System.Drawing.Point(10, 39);
            this.scoreMultiplierTimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.scoreMultiplierTimeLabel.Name = "scoreMultiplierTimeLabel";
            this.scoreMultiplierTimeLabel.Size = new System.Drawing.Size(274, 28);
            this.scoreMultiplierTimeLabel.TabIndex = 11;
            this.scoreMultiplierTimeLabel.Text = "Double Points: 0s";
            this.scoreMultiplierTimeLabel.Visible = false;
            // 
            // doubleShootTimeLabel
            // 
            this.doubleShootTimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.doubleShootTimeLabel.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doubleShootTimeLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.doubleShootTimeLabel.Location = new System.Drawing.Point(10, 67);
            this.doubleShootTimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.doubleShootTimeLabel.Name = "doubleShootTimeLabel";
            this.doubleShootTimeLabel.Size = new System.Drawing.Size(274, 28);
            this.doubleShootTimeLabel.TabIndex = 12;
            this.doubleShootTimeLabel.Text = "Double Shoot: 0s";
            this.doubleShootTimeLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(987, 624);
            this.Controls.Add(this.doubleShootTimeLabel);
            this.Controls.Add(this.scoreMultiplierTimeLabel);
            this.Controls.Add(this.ScoreView);
            this.Controls.Add(this.LifePointslbl);
            this.Controls.Add(this.Lifelbl);
            this.Controls.Add(this.Pointslbl);
            this.Controls.Add(this.Scorelbl);
            this.Controls.Add(this.pauseLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Scorelbl;
        private System.Windows.Forms.Label Pointslbl;
        private Label Lifelbl;
        private Label LifePointslbl;
        private ListView ScoreView;
        public Label scoreMultiplierTimeLabel;
        public Label doubleShootTimeLabel;
        public Label pauseLabel;
    }
}

