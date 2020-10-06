using System;

namespace WebAPI.Services
{
    public interface IExceptionProcessor
    {
        string GetMethodNameThrowingException(Exception ex);
    }
}