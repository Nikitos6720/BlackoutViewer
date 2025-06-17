using BlackoutViewer.Models;

using ClosedXML.Excel;

namespace BlackoutViewer.Data.FileDataConverter;

public class ExcelDataConverter : IDataConverter
{
    public Task<List<Schedule>> ConvertAsync(IFormFile File)
    {
        var list = new List<Schedule>();
        var file = Path.GetFileName(File.FileName);

        using var workbook = new XLWorkbook(File.OpenReadStream());
        var rows = workbook.Worksheet(1).RowsUsed();

        foreach (var  row in rows)
        {
            list.AddRange(CreateSchedule(row));
        }

        return Task.FromResult(list);
    }

    private List<Schedule> CreateSchedule(IXLRow row)
    {
        var list = new List<Schedule>();

        var value = row.Cell(1).GetString().Trim(' ');
        var values = value.Split('.');
        if (values.Length != 2)
        {
            throw new FormatException("Data format is incorrect. Expected format: 'Group Name,Day enum.Start Time-End Time'.");
        }

        var names = values[0].Split(",");
        if (!int.TryParse(names[0], out int groupId))
        {
            throw new FormatException("Group name is not in a valid format. Expected format: 'Group Name,Day enum'.");
        }

        if (!Enum.TryParse<DayEnum>(names[1], out var dayEnum))
        {
            throw new FormatException("Day enum is not in a valid format. Expected format: 'DayEnum'.");
        }

        var datas = values[1].Split(';');

        foreach(var data in datas)
        {
            var parts = data.Split('-');
            if (parts.Length < 2)
            {
                throw new FormatException("Data of time's format is incorrect. Expected format: 'Start Time-End Time'.");
            }

            var start = parts[0];
            var end= parts[1];
            if (!TimeOnly.TryParse(start, out var startTime) || !TimeOnly.TryParse(end, out var endTime))
            {
                throw new FormatException("Start or End time is not in a valid format. Expected format: 'HH:mm'.");
            }

            if (startTime >= endTime)
            {
                throw new FormatException("Start time must be earlier than end time.");
            }

            list.Add(CreateSchedule(groupId, dayEnum, startTime, endTime));
        }

        return list;
    }

    private Schedule CreateSchedule(int groupId, DayEnum dayEnum, TimeOnly startTime, TimeOnly endTime)
    {
        return new Schedule
        {
            GroupId = groupId,
            Day = dayEnum,
            StartTime = startTime,
            EndTime = endTime
        };
    }
}