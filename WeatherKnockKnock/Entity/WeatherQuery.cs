namespace WeatherKnockKnock.Entity
{
    public class WeatherQuery
    {
        public int? limit { get; set; }
        public int? offset { get; set; }
        public string? format { get; set; }
        public string? locationName { get; set; }
        public string? elementName { get; set; }
        public string? sort { get; set; }
        public string? startTime { get; set; }
        public string? timeFrom { get; set; }
        public string? timeTo { get; set; }
    }
}
