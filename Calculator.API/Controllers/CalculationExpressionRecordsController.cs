using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Calculator.API.DTO;
using Calculator.API.DatabaseContexts;

namespace Calculator.API.Controllers
{
    public class CalculationExpressionRecordsController : Controller
    {
        private readonly CalculationExpressionRecordContext _context;

        public CalculationExpressionRecordsController(CalculationExpressionRecordContext context)
        {
            _context = context;
        }

        // GET: CalculationExpressionRecords
        public async Task<IActionResult> Index()
        {
            return View(await _context.CalculationExpressionRecord.ToListAsync());
        }

        // GET: CalculationExpressionRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calculationExpressionRecord = await _context.CalculationExpressionRecord
                .FirstOrDefaultAsync(m => m.ID == id);
            if (calculationExpressionRecord == null)
            {
                return NotFound();
            }

            return View(calculationExpressionRecord);
        }

        // GET: CalculationExpressionRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CalculationExpressionRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Record")] CalculationExpressionRecord calculationExpressionRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calculationExpressionRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(calculationExpressionRecord);
        }

        // GET: CalculationExpressionRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calculationExpressionRecord = await _context.CalculationExpressionRecord.FindAsync(id);
            if (calculationExpressionRecord == null)
            {
                return NotFound();
            }
            return View(calculationExpressionRecord);
        }

        // POST: CalculationExpressionRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Record")] CalculationExpressionRecord calculationExpressionRecord)
        {
            if (id != calculationExpressionRecord.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calculationExpressionRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalculationExpressionRecordExists(calculationExpressionRecord.ID))
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
            return View(calculationExpressionRecord);
        }

        // GET: CalculationExpressionRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calculationExpressionRecord = await _context.CalculationExpressionRecord
                .FirstOrDefaultAsync(m => m.ID == id);
            if (calculationExpressionRecord == null)
            {
                return NotFound();
            }

            return View(calculationExpressionRecord);
        }

        // POST: CalculationExpressionRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calculationExpressionRecord = await _context.CalculationExpressionRecord.FindAsync(id);
            if (calculationExpressionRecord != null)
            {
                _context.CalculationExpressionRecord.Remove(calculationExpressionRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalculationExpressionRecordExists(int id)
        {
            return _context.CalculationExpressionRecord.Any(e => e.ID == id);
        }
    }
}
