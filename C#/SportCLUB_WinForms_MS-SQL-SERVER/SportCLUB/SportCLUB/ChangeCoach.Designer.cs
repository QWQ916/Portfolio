namespace SportCLUB
{
    partial class ChangeCoach
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeCoach));
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.dgv_coach = new System.Windows.Forms.DataGridView();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.but_okcoach = new System.Windows.Forms.Button();
            this.tb_messCoach = new System.Windows.Forms.TextBox();
            this.but_coach = new System.Windows.Forms.Button();
            this.but_confirm = new System.Windows.Forms.Button();
            this.t = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_coach)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.Color.Blue;
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox6.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox6.ForeColor = System.Drawing.Color.White;
            this.textBox6.Location = new System.Drawing.Point(12, 12);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(300, 39);
            this.textBox6.TabIndex = 128;
            this.textBox6.TabStop = false;
            this.textBox6.Text = "Выберите тренера";
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dgv_coach
            // 
            this.dgv_coach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_coach.Location = new System.Drawing.Point(12, 57);
            this.dgv_coach.Name = "dgv_coach";
            this.dgv_coach.Size = new System.Drawing.Size(861, 253);
            this.dgv_coach.TabIndex = 127;
            this.dgv_coach.SelectionChanged += new System.EventHandler(this.dgv_coach_SelectionChanged);
            // 
            // rtb
            // 
            this.rtb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rtb.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.rtb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtb.Location = new System.Drawing.Point(12, 350);
            this.rtb.Name = "rtb";
            this.rtb.ReadOnly = true;
            this.rtb.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rtb.Size = new System.Drawing.Size(776, 79);
            this.rtb.TabIndex = 129;
            this.rtb.TabStop = false;
            this.rtb.Text = resources.GetString("rtb.Text");
            // 
            // but_okcoach
            // 
            this.but_okcoach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.but_okcoach.Enabled = false;
            this.but_okcoach.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_okcoach.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_okcoach.ForeColor = System.Drawing.Color.Green;
            this.but_okcoach.Location = new System.Drawing.Point(816, 316);
            this.but_okcoach.Name = "but_okcoach";
            this.but_okcoach.Size = new System.Drawing.Size(57, 39);
            this.but_okcoach.TabIndex = 132;
            this.but_okcoach.TabStop = false;
            this.but_okcoach.Text = "OK";
            this.but_okcoach.UseVisualStyleBackColor = false;
            this.but_okcoach.Visible = false;
            this.but_okcoach.Click += new System.EventHandler(this.but_okcoach_Click);
            // 
            // tb_messCoach
            // 
            this.tb_messCoach.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tb_messCoach.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_messCoach.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tb_messCoach.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_messCoach.ForeColor = System.Drawing.Color.Blue;
            this.tb_messCoach.Location = new System.Drawing.Point(217, 316);
            this.tb_messCoach.Multiline = true;
            this.tb_messCoach.Name = "tb_messCoach";
            this.tb_messCoach.ReadOnly = true;
            this.tb_messCoach.Size = new System.Drawing.Size(462, 28);
            this.tb_messCoach.TabIndex = 131;
            this.tb_messCoach.TabStop = false;
            this.tb_messCoach.Text = "Выберите строчку с тренером из доступных";
            this.tb_messCoach.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // but_coach
            // 
            this.but_coach.BackColor = System.Drawing.Color.LightSkyBlue;
            this.but_coach.Enabled = false;
            this.but_coach.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_coach.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_coach.ForeColor = System.Drawing.Color.RoyalBlue;
            this.but_coach.Location = new System.Drawing.Point(722, 12);
            this.but_coach.Name = "but_coach";
            this.but_coach.Size = new System.Drawing.Size(151, 39);
            this.but_coach.TabIndex = 130;
            this.but_coach.Text = "Поменять";
            this.but_coach.UseVisualStyleBackColor = false;
            this.but_coach.Visible = false;
            this.but_coach.Click += new System.EventHandler(this.but_coach_Click);
            // 
            // but_confirm
            // 
            this.but_confirm.BackColor = System.Drawing.Color.PaleGreen;
            this.but_confirm.Enabled = false;
            this.but_confirm.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_confirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_confirm.ForeColor = System.Drawing.Color.DarkGreen;
            this.but_confirm.Location = new System.Drawing.Point(355, 453);
            this.but_confirm.Name = "but_confirm";
            this.but_confirm.Size = new System.Drawing.Size(190, 44);
            this.but_confirm.TabIndex = 133;
            this.but_confirm.Text = "Применить";
            this.but_confirm.UseVisualStyleBackColor = false;
            this.but_confirm.Visible = false;
            this.but_confirm.Click += new System.EventHandler(this.but_confirm_Click);
            // 
            // t
            // 
            this.t.Interval = 2000;
            this.t.Tick += new System.EventHandler(this.t_Tick);
            // 
            // ChangeCoach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(883, 509);
            this.Controls.Add(this.but_confirm);
            this.Controls.Add(this.but_okcoach);
            this.Controls.Add(this.tb_messCoach);
            this.Controls.Add(this.but_coach);
            this.Controls.Add(this.rtb);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.dgv_coach);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChangeCoach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Смена тренера";
            this.Load += new System.EventHandler(this.ChangeCoach_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_coach)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.DataGridView dgv_coach;
        private System.Windows.Forms.RichTextBox rtb;
        private System.Windows.Forms.Button but_okcoach;
        private System.Windows.Forms.TextBox tb_messCoach;
        private System.Windows.Forms.Button but_coach;
        private System.Windows.Forms.Button but_confirm;
        private System.Windows.Forms.Timer t;
    }
}