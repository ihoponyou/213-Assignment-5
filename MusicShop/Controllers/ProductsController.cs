using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using MusicShop.Models;
using Newtonsoft.Json;

namespace MusicShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MusicShopContext _context;

        public ProductsController(MusicShopContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string productGenre, string productPerformer, string searchString)
        {
            /*
              return _context.Product != null ? 
                          View(await _context.Product.ToListAsync()) :
                          Problem("Entity set 'MusicShopContext.Product'  is null.");
            */

            if (_context.Product == null)
            {
                return Problem("Entity set 'MusicShopContext.Product'  is null.");
            }

            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Product
                                            orderby m.Genre
                                            select m.Genre;
            // LINQ to get list of performers.
            IQueryable<string> performerQuery = from m in _context.Product
                                                orderby m.Performer
                                                select m.Performer;
            var movies = from m in _context.Product
                         select m;

            if (!string.IsNullOrEmpty(productGenre))
            {
                movies = movies.Where(x => x.Genre == productGenre);
            }

            if (!string.IsNullOrEmpty(productPerformer))
            {
                movies = movies.Where(x => x.Performer == productPerformer);
            }

            var movieGenreVM = new ProductGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Performers = new SelectList(await performerQuery.Distinct().ToListAsync()),
                Products = await movies.ToListAsync()
            };

            return View(movieGenreVM);
        }

        // GET: Products
        public async Task<IActionResult> Admin()
        {
            return _context.Product != null ?
                        View(await _context.Product.ToListAsync()) :
                        Problem("Entity set 'MusicShopContext.Product'  is null.");
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,Performer,Price,Quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,Performer,Price,Quantity")] Product product)
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

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'MusicShopContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddToCart(List<int> selectedProducts)
        {
            // get current items in cart
            List<int> cartItems = JsonConvert.DeserializeObject<List<int>>(Request.Cookies["CartItems"] ?? "[]");

            if (selectedProducts != null && selectedProducts.Any())
            {
                foreach (var productId in selectedProducts)
                {
                    // get products from database
                    var product = _context.Product.FirstOrDefault(p => p.Id == productId);

                    if (product != null && product.Quantity > 0)
                    {
                        // decrease quantity -1 when adding to cart
                        product.Quantity -= 1;
                        _context.SaveChanges();

                        cartItems.Add(productId);
                    }
                }
            }

            // save the cart
            Response.Cookies.Append("CartItems", JsonConvert.SerializeObject(cartItems));

            // stay on cart page
            return RedirectToAction("Cart");
        }


        public IActionResult Cart()
        {
            // get items in the cart
            List<int> cartItems = JsonConvert.DeserializeObject<List<int>>(Request.Cookies["CartItems"] ?? "[]");

            // get the products based on id
            List<Product> cartProducts = _context.Product.Where(p => cartItems.Contains(p.Id)).ToList();

            return View(cartProducts);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(List<int> selectedProductsToRemove)
        {
            // get current cart items from the list
            List<int> cartItems = JsonConvert.DeserializeObject<List<int>>(Request.Cookies["CartItems"] ?? "[]");

            if (selectedProductsToRemove != null && selectedProductsToRemove.Any())
            {
                foreach (var productId in selectedProductsToRemove)
                {
                    // get the product from the database
                    var product = _context.Product.FirstOrDefault(p => p.Id == productId);

                    if (product != null)
                    {
                        // Add quantity +1 when removed from the cart
                        product.Quantity += 1;
                        _context.SaveChanges();

                        cartItems.Remove(productId);
                    }
                }
            }

            // save cart
            Response.Cookies.Append("CartItems", JsonConvert.SerializeObject(cartItems));

            // stay in the cart page after removing items
            return RedirectToAction("Cart");
        }


        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
