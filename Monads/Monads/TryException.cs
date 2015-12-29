using System;

namespace Monads
{
    public class TryException : Exception
    {
        public TryException(string message, params Object[] p) : base(string.Format(message, p))
        {
        }
    }
}
