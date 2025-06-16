using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlackoutViewer.Data;
using BlackoutViewer.Models;

namespace BlackoutViewer.Controllers;

public class SchedulesController : Controller
{
    private readonly ApplicationDbContext _context;

    public SchedulesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Schedules
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Schedules.Include(s => s.Group);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Schedules/Details/5
    public async Task<IActionResult> Details(int? id)
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

    // GET: Schedules/Create
    public IActionResult Create()
    {
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
        return View();
    }

    // POST: Schedules/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        return Json("data: \"success\"");
    }

    private bool ScheduleExists(int id)
    {
        return _context.Schedules.Any(e => e.Id == id);
    }
}