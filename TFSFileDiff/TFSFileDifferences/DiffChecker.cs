using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFSFileDiff
{
    public class DiffChecker
    {
        private List<string> localFileList;
        public string ServerAddress { get; set; }
        public string LocalBasePath { get; set; }
        public string ServerBasePath { get; set; }
        public List<string> IdenticalFileList { get; private set; }
        public List<string> ChangedFileList { get; private set; }

        public DiffChecker(List<string> LocalFileList)
        {
            localFileList = LocalFileList;
        }


        public void FindIdenticalFiles()
        {
            Initialize();
            foreach (string eachLocalFile in localFileList)
            {
                if(isFileIdentical(eachLocalFile))
                {
                    IdenticalFileList.Add(eachLocalFile);
                }
                else
                {
                    ChangedFileList.Add(eachLocalFile);
                }
            }

        }

        private bool isFileIdentical(string filePath)
        {
            string localFileText = GetLocalFiletext(filePath);

            string serverFilePath = ServerBasePath + filePath.Substring(LocalBasePath.Length, filePath.Length - LocalBasePath.Length).Replace("\\","/");

            string serverFileText = GetServerFileText(serverFilePath);

            if(localFileText.Equals(serverFileText))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private string GetLocalFiletext(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        private string GetServerFileText(string filePath)
        {
            TfsTeamProjectCollection server = new TfsTeamProjectCollection(new Uri(ServerAddress));
            VersionControlServer version = server.GetService(typeof(VersionControlServer)) as VersionControlServer;
            Item item = version.GetItem(filePath);
            string tempFileName = System.IO.Path.GetTempFileName();
            item.DownloadFile(tempFileName);

            return File.ReadAllText(tempFileName);
        }

        private void Initialize()
        {
            if (IdenticalFileList == null)
            {
                IdenticalFileList = new List<string>();
            }
            if (ChangedFileList == null)
            {
                ChangedFileList = new List<string>();
            }

        }

    }
}
