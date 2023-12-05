using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using MusicShop.Models;

namespace MusicShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly MusicShopContext _context;

        public OrdersController(MusicShopContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var musicShopContext = _context.Order.Include(o => o.Customer).Include(o => o.Product).Include(o => o.ShippingInfo);
            return View(await musicShopContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Include(o => o.ShippingInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "Id", "Id");
            ViewData["ShippingInfoId"] = new SelectList(_context.Set<ShippingInfo>(), "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CardNumber,CustomerId,ShippingInfoId,ProductId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "Id", "Id", order.ProductId);
            ViewData["ShippingInfoId"] = new SelectList(_context.Set<ShippingInfo>(), "Id", "Id", order.ShippingInfoId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "Id", "Id", order.ProductId);
            ViewData["ShippingInfoId"] = new SelectList(_context.Set<ShippingInfo>(), "Id", "Id", order.ShippingInfoId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CardNumber,CustomerId,ShippingInfoId,ProductId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "Id", "Id", order.ProductId);
            ViewData["ShippingInfoId"] = new SelectList(_context.Set<ShippingInfo>(), "Id", "Id", order.ShippingInfoId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Include(o => o.ShippingInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'MusicShopContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
