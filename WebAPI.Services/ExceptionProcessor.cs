using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace WebAPI.Services
{
    public class ExceptionProcessor : IExceptionProcessor
    {
        public string GetMethodNameThrowingException(Exception ex)
        {
            StackTrace s = new StackTrace(ex);
            Assembly thisasm = Assembly.GetExecutingAssembly();
            return s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
        }
    }
}
