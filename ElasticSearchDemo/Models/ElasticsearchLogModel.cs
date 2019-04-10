using Nest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchDemo.Models {
    /// <summary>
    /// Elasticsearch日志模型
    /// </summary>
    public class ElasticsearchLogModel {
        [Date(Name ="@timestamp")]
        public DateTime Timestamp { get; set; }
        [Text(Name="level")]
        public string Level { get; set; }
        [Text(Name= "messageTemplate")]
        public string MessageTemplate { get; set; }
        [Text(Name= "message")]
        public string Message { get; set; }
        [Object(Name="fields")]
        public JObject Fields { get; set; }
        [Text(Name="Application")]
        public string Application { get; set; }
    }
}
