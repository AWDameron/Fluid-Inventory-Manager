using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public async Task<IActionResult> Create()
        {
            ViewData["LotNumber"] = new SelectList(await _context.Lots
                .Where(l => l.IsActive)
                .Select(l => new {
                    Value = l.LotNumber,
                    Text = $"{l.LotNumber} - {l.LotName}"
                }).ToListAsync(), "Value", "Text");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AllocationID,CustomerNumber,QuantityUsed,LotNumber")] CustomerAllocation CustomerAllocation)
        {
            ModelState.Remove(nameof(CustomerAllocation.Lot));
            var lots = await _context.Lots
            .Select(l => new { Value = l.LotNumber, Text = $"{l.LotNumber} - {l.LotName}" })
            .ToListAsync();
            ViewData["LotNumber"] = new SelectList(lots, "Value", "Text");

            try
            {
                if (ModelState.IsValid)
                {
                    // Retrieve the corresponding Lot record
                    var lot = await _context.Lots.FirstOrDefaultAsync(l => l.LotNumber == CustomerAllocation.LotNumber);

                    if (lot == null)
                    {
                        // Lot does not exist
                        ModelState.AddModelError(string.Empty, "Lot not found.");
                        return View(CustomerAllocation);
                    }

                    if (CustomerAllocation.QuantityUsed < 0)
                    {
                        ModelState.AddModelError(nameof(CustomerAllocation.QuantityUsed), "Quantity used cannot be a negative number");
                        return View(CustomerAllocation);
                    }

                    // Check if the QuantityUsed exceeds the TotalQuantity in the lot
                    if (CustomerAllocation.QuantityUsed > lot.TotalQuantity)
                    {
                        ModelState.AddModelError(nameof(CustomerAllocation.QuantityUsed), "Quantity used exceeds the total quantity available in the lot.");
                        return View(CustomerAllocation);
                    }

                    // Check if the AvailableQuantity would become negative after deduction
                    if (lot.AvailableQuantity - CustomerAllocation.QuantityUsed < 0)
                    {
                        ModelState.AddModelError(string.Empty, "Deducting the quantity used would result in a negative available quantity.");
                        return View(CustomerAllocation);
                    }

                    // Add the CustomerAllocation record to the database
                    _context.Add(CustomerAllocation);
                    await _context.SaveChangesAsync();

                    // Subtract the QuantityUsed from the AvailableQuantity
                    lot.AvailableQuantity -= CustomerAllocation.QuantityUsed;
                    
                    // Update the Lot record
                    _context.Update(lot);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                ViewData["LotNumber"] = new SelectList(_context.Lots, "LotNumber", "LotNumber", CustomerAllocation.LotNumber);
                return View(CustomerAllocation);
            }

            return View(CustomerAllocation);
        }

//Commenting out the Edit Controller,It's not something to be utilized in the program at the moment.  May be implemented in the future, but for now we are okay with having CREATE and DELETE

//// GET: CustomerAllocations/Edit/5
//public async Task<IActionResult> Edit(int? id)
//{
//    if (id == null)
//    {
//        return NotFound();
//    }

//    var customerAllocation = await _context.CustomerAllocations.FindAsync(id);
//    if (customerAllocation == null)
//    {
//        return NotFound();
//    }
//    ViewData["LotNumber"] = new SelectList(_context.Lots, "LotNumber", "LotNumber", customerAllocation.LotNumber);
//    return View(customerAllocation);
//}

//// POST: CustomerAllocations/Edit/5
//// To protect from overposting attacks, enable the specific properties you want to bind to.
//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("AllocationID,CustomerNumber,QuantityUsed,LotNumber")] CustomerAllocation customerAllocation)
//        {
//            if (id != customerAllocation.AllocationID)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(customerAllocation);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException ex)
//                {
//                    var entry = ex.Entries.Single();
//                    var databaseValues = entry.GetDatabaseValues();

//                    if (databaseValues == null)
//                    {
//                        return NotFound();
//                    }

//                    var databaseAllocation = (CustomerAllocation)databaseValues.ToObject();
//                    ModelState.AddModelError("", "The record you attempted to edit was modified by another user. The current values have been displayed. Please make your changes again.");
//                    return View(customerAllocation);
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["LotNumber"] = new SelectList(_context.Lots, "LotNumber", "LotNumber", customerAllocation.LotNumber);
//            return View(customerAllocation);
//        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerAllocation = await _context.CustomerAllocations.FindAsync(id);
            if (customerAllocation == null)
            {
                return NotFound();
            }

            // Retrieve the corresponding Lot record
            var lot = await _context.Lots.FirstOrDefaultAsync(l => l.LotNumber == customerAllocation.LotNumber);

            if (lot != null)
            {
                // Add QuantityUsed back to AvailableQuantity
                lot.AvailableQuantity += customerAllocation.QuantityUsed;
                _context.Update(lot);
            }

            // Remove the CustomerAllocation record
            _context.CustomerAllocations.Remove(customerAllocation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
