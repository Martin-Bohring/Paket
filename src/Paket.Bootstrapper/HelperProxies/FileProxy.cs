using System.IO;
using System.IO.Compression;

namespace Paket.Bootstrapper.HelperProxies
{
    class FileProxy : IFileProxy
    {
        public bool Exists(string filename)
        {
            return File.Exists(filename);
        }

        public void Copy(string fileFrom, string fileTo, bool overwrite)
        {
            File.Copy(fileFrom, fileTo, overwrite);
        }

        public void Delete(string filename)
        {
            File.Delete(filename);
        }

        public Stream Create(string filename)
        {
            return File.Create(filename);
        }

        public string GetLocalFileVersion(string filename)
        {
            return BootstrapperHelper.GetLocalFileVersion(filename);
        }

        public void FileMove(string fromFile, string toFile)
        {
            BootstrapperHelper.FileMove(fromFile, toFile);
        }

        public void ExtractToDirectory(string zipFile, string targetLocation)
        {
            ZipFile.ExtractToDirectory(zipFile, targetLocation);
        }
        
    }
}