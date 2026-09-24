using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Sharing.Data
{
    public class Host
    {
        private string? arch;
        private string? hostname;

        public int TotalCount { get; set; }
        public string? IP { get; set; }
        public string? OS { get; set; }
        public string? ClusterName { get; set; }
        public string? StorageClusterName { get; set; }
        public string? Hostname
        {
            get { return hostname; }
            set
            {
                hostname = value?.Split('.')[0].Trim().ToString();
            }
        }
        public string? Arch { get => arch; set => arch = value?.ToLower(); }

        private string _status;
        public string? Status { 
            get => _status; 
            set
            {
                switch (value)
                {
                    case "ONLINE":
                        _status = "Онлайн";
                        HexForeColor = "#1B5E20"; break;
                    default:
                        _status = string.Empty;
                        HexForeColor = "#FFFFFF";
                        break;
                }
            } 
        }

        private string _statusSDK;
        public string? SDKStatus { 
            get => _statusSDK;
            set
            {
                switch (value)
                {
                    case "AVAILABLE":
                        _statusSDK = "Доступно";
                        HexForeColorSDK = "#1B5E20"; break;
                    default:
                        _statusSDK = string.Empty;
                        HexForeColorSDK = "#FFFFFF";
                        break;
                }
            }
        }

        public string HexForeColorSDK { get; set; }
        public string HexForeColor { get; set; }
    }
}
