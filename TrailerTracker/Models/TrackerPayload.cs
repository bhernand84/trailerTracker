namespace TrailerTracker.Models
{
    public class TrackerPayload
    {
        public bool Result { get;set;}

        public TrackerPayload() { }
        public TrackerPayload(bool result) {
            Result = result;
        }
    }
}