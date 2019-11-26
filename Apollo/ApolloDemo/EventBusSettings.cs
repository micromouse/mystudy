using System.Collections.Generic;

namespace ApolloDemo {
    public class EventBus {
        public string Message { get; set; }
        public IList<DetailSetting> Details { get; set; }
    }

    public class DetailSetting {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
