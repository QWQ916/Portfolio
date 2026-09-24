using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportCLUB
{
    public class Coaches : Access
    {
        TextBox tb; Timer timer; List<Coaches> coaches;
        int id; string lastname; string firstname; string surname; int age; string login; int masteryidmaxplayers;
        int experience; int contactid; int masteryscore; bool inclub; Access a; string password;
        public static Coaches current;

        public int ID { get => id; }
        public string Lastname { get => lastname; }
        public string Firstname { get => firstname; }
        public string Surname { get => surname; }
        public string Login { get => login; }
        public string Password { get => password; }
        public int Age { get => age; }
        public int Experience { get => experience; }
        public int ContactID { get => contactid; }
        public int MasteryScore { get => masteryscore; }
        public bool Inclub { get => inclub; }
        public int Masteryidmaxplayers { get => masteryidmaxplayers; }


        // Конструктор для создания объекта
        public Coaches(int id, string lastname, string firstname, string surname, int age, int experience, int masteryidmaxplayers, int contactid, int masteryscore, string inclub, string login, string password)
        {
            this.id = id; this.age = age; this.lastname = lastname; this.firstname = firstname; this.masteryidmaxplayers = masteryidmaxplayers;
            this.surname = surname; this.contactid = contactid; this.experience = experience; 
            if (inclub == "Yes") { this.inclub = true; } else { this.inclub = false; }
            this.login = login; this.password = password;
            this.masteryscore = masteryscore;
        }

        public Coaches(TextBox tb, Timer timer, List<Coaches> coaches)
        {
            this.tb = tb;
            this.timer = timer;
            this.coaches = coaches;
        }


        // Методы для реализации паттерна проектирования для входа в систему
        public void LowerAccess(Access access)
        {
            a = access;
        }

        public void Log_in(Players player)
        {
            int k = 0;
            foreach (Coaches c in coaches)
            {
                if (player.Login == c.Login && player.Password == c.Password)
                {
                    k++; tb.Text = $"Добро пожаловать Тренер - {c.Firstname} {c.Surname}!"; tb.ForeColor = Color.DarkGreen;
                    timer.Enabled = true; current = c; break;
                }
            }
            if (k == 0) { a.Log_in(player); }
        }


        // Переопределение операторов сравнения
        public static bool operator ==(Coaches a, Coaches b)
        {
            return a.login == b.login && a.password == b.password;
        }
        public static bool operator !=(Coaches a, Coaches b)
        {
            return a.login != b.login || a.password != b.password;
        }
    }
}
