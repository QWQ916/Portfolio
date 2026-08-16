using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportCLUB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string CipherKey = "SportCLUB2025021";
        
        List<Admins> admins = new List<Admins>();
        List<Coaches> coaches = new List<Coaches>();
        List<Players> players = new List<Players>();

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable da = GetDataFromDB("SELECT AdminID, Lastname, Firstname, Surname, Login, Password FROM dbo.Admins");
            foreach (DataRow v in da.Rows)
            {
                string p = v["Password"].ToString();
                string pp = Decrypt(p, CipherKey);
                admins.Add(new Admins(Convert.ToInt32(v["AdminID"].ToString()), v["Lastname"].ToString(), v["Firstname"].ToString(), v["Surname"].ToString(), v["Login"].ToString(), pp));
            }

            DataTable dc = GetDataFromDB("SELECT CoachID, LastName, FirstName, Surname, Age, Experience_years, MasteryID_MAXPlayers, ContactID, Mastery_Score, IsInClubNow, Login, Password FROM guest.Coaches");
            foreach (DataRow v in dc.Rows)
            {
                string p = v["Password"].ToString();
                string pp = Decrypt(p, CipherKey);
                coaches.Add(new Coaches(Convert.ToInt32(v["CoachID"].ToString()), v["LastName"].ToString(), v["FirstName"].ToString(), v["Surname"].ToString(), Convert.ToInt32(v["Age"].ToString()), Convert.ToInt32(v["Experience_years"].ToString()), Convert.ToInt32(v["MasteryID_MAXPlayers"].ToString()), Convert.ToInt32(v["ContactID"].ToString()), Convert.ToInt32(v["Mastery_Score"].ToString()), v["IsInClubNow"].ToString(), v["Login"].ToString(), pp));
            }

            DataTable dp = GetDataFromDB("SELECT PlayerID, LastName, FirstName, Surname, Age, CoachID, ContactID, MasteryScore, IsInClubNow, Login, Password FROM guest.Players");
            foreach (DataRow v in dp.Rows)
            {
                string p = v["Password"].ToString();
                string pp = Decrypt(p, CipherKey);
                players.Add(new Players(Convert.ToInt32(v["PlayerID"].ToString()), v["LastName"].ToString(), v["FirstName"].ToString(), v["Surname"].ToString(), Convert.ToInt32(v["Age"].ToString()), Convert.ToInt32(v["CoachID"].ToString()), Convert.ToInt32(v["ContactID"].ToString()), Convert.ToInt32(v["MasteryScore"].ToString()), v["IsInClubNow"].ToString(), v["Login"].ToString(), pp));
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


        // Кнопка входа
        private void but_enter_Click(object sender, EventArgs e)
        {
            string log = tb_login.Text; string pass = tb_password.Text;

            // Реализация паттерна проектирования ChainResponsibillity
            Access Role1 = new Admins(tb_message, t_admin, admins);
            Access Role2 = new Coaches(tb_message, t_coach, coaches);
            Access Role3 = new Players(tb_message, t_player, players);

            Role1.LowerAccess(Role2);
            Role2.LowerAccess(Role3);

            if (log == "" || pass == "")
            {
                tb_message.Text = "Логин и пароль не могут быть пустыми!"; tb_message.ForeColor = Color.Red;
            }
            else
            {
                Players input = new Players(log, pass);
                Role1.Log_in(input);
            }
        }


        // Таймеры для перехода на соответствующую форму
        private void t_admin_Tick(object sender, EventArgs e)
        {
            t_admin.Enabled = false; AdminsForm FA = new AdminsForm(players, coaches); Hide(); FA.Show();
        }

        private void t_coach_Tick(object sender, EventArgs e)
        {
            t_coach.Enabled = false; CoachForm FC = new CoachForm(Coaches.current, players); Hide(); FC.Show();
        }

        private void t_player_Tick(object sender, EventArgs e)
        {
            t_player.Enabled = false; PlayerForm FP = new PlayerForm(Players.current); Hide(); FP.Show();
        }
        

        // Показ пароля
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cb.Checked)
            {
                tb_password.PasswordChar = '\0';
            }
            else
            {
                tb_password.PasswordChar = '•';
            }
        }
    }
}

