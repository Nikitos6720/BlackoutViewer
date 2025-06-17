using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlackoutViewer.Data;
using BlackoutViewer.Models;

namespace BlackoutViewer.Controllers;

public class AddressesController : Controller
{
    private readonly ApplicationDbContext _context;

    public AddressesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Addresses.Include(a => a.Group);
        return View(await applicationDbContext.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var address = await _context.Addresses
            .Include(a => a.Group)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (address is null)
        {
            return NotFound();
        }

        return View(address);
    }

    public IActionResult Create()
    {
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,GroupId,Title")] Address address)
    {
        if (ModelState.IsValid)
        {
            _context.Add(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", address.GroupId);
        return View(address);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var address = await _context.Addresses.FindAsync(id);
        if (address is null)
        {
            return NotFound();
        }
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", address.GroupId);
        return View(address);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,GroupId,Title")] Address address)
    {
        if (id != address.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(address);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(address.Id))
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
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", address.GroupId);
        return View(address);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var address = await _context.Addresses
            .Include(a => a.Group)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (address is null)
        {
            return NotFound();
        }

        return View(address);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address is not null)
        {
            _context.Addresses.Remove(address);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AddressExists(int id)
    {
        return _context.Addresses.Any(e => e.Id == id);
    }
}
