using System;
using System.IO;

namespace Tools.Utils
{
    public static class FileUtils
    {
        /// <summary>
        ///  Copier un fichier, en créant les dossiers parents s'ils n'existent pas
        /// </summary>
        /// <param name="file">Fichier a copier</param>
        /// <param name="destinationPath">dossier de destination</param>
        public static void CopyTo(this FileInfo file, DirectoryInfo destinationPath)
        {
            CopyTo(file, new FileInfo(destinationPath.FullName + '/' + file.Name));
        }

        /// <summary>
        ///  Copier un fichier, en créant les dossiers parents s'ils n'existent pas
        /// </summary>
        /// <param name="file">Fichier a copier</param>
        /// <param name="destinationPath">fichier de destination</param>
        public static void CopyTo(this FileInfo file, FileInfo destinationPath)
        {
            if (File.Exists(destinationPath.FullName))
                File.Delete(destinationPath.FullName);
            destinationPath.Directory.CreateParentsFolders();
            File.Copy(file.FullName, destinationPath.FullName);
        }

        /// <summary>
        /// Check if a file exists or not. Will return true if the file is used by another process instead of throw an exception like File.Exists()
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            try
            {
                return File.Exists(path);
            }
            catch (IOException e)
            {
                if (e.Message.Contains("it is being used by another process"))
                    return true;
                else
                    throw e;
            }
        }

        /// <summary>
        /// Creates a new file (with his parent directories), writes the specified string to the file, and then closes
        //     the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="filePath">chemin vers le fichier</param>
        /// <param name="contents">contenu du fichier</param>
        public static void WriteAllText(string filePath, string contents)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new Exception("Can't create the file -> file path is null or empty");
            new DirectoryInfo(Path.GetDirectoryName(filePath)).CreateParentsFolders();
            File.WriteAllText(filePath, contents ?? "");
        }

        /// <summary>
        /// Opens a file, add a new line, appends the specified string to the file, and then closes the file.
        //     If the file does not exist, this method creates a file, writes the specified
        //     string to the file, then closes the file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newLine"></param>
        public static void AppendNewLine(string filePath, string newLine)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new Exception("Can't create the file -> file path is null or empty");

            if (!File.Exists(filePath))
                WriteAllText(filePath, newLine ?? "");
            File.AppendAllText(filePath, Environment.NewLine + newLine ?? "");
        }
    }
}
