using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.Json;

namespace ApolloDemo {
    public class EventBusSetting {
        public string Message { get; set; }
        public IList<EventBusDetailSetting> Details { get; set; }
    }

    public class EventBusDetailSetting {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public static class StringExtensions {
        public static T ToDeserializer<T>(this string s) {
            return JsonSerializer.Deserialize<T>(s);
        }
    }

    public static class IConfigurationExtensions {
        /// <summary>
        /// 使用IConfiguration[key]获得值并反序列化到类型T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetFromIndexer<T>(this IConfiguration configuration, string key) {
            return configuration[key].ToDeserializer<T>();
        }
    }
}
