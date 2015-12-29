using System;

namespace Monads
{
    public class Failure<T> : Try<T>
    {
        private readonly Exception exception;

        public Failure(Exception e)
        {
            exception = e;
        }

        public Failure(string message, params object[] p)
        {
            exception = new TryException(message, p);
        }

        public override bool IsSuccess
        {
            get { return false; }
        }

        public override bool IsFailure
        {
            get { return true; }
        }

        public override T Get()
        {
            throw exception;
        }

        public override T GetOrDefault()
        {
            return default(T);
        }


        public override Try<U> Map<U>(Func<T, U> mapper)
        {
            return new Failure<U>(exception);
        }

        public override Try<U> FlatMap<U>(Func<T, Try<U>> mapper)
        {
            return new Failure<U>(exception);
        }

        public override Try<T> Recover(Func<Exception, T> mapper)
        {
            try
            {
                return new Success<T>(mapper.Invoke(exception));
            }
            catch
            {
                return new Failure<T>(exception);
            }
        }

        public override Try<T> Recover<U>(Func<Exception, T> recover)
        {
            return (exception.GetType().IsAssignableFrom(typeof (U))) ? Recover(recover) : this;
        }

        public override Try<T> Filter(Predicate<T> predicate)
        {
            return this;
        }
 


    }
}
