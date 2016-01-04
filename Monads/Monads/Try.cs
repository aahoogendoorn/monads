using System;

namespace Monads
{
    public abstract class Try<T>
    {
        /// <summary>
        /// Returns whether a <see cref="Try{T}"/> is a <see cref="Success{T}"/> (true) or a <see cref="Failure{T}"/>  (false)
        /// </summary>
        public abstract bool IsSuccess { get; }

        /// <summary>
        /// Returns whether a <see cref="Try{T}"/>is a <see cref="Failure{T}"/> (true) or a <see cref="Success{T}"/> (false)
        /// </summary>
        public abstract bool IsFailure { get; }

        /// <summary>
        /// Gets the value of the <see cref="Try{T}"/> if it is a <see cref="Success{T}"/>, and throws an exception if it is a <see cref="Failure{T}"/>
        /// </summary>
        /// <returns>Value if it is a <see cref="Success{T}"/> or the exception if it is a <see cref="Failure{T}"/></returns>
        public abstract T Get();


        /// <summary>
        /// Gets the value of the <see cref="Try{T}"/> if it is a <see cref="Success{T}"/>, and the dafult value of T if it is a <see cref="Failure{T}"/>
        /// </summary>
        /// <returns>Value if it is a <see cref="Success{T}"/> or the default of T if it is a <see cref="Failure{T}"/></returns>
        public abstract T GetOrDefault();

        /// <summary>
        /// Gets the value if it is a <see cref="Success{T}"/> or the <paramref name="other"/> otherwise
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Value or <paramref name="other"/> if this is a <see cref="Failure{T}"/></returns>
        public abstract T GetOrElse(T other);

        /// <summary>
        /// Invokes a function <paramref name="mapper"/> on type <typeparamref name="U"/>
        /// </summary>
        /// <typeparam name="U">Any type</typeparam>
        /// <param name="mapper">Function on type U</param>
        /// <returns>A <see cref="Success{T}"/> if the function does not throw an exception, a <see cref="Failure{T}"/> if it does, holding the exception</returns>
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

        /// <summary>
        /// Maps an instance of type T to an instance of type U by invoking function mapper  
        /// </summary>
        /// <typeparam name="U">Any type that is the result of invoking the function</typeparam>
        /// <param name="mapper">Function on type T, which is the type of the current value</param>
        /// <returns>A <see cref="Success{T}"/> if the function does not throw an exception, A <see cref="Failure{T}"/> if it does, holding the exception</returns>
        public abstract Try<U> Map<U>(Func<T, U> mapper);

        /// <summary>
        /// Maps an instance of type T to an instance of type <see cref="Try{T}"/> by invoking the function mapper 
        /// </summary>
        /// <typeparam name="U">Any type that is the result of invoking the function</typeparam>
        /// <param name="mapper">Function on type T, which is the type of the current value</param>
        /// <returns>A <see cref="Success{T}"/> if the function does not throw an exception, A <see cref="Failure{T}"/> if it does, holding the exception</returns>
        public abstract Try<U> FlatMap<U>(Func<T, Try<U>> mapper);

        /// <summary>
        /// Recover from a Failure holding any exception to a <see cref="Try{T}"/>
        /// </summary>
        /// <param name="recover">Function returning an instance of type T</param>
        /// <returns><see cref="Success{T}"/> if invoking function does not throw an exception, a <see cref="Failure{T}"/> otherwise</returns>
        public abstract Try<T> Recover(Func<Exception, T> recover);

        /// <summary>
        /// Recover from a Failure holding an exception of type U to a <see cref="Try{T}"/>
        /// </summary>
        /// <param name="recover">Function returning an instance of type T</param>
        /// <returns><see cref="Success{T}"/> if invoking function does not throw an exception, a <see cref="Failure{T}"/> otherwise</returns>
        public abstract Try<T> Recover<U>(Func<Exception, T> recover) where U : Exception;

        /// <summary>
        /// Filters using a predicate
        /// </summary>
        /// <param name="predicate">Predicate to test on T</param>
        /// <returns><see cref="Success{T}"/> if predicate returns true, <see cref="Failure{T}"/> if it returns false</returns>
        public abstract Try<T> Filter(Predicate<T> predicate);     
        
    }
}
