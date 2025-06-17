using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlackoutViewer.Data;
using BlackoutViewer.Models;

namespace BlackoutViewer.Controllers;

public class GroupsController : Controller
{
    private readonly ApplicationDbContext _context;

    public GroupsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Groups.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var @group = await _context.Groups
            .FirstOrDefaultAsync(m => m.Id == id);
        if (@group is null)
        {
            return NotFound();
        }

        return View(@group);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description")] Group @group)
    {
        if (ModelState.IsValid)
        {
            _context.Add(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(@group);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var @group = await _context.Groups.FindAsync(id);
        if (@group is null)
        {
            return NotFound();
        }
        return View(@group);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Group @group)
    {
        if (id != @group.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(@group);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(@group.Id))
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
        return View(@group);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var @group = await _context.Groups
            .FirstOrDefaultAsync(m => m.Id == id);
        if (@group is null)
        {
            return NotFound();
        }

        return View(@group);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var @group = await _context.Groups.FindAsync(id);
        if (@group is not null)
        {
            _context.Groups.Remove(@group);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GroupExists(int id)
    {
        return _context.Groups.Any(e => e.Id == id);
    }
}
