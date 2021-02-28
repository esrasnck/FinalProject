using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims); // token üretecek mekanizma

        // user, login olduğunda, eğer doğru ise, ilgili kullanıcı için veritabanına gidip, o kullanıcının rollerini (claimlerini) bulacak. orada bir tane jason web token üretecek( içerisinde bu bilgileri bulunduran.) sonra onları buraya verecek.
    }
}
