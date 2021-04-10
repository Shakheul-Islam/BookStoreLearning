using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOKSTORE.DOMAIN.Common
{
  public  class BaseEntity
    {
        public bool Deleted { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get;set; }
    }
}
