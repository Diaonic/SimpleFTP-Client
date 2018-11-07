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
using System.Windows.Threading;

namespace SimpleFTP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool cursorActive;
        ConnectionManager connMan;
        ConnectionProfile connProfile;
        public MainWindow()
        {
            InitializeComponent();
            cursorActive = false;
            DispatcherTimerSample();
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            AppendConsole("Starting connection process...");
            Uri providedUri = ParseUrlForFTP(txt_server.Text, txt_port.Text);

            connProfile = new ConnectionProfile(providedUri, txt_user.Text, txt_pass.Text, txt_port.Text);
            connMan = new ConnectionManager();
            bool isConnected = connMan.ConnectToFTP(connProfile);

            if(isConnected)
            {
                AppendConsole("Connected!");
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("{0, -30}{1, -15}", "File Name", "File Size"));
              
                foreach (var item in connMan.lines)
                {
                    sb.AppendLine(string.Format("{0, -30}{1, -15}", item.FileDisplayName, item.FileSize));
                }
                txt_remoteFileSystem.Text = sb.ToString();
            }
            AppendConsole(connMan.connResponse);
        }

              
        //Checks FTP Url for ftp://
        private Uri ParseUrlForFTP(string url, string port)
        {
            AppendConsole("Parsing provided url and port...");
            if(url.StartsWith("ftp://"))
            {
                AppendConsole(url + ":" + port);
                return new Uri(url + ":" + port);
            } else
            {
                string formattedUrl = "ftp://" + url;
                AppendConsole(formattedUrl + ":" + port);
                return new Uri(formattedUrl + ":" + port);
            }         
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
           txt_AppendConsole.Text = "";
        }

        private void btn_fileBrowse_Click(object sender, RoutedEventArgs e)
        {
            AppendConsole("Browsing for local files...");
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                txt_fileToUpload.Text = filename;
                AppendConsole("File Selected: " + filename);
                btn_uploadFile.IsEnabled = true;  
            }
        }

        private void AppendConsole(string message)
        {
            txt_AppendConsole.Text += message;
            txt_AppendConsole.Text += "\n";
            txt_AppendConsole.ScrollToEnd();
        }

        public void DispatcherTimerSample()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(.5);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if(!cursorActive)
            {
                txt_AppendConsole.Text += "_";
                cursorActive = true;
            } else
            {
                txt_AppendConsole.Text = txt_AppendConsole.Text.Replace("_", "");
                //txt_AppendConsole.Text = txt_AppendConsole.Text.Remove(txt_AppendConsole.Text.Length - 1);
                cursorActive = false;
            }
        }

        private void btn_uploadFile_Click(object sender, RoutedEventArgs e)
        {
            connMan.UploadFile(txt_fileToUpload.Text, connProfile.ConnUri);
        }
    }
}
