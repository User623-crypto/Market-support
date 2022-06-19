using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_support10.Data;
using e_support10.Models;
using Microsoft.AspNetCore.Authorization;

namespace e_support10.Controllers
{
    public class SupplyOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplyOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        // GET: SupplyOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.SupplyOrders.ToListAsync());
        }

        // GET: SupplyOrders/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplyOrder = await _context.SupplyOrders
                .FirstOrDefaultAsync(m => m.Id == id); 
            if (supplyOrder == null)
            {
                return NotFound();
            }
            var supplyOrderRows = _context.SupplyOrderRows.Include(c => c.Product).Where(o => o.SupplyOrderId == id);
            if (supplyOrderRows == null) return NotFound();

            return View(supplyOrderRows);
        }

        // GET: SupplyOrders/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ProduktiId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: SupplyOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,SupplierName,Date,Total")] SupplyOrder supplyOrder, int ProductId, int quantity, int price)
        {
            if (ModelState.IsValid)
            {

                using (var transaction = _context.Database.BeginTransaction())
                {
                    var Product = _context.Products.Find(ProductId);
                    var order = new SupplyOrder
                    {
                        Date = supplyOrder.Date,
                        SupplierName = supplyOrder.SupplierName,
                        Total = quantity * price
                    };
                    _context.SupplyOrders.Add(order);
                    _context.SaveChanges();

                    var orderRow = new SupplyOrderRow
                    {
                        SupplyOrderId = order.Id,
                        ProductId = ProductId,
                        Quantity = quantity,
                        Cost = price,
                        Warranty = 5,
                        Total = price * quantity
                    };

                    _context.SupplyOrderRows.Add(orderRow);
                    _context.SaveChanges();
                    transaction.Commit();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(supplyOrder);
        }

        // GET: SupplyOrders/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplyOrder = await _context.SupplyOrders.FindAsync(id);
            if (supplyOrder == null)
            {
                return NotFound();
            }
            return View(supplyOrder);
        }

        // POST: SupplyOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SupplierName,Date,Total")] SupplyOrder supplyOrder)
        {
            if (id != supplyOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplyOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplyOrderExists(supplyOrder.Id))
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
            return View(supplyOrder);
        }

        // GET: SupplyOrders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplyOrder = await _context.SupplyOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplyOrder == null)
            {
                return NotFound();
            }

            return View(supplyOrder);
        }

        // POST: SupplyOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplyOrder = await _context.SupplyOrders.FindAsync(id);
            _context.SupplyOrders.Remove(supplyOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplyOrderExists(int id)
        {
            return _context.SupplyOrders.Any(e => e.Id == id);
        }
    }
}
