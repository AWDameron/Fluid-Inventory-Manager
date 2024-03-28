using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FIMS2.Database;
using FIMS2.Models;

namespace FIMS2.Controllers
{
    public class LotsController : Controller
    {
        private readonly FIMSContext _context;

        public LotsController(FIMSContext context)
        {
            _context = context;
        }

        // GET: Lots
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lots
                .Where(x => x.IsActive == true)
                .ToListAsync());

        }

        // GET: Lots/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = await _context.Lots
                .FirstOrDefaultAsync(m => m.LotNumber == id);
            if (lot == null)
            {
                return NotFound();
            }

            return View(lot);
        }

        // GET: Lots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LotNumber,LotName,LotNotes, TotalQuantity,AvailableQuantity,DateOnly")] Lot lot)
        {
            

            if (ModelState.IsValid)
            {
                if (lot.TotalQuantity < 0)
                {
                    // verifies if total quantity is greater than 0
                    ModelState.AddModelError(nameof(Lot.TotalQuantity), "Total quantity cannot be below 0.");
                    return View(lot);
                }

                // Set AvailableQuantity and save changes only if ModelState is valid
                lot.AvailableQuantity = lot.TotalQuantity;
                _context.Add(lot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(lot);
        }

        // GET: Lots/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = await _context.Lots.FindAsync(id);
            if (lot == null)
            {
                return NotFound();
            }
            return View(lot);
        }

        // POST: Lots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LotNumber,AvailableQuantity,TotalQuantity,LotName,LotNotes")] Lot lot)
        {
            if (id != lot.LotNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LotExists(lot.LotNumber))
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
            return View(lot);
        }

        // GET: Lots/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = await _context.Lots
                .FirstOrDefaultAsync(m => m.LotNumber == id);
            if (lot == null)
            {
                return NotFound();
            }

            return View(lot);
        }

        // POST: Lots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var lot = await _context.Lots.FindAsync(id);
            if (lot != null)
            {
                lot.IsActive = false;
                _context.Lots.Update(lot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LotExists(string id)
        {
            return _context.Lots.Any(e => e.LotNumber == id);
        }
    }
}
