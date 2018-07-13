using System.Collections.Generic;

namespace TestCoreApp.Entities.ServiceTypes
{
    /// <inheritdoc cref="Result" />
    /// <summary>
    /// Generic result class with result object
    /// </summary>
    /// <typeparam name="T">type of result object</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="isSuccess">
        /// The is success.
        /// </param>
        public Result(bool isSuccess)
            : base(isSuccess)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="resultObject">
        /// The result object.
        /// </param>
        public Result(T resultObject)
        {
            this.ResultObject = resultObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="errors">
        /// The errors.
        /// </param>
        public Result(ICollection<Error> errors)
            : base(errors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="error">
        /// The error.
        /// </param>
        public Result(string error)
            : base(error)
        {
        }

        /// <summary>
        /// Gets or sets the object responsible for the return value
        /// </summary>
        public T ResultObject { get; set; }
    }
}
