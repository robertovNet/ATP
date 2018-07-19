using System;
using System.Collections.Generic;
using System.Linq;

namespace ATP.Common.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Divide un conjunto de elementos en subconjuntos del tamaño especificado
        /// </summary>
        /// <typeparam name="T">Tipo de los elementos que componen el conjunto</typeparam>
        /// <param name="inputSet">Conjunto de entrada</param>
        /// <param name="clusterSize">Cantidad de elementos que deben tener los subconjuntos a generar</param>
        /// <returns>Subconjuntos que contienen los elementos del conjunto de entrada</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> inputSet, int clusterSize)
        {
            if (inputSet == null || !inputSet.Any())
                throw new ArgumentException("El conjunto ingresado para segmentar debe ser no nulo y tener elementos", "inputSet");

            if (clusterSize < 1)
                throw new ArgumentException("El tamaño de los subconjuntos resultantes debe ser mayor que cero", "clusterSize");

            var clusters = inputSet.Select((value, index) => new { Index = index, Value = value })
                   .GroupBy(x => x.Index / clusterSize)
                   .Select(g => g.Select(x => x.Value));

            return clusters;
        }

        /// <summary>
        /// Elimina elementos duplicados de un conjunto a partir de un criterio de unicidad
        /// </summary>
        /// <typeparam name="TSource">Tipo de los elementos que componen el conjunto</typeparam>
        /// <typeparam name="TKey">Tipo de la función de diferenciación de elementos</typeparam>
        /// <param name="inputSet">Conjunto de entrada</param>
        /// <param name="keySelector">Función de diferenciación de los elementos, expresa el criterio de unicidad para el conjunto</param>
        /// <returns>Un conjunto igual o inferior al de entrada, que contiene elementos que no se repiten de acuerdo al criterio de unicidad</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> inputSet, Func<TSource, TKey> keySelector)
        {
            if (inputSet == null)
                throw new ArgumentException("El conjunto ingresado debe ser no nulo.", "inputSet");

            if (keySelector == null)
                throw new ArgumentException("La expresión para diferenciar elementos no puede ser nula.", "keySelector");

            if (!inputSet.Any())
                return inputSet;

            var output = inputSet.GroupBy(keySelector).Select(g => g.First());

            return output;
        }

        /// <summary>
        /// Determina si una colección posee elementos.
        /// </summary>
        /// <typeparam name="T">Tipo de los elementos de la colección</typeparam>
        /// <param name="input">Colección a evaluar</param>
        /// <returns>Devuelve <c>true</c> si la colección especificada posee elementos, <c>false</c> en caso contrario.</returns>
        public static bool HasElements<T>(this IEnumerable<T> input)
        {
            return (input != null && input.Any());
        }

        /// <summary>
        /// Obtiene una colección vacía no nula en caso de ser originalmente nula, en otros casos retorna la colección original
        /// </summary>
        /// <typeparam name="T">Tipo de los elementos de la colección</typeparam>
        /// <param name="input">Colección a evaluar</param>
        /// <returns>Si la colección no es nula, la devuelve sin modificaciones, en caso contrario devuelve una colección vacía.</returns>
        public static IEnumerable<T> Enum<T>(this IEnumerable<T> input)
        {
            return input ?? Enumerable.Empty<T>();
        }
    }
}
