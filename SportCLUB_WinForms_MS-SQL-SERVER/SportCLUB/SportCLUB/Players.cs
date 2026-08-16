using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportCLUB
{
    public class Players : Access
    {
        TextBox tb; Timer timer; List<Players> players;
        int id; string lastname; string firstname; string surname; int age; string login;
        int coachid; int contactid; int masteryscore; bool inclub; Access a; string password;

        public static Players current;

        public int ID { get => id; }
        public string Lastname { get => lastname; }
        public string Firstname { get => firstname; }
        public string Surname { get => surname; }
        public string Login { get => login; }
        public string Password { get => password; }
        public int Age { get => age; }
        public int CoachID { get => coachid; }
        public int ContactID { get => contactid; }
        public int MasteryScore { get => masteryscore; }
        public bool Inclub { get => inclub; }



        // Конструктор для создания объекта
        public Players(int id, string lastname, string firstname, string surname, int age, int coachid, int contactid, int masteryscore, string inclub, string login, string password)
        {
            this.id = id; this.age = age; this.lastname = lastname; this.firstname = firstname; this.login = login; this.password = password;
            this.surname = surname; this.contactid = contactid; this.coachid = coachid;
            if (inclub == "Yes") { this.inclub = true; } else { this.inclub = false; }
            this.masteryscore = masteryscore;
        }

        public Players(string login, string password)
        {
            this.login = login; this.password = password;
        }

        public Players(TextBox tb, Timer timer, List<Players> players)
        {
            this.tb = tb;
            this.timer = timer;
            this.players = players;
        }

        public Players()
        {
            
        }


        // Методы для реализации паттерна проектирования для входа в систему
        public void LowerAccess(Access access)
        {
            a = access;
        }

        public void Log_in(Players player)
        {
            int k = 0;
            foreach (var v in players)
            {
                if (v == player)
                {
                    tb.ForeColor = Color.DarkGreen; tb.Text = $"Добро пожаловать Игрок - {v.Firstname} {v.Surname}!"; k++; current = v;
                    timer.Enabled = true;
                    break;
                }
            }
            if (k == 0) { tb.Text = "Данный пользователь не найден! Попробуйте перепроверить информацию!"; tb.ForeColor = Color.Red; }
        }


        // Переопределение операторов сравнения
        public static bool operator ==(Players a, Players b)
        {
            return a.login == b.login && a.password == b.password;
        }
        public static bool operator !=(Players a, Players b)
        {
            return a.login != b.login || a.password != b.password;
        }
    }
}
