using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tools.Utils
{
    public static class PathUtils
    {
        /// <summary>
        /// Normalize a path (add backslash if does not exist)
        /// </summary>
        /// <param name="path">Path To normalize</param>
        /// <returns></returns>
        public static string Normalize(string path)
        {
            if (string.IsNullOrEmpty(path)) 
                return null;
            return path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path : path + Path.DirectorySeparatorChar;
        }
    }
}
