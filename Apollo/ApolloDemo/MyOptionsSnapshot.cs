using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace ApolloDemo {
    public class MyOptionsSnapshot<T> : IOptionsSnapshot<T>
        where T : class, new() {
        private IConfiguration _configuration;

        public MyOptionsSnapshot(IConfiguration configuration) {
            _configuration = configuration;
        }

        public T Value => JsonSerializer.Deserialize<T>(_configuration["EventBus"]);

        public T Get(string name) {
            return JsonSerializer.Deserialize<T>(_configuration[name]);
        }
    }
}
