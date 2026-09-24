namespace SportCLUB
{
    partial class AdminsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminsForm));
            this.dgv = new System.Windows.Forms.DataGridView();
            this.cb1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.but_change = new System.Windows.Forms.Button();
            this.but_createPlayer = new System.Windows.Forms.Button();
            this.but_addCoach = new System.Windows.Forms.Button();
            this.dgv_competition = new System.Windows.Forms.DataGridView();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.but_addcomp = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.but_changecomp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_competition)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(12, 60);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.Size = new System.Drawing.Size(899, 521);
            this.dgv.TabIndex = 0;
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // cb1
            // 
            this.cb1.BackColor = System.Drawing.Color.SkyBlue;
            this.cb1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cb1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cb1.FormattingEnabled = true;
            this.cb1.Items.AddRange(new object[] {
            "Игроки",
            "Тренеры"});
            this.cb1.Location = new System.Drawing.Point(12, 595);
            this.cb1.Name = "cb1";
            this.cb1.Size = new System.Drawing.Size(278, 32);
            this.cb1.TabIndex = 1;
            this.cb1.TabStop = false;
            this.cb1.SelectedIndexChanged += new System.EventHandler(this.cb1_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(12, 633);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(278, 41);
            this.textBox1.TabIndex = 2;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "Выберите строчку с пользователем, чтобы изменить его данные";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // but_change
            // 
            this.but_change.BackColor = System.Drawing.Color.Blue;
            this.but_change.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_change.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_change.ForeColor = System.Drawing.Color.White;
            this.but_change.Location = new System.Drawing.Point(296, 595);
            this.but_change.Name = "but_change";
            this.but_change.Size = new System.Drawing.Size(101, 79);
            this.but_change.TabIndex = 3;
            this.but_change.Text = "Изменить";
            this.but_change.UseVisualStyleBackColor = false;
            this.but_change.Click += new System.EventHandler(this.but_change_Click);
            // 
            // but_createPlayer
            // 
            this.but_createPlayer.BackColor = System.Drawing.Color.Khaki;
            this.but_createPlayer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_createPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_createPlayer.Location = new System.Drawing.Point(691, 13);
            this.but_createPlayer.Name = "but_createPlayer";
            this.but_createPlayer.Size = new System.Drawing.Size(220, 43);
            this.but_createPlayer.TabIndex = 4;
            this.but_createPlayer.Text = "Добавить игрока";
            this.but_createPlayer.UseVisualStyleBackColor = false;
            this.but_createPlayer.Click += new System.EventHandler(this.but_createPlayer_Click);
            // 
            // but_addCoach
            // 
            this.but_addCoach.BackColor = System.Drawing.Color.Chartreuse;
            this.but_addCoach.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_addCoach.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_addCoach.Location = new System.Drawing.Point(465, 13);
            this.but_addCoach.Name = "but_addCoach";
            this.but_addCoach.Size = new System.Drawing.Size(220, 43);
            this.but_addCoach.TabIndex = 5;
            this.but_addCoach.Text = "Добавить тренера";
            this.but_addCoach.UseVisualStyleBackColor = false;
            this.but_addCoach.Click += new System.EventHandler(this.but_addCoach_Click);
            // 
            // dgv_competition
            // 
            this.dgv_competition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_competition.Location = new System.Drawing.Point(917, 145);
            this.dgv_competition.Name = "dgv_competition";
            this.dgv_competition.ReadOnly = true;
            this.dgv_competition.Size = new System.Drawing.Size(887, 631);
            this.dgv_competition.TabIndex = 6;
            this.dgv_competition.SelectionChanged += new System.EventHandler(this.dgv_competition_SelectionChanged);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.Navy;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox2.ForeColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(12, 13);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(447, 43);
            this.textBox2.TabIndex = 7;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Просмотр игроков и тренеров";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Navy;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox3.ForeColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(1222, 97);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(582, 43);
            this.textBox3.TabIndex = 8;
            this.textBox3.TabStop = false;
            this.textBox3.Text = "Просмотр доступных соревнований";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // but_addcomp
            // 
            this.but_addcomp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.but_addcomp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_addcomp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_addcomp.Location = new System.Drawing.Point(917, 96);
            this.but_addcomp.Name = "but_addcomp";
            this.but_addcomp.Size = new System.Drawing.Size(299, 43);
            this.but_addcomp.TabIndex = 9;
            this.but_addcomp.Text = "Добавить соревнование";
            this.but_addcomp.UseVisualStyleBackColor = false;
            this.but_addcomp.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox4.Location = new System.Drawing.Point(616, 688);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(295, 41);
            this.textBox4.TabIndex = 10;
            this.textBox4.TabStop = false;
            this.textBox4.Text = "Выберите строчку с соревнованием, \r\nчтобы изменить его данные";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // but_changecomp
            // 
            this.but_changecomp.BackColor = System.Drawing.Color.Teal;
            this.but_changecomp.Enabled = false;
            this.but_changecomp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_changecomp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_changecomp.ForeColor = System.Drawing.Color.White;
            this.but_changecomp.Location = new System.Drawing.Point(810, 603);
            this.but_changecomp.Name = "but_changecomp";
            this.but_changecomp.Size = new System.Drawing.Size(101, 79);
            this.but_changecomp.TabIndex = 11;
            this.but_changecomp.Text = "Изменить";
            this.but_changecomp.UseVisualStyleBackColor = false;
            this.but_changecomp.Visible = false;
            this.but_changecomp.Click += new System.EventHandler(this.but_changecomp_Click);
            // 
            // AdminsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1816, 786);
            this.Controls.Add(this.but_changecomp);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.but_addcomp);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.dgv_competition);
            this.Controls.Add(this.but_addCoach);
            this.Controls.Add(this.but_createPlayer);
            this.Controls.Add(this.but_change);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cb1);
            this.Controls.Add(this.dgv);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdminsForm";
            this.Text = "SportCLUB - Администратор";
            this.Load += new System.EventHandler(this.AdminsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_competition)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.ComboBox cb1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button but_change;
        private System.Windows.Forms.Button but_createPlayer;
        private System.Windows.Forms.Button but_addCoach;
        private System.Windows.Forms.DataGridView dgv_competition;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button but_addcomp;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button but_changecomp;
    }
}