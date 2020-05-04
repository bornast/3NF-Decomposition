namespace _3NF.Decomposition.Core.Entities
{
    public class FminAttribute
    {
        public int Id { get; set; }
        public virtual Relation Relation { get; set; }
        public int RelationId { get; set; }
        public virtual Attribute LeftSideAttribute { get; set; }
        public int LeftSideAttributeId { get; set; }
        public virtual Attribute RightSideAttribute { get; set; }
        public int RightSideAttributeId { get; set; }
        public int Sequence { get; set; }
    }
}
