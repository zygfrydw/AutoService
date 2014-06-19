namespace AutoServiceManager.Common.Model
{
    public class PartOperation
    {
        public long Id { get; set; }
        public long WorkerId { get; set; }
        public virtual Worker Worker { get; set; }
        public virtual Part Part { get; set; }
        public long PartId{ get; set; }
        public PartOperationType OperationType { get; set; }
    }
}