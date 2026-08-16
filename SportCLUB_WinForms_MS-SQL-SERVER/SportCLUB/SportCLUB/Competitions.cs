using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportCLUB
{
    public class Competitions
    {
        int id; string name; string country; string city; DateTime datestart; DateTime dateend; int minmasteryid; int maxage; int playerid; int score;
        public Competitions(int id, string name, string country, string city, DateTime datestart, DateTime dateend, int minmasteryid, int maxage, int playerid, int score)
        {
            this.id = id; this.city = city; this.name = name; this.country = country; this.dateend = dateend; this.datestart = datestart; this.minmasteryid = minmasteryid; this.maxage = maxage; this.score = score; this.playerid = playerid;
        }
        public int ID { get => id; }
        public int MinMasteryID { get => minmasteryid; }
        public int MaxAge { get => maxage; }
        public int PlayerID { get => playerid; }
        public int Score { get => score; }
        public string Name { get => name; }
        public string Country { get => country; }
        public string City { get => city; }
        public DateTime DateStart { get => datestart; }
        public DateTime DateEnd { get => dateend; }
    }
}
