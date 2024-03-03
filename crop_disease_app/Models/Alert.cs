namespace hackathon_template.Models; 

public class Alert(int id, string postedBy, long timestamp, double latitude, double longitude, string pest) {
    public int Id { get; init; } = id;
    public string PostedBy { get; set; } = postedBy;
    public long Timestamp { get; set; } = timestamp;
    public double Latitude { get; set; } = latitude;
    public double Longitude { get; set; } = longitude;
    public string Pest { get; set; } = pest;

}