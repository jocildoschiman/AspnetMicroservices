using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        /// <summary>
        /// Construtor que mostra os erros
        /// </summary>
        public ValidationException() : base ("One or more validation failures have occured.")
        {
            Errors = new Dictionary<string, string[]>();
        }
        /// <summary>
        /// Construtor que retorna a lista de erros:
        /// </summary>
        /// <param name="failures"></param>
        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
        /// <summary>
        /// Dicionário de erros:
        /// </summary>
        public IDictionary<string, string[]> Errors { get; }
    }
}
