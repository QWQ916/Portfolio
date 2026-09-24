using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Sharing.Interfaces
{
    public interface IAppConfig
    {
        string Get(string key);
        T Get<T>(string key);
        string GetOrDefault(string key, string defaultValue = null);
    }
}
