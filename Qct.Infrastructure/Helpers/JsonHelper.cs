using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Qct.Infrastructure.Helpers
{
    public static class JsonHelper
    {
        public static StreamWriter ToJsonStream<T>(Stream stream, T obj)
        {
            JsonSerializer serializer = JsonSerializer.Create();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            StreamWriter sw = new StreamWriter(stream);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
            }
            return sw;
        }

        public static T ToObjectFromStream<T>(Stream stream)
        {
            using (StreamReader sw = new StreamReader(stream))
            {
                JsonTextReader reader = new JsonTextReader(sw);

                JsonSerializer ser = JsonSerializer.Create();
                var result = ser.Deserialize<T>(reader);
                reader.Close();
                return result;
            }
        }
        public static T ToObject<T>(this string sJasonData)
        {
            JsonSerializerSettings jsonSs = new JsonSerializerSettings();
            var timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            jsonSs.Converters.Add(timeConverter);
            jsonSs.Converters.Add(new DataTableConverter());
            try
            {
                return JsonConvert.DeserializeObject<T>(sJasonData, jsonSs);
            }
            catch { return default(T); }
        }

        public static object ToObject(this string sJasonData, Type type)
        {
            JsonSerializerSettings jsonSs = new JsonSerializerSettings();
            var timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            jsonSs.Converters.Add(timeConverter);
            jsonSs.Converters.Add(new DataTableConverter());
            try
            {
                return JsonConvert.DeserializeObject(sJasonData, type, jsonSs);
            }
            catch { return null; }
        }

        public static String ToJson<T>(this T obj)
        {
            JsonSerializerSettings jsonSs = new JsonSerializerSettings();
            var timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            jsonSs.Converters.Add(timeConverter);
            jsonSs.Converters.Add(new DataTableConverter());
            //jsonSs.Converters.Add(new JavaScriptDateTimeConverter());
            //jsonSs.Culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            //jsonSs.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            //jsonSs.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            string json = JsonConvert.SerializeObject(obj, Formatting.None, jsonSs);
            return json;
        }

        public static String ToJsonTime<T>(this T obj)
        {
            JsonSerializerSettings jsonSs = new JsonSerializerSettings();
            var timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            jsonSs.Converters.Add(timeConverter);
            jsonSs.Converters.Add(new DataTableConverter());
            string json = JsonConvert.SerializeObject(obj, Formatting.None, jsonSs);
            return json;
        }
        public static string Property(this JObject obj, string name, bool IgnoreCase)
        {
            var result="";
            if (obj == null) return result;
            foreach(var prop in obj.Properties())
            {
                if(string.Equals(prop.Name,name,StringComparison.CurrentCultureIgnoreCase))
                   result= prop.Value.ToString();
            }
            return result;
        }
    }
}
