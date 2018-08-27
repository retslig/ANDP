using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common.Lib.Domain.Common.Services.ConnectionManager.Socket
{
    public class SocketConnectionInfo
    {
        private IPEndPoint _ipEndPoint;
        private IpVersionType _ipVersion;
        private Encoding _encoder;
        private ProtocolType _socketProtocolType;
        private SocketType _socketType;
        private System.Net.Sockets.Socket _socket;

        //Telnet Protocol Specification
        //http://datatracker.ietf.org/doc/rfc854/
        private bool _isTelnet;
        private bool _showTelentCodes;
        private bool _removeNonPrintableChars;
        private bool _replaceNonPrintableChars;

        public SocketConnectionInfo(string ipOrHost, int port, IpVersionType ipVersion, ProtocolType socketProtocolType, 
            SocketType socketType, EncodingType encodingType)
        {
            IPAddress ip = null;
            try
            {
                foreach (var tempIp in Dns.GetHostAddresses(ipOrHost))
                {
                    if (ipVersion == IpVersionType.IPv4)
                    {
                        // IPv4 address
                        if (tempIp.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ip = tempIp;
                            break;
                        }
                    }
                    else if (ipVersion == IpVersionType.IPv6)
                    {
                        // IPv4 address
                        if (tempIp.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            ip = tempIp;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //If we cant find dns name assume it is an IP.
                if (!IPAddress.TryParse(ipOrHost, out ip))
                    throw new Exception("Couldn't resolve HostName (" + ipOrHost + "). " + Environment.NewLine + ex + Environment.NewLine);
            }

            if (ip == null)
            {
                throw new Exception("Couldn't resolve HostName or IP.");
            }

            switch (encodingType)
            {
                case EncodingType.Ascii:
                    _encoder = Encoding.ASCII;
                    break;
                case EncodingType.Unicode:
                    _encoder = Encoding.Unicode;
                    break;
                case EncodingType.BigEndianUnicode:
                    _encoder = Encoding.BigEndianUnicode;
                    break;
                case EncodingType.Utf32:
                    _encoder = Encoding.UTF32;
                    break;
                case EncodingType.Utf8:
                    _encoder = Encoding.UTF8;
                    break;
                case EncodingType.Utf7:
                    _encoder = Encoding.UTF7;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("encodingType");
            }
            
            _ipEndPoint = new IPEndPoint(ip, port);
            _ipVersion = ipVersion;
            _socketProtocolType = socketProtocolType;
            _socketType = socketType;
        }

        public SocketConnectionInfo(string ipOrHost, int port, IpVersionType ipVersion, ProtocolType socketProtocolType,
            EncodingType encodingType, bool isTelnet, bool showTelentCodes, bool removeNonPrintableChars, bool replaceNonPrintableChars)
        {
            IPAddress ip = null;
            try
            {
                foreach (var tempIp in Dns.GetHostAddresses(ipOrHost))
                {
                    if (ipVersion == IpVersionType.IPv4)
                    {
                        // IPv4 address
                        if (tempIp.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ip = tempIp;
                            break;
                        }
                    }
                    else if (ipVersion == IpVersionType.IPv6)
                    {
                        // IPv4 address
                        if (tempIp.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            ip = tempIp;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //If we cant find dns name assume it is an IP.
                if (!IPAddress.TryParse(ipOrHost, out ip))
                    throw new Exception("Couldn't resolve HostName (" + ipOrHost + "). " + Environment.NewLine + ex + Environment.NewLine);
            }

            if (ip == null)
            {
                throw new Exception("Couldn't resolve HostName or IP.");
            }

            switch (encodingType)
            {
                case EncodingType.Ascii:
                    _encoder = Encoding.ASCII;
                    break;
                case EncodingType.Unicode:
                    _encoder = Encoding.Unicode;
                    break;
                case EncodingType.BigEndianUnicode:
                    _encoder = Encoding.BigEndianUnicode;
                    break;
                case EncodingType.Utf32:
                    _encoder = Encoding.UTF32;
                    break;
                case EncodingType.Utf8:
                    _encoder = Encoding.UTF8;
                    break;
                case EncodingType.Utf7:
                    _encoder = Encoding.UTF7;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("encodingType");
            }

            _ipEndPoint = new IPEndPoint(ip, port);
            _ipVersion = ipVersion;
            _socketProtocolType = socketProtocolType;
            _isTelnet = isTelnet;
            _showTelentCodes = showTelentCodes;
            _removeNonPrintableChars = removeNonPrintableChars;
            _replaceNonPrintableChars = replaceNonPrintableChars;
        }

        public SocketConnectionInfo(string ipOrHost, int port, IpVersionType ipVersion, ProtocolType socketProtocolType, EncodingType encodingType)
        {
            IPAddress ip = null;
            try
            {
                foreach (var tempIp in Dns.GetHostAddresses(ipOrHost))
                {
                    if (ipVersion == IpVersionType.IPv4)
                    {
                        // IPv4 address
                        if (tempIp.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ip = tempIp;
                            break;
                        }
                    }
                    else if (ipVersion == IpVersionType.IPv6)
                    {
                        // IPv4 address
                        if (tempIp.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            ip = tempIp;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //If we cant find dns name assume it is an IP.
                if (!IPAddress.TryParse(ipOrHost, out ip))
                    throw new Exception("Couldn't resolve HostName (" + ipOrHost + "). " + Environment.NewLine + ex + Environment.NewLine);
            }

            if (ip == null)
            {
                throw new Exception("Couldn't resolve HostName or IP.");
            }

            switch (encodingType)
            {
                case EncodingType.Ascii:
                    _encoder = Encoding.ASCII;
                    break;
                case EncodingType.Unicode:
                    _encoder = Encoding.Unicode;
                    break;
                case EncodingType.BigEndianUnicode:
                    _encoder = Encoding.BigEndianUnicode;
                    break;
                case EncodingType.Utf32:
                    _encoder = Encoding.UTF32;
                    break;
                case EncodingType.Utf8:
                    _encoder = Encoding.UTF8;
                    break;
                case EncodingType.Utf7:
                    _encoder = Encoding.UTF7;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("encodingType");
            }

            _ipEndPoint = new IPEndPoint(ip, port);
            _ipVersion = ipVersion;
            _socketProtocolType = socketProtocolType;
        }

        /// <summary> This property is for collection info about the socket. </summary>
        public System.Net.Sockets.Socket Socket
        {
            set { _socket = value; }
        }

        /// <summary> This property is the Socket Type that you want your connection to connect with. raw, Stream, etc... </summary>
        public SocketType SocketType
        {
            get { return _socketType; }
            set { _socketType = value; }
        }

        /// <summary> This property is the Protocol Type that you want your connection to connect with. </summary>
        public ProtocolType SocketProtocolType
        {
            get { return _socketProtocolType; }
            set { _socketProtocolType = value; }
        }

        /// <summary> This property is the Encoding you want your data sent and recieved in. </summary>
        public Encoding Encoder
        {
            get { return _encoder; }
            set { _encoder = value; }
        }

        /// <summary> This property is the IPVersion you want your connection to connect with.(IPV4 or IPV6) </summary>
        public IpVersionType IPVersion
        {
            get { return _ipVersion; }
            set { _ipVersion = value; }
        }

        /// <summary> This property is the IP enpoit that include the port you want your connection to connect too. </summary>
        public IPEndPoint IpEndPoint
        {
            get { return _ipEndPoint; }
            set { _ipEndPoint = value; }
        }

        /// <summary> This property if true this will display the requests and responses from the server and client when setting up the connection. </summary>
        public bool ShowTelentCodes
        {
            get { return _showTelentCodes; }
            set { _showTelentCodes = value; }
        }

        /// <summary> This property allows you to remove non printable characters from your response. It allows for set or get. </summary>
        public bool RemoveNonPrintableChars
        {
            get { return _removeNonPrintableChars; }
            set { _removeNonPrintableChars = value; }
        }

        /// <summary> This property allows you to remove non printable characters from your response. It allows for set or get. </summary>
        public bool ReplaceNonPrintableCharacters
        {
            get { return _replaceNonPrintableChars; }
            set { _replaceNonPrintableChars = value; }
        }

        /// <summary> This property if in TCP mode will use the Telnet protocol as specified in the RFC 854. </summary>
        public bool IsTelnet
        {
            get { return _isTelnet; }
            set { _isTelnet = value; }
        }

        /// <summary> This property returns all the valid operating system encodings available.</summary>
        public string AvailableOperatingSystemEncodingMembers
        {
            get
            {
                string Encodings = "";
                foreach (EncodingInfo test in Encoding.GetEncodings())
                {
                    Encodings += test.DisplayName + ",";
                }
                return Encodings;
            }
        }

        /// <summary> This property returns all protocol types for a socket.</summary>
        public string AvailableSocketProtocolTypeMembers
        {
            get
            {
                return "IP,IPv6HopByHopOptions,Icmp,Igmp,Ggp,IPv4,Tcp,Pup,Udp,Idp,IPv6," +
                       "IPv6RoutingHeader,IPv6FragmentHeader,IPSecEncapsulatingSecurityPayload," +
                       "IPSecAuthenticationHeader,IcmpV6,IPv6NoNextHeader,IPv6DestinationOptions," +
                       "ND,Raw,Unspecified,Ipx,Spx,SpxII,Unknown";
            }
        }

        /// <summary> This property returns all socket types avaiable.</summary>
        public string AvailableSocketTypeMembers
        {
            get { return "Dgram,Raw,Rdm,Seqpacket,Stream,Unknown"; }
        }

        /// <summary> This property returns windows code page of your system.</summary>
        public int CurrentWindowsCodePageEncoding
        {
            get { return Encoding.Default.WindowsCodePage; }
        }

        /// <summary> This property returns the default encoding of the operating system.</summary>
        public string CurrentOperatingSystemEncoding
        {
            get { return Encoding.Default.EncodingName; }
        }

        /// <summary> This property returns protocol type of the socket.</summary>
        public string CurrentSocketProtocolType
        {
            get { return Enum.GetName(typeof(ProtocolType), _socket.ProtocolType); }
        }

        /// <summary> This property returns the type of socket.</summary>
        public string CurrentSocketType
        {
            get { return Enum.GetName(typeof(SocketType), _socket.SocketType); }
        }

        /// <summary> This property returns true if the socket is connected.</summary>
        public bool CurrentSocketConnected
        {
            get { return _socket.Connected; }
        }

        /// <summary> This property returns the amount of data that is available to read from the socket.</summary>
        public int CurrentSocketDataAvailable
        {
            get { return _socket.Available; }
        }
    }
}