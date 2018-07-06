using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using iMisho.API.Data;
using iMisho.API.Models;
using iMisho.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMisho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _http;
        private readonly ApplicationDbContext _context;

        public ProductsController(UserManager<ApplicationUser> userManager, IHttpContextAccessor http, ApplicationDbContext context)
        {
            _userManager = userManager;
            _http = http;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Error, some data is missing." });
            }

            var userId = _http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userId);

            var products = await _context.Products.Where(x => x.Owner == user).ToListAsync();

            var models = new List<ProductViewModel>();

            foreach (var product in products)
            {
                models.Add(new ProductViewModel
                {
                    Title= product.Title,
                    ShortDescription = product.ShortDescription,
                    LongDescription = product.LongDescription,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Category = product.Category
                });

            }

            return Ok(new { success = true, products = models });
        }


        [HttpPost("create")]
        public async Task<IActionResult> AddProduct([FromBody] ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Error, some data is missing." });
            }

            var userId = _http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userId);

            var product = new Product
            {
                Title = model.Title,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,
                Category = model.Category,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                Owner = user,
                OwnerId = user.Id
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, product.Id });
        }
    }
}