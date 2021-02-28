﻿using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using Core.Entities.Concrete;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        List<OperationClaim> GetClaims(User user); // kullanıcının sahip olduğu claimleri çekmek istiyorum. bu benim için bir join operasyonu olacak
    }
}
