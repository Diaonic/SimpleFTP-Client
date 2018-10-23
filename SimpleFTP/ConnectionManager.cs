using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFTP
{
    class ConnectionManager
    {
        public void ConnectToFTP()
        {

        }

        public void DisconnectFromFTP()
        {

        }

        public void UploadFile()
        {

        }

        public void DownloadFile()
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
