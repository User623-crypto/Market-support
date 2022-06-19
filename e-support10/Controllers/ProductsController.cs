using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_support10.Data;
using e_support10.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Data.SqlClient;

namespace e_support10.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> ClientIndex()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,Quantity")] Product product,IFormFile Photo)
        {
            if (ModelState.IsValid)
            {

                //process the photo upload
                if(Photo.Length > 0)
                {
                    //if true it means client uploaded a photo
                    var tempFile = Path.GetTempFileName();

                    //create unique name using GUID
                    var fileName = Guid.NewGuid() + "-" + Photo.FileName;
                    //set destination path and filename
                    var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\img\\product_uploads\\" + fileName;
                    //use a stream to create the new file in the server

                    using var stream = new FileStream(uploadPath, FileMode.Create);
                    await Photo.CopyToAsync(stream);

                    //add unique filename to the product for the photo
                    product.Photo = fileName;
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Photo,Description,Quantity")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public  IActionResult AddToCart(int ProductId,int Quantity)
        {
            // get the price
            var price = _context.Products.Find(ProductId).Price;
            //var customerId = HttpContext.Session.GetString("CustomerId");
            var customerId = this.User.GetUserId();
            

            var cartItem = _context.Carts.SingleOrDefault(c => c.ProductId == ProductId && c.CustomerId == customerId);

            if(cartItem!= null)
            {
                //update the qunatity
                cartItem.Quantity += Quantity;
                _context.Update(cartItem);
                _context.SaveChanges();
                return RedirectToAction("Cart");

            }
            else
            {
                // create a new Cart Object

                var cart = new Cart
                {
                    ProductId = ProductId,
                    Quantity = Quantity,
                    Price = price,
                    CustomerId = customerId,
                    DateCreated = DateTime.Now
                };
                //save the cart to db
                _context.Carts.Add(cart);
                _context.SaveChanges();

                //redirect to show the cart
                return RedirectToAction("Cart");

            }
           
        }

        //Get /Products/RemoveFromCart/1
        [Authorize(Roles = "Customer")]
        public IActionResult RemoveFromCart(int Id)
        {

            var cartItem = _context.Carts.Find(Id);

            if (cartItem!=null)
            {
                _context.Carts.Remove(cartItem);
                _context.SaveChanges();
            }
            return RedirectToAction("Cart");

        }

        //Get:Products/Cart
        [Authorize(Roles = "Customer")]
        public IActionResult Cart()
        {
            var customerId = this.User.GetUserId();

            var cartItems = _context.Carts.Include(c=>c.Product).Where(c => c.CustomerId == customerId);
            //load the cart page
            return View(cartItems);

        }
        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public IActionResult Checkout()
        {
            /**Create a new Order and get the id back*/
            var userId = User.GetUserId();
            var list = new List<string>();
            var passedTheQuantityTest = true;
            IQueryable<Cart> cartItemsTemp;

            ViewBag.list = new List<string>();
            using (var transaction = _context.Database.BeginTransaction())
            {
                var total = (from c in _context.Carts
                             where c.CustomerId == userId
                             select c.Quantity * c.Price).Sum();
                var order = new Order
                {
                    CustomerId = userId,
                    Date = DateTime.Now,
                    Total = total
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                var cartItems = _context.Carts.Where(c => c.CustomerId == userId);
                cartItemsTemp=cartItems;
                foreach (var item in cartItems)
                {
                    var orderRow = new OrderRow
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Cost = item.Price,
                        Warranty = 5,
                        Total = item.Price * item.Quantity
                    };
                    var produkti = _context.Products.Find(orderRow.ProductId);
                    if (item.Quantity > produkti?.Quantity)
                    {
                        passedTheQuantityTest = false;
                        ViewBag.list.Add(produkti.Name);
                    };
                    _context.OrderRows.Add(orderRow);
                }

                _context.SaveChanges();

               /* foreach (var item in cartItems)
                {
                    
                    if (item.ProductId != null)
                    {
                        var product = _context.Products.Find(item.ProductId);
                        product.Quantity -= item.Quantity;

                        _context.Update(product);
                        _context.SaveChanges();
                    }
                }*/

                

                _context.Database.ExecuteSqlRaw("Delete carts where CustomerId=@id",new SqlParameter("@id",userId));

                _context.SaveChanges();
                if (passedTheQuantityTest)
                {
                    transaction.Commit();

                }else transaction.Rollback();
            }
            if(passedTheQuantityTest)
            return Redirect("/Orders");

            return View("Cart", cartItemsTemp);
        }
        
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
