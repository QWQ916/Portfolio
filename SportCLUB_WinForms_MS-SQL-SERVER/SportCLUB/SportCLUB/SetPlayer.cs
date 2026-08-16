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
    public partial class SetPlayer : Form
    {
        public SetPlayer(Players p)
        {
            InitializeComponent(); this.p = p;
        }
        Players p;
        int rank;
        private void SetPlayer_Load(object sender, EventArgs e)
        {
            tb_lastname.Text = p.Lastname;
            tb_firstname.Text = p.Firstname;
            tb_surname.Text = p.Surname;
            tb_id.Text = $"Игрок #{p.ID}";
            rank = Convert.ToInt32(GetDataFromDB($"SELECT MasteryID FROM guest.Players WHERE PlayerID = '{p.ID}'").Rows[0]["MasteryID"].ToString());
            tb_masteryscore.Text = p.MasteryScore.ToString();
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
            if (isinj == "Yes" || oncomp == "Yes")
            {
                dgv.Enabled = false; tb_mess.Text = "В данный момент нельзя назначить игрока на соревнование, так как он травмирован или уже на соревновании!"; tb_mess.ForeColor = Color.Red;
                but_change.Enabled = false; but_change.Visible = false; but_confirm.Visible = false; but_confirm.Enabled = false;
            }
            else
            {
                dgv.DataSource = GetDataFromDB($"SELECT * FROM guest.Competition WHERE MinMasteryID < {rank} AND MaxAge > {p.Age}"); 
                dgv.AllowUserToAddRows = false; tb_mess.Text = "Выберите только одну строчку с соревнованием!"; tb_mess.ForeColor = Color.Red;
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

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            int c = dgv.SelectedRows.Count;
            if (c != 1)
            {
                tb_mess.Text = "Выберите только одну строчку с соревнованием!"; tb_mess.ForeColor = Color.Red;
                but_ok.Enabled = false; but_ok.Visible = false;
            }
            else
            {
                tb_mess.Text = "Нажмите ОК для подтверждения"; tb_mess.ForeColor = Color.DarkGreen; but_ok.Enabled = true; but_ok.Visible = true;
            }
        }

        private void but_change_Click(object sender, EventArgs e)
        {
            but_ok.Enabled = false; but_ok.Visible = false; tb_mess.Text = "Выберите только одну строчку с соревнованием!"; tb_mess.ForeColor = Color.Red;
            dgv.DataSource = GetDataFromDB($"SELECT * FROM guest.Competition WHERE MinMasteryID < {rank} AND MaxAge > {p.Age}");
            dgv.AllowUserToAddRows = false; but_change.Enabled = false; but_change.Visible = false; but_confirm.Enabled = false; but_confirm.Visible = false;
        }

        private void but_ok_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgv.SelectedRows[0].Cells["CompetitionID"].Value.ToString());
            dgv.DataSource = GetDataFromDB($"SELECT * FROM guest.Competition WHERE CompetitionID = {id}"); tb_mess.Text = "";
            dgv.AllowUserToAddRows = false; but_change.Enabled = true; but_change.Visible = true; but_confirm.Enabled = true; but_confirm.Visible = true;
        }

        private void but_confirm_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgv.Rows[0].Cells["CompetitionID"].Value.ToString());
            EditDataFromDB($"UPDATE guest.Competition SET PlayerID = {p.ID} WHERE CompetitionID = {id}"); t.Enabled = true;
        }

        private void t_Tick(object sender, EventArgs e)
        {
            t.Enabled = false; tb_mess.Enabled = true; tb_mess.Visible = true; tb_mess.Text = "Изменения приняты! Ожидайте!"; tb_mess.ForeColor = Color.DarkGreen; Application.Exit();
        }
    }
}
