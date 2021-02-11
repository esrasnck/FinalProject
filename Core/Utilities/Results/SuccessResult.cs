using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
   public class SuccessResult:Result
    {
        public SuccessResult(string message):base(true,message) // başarılı anlamında true gönderdi
        {

        }
        public SuccessResult():base(true) // base'in tek parametreli olanı çalıştır.
        {
            // true default olarak verildi.
        }
    }
}
