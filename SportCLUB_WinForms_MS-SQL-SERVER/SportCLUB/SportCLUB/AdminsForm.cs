using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportCLUB
{
    public partial class AdminsForm : Form
    {
        public AdminsForm(List<Players> players, List<Coaches> coaches)
        {
            InitializeComponent();
            this.players = players;
            this.coaches = coaches;
        }
        public void Restart()
        {
            Application.Restart(); 
        }
        private void AdminsForm_Load(object sender, EventArgs e)
        {
            cb1.SelectedIndex = 0;
            BuildTable(0);
            dgv_competition.DataSource = GetDataFromDB("SELECT * FROM guest.Competition"); dgv_competition.AllowUserToAddRows = false;
            DataTable da = GetDataFromDB("SELECT CompetitionID, Name, Country, City, DateStart, DateEnd, MinMasteryID, MaxAge, PlayerID, Score FROM guest.Competition");
            foreach (DataRow v in da.Rows)
            {
                int id = Convert.ToInt32(v["CompetitionID"].ToString());
                string name = v["Name"].ToString();
                string country = v["Country"].ToString();
                string city = v["City"].ToString();
                DateTime datestart = Convert.ToDateTime(v["DateStart"].ToString());
                DateTime dateend; 
                if (v["DateEnd"].ToString() == "") { dateend = new DateTime(1, 1, 1); } else { dateend = Convert.ToDateTime(v["DateEnd"].ToString()); }
                int minmasteryid = Convert.ToInt32(v["MinMasteryID"].ToString());
                int maxage = Convert.ToInt32(v["MaxAge"].ToString());
                int playerid = Convert.ToInt32(v["PlayerID"].ToString());
                int score = Convert.ToInt32(v["Score"].ToString());
                Competitions c = new Competitions(id, name, country, city, datestart, dateend, minmasteryid, maxage, playerid, score);
                competitions.Add(c);
            }
        }

        List<Players> players;
        List<Coaches> coaches;
        List<Competitions> competitions = new List<Competitions>();



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


        // Функция шифрования паролей
        public string Encrypt(string text, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16]; // Инициализационный вектор (можно оставить нулями)

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                byte[] encrypted = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                return Convert.ToBase64String(encrypted);
            }
        }

        // Функция расшифровки паролей
        public string Decrypt(string encryptedText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16];

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] buffer = Convert.FromBase64String(encryptedText);
                byte[] decrypted = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(decrypted);
            }
        }

        // Функция заполнения таблицы
        public void BuildTable(int index)
        {
            dgv.AllowUserToAddRows = false;
            if (index == 0)
            {
                dgv.DataSource = GetDataFromDB("SELECT * FROM dbo.vPlayersFullInfo");
            }
            else
            {
                dgv.DataSource = GetDataFromDB("SELECT * FROM dbo.vCoachesFullInfo");
            }
        }

        private void cb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildTable(cb1.SelectedIndex);
        }



        // Кнопка для изменения данных пользователя
        private void but_change_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgv.SelectedRows[0];
            int index = row.Index;
            if (cb1.SelectedIndex == 0)
            {
                Players user = players[index];
                ChangeInfoForUser F1 = new ChangeInfoForUser(user, this); F1.Show();
            }
            else
            {
                Coaches user = coaches[index];
                ChangeInfoForCoach F1 = new ChangeInfoForCoach(user, this); F1.Show();
            }
        }


        // Выбор одной строки с пользователем
        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            var sel = dgv.SelectedRows;
            if (sel.Count == 1)
            {
                but_change.Enabled = true;
                but_change.Visible = true;
            }
            else
            {
                but_change.Enabled = false;
                but_change.Visible = false;
            }
        }

        private void but_createPlayer_Click(object sender, EventArgs e)
        {
            AddPlayer A1 = new AddPlayer(this); A1.Show();
        }

        private void but_addCoach_Click(object sender, EventArgs e)
        {
            AddCoach A2 = new AddCoach(this); A2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddCompetition A3 = new AddCompetition(this); A3.Show();
        }

        private void but_changecomp_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgv_competition.SelectedRows[0];
            int index = row.Index;
            Competitions c = competitions[index];
            ChangeInfoComp A4 = new ChangeInfoComp(this, c); A4.Show();
        }

        private void dgv_competition_SelectionChanged(object sender, EventArgs e)
        {
            int c = dgv_competition.SelectedRows.Count;
            if (c == 1)
            {
                but_changecomp.Enabled = true;
                but_changecomp.Visible = true;
            }
            else
            {
                but_changecomp.Enabled = false;
                but_changecomp.Visible = false;
            }
        }
    }
}
