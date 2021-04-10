﻿using BOOKSTORE.DOMAIN.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOKSTORE.DOMAIN.Model
{
   public class Book :BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
