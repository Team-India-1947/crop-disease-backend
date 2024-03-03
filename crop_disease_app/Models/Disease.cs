namespace hackathon_template.Models; 

public class Disease
{
    public Disease(int id, string userId, string diseaseName, decimal latitude, decimal longitude, string url) {
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
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Url { get; set; }
}