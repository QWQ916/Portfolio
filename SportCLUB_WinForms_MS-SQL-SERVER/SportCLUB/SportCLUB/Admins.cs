using System;
using System.Collections.Generic;
using System.Drawing;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportCLUB
{
    public class Admins : Access
    {
        TextBox tb; Timer timer; List<Admins> admins;
        int id; string lastname; string firstname; string surname; string login; string password;
        Access a;

        public int ID { get => id; }
        public string Lastname { get => lastname; }
        public string Firstname { get => firstname; }
        public string Surname { get => surname; }
        public string Login { get => login; }
        public string Password { get => password; }


        public Admins(int id, string lastname, string firstname, string surname, string login, string password)
        {
            this.id = id; this.firstname = firstname; this.lastname = lastname;
            this.surname = surname; this.login = login; this.password = password;
        }

        public Admins(TextBox tb, Timer timer, List<Admins> admins)
        {
            this.tb = tb; this.timer = timer; this.admins = admins;
        }


        // Методы для реализации паттерна проектирования для входа в систему
        public void LowerAccess(Access access)
        {
            a = access;
        }

        public void Log_in(Players player)
        {
            int k = 0;
            foreach (Admins a in admins)
            {
                if (player.Login == a.Login && player.Password == a.Password)
                {
                    k++; tb.Text = $"Добро пожаловать Администратор - {a.Firstname} {a.Surname}!"; tb.ForeColor = Color.DarkGreen;
                    timer.Enabled = true; break;
                }
            }
            if (k == 0) { a.Log_in(player); }
        }


        // Переопределение операторов сравнения
        public static bool operator ==(Admins a, Admins b)
        {
            return a.login == b.login && a.password == b.password;
        }
        public static bool operator !=(Admins a, Admins b)
        {
            return a.login != b.login || a.password != b.password;
        }
    }
}
