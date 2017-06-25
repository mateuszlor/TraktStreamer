using System.Diagnostics;
using AopAlliance.Intercept;
using NLog;

namespace TraktStreamer.AOP
{
    public class ServiceExecutionInterceptor : IMethodInterceptor
    {

        private Logger _logger = LogManager.GetCurrentClassLogger();

        public object Invoke(IMethodInvocation invocation)
        {
            var sw = Stopwatch.StartNew();
            var retValue = invocation.Proceed();
            sw.Stop();

            _logger.Trace("### Execution of {0}.{1}() took {2}ms ({3})", invocation.Method.DeclaringType?.Assembly.GetName().Name, invocation.Method.Name , sw.ElapsedMilliseconds, sw.Elapsed);

            return retValue;
        }
    }
}
