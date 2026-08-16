using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportCLUB
{
    public partial class PlayerForm : Form
    {
        public PlayerForm(Players p)
        {
            InitializeComponent(); this.p = p;
        }
        Players p; string change;

        private void PlayerForm_Load(object sender, EventArgs e)
        {
            tb_lastname.Text = p.Lastname;
            tb_firstname.Text = p.Firstname;
            tb_surname.Text = p.Surname;
            tb_id.Text = $"Игрок #{p.ID}";
            tb_age.Text = p.Age.ToString();
            tb_masteryscore.Text = p.MasteryScore.ToString();
            tb_rank.Text = GetDataFromDB($"SELECT MasteryID FROM guest.Players WHERE PlayerID = '{p.ID}'").Rows[0]["MasteryID"].ToString();
            tb_title.Text = GetDataFromDB($"SELECT Title FROM guest.Mastery_Players WHERE MasteryID = {Convert.ToInt32(tb_rank.Text)}").Rows[0]["Title"].ToString();
            dgv_comp.DataSource = GetDataFromDB($"SELECT * FROM guest.Competition WHERE PlayerID = {p.ID}"); dgv_comp.AllowUserToAddRows = false;
            dgv_travm.DataSource = GetDataFromDB($"SELECT * FROM guest.Injuries WHERE PlayerID = {p.ID}"); dgv_travm.AllowUserToAddRows = false;
            string oncomp = GetDataFromDB($"SELECT OnCompetitionNow FROM dbo.vPlayersFullInfo WHERE PlayerID = {p.ID}").Rows[0]["OnCompetitionNow"].ToString();
            string isinj = GetDataFromDB($"SELECT IsInjuredNow FROM dbo.vPlayersFullInfo WHERE PlayerID = {p.ID}").Rows[0]["IsInjuredNow"].ToString();
            if (oncomp == "Yes")
            {
                tb_status.Text = "На соревновании"; tb_status.BackColor = Color.FromArgb(255, 192, 192); tb_status.ForeColor = Color.Red;
            }
            else
            {
                tb_status.Text = "Свободен"; tb_status.BackColor = Color.FromArgb(192, 255, 192); tb_status.ForeColor = Color.DarkGreen;
            }
            if (isinj == "Yes")
            {
                tb_state.Text = "Травмирован"; tb_state.BackColor = Color.FromArgb(255, 192, 192); tb_state.ForeColor = Color.Red;
            }
            else
            {
                tb_state.Text = "Здоров"; tb_state.BackColor = Color.FromArgb(192, 255, 192); tb_state.ForeColor = Color.DarkGreen;
            }
            tb_email.Text = GetDataFromDB($"SELECT Email FROM dbo.vPlayersFullInfo").Rows[0]["Email"].ToString();
            tb_tel.Text = GetDataFromDB($"SELECT Phone FROM dbo.vPlayersFullInfo").Rows[0]["Phone"].ToString();
            tb_telp.Text = GetDataFromDB($"SELECT ParentPhone FROM dbo.vPlayersFullInfo").Rows[0]["ParentPhone"].ToString();
            change = GetDataFromDB($"SELECT Changeable FROM guest.Players WHERE PlayerID = {p.ID}").Rows[0]["Changeable"].ToString();
            dgv_coach.DataSource = GetDataFromDB($"SELECT LastName + ' ' + FirstName + ' ' + Surname AS 'Полное имя', Age AS 'Возраст', Experience_years AS 'Опыт в годах', MasteryID_Coach AS 'Ранг', Mastery_Score AS 'Рейтинг', Title AS 'Звание', MasteryID_MAXPlayers AS 'Максимальный ранг ученика' FROM guest.Coaches c INNER JOIN guest.Mastery_Coaches ms ON c.MasteryID_Coach = ms.MasteryID WHERE CoachID = {p.CoachID}");
            dgv_coach.AllowUserToAddRows = false;
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

        private void but_coaches_Click(object sender, EventArgs e)
        {
            if (change == "No")
            {
                but_coaches.Enabled = false; but_coaches.BackColor = Color.FromArgb(192, 255, 255); but_coaches.ForeColor = Color.Red;
                tb_mess.Text = "Вы уже меняли тренера, самостоятельно поменять тренера можно только один раз, обратитесь к администратору!"; tb_mess.ForeColor = Color.Red;
            }
            else
            {
                ChangeCoach F = new ChangeCoach(p); F.Show();
            }
        }
    }
}
