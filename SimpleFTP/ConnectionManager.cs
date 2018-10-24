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

        public bool ConnectToFTP(ConnectionProfile connProfile)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(connProfile.ConnUri);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(connProfile.ConnUser, connProfile.ConnPass);
            request.KeepAlive = connProfile.ConnKeepAlive;
            request.UseBinary = connProfile.ConnBinary;
            request.UsePassive = connProfile.ConnPassiveMode;

            FtpWebResponse response;
            String serverRsp = "";
            StreamReader reader;
            try
            {
                response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);
                serverRsp = reader.ReadToEnd();
                // txt_block.Text += "Conncted to: " + uri + "\n";
                // txt_block.Text += "Directory Listing:\n ----------------- \n";
                //txt_block.Text += serverRsp;
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
