using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFTP
{
    class ConnectionProfile
    {

        private Uri _connUri;
        private string _connUser;
        private string _connPass;
        private string _connPort;
        private bool _connKeepAlive;
        private bool _connBinary;
        private bool _connPassiveMode;

        public ConnectionProfile (Uri uri, string user, string pass, string port, bool keepalive = false, bool binary = true, bool passivemode = true)
        {
            ConnUri            = uri;
            ConnUser           = user;
            ConnPass           = pass;
            ConnPort           = port;
            ConnKeepAlive      = keepalive;
            ConnBinary         = binary;
            ConnPassiveMode    = passivemode;
        }

        public Uri ConnUri
        {
            get { return _connUri; }
            set { _connUri = value; }
        }

        public string ConnUser
        {
            get { return _connUser; }
            set { _connUser = value; }
        }

        public string ConnPass
        {
            get { return _connPass; }
            set { _connPass = value; }
        }

        public string ConnPort
        {
            get { return _connPort; }
            set { _connPort = value; }
        }

        public bool ConnKeepAlive
        {
            get { return _connKeepAlive; }
            set { _connKeepAlive = value; }
        }

        public bool ConnBinary
        {
            get { return _connBinary; }
            set { _connBinary = value;}
        }

        public bool ConnPassiveMode
        {
            get { return _connPassiveMode; }
            set { _connPassiveMode = value; }
        }
    }
}
