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
    public partial class CoachForm : Form
    {
        public CoachForm(Coaches c, List<Players> players)
        {
            InitializeComponent(); this.c = c; this.players = players;
        }
        Coaches c; List<Players> players; 

        private void CoachForm_Load(object sender, EventArgs e)
        {
            tb_lastname.Text = c.Lastname;
            tb_firstname.Text = c.Firstname;
            tb_surname.Text = c.Surname;
            tb_age.Text = c.Age.ToString();
            tb_masteryscore.Text = c.MasteryScore.ToString();
            tb_rank.Text = GetDataFromDB($"SELECT MasteryID_Coach FROM guest.Coaches WHERE CoachID = '{c.ID}'").Rows[0]["MasteryID_Coach"].ToString();
            tb_maxid.Text = c.Masteryidmaxplayers.ToString();
            tb_id.Text = $"Тренер #{c.ID}";
            tb_title.Text = GetDataFromDB($"SELECT Title FROM guest.Mastery_Coaches WHERE MasteryID = {Convert.ToInt32(tb_rank.Text)}").Rows[0]["Title"].ToString();
            tb_experience.Text = c.Experience.ToString();
            dgv_players.DataSource = GetDataFromDB($"SELECT PlayerID, LastName + ' ' + FirstName + ' ' + Surname AS 'Полное имя', Age AS 'Возраст', p.MasteryID AS 'Ранг', MasteryScore AS 'Рейтинг', Title AS 'Звание', Phone AS 'Телефон', Email, ParentPhone AS 'Телефон родителей' FROM guest.Players p INNER JOIN guest.Mastery_Players mp ON p.MasteryID = mp.MasteryID INNER JOIN guest.Contact_Players cp ON p.ContactID = cp.ContactID WHERE CoachID = {c.ID}");
            dgv_players.AllowUserToAddRows = false;
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

        private void dgv_players_SelectionChanged(object sender, EventArgs e)
        {
            int q = dgv_players.SelectedRows.Count;
            if (q != 1)
            {
                but_players.Enabled = false; but_players.Visible = false; tb_messPlayers.Text = "Выберите только одну строчку с игроком"; tb_messPlayers.ForeColor = Color.Red; but_inj.Enabled = false; but_inj.Visible = false;
                but_comp.Enabled = false; but_comp.Visible = false;
            }
            else
            {
                but_players.Enabled = true; but_players.Visible = true; tb_messPlayers.Text = "Нажмите сверху на кнопку с подробностями"; tb_messPlayers.ForeColor = Color.Green; but_inj.Enabled = true; but_inj.Visible = true;
                but_comp.Enabled = true; but_comp.Visible = true;
            }
        }

        private void but_players_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgv_players.SelectedRows[0].Cells["PlayerID"].Value);
            Players curr = new Players();
            foreach (Players p in players)
            {
                if (p.ID == id) 
                {
                    curr = p; break;
                }
            }
            ShowInfoPlayer F1 = new ShowInfoPlayer(curr); F1.Show();
        }

        private void but_inj_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgv_players.SelectedRows[0].Cells["PlayerID"].Value);
            Players curr = new Players();
            foreach (Players p in players)
            {
                if (p.ID == id)
                {
                    curr = p; break;
                }
            }
            AddInjures F2 = new AddInjures(curr); F2.Show();
        }

        private void but_comp_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgv_players.SelectedRows[0].Cells["PlayerID"].Value);
            Players curr = new Players();
            foreach (Players p in players)
            {
                if (p.ID == id)
                {
                    curr = p; break;
                }
            }
            SetPlayer F = new SetPlayer(curr); F.Show();
        }
    }
}
