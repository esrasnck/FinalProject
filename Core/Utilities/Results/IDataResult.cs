using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public interface IDataResult<T> :IResult
    { // hangi tipi döndüreceğini bana söyle diyoruz
        // msj işlem sonucu ve data döndersin 

        T Data { get; }
    }
}
