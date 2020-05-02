using System.ComponentModel.DataAnnotations;

namespace _3NF.Decomposition.Core.Entities
{
    public class Member
    {
        public int Id { get; set; }

        public virtual Relation Relation { get; set; }

        public int RelationId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
