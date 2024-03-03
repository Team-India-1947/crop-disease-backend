namespace hackathon_template.Services; 

public interface IBlobService {
    Task<string> StoreImage(BinaryData image);
}