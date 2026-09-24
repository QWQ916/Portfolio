using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportCLUB
{
    public partial class AddCompetition : Form
    {
        public AddCompetition(AdminsForm F)
        {
            InitializeComponent(); this.F = F;
        }
        AdminsForm F;


        private void AddCompetition_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(GetDataFromDB("SELECT TOP 1 CompetitionID FROM guest.Competition ORDER BY CompetitionID DESC").Rows[0]["CompetitionID"].ToString()) + 1;
            tb_id.Text = $"Соревнование #{id}"; CheckAll();
        }


        // Задать текст для TextBox
        public void Message(TextBox b, string mess, int x)
        {
            b.Text = mess;
            if (x == 0)
            {
                b.ForeColor = Color.Blue;
            }
            else if (x == 1)
            {
                b.ForeColor = Color.Green;
            }
            else
            {
                b.ForeColor = Color.Red;
            }
        }

        // Функция получения данных из SQL-запроса к БД
        public DataTable GetDataFromDB(string query)
        {
            DataBase db = new DataBase();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, db.GetConnection());

            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            db.CloseCon();

            return dt;
        }


        // Функция выполнения команды из SQL-запроса в БД
        public void EditDataFromDB(string query)
        {
            try
            {
                DataBase db = new DataBase();
                SqlCommand cmd = new SqlCommand(query, db.GetConnection());
                db.OpenCon();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Изменения приняты!", "Успех!");
                db.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void CheckStrings(TextBox tb)
        {
            if (tb.Text == "") { tb.BackColor = Color.FromArgb(255, 192, 192); }
            else { tb.BackColor = SystemColors.Info; }
        }


        // Изменение видимости для элементов
        public void ChangeBut(Button b, bool x)
        {
            b.Enabled = x; b.Visible = x;
        }

        // Проверка на пустоту всех полей
        public void CheckAll()
        {
            if (tb_name.Text == "" || tb_country.Text == "" || tb_city.Text == "" || tb_datestart.Text == "" || tb_dateend.Text == "" || tb_age.Text == "" || tb_rank.Text == "" || tb_score.Text == "")
            {
                ChangeBut(but_confirm, false); Message(tb_mess, "Заполните все поля!", 2);
            }
            else
            {
                ChangeBut(but_confirm, true); tb_mess.Text = "";
            }
        }


        private void tb_name_TextChanged(object sender, EventArgs e)
        {
            CheckStrings(tb_name); CheckAll();
        }

        private void tb_country_TextChanged(object sender, EventArgs e)
        {
            CheckStrings(tb_country); CheckAll();
        }

        private void tb_city_TextChanged(object sender, EventArgs e)
        {
            CheckStrings(tb_city); CheckAll();
        }



        private void tb_age_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(tb_age.Text); tb_age.BackColor = SystemColors.Info;
                if (value <= 0 || value > 80) { throw new Exception("Указан неверный возраст"); }
                tb_mess.Text = ""; CheckAll();
            }
            catch (Exception ex)
            {
                tb_age.BackColor = Color.FromArgb(255, 192, 192);
                tb_mess.Text = $"Поле с красным фоном содержит ошибку: {ex.Message}"; tb_mess.ForeColor = Color.Red;
                ChangeBut(but_confirm, false);
            }
        }

        private void tb_rank_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(tb_rank.Text); tb_rank.BackColor = SystemColors.Info;
                if (value <= 0 || value > 11) { throw new Exception("Указан неверный ID мастерства игроков"); }
                tb_mess.Text = ""; CheckAll();
            }
            catch (Exception ex)
            {
                tb_rank.BackColor = Color.FromArgb(255, 192, 192);
                tb_mess.Text = $"Поле с красным фоном содержит ошибку: {ex.Message}"; tb_mess.ForeColor = Color.Red;
                ChangeBut(but_confirm, false);
            }
        }

        private void tb_score_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(tb_score.Text); tb_score.BackColor = SystemColors.Info;
                if (value <= 0) { throw new Exception("Указан неверный рейтинг!"); }
                tb_mess.Text = ""; CheckAll();
            }
            catch (Exception ex)
            {
                tb_score.BackColor = Color.FromArgb(255, 192, 192);
                tb_mess.Text = $"Поле с красным фоном содержит ошибку: {ex.Message}"; tb_mess.ForeColor = Color.Red;
                ChangeBut(but_confirm, false);
            }
        }


        private void tb_start_d_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_start_d, tb_start_m, tb_start_y, tb_datestart);
        }

        private void tb_start_m_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_start_d, tb_start_m, tb_start_y, tb_datestart);
        }

        private void tb_start_y_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_start_d, tb_start_m, tb_start_y, tb_datestart); 
        }


        private void tb_end_d_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_end_d, tb_end_m, tb_end_y, tb_dateend);
        }

        private void tb_end_m_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_end_d, tb_end_m, tb_end_y, tb_dateend);
        }

        private void tb_end_y_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_end_d, tb_end_m, tb_end_y, tb_dateend);
        }


        public void CheckDate(TextBox day, TextBox month, TextBox year, TextBox show)
        {
            try
            {
                int d = Convert.ToInt32(day.Text); day.BackColor = SystemColors.Info;
                int m = Convert.ToInt32(month.Text); month.BackColor = SystemColors.Info;
                int y = Convert.ToInt32(year.Text); year.BackColor = SystemColors.Info;
                DateTime date = new DateTime(y, m, d);
                tb_mess.Text = ""; CheckAll();
                show.Text = date.ToString();
            }
            catch (Exception ex)
            {
                day.BackColor = Color.FromArgb(255, 192, 192);
                month.BackColor = Color.FromArgb(255, 192, 192);
                year.BackColor = Color.FromArgb(255, 192, 192);
                show.Text = "";
                tb_mess.Text = $"Поле с красным фоном содержит ошибку: {ex.Message}"; tb_mess.ForeColor = Color.Red;
                ChangeBut(but_confirm, false);
            }
        }

        private void chb_unk_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_unk.Checked)
            {
                tb_end_d.Enabled = false; tb_end_d.BackColor = SystemColors.ControlDark; tb_end_d.Text = "";
                tb_end_m.Enabled = false; tb_end_m.BackColor = SystemColors.ControlDark; tb_end_m.Text = "";
                tb_end_y.Enabled = false; tb_end_y.BackColor = SystemColors.ControlDark; tb_end_y.Text = "";
                tb_dateend.Text = "NULL";
            }
            else
            {
                tb_end_d.Enabled = true; tb_end_d.BackColor = SystemColors.Window;
                tb_end_m.Enabled = true; tb_end_m.BackColor = SystemColors.Window; 
                tb_end_y.Enabled = true; tb_end_y.BackColor = SystemColors.Window; tb_dateend.Text = "";
                CheckDate(tb_end_d, tb_end_m, tb_end_y, tb_dateend); 
            }
            CheckAll();     
        }

        private void but_confirm_Click(object sender, EventArgs e)
        {
            int d2; int m2; int y2;
            if (!chb_unk.Checked)
            {
                d2 = Convert.ToInt32(tb_end_d.Text); m2 = Convert.ToInt32(tb_end_m.Text); y2 = Convert.ToInt32(tb_end_y.Text);
            }
            else
            {
                d2 = 31; m2 = 12; y2 = 9999;
            }
            int d1 = Convert.ToInt32(tb_start_d.Text); 
            int m1 = Convert.ToInt32(tb_start_m.Text); 
            int y1 = Convert.ToInt32(tb_start_y.Text); 
            DateTime date1 = new DateTime(y1, m1, d1);
            DateTime date2 = new DateTime(y2, m2, d2);
            if (DateTime.Compare(date1, date2) > 0 && !chb_unk.Checked)
            {
                Message(tb_mess, "Дата начала соревнования не может быть позже даты окончания!", 2);
            }
            else
            {
                string name = tb_name.Text;
                string country = tb_country.Text;
                string city = tb_city.Text;
                int age = Convert.ToInt32(tb_age.Text);
                int score = Convert.ToInt32(tb_score.Text);
                int rank = Convert.ToInt32(tb_rank.Text);
                if (chb_unk.Checked)
                {
                    EditDataFromDB($"INSERT INTO guest.Competition(Name, Country, City, DateStart, MinMasteryID, MaxAge, PlayerID, Score) VALUES ('{name}', '{country}', '{city}', '{date1}', {rank}, {age}, 1, {score})");
                }
                else
                {
                    EditDataFromDB($"INSERT INTO guest.Competition(Name, Country, City, DateStart, DateEnd, MinMasteryID, MaxAge, PlayerID, Score) VALUES ('{name}', '{country}', '{city}', '{date1}', '{date2}', {rank}, {age}, 1, {score})");
                }
                tb_name.Enabled = false;
                tb_country.Enabled = false;
                tb_city.Enabled = false;
                tb_age.Enabled = false;
                tb_score.Enabled = false;
                tb_rank.Enabled = false;
                tb_start_d.Enabled = false;
                tb_end_d.Enabled = false;
                tb_start_m.Enabled = false;
                tb_end_m.Enabled = false;
                tb_start_y.Enabled = false;
                tb_end_y.Enabled = false;
                Message(tb_mess, "Соревнование успешно создано! Ожидайте!", 1);
                ChangeBut(but_confirm, false);
                t.Enabled = true;
            }
        }

        private void t_Tick(object sender, EventArgs e)
        {
            t.Enabled = false; Close(); F.Restart();
        }
    }
}
