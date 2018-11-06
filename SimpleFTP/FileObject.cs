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
        private string _fileSize;
        private string _fileDate;


        public FileObject(string fname, string fsize, string fdate)
        {
            _fileName = fname;
            _fileSize = fsize;
            _fileDate = fdate; 
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
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
