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
    public partial class AddPlayer : Form
    {
        public AddPlayer(AdminsForm F)
        {
            InitializeComponent(); this.F = F;
        }
        AdminsForm F;
        private void AddPlayer_Load(object sender, EventArgs e)
        {
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone AS 'Телефон', Email, ParentPhone AS 'Телефон родителя', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Players AS ca INNER JOIN guest.Address_Players AS a ON ca.AddressID = a.AddressID"); dgv_address.AllowUserToAddRows = false; 
            int maxid = Convert.ToInt32(GetDataFromDB("SELECT TOP 1 PlayerID FROM guest.Players ORDER BY PlayerID DESC").Rows[0]["PlayerID"].ToString()) + 1;
            tb_id.Text = $"Игрок #{maxid.ToString()}";
        }

        int masteryscore;
        string CipherKey = "SportCLUB2025021";

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



        private void tb_age_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(tb_age.Text); tb_age.BackColor = SystemColors.Info;
                if (value <= 0 || value > 80) { throw new Exception("Указан неверный возраст"); }
                tb_mess.Text = ""; ChangeBut(but_confirm, true);
            }
            catch (Exception ex)
            {
                tb_age.BackColor = Color.FromArgb(255, 192, 192);
                tb_mess.Text = $"Поле с красным фоном содержит ошибку: {ex.Message}"; tb_mess.ForeColor = Color.Red;
                ChangeBut(but_confirm, false);
            }
        }

        private void tb_masteryscore_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(tb_masteryscore.Text);
                if (value < 0)
                {
                    throw new Exception("Указан неверный рейтинг");
                }
                masteryscore = value;
                int mid = GetIDByScore(masteryscore);
                tb_mess.Text = ""; ChangeBut(but_confirm, true);
                dgv_coach.DataSource = GetDataFromDB($"SELECT CoachID AS 'Код тренера', LastName AS 'Фамилия', FirstName AS 'Имя', Surname AS 'Отчество', Age AS 'Возраст', Experience_years AS 'Опыт преподавания (лет)'," +
                $" MasteryID_MAXPlayers AS 'Макс. ранг учеников', Title AS 'Звание'" +
                $" FROM guest.Coaches AS c INNER JOIN guest.Mastery_Coaches AS mc ON c.MasteryID_Coach = mc.MasteryID WHERE MasteryID_MAXPlayers >= {mid} "); tb_masteryscore.BackColor = SystemColors.Info;
                tb_rank.Text = mid.ToString(); dgv_coach.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                tb_masteryscore.BackColor = Color.FromArgb(255, 192, 192);
                tb_mess.Text = $"Поле с красным фоном содержит ошибку: {ex.Message}"; tb_mess.ForeColor = Color.Red;
                ChangeBut(but_confirm, false);
            }
        }

        int choosencoach = -1;
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
                choosencoach = Convert.ToInt32(sel[0].Cells[0].Value);
            }
            else
            {
                ChangeBut(but_okcoach, false);
            }
        }


        int choosenadd = -1;
        private void dgv_address_SelectionChanged(object sender, EventArgs e)
        {
            var sel = dgv_address.SelectedRows;
            if (sel.Count > 1)
            {
                Message(tb_messAddress, "Выберите одну строчку (один адрес)", 2);
                ChangeBut(but_okcontacts, false);
            }
            else if (sel.Count == 1)
            {
                Message(tb_messAddress, "Нажмите ОК для подтверждения", 1);
                ChangeBut(but_okcontacts, true);
                choosenadd = Convert.ToInt32(sel[0].Cells[0].Value);
            }
            else
            {
                ChangeBut(but_okcontacts, false);
            }
        }


        private void but_okcoach_Click(object sender, EventArgs e)
        {
            Message(tb_messCoach, "Выберите строчку с тренером из доступных", 0);
            ChangeBut(but_okcoach, false); ChangeBut(but_coach, true); ChangeTextbox(tb_messCoach, false);
            dgv_coach.DataSource = GetDataFromDB($"SELECT CoachID AS 'Код тренера', LastName AS 'Фамилия', FirstName AS 'Имя', Surname AS 'Отчество', Age AS 'Возраст', Experience_years AS 'Опыт преподавания (лет)', MasteryID_MAXPlayers AS 'Макс. ранг учеников', Title AS 'Звание' FROM guest.Coaches AS c INNER JOIN guest.Mastery_Coaches AS mc ON c.MasteryID_Coach = mc.MasteryID WHERE CoachID = {choosencoach}");
            choosencoach = -1; dgv_coach.AllowUserToAddRows = false;
        }

        private void but_okcontacts_Click(object sender, EventArgs e)
        {
            Message(tb_messAddress, "Выберите строчку с адресом", 0);
            ChangeBut(but_address, true); ChangeTextbox(tb_messAddress, false); 
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone AS 'Телефон', Email, ParentPhone AS 'Телефон родителя', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Players AS ca INNER JOIN guest.Address_Players AS a ON ca.AddressID = a.AddressID WHERE ContactID = {choosenadd}"); dgv_address.AllowUserToAddRows = false;
        }

        private void but_coach_Click(object sender, EventArgs e)
        {
            int mid = GetIDByScore(masteryscore);
            ChangeBut(but_coach, false); ChangeTextbox(tb_messCoach, true);
            ChangeBut(but_okcoach, true); dgv_coach.DataSource = GetDataFromDB($"SELECT CoachID AS 'Код тренера', LastName AS 'Фамилия', FirstName AS 'Имя', Surname AS 'Отчество', Age AS 'Возраст', Experience_years AS 'Опыт преподавания (лет)'," +
                $" MasteryID_MAXPlayers AS 'Макс. ранг учеников', Title AS 'Звание'" +
                $" FROM guest.Coaches AS c INNER JOIN guest.Mastery_Coaches AS mc ON c.MasteryID_Coach = mc.MasteryID WHERE MasteryID_MAXPlayers >= {mid} ");
            dgv_coach.AllowUserToAddRows = false;
        }

        private void but_address_Click(object sender, EventArgs e)
        {
            ChangeBut(but_address, false); ChangeTextbox(tb_messAddress, true);
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone AS 'Телефон', Email, ParentPhone AS 'Телефон родителя', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Players AS ca INNER JOIN guest.Address_Players AS a ON ca.AddressID = a.AddressID"); dgv_address.AllowUserToAddRows = false;
        }


        public int GetIDByScore(int score)
        {
            DataTable dt = GetDataFromDB("SELECT MasteryID, MasteryMinScore FROM guest.Mastery_Players");
            foreach (DataRow dr in dt.Rows)
            {
                int minsore = Convert.ToInt32(dr["MasteryMinScore"].ToString());
                if (minsore > score)
                {
                    int id = Convert.ToInt32(dr["MasteryID"].ToString());
                    return id - 1;
                }
            }
            return 11;
        }

        private void tb_lastname_TextChanged(object sender, EventArgs e)
        {
            if (tb_lastname.Text == "") { tb_lastname.BackColor = Color.FromArgb(255, 192, 192); }
            else { tb_lastname.BackColor = SystemColors.Info; }
        }

        private void tb_firstname_TextChanged(object sender, EventArgs e)
        {
            if (tb_firstname.Text == "") { tb_firstname.BackColor = Color.FromArgb(255, 192, 192); }
            else { tb_firstname.BackColor = SystemColors.Info; }
        }

        private void tb_surname_TextChanged(object sender, EventArgs e)
        {
            if (tb_surname.Text == "") { tb_surname.BackColor = Color.FromArgb(255, 192, 192); }
            else { tb_surname.BackColor = SystemColors.Info; }
        }

        private void tb_login_TextChanged(object sender, EventArgs e)
        {
            if (tb_login.Text == "") { tb_login.BackColor = Color.FromArgb(255, 192, 192); }
            else { tb_login.BackColor = SystemColors.Info; }
        }

        private void tb_password_TextChanged(object sender, EventArgs e)
        {
            if (tb_password.Text == "") { tb_password.BackColor = Color.FromArgb(255, 192, 192); }
            else { tb_password.BackColor = SystemColors.Info; }
        }


        // Кнопка применить
        private void but_confirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_lastname.Text == "" || tb_firstname.Text == "" || tb_surname.Text == "" || tb_login.Text == "" || tb_password.Text == "")
                {
                    throw new Exception("Заполните все поля!");
                }
                if (dgv_address.Rows.Count != 1 || dgv_coach.Rows.Count != 1)
                {
                    throw new Exception("Выберите ровно одного тренера для игрока и один его адрес");
                }
                int id = Convert.ToInt32(GetDataFromDB("SELECT TOP 1 PlayerID FROM guest.Players ORDER BY PlayerID DESC").Rows[0]["PlayerID"].ToString()) + 1;
                string lastname = tb_lastname.Text;
                string firstname = tb_firstname.Text;
                string surname = tb_surname.Text;
                string login = tb_login.Text;
                string password = Encrypt(tb_password.Text, CipherKey);
                int age = Convert.ToInt32(tb_age.Text);
                int masteryscore = Convert.ToInt32(tb_masteryscore.Text);
                int coachid = Convert.ToInt32(dgv_coach.Rows[0].Cells["Код тренера"].Value);
                int contactid = Convert.ToInt32(dgv_address.Rows[0].Cells["Код контактов"].Value);
                EditDataFromDB($"INSERT INTO guest.Players(PlayerID, LastName, FirstName, Surname, Age, CoachID, ContactID, MasteryScore, IsInClubNow, Login, Password) VALUES ({id}, '{lastname}', '{firstname}', '{surname}', {age}, {coachid}, {contactid}, {masteryscore}, 'Yes', '{login}', '{password}')");
                tb_lastname.Enabled = false;
                tb_firstname.Enabled = false;
                tb_surname.Enabled = false;
                tb_age.Enabled = false;
                tb_masteryscore.Enabled = false;
                tb_login.Enabled = false;
                tb_password.Enabled = false;
                dgv_address.Enabled = false;
                dgv_coach.Enabled = false;
                Message(tb_mess, "Игрок успешно создан! Ожидайте!", 1);
                ChangeBut(but_confirm, false);
                t.Enabled = true; 
            }
            catch (Exception ex)
            {
                Message(tb_mess, ex.Message, 2);
            }
        }

        private void t_Tick(object sender, EventArgs e)
        {
            t.Enabled = false; Close(); F.Restart();
        }
    }
}
