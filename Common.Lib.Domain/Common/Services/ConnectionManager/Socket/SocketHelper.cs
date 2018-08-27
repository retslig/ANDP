using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Common.Lib.Common.Enums;
using Common.Lib.Interfaces;
using Common.Lib.Utility;

namespace Common.Lib.Domain.Common.Services.ConnectionManager.Socket
{
    public class SocketHelper
    {
        private System.Net.Sockets.Socket _socket;
        private byte[] _buffer;
        private string _receivedData = "";
        private List<string> _expectedResponse = new List<string>();
        private EventWaitHandle _dataReceivedSignal;
        private readonly SocketConnectionInfo _connInfo;
        private readonly ILogger _logger;

        public SocketHelper(SocketConnectionInfo connInfo, ILogger logger)
        {
            _connInfo = connInfo;
            _logger = logger;
        }

        public SocketConnectionInfo ConnectionInfo
        {
            get { return _connInfo; }
        }
        
        public bool IsConnected
        {
            get
            {
                return _socket != null && _socket.Connected;
            }
        }

        /// <summary> This method opens a socket connection to the specified server.</summary>
        /// <param name="timeoutInMilliSeconds"> This is the amount of time in milliseconds that this method will wait for data. If you set Timeout to a negative number then we dont wait for a response.</param>
        /// <param name="expectedResponse"> This is the expected string reponse from the server.</param>
        /// <returns> The return value is a string of the response from the server.</returns>
        public SocketResponse Connect(int timeoutInMilliSeconds, string expectedResponse)
        {
            bool timedOut = false;

            if (_connInfo == null)
                throw new Exception("The SocketConnectionInfo class is null this is mandatory field.");

            if (_connInfo.SocketProtocolType != ProtocolType.Tcp && _connInfo.IsTelnet)
                throw new Exception("If you want to use the Telnet Protocol you must set your ProtocolType to \"TCP\".");

            if (_connInfo.IPVersion == IpVersionType.IPv6 && !System.Net.Sockets.Socket.OSSupportsIPv6)
            {
                throw new Exception("Your system does not support IPv6\r\n" +
                    "Check you have IPv6 enabled and have changed machine.config");
            }
            
            // Create a TCP/IP  socket.
            _socket = new System.Net.Sockets.Socket(_connInfo.IPVersion == IpVersionType.IPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork, _connInfo.SocketType, _connInfo.SocketProtocolType);
            //Connect
            _socket.Connect(_connInfo.IpEndPoint);
            //Store the cocket info once connected.
            _connInfo.Socket = _socket;

            _buffer = new byte[1024];
            _receivedData = "";
            _expectedResponse = new List<string> { expectedResponse };
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);

            _dataReceivedSignal = new EventWaitHandle(false, EventResetMode.ManualReset);
            
            //If you set Timeout to a negative number then we dont wait for a response.
            if (timeoutInMilliSeconds >= 0)
            {
                //If timeout is equal to 0 seconds then default to 10 seconds
                timeoutInMilliSeconds = timeoutInMilliSeconds == 0 ? 10000 : timeoutInMilliSeconds;
                //Will wait while data is returned.
                timedOut = !_dataReceivedSignal.WaitOne(timeoutInMilliSeconds);
            }

            return new SocketResponse
            {
                Data = _receivedData,
                TimeoutOccurred = timedOut
            };
        }

        /// <summary> This method opens a socket connection to the specified server.</summary>
        /// <returns> The return value is a string of the response from the server.</returns>
        public void Connect()
        {
            if (_connInfo == null)
                throw new Exception("The SocketConnectionInfo class is null this is mandatory field.");

            if (_connInfo.SocketProtocolType != ProtocolType.Tcp && _connInfo.IsTelnet)
                throw new Exception("If you want to use the Telnet Protocol you must set your ProtocolType to \"TCP\".");

            if (_connInfo.IPVersion == IpVersionType.IPv6 && !System.Net.Sockets.Socket.OSSupportsIPv6)
            {
                throw new Exception("Your system does not support IPv6\r\n" +
                    "Check you have IPv6 enabled and have changed machine.config");
            }

            // Create a TCP/IP  socket.
            _socket = new System.Net.Sockets.Socket(_connInfo.IPVersion == IpVersionType.IPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork, _connInfo.SocketType, _connInfo.SocketProtocolType);
            //Connect
            _socket.Connect(_connInfo.IpEndPoint);
            //Store the cocket info once connected.
            _connInfo.Socket = _socket;

            _buffer = new byte[1024];
            _receivedData = "";
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
        }

        /// <summary> This method opens a synchronous socket connection to the specified server.</summary>
        /// <returns> The return value is a string of the response from the server.</returns>
        public void ConnectSynchronous()
        {
            if (_connInfo == null)
                throw new Exception("The SocketConnectionInfo class is null this is mandatory field.");

            if (_connInfo.SocketProtocolType != ProtocolType.Tcp && _connInfo.IsTelnet)
                throw new Exception("If you want to use the Telnet Protocol you must set your ProtocolType to \"TCP\".");

            if (_connInfo.IPVersion == IpVersionType.IPv6 && !System.Net.Sockets.Socket.OSSupportsIPv6)
            {
                throw new Exception("Your system does not support IPv6\r\n" +
                    "Check you have IPv6 enabled and have changed machine.config");
            }

            // Create a TCP/IP  socket.
            _socket = new System.Net.Sockets.Socket(_connInfo.IPVersion == IpVersionType.IPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork, _connInfo.SocketType, _connInfo.SocketProtocolType);
            //Connect
            _socket.Connect(_connInfo.IpEndPoint);
            //Store the socket info once connected.
            _connInfo.Socket = _socket;

            _buffer = new byte[1024];
        }

        /// <summary> This method sends data down your connected socket connection to the specified server.</summary>
        /// <param name="command"> This is the command that your are sending to the server.</param>
        /// <param name="timeout"> This is the amount of time in milliseconds that this method will wait for data. If you set Timeout to a negative number then we dont wait for a response.</param>
        /// <param name="expectedResponse"> This is the expected string response from the server.</param>
        /// <returns> The return value is a string of the response from the server.</returns>
        public SocketResponse TransmitDataAndWaitForResponse(string command, string expectedResponse, TimeSpan timeout)
        {
            string data = "";
            bool timedOut = false;
            _expectedResponse = new List<string> { expectedResponse };

            if (!_socket.Connected)
                throw new Exception("Socket is no longer connected." + Environment.NewLine);

            //_buffer = new byte[1024];
            //_socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
            TransmitString(command);

            _dataReceivedSignal = new EventWaitHandle(false, EventResetMode.ManualReset);
            
            int timeoutInMilliSeconds = Convert.ToInt32(timeout.TotalMilliseconds);

            //If you set Timeout to a negative number then we dont wait for a response.
            if (timeoutInMilliSeconds >= 0)
            {
                //If timeout is equal to 0 seconds then default to 10 seconds
                timeoutInMilliSeconds = timeoutInMilliSeconds == 0 ? 10000 : timeoutInMilliSeconds;
                //Will wait while data is returned.
                timedOut = !_dataReceivedSignal.WaitOne(timeoutInMilliSeconds);

                data = _receivedData;
                _receivedData = "";
            }

            return new SocketResponse
            {
                Data = data,
                TimeoutOccurred = timedOut
            };
        }

        /// <summary> This method sends data down your connected socket connection to the specified server.</summary>
        /// <param name="command"> This is the command that your are sending to the server.</param>
        /// <param name="timeout"> This is the amount of time in milliseconds that this method will wait for data. If you set Timeout to a negative number then we dont wait for a response.</param>
        /// <param name="expectedResponse"> This is the expected string reponse from the server.</param>
        /// <returns> The return value is a string of the response from the server.</returns>
        public SocketResponse TransmitDataAndWaitForResponse(string command, List<string> expectedResponse, TimeSpan timeout)
        {
            string data = "";
            bool timedOut = false;
            _expectedResponse = expectedResponse;

            if (!_socket.Connected)
                throw new Exception("Socket is no longer connected." + Environment.NewLine);

            //_buffer = new byte[1024];
            //_socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
            TransmitString(command);

            _dataReceivedSignal = new EventWaitHandle(false, EventResetMode.ManualReset);

            int timeoutInMilliSeconds = Convert.ToInt32(timeout.TotalMilliseconds);

            //If you set Timeout to a negative number then we dont wait for a response.
            if (timeoutInMilliSeconds >= 0)
            {
                //If timeout is equal to 0 seconds then default to 10 seconds
                timeoutInMilliSeconds = timeoutInMilliSeconds == 0 ? 10000 : timeoutInMilliSeconds;
                //Will wait while data is returned.
                //true if the current instance receives a signal; otherwise, false.
                timedOut = !_dataReceivedSignal.WaitOne(timeoutInMilliSeconds);

                data = _receivedData;
                _receivedData = "";
            }

            return new SocketResponse
            {
                Data = data,
                TimeoutOccurred = timedOut
            };
        }

        /// <summary> This method sends data down your connected socket connection to the specified server.</summary>
        /// <param name="command"> This is the command that your are sending to the server.</param>
        /// <returns> The return value is a string of the response from the server.</returns>
        public void TransmitData(string command)
        {
            if (!_socket.Connected)
                throw new Exception("Socket is no longer connected." + Environment.NewLine);

            //_buffer = new byte[1024];
            //_socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
            TransmitString(command);
        }

        /// <summary> This method sends data down your connected socket connection to the specified server Synchronously.</summary>
        /// <param name="command"> This is the command that your are sending to the server.</param>
        /// <returns> The return value is a string of the response from the server.</returns>
        public SocketResponse TransmitDataSynchronously(string command)
        {
            if (!_socket.Connected)
                throw new Exception("Socket is no longer connected." + Environment.NewLine);

            byte[] byteData = _connInfo.Encoder.GetBytes(command);

            _socket.Send(byteData, byteData.Length, 0);
            int bytesReceived;
            do
            {
                System.Threading.Thread.Sleep(1000);
                bytesReceived = _socket.Available;
                if (bytesReceived > 0)
                {
                    bytesReceived = _socket.Receive(_buffer, _buffer.Length, 0);
                    _receivedData += _connInfo.Encoder.GetString(_buffer, 0, bytesReceived);
                }
            } while (bytesReceived > 0);

            return new SocketResponse
            {
                Data = _receivedData,
                TimeoutOccurred = false
            };
        }

        /// <summary> This method is if you want to send specific telenet options to the server.</summary>
        /// <param name="telnetCommand"> This is the TelnetCommand you want to send.</param>
        /// <param name="telnetOption"> This is the TelnetOption you want to send.</param>
        /// <param name="data"> This is data you want to send along.  If you do not want to send any data set as null.</param>
        public void TransmitTelnetCommand(TelnetCommands telnetCommand, TelnetOptions telnetOption, byte[] data)
        {
            if (!_socket.Connected)
                throw new Exception("Socket is no longer connected." + Environment.NewLine);

            var wholeRequestList = new List<byte> { (byte)TelnetCommands.IAC, (byte)telnetCommand, (byte)telnetOption};
            if (data != null)
            {
                if (data.Length > 0)
                    wholeRequestList.AddRange(data);
            }
            TransmitByteArray(wholeRequestList.ToArray());
        }

        /// <summary> This method closes and cleans up the socket.</summary>
        public void Close()
        {
            // Release the socket.
            if (_socket != null)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
        }

        /// <summary> Once connected to the server this method is the data you want to send to the server.</summary>
        /// <param name="data"> This is data you want to send to the server.</param>
        protected void TransmitString(string data)
        {
            byte[] byteData = _connInfo.Encoder.GetBytes(data);

            SocketError myError;
            // Begin sending the data to the remote device.
            _socket.Send(byteData, 0, byteData.Length, SocketFlags.None, out myError);
            if (myError != SocketError.Success)
                throw new Exception("SocketError: " + myError + Environment.NewLine);
        }

        /// <summary> Once connected to the server this method is the data you want to send to the server.</summary>
        /// <param name="data"> This is data you want to send to the server.</param>
        protected void TransmitByteArray(byte[] data)
        {
            SocketError myError;
            // Begin sending the data to the remote device.
            _socket.Send(data, 0, data.Length, SocketFlags.None, out myError);
            if (myError != SocketError.Success)
                throw new Exception("SocketError: " + myError + Environment.NewLine);
        }

        /// <summary> Once connected to the server this method keeps retrieving data from the socket.</summary>
        /// <param name="result"> This is the IAsyncResult that comes back from the aynsc call.</param>
        private void OnDataReceived(IAsyncResult result)
        {
            try
            {
                if (_socket.Connected)
                {
                    int receivedBytes = _socket.EndReceive(result);

                    if (receivedBytes > 0)
                    {
                        if (_connInfo.IsTelnet)
                            ProcessTelnetCommands(_buffer, 0, receivedBytes);
                        else
                        {
                            if (_connInfo.RemoveNonPrintableChars)
                                _receivedData += RemoveNonPrintableCharacters(_buffer, 0, receivedBytes);
                            else
                                _receivedData += _connInfo.Encoder.GetString(_buffer, 0, receivedBytes);
                        }

                        if (_receivedData.ContainsAny(_expectedResponse))
                        {
                            if (_dataReceivedSignal != null)
                                _dataReceivedSignal.Set();
                        }
                        else
                        {
                            //_socket.Poll();
                            if (_socket != null)
                            {
                                if (_socket.Connected)
                                {
                                    //_buffer = new byte[1024];
                                    _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
                                }
                            }
                        }
                    }

                    if (_socket.Connected)
                    {
                        //_buffer = new byte[1024];
                        _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.WriteLogEntry(new List<object> { ex.Message, ex.ToString() }, string.Format(MethodBase.GetCurrentMethod().Name + " in " + Assembly.GetExecutingAssembly().GetName().Name), LogLevelType.Error, ex);

                if (_socket.Connected)
                {
                    //_buffer = new byte[1024];
                    _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
                }

                //if ((ex is SocketException) && ((SocketException)ex).ErrorCode == 995)
                //{
                //    //in case of .NET1.1 on Win9x, EndReceive() changes the behavior. it throws SocketException with an error code 995. 
                //   // _buffer = new byte[1024];
                //    _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
                //}
                //else
                //    throw new Exception(ex.ToString());
            }
        }

        /// <summary> Once connected to the server this method process the telnet commands received from the server.</summary>
        /// <param name="buffer"> This is the byte buffer of all received data from the server.</param>
        /// <param name="offset"> This where you need to start reading your data from in the buffer.</param>
        /// <param name="receivedBytes"> This is the amount of data received from the last packet.</param>
        private void ProcessTelnetCommands(byte[] buffer, int offset, int receivedBytes)
        {
            // Commands that may need to be sent back to the sender in response to commands received.
            var sendCommandList = new List<byte[]>();

            //For example, if the client wants to identify the terminal type to the server, the following exchange might take place:

            //CLIENT		IAC	WILL	Terminal Type
            //SERVER		IAC	DO	Terminal Type
            //CLIENT		IAC	SB	Terminal Type	1	IAC	SE
            //SERVER		IAC	SB	Terminal Type	0	V	T	2	2	0	IAC	SE

            //The first exchange establishes that terminal type (option number 24) is handled, the server then inquiries of the client what value it wishes to associate with the terminal type.
            //The sequence SB,24,1 implies sub-option negotiation for option type 24, value required (1). The IAC,SE sequence indicates the end of this request.
            //The response IAC,SB,24,0,'V'... implies sub-option negotiation for option type 24, value supplied (0), the IAC,SE sequence indicates the end of the response (and the supplied value).
            //The encoding of the value is specific to the option but a sequence of characters, as shown above, is common.

            //GetIndexOfByte will get the first IAC.  Each Telnet Code must start with IAC. RFC 854 protocol.
            int iacPos = buffer.GetIndexOfByte(offset, receivedBytes, (byte)TelnetCommands.IAC);
            //If we find an IAC then we need to look for telnet codes. If not then just add the buffer to our response variable _receivedData.
            if (iacPos > -1)
            {
                //Loop through the whole buffer and collect telnet codes vs regular text.
                for (int count = offset; count < receivedBytes; count++)
                {
                    //You must remove all nulls or the text will not get display correctly.
                    if (buffer[count] == 0)
                        count++;

                    // Once we get to the IAC then we need to either remove the Telnet Codes or put them into a readable format.
                    if (count >= iacPos && iacPos > -1)
                    {
                        byte[] tempByteArray;
                        string interpretOption;

                        switch (buffer[count])
                        {
                            case (byte)TelnetCommands.SE:
                            case (byte)TelnetCommands.NOP:
                            case (byte)TelnetCommands.DM:
                            case (byte)TelnetCommands.IP:
                            case (byte)TelnetCommands.AO:
                            case (byte)TelnetCommands.AYT:
                            case (byte)TelnetCommands.EC:
                            case (byte)TelnetCommands.EL:
                            case (byte)TelnetCommands.GA:
                            case (byte)TelnetCommands.IAC:
                                if (_connInfo.ShowTelentCodes)
                                    _receivedData += "{Rcv-IAC-";
                                break;
                            case (byte)TelnetCommands.WILL:
                                interpretOption = InterpretTelnetOption(buffer[++count]);
                                if (_connInfo.ShowTelentCodes)
                                    _receivedData += "WILL-" + interpretOption + "}" + Environment.NewLine;

                                // Decide if we want to accept servers requests.
                                tempByteArray = HandleWILLRequest(buffer[count]);
                                if (tempByteArray != null)
                                    sendCommandList.Add(tempByteArray);

                                break;
                            case (byte)TelnetCommands.WONT:
                                interpretOption = InterpretTelnetOption(buffer[++count]);
                                if (_connInfo.ShowTelentCodes)
                                    _receivedData += "WONT-" + interpretOption + "}" + Environment.NewLine;

                                // Decide if we want to accept servers requests.
                                tempByteArray = HandleWONTRequest(buffer[count]);
                                if (tempByteArray != null)
                                    sendCommandList.Add(tempByteArray);

                                break;
                            case (byte)TelnetCommands.DO:
                                interpretOption = InterpretTelnetOption(buffer[++count]);
                                if (_connInfo.ShowTelentCodes)
                                    _receivedData += "DO-" + interpretOption + "}" + Environment.NewLine;

                                // Decide if we want to accept servers requests.
                                tempByteArray = HandleDORequest(buffer[count]);
                                if (tempByteArray != null)
                                    sendCommandList.Add(tempByteArray);

                                break;
                            case (byte)TelnetCommands.DONT:
                                interpretOption = InterpretTelnetOption(buffer[++count]);
                                if (_connInfo.ShowTelentCodes)
                                    _receivedData += "DONT-" + interpretOption + "}" + Environment.NewLine;

                                // Decide if we want to accept servers requests.
                                tempByteArray = HandleDONTRequest(buffer[count]);
                                if (tempByteArray != null)
                                    sendCommandList.Add(tempByteArray);

                                break;
                            case (byte)TelnetCommands.SB:
                                // Subnegotiation
                                interpretOption = InterpretTelnetOption(buffer[++count]);

                                // Look for the next {IAC}{SE}, everything between is part of the subnegotiation.
                                // The + 2 is so it moves past the IAC and the SE.
                                int iacEndPos = buffer.GetIndexOfByte(count, buffer.Length - count, (byte)TelnetCommands.IAC) + 2;
                                if (iacEndPos > -1)
                                {
                                    var tempBuffer = new List<byte>();
                                    for (int innerCount = count; innerCount <= iacEndPos; innerCount++)
                                    {
                                        tempBuffer.Add(buffer[count]);
                                    }

                                    if (_connInfo.ShowTelentCodes)
                                        _receivedData += "SB-" + interpretOption + "-" + _connInfo.Encoder.GetString(tempBuffer.ToArray(), 0, tempBuffer.Count) + "-IAC-SE}" + Environment.NewLine;

                                    tempByteArray = HandleSubNegotiation(tempBuffer.ToArray(), buffer[count]);
                                    if (tempByteArray != null)
                                        sendCommandList.Add(tempByteArray);

                                    count = iacEndPos;
                                }
                                else
                                {
                                    // Didn't find the end of sequence so just abort the {IAC}{SE}.
                                    if (_connInfo.ShowTelentCodes)
                                        _receivedData += "SB-" + interpretOption + "-" + buffer[++count] + "-could not determine end sequence.}";

                                    //Send wont command now since, we want to abort.
                                    tempByteArray = new[] { (byte)TelnetCommands.IAC, (byte)TelnetCommands.WONT, buffer[count] };
                                    sendCommandList.Add(tempByteArray);
                                }

                                break;
                            default:
                                throw new Exception("UNKNOWN TelnetCommand \"" + (char)buffer[count] + "\". Byte Value: " + buffer[count]);
                        }

                        //If there are commands built up then we need to send the requests.
                        if (sendCommandList.Count > 0)
                        {
                            // We need to respond to the sender.
                            foreach (byte[] myCommand in sendCommandList)
                            {
                                TransmitByteArray(myCommand);
                            }
                            sendCommandList.Clear();
                        }

                        //Get the next postion of the IAC.
                        iacPos = buffer.GetIndexOfByte(count, buffer.Length - count, (byte)TelnetCommands.IAC);
                    }
                    else
                    {
                        if (_connInfo.RemoveNonPrintableChars)
                        {
                            _receivedData += RemoveNonPrintableCharacters(new[] { buffer[count] }, 0, 1);
                        }
                        else
                        {
                            //Everything else add the chars.
                            _receivedData += _connInfo.Encoder.GetString(new[] { buffer[count] });
                        }
                    }
                }
            }
            else
            {
                if (_connInfo.RemoveNonPrintableChars)
                    _receivedData += RemoveNonPrintableCharacters(buffer, offset, receivedBytes);
                else
                    _receivedData += _connInfo.Encoder.GetString(buffer, offset, receivedBytes);
            }
        }

        /// <summary> Once connected to the server this is the logic needed to process the WILL request from the server.</summary>
        /// <param name="myTelnetOption"> This is specific Telnet Option received from the server.</param>
        protected virtual byte[] HandleWILLRequest(byte myTelnetOption)
        {
            #warning "May need to implemented this at some point."
            // For every command we're asked to process, tell the sender that we will not process it.
            var myTelnetCommand = TelnetCommands.WONT;

            if (_connInfo.ShowTelentCodes)
                _receivedData += "{Snd-IAC-" + myTelnetCommand + "-" + InterpretTelnetOption(myTelnetOption) + "}" + Environment.NewLine;

            //Creating our reply buffer.  First IAC, then WONT, then the option that server sent.
            return new[] { (byte)TelnetCommands.IAC, (byte)myTelnetCommand, myTelnetOption };
        }

        /// <summary> Once connected to the server this is the logic needed to process the WONT request from the server.</summary>
        /// <param name="myTelnetOption"> This is specific Telnet Option received from the server.</param>
        protected virtual byte[] HandleWONTRequest(byte myTelnetOption)
        {
            #warning "May need to implemented this at some point."
            // For every command we're asked to process, tell the sender that we will not process it.
            var myTelnetCommand = TelnetCommands.WONT;

            if (_connInfo.ShowTelentCodes)
                _receivedData += "{Snd-IAC-" + myTelnetCommand + "-" + InterpretTelnetOption(myTelnetOption) + "}" + Environment.NewLine;

            //Creating our reply buffer.  First IAC, then WONT, then the option that server sent.
            return new[] { (byte)TelnetCommands.IAC, (byte)myTelnetCommand, myTelnetOption };
        }

        /// <summary> Once connected to the server this is the logic needed to process the DO request from the server.</summary>
        /// <param name="myTelnetOption"> This is specific Telnet Option received from the server.</param>
        protected virtual byte[] HandleDORequest(byte myTelnetOption)
        {
            #warning "May need to implement this more at some point."
            // For every command we're asked to process, tell the sender that we will not process it.
            TelnetCommands myTelnetCommand = TelnetCommands.WONT;

            if (_connInfo.ShowTelentCodes)
                _receivedData += "{Snd-IAC-" + myTelnetCommand + "-" + InterpretTelnetOption(myTelnetOption) + "}" + Environment.NewLine;

            //Creating our reply buffer.  First IAC, then WONT, then the option that server sent.
            return new[] { (byte)TelnetCommands.IAC, (byte)myTelnetCommand, myTelnetOption };
        }

        /// <summary> Once connected to the server this is the logic needed to process the DONT request from the server.</summary>
        /// <param name="myTelnetOption"> This is specific Telnet Option received from the server.</param>
        protected virtual byte[] HandleDONTRequest(byte myTelnetOption)
        {
            #warning "May need to implement this more at some point."
            // For every command we're asked to process, tell the sender that we will not process it.
            var myTelnetCommand = TelnetCommands.WONT;

            if (_connInfo.ShowTelentCodes)
                _receivedData += "{Snd-IAC-" + myTelnetCommand + "-" + InterpretTelnetOption(myTelnetOption) + "}" + Environment.NewLine;
            
            //Creating our reply buffer.  First IAC, then WONT, then the option that server sent.
            return new[] { (byte)TelnetCommands.IAC, (byte)myTelnetCommand, myTelnetOption };
        }

        /// <summary> Once connected to the server this is the logic neded to process the SubNegotiation request from the server.</summary>
        /// <param name="negSeq"> This is all the Sub Negotiation data received from the server request.</param>
        /// <param name="myTelnetOption"> This is specific Telnet Option received from the server.</param>
        protected virtual byte[] HandleSubNegotiation(byte[] negSeq, byte myTelnetOption)
        {
            //If it is a 1 then this is a request otherwise it is a response and we do not need to respond.
            if (negSeq[0] != 1)
                return null;

            if (_connInfo.ShowTelentCodes)
                _receivedData += "{Snd-IAC-SB-" + InterpretTelnetOption(myTelnetOption) + "}" + Environment.NewLine;

            #warning "May need to implement this more at some point."
            //Here is where you would put logic about how you want to respond.
            //This example is responding that we want to use VT100 compliant terminaltype.
            //byte[] Request = _encodingSet.GetBytes("VT100");
            
            byte[] request = {};
            var wholeRequestList = new List<byte> {(byte)TelnetCommands.IAC, (byte)TelnetCommands.SB, 0};
            wholeRequestList.AddRange(request);
            wholeRequestList.Add((byte)TelnetCommands.IAC);
            wholeRequestList.Add((byte)TelnetCommands.SE);
            return wholeRequestList.ToArray();
        }

        /// <summary> This takes a byte array and will return a string with all nonprintable characters removed or replaced with string representations except for CR LF. </summary>
        /// <param name="buffer"> This is the buffer with the data received.</param>
        /// <param name="offset"> This is where to start reading your data from in the buffer. </param>
        /// <param name="length"> This is the length for which to read the data out of the buffer. </param>
        /// <returns> The return value is a string with non printable characters removed. </returns>
        private string RemoveNonPrintableCharacters(byte[] buffer, int offset, int length)
        {
            string response = string.Empty;

            //Loop through the whole buffer.
            for (int count = offset; count < length; count++)
            {
                var myNonPrintableChar = (NonPrintableChars)buffer[count];

                //Make sure that we do not go outside the bounds of the array buffer.
                int num = 0;
                if (count + 1 < buffer.Length)
                    num = 1;

                //If it is a carriage and then line feed or line feed then carriage return then put in the newline.
                if (buffer[count] == 13 && buffer[count + num] == 10 || buffer[count] == 10 && buffer[count + num] == 13)
                {
                    response += Environment.NewLine;
                }
                //Lower non printable chars.
                else if (buffer[count] < 32)
                {
                    if (_connInfo.ReplaceNonPrintableCharacters)
                        response += "{" + Enum.GetName(typeof(NonPrintableChars), myNonPrintableChar) + " Byte: " + buffer[count] + "}" + Environment.NewLine;
                }
                //Extended ASCII char set is above 127.
                else if (buffer[count] > 127)
                {
                    if (_connInfo.ReplaceNonPrintableCharacters)
                        response += "(Possibly non-printable char: " + buffer[count] + ")";
                }
                else
                {
                    //Everything else add the chars.
                    response += _connInfo.Encoder.GetString(new[] { buffer[count] });
                }
            }

            return response;
        }

        /// <summary> This method translates the Enum to readable text.</summary>
        /// <param name="myByte"> This is the specific byte.</param>
        private static string InterpretTelnetOption(byte myByte)
        {
            switch (myByte)
            {
                case (byte)TelnetOptions.E:
                    return "Echo";
                case (byte)TelnetOptions.R:
                    return "Reconnection";
                case (byte)TelnetOptions.SGA:
                    return "SuppressGoAhead";
                case (byte)TelnetOptions.AMSN:
                    return "ApproxMessageSizeNegotiation";
                case (byte)TelnetOptions.S:
                    return "Status";
                case (byte)TelnetOptions.TM:
                    return "TimingMark";
                case (byte)TelnetOptions.RCT:
                    return "RemoteControlledTransAndEcho";
                case (byte)TelnetOptions.OLW:
                    return "OutputLineWidth";
                case (byte)TelnetOptions.OPS:
                    return "OutputPageSize";
                case (byte)TelnetOptions.OCRD:
                    return "OutputCarriageReturnDisposition";
                case (byte)TelnetOptions.OHTS:
                    return "OutputHorizontalTabStops";
                case (byte)TelnetOptions.OHTD:
                    return "OutputHorizontalTabDisposition";
                case (byte)TelnetOptions.OFD:
                    return "OutputFormfeedDisposition";
                case (byte)TelnetOptions.OVT:
                    return "OutputVerticalTabstops";
                case (byte)TelnetOptions.OVTD:
                    return "OutputVerticalTabDisposition ";
                case (byte)TelnetOptions.OLD:
                    return "OutputLinefeedDisposition ";
                case (byte)TelnetOptions.EA:
                    return "ExtendedASCII ";
                case (byte)TelnetOptions.LO:
                    return "Logout";
                case (byte)TelnetOptions.BM:
                    return "ByteMacro";
                case (byte)TelnetOptions.DET:
                    return "DataEntryTerminal";
                case (byte)TelnetOptions.SD:
                    return "SUPDUP";
                case (byte)TelnetOptions.SDO:
                    return "SUPDUPOutput";
                case (byte)TelnetOptions.SL:
                    return "SendLocation ";
                case (byte)TelnetOptions.TT:
                    return "TerminalType";
                case (byte)TelnetOptions.EOR:
                    return "EndOfRecord";
                case (byte)TelnetOptions.TUI:
                    return "TACACSUserIdentification";
                case (byte)TelnetOptions.OM:
                    return "OutputMarking";
                case (byte)TelnetOptions.TLN:
                    return "TerminalLocationNumber";
                case (byte)TelnetOptions.TR:
                    return "Telnet3270Regime";
                case (byte)TelnetOptions.PAD:
                    return "X.3PAD";
                case (byte)TelnetOptions.WS:
                    return "WindowSize";
                case (byte)TelnetOptions.TS:
                    return "TerminalSpeed";
                case (byte)TelnetOptions.RFC:
                    return "RemoteFlowControl";
                case (byte)TelnetOptions.LM:
                    return "LineMode";
                case (byte)TelnetOptions.XDL:
                    return "XDisplayLocation";
                case (byte)TelnetOptions.EV:
                    return "EnvironmentVariables";
                case (byte)TelnetOptions.AO:
                    return "AuthenticationOption";
                case (byte)TelnetOptions.EO:
                    return "EncryptionOption";
                case (byte)TelnetOptions.EOL:
                    return "ExtendedOptionsList";
                default:
                    return "UNKNOWN";
            }
        }
    }

    internal static class ExtensionClass
    {
        public static int GetIndexOfByte(this byte[] buffer, int startPos,  int length, byte searchByte)
        {
            for (int count = startPos; count < length; count++)
            {
                if (buffer[count] == searchByte)
                    return count;
            }

            return -1;
        }
    }
}
