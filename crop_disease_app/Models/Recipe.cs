namespace hackathon_template.Models;

public class Recipe(int id, string title, long timestamp, string body) {
    public int id { get; set; } = id;
    public string title { get; set; } = title;
    public long timestamp { get; set; } = timestamp;
    public string body { get; set; } = body;
}