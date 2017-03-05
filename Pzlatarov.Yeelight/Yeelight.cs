using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using AwesomeSockets.Sockets;
using Buffer = AwesomeSockets.Buffers.Buffer;
using Newtonsoft.Json;
using Pzlatarov.Yeelight.Exceptions;

namespace Pzlatarov.Yeelight
{
    public class Yeelight
    {
        private string _location;
        private ModelType _model;
        private readonly int _firmwareVersion;
        private PowerStateType _powerState;
        private readonly ColorModeType _colorMode;
        private int _brightness;
        private readonly int _colorTemperature;
        private int _rgbColor;
        private readonly int _saturation;
        private readonly int _hue;
        private readonly string _name = "";
        private string[] _features;

        public Yeelight()
        {
            throw new NotImplementedException();
        }

        public Yeelight(string id, string location, ModelType model, int firmwareVersion, PowerStateType powerState, ColorModeType colorMode, int brightness, int colorTemperature, int rgbColor, int saturation, int hue, string name, string[] features = null)
        {
            Id = id;
            _location = location;
            _model = model;
            _firmwareVersion = firmwareVersion;
            _powerState = powerState;
            _colorMode = colorMode;
            _brightness = brightness;
            _colorTemperature = colorTemperature;
            _rgbColor = rgbColor;
            _saturation = saturation;
            _hue = hue;
            _name = name;
            _features = features;
        }

        public enum ModelType
        {
            Color = 1, White = 0, Strip = 2
        }

        public enum PowerStateType
        {
            On = 1, Off = 0
        }

        public enum ColorModeType
        {
            Color = 1,
            ColorTemperature = 2,
            Hsv = 3
        }

        public string Id { get; }

        public string Location
        {
            get { return _location; }
        }

        public ModelType Model
        {
            get { return _model; }
        }

        public int FirmwareVersion
        {
            get { return _firmwareVersion; }
        }

        public PowerStateType PowerState
        {
            get { return _powerState; }
            set
            {
               var methodResult = RunMethod(1, "set_power", new object[] {value==PowerStateType.On?"on":"off","smooth",500});
                if (methodResult?.result != null && methodResult.result[0] == "ok") _powerState = value;
            }
        }

        public ColorModeType ColorMode
        {
            get { return _colorMode; }

        }

        public int Brightness
        {
            get { return _brightness; }
            set
            {
                var methodResult = RunMethod(1, "set_bright", new object[] {value, "smooth", 500 });
                if (methodResult?.result != null && methodResult.result[0] == "ok") _brightness = value;
            }
        }

        public int ColorTemperature
        {
            get { return _colorTemperature; }
        }

        public int RgbColor
        {
            get { return _rgbColor; }
            set {
                var methodResult = RunMethod(1, "set_rgb", new object[] { value, "smooth", 500 });
                if (methodResult?.result != null && methodResult.result[0] == "ok") _rgbColor = value;
            }
        }

        public int Saturation
        {
            get { return _saturation; }
        }

        public int Hue
        {
            get { return _hue; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string[] Features
        {
            get { return _features;}
        }

        public MethodResult RunMethod(int id, string method, object[] parameters)
        {
            if (!_features.Contains(method))
            {
                throw new FeatureNotSupportedException();
            }
            //todo finish
            var encoding = Encoding.ASCII;
            var json = MethodRequest.Create(id, method, parameters).Serialize()+"\r\n";
            var msg = encoding.GetBytes(json);
            var msgBuffer = Buffer.New(msg.Length);
            var receiveBuffer = Buffer.New(1024);
            Buffer.Add(msgBuffer, msg);
            Buffer.FinalizeBuffer(msgBuffer);
            var clientSocket = AweSock.TcpConnect(GetIp(),GetPort());
            clientSocket.GetSocket().ReceiveTimeout = 5000;
            AweSock.SendMessage(clientSocket,msgBuffer);
            MethodResult returnResult;
            try
            {
                AweSock.ReceiveMessage(clientSocket, receiveBuffer);
                Buffer.FinalizeBuffer(receiveBuffer);
                returnResult =
                    JsonConvert.DeserializeObject<MethodResult>(encoding.GetString(Buffer.GetBuffer(receiveBuffer)).Replace("\0", ""));
            }
            catch (SocketException)
            {
                //todo return error
                returnResult = null;
                return returnResult;
            }
            finally
            {
                AweSock.CloseSock(clientSocket);
                clientSocket.Close();
            }
            return returnResult;
        }

        private string GetIp()
        {
            var parts = Location.Split(':');
            if (parts.Length == 3)
            {
                return parts[1].Replace("//","");
            }

            return null;
        }
        private int GetPort()
        {
            var parts = Location.Split(':');
            if (parts.Length == 3)
            {
                return int.Parse(parts[2]);
            }

            return 0;
        }
    }
}
