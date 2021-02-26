using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        // bana iş kurallarını gönder diyorum.
        public static IResult Run(params IResult[] logics) // iş kurallarını gönder. Run içine istediğimiz kadar IResult verebiliyoruz.CheckIdProductExist, CheckCountOfCategories vs. istediğiniz kadar IResult döndüren iş kuralı gönderebilirsiniz.
        {
            foreach (IResult logic in logics)
            {
                if (!logic.Success)
                {
                    return logic; // başarısız ise error result döndürcek
                }
            }
            return null; // başarısız ise birşey döndürmeyecek
        }
        #region açıklamalar
        // parametre ile gönderdiğiniz iş kurallarını başarısız ise, hatalıysa error göndercek. logic'in success durumu başarısız ise, return error result döndürcek. başarılı ise birşey döndürmesine gerek yok. 

        // mevcut hata varsa, direkt o hatayı döndürür. Liste de isteyebilirdik. o da olurdu. dizi yerine gibi.
        #endregion

        // yada;

        #region şeklinde de yapılabir üsteki
        //public static List<IResult> Run(params IResult[] logics)
        //{
        //    List<IResult> errorResult = new List<IResult>();
        //    foreach (IResult logic in logics)
        //    {
        //        if (!logic.Success)
        //        {
        //            errorResult.Add(logic);
        //        }
        //    }
        //    return errorResult;
        //}
        #endregion
    }
}
