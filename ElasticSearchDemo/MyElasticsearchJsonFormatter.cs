using Elasticsearch.Net;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchDemo
{
    /// <summary>
    /// 我的Elasticsearch日志格式化器
    /// </summary>
    public class MyElasticsearchJsonFormatter : ElasticsearchJsonFormatter
    {
        private readonly bool _inlineFields;
        private readonly IList<string> _rootFields;

        /// <summary>
        /// 初始化我的Elasticsearch日志格式化器
        /// </summary>
        /// <param name="formatProvider">格式化提供者</param>
        /// <param name="closingDelimiter">A string that will be written after each log event is formatted. If null, System.Environment.NewLine will be used. Ignored if omitEnclosingObject is true.</param>
        /// <param name="serializer">序列化器</param>
        /// <param name="inlineFields">When set to true values will be written at the root of the json document</param>
        /// <param name="rootFields">需要显示在根文档下的字段</param>
        public MyElasticsearchJsonFormatter(IFormatProvider formatProvider = null,
            string closingDelimiter = null, IElasticsearchSerializer serializer = null,
            bool inlineFields = false, IList<string> rootFields = null) :
            base(formatProvider: formatProvider, closingDelimiter: closingDelimiter, serializer: serializer, inlineFields: inlineFields)
        {
            _inlineFields = inlineFields;
            _rootFields = rootFields ?? throw new ArgumentNullException(nameof(rootFields));
        }

        /// <summary>
        /// 写自定义属性
        /// </summary>
        /// <param name="properties">属性集合</param>
        /// <param name="output">TextWriter</param>
        protected override void WriteProperties(IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
        {
            //内联属性
            var innerProperties = properties.Where(x => !_inlineFields && !_rootFields.Contains(x.Key));
            if (innerProperties.Any())
            {
                output.Write(",\"{0}\":{{", "fields");
                WritePropertiesValues(innerProperties, output);
                output.Write("}");
            }

            //根属性
            var rootProperties = properties.Where(x => _inlineFields || _rootFields.Contains(x.Key));
            if (rootProperties.Any())
            {
                output.Write(",");
                WritePropertiesValues(rootProperties, output);
            }
        }

        /// <summary>
        /// 写属性值
        /// </summary>
        /// <param name="properties">属性集合</param>
        /// <param name="output">TextWriter</param>
        private void WritePropertiesValues(IEnumerable<KeyValuePair<string, LogEventPropertyValue>> properties, TextWriter output)
        {
            var precedingDelimiter = "";
            foreach (var property in properties)
            {
                WriteJsonProperty(property.Key, property.Value, ref precedingDelimiter, output);
            }
        }
    }
}
