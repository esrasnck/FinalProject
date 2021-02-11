using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
   public class ErrorDataResult<T> :DataResult<T>
    {
        public ErrorDataResult(T data, string message) : base(data, false, message)
        {
            // data - mesaj - true => defalut olaraktan
        }
        public ErrorDataResult(T data) : base(data, false)
        {
            // mesajsız hali data - işlem sonucu
        }
        public ErrorDataResult(string message) : base(default, false, message)
        {
            // data default hali mesaj göndercek sadece. çok az kullancaz.
        }
        public ErrorDataResult() : base(default, false)
        {
            // msj verme. true dön gibi versiyon bunlar
        }
    }
}
