using Elasticsearch.Net;
using System;

namespace ElasticSearchConsoleApp {
    /// <summary>
    /// ElasticSearch示例
    /// </summary>
    class Program {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">参数</param>
        static void Main(string[] args) {
            var settings = new ConnectionConfiguration(new Uri("http://localhost:9200"))
                .RequestTimeout(TimeSpan.FromMinutes(2));
            var client = new ElasticLowLevelClient(settings);

            //新增一个人
            /*
            var person = new Person {
                FirstName = "Martign",
                LastName = "Laarman"
            };
            var indexResponse = client.Index<BytesResponse>("people", "person", "1", PostData.Serializable(person));
            var responseBytes = indexResponse.Body;
            */

            //批量新增
            var persons = new object[] {
                new { index = new { _index = "people", _type = "person", _id = "2" } },
                new { FirstName = "Russ", LastName = "Cam" },
                new { index = new { _index = "people", _type = "person", _id = "3" } },
                new { FirstName = "Rose", LastName = "Cam" },
                new { index = new { _index = "people", _type = "person", _id = "4" } },
                new { FirstName = "Malilian", LastName = "Menluo" }
            };
            var bulkResponse = client.Bulk<StringResponse>(PostData.MultiJson(persons));
            var responseStream = bulkResponse.Body;
        }
    }
}
