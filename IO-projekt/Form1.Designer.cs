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
            this.Replaybtn = new System.Windows.Forms.Button();
            this.Exitbtn = new System.Windows.Forms.Button();
            this.Scorelbl = new System.Windows.Forms.Label();
            this.Pointslbl = new System.Windows.Forms.Label();
            this.Lifelbl = new System.Windows.Forms.Label();
            this.LifePointslbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pauseLabel
            // 
            this.pauseLabel.BackColor = System.Drawing.Color.Transparent;
            this.pauseLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pauseLabel.Font = new System.Drawing.Font("Stencil", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pauseLabel.ForeColor = System.Drawing.Color.Red;
            this.pauseLabel.Location = new System.Drawing.Point(441, 50);
            this.pauseLabel.Name = "pauseLabel";
            this.pauseLabel.Size = new System.Drawing.Size(475, 128);
            this.pauseLabel.TabIndex = 0;
            this.pauseLabel.Text = "Pause";
            this.pauseLabel.Visible = false;
            // 
            // Replaybtn
            // 
            this.Replaybtn.BackColor = System.Drawing.Color.MintCream;
            this.Replaybtn.Font = new System.Drawing.Font("Stencil", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Replaybtn.Location = new System.Drawing.Point(445, 227);
            this.Replaybtn.Name = "Replaybtn";
            this.Replaybtn.Size = new System.Drawing.Size(226, 83);
            this.Replaybtn.TabIndex = 1;
            this.Replaybtn.Text = "replay";
            this.Replaybtn.UseVisualStyleBackColor = false;
            this.Replaybtn.Visible = false;
            // 
            // Exitbtn
            // 
            this.Exitbtn.BackColor = System.Drawing.Color.MintCream;
            this.Exitbtn.Font = new System.Drawing.Font("Stencil", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Exitbtn.Location = new System.Drawing.Point(677, 227);
            this.Exitbtn.Name = "Exitbtn";
            this.Exitbtn.Size = new System.Drawing.Size(221, 83);
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
            this.Scorelbl.Location = new System.Drawing.Point(13, 13);
            this.Scorelbl.Name = "Scorelbl";
            this.Scorelbl.Size = new System.Drawing.Size(113, 34);
            this.Scorelbl.TabIndex = 3;
            this.Scorelbl.Text = "Score:";
            // 
            // Pointslbl
            // 
            this.Pointslbl.BackColor = System.Drawing.Color.Transparent;
            this.Pointslbl.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pointslbl.ForeColor = System.Drawing.Color.Gainsboro;
            this.Pointslbl.Location = new System.Drawing.Point(114, 13);
            this.Pointslbl.Name = "Pointslbl";
            this.Pointslbl.Size = new System.Drawing.Size(68, 39);
            this.Pointslbl.TabIndex = 4;
            this.Pointslbl.Text = "0";
            // 
            // Lifelbl
            // 
            this.Lifelbl.BackColor = System.Drawing.Color.Transparent;
            this.Lifelbl.Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lifelbl.ForeColor = System.Drawing.Color.Gainsboro;
            this.Lifelbl.Location = new System.Drawing.Point(937, 13);
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
            this.LifePointslbl.Location = new System.Drawing.Point(1014, 13);
            this.LifePointslbl.Name = "LifePointslbl";
            this.LifePointslbl.Size = new System.Drawing.Size(41, 39);
            this.LifePointslbl.TabIndex = 6;
            this.LifePointslbl.Text = "3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.LifePointslbl);
            this.Controls.Add(this.Lifelbl);
            this.Controls.Add(this.Pointslbl);
            this.Controls.Add(this.Scorelbl);
            this.Controls.Add(this.Exitbtn);
            this.Controls.Add(this.Replaybtn);
            this.Controls.Add(this.pauseLabel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label pauseLabel;
        private System.Windows.Forms.Button Replaybtn;
        private System.Windows.Forms.Button Exitbtn;
        private System.Windows.Forms.Label Scorelbl;
        private System.Windows.Forms.Label Pointslbl;
        private Label Lifelbl;
        private Label LifePointslbl;
    }
}

