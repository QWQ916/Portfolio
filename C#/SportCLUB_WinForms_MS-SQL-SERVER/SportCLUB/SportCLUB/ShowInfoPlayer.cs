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
    public partial class ShowInfoPlayer : Form
    {
        public ShowInfoPlayer(Players p)
        {
            InitializeComponent(); this.p = p;
        }
        Players p;

        private void ShowInfoPlayer_Load(object sender, EventArgs e)
        {
            tb_lastname1.Text = p.Lastname;
            tb_firstname1.Text = p.Firstname;
            tb_surname1.Text = p.Surname;
            tb_id1.Text = $"Игрок #{p.ID}";
            tb_age1.Text = p.Age.ToString();
            tb_masteryscore1.Text = p.MasteryScore.ToString();
            tb_rank1.Text = GetDataFromDB($"SELECT MasteryID FROM guest.Players WHERE PlayerID = '{p.ID}'").Rows[0]["MasteryID"].ToString();
            tb_title1.Text = GetDataFromDB($"SELECT Title FROM guest.Mastery_Players WHERE MasteryID = {Convert.ToInt32(tb_rank1.Text)}").Rows[0]["Title"].ToString();
            dgv_comp1.DataSource = GetDataFromDB($"SELECT * FROM guest.Competition WHERE PlayerID = {p.ID}"); dgv_comp1.AllowUserToAddRows = false; 
            dgv_travm1.DataSource = GetDataFromDB($"SELECT * FROM guest.Injuries WHERE PlayerID = {p.ID}"); dgv_travm1.AllowUserToAddRows = false;
            string oncomp = GetDataFromDB($"SELECT OnCompetitionNow FROM dbo.vPlayersFullInfo WHERE PlayerID = {p.ID}").Rows[0]["OnCompetitionNow"].ToString();
            string isinj = GetDataFromDB($"SELECT IsInjuredNow FROM dbo.vPlayersFullInfo WHERE PlayerID = {p.ID}").Rows[0]["IsInjuredNow"].ToString();
            if (oncomp == "Yes")
            {
                tb_status1.Text = "На соревновании"; tb_status1.BackColor = Color.FromArgb(255, 192, 192); tb_status1.ForeColor = Color.Red;
            }
            else
            {
                tb_status1.Text = "Свободен"; tb_status1.BackColor = Color.FromArgb(192, 255, 192); tb_status1.ForeColor = Color.DarkGreen;
            }
            if (isinj == "Yes")
            {
                tb_state1.Text = "Травмирован"; tb_state1.BackColor = Color.FromArgb(255, 192, 192); tb_state1.ForeColor = Color.Red;
            }
            else
            {
                tb_state1.Text = "Здоров"; tb_state1.BackColor = Color.FromArgb(192, 255, 192); tb_state1.ForeColor = Color.DarkGreen;
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
    }
}
