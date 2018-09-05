﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Infrastructure.Idempotency {
    /// <summary>
    /// 客户端请求 
    /// </summary>
    public class ClientRequest {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }
}
