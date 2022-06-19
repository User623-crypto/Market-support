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
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Index()
        {
            var customerId = User.GetUserId();
            return View(await _context.Orders.Where(o=>o.CustomerId==customerId).ToListAsync());
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customerId = User.GetUserId();


            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.CustomerId != customerId) return NotFound();

            var orderRows = _context.OrderRows.Include(c => c.Product).Where(o => o.OrderId == id);
            if (orderRows == null) return NotFound();

            return View(orderRows);
        }

       


       
    

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
