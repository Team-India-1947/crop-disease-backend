namespace hackathon_template.Models; 

public class UserSettings {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int AlertsFromInDays { get; set; }
    public double AlertRadius { get; set; }
    public bool GetEmailAlerts { get; set; }
    public bool ShareLocation { get; set; }
}