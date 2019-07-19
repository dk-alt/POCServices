using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCEntities
{
    
    
        public class RestResponse<T>
        {
            public Header Header { get; set; }
            public T ResponseData { get; set; }
        }

        public class Header
        {
            public string StatusCode { get; set; }
            public string Message { get; set; }
        }
    
}
