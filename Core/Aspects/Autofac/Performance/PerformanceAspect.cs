using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Performance
{
    public class PerformanceAspect:MethodInterception
    {
        private int _interval;
        Stopwatch _stopWatch;

        public PerformanceAspect(int interval)
        {
            _stopWatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _stopWatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopWatch.Elapsed.TotalSeconds> _interval)
            {
                Debug.WriteLine($"Performance :  {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}----> {_stopWatch.Elapsed.TotalSeconds}"); // => mail attırabiliriz mesela
            }
            _stopWatch.Reset();
        }
    }
}

// using System.Diagnostics; Stopwatcj