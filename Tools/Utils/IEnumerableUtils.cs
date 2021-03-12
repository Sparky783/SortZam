using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Utils
{
    public static class IEnumerableUtils
    {
        public static void AddRange<T>(this IEnumerable<T> listEnumerable, IEnumerable<T> listObject)
        {
            if (listEnumerable == null)
                return;
            ((List<T>)listEnumerable).AddRange(listObject);
        }

        /// <summary>
        /// Ajouter un élément à un IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEnumerable"></param>
        /// <param name="objet"></param>
        public static void Add<T>(this IEnumerable<T> listEnumerable, T objet)
        {
            if (listEnumerable == null)
                return;
            ((List<T>)listEnumerable).Add(objet);
        }

        /// <summary>
        /// Randomize/Shuffle a list of T objects
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="list">List of Objects T to randomize</param>
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        /// <summary>
        /// Récuperer une chaine de caractère à partir d'une liste de T (utile requete SQL)
        /// </summary>
        /// <param name="list">Liste de T</param>
        /// <param name="separator">Séparateur</param>
        /// <param name="withQuoteForSql">Si on doit ajouter des quotes sur les champs (utile SQL)</param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> list, string separator, bool withQuoteForSql)
        {
            return list.ToArray().ToString(separator, withQuoteForSql);
        }

        /// <summary>
        /// Récuperer une chaine de caractère à partir d'une liste de T (utile requete SQL)
        /// </summary>
        /// <param name="list">Liste de T</param>
        /// <param name="separator">Séparateur</param>
        /// <param name="withQuoteForSql">Si on doit ajouter des quotes sur les champs (utile SQL)</param>
        /// <returns></returns>
        public static string ToString<T>(this Array list, string separator, bool withQuoteForSql)
        {
            return list.OfType<T>().ToArray().ToString(separator, withQuoteForSql);
        }

        /// <summary>
        /// Récuperer une chaine de caractère à partir d'une tableau de T (utile requete SQL)
        /// </summary>
        /// <param name="list">Liste de T</param>
        /// <param name="separator">Séparateur</param>
        /// <param name="withQuoteForSql">Si on doit ajouter des quotes sur les champs (utile SQL)</param>
        /// <returns></returns>
        public static string ToString<T>(this T[] list, string separator, bool withQuoteForSql)
        {
            if (list == null || !list.Any())
                return null;

            var valueQuote = (withQuoteForSql) ? "'" : "";

            var sb = new StringBuilder();
            foreach (var t in list)
            {
                var uneValeur = t.ToString();
                if (withQuoteForSql)
                    uneValeur = uneValeur.Replace("'", "''");
                sb.AppendFormat("{0}{1}{0}{2}", valueQuote, uneValeur, separator);
            }
            return sb.ToString().TrimEnd(separator.ToCharArray());
        }

        /// <summary>
        /// Distinct sur une liste par un keySelector
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        /// <summary>
        /// Cloner une list dans une autre lsite
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToClone"></param>
        /// <returns></returns>
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> listToClone)
        {
            return new List<T>(listToClone);
        }
    }
}
