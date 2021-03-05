using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Core.Aspects.Transaction
{
   public class TransactionScopeAspect:MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            using(TransactionScope transactionScope = new TransactionScope())
            {

                try
                {
                    invocation.Proceed(); // metodu çalıştırmaya çalış başarılı oldun mu?
                    transactionScope.Complete(); //  o zaman transactionScope'u tamamla. yani işlemi kabul et ve çalıştıs
                }
                catch (Exception e)
                {
                    transactionScope.Dispose(); // başarılı olamadıysan, yapılan işlemleri geri al ve bir hata fırlat.
                    throw;
                }

            }
        }
    }
}
