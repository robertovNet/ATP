using FluentValidation.Results;
using System.Collections.Generic;

namespace ATP.Common.Entities
{
    public class IntegrationServiceResponse<T> : IIntegrationServiceBaseResponse
    {
        #region "CONSTRUCTOR"

        public IntegrationServiceResponse()
        {
            ModelErrors = new Dictionary<string, IList<string>>();
        }

        #endregion "CONSTRUCTOR"

        #region "METHODS"

        /// <summary>
        /// MÉTODO QUE AGREGA UN ERROR AL DICCIONARIO DE ERRORES
        /// </summary>
        /// <param name="key">KEY A LA QUE SE AGREGARÁ EL ERROR</param>
        /// <param name="errorText">TEXTO DEL ERROR</param>
        /// <returns></returns>
        public IntegrationServiceResponse<T> AddError(string key, string errorText)
        {
            if (ModelErrors.ContainsKey(key))
            {
                var error = ModelErrors[key];
                error.Add(errorText);
            }
            else
                ModelErrors.Add(key, new List<string> { errorText });

            this.IsValid = false;

            return this;
        }

        /// <summary>
        /// Método que agrega los errores de un ModelValidator
        /// </summary>
        /// <param name="errors">ModelValidator con los errores</param>
        /// <returns></returns>
        public IntegrationServiceResponse<T> AddError(IEnumerable<ValidationFailure> errors)
        {
            foreach (var validationFailure in errors)
                AddError(validationFailure.PropertyName, validationFailure.ErrorMessage);

            return this;
        }

        #endregion "METHODS"

        #region "PROPERTIES"

        public T Data { get; set; }

        public bool IsValid { get; set; }

        public Dictionary<string, IList<string>> ModelErrors { get; set; }

        #endregion "PROPERTIES"
    }

    public interface IIntegrationServiceBaseResponse
    {
        bool IsValid { get; set; }

        Dictionary<string, IList<string>> ModelErrors { get; set; }
    }
}
