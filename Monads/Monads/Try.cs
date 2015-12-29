using System;

namespace Monads
{
    public abstract class Try<T>
    {
        protected Try() {}

        public abstract bool IsSuccess { get; }

        public abstract bool IsFailure { get; }

        public abstract T Get();

        public abstract T GetOrDefault();

        public static Try<U> Invoke<U>(Func<U> mapper)
        {
            try
            {
                return new Success<U>(mapper.Invoke());
            }
            catch (Exception e)
            {
                return new Failure<U>(e);
            }
        }

        public abstract Try<U> Map<U>(Func<T, U> mapper);

        public abstract Try<U> FlatMap<U>(Func<T, Try<U>> mapper);

        public abstract Try<T> Recover(Func<Exception, T> recover);

        public abstract Try<T> Recover<U>(Func<Exception, T> recover) where U : Exception;

        public abstract Try<T> Filter(Predicate<T> predicate);     
        
    }
}
