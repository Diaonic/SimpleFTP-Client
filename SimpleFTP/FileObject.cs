using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFTP
{
    class FileObject
    {
        private string _fileName;
        private string _fileDisplayName;
        private string _fileSize;
        private string _fileDate;


        public FileObject(string fname, string fsize, string fdate)
        {
            FileName = fname;
            FileSize = fsize;
            FileDate = fdate; 
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                Console.WriteLine(value.Length);
               if(value.Length >= 25)
                {
                    _fileName = value;
                    _fileDisplayName = _fileName.Substring(0, 10) + "..." + _fileName.Substring(_fileName.Length-6);

                } else
                {
                    _fileName = value;
                    _fileDisplayName = value;
                }
            }
        }

        public string FileDisplayName
        {
            get { return _fileDisplayName; }
        }

        public string FileSize
        {
            get { return _fileSize; }
            set { _fileSize = value; }
        }

        public string FileDate
        {
            get { return _fileDate; }
            set { _fileDate = value; }
        }
    }
}
