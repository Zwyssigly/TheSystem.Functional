using System;

namespace Zwyssigly.Functional
{
    public class UnwrapException : Exception
    {
        public UnwrapException(string message) : base(message)
        {
        }
    }
}