using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pzlatarov.Yeelight
{
    public class MethodRequest
    {
        public int id;
        public string method;
        public object[] parametersTitle;

        public static MethodRequest Create(int id, string method, object[] parameters)
        {
            return new MethodRequest()
            {
                id = id,
                method = method,
                parametersTitle = parameters
            };
        }

        public static string Serialize(MethodRequest request)
        {
            return JsonConvert.SerializeObject(request).Replace("parametersTitle", "params");
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this).Replace("parametersTitle", "params");
        }
    }
}
