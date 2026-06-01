using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cache
{
    public static class HashSignSessionStore
    {
        public static ConcurrentDictionary<string, SignSession> Sessions
            = new ConcurrentDictionary<string, SignSession>();
    }
}
