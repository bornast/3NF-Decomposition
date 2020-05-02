using System.Collections.Generic;

namespace _3NF.Decomposition.Core.Entities
{
    public class Relation
    {
        public int Id { get; set; }
        public virtual ICollection<Member> Members { get; set; } = new List<Member>();
        public virtual ICollection<Key> Keys { get; set; } = new List<Key>();
        public virtual ICollection<FminMember> FminMembers { get; set; } = new List<FminMember>();
    }
}
