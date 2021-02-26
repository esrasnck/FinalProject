using System;
using System.Collections.Generic;
using System.Text;

namespace Business.CCS
{
    public class FileLogger:ILogger
    {
        // ILogger'dan miras vermemizin nedeni : ILogger türü. interface yaptık. çünkü dosya, uzak sunucu, mail atabilirim vs. şeklinde loglama yapabilirim. kısacası birbirinin alternatifi olabilir. bu yüzden interfaceden tutuyorum. Bunun yerine enum kullanmak daha kötü. Enumları tikkatlı kullan kızım esra :)

        // FileLogger: Loglar bir dosyaya alınıyor demek.
        public void Log()
        {

            Console.WriteLine("Dosyaya Loglandı");
        }
    }
}
