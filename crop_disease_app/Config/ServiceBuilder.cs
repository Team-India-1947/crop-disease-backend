using Azure.Storage.Blobs;
using hackathon_template.Services;

namespace hackathon_template.Config; 

public static class ServiceBuilder {
    public static void AddServices(WebApplicationBuilder builder) {
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        
        builder.Services.AddSingleton(x => 
            new BlobServiceClient(builder.Configuration.GetSection("AzureBlobStorageConnectionString").Get<string>()));
        builder.Services.AddSingleton<IBlobService>( x=> 
            new BlobService(
                x.GetRequiredService<BlobServiceClient>(), 
                builder.Configuration.GetSection("AzureBlobStorageConnectionString").Get<string>(),
                builder.Configuration.GetSection("AccountKey").Get<string>()
            ));
    }
}