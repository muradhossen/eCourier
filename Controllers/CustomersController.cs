using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eCourier.Data;
using eCourier.Models;
using eCourier.Repositories.Abstraction;
using eCourier.Dto.CriteriaDto;
using Microsoft.AspNetCore.Authorization;

namespace eCourier.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ApplicationDbContext context
            , ICustomerRepository customerRepository)
        {
            _context = context;
            _customerRepository = customerRepository;
        }

 
        public async Task<IActionResult> Index([FromQuery] CustomerCriteriaDto criteriaDto)
        {
            var orders = await _customerRepository.GetCustomersByCriteriaAsync(criteriaDto);
              return View(orders);
        }
         
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
      
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
 
        [HttpPost]         
        public async Task<IActionResult> Edit(int id,[FromForm] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }


    }
}
