using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core {
    public class CacheOptions {
        public TimeSpan SlidingExpiration { get; set; }

        public TimeSpan AbsoluteExpiration { get; set; }
    }
}
