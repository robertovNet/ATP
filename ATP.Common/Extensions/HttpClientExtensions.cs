using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using ATP.Common.Entities;
using ATP.Common.Exceptions;

namespace ATP.Common.Extensions
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Método que convierte un pbject en parámetros para ser utilizados en el QueryString de la petición a Realizar
        /// </summary>
        /// <param name="parameters">Parámetros con los que se invocará al Servicio</param>
        /// <returns>Retorna un string con los parámetros</returns>
        private static string GetQueryString(object parameters)
        {
            var properties = parameters.GetType().GetProperties().Enum()
                .Where(p => p.GetValue(parameters, null) != null)
                .Select(p => p.Name.ToCamelCase() + "=" + HttpUtility.UrlEncode(p.GetValue(parameters, null).ToString()));

            return string.Join("&", properties);
        }

        /// <summary>
        /// Agrega los headers de la configuración a la petición a realizar
        /// </summary>
        /// <param name="client">HttpClient</param>
        /// <param name="endPointConfiguration">Configuración del EndPoint con los settings a agregar como header</param>
        private static void SetHttpClientSettings(HttpClient client, IEnumerable<Policy> policies)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var policySetting in policies.Enum())
                client.DefaultRequestHeaders.Add(policySetting.Name, policySetting.Value);
        }

        /// <summary>
        /// Método que realiza un GET al Servicio especificado
        /// </summary>
        /// <param name="client">HttpClient con la instancia</param>
        /// <param name="serviceName">Nombre del Servicio a utilizar</param>
        /// <param name="value">Parámetros a utilizar para invocar el Servicio</param>
        /// <returns></returns>
        public static async Task<TReturn> GetAsync<TReturn, TInput>(this HttpClient client, string serviceName, TInput value)
        {
            //using (var trace = new TracerDb())
            //{
            //    trace.TraceInformation("Invocando Método GetAsync(), Servicio: [{0}], Parametros:[{1}]", serviceName, value);

            var success = false;

            //var endPointSection = EndPointConfigurationManager.GetPolicyConfiguration(serviceName);
            var endPointSection = new EndPointConfiguration { Address = "" };

            //var currentRetry = endPointSection.Retries;

            var queryParameters = value is int ? value.ToString() : "?" + GetQueryString(value);

            //trace.TraceInformation("Invocando Método GetAsync(), Servicio: [{0}], Parametros:[{1}]", serviceName, queryparameters);

            //do
            //{
            try
            {
                // Checkea el intento actual de la llamada, si es distinto al de la configuración aplica el Task.Delay de acuerdo a la configuración
                //if (currentRetry != endPointSection.Retries)
                //    await Task.Delay(endPointSection.Wait);

                // Setea los headers para la petición
                SetHttpClientSettings(client, endPointSection.Policies);

                var response = await client.GetAsync(endPointSection.Address + queryParameters).ConfigureAwait(false);

                var jsonString = await response.Content.ReadAsStringAsync();

                success = response.IsSuccessStatusCode;

                //trace.TraceInformation("jsonString: [{0}], IsSuccessStatusCode: [{1}]", jsonString, response.IsSuccessStatusCode,
                //    new TraceMainData(endPointSection.Address + queryparameters, 0, serviceName));

                // Si el Response es Success, Deserializa el json string en un objeto Generic T
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<TReturn>();

                //trace.TraceError("Ha ocurrido un error con el resultado de la llamada al Servicio: [{0}], ReasonPhrase:[{1}], StatusCode: [{2}], Reintento:[{3}]",
                //    endPointSection.Address, response.ReasonPhrase, response.StatusCode, currentRetry);

                //currentRetry--;
            }
            catch (Exception ex)
            {
                //currentRetry--;

                //trace.TraceError("Se ha generado una excepción: [{0}]. Detalle de excepción: [{1}]. Se setea el Reintento: [{2}]", ex.Message, ex.GetInnerExceptionsDetail(), currentRetry,
                //     new TraceMainData(endPointSection.Address + queryparameters, 0, serviceName));
            }
            //} while (!success && currentRetry > 0);

            throw new ServiceResponseException(Messages.ServiceInternalError, endPointSection.Address);
            //}
        }

        /// <summary>
        /// Método que realiza un GET al Servicio especificado sin parámetros
        /// </summary>
        /// <param name="client">HttpClient con la instancia</param>
        /// <param name="serviceName">Nombre del Servicio a utilizar</param>
        /// <returns></returns>
        public static async Task<TReturn> GetAsync<TReturn>(this HttpClient client, string serviceName)
        {
            //using (var trace = new TracerDb())
            //{
            //    trace.TraceInformation("Invocando Método GetAsync(), Servicio: [{0}]", serviceName);

            var success = false;

            //var endPointSection = EndPointConfigurationManager.GetPolicyConfiguration(serviceName);
            var endPointSection = new EndPointConfiguration { Address = "" };

            //var currentRetry = endPointSection.Retries;

            //do
            //{
            try
            {
                // Checkea el intento actual de la llamada, si es distinto al de la configuración aplica el Task.Delay de acuerdo a la configuración
                //if (currentRetry != endPointSection.Retries)
                //    await Task.Delay(endPointSection.Wait);

                // Setea los headers para la petición
                SetHttpClientSettings(client, endPointSection.Policies);

                //trace.TraceInformation("Se generará la Petición (GetAsync): [{0}]", endPointSection.Address);

                var response = await client.GetAsync(endPointSection.Address).ConfigureAwait(false);

                var jsonString = await response.Content.ReadAsStringAsync();

                success = response.IsSuccessStatusCode;

                //trace.TraceInformation("jsonString: [{0}], IsSuccessStatusCode: [{1}]", jsonString, response.IsSuccessStatusCode,
                //    new TraceMainData(endPointSection.Address, 0, serviceName));

                // Si el Response es Success, Deserializa el json string en un objeto Generic T
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<TReturn>();

                //trace.TraceError("Ha ocurrido un error con el resultado de la llamada al Servicio: [{0}], ReasonPhrase:[{1}], StatusCode: [{2}], Reintento:[{3}]",
                //    endPointSection.Address, response.ReasonPhrase, response.StatusCode, currentRetry);

                //currentRetry--;
            }
            catch (Exception ex)
            {
                //currentRetry--;

                //trace.TraceError("Se ha generado una excepción: [{0}]. Detalle de excepción: [{1}]. Se setea el Reintento: [{2}]", ex.Message, ex.GetInnerExceptionsDetail(), currentRetry,
                //    new TraceMainData(endPointSection.Address, 0, serviceName));
            }
            //} while (!success && currentRetry > 0);

            throw new ServiceResponseException(Messages.ServiceInternalError, endPointSection.Address);
            //}
        }

        public static async Task<TReturn> GetAsync<TReturn>(this HttpClient client, EndPointConfiguration endPointConfiguration)
        {
            var success = false;

            try
            {
                // Setea los headers para la petición
                SetHttpClientSettings(client, endPointConfiguration.Policies);
                var response = await client.GetAsync(endPointConfiguration.Address).ConfigureAwait(false);

                var jsonString = await response.Content.ReadAsStringAsync();

                success = response.IsSuccessStatusCode;
                // Si el Response es Success, Deserializa el json string en un objeto Generic T
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<TReturn>();
            }
            catch (Exception ex)
            {
            }

            throw new ServiceResponseException(Messages.ServiceInternalError, endPointConfiguration.Address);
        }

        public static async Task<TReturn> GetAsync<TReturn>(this HttpClient client, EndPointConfiguration endPointConfiguration, object value)
        {
            var queryParameters = value is int ? value.ToString() : "?" + GetQueryString(value);

            endPointConfiguration.Address += queryParameters;

            return await GetAsync<TReturn>(client, endPointConfiguration);
        }

        /// <summary>
        /// Método que realiza un POST al Servicio especificado
        /// </summary>
        ///<param name="client">HttpClient con la instancia</param>
        /// <param name="serviceName">Nombre del Servicio a utilizar</param>
        /// <param name="value">Parámetros a utilizar para invocar el Servicio</param>
        /// <returns></returns>
        public static async Task<TReturn> PostAsync<TReturn, TInput>(this HttpClient client, string serviceName, TInput value, Guid? activityId = null)
             where TReturn : IIntegrationServiceBaseResponse, new()
        {
            //using (var trace = new TracerDb())
            //{
            var success = false;

            //Obtiene la configuración del EndPoint del Servicio de acuerdo al Nombre de la Integración
            //var endPointSection = EndPointConfigurationManager.GetPolicyConfiguration(serviceName);
            var endPointSection = new EndPointConfiguration { Address = "" };

            //var currentRetry = endPointSection.Retries;

            //do
            //{
            try
            {
                // Checkea el intento actual de la llamada, si es distinto al de la configuración aplica el Task.Delay de acuerdo a la configuración
                //if (currentRetry != endPointSection.Retries)
                //    await Task.Delay(endPointSection.Wait);

                // Setea los headers para la petición
                SetHttpClientSettings(client, endPointSection.Policies);
                client.Timeout = new TimeSpan(0, 0, 10, 0);

                var response = await client.PostAsJsonAsync(endPointSection.Address, value).ConfigureAwait(false);

                success = response.IsSuccessStatusCode;

                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                client.DefaultRequestHeaders.Add("ResultString", responseString);

                //if (endPointSection.Trace)
                //    trace.TraceInformation("jsonString: [{0}], IsSuccessStatusCode: [{1}]", responseString, response.IsSuccessStatusCode,
                //            new TraceMainData(value, 0, serviceName, activityId: activityId));

                if (response.IsSuccessStatusCode)
                {
                    var responseService = await response.Content.ReadAsAsync<TReturn>().ConfigureAwait(false);
                    return responseService;
                }

                //trace.TraceError(
                //"Ha ocurrido un error con el resultado de la llamada al Servicio: [{0}], ReasonPhrase:[{1}], StatusCode: [{2}], Reintento:[{3}]",
                //endPointSection.Address, response.ReasonPhrase, response.StatusCode, currentRetry, new TraceMainData(value, 0, serviceName, activityId: activityId));

                //currentRetry--;
            }
            catch (Exception ex)
            {
                var error = string.Format("Message: {0}, StackTrace: {1}", ex.Message, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    error = string.Format("Message: {0}, StackTrace: {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                    if (ex.InnerException.InnerException != null)
                        error = string.Format("Message: {0}, StackTrace: {1}", ex.InnerException.InnerException.Message, ex.InnerException.InnerException.StackTrace);
                }

                //trace.TraceError("Se ha generado una excepción: [{0}], Se setea el Reintento: [{1}], Link: [{2}]",
                //    error, currentRetry, endPointSection.Address, new TraceMainData(value, 0, serviceName, activityId: activityId));

                //currentRetry--;
            }
            //} while (!success && currentRetry > 0);
            //}

            return new TReturn { IsValid = false };
        }
    }
}
