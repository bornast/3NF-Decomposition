﻿using System.Collections;
using System.Collections.Generic;

namespace _3NF.Decomposition.Core.Entities
{
    public class Key
    {
        public int Id { get; set; }
        public virtual Relation Relation { get; set; }
        public int RelationId { get; set; }
        public virtual ICollection<KeyAttribute> KeyAttributes { get; set; } = new List<KeyAttribute>();
    }
}
