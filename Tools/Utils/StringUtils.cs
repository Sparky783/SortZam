using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Tools.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Remplacer un caractère par sa position
        /// </summary>
        /// <param name="str">source</param>
        /// <param name="replaceBy"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string ReplaceByPosition(this string str, string replaceBy, int offset, int count)
        {
            return new StringInfo(str).ReplaceByPosition(replaceBy, offset, count).String;
        }
        public static StringInfo ReplaceByPosition(this StringInfo str, string replaceBy, int offset, int count)
        {
            return str.RemoveByTextElements(offset, count).InsertByTextElements(offset, replaceBy);
        }
        public static StringInfo RemoveByTextElements(this StringInfo str, int offset, int count)
        {
            return new StringInfo(string.Concat(
                str.SubstringByTextElements(0, offset),
                offset + count < str.LengthInTextElements ? str.SubstringByTextElements(offset + count, str.LengthInTextElements - count - offset) : ""
            ));
        }
        public static StringInfo InsertByTextElements(this StringInfo str, int offset, string insertStr)
        {
            if (string.IsNullOrEmpty(str?.String))
                return new StringInfo(insertStr);
            return new StringInfo(string.Concat(
                str.SubstringByTextElements(0, offset),
                insertStr,
                str.LengthInTextElements - offset > 0 ? str.SubstringByTextElements(offset, str.LengthInTextElements - offset) : ""
            ));
        }
        /// <summary>
        /// Retourne true si le string contient un caractère unicode (c'est à dire au délà de l'ASCII, comme les émojis et autre caractères spéciaux)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasUnicodeChar(this string input)
        {
            return input.Any(c => c.IsUnicode());
        }
        /// <summary>
        /// Retourne true si le caractère est un caractère unicode (c'est à dire au délà de l'ASCII, comme les émojis et autre caractères spéciaux)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUnicode(this char input)
        {
            return input > 255;
        }

        /// <summary>
        /// Effacer tous les accents d'une chaine de caractère
        /// </summary>
        /// <param name="libelle">Chaine caractère comprenant les accents</param>
        /// <returns>La chaine de caractères sans accent</returns>
        public static string DeleteAccent(this string libelle)
        {
            if (string.IsNullOrEmpty(libelle)) return libelle;
            char[] oldChar = { 'À', 'Á', 'Â', 'Ã', 'Ä', 'Å', 'à', 'á', 'â', 'ã', 'ä', 'å', 'Ò', 'Ó', 'Ô', 'Õ', 'Ö', 'Ø', 'ò', 'ó', 'ô', 'õ', 'ö', 'ø', 'È', 'É', 'Ê', 'Ë', 'è', 'é', 'ê', 'ë', 'Ì', 'Í', 'Î', 'Ï', 'ì', 'í', 'î', 'ï', 'Ù', 'Ú', 'Û', 'Ü', 'ù', 'ú', 'û', 'ü', 'ÿ', 'Ñ', 'ñ', 'Ç', 'ç', '°' };
            char[] newChar = { 'A', 'A', 'A', 'A', 'A', 'A', 'a', 'a', 'a', 'a', 'a', 'a', 'O', 'O', 'O', 'O', 'O', 'O', 'o', 'o', 'o', 'o', 'o', 'o', 'E', 'E', 'E', 'E', 'e', 'e', 'e', 'e', 'I', 'I', 'I', 'I', 'i', 'i', 'i', 'i', 'U', 'U', 'U', 'U', 'u', 'u', 'u', 'u', 'y', 'N', 'n', 'C', 'c', ' ' };
            for (var i = 0; i < oldChar.Length; i++)
                libelle = libelle.Replace(oldChar[i], newChar[i]);
            return libelle;
        }

        /// <summary>
        /// Mettre en majuscule uniquement la première lettre d'un mot
        /// </summary>
        /// <param name="input">Mot</param>
        /// <returns>Mot avec la première lettre en majuscule, le reste en minuscule</returns>
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return input.First().ToString().ToUpper() + input.Substring(1).ToLower();
        }

        /// <summary>
        /// Converti un chaine de caractère en string type URL
        /// </summary>
        /// <param name="chaine"></param>
        /// <returns></returns>
        public static string ToUrl(this string chaine)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyz1234567890-/";
            if (string.IsNullOrEmpty(chaine))
                return chaine;

            chaine = Encoding.ASCII.GetString(Encoding.GetEncoding(1251).GetBytes(chaine.ToLower()));
            var url = "";
            foreach (var c in chaine)
            {
                if (c != '/' && (char.IsPunctuation(c) || char.IsSeparator(c)))
                {
                    if (!url.EndsWith("-"))
                        url += "-";
                    continue;
                }
                if (!allowedChars.Contains(c.ToString().ToLower()))
                    continue;
                url += c.ToString();
            }
            return url.Trim('-').Replace("-/", "/");
        }

        /// <summary>
        /// Tester si l'email est valide ou non.
        /// </summary>
        /// <param name="email">l'email a tester</param>
        /// <returns>true si l'email est validé, false sinon</returns>
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Transformer un chaine splittée par "separator" en Liste de T
        /// </summary>
        /// <param name="uneChaine">Chaine à diviser</param>
        /// <param name="separator">Séparateur</param>
        /// <returns>Liste de T</returns>
        public static IEnumerable<T> ToEnumList<T>(this string uneChaine, char separator)
        {
            var list = new List<T>();
            if (string.IsNullOrEmpty(uneChaine) || string.IsNullOrEmpty(uneChaine.Trim()))
                return list;
            list.AddRange(from s in uneChaine.Trim().Split(separator) where typeof(T).IsEnum select (T)Enum.Parse(typeof(T), s, true));
            return list;
        }

        /// <summary>
        /// Parser un string en int?
        /// </summary>
        /// <param name="str">string a parser</param>
        /// <returns>null si le string est null, la valeur parsée de str si non</returns>
        public static int? ToIntNullable(this string str)
        {
            int i;
            if (int.TryParse(str, out i)) return i;
            return null;
        }

        /// <summary>
        /// Transformer un chaine splittée par "separator" en Liste
        /// </summary>
        /// <param name="uneChaine">Chaine à diviser</param>
        /// <param name="separator">Séparateur</param>
        /// <returns>Liste de Caractères</returns>
        public static List<string> ToStringList(this string uneChaine, char separator)
        {
            var stringList = new List<string>();
            if (string.IsNullOrEmpty(uneChaine) || string.IsNullOrEmpty(uneChaine.Trim()))
                return stringList;
            stringList.AddRange(uneChaine.Trim().Split(separator));
            return stringList;
        }
        /// <summary>
        /// Transformer un chaine splittée par "separator" en Liste
        /// </summary>
        /// <param name="uneChaine">Chaine à diviser</param>
        /// <param name="separator">Séparateur</param>
        /// <returns>Liste d'entier</returns>
        public static List<int> ToIntList(this string uneChaine, char separator)
        {
            var stringList = new List<int>();
            if (string.IsNullOrEmpty(uneChaine) || string.IsNullOrEmpty(uneChaine.Trim()))
                return stringList;

            var tab = uneChaine.Trim().Split(separator);
            stringList.AddRange(tab.Select(s => (int)int.Parse(s)));
            return stringList;
        }

        /// <summary>
        /// Encode en html une chaine de caractères sous forme ASCII (&lt; etc)
        /// </summary>
        /// <param name="uneChaine"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string uneChaine)
        {
            return HttpUtility.HtmlEncode(uneChaine);
        }
        /// <summary>
        /// Decode une chaine de caractère html en caractères normaux et affichable
        /// </summary>
        /// <param name="uneChaine"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string uneChaine)
        {
            return HttpUtility.HtmlDecode(uneChaine);
        }

        /// <summary>
        /// Faire un Split sur une chaine de caractères
        /// </summary>
        /// <param name="uneChaine">String a splitter</param>
        /// <param name="separator">Séparateur (string)</param>
        /// <returns></returns>
        public static string[] Split(this string uneChaine, string separator)
        {
            return uneChaine.Split(new string[] { separator }, StringSplitOptions.None);
        }

        /// <summary>
        /// Return if one or more occurence of the searchValues is found into the string source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="searchValues"></param>
        /// <returns></returns>
        public static bool ContainsOneOf(this string source, IEnumerable<string> searchValues)
        {
            foreach (var i in searchValues)
                if (source.Contains(i))
                    return true;
            return false;
        }
    }
}
