namespace WeatherKnockKnock.Entity
{
    public class WeatherKnockData
    {
        public string datasetDescription { get; set; } = "";
        public string cityName { get; set; } = "";
        public List<WeatherKnockTime> weatherKnockTimes { get; set; } = new List<WeatherKnockTime>();
    }
    public class WeatherKnockTime
    {
        public string startTime { get; set; } = "";
        public string endTime { get; set; } = "";
        public string weatherStatus { get; set; } = "";
    }
}
