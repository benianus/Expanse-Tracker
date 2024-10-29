using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseTrackerDataLayer
{
    public static class DataSettings
    {
        public static readonly string ConnectionString = "Server = .; Database = ExpanseTracker; User id = sa; Password = 123456; " +
                                                         "Encrypt=False; TrustServerCertificate=True; Connection Timeout=30;";
    }
}
