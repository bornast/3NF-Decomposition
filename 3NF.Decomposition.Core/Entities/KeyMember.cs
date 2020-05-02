namespace _3NF.Decomposition.Core.Entities
{
    public class KeyMember
    {
        public virtual Key Key { get; set; }
        public int KeyId { get; set; }
        public virtual Member Member { get; set; }
        public int MemberId { get; set; }
    }
}
