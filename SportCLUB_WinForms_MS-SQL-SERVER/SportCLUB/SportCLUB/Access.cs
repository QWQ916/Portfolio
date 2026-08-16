using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportCLUB
{
    public interface Access
    {
        void Log_in(Players player);
        void LowerAccess(Access access);
    }
}
