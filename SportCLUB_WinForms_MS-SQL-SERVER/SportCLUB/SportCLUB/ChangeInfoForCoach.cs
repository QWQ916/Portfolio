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
    public partial class ChangeInfoForCoach : Form
    {
        public ChangeInfoForCoach(Coaches p, AdminsForm F)
        {
            InitializeComponent();
            this.p = p; this.F = F;
        }

        string CipherKey = "SportCLUB2025021";
        Coaches p;
        AdminsForm F;

        private void ChangeInfoForCoach_Load(object sender, EventArgs e)
        {
            tb_id.Text = $"Тренер #{p.ID.ToString()}";
            tb_lastname.Text = p.Lastname;
            tb_age.Text = p.Age.ToString();
            tb_firstname.Text = p.Firstname;
            tb_surname.Text = p.Surname.ToString();
            tb_masteryscore.Text = p.MasteryScore.ToString();
            tb_login.Text = p.Login;
            tb_experience.Text = p.Experience.ToString();
            tb_maxid.Text = p.Masteryidmaxplayers.ToString();
            tb_password.Text = p.Password;
            tb_rank.Text = GetDataFromDB($"SELECT MasteryID_Coach FROM guest.Coaches WHERE CoachID = '{p.ID}'").Rows[0]["MasteryID_Coach"].ToString();
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone1 AS 'Телефон основной', Phone2 AS 'Телефон дополнительный', EmailPrivate AS 'Личный Email', EmailCorp AS 'Корпоративный Email', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Coaches AS ca INNER JOIN guest.Address_Coaches AS a ON ca.AddressID = a.AddressID WHERE ContactID = {p.ContactID}");
            if (p.Inclub)
            {
                rb_yes.Checked = true;
                rb_no.Checked = false;
            }
            else
            {
                rb_no.Checked = true;
                rb_yes.Checked = false;
            }
            dgv_address.AllowUserToAddRows = false;
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




        // Функция проверки заполнения textbox
        public void StringEventText(string value, TextBox tb)
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

        private void rb_yes_CheckedChanged(object sender, EventArgs e)
        {
            if (p.Inclub && rb_yes.Checked)
            {
                rb_yes.Font = new Font(rb_yes.Font, FontStyle.Regular); rb_yes.ForeColor = Color.White;
                rb_no.Font = new Font(rb_no.Font, FontStyle.Regular); rb_no.ForeColor = Color.White;
            }
            else if (p.Inclub && !rb_yes.Checked)
            {
                rb_no.Font = new Font(rb_no.Font, FontStyle.Italic); rb_no.ForeColor = Color.DarkGreen;
                rb_yes.Font = new Font(rb_yes.Font, FontStyle.Regular); rb_yes.ForeColor = Color.White;
            }
            else if (!p.Inclub && rb_yes.Checked)
            {
                rb_yes.Font = new Font(rb_yes.Font, FontStyle.Italic); rb_yes.ForeColor = Color.DarkGreen;
                rb_no.Font = new Font(rb_no.Font, FontStyle.Regular); rb_no.ForeColor = Color.White;
            }
            else
            {
                rb_yes.Font = new Font(rb_yes.Font, FontStyle.Regular); rb_yes.ForeColor = Color.White;
                rb_no.Font = new Font(rb_no.Font, FontStyle.Regular); rb_no.ForeColor = Color.White;
            }
        }

        private void rb_no_CheckedChanged(object sender, EventArgs e)
        {
            if (p.Inclub && rb_yes.Checked)
            {
                rb_yes.Font = new Font(rb_yes.Font, FontStyle.Regular); rb_yes.ForeColor = Color.White;
                rb_no.Font = new Font(rb_no.Font, FontStyle.Regular); rb_no.ForeColor = Color.White;
            }
            else if (p.Inclub && !rb_yes.Checked)
            {
                rb_no.Font = new Font(rb_no.Font, FontStyle.Italic); rb_no.ForeColor = Color.DarkGreen;
                rb_yes.Font = new Font(rb_yes.Font, FontStyle.Regular); rb_yes.ForeColor = Color.White;
            }
            else if (!p.Inclub && rb_yes.Checked)
            {
                rb_yes.Font = new Font(rb_yes.Font, FontStyle.Italic); rb_yes.ForeColor = Color.DarkGreen;
                rb_no.Font = new Font(rb_no.Font, FontStyle.Regular); rb_no.ForeColor = Color.White;
            }
            else
            {
                rb_yes.Font = new Font(rb_yes.Font, FontStyle.Regular); rb_yes.ForeColor = Color.White;
                rb_no.Font = new Font(rb_no.Font, FontStyle.Regular); rb_no.ForeColor = Color.White;
            }
        }

        private void tb_lastname_TextChanged(object sender, EventArgs e)
        {
            StringEventText(p.Lastname, tb_lastname);
        }

        private void tb_firstname_TextChanged(object sender, EventArgs e)
        {
            StringEventText(p.Firstname, tb_firstname);
        }

        private void tb_surname_TextChanged(object sender, EventArgs e)
        {
            StringEventText(p.Surname, tb_surname);
        }

        private void tb_age_TextChanged(object sender, EventArgs e)
        {
            IntEventText(tb_age, p.Age);
        }

        private void tb_masteryscore_TextChanged(object sender, EventArgs e)
        {
            IntEventText(tb_masteryscore, p.MasteryScore);
        }

        private void tb_experience_TextChanged(object sender, EventArgs e)
        {
            IntEventText(tb_experience, p.Experience);
        }

        private void tb_maxid_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = tb_maxid; int val = p.Masteryidmaxplayers;
            try
            {
                int value = Convert.ToInt32(tb.Text);
                if (value <= 0 || value > 11)
                {
                    throw new Exception("ID ранга игрока должен быть в пределах от 1 до 11 включительно");
                }
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

        private void tb_password_TextChanged(object sender, EventArgs e)
        {
            StringEventText(p.Password, tb_password);
        }

        private void tb_login_TextChanged(object sender, EventArgs e)
        {
            StringEventText(p.Login, tb_login);
        }



        // Кнопка смены адреса
        private void but_address_Click(object sender, EventArgs e)
        {
            ChangeBut(but_confirm, false); ChangeBut(but_okcontacts, true); ChangeTextbox(tb_messAddress, true);
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone1 AS 'Телефон основной', Phone2 AS 'Телефон дополнительный', EmailPrivate AS 'Личный Email', EmailCorp AS 'Корпоративный Email', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Coaches AS ca INNER JOIN guest.Address_Coaches AS a ON ca.AddressID = a.AddressID"); dgv_address.AllowUserToAddRows = false;
        }

        int choosenadd = -1;
        private void dgv_address_SelectionChanged_1(object sender, EventArgs e)
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

        private void but_okcontacts_Click(object sender, EventArgs e)
        {
            ChangeBut(but_okcontacts, false); ChangeTextbox(tb_messAddress, false); Message(tb_messAddress, "Выберите строчку с адресом", 0); ChangeBut(but_address, true);
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone1 AS 'Телефон основной', Phone2 AS 'Телефон дополнительный', EmailPrivate AS 'Личный Email', EmailCorp AS 'Корпоративный Email', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Coaches AS ca INNER JOIN guest.Address_Coaches AS a ON ca.AddressID = a.AddressID WHERE ContactID = {choosenadd}"); dgv_address.AllowUserToAddRows = false; choosenadd = -1; ChangeBut(but_confirm, true);
        }


        // Кнопка применить
        private void but_confirm_Click(object sender, EventArgs e)
        {
            string lastname = tb_lastname.Text;
            string firstname = tb_firstname.Text;
            string surname = tb_surname.Text;
            int age = Convert.ToInt32(tb_age.Text);
            int masteryscore = Convert.ToInt32(tb_masteryscore.Text);
            int contactid = Convert.ToInt32(dgv_address.Rows[0].Cells["Код контактов"].Value);
            string login = tb_login.Text;
            int exp = Convert.ToInt32(tb_experience.Text);
            int maxid = Convert.ToInt32(tb_maxid.Text);
            string password = Encrypt(tb_password.Text, CipherKey);
            string isthere;
            if (rb_yes.Checked) { isthere = "Yes"; } else { isthere = "No"; }
            EditDataFromDB($"UPDATE guest.Coaches SET LastName = '{lastname}', FirstName = '{firstname}', Surname = '{surname}', Age = {age}, Mastery_Score = {masteryscore}, ContactID = {contactid}, Login = '{login}', Password = '{password}', IsInClubNow = '{isthere}', MasteryID_MAXPlayers = {maxid}, Experience_years = {exp} WHERE CoachID = {p.ID}");
            ChangeBut(but_confirm, false);
            tb_lastname.Enabled = false;
            tb_firstname.Enabled = false;
            tb_surname.Enabled = false;
            tb_age.Enabled = false;
            tb_masteryscore.Enabled = false;
            tb_experience.Enabled = false;
            tb_maxid.Enabled = false;
            dgv_address.Enabled = false;
            tb_login.Enabled = false;
            tb_password.Enabled = false;
            rb_no.Enabled = false;
            rb_yes.Enabled = false;
            t_close.Enabled = true;
        }

        private void t_close_Tick(object sender, EventArgs e)
        {
            t_close.Enabled = false; Close(); F.Show(); F.Restart();
        }
    }
}
