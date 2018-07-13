using System.Collections.Generic;

namespace TestCoreApp.Entities.ServiceTypes
{
    /// <summary>
    /// The result.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// Will create a new instance of Result class. IsSuccess property will have the value of the parameter. Errors collection will be initialized and not filled.
        /// </summary>
        /// <param name="isSuccess">IsSuccess value</param>
        public Result(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
            this.Errors = new List<Error>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// Will create a new instance of Result class. Errors property will have the value of the parameter. IsSuccess property will be set to false.
        /// </summary>
        /// <param name="errors">IsSuccess value</param>
        public Result(ICollection<Error> errors)
        {
            this.IsSuccess = false;
            this.Errors = errors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// Will create a new instance of Result class. Errors collection will be initialized and filled with value of parameter. IsSuccess property will be set to false.
        /// </summary>
        /// <param name="error"> error from source </param>
        public Result(string error)
        {
            this.IsSuccess = false;
            this.Errors = new List<Error> { new Error { Message = error } };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        protected Result()
        {
            this.IsSuccess = true;
            this.Errors = new List<Error>();
        }

        /// <summary>
        /// Gets a value indicating whether
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets or sets collection of Error values.
        /// </summary>
        public ICollection<Error> Errors { get; set; }

        /// <summary>
        /// The overloaded conversion operator (bool) returns the value of the IsSuccess property
        /// </summary>
        /// <param name="value">Result object for conversation</param>
        public static explicit operator bool(Result value)
        {
            return value.IsSuccess;
        }
    }
}
