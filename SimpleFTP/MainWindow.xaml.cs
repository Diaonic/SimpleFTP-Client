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
using System.Threading;

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
        public static MainWindow mainWindow;

        public MainWindow()
        {
            InitializeComponent();
            cursorActive = false;
            DispatcherTimerSample();
            mainWindow = this;
        }

        private void Btn_connect_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                AppendConsole("Starting connection process...");
                Uri providedUri = ParseUrlForFTP(txt_server.Text, txt_port.Text);

                connProfile = new ConnectionProfile(providedUri, txt_user.Text, txt_pass.Password, txt_port.Text);
                connMan = new ConnectionManager();
                bool isConnected = connMan.ConnectToFTP(connProfile);

                if (isConnected)
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
            }), DispatcherPriority.Background);
           
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

        //Clears console window
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
           txt_AppendConsole.Text = "";
        }

        // Browse for file to upload
        private void Btn_fileBrowse_Click(object sender, RoutedEventArgs e)
        {
            AppendConsole("Browsing for local files...");
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            
            if (result == true)
            {
                string filename = dlg.FileName;
                txt_fileToUpload.Text = filename;
                AppendConsole("File Selected: " + filename);
                Btn_uploadFile.IsEnabled = true;  
            }
        }



        public void AppendConsole(string message)
        {         
            Dispatcher.BeginInvoke(new Action(() =>
            {
                txt_AppendConsole.Text += message;
                txt_AppendConsole.Text += "\n";
                txt_AppendConsole.ScrollToEnd();
            }), DispatcherPriority.Background);

        }

        public void DispatcherTimerSample()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(.5)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if(!cursorActive)
            {
                txt_AppendConsole.Text += "_";
                cursorActive = true;
            } else
            {
                txt_AppendConsole.Text = txt_AppendConsole.Text.Replace("_", "");
                cursorActive = false;
            }
        }

        private void Btn_uploadFile_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                AppendConsole(ConnectionManager.FtpUpload(connProfile.ConnUri.ToString(), connProfile.ConnUser, connProfile.ConnPass, txt_fileToUpload.Text));
            }), DispatcherPriority.Background);

        }

        private void Btn_Download_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                // Using 5 as a check due to extension and dot character in the file name
                if (txt_downloadFile.Text.Length >= 5)
                {
                    foreach (var line in connMan.lines)
                    {
                        if (line.FileName.Equals(txt_downloadFile.Text))
                        {
                            //download
                            AppendConsole(String.Format("File match found! {0}", txt_downloadFile.Text));
                            AppendConsole("Starting Download...");
                            this.Dispatcher.Invoke(() =>
                            {
                                ConnectionManager.FtpDownload(
                                connProfile.ConnUri.ToString(),
                                connProfile.ConnUser,
                                connProfile.ConnPass,
                                txt_downloadFile.Text,
                                "../",
                                line.FileSize);
                            });
                            txt_downloadFile.Text = "";
                        }
                    }
                }
            }), DispatcherPriority.Background);
           
        }
    }
}
