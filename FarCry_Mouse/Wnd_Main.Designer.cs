﻿namespace FarCry_Mouse
{
    partial class Wnd_Main
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Wnd_Main));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Bgw_Mouse = new System.ComponentModel.BackgroundWorker();
            this.Txt_Log = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Lbl_Screen = new System.Windows.Forms.Label();
            this.Lbl_Client = new System.Windows.Forms.Label();
            this.Lbl_Joystick = new System.Windows.Forms.Label();
            this.Chk_x = new System.Windows.Forms.CheckBox();
            this.Chk_y = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Lbl_ClientSize = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Screen :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 59);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Joystick :";
            // 
            // Bgw_Mouse
            // 
            this.Bgw_Mouse.WorkerReportsProgress = true;
            this.Bgw_Mouse.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgwMouse_DoWork);
            this.Bgw_Mouse.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BgwMouse_ProgressChanged);
            this.Bgw_Mouse.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgwMouse_RunWorkerCompleted);
            // 
            // Txt_Log
            // 
            this.Txt_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Txt_Log.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_Log.Location = new System.Drawing.Point(12, 92);
            this.Txt_Log.Name = "Txt_Log";
            this.Txt_Log.Size = new System.Drawing.Size(410, 247);
            this.Txt_Log.TabIndex = 5;
            this.Txt_Log.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Client :";
            // 
            // Lbl_Screen
            // 
            this.Lbl_Screen.AutoSize = true;
            this.Lbl_Screen.Location = new System.Drawing.Point(81, 11);
            this.Lbl_Screen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lbl_Screen.Name = "Lbl_Screen";
            this.Lbl_Screen.Size = new System.Drawing.Size(0, 16);
            this.Lbl_Screen.TabIndex = 6;
            // 
            // Lbl_Client
            // 
            this.Lbl_Client.AutoSize = true;
            this.Lbl_Client.Location = new System.Drawing.Point(81, 43);
            this.Lbl_Client.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lbl_Client.Name = "Lbl_Client";
            this.Lbl_Client.Size = new System.Drawing.Size(0, 16);
            this.Lbl_Client.TabIndex = 7;
            // 
            // Lbl_Joystick
            // 
            this.Lbl_Joystick.AutoSize = true;
            this.Lbl_Joystick.Location = new System.Drawing.Point(81, 59);
            this.Lbl_Joystick.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lbl_Joystick.Name = "Lbl_Joystick";
            this.Lbl_Joystick.Size = new System.Drawing.Size(0, 16);
            this.Lbl_Joystick.TabIndex = 8;
            // 
            // Chk_x
            // 
            this.Chk_x.AutoSize = true;
            this.Chk_x.Location = new System.Drawing.Point(329, 7);
            this.Chk_x.Name = "Chk_x";
            this.Chk_x.Size = new System.Drawing.Size(98, 20);
            this.Chk_x.TabIndex = 9;
            this.Chk_x.Text = "Invert X Axis";
            this.Chk_x.UseVisualStyleBackColor = true;
            // 
            // Chk_y
            // 
            this.Chk_y.AutoSize = true;
            this.Chk_y.Location = new System.Drawing.Point(329, 27);
            this.Chk_y.Name = "Chk_y";
            this.Chk_y.Size = new System.Drawing.Size(98, 20);
            this.Chk_y.TabIndex = 10;
            this.Chk_y.Text = "Invert Y axis";
            this.Chk_y.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 27);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Client size :";
            // 
            // Lbl_ClientSize
            // 
            this.Lbl_ClientSize.AutoSize = true;
            this.Lbl_ClientSize.Location = new System.Drawing.Point(98, 27);
            this.Lbl_ClientSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lbl_ClientSize.Name = "Lbl_ClientSize";
            this.Lbl_ClientSize.Size = new System.Drawing.Size(0, 16);
            this.Lbl_ClientSize.TabIndex = 12;
            // 
            // Wnd_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 351);
            this.Controls.Add(this.Lbl_ClientSize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Chk_y);
            this.Controls.Add(this.Chk_x);
            this.Controls.Add(this.Lbl_Joystick);
            this.Controls.Add(this.Lbl_Client);
            this.Controls.Add(this.Lbl_Screen);
            this.Controls.Add(this.Txt_Log);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Wnd_Main";
            this.Text = "FarCry Arcade - Mouse";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WndMain_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.ComponentModel.BackgroundWorker Bgw_Mouse;
        private System.Windows.Forms.RichTextBox Txt_Log;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Lbl_Screen;
        private System.Windows.Forms.Label Lbl_Client;
        private System.Windows.Forms.Label Lbl_Joystick;
        private System.Windows.Forms.CheckBox Chk_x;
        private System.Windows.Forms.CheckBox Chk_y;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Lbl_ClientSize;
    }
}

