namespace _3NF.Decomposition.Core.Entities
{
    public class FminMember
    {
        public int Id { get; set; }
        public virtual Relation Relation { get; set; }
        public int RelationId { get; set; }
        public virtual Member LeftSideMember { get; set; }
        public int LeftSideMemberId { get; set; }
        public virtual Member RightSideMember { get; set; }
        public int RightSideMemberId { get; set; }
        public int Sequence { get; set; }
    }
}
