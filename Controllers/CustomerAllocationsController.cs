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
    public class CustomerAllocationsController : Controller
    {
        private readonly FIMSContext _context;

        public CustomerAllocationsController(FIMSContext context)
        {
            _context = context;
        }

        // GET: CustomerAllocations
        public async Task<IActionResult> Index()
        {
            var fIMSContext = _context.CustomerAllocations.Include(c => c.Lot);
            return View(await fIMSContext.ToListAsync());
        }

        // GET: CustomerAllocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerAllocation = await _context.CustomerAllocations
                .Include(c => c.Lot)
                .FirstOrDefaultAsync(m => m.AllocationID == id);
            if (customerAllocation == null)
            {
                return NotFound();
            }

            return View(customerAllocation);
        }

        // GET: CustomerAllocations/Create
        public IActionResult Create()
        {
            ViewData["LotNumber"] = new SelectList(_context.Lots.Where(x => x.IsActive == true), "LotNumber", "LotNumber");
            return View();
        }

        // POST: CustomerAllocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AllocationID,CustomerNumber,QuantityUsed,LotNumber")] CustomerAllocation customerAllocation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Retrieve the corresponding Lot record
                    var lot = await _context.Lots.FirstOrDefaultAsync(l => l.LotNumber == customerAllocation.LotNumber);

                    if (lot == null)
                    {
                        // Lot does not exist
                        ModelState.AddModelError(string.Empty, "Lot not found.");
                        ViewData["LotNumber"] = new SelectList(_context.Lots, "LotNumber", "LotNumber", customerAllocation.LotNumber);
                        return View(customerAllocation);
                    }

                    // Check if the QuantityUsed exceeds the TotalQuantity in the lot
                    if (customerAllocation.QuantityUsed > lot.TotalQuantity)
                    {
                        ModelState.AddModelError(nameof(CustomerAllocation.QuantityUsed), "Quantity used exceeds the total quantity available in the lot.");
                        ViewData["LotNumber"] = new SelectList(_context.Lots, "LotNumber", "LotNumber", customerAllocation.LotNumber);
                        return View(customerAllocation);
                    }

                    // Check if the AvailableQuantity would become negative after deduction
                    if (lot.AvailableQuantity - customerAllocation.QuantityUsed < 0)
                    {
                        ModelState.AddModelError(string.Empty, "Deducting the quantity used would result in a negative available quantity.");
                        ViewData["LotNumber"] = new SelectList(_context.Lots, "LotNumber", "LotNumber", customerAllocation.LotNumber);
                        return View(customerAllocation);
                    }

                    // Add the CustomerAllocation record to the database
                    _context.Add(customerAllocation);
                    await _context.SaveChangesAsync();

                    // Subtract the QuantityUsed from the AvailableQuantity
                    lot.AvailableQuantity -= customerAllocation.QuantityUsed;

                    // Update the Lot record
                    _context.Update(lot);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                ViewData["LotNumber"] = new SelectList(_context.Lots, "LotNumber", "LotNumber", customerAllocation.LotNumber);
                return View(customerAllocation);
            }

            return View(customerAllocation);
        }

        // GET: CustomerAllocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerAllocation = await _context.CustomerAllocations.FindAsync(id);
            if (customerAllocation == null)
            {
                return NotFound();
            }
            ViewData["LotNumber"] = new SelectList(_context.Lots, "LotNumber", "LotNumber", customerAllocation.LotNumber);
            return View(customerAllocation);
        }

        // POST: CustomerAllocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AllocationID,CustomerNumber,QuantityUsed,LotNumber")] CustomerAllocation customerAllocation)
        {
            if (id != customerAllocation.AllocationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerAllocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerAllocationExists(customerAllocation.AllocationID))
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
            ViewData["LotNumber"] = new SelectList(_context.Lots, "LotNumber", "LotNumber", customerAllocation.LotNumber);
            return View(customerAllocation);
        }

        // GET: CustomerAllocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerAllocation = await _context.CustomerAllocations
                .Include(c => c.Lot)
                .FirstOrDefaultAsync(m => m.AllocationID == id);
            if (customerAllocation == null)
            {
                return NotFound();
            }

            return View(customerAllocation);
        }

        // POST: CustomerAllocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerAllocation = await _context.CustomerAllocations.FindAsync(id);
            if (customerAllocation != null)
            {
                _context.CustomerAllocations.Remove(customerAllocation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerAllocationExists(int id)
        {
            return _context.CustomerAllocations.Any(e => e.AllocationID == id);
        }
    }
}
