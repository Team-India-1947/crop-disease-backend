namespace hackathon_template.Models; 

public class Disease
{
    public Disease(int id, string userId, string diseaseName, double latitude, double longitude, string url) {
        Id = id;
        UserId = userId;
        DiseaseName = diseaseName;
        Latitude = latitude;
        Longitude = longitude;
        Url = url;
    }

    public int Id { get; set; }
    public string UserId { get; set; }
    public string DiseaseName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Url { get; set; }
}