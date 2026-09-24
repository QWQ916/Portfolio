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
    public partial class ChangeInfoComp : Form
    {
        public ChangeInfoComp(AdminsForm F, Competitions c)
        {
            InitializeComponent(); this.F = F; this.c = c;
        }
        AdminsForm F; Competitions c; 

        private void ChangeInfoComp_Load(object sender, EventArgs e)
        {
            tb_id.Text = $"Соревнование #{c.ID}";
            tb_name.Text = c.Name;
            tb_city.Text = c.City;
            tb_age.Text = c.MaxAge.ToString();
            tb_rank.Text = c.MinMasteryID.ToString();
            tb_score.Text = c.Score.ToString();
            tb_datestart.Text = c.DateStart.ToString();
            DateTime DE = c.DateEnd;
            if (DE.Year == 1) { chb_unk.Checked = true; chb_unk_CheckedChanged(sender, e); }
            else
            {
                tb_dateend.Text = c.DateEnd.ToString();
                tb_end_d.Text = c.DateEnd.Day.ToString();
                tb_end_m.Text = c.DateEnd.Month.ToString();
                tb_end_y.Text = c.DateEnd.Year.ToString();
            }
            tb_start_d.Text = c.DateStart.Day.ToString();
            tb_start_m.Text = c.DateStart.Month.ToString();
            dgv_player.DataSource = GetDataFromDB($"SELECT * FROM dbo.vPlayersFullInfo WHERE PlayerID = {c.PlayerID}"); dgv_player.AllowUserToAddRows = false;
            tb_start_y.Text = c.DateStart.Year.ToString();
            tb_country.Text = c.Country;
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


        // Функция выролнения команды из SQL-запроса в БД
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


        // Функция проверки заполнения textbox
        public void StringEventText(string value, TextBox tb)
        {
            if (tb.Text == "")
            {
                tb.BackColor = Color.FromArgb(255, 192, 192);
            }
            else
            {
                if (tb.Text != value)
                {
                    tb.BackColor = Color.LimeGreen; tb.Font = new Font(tb.Font, FontStyle.Italic);
                }
                else
                {
                    tb.BackColor = SystemColors.Window; tb.Font = new Font(tb.Font, FontStyle.Regular);
                }
            }
            
        }

        // Функция проверки и заполнения textbox с int
        public void IntEventText(TextBox tb, int val)
        {
            try
            {
                int value = Convert.ToInt32(tb.Text);
                if (value == val)
                {
                    tb.BackColor = SystemColors.Window; tb.Font = new Font(tb.Font, FontStyle.Regular);
                }
                else
                {
                    tb.BackColor = Color.LimeGreen; tb.Font = new Font(tb.Font, FontStyle.Italic);
                }
                tb_mess.Text = ""; ChangeBut(but_confirm, true);
            }
            catch (Exception ex)
            {
                tb.BackColor = Color.FromArgb(255, 192, 192);
                tb_mess.Text = $"Поле с красным фоном содержит ошибку: {ex.Message}"; tb_mess.ForeColor = Color.Red;
                ChangeBut(but_confirm, false);
            }
        }


        public void CheckDate(TextBox day, TextBox month, TextBox year, TextBox show, bool start)
        {
            try
            {
                int d = Convert.ToInt32(day.Text); day.BackColor = SystemColors.Info;
                int m = Convert.ToInt32(month.Text); month.BackColor = SystemColors.Info;
                int y = Convert.ToInt32(year.Text); year.BackColor = SystemColors.Info;
                if (start)
                {
                    IntEventText(day, c.DateStart.Day);
                    IntEventText(month, c.DateStart.Month);
                    IntEventText(year, c.DateStart.Year);
                }
                else
                {
                    IntEventText(day, c.DateEnd.Day);
                    IntEventText(month, c.DateEnd.Month);
                    IntEventText(year, c.DateEnd.Year);
                }
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

        // Проверка на пустоту всех полей
        public void CheckAll()
        {
            if (tb_name.Text == "" || tb_country.Text == "" || tb_city.Text == "" || tb_datestart.Text == "" || tb_dateend.Text == "" || tb_age.Text == "" || tb_rank.Text == "" || tb_score.Text == "" || dgv_player.Rows.Count != 1)
            {
                ChangeBut(but_confirm, false); Message(tb_mess, "Заполните все поля и выберите только одного игрока!", 2);
            }
            else
            {
                ChangeBut(but_confirm, true); tb_mess.Text = "";
            }
        }

        // Изменениее видимости для элементов
        public void ChangeBut(Button b, bool x)
        {
            b.Enabled = x; b.Visible = x;
        }
        public void ChangeTextbox(TextBox b, bool x)
        {
            b.Enabled = x; b.Visible = x;
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

        private void chb_unk_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_unk.Checked)
            {
                tb_end_d.Text = ""; tb_end_d.Enabled = false;
                tb_end_m.Text = ""; tb_end_m.Enabled = false; 
                tb_end_y.Text = ""; tb_end_y.Enabled = false; 
                tb_dateend.Text = "NULL"; tb_end_y.BackColor = SystemColors.ControlDark; tb_end_m.BackColor = SystemColors.ControlDark; tb_end_d.BackColor = SystemColors.ControlDark;
            }
            else
            {
                tb_end_d.Enabled = true; tb_end_d.BackColor = SystemColors.Window;
                tb_end_m.Enabled = true; tb_end_m.BackColor = SystemColors.Window;
                tb_end_y.Enabled = true; tb_end_y.BackColor = SystemColors.Window; tb_dateend.Text = "";
                CheckDate(tb_end_d, tb_end_m, tb_end_y, tb_dateend, false);
            }
            CheckAll();
        }


        private void tb_country_TextChanged(object sender, EventArgs e)
        {
            StringEventText(c.Country, tb_country); CheckAll();
        }

        private void tb_city_TextChanged(object sender, EventArgs e)
        {
            StringEventText(c.City, tb_city); CheckAll();
        }

        private void tb_start_d_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_start_d, tb_start_m, tb_start_y, tb_datestart, true);
        }

        private void tb_end_d_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_end_d, tb_end_m, tb_end_y, tb_dateend, false);
        }

        private void tb_start_m_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_start_d, tb_start_m, tb_start_y, tb_datestart, true);
        }

        private void tb_end_m_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_end_d, tb_end_m, tb_end_y, tb_dateend, false);
        }

        private void tb_start_y_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_start_d, tb_start_m, tb_start_y, tb_datestart, true);
        }

        private void tb_end_y_TextChanged(object sender, EventArgs e)
        {
            CheckDate(tb_end_d, tb_end_m, tb_end_y, tb_dateend, false);
        }

        private void tb_age_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(tb_age.Text); tb_age.BackColor = SystemColors.Info; IntEventText(tb_age, c.MaxAge);
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
                int value = Convert.ToInt32(tb_rank.Text); IntEventText(tb_rank, c.MinMasteryID);
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
            CheckAll(); IntEventText(tb_score, c.Score); 
        }

        private void tb_name_TextChanged(object sender, EventArgs e)
        {
            StringEventText(c.Name, tb_name); CheckAll();
        }

        private void but_coach_Click(object sender, EventArgs e)
        {
            ChangeBut(but_coach, false); ChangeTextbox(tb_messCoach, true);
            ChangeBut(but_okcoach, true); dgv_player.DataSource = GetDataFromDB($"SELECT * FROM dbo.GetBestCandidatesForCompetition({c.ID})");
             dgv_player.AllowUserToAddRows = false; CheckAll();
        }

        private void but_okcoach_Click(object sender, EventArgs e)
        {
            Message(tb_messCoach, "Выберите строчку с игроком из доступных", 0);
            ChangeBut(but_okcoach, false); ChangeBut(but_coach, true); ChangeTextbox(tb_messCoach, false);
            dgv_player.DataSource = GetDataFromDB($"SELECT * FROM dbo.vPlayersFullInfo WHERE PlayerID = {dgv_player.SelectedRows[0].Cells["PlayerID"].Value}");
            dgv_player.AllowUserToAddRows = false; CheckAll();
        }

        private void dgv_player_SelectionChanged(object sender, EventArgs e)
        {
            var sel = dgv_player.SelectedRows;
            if (sel.Count > 1)
            {
                Message(tb_messCoach, "Выберите одну строчку (одного игрока)", 2);
                ChangeBut(but_okcoach, false);
            }
            else if (sel.Count == 1)
            {
                Message(tb_messCoach, "Нажмите ОК для подтверждения", 1);
                ChangeBut(but_okcoach, true);
            }
            else
            {
                ChangeBut(but_okcoach, false);
            }
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
                int playerid = Convert.ToInt32(dgv_player.Rows[0].Cells["PlayerID"].Value);
                string name = tb_name.Text;
                string country = tb_country.Text;
                string city = tb_city.Text;
                int age = Convert.ToInt32(tb_age.Text);
                int score = Convert.ToInt32(tb_score.Text);
                int rank = Convert.ToInt32(tb_rank.Text);
                if (chb_unk.Checked)
                {
                    EditDataFromDB($"UPDATE guest.Competition SET Name = '{name}', Country = '{country}', City = '{city}', DateStart = '{date1}', MinMasteryID = {rank}, MaxAge = {age}, PlayerID = {playerid}, Score = {score} WHERE CompetitionID = {c.ID}");
                }
                else
                {
                    EditDataFromDB($"UPDATE guest.Competition SET Name = '{name}', Country = '{country}', City = '{city}', DateStart = '{date1}', DateEnd = '{date2}', MinMasteryID = {rank}, MaxAge = {age}, PlayerID = {playerid}, Score = {score} WHERE CompetitionID = {c.ID}");
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
                dgv_player.Enabled = false;
                Message(tb_mess, "Соревнование успешно изменено! Ожидайте!", 1);
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
