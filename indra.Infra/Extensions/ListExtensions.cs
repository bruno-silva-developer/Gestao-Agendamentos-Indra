using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace indra.Infra.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        ///     Verifica se a lista contém pelo menos um elemento e não é nula
        /// </summary>
        /// <typeparam name="T">Objeto da lista</typeparam>
        /// <param name="lista">Lista a ser verificada</param>
        /// <returns>Verdadeiro caso a lista contenha pelo menos um elemento, caso contrário, falso.</returns>
        public static bool ContemElementos<T>(this IEnumerable<T> lista)
        {
            return lista != null && lista.Any();
        }
    }
}
