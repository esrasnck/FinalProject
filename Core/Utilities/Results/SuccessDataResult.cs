using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class SuccessDataResult<T>:DataResult<T>
    {
        public SuccessDataResult(T data, string message):base(data,true,message)
        {
            // data - mesaj - true => defalut olaraktan
        }
        public SuccessDataResult(T data):base(data,true)
        {
            // mesajsız hali data - işlem sonucu
        }
        public SuccessDataResult(string message):base(default,true,message)
        {
            // data default hali mesaj göndercek sadece. çok az kullancaz.
        }
        public SuccessDataResult():base(default,true)
        {
            // msj verme. true dön gibi versiyon bunlar
        }
    }
}
