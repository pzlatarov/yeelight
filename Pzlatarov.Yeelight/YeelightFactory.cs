using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pzlatarov.Yeelight
{
    public class YeelightFactory
    {
        public static Yeelight BuildYeelight(string properties)
        {
            string id = "";
            string location = "";
            Yeelight.ModelType model = Yeelight.ModelType.Color;
            int firmwareVersion = 0;
            Yeelight.PowerStateType powerState = Yeelight.PowerStateType.Off;
            Yeelight.ColorModeType colorMode = Yeelight.ColorModeType.ColorTemperature;
            int brightness = 0;
            int colorTemperature = 0;
            int rgbColor = 0;
            int saturation = 0;
            int hue = 0;
            string _name = "";
            string[] features = { };

            var lines = properties.Split('\n');
            foreach (var line in lines)
            {
                var kv = line.Split(new[] { ':' }, 2);
                switch (kv[0])
                {
                    default:
                        break;

                    case "Location":
                        location = kv[1].Trim();
                        break;

                    case "id":
                        id = kv[1].Trim();
                        break;

                    case "model":
                        switch (kv[1].Trim())
                        {
                            case "color":
                                model = Yeelight.ModelType.Color;
                                break;

                            case "mono":
                                model = Yeelight.ModelType.White;
                                break;

                            case "strip":
                                model = Yeelight.ModelType.Strip;
                                break;
                        }
                        break;

                    case "fw_ver":
                        firmwareVersion = int.Parse(kv[1].Trim());
                        break;

                    case "support":
                        features = kv[1].Trim().Split(' ');
                        break;

                    case "power":
                        switch (kv[1].Trim())
                        {
                            case "on":
                                powerState = Yeelight.PowerStateType.On;
                                break;

                            case "off":
                                powerState = Yeelight.PowerStateType.Off;
                                break;
                        }
                        break;

                    case "bright":
                        brightness = int.Parse(kv[1].Trim());
                        break;

                    case "color_mode":
                        colorMode = (Yeelight.ColorModeType)int.Parse(kv[1].Trim());
                        break;

                    case "ct":
                        colorTemperature = int.Parse(kv[1].Trim());
                        break;

                    case "rgb":
                        rgbColor = int.Parse(kv[1].Trim());
                        break;

                    case "hue":
                        hue = int.Parse(kv[1].Trim());
                        break;

                    case "sat":
                        saturation = int.Parse(kv[1].Trim());
                        break;

                    case "name":
                        if (kv.Length > 1 && !string.IsNullOrEmpty(kv[1]))
                        {
                            _name = kv[1].Trim();
                        }
                        break;




                }
            }

            return new Yeelight(id, location, model, firmwareVersion, powerState, colorMode, brightness, colorTemperature, rgbColor, saturation, hue, _name,features);
        }
    }
}
