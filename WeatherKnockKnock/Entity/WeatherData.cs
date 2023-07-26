namespace Entity
{

    public class WeatherData
    {
        public string success { get; set; } = "";
        public Records records { get; set; } = new Records();
    }


    public class Records
    {
        public string datasetDescription { get; set; } = "";
        public List<Location> location { get; set; } = new List<Location>();
    }

    public class Location
    {
        public string locationName { get; set; } = "";
        public List<WeatherElement> weatherElement { get; set; } = new List<WeatherElement>();
    }

    public class WeatherElement
    {
        public string elementName { get; set; } = "";
        public List<Time> time { get; set; } = new List<Time>();
    }

    public class Time
    {
        public string startTime { get; set; } = "";
        public string endTime { get; set; } = "";
        public Parameter parameter { get; set; } = new Parameter();
    }

    public class Parameter
    {
        public string parameterName { get; set; } = "";
        public string parameterValue { get; set; } = "";
    }
}
