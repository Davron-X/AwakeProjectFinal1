
using System.Text.Json;

namespace AwakeProject.Utility
{
    public static class SessionExtent
    {
        public static void Set<T>(this ISession session,T value,string key)
        {
            string result = JsonSerializer.Serialize(value);
            session.SetString(key, result);
        }
        public static T? Get<T>(this ISession session,string key)
        {
            string value = session.GetString(key);            
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value); 
        }
    }
}
