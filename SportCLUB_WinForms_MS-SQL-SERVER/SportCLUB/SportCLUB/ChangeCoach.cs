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
    public partial class ChangeCoach : Form
    {
        public ChangeCoach(Players p)
        {
            InitializeComponent(); this.p = p;
        }
        Players p;

        private void ChangeCoach_Load(object sender, EventArgs e)
        {
            dgv_coach.DataSource = GetDataFromDB("SELECT CoachID AS 'Код тренера', LastName AS 'Фамилия', FirstName AS 'Имя', Surname AS 'Отчество', Age AS 'Возраст', Experience_years AS 'Опыт преподавания (лет)', MasteryID_MAXPlayers AS 'Макс. ранг учеников', Title AS 'Звание' FROM guest.Coaches AS c INNER JOIN guest.Mastery_Coaches AS mc ON c.MasteryID_Coach = mc.MasteryID"); dgv_coach.AllowUserToAddRows = false;
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

        private void dgv_coach_SelectionChanged(object sender, EventArgs e)
        {
            var sel = dgv_coach.SelectedRows;
            if (sel.Count > 1)
            {
                Message(tb_messCoach, "Выберите одну строчку (одного тренера)", 2);
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

        private void but_okcoach_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgv_coach.SelectedRows[0].Cells["Код тренера"].Value.ToString());
            Message(tb_messCoach, "Выберите строчку с тренером из доступных", 0);
            ChangeBut(but_okcoach, false); ChangeBut(but_coach, true); ChangeTextbox(tb_messCoach, false); ChangeBut(but_confirm, true);
            dgv_coach.DataSource = GetDataFromDB($"SELECT CoachID AS 'Код тренера', LastName AS 'Фамилия', FirstName AS 'Имя', Surname AS 'Отчество', Age AS 'Возраст', Experience_years AS 'Опыт преподавания (лет)', MasteryID_MAXPlayers AS 'Макс. ранг учеников', Title AS 'Звание' FROM guest.Coaches AS c INNER JOIN guest.Mastery_Coaches AS mc ON c.MasteryID_Coach = mc.MasteryID WHERE CoachID = {id}");
            dgv_coach.AllowUserToAddRows = false;
        }

        private void but_coach_Click(object sender, EventArgs e)
        {
            dgv_coach.DataSource = GetDataFromDB("SELECT CoachID AS 'Код тренера', LastName AS 'Фамилия', FirstName AS 'Имя', Surname AS 'Отчество', Age AS 'Возраст', Experience_years AS 'Опыт преподавания (лет)', MasteryID_MAXPlayers AS 'Макс. ранг учеников', Title AS 'Звание' FROM guest.Coaches AS c INNER JOIN guest.Mastery_Coaches AS mc ON c.MasteryID_Coach = mc.MasteryID"); dgv_coach.AllowUserToAddRows = false;
            ChangeBut(but_coach, false); ChangeBut(but_confirm, false); ChangeTextbox(tb_messCoach, true);
        }

        private void but_confirm_Click(object sender, EventArgs e)
        {
            int coachid = Convert.ToInt32(dgv_coach.Rows[0].Cells["Код тренера"].Value.ToString());
            EditDataFromDB($"EXEC dbo.GetAnotherCoach @PlayerID = {p.ID}, @CoachID_Pref = {coachid}");
            EditDataFromDB($"UPDATE guest.Players SET Changeable = 'No' WHERE PlayerID = {p.ID}");
            Message(tb_messCoach, "Тренер успешно поменян, ожидайте!", 1); ChangeTextbox(tb_messCoach, true);
            ChangeBut(but_confirm, false); ChangeBut(but_coach, false); t.Enabled = true;
        }

        private void t_Tick(object sender, EventArgs e)
        {
            t.Enabled = false; Application.Exit();
        }
    }
}
