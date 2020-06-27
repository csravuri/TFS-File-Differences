using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSFileDiff
{
    class Program
    {

        static void Main(string[] args)
        {
            List<string> fileList = File.ReadAllLines(@"D:\Delete\testFilenames.txt").ToList();


            DiffChecker diffChecker = new DiffChecker(fileList)
            {
                ServerAddress = "http://TFSIP:8080/DefaultCollection",
                LocalBasePath = @"D:\PALMS\5.13",
                ServerBasePath = "$/5.13"
            };

            diffChecker.FindIdenticalFiles();

            List<string> identicalFileList = diffChecker.IdenticalFileList;
            List<string> changedFileList = diffChecker.ChangedFileList;

            File.WriteAllLines(@"D:\Delete\itentical.txt", identicalFileList);

            File.WriteAllLines(@"D:\Delete\changed.txt", changedFileList);





        }
    }
}
