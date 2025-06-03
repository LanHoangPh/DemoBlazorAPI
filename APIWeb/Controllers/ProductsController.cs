using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIWeb.Data;
using APIWeb.IRepository;
using Microsoft.Extensions.Hosting;

namespace APIWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryProduct _repositoryProduct;

        private readonly IWebHostEnvironment _environment;

        public ProductsController(IRepositoryProduct repositoryProduct, IWebHostEnvironment environment)
        {
            this._repositoryProduct = repositoryProduct;
            _environment = environment;
        }


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
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Không có file được chọn." });
            }

            // Validate kích thước file (tối đa 3MB)
            const long maxFileSize = 3 * 1024 * 1024; // 3MB
            if (file.Length > maxFileSize)
            {
                return BadRequest(new { message = "File quá lớn! Kích thước tối đa cho phép là 3MB." });
            }

            // Validate định dạng file (jpg, png, gif)
            var allowedExtensions = new[] { ".jpg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { message = "Định dạng file không hợp lệ! Chỉ chấp nhận .jpg, .png, .gif." });
            }

            // Tạo tên file duy nhất và lưu vào wwwroot/uploads
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadsDir = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            var filePath = Path.Combine(uploadsDir, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về đường dẫn của ảnh
            var fileUrl = $"/uploads/{fileName}";
            return Ok(new { message = "Upload thành công!", fileUrl });
        }
    }
}
