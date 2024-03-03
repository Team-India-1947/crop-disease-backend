namespace hackathon_template.Models; 

public class UserSettings(double latitude, double longitude, int alertsFromInDays, double alertRadius, bool getEmailAlerts, bool shareLocation) {
    public double Latitude { get; set; } = latitude;
    public double Longitude { get; set; } = longitude;
    public int AlertsFromInDays { get; set; } = alertsFromInDays;
    public double AlertRadius { get; set; } = alertRadius;
    public bool GetEmailAlerts { get; set; } = getEmailAlerts;
    public bool ShareLocation { get; set; } = shareLocation;
}