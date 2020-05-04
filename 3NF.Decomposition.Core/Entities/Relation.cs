using System.Collections.Generic;

namespace _3NF.Decomposition.Core.Entities
{
    public class Relation
    {
        public int Id { get; set; }
        public virtual ICollection<Attribute> Attributes { get; set; } = new List<Attribute>();
        public virtual ICollection<Key> Keys { get; set; } = new List<Key>();
        public virtual ICollection<FminAttribute> FminAttributes { get; set; } = new List<FminAttribute>();
    }
}
