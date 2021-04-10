using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOKSTORE.DOMAIN.Common
{
   public  class ResultModel
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public bool Result { get; set; }
        public Object Data { get; set; }
    }
}
