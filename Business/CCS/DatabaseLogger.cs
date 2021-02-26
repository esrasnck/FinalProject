using System;

namespace Business.CCS
{
    public class DatabaseLogger : ILogger
   {
       // DatabaseLogger: Loglar bir veritabanına alınıyor demek.
        public void Log()
       {
           Console.WriteLine("Veritabanına Loglandı");
       }
   }
}
