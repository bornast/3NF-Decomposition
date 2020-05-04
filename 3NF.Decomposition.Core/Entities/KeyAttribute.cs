namespace _3NF.Decomposition.Core.Entities
{
    public class KeyAttribute
    {
        public virtual Key Key { get; set; }
        public int KeyId { get; set; }
        public virtual Attribute Attribute { get; set; }
        public int AttributeId { get; set; }
    }
}
