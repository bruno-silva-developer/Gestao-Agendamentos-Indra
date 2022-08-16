using System;
using System.Collections.Generic;
using System.Text;

namespace indra.Infra.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Verifica se o texto digitado é nulo, vazio ou tem espaços em branco
        /// </summary>
        /// <param name="texto">Texto a ser verificado</param>
        /// <returns>Retorna verdadeiro caso esteja nulo, vazio ou com espaço em branco e falso se não tiver erro</returns>
        public static bool IsNullEmptyOrWhitespace(this string texto)
        {
            return string.IsNullOrEmpty(texto) || string.IsNullOrWhiteSpace(texto);
        }
    }
}
