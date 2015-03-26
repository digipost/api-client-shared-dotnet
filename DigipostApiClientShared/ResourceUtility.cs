using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DigipostApiClientShared
{
    public class ResourceUtility
    {
        /// <summary>
        /// Initialize a resource utility with base path for resources.
        /// </summary>
        /// <param name="basePathForResources"> Must be in following form 'SolutionNameSpace.ProjectName.File.Path.Separated.ByDots>'"</param>
        public ResourceUtility(string basePathForResources)
        {
            BasePath = basePathForResources;
            CurrentExecutingAssembly = Assembly.GetCallingAssembly();
        }

        public string BasePath { get; set; }
        public Assembly CurrentExecutingAssembly { get; set; }

        public IEnumerable<string> GetFiles(params string[] pathRelativeToBase)
        {
            var path = GetFullPath(pathRelativeToBase);

            return GetAllFiles().Where(file => file.Contains(path));
        }

        public string GetFileName(string resource, bool withExtension = true)
        {
            var parts = resource.Split('.');
            var filename = parts[parts.Length - 2];

            if (withExtension)
            {
                var extension = parts[parts.Length - 1];
                filename = String.Format("{0}.{1}", filename, extension);
            }

            return filename;
        }

        public byte[] ReadAllBytes(bool isRelative, params string[] path)
        {
            var fullpath = isRelative ? GetFullPath(path) : String.Join(".", path);

            using (Stream fileStream = CurrentExecutingAssembly.GetManifestResourceStream(fullpath))
            {
                if (fileStream == null) return null;
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        internal string GetFullPath(params string[] path)
        {
            return String.Join(".", BasePath, String.Join(".", path));
        }

        private IEnumerable<string> GetAllFiles()
        {
            return CurrentExecutingAssembly.GetManifestResourceNames();
        }
    }
}
