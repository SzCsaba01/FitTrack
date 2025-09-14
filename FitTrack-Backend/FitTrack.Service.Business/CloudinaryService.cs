using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FitTrack.Service.Business;
// TEST:
public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;
    private readonly IMapper _mapper;
    private readonly ILogger<CloudinaryService> _logger;

    public CloudinaryService(
        IConfiguration configuration,
        IMapper mapper,
        ILogger<CloudinaryService> logger)
    {
        _mapper = mapper;
        _logger = logger;

        var cloudName = configuration["Cloudinary:CloudName"];
        var apiKey = configuration["Cloudinary:ApiKey"];
        var apiSecret = configuration["Cloudinary:ApiSecret"];

        if (String.IsNullOrEmpty(cloudName) || String.IsNullOrEmpty(apiKey) || String.IsNullOrEmpty(apiSecret))
        {
            _logger.LogError("Cloud Name, Api Key or Api Secret is missing");

            throw new ConfigurationException();
        }

        _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
    }

    public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string folder)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = folder,
            Format = "webp",
            Transformation = new Transformation().Quality("auto").FetchFormat("auto")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams).ConfigureAwait(false);

        if (uploadResult.Error != null)
        {
            _logger.LogError(uploadResult.Error.Message);
        }

        return uploadResult.SecureUrl.ToString();
    }
}
