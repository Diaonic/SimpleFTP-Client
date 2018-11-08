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
                connResponse += "Downloading file list...";               
                reader.Close();
                response.Close();
                return true;
            }
            catch (WebException ex)
            {
                connResponse += ex.Message;
                return false;
            }
        }

        // Parses http stream response into a FileObject
        public FileObject ParseResponseObjects(string line)
        {
            string[] splitFile = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            return new FileObject(splitFile[8], splitFile[4], splitFile[6]);
        }

        public void DownloadFile(ConnectionProfile connProfile, string fileName, string localPath)
        {
            Console.WriteLine("Download URL: " + connProfile.ConnUri + fileName);
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential(connProfile.ConnUser, connProfile.ConnPass);
            client.DownloadFile(connProfile.ConnUri + fileName, localPath);
            
        }

        public static string FtpUpload(string uri, string userName, string password, string filePath)
        {
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri + "/" + System.IO.Path.GetFileName(filePath));
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(userName, password);

                // Copy the contents of the file to the request stream.
                StreamReader sourceStream = new StreamReader(filePath);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                response.Close();

                return response.StatusDescription;
            }
            catch (Exception w)
            {
                return w.Message;
            }        
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
