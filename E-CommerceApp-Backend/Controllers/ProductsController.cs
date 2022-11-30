using AutoMapper;
using E_CommerceApp_Backend.DTOs;
using E_CommerceApp_Backend.Extensions;
using E_CommerceApp_Backend.Models;
using E_CommerceApp_Backend.RequestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace E_CommerceApp_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseApiController
    {
        private readonly ECommerceContext _context;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductsController(ECommerceContext context, IMapper mapper, IWebHostEnvironment webHostEnvironmenthe)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironmenthe;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProducts([FromQuery] ProductParams productParams)
        {
            //if (string.IsNullOrEmpty(searchTerm)) return await _context.Products.ToListAsync();

            var query = _context.Products
                .Sort(productParams.OrderBy) 
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types)
                .AsQueryable();

            var products = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

            Response.AddPaginationHeader(products.MetaData);

            return products;
        }

        [HttpGet("{id}", Name ="GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product= await _context.Products.FindAsync(id);
            if(product==null) return NotFound();
            return product;
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Products.Select(p=>p.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();

            return Ok(new {brands, types});
        }


        [Authorize(Roles = "Seller")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            if (productDto.File != null)
            {
                var name = Path.Combine(_webHostEnvironment.WebRootPath + "/images/products/", Path.GetFileName(productDto.File.FileName));
                await productDto.File.CopyToAsync(new FileStream(name, FileMode.Create));
                product.PictureUrl = Path.Combine("http://localhost:5000"+"/images/products/", productDto.File.FileName);
            }
            _context.Products.Add(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);

            return BadRequest(new ProblemDetails { Title = "Problem creating new product" });
        }


        [Authorize(Roles = "Seller")]
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct([FromForm] UpdateProductDto productDto)
        {
            var product = await _context.Products.FindAsync(productDto.Id);

            if (product == null) return NotFound();

            _mapper.Map(productDto, product);

            if (productDto.File != null)
            {
                var name = Path.Combine(_webHostEnvironment.WebRootPath + "/images/products/", Path.GetFileName(productDto.File.FileName));
                await productDto.File.CopyToAsync(new FileStream(name, FileMode.Create));
                product.PictureUrl = Path.Combine("http://localhost:5000" + "/images/products/", productDto.File.FileName);
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(product);

            return BadRequest(new ProblemDetails { Title = "Problem updating product" });
        }

        [Authorize(Roles = "Seller")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            //if (!string.IsNullOrEmpty(product.PublicId))
            //    await _imageService.DeleteImageAsync(product.PublicId);

            _context.Products.Remove(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting product" });
        }


    }
}
