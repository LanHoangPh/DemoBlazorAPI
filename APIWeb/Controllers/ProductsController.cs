using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIWeb.Data;
using APIWeb.IRepository;

namespace APIWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryProduct _repositoryProduct;

        public ProductsController(IRepositoryProduct repositoryProduct)
        {
            this._repositoryProduct = repositoryProduct;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repositoryProduct.GetAllProducts();
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repositoryProduct.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != product.ID)
            {
                return BadRequest();
            }

            await _repositoryProduct.UpdateProduct(product);

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repositoryProduct.CreateProduct(product);

            return CreatedAtAction("GetProduct", new { id = product.ID }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _repositoryProduct.GetProductById(id);
            if (product == null)
                return NotFound();

            await _repositoryProduct.DeleteProduct(id);
            return NoContent();
        }
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
                return BadRequest("File không hợp lệ.");

            var fileName = Path.GetFileName(formFile.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/{fileName}";
            return Ok(new { FilePath = relativePath });
        }
    }
}
