using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RecruProj.Validators.GlobalExceptionHandler
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {

        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {

        }

    }
}