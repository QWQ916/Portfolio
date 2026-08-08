using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Globalization;

namespace Monitoring.Sharing.Data
{
    public class VirtualMachine
    {
        public int TotalCount { get; set; }

        public string? Name { get; set; }
        public string? IP { get; set; }
        public int CoresCount { get; set; }
        public string? GuestOsVersion { get; set; }
        public string? GuestOs { get; set; }
        public string? Location { get; set; }
        public int RamSize { get; set; }
        public string RamSizeText { get => RamSize.ToString("N0", CultureInfo.GetCultureInfo("ru-RU")) + " МБ"; set => RamSize.ToString("N0", CultureInfo.GetCultureInfo("ru-RU")); }

        private string _status;
        public string? Status 
        { 
            get => _status;
            set
            {
                switch (value)
                {
                    case "RUNNING":
                        _status = "Запущено";
                        HexForeColor = "#1B5E20";
                         break;
                    case "STOPPED":
                        _status = "Остановлено";
                        HexForeColor = "#B71C1C";
                         break;
                    default:
                        _status = string.Empty;
                        HexForeColor = "#FFFFFF";
                        break;
                }
            }
        }

        public string HexForeColor { get; set; }

        // Маппинг вручную снизу

        public string? NodeName { get; set; }
        public int NodeID { get; set; }
    }
}
