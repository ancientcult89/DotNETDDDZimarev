namespace Marketplace.Infrastructure
{
    public class EventData
    {
        public readonly Guid EventId;
        public readonly string Type;
        public readonly bool IsJson;
        public readonly byte[] Data;
        public readonly byte[] Metadata;
    }
}
