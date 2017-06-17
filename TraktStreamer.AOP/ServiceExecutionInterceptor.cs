using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AopAlliance.Intercept;

namespace TraktStreamer.AOP
{
    public class ServiceExecutionInterceptor : IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            var sw = Stopwatch.StartNew();
            var retValue = invocation.Proceed();
            sw.Stop();

            Debug.WriteLine("### Execution of {0}.{1}() took {2}ms ({3})", invocation.Method.DeclaringType?.Assembly.GetName().Name, invocation.Method.Name , sw.ElapsedMilliseconds, sw.Elapsed);

            return retValue;
        }
    }
}
