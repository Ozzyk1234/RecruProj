using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RecruProj.Validators.GlobalExceptionHandler
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException )
        {

        }

    }
}   