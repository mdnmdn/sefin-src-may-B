using System;
using System.IO;

namespace Sefin.AnacenImporter
{
    internal class ImportFileInfo
    {
        public ImportFileInfo()
        {
        }

        public string Abi { get; set; }
        public string FilePath { get; set; }

        internal void MoveToFolder(string destinationFolder)
        {
            var fileName = Path.GetFileName(FilePath);
            var destinationPath = Path.Combine(destinationFolder, fileName);
            File.Move(FilePath, destinationPath);
            FilePath = destinationPath;
        }

        public override string ToString()
        {
            return "[" + Abi + "] - " + Path.GetFileName(FilePath);  
        }
    }
}