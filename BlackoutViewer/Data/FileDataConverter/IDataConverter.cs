using BlackoutViewer.Models;

namespace BlackoutViewer.Data.FileDataConverter;

public interface IDataConverter
{
    public Task<List<Schedule>> ConvertAsync(IFormFile File);
}