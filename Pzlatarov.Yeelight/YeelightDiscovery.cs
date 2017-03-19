using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using AwesomeSockets.Sockets;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace Pzlatarov.Yeelight
{
    public class YeelightDiscovery
    {
        /// <summary>
        /// Discovers Yeelight products within the local network
        /// </summary>
        public static List<Yeelight> Discover()
        {
            var encoding = Encoding.ASCII;
            var msg =  encoding.GetBytes("M-SEARCH * HTTP/1.1\r\nHOST: 239.255.255.250:1982\r\nMAN: \"ssdp:discover\"\r\nST: wifi_bulb");
            var msgBuffer = Buffer.New(msg.Length);
            var receiveBuffer = Buffer.New(1024);
            Buffer.Add(msgBuffer,msg);
            Buffer.FinalizeBuffer(msgBuffer);
            var clientSocket = AweSock.UdpConnect(59599);
            clientSocket.GetSocket().ReceiveTimeout = 5000;
            AweSock.SendMessage(clientSocket, "239.255.255.250", 1982, msgBuffer);
            string globalBuffer = "";
            while (true)
            {
                try
                {
                    AweSock.ReceiveMessage(clientSocket, receiveBuffer);
                    Buffer.FinalizeBuffer(receiveBuffer);
                     globalBuffer += encoding.GetString(Buffer.GetBuffer(receiveBuffer)).Trim().Replace("\0","");
                }

                catch (SocketException e)
                {
                    break;
                }
            }

            List<Yeelight> discoveredYeelights = new List<Yeelight>();

            var properties = globalBuffer.Split(new [] { "HTTP/1.1 200 OK" },StringSplitOptions.None);

            if (!string.IsNullOrEmpty(globalBuffer)) { 
            foreach (var property in properties)
            {
                if (string.IsNullOrEmpty(property))
                {
                    continue;
                }

                var bulb = YeelightFactory.BuildYeelight(property);
                var linq = from Yeelight light in discoveredYeelights where light.Id == bulb.Id select light;
                if (!linq.Any())
                {
                    discoveredYeelights.Add(bulb);
                }

            }
            }
            AweSock.CloseSock(clientSocket);
            return discoveredYeelights;
        }
    }
}
