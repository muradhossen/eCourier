using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eCourier.Data;
using eCourier.Models;
using eCourier.Repositories.Abstraction;
using eCourier.Dto.CriteriaDto;
using eCourier.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using eCourier.Extention;
using System.Drawing.Text;

namespace eCourier.Controllers
{
    [Authorize(Roles = "Customer,Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(ApplicationDbContext context
            , IOrderRepository orderRepository
            , ICustomerRepository customerRepository
            , UserManager<AppUser> userManager)
        {
            _context = context;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Index([FromQuery] OrderCriteriaDto criteriaDto)
        {

            criteriaDto.AppUserId = User.IsInRole("Customer") 
                ? criteriaDto.AppUserId = User.GetUserId() 
                : null;

            var orders = await _orderRepository.GetOrdersByCriteriaAsync(criteriaDto);
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetOrderDetails(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] OrderDto order)
        {
            if (ModelState.IsValid)
            {
                var recipient = await _customerRepository.CreateOrUpdateCustomerAsync(order.Recipient);
                if (recipient is null)
                {
                    return BadRequest("Failed to save recipient info.Please try again!");
                }
                order.RecipientId = recipient.Id;

                order.Recipient = null;
                order.AppUserId = User.GetUserId();

                await _orderRepository.CreateOrderAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }


        //[Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (await _orderRepository.UpdateOrderAsync(order))
                {
                    return RedirectToAction(nameof(Index));

                }

            }
            return View(order);
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CustomerOrders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpGet("Order/OrderStatus/{consigmentNumber}")]
        public async Task<IActionResult> OrderStatus(string consigmentNumber)
        {
            var orderStatus = await _orderRepository.GetOrderStatusAsync(consigmentNumber);
            return View(orderStatus);
        }         
    }
}
