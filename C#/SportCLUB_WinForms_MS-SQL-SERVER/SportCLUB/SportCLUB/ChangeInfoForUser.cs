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
    public partial class ChangeInfoForUser : Form
    {
        public ChangeInfoForUser(Players p, AdminsForm F)
        {
            InitializeComponent();
            this.p = p; this.F = F;
        }
        AdminsForm F;
        Players p;
        string CipherKey = "SportCLUB2025021";

        private void ChangeInfoForUser_Load(object sender, EventArgs e)
        {
            tb_id.Text = $"Игрок #{p.ID.ToString()}";
            tb_lastname.Text = p.Lastname;
            tb_age.Text = p.Age.ToString();
            tb_firstname.Text = p.Firstname;
            tb_surname.Text = p.Surname.ToString();
            tb_masteryscore.Text = p.MasteryScore.ToString();
            tb_login.Text = p.Login;
            tb_password.Text = p.Password;
            tb_rank.Text = GetDataFromDB($"SELECT MasteryID FROM guest.Players WHERE PlayerID = '{p.ID}'").Rows[0]["MasteryID"].ToString();
            dgv_coach.DataSource = GetDataFromDB($"SELECT CoachID AS 'Код тренера', LastName AS 'Фамилия', FirstName AS 'Имя', Surname AS 'Отчество', Age AS 'Возраст', Experience_years AS 'Опыт преподавания (лет)', MasteryID_MAXPlayers AS 'Макс. ранг учеников', Title AS 'Звание'" +
                $"                                 FROM guest.Coaches AS c INNER JOIN guest.Mastery_Coaches AS mc ON c.MasteryID_Coach = mc.MasteryID WHERE CoachID = {p.CoachID}");
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone AS 'Телефон', Email, ParentPhone AS 'Телефон родителя', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Players AS ca INNER JOIN guest.Address_Players AS a ON ca.AddressID = a.AddressID WHERE ContactID = {p.ContactID}");
            if (p.Inclub)
            {
                rb_yes.Checked = true;
                rb_no.Checked = false;
            }
            else{
                rb_no.Checked = true;
                rb_yes.Checked = false;
            }
            dgv_coach.AllowUserToAddRows = false;
            dgv_address.AllowUserToAddRows = false;
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


        public void Check()
        {
            if (!butcoach && !butaddress)
            {
                ChangeBut(but_confirm, true);
            }
            else
            {
                ChangeBut(but_confirm, false);
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

        private void tb_login_TextChanged(object sender, EventArgs e)
        {
            StringEventText(p.Login, tb_login);
        }

        private void tb_password_TextChanged(object sender, EventArgs e)
        {
            StringEventText(p.Password, tb_password);
        }

        private void tb_age_TextChanged(object sender, EventArgs e)
        {
            IntEventText(tb_age, p.Age);
        }

        private void tb_masteryscore_TextChanged(object sender, EventArgs e)
        {
            IntEventText(tb_masteryscore, p.MasteryScore);
        }

        bool butcoach = false; bool butaddress = false;
        private void but_coach_Click(object sender, EventArgs e)
        {
            ChangeBut(but_coach, false); ChangeTextbox(tb_messCoach, true);
            int mid = Convert.ToInt32(GetDataFromDB($"SELECT MasteryID FROM guest.Players WHERE PlayerID = {p.ID}").Rows[0]["MasteryID"].ToString()); 
            ChangeBut(but_okcoach, true); dgv_coach.DataSource = GetDataFromDB($"SELECT CoachID AS 'Код тренера', LastName AS 'Фамилия', FirstName AS 'Имя', Surname AS 'Отчество', Age AS 'Возраст', Experience_years AS 'Опыт преподавания (лет)'," +
                $" MasteryID_MAXPlayers AS 'Макс. ранг учеников', Title AS 'Звание'" +
                $" FROM guest.Coaches AS c INNER JOIN guest.Mastery_Coaches AS mc ON c.MasteryID_Coach = mc.MasteryID WHERE MasteryID_MAXPlayers >= {mid} ");
            butcoach = true; Check(); dgv_coach.AllowUserToAddRows = false;
        }


        // Смена тренера
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

        private void but_okcoach_Click(object sender, EventArgs e)
        {
            Message(tb_messCoach, "Выберите строчку с тренером из доступных", 0);
            ChangeBut(but_okcoach, false); ChangeBut(but_coach, true); ChangeTextbox(tb_messCoach, false); 
            dgv_coach.DataSource = GetDataFromDB($"SELECT CoachID AS 'Код тренера', LastName AS 'Фамилия', FirstName AS 'Имя', Surname AS 'Отчество', Age AS 'Возраст', Experience_years AS 'Опыт преподавания (лет)', MasteryID_MAXPlayers AS 'Макс. ранг учеников', Title AS 'Звание' FROM guest.Coaches AS c INNER JOIN guest.Mastery_Coaches AS mc ON c.MasteryID_Coach = mc.MasteryID WHERE CoachID = {choosencoach}");
            choosencoach = -1; butcoach = false; Check(); dgv_coach.AllowUserToAddRows = false;
        }



        // Смена адреса
        private void but_address_Click(object sender, EventArgs e)
        {
            ChangeBut(but_address, false); ChangeTextbox(tb_messAddress, true); butaddress = true; Check(); 
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone AS 'Телефон', Email, ParentPhone AS 'Телефон родителя', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Players AS ca INNER JOIN guest.Address_Players AS a ON ca.AddressID = a.AddressID"); dgv_address.AllowUserToAddRows = false;
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

        private void but_okcontacts_Click(object sender, EventArgs e)
        {
            ChangeBut(but_okcontacts, false); ChangeTextbox(tb_messAddress, false); Message(tb_messAddress, "Выберите строчку с адресом", 0); ChangeBut(but_address, true); 
            dgv_address.DataSource = GetDataFromDB($"SELECT ContactID AS 'Код контактов', Phone AS 'Телефон', Email, ParentPhone AS 'Телефон родителя', CONCAT('город ', a.City, ', улица ', a.Street, ', дом ', a.Home, ', квартира - ', a.Room) AS 'Адрес', a.Postcode AS 'Почтовый индекс'" +
                $"                                   FROM guest.Contact_Players AS ca INNER JOIN guest.Address_Players AS a ON ca.AddressID = a.AddressID WHERE ContactID = {choosenadd}"); choosenadd = -1; butaddress = false; Check(); dgv_address.AllowUserToAddRows = false;
        }



        // Кнопка применения изменений
        private void but_confirm_Click(object sender, EventArgs e)
        {
            string lastname = tb_lastname.Text;
            string firstname = tb_firstname.Text;
            string surname = tb_surname.Text;
            int age = Convert.ToInt32(tb_age.Text);
            int masteryscore = Convert.ToInt32(tb_masteryscore.Text);
            int coachid = Convert.ToInt32(dgv_coach.Rows[0].Cells["Код тренера"].Value);
            int contactid = Convert.ToInt32(dgv_address.Rows[0].Cells["Код контактов"].Value);
            string login = tb_login.Text;
            string password = Encrypt(tb_password.Text, CipherKey);
            string isthere;
            if (rb_yes.Checked) { isthere = "Yes"; } else { isthere = "No"; }
            EditDataFromDB($"UPDATE guest.Players SET LastName = '{lastname}', FirstName = '{firstname}', Surname = '{surname}', Age = {age}, MasteryScore = {masteryscore}, CoachID = {coachid}, ContactID = {contactid}, Login = '{login}', Password = '{password}', IsInClubNow = '{isthere}' WHERE PlayerID = {p.ID}");
            ChangeBut(but_confirm, false);
            tb_lastname.Enabled = false;
            tb_firstname.Enabled = false;
            tb_surname.Enabled = false;
            tb_age.Enabled = false;
            tb_masteryscore.Enabled = false;
            dgv_coach.Enabled = false;
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
