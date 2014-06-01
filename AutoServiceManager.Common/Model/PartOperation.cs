namespace AutoServiceManager.Common.Model
{
    public class PartOperation
    {
        public long Id { get; set; }
        public long WorkerId { get; set; }
        public Worker Worker { get; set; }
        public Part Part { get; set; }
        public long PartId{ get; set; }
        public PartOperationType OperationType { get; set; }
    }
}