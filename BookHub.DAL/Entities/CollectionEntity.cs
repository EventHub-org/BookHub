﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.DAL.Entities
{
    public class CollectionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserEntity User { get; set; }
        public int UserId { get; set; }
    }
}
