using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Domain.Common.Services.ConnectionManager.Socket;
using Common.Lib.Extensions;
using Common.Lib.Interfaces;
using Renci.SshNet;

namespace Common.Lib.Domain.Common.Services.ConnectionManager
{
    public class ConnectionManagerService : IConnectionManagerService
    {
        public  EquipmentConnectionSetting EquipmentConnectionSetting { get; }
        private readonly ILogger _logger;
        private readonly SshClient _sshClient = null;
        private readonly SocketHelper _socketClient = null;
        private ShellStream _shellStream = null;

        private ConnectionManagerService()
        {
            
        }

        public ConnectionManagerService(EquipmentConnectionSetting settings, ILogger logger)
        {
            EquipmentConnectionSetting = settings;
            _logger = logger;

            switch (settings.ConnectionType)
            {
                case ConnectionType.TcpIp:
                    if (settings.Ip == null || settings.Port == null)
                        throw new ArgumentNullException("For TcpIp connections you must provide a IP and Port.");

                    _socketClient = new SocketHelper(
                        new SocketConnectionInfo(settings.Ip, (int) settings.Port, settings.IpVersion, ProtocolType.Tcp,
                            settings.Encoding, false, false, false, false),
                        _logger
                        );
                    break;
                case ConnectionType.Ssh:
                    if (settings.Ip == null || settings.Port == null)
                        throw new ArgumentNullException("For SSH connections you must provide a IP and Port.");

                    AuthenticationMethod method = null;
                    if (settings.AuthenticationType == AuthenticationType.Password)
                        method = new PasswordAuthenticationMethod(settings.Username, settings.Password);

                    if (method == null)
                        throw new ArgumentNullException("settings.AuthenticationType");

                    _sshClient = new SshClient(new ConnectionInfo(settings.Ip, (int)settings.Port, settings.Username, new[] { method }));
                    break;
                case ConnectionType.Telnet:
                    if (settings.ShowTelnetCodes == null || settings.RemoveNonPrintableChars == null || settings.ReplaceNonPrintableChars == null || settings.Port == null)
                        throw new Exception("For the Telnet Connection you must specify ShowTelnetCodes, RemoveNonPrintableChars, ReplaceNonPrintableChars and Port.");
                    
                    _socketClient = new SocketHelper(
                        new SocketConnectionInfo(settings.Ip, (int) settings.Port, settings.IpVersion, ProtocolType.Tcp,
                            settings.Encoding, true, (bool) settings.ShowTelnetCodes,
                            (bool) settings.RemoveNonPrintableChars, (bool) settings.ReplaceNonPrintableChars),
                        _logger
                        );
                    break;
            }
        }

        public bool IsConnected
        {
            get
            {
                switch (EquipmentConnectionSetting.ConnectionType)
                {
                    case ConnectionType.TcpIp:
                        return _socketClient != null && _socketClient.IsConnected;
                    case ConnectionType.Ssh:
                        return _sshClient != null && _sshClient.IsConnected;
                    case ConnectionType.Telnet:
                        return _socketClient != null && _socketClient.IsConnected;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public SocketResponse Connect()
        {
            string totalResponse = "";

            switch (EquipmentConnectionSetting.ConnectionType)
            {
                case ConnectionType.TcpIp:
                    _socketClient.Connect();

                    //Add login sequence here
                    if (EquipmentConnectionSetting.LoginSequences != null && EquipmentConnectionSetting.LoginSequences.Any())
                    {
                        foreach (var sequence in EquipmentConnectionSetting.LoginSequences.OrderBy(p => p.SequenceNumber))
                        {
                            if (!string.IsNullOrEmpty(sequence.ExpectedResponse))
                            {
                                var response = SendCommandAndWaitForResponse(sequence.Command.Unescape(), new List<string> { sequence.ExpectedResponse }, new TimeSpan(0, 0, 0, sequence.Timeout));
                                totalResponse += response.Data + Environment.NewLine;
                                if (response.TimeoutOccurred)
                                {
                                    throw new Exception("Unable to login." + totalResponse);
                                }  
                            }
                            else
                            {
                                SendCommand(sequence.Command);
                            }
                        }
                    }

                    break;
                case ConnectionType.Ssh:
                    _sshClient.Connect();
                    _shellStream = _sshClient.CreateShellStream("TestTerm", 80, 24, 800, 600, 1024);
                    _shellStream.ErrorOccurred += _shellStream_ErrorOccurred;

                    //Add login sequence here
                    if (EquipmentConnectionSetting.LoginSequences != null && EquipmentConnectionSetting.LoginSequences.Any())
                    {
                        foreach (var sequence in EquipmentConnectionSetting.LoginSequences.OrderBy(p => p.SequenceNumber))
                        {
                            if (!string.IsNullOrEmpty(sequence.ExpectedResponse))
                            {
                                var response = SendCommandAndWaitForResponse(sequence.Command.Unescape(), new List<string> { sequence.ExpectedResponse }, new TimeSpan(0, 0, 0, sequence.Timeout));
                                totalResponse += response.Data + Environment.NewLine;
                                if (response.TimeoutOccurred)
                                {
                                    throw new Exception("Unable to login." + totalResponse);
                                }
                            }
                            else
                            {
                                SendCommand(sequence.Command);
                            }
                        }
                    }

                    break;
                case ConnectionType.Telnet:
                    _socketClient.Connect();

                    //Add login sequence here
                    if (EquipmentConnectionSetting.LoginSequences != null && EquipmentConnectionSetting.LoginSequences.Any())
                    {
                        foreach (var sequence in EquipmentConnectionSetting.LoginSequences.OrderBy(p => p.SequenceNumber))
                        {
                            if (!string.IsNullOrEmpty(sequence.ExpectedResponse))
                            {
                                var response = SendCommandAndWaitForResponse(sequence.Command.Unescape(), new List<string> { sequence.ExpectedResponse }, new TimeSpan(0, 0, 0, sequence.Timeout));
                                totalResponse += response.Data + Environment.NewLine;
                                if (response.TimeoutOccurred)
                                {
                                    throw new Exception("Unable to login." + totalResponse);
                                }
                            }
                            else
                            {
                                SendCommand(sequence.Command);
                            }
                        }
                    }

                    break;
                default:
                    throw new NotImplementedException();
            }

            return new SocketResponse
            {
                Data = totalResponse,
                TimeoutOccurred = false
            };
        }

        public void SendCommand(string command)
        {
            switch (EquipmentConnectionSetting.ConnectionType)
            {
                case ConnectionType.TcpIp:
                    if (!_socketClient.IsConnected)
                        _socketClient.Connect();

                    _socketClient.TransmitData(command);
                    break;
                case ConnectionType.Ssh:
                    if (!_sshClient.IsConnected)
                        _sshClient.Connect();

                    _shellStream.WriteLine(command);
                    break;
                case ConnectionType.Telnet:
                    if (!_socketClient.IsConnected)
                        _socketClient.Connect();

                    _socketClient.TransmitData(command);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public SocketResponse SendCommandAndWaitForResponse(string command, List<string> expectedResponse, TimeSpan timeout)
        {
            switch (EquipmentConnectionSetting.ConnectionType)
            {
                case ConnectionType.TcpIp:
                    if (!_socketClient.IsConnected)
                        _socketClient.Connect();

                    return _socketClient.TransmitDataAndWaitForResponse(command, expectedResponse,timeout);
                case ConnectionType.Ssh:
                    if (!_sshClient.IsConnected)
                        _sshClient.Connect();

                    _shellStream.WriteLine(command);
                    return new SocketResponse
                    {
                        Data = _shellStream.Expect(expectedResponse.FirstOrDefault(), timeout)
                    };
                case ConnectionType.Telnet:
                    if (!_socketClient.IsConnected)
                        _socketClient.Connect();

                    return _socketClient.TransmitDataAndWaitForResponse(command, expectedResponse, timeout);
                default:
                    throw new NotImplementedException();
            } 
        }

        public SocketResponse SendCommandAndWaitForResponse(string command, Regex expectedRegex, TimeSpan timeout)
        {
            bool timeoutOccurred = false;
            switch (EquipmentConnectionSetting.ConnectionType)
            {
                case ConnectionType.TcpIp:
                    throw new NotImplementedException();
                case ConnectionType.Ssh:
                    if (!_sshClient.IsConnected)
                        _sshClient.Connect();
                    
                    _shellStream.WriteLine(command);
                    string data = _shellStream.Expect(expectedRegex, timeout);

                    if (data == null)
                        timeoutOccurred = true;

                    if (_shellStream.DataAvailable)
                        data +=_shellStream.Expect("", timeout);

                    return new SocketResponse
                    {
                        Data = data,
                        TimeoutOccurred = timeoutOccurred
                    };
                case ConnectionType.Telnet:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public SocketResponse Disconnect()
        {
            string totalResponse = "";

            switch (EquipmentConnectionSetting.ConnectionType)
            {
                case ConnectionType.TcpIp:

                    //Add logout sequence here
                    if (EquipmentConnectionSetting.LogoutSequences != null && EquipmentConnectionSetting.LogoutSequences.Any())
                    {
                        
                        foreach (var sequence in EquipmentConnectionSetting.LogoutSequences.OrderBy(p => p.SequenceNumber))
                        {
                            if (!string.IsNullOrEmpty(sequence.ExpectedResponse))
                            {
                                var response = SendCommandAndWaitForResponse(sequence.Command.Unescape(), new List<string>{ sequence.ExpectedResponse }, new TimeSpan(0, 0, 0, sequence.Timeout));
                                totalResponse += response + Environment.NewLine;
                                if (response.TimeoutOccurred)
                                {
                                    throw new Exception("Unable to logout." + totalResponse);
                                }
                            }
                            else
                            {
                                SendCommand(sequence.Command);
                            }
                        }
                    }

                    _socketClient.Close();
                    break;
                case ConnectionType.Ssh:

                    //Add logout sequence here
                    if (EquipmentConnectionSetting.LogoutSequences != null && EquipmentConnectionSetting.LogoutSequences.Any())
                    {
                        foreach (var sequence in EquipmentConnectionSetting.LogoutSequences.OrderBy(p => p.SequenceNumber))
                        {
                            if (!string.IsNullOrEmpty(sequence.ExpectedResponse))
                            {
                                var response = SendCommandAndWaitForResponse(sequence.Command.Unescape(), new List<string> { sequence.ExpectedResponse }, new TimeSpan(0, 0, 0, sequence.Timeout));
                                totalResponse += response.Data + Environment.NewLine;
                                if (response.TimeoutOccurred)
                                {
                                    throw new Exception("Unable to logout." + totalResponse);
                                }
                            }
                            else
                            {
                                SendCommand(sequence.Command);
                            }
                        }
                    }

                    _shellStream.Close();
                    _sshClient.Disconnect();
                    //_sshClient.Dispose();
                    break;
                case ConnectionType.Telnet:

                    //Add logout sequence here
                    if (EquipmentConnectionSetting.LogoutSequences != null && EquipmentConnectionSetting.LogoutSequences.Any())
                    {
                        foreach (var sequence in EquipmentConnectionSetting.LogoutSequences.OrderBy(p => p.SequenceNumber))
                        {
                            if (!string.IsNullOrEmpty(sequence.ExpectedResponse))
                            {
                                var response = SendCommandAndWaitForResponse(sequence.Command.Unescape(), new List<string> { sequence.ExpectedResponse }, new TimeSpan(0, 0, 0, sequence.Timeout));
                                totalResponse += response.Data + Environment.NewLine;
                                if (response.TimeoutOccurred)
                                {
                                    throw new Exception("Unable to logout." + totalResponse);
                                }
                            }
                            else
                            {
                                SendCommand(sequence.Command);
                            }
                        }
                    }

                    _socketClient.Close();
                    break;
                default:
                    throw new NotImplementedException();
            }

            return new SocketResponse
            {
                Data = totalResponse,
                TimeoutOccurred = false
            };
        }

        private void _shellStream_ErrorOccurred(object sender, Renci.SshNet.Common.ExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
