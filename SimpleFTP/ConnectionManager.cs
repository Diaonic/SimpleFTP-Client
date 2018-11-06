using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SimpleFTP
{
    class ConnectionManager
    {
        public string connResponse;
        public List<FileObject> lines = new List<FileObject>();
        public bool ConnectToFTP(ConnectionProfile connProfile)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(connProfile.ConnUri);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(connProfile.ConnUser, connProfile.ConnPass);
            request.KeepAlive = connProfile.ConnKeepAlive;
            request.UseBinary = connProfile.ConnBinary;
            request.UsePassive = connProfile.ConnPassiveMode;

            FtpWebResponse response;
            Stream responseStream;
            String serverRsp = "";
            lines.Clear();
            try
            {
                response = (FtpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
               
                string line = "";
                while((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    serverRsp += line;
                    
                    lines.Add(ParseResponseObjects(line));
                }
                connResponse = serverRsp;
                reader.Close();
                response.Close();
                return true;
            }
            catch (WebException ex)
            {
                connResponse = ex.Message;
                return false;
            }
        }

        // Parses http stream response into a FileObject
        public FileObject ParseResponseObjects(string line)
        {
            string[] splitFile = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            return new FileObject(splitFile[8], splitFile[7], splitFile[6]);
        }

        public void DownloadFile(ConnectionProfile connProfile, string fileName, string localPath)
        {
            Console.WriteLine("Download URL: " + connProfile.ConnUri + fileName);
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential(connProfile.ConnUser, connProfile.ConnPass);
            client.DownloadFile(connProfile.ConnUri + fileName, localPath);
            
        }

        public void DisconnectFromFTP()
        {

        }

        public void UploadFile()
        {

        }

      
        public void CleanupConnObjs()
        {

        }

        public Uri ParseConnectionUrl(string url)
        {
            Console.WriteLine(url);
            if (url.StartsWith("ftp://"))
            {
                Console.WriteLine("Parsed URL: " + url);

                return new Uri(url);
            }
            else
            {
                string formattedUrl = "ftp://" + url;
                Console.WriteLine("Parsed URL: " + formattedUrl);
                return new Uri(formattedUrl);
            }
        }
    }
}
