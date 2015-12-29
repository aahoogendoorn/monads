using System;

namespace Monads
{
    public class Success<T> : Try<T>
    {
        private readonly T value;

        public override T Get()
        {
            return value;
        }

        public override T GetOrDefault()
        {
            return value;
        }

        public Success(T value)
        {
            this.value = value;
        }

        public override bool IsSuccess
        {
            get { return true; }
        }

        public override bool IsFailure
        {
            get { return false; }
        }

        public override Try<U> Map<U>(Func<T, U> mapper)
        {
            return Try<U>.Invoke(() => mapper.Invoke(value));
        }

        public override Try<U> FlatMap<U>(Func<T, Try<U>> mapper)
        {
            return Try<U>.Invoke(() => mapper.Invoke(value).Get());
        }

        public override Try<T> Recover(Func<Exception, T> mapper)
        {
            return this;
        }
        public override Try<T> Recover<U>(Func<Exception, T> mapper)
        {
            return this;
        }

        public override Try<T> Filter(Predicate<T> predicate)
        {
            try
            {
                var result = predicate.Invoke(value);

                if (result)
                {
                    return this;
                }
                
                return new Failure<T>("There are no results from applying the predicate");
            }
            catch
            {
                return new Failure<T>("There are no results from applying the predicate");
            }
        }
    }
}
