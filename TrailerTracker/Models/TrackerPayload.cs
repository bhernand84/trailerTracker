namespace TrailerTracker.Models
{
    public class TrackerPayload
    {
        public TrailerInfo trailerInfo { get; set; }
        public bool result { get;set;}

        public TrackerPayload() { }
        public TrackerPayload(bool result) {
            this.result = result;
        }
    }
}