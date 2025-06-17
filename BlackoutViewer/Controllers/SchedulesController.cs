using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlackoutViewer.Data;
using BlackoutViewer.Models;
using BlackoutViewer.Data.FileDataConverter;

namespace BlackoutViewer.Controllers;

public class SchedulesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IDataConverter _dataConverter;

    public SchedulesController(ApplicationDbContext context, IDataConverter dataConverter)
    {
        _context = context;
        _dataConverter = dataConverter;
    }

    // GET: Schedules
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Schedules.Include(s => s.Group);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Schedules/Create
    public IActionResult Create()
    {
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
        return View();
    }

    // POST: Schedules/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,GroupId,Day,StartTime,EndTime")] Schedule schedule)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", schedule.GroupId);
        return View(schedule);
    }

    // GET: Schedules/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule is null)
        {
            return NotFound();
        }
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", schedule.GroupId);
        return View(schedule);
    }

    // POST: Schedules/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,GroupId,Day,StartTime,EndTime")] Schedule schedule)
    {
        if (id != schedule.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schedule);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(schedule.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", schedule.GroupId);
        return View(schedule);
    }

    // GET: Schedules/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var schedule = await _context.Schedules
            .Include(s => s.Group)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (schedule is null)
        {
            return NotFound();
        }

        return View(schedule);
    }

    // POST: Schedules/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule is not null)
        {
            _context.Schedules.Remove(schedule);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        using var reader = new StreamReader(file.OpenReadStream());
        string content = await reader.ReadToEndAsync();

        var schedules = await _dataConverter.ConvertAsync(file);
        if (schedules.Count == 0)
        {
            return BadRequest("No valid schedules found in the file.");
        }

        foreach (var schedule in schedules)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid schedule data.");
            }
            _context.Schedules.Add(schedule);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            return BadRequest($"Error saving schedules: {ex.Message}");
        }

        return Json("data: \"success\"");
    }

    private bool ScheduleExists(int id)
    {
        return _context.Schedules.Any(e => e.Id == id);
    }
}