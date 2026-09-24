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
    public partial class AddCoach : Form
    {
        public AddCoach(AdminsForm F)
        {
            InitializeComponent(); this.F = F;
        }

        AdminsForm F;
        string CipherKey = "SportCLUB2025021";

        private void AddCoach_Load(object sender, EventArgs e)
        {
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone1 AS 'Телефон основной', Phone2 AS 'Телефон дополительный', EmailPrivate AS 'Личный Email', EmailCorp AS 'Корпоративный Email', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Coaches AS ca INNER JOIN guest.Address_Coaches AS a ON ca.AddressID = a.AddressID"); dgv_address.AllowUserToAddRows = false;
            int id = Convert.ToInt32(GetDataFromDB("SELECT TOP 1 CoachID FROM guest.Coaches ORDER BY CoachID DESC").Rows[0]["CoachID"].ToString()) + 1;
            tb_id.Text = $"Тренер #{id.ToString()}";
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

        public int GetIDByScore(int score)
        {
            DataTable dt = GetDataFromDB("SELECT MasteryID, MasteryMinScore FROM guest.Mastery_Coaches");
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

        // Проверка на пустоту String TextBox
        public void CheckStrings(TextBox tb)
        {
            if (tb.Text == "") { tb.BackColor = Color.FromArgb(255, 192, 192); }
            else { tb.BackColor = SystemColors.Info; }
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
                int value = Convert.ToInt32(tb_masteryscore.Text); tb_masteryscore.BackColor = SystemColors.Info;
                if (value < 0) { throw new Exception("Указан неверный рейтинг!"); }
                tb_mess.Text = ""; ChangeBut(but_confirm, true);
                tb_rank.Text = GetIDByScore(value).ToString(); 
            }
            catch (Exception ex)
            {
                tb_masteryscore.BackColor = Color.FromArgb(255, 192, 192);
                tb_mess.Text = $"Поле с красным фоном содержит ошибку: {ex.Message}"; tb_mess.ForeColor = Color.Red;
                ChangeBut(but_confirm, false);
            }
        }

        private void tb_experience_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(tb_experience.Text); tb_experience.BackColor = SystemColors.Info;
                if (value <= 0 || value > 80) { throw new Exception("Указан неверный опыт работы!"); }
                tb_mess.Text = ""; ChangeBut(but_confirm, true);
            }
            catch (Exception ex)
            {
                tb_experience.BackColor = Color.FromArgb(255, 192, 192);
                tb_mess.Text = $"Поле с красным фоном содержит ошибку: {ex.Message}"; tb_mess.ForeColor = Color.Red;
                ChangeBut(but_confirm, false);
            }
        }

        private void tb_maxid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(tb_maxid.Text); tb_maxid.BackColor = SystemColors.Info;
                if (value <= 0 || value > 11) { throw new Exception("Указан неверный ID мастерства игроков"); }
                tb_mess.Text = ""; ChangeBut(but_confirm, true);
            }
            catch (Exception ex)
            {
                tb_maxid.BackColor = Color.FromArgb(255, 192, 192);
                tb_mess.Text = $"Поле с красным фоном содержит ошибку: {ex.Message}"; tb_mess.ForeColor = Color.Red;
                ChangeBut(but_confirm, false);
            }
        }

        private void tb_lastname_TextChanged(object sender, EventArgs e)
        {
            CheckStrings(tb_lastname);
        }

        private void tb_firstname_TextChanged(object sender, EventArgs e)
        {
            CheckStrings(tb_firstname);
        }

        private void tb_surname_TextChanged(object sender, EventArgs e)
        {
            CheckStrings(tb_surname);
        }

        private void tb_login_TextChanged(object sender, EventArgs e)
        {
            CheckStrings(tb_login);
        }

        private void tb_password_TextChanged(object sender, EventArgs e)
        {
            CheckStrings(tb_password);
        }

        private void but_address_Click(object sender, EventArgs e)
        {
            ChangeBut(but_address, false); ChangeTextbox(tb_messAddress, true);
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone1 AS 'Телефон основной', Phone2 AS 'Телефон дополнительный', EmailPrivate AS 'Личный Email', EmailCorp AS 'Корпоративный Email', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Coaches AS ca INNER JOIN guest.Address_Coaches AS a ON ca.AddressID = a.AddressID"); dgv_address.AllowUserToAddRows = false;

        }

        private void but_okcontacts_Click(object sender, EventArgs e)
        {
            Message(tb_messAddress, "Выберите строчку с адресом", 0);
            ChangeBut(but_address, true); ChangeTextbox(tb_messAddress, false);
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone1 AS 'Телефон основной', Phone2 AS 'Телефон дополнительный', EmailPrivate AS 'Личный Email', EmailCorp AS 'Корпоративный Email', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Coaches AS ca INNER JOIN guest.Address_Coaches AS a ON ca.AddressID = a.AddressID WHERE ContactID = {choosenadd}"); dgv_address.AllowUserToAddRows = false;
        }

        private void but_confirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_lastname.Text == "" || tb_firstname.Text == "" || tb_surname.Text == "" || tb_login.Text == "" || tb_password.Text == "")
                {
                    throw new Exception("Заполните все поля!");
                }
                if (dgv_address.Rows.Count != 1)
                {
                    throw new Exception("Выберите ровно один адрес");
                }
                int id = Convert.ToInt32(GetDataFromDB("SELECT TOP 1 CoachID FROM guest.Coaches ORDER BY CoachID DESC").Rows[0]["CoachID"].ToString()) + 1;
                string lastname = tb_lastname.Text;
                string firstname = tb_firstname.Text;
                string surname = tb_surname.Text;
                string login = tb_login.Text;
                string password = Encrypt(tb_password.Text, CipherKey);
                int age = Convert.ToInt32(tb_age.Text);
                int masteryscore = Convert.ToInt32(tb_masteryscore.Text);
                int contactid = Convert.ToInt32(dgv_address.Rows[0].Cells["Код контактов"].Value);
                int exp = Convert.ToInt32(tb_experience.Text);
                int maxid = Convert.ToInt32(tb_maxid.Text);
                EditDataFromDB($"INSERT INTO guest.Coaches(CoachID, LastName, FirstName, Surname, Age, ContactID, Experience_years, MasteryID_MAXPlayers, Mastery_Score, IsInClubNow, Login, Password) VALUES ({id}, '{lastname}', '{firstname}', '{surname}', {age}, {contactid}, {exp}, {maxid}, {masteryscore}, 'Yes', '{login}', '{password}')");
                tb_lastname.Enabled = false;
                tb_firstname.Enabled = false;
                tb_surname.Enabled = false;
                tb_age.Enabled = false;
                tb_experience.Enabled = false;
                tb_maxid.Enabled = false;
                tb_masteryscore.Enabled = false;
                tb_login.Enabled = false;
                tb_password.Enabled = false;
                dgv_address.Enabled = false;
                Message(tb_mess, "Тренер успешно создан! Ожидайте!", 1);
                t.Enabled = true;
                ChangeBut(but_confirm, false);
            }
            catch (Exception ex)
            {
                Message(tb_mess, ex.Message, 2);
            }
        }

        private void t_Tick(object sender, EventArgs e)
        {
            t.Enabled = false; F.Restart(); Close();
        }
    }
}
