using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;

namespace SimpleFTP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_connect_Click(object sender, RoutedEventArgs e)
        {
            txt_block.Text = "";
            txt_block.Text += "Starting connection process...\n";
            Uri providedUri = ParseUrlForFTP(txt_server.Text);
            EstablishFTPConnection(providedUri);
        }

        private void EstablishFTPConnection(Uri uri)
        {
            FTPConnProfile connProfile = new FTPConnProfile(uri, txt_user.Text, txt_pass.Text, txt_port.Text);
            

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(txt_user.Text, txt_pass.Text);
            request.KeepAlive = false;
            request.UseBinary = true;
            request.UsePassive = true;
            FtpWebResponse response;
            String serverRsp = "";
            StreamReader reader;
            try
            {
                response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);
                serverRsp = reader.ReadToEnd();
                txt_block.Text += "Conncted to: " + uri+ "\n";
                txt_block.Text += "Directory Listing:\n ----------------- \n";
                txt_block.Text += serverRsp;
                reader.Close();
                response.Close();
            }
            catch (WebException ex)
            {
                txt_block.Text += ex.Message;
            }           
        }

        
        //Checks FTP Url for ftp://
        private Uri ParseUrlForFTP(string url)
        {
            Console.WriteLine(url);
            
            if(url.StartsWith("ftp://"))
            {
                Console.WriteLine("Parsed URL: " + url);
               
                return new Uri(url);
            } else
            {
                string formattedUrl = "ftp://" + url;
                Console.WriteLine("Parsed URL: " + formattedUrl);
                return new Uri(formattedUrl);
            }         
        }

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            txt_block.Text = "";
        }
    }
}
