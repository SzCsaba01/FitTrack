namespace FitTrack.Service.Contract;

public interface ICloudinaryService
{
    public Task<string> UploadImageAsync(Stream fileStream, string fileName, string folder);
}
