namespace _3NF.Decomposition.Core.Entities
{
    public class Key
    {
        public int Id { get; set; }
        public virtual Relation Relation { get; set; }
        public int RelationId { get; set; }
    }
}
