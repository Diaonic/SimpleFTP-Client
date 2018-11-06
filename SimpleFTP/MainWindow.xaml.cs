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
        ConnectionManager connMan; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_connect_Click(object sender, RoutedEventArgs e)
        {
            txt_block.Text = "";
            txt_block.Text += "Starting connection process...\n";
            Uri providedUri = ParseUrlForFTP(txt_server.Text);

            ConnectionProfile connProfile = new ConnectionProfile(providedUri, txt_user.Text, txt_pass.Text, txt_port.Text);
            connMan = new ConnectionManager();
            bool isConnected = connMan.ConnectToFTP(connProfile);
            if(isConnected)
            {
                lst_fileBox.Items.Add("File Name         File Size            Modified Date");
                foreach (var item in connMan.lines)
                {
                    lst_fileBox.Items.Add(item.FileName + " ");
                }
            }
            txt_block.Text = connMan.connResponse;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Uri providedUri = ParseUrlForFTP(txt_server.Text);
            ConnectionProfile connProfile = new ConnectionProfile(providedUri, txt_user.Text, txt_pass.Text, txt_port.Text);

            //connMan.DownloadFile(connProfile, "FTP.txt", @"c:\test\FTP.txt");
            WebClient Client = new WebClient();
            Client.Credentials = new NetworkCredential(connProfile.ConnUser, connProfile.ConnPass);
            Client.DownloadFile(providedUri + "FTP.txt", @"C:\test\FTP.txt");
        }


    }
}
