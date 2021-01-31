using System;
using System.IO;

namespace Tools.Utils
{
    public static class DirectoryUtils
    {
        /// <summary>
        /// Créé tous les dossiers parent du dossier en paramètre
        /// <example>Si \\SERVEUR\DOSSIER\toto\titi envoyé et que toto existe pas, alors il sera crée, puis titi sera crée</example>
        /// </summary>
        /// <param name="folder">Objet dossier décrivant le dossier à crée</param>
        public static void CreateParentsFolders(this DirectoryInfo folder)
        {
            if (folder == null || folder.Exists)
                return;
            CreateParentsFolders(folder.Parent);
            folder.Create();
        }

        /// <summary>
        /// Copy Directory
        /// </summary>
        /// <param name="sourceDirectory">absolute source path from copy</param>
        /// <param name="destDirectory">absolute destination path to copy</param>
        /// <param name="includeSubDirectory">include SubDirectory or not</param>
        public static void CopyTo(this DirectoryInfo sourceDirectory, DirectoryInfo destDirectory, bool includeSubDirectory = true)
        {
            if (!sourceDirectory.Exists)
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirectory.Root);

            // If the destination directory doesn't exist, create it. 
            CreateParentsFolders(destDirectory);

            // Get the files in the directory and copy them to the new location.
            var files = sourceDirectory.GetFiles();
            foreach (var file in files)
                file.CopyTo(Path.Combine(destDirectory.FullName, file.Name), false);

            // If copying subdirectories, copy them and their contents to new location. 
            if (!includeSubDirectory) return;
            foreach (var subdir in sourceDirectory.GetDirectories())
                subdir.CopyTo(destDirectory.CreateSubdirectory(subdir.Name), true);
        }

        /// <summary>
        /// Effacer tout le contenu trouvé dans le dossier passé en paramètre
        /// </summary>
        /// <param name="directory">Dossier dans lequel tout effacer</param>
        public static void DeleteAllContent(this DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles()) file.Delete();
            foreach (var subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
    }
}
