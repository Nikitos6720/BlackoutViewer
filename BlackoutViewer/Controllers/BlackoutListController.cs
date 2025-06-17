using System.Text;
using System.Text.Json;
using BlackoutViewer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlackoutViewer.Controllers;

public class BlackoutListController : Controller
{
    private readonly ApplicationDbContext _context;

    public BlackoutListController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> GetGroup(int? groupId)
    {
        ArgumentNullException.ThrowIfNull(groupId, nameof(groupId));
        var now = TimeOnly.Parse(DateTime.Now.ToString("HH:mm"));
        var day = DateTime.Now.DayOfWeek == 0 ? 6 : (int)DateTime.Now.DayOfWeek - 1;

        var schedules = await _context.Schedules
                                      .AnyAsync(a => a.GroupId == groupId &&
                                                     (int)a.Day == day &&
                                                     now.IsBetween(a.StartTime, a.EndTime));

        return Json("Your group " + (schedules ? "have" : "do not have") + " blackout at this time.");
    }

    public async Task<IActionResult> SelectSchedulesByGroup(int? groupId)
    {
        ArgumentNullException.ThrowIfNull(groupId, nameof(groupId));

        var schedules = await _context.Schedules
                                      .Where(a => a.GroupId == groupId)
                                      .ToListAsync();

        ArgumentNullException.ThrowIfNull(schedules, nameof(schedules));
        return Json(schedules);
    }

    public async Task<IActionResult> SelectOneDay(int? groupId)
    {
        ArgumentNullException.ThrowIfNull(groupId, nameof(groupId));
        var day = DateTime.Now.DayOfWeek == 0 ? 6 : (int)DateTime.Now.DayOfWeek - 1;

        var schedules = await _context.Schedules
                                      .Where(a => a.GroupId == groupId && (int)a.Day == day)
                                      .ToListAsync();

        if (schedules is null || schedules.Count == 0)
        {
            return NotFound($"No schedules found for group ID {groupId}.");
        }

        return Json(schedules);
    }

    public async Task<FileResult> DownloadJsonFile(int? groupId)
    {
        ArgumentNullException.ThrowIfNull(groupId, nameof(groupId));

        var schedules = await _context.Schedules
                                      .Where(b => b.GroupId == groupId)
                                      .ToListAsync();

        if (schedules is null || schedules.Count == 0)
        {
            throw new ArgumentException("No schedules found for the specified group ID.", nameof(groupId));
        }

        var json = JsonSerializer.Serialize(schedules);
        var fileName = $"Schedules_Group_{groupId}.json";
        return File(Encoding.UTF8.GetBytes(json), "application/json", fileName);
    }
}