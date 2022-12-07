using AutoMapper;
using E_CommerceApp_Backend.Authentication;
using E_CommerceApp_Backend.DTOs;
using E_CommerceApp_Backend.Extensions;
using E_CommerceApp_Backend.Models;
using E_CommerceApp_Backend.RequestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceApp_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseApiController
    {
        private readonly ECommerceContext _context;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(ECommerceContext context, IMapper mapper, IWebHostEnvironment webHostEnvironmenthe, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironmenthe;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProducts([FromQuery] ProductParams productParams)
        {

            var query = _context.Products
                .Sort(productParams.OrderBy) 
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types, productParams.SellerNameList)
                .AsQueryable();

            var products = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

            Response.AddPaginationHeader(products.MetaData);

            return products;
        }

        //[Authorize]
        [HttpGet("{id}", Name ="GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product= await _context.Products.FindAsync(id);
            product.ViewsCounter++;
            await _context.SaveChangesAsync();



            if (product==null) return NotFound();
            return product;
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Products.Select(p=>p.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();
            var sellerNameList = await _context.Products.Select(p => p.SellerName).Distinct().ToListAsync();

            return Ok(new {brands, types, sellerNameList});
        }
        [HttpGet("newProducts")]
        public async Task<ActionResult<List<NewProduct>>> GetNewProducts(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return await _context.NewProducts.ToListAsync();
            }

            var user =  _context.Users.First(u => u.Email.Equals(email));
            var products = await _context.NewProducts.Where(u => u.UserId == user.Id).ToListAsync();
            return  products;
        }

        [Authorize(Roles = "Seller")]
        [HttpPost("createProducts")]
        public async Task<ActionResult<NewProduct>> CreateProduct([FromForm] CreateProductDto productDto)
        {
            var newProduct = _mapper.Map<NewProduct>(productDto);

            if (productDto.File != null)
            {
                Random rnd = new Random();
                int num = rnd.Next();
                var name = Path.Combine(_webHostEnvironment.WebRootPath + "/images/products/", num.ToString()+Path.GetFileName(productDto.File.FileName));
                await productDto.File.CopyToAsync(new FileStream(name, FileMode.Create));
                newProduct.PictureUrl = Path.Combine("http://localhost:5000"+"/images/products/", num.ToString() + productDto.File.FileName);
            }

            newProduct.User = await _userManager.FindByNameAsync(User.Identity.Name);
            newProduct.UserId = newProduct.User.Id;
            newProduct.SellerName = newProduct.User.UserName;
            _context.NewProducts.Add(newProduct);
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetProduct", new { Id = newProduct.Id }, newProduct);

            return BadRequest(new ProblemDetails { Title = "Problem creating new product" });
        }


        [Authorize(Roles = "Seller")]
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct([FromForm] UpdateProductDto productDto)
        {
            var product = await _context.NewProducts.FindAsync(productDto.Id);

            if (product == null) return NotFound();

            _mapper.Map(productDto, product);

            if (productDto.File != null)
            {
                Random rnd = new Random();
                int num = rnd.Next();
                var name = Path.Combine(_webHostEnvironment.WebRootPath + "/images/products/", num.ToString() + Path.GetFileName(productDto.File.FileName));
                await productDto.File.CopyToAsync(new FileStream(name, FileMode.Create));
                product.PictureUrl = Path.Combine("http://localhost:5000" + "/images/products/", num.ToString() + productDto.File.FileName);
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(product);

            return BadRequest(new ProblemDetails { Title = "Problem updating product" });
        }

        [Authorize(Roles = "Seller")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.NewProducts.FindAsync(id);

            if (product == null) return NotFound();


            _context.NewProducts.Remove(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting product" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("approveNewProducts")]
        public async Task<ActionResult<Product>> ApproveProducts(bool response, int id)
        {
            var newProduct = _context.NewProducts.FirstOrDefault(i => i.Id == id);

            if (!response)
            {
                _context.NewProducts.Remove(newProduct);
                await _context.SaveChangesAsync();
                return Ok();
            }

            var product = _mapper.Map<Product>(newProduct);
            _context.Products.Add(product);
            var result = await _context.SaveChangesAsync() > 0;

            _context.NewProducts.Remove(newProduct);
            await _context.SaveChangesAsync();

            if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);

            return BadRequest(new ProblemDetails { Title = "There was an error while approving the product, please try again!" });
        }
    }
}
