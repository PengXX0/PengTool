using Newtonsoft.Json;

namespace DotNet.Utilities
{
    //JSON转换类
    public static class ConvertJson
    {
        public static string ToJsonString<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToDataFromJson<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }
    }
}