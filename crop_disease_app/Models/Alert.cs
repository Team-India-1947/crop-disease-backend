namespace hackathon_template.Models; 

public class Alert(long timestamp, double latitude, double longitude, string pest) {
    public string PostedBy { get; set; }
    public long Timestamp { get; set; } = timestamp;
    public double Latitude { get; set; } = latitude;
    public double Longitude { get; set; } = longitude;
    public string Pest { get; set; } = pest;

}