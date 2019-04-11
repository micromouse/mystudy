using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ElasticSearchDemo.Models;
using Nest;
using Nest.JsonNetSerializer;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;

namespace ElasticSearchDemo.Controllers {
    /// <summary>
    /// Home控制器
    /// </summary>
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// 初始化Home控制器
        /// </summary>
        /// <param name="logger">日志</param>
        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>action结果</returns>
        public IActionResult Index() {
            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var settings = new ConnectionSettings(pool, JsonNetSerializer.Default)
                .DefaultMappingFor<ElasticsearchLogModel>(m => m.IndexName("lylog-*").TypeName("log"));

            var client = new ElasticClient(settings);
            /*
            var search = new SearchRequest<ElasticsearchLogModel>();
            search.Query = new MatchQuery { Field = "level", Query = "Information" };
            search.Query = search.Query && new DateRangeQuery { Field = "@timestamp", GreaterThanOrEqualTo = DateTime.Parse("2019-04-10") };
            var x = client.Search<ElasticsearchLogModel>(search);
            */

            var resut = client.Search<ElasticsearchLogModel>(s => s
                .From(0)
                .Size(10)
                .Query(q => q
                    .Bool(b=>b
                        .Must(mu=>mu.Match(mq => mq.Field(f => f.Level).Query("Information")),
                            mu=>mu.Match(mq=>mq.Field(f=>f.Application).Query("ElasticSearchDemo.API4")))
                        .Filter(fi=>fi
                            .DateRange(r => r
                                .Field(f => f.Timestamp)
                                .GreaterThanOrEquals(DateTime.Parse("2019-04-09"))
                                .LessThan(DateTime.Parse("2019-04-11"))))
                    )
                ));


            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
