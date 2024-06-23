using EStore.Domain.DTO;
using EStore.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;

namespace EStore.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase {

    // Fields

    private readonly IReadUserRepository _readUserRepository;
    private readonly IReadProductRepository _readProductRepository;
    private readonly IReadCategoryRepository _readCategoryRepository;
    private readonly IWriteProductRepository _writeProductRepository;

    // Constructor

    public ProductController(IReadProductRepository readProductRepository, IWriteProductRepository writeProductRepository, IReadUserRepository readUserRepository, IReadCategoryRepository readCategoryRepository) {
        _readUserRepository = readUserRepository;
        _readProductRepository = readProductRepository;
        _writeProductRepository = writeProductRepository;
        _readCategoryRepository = readCategoryRepository;
    }

    // Methods

    #region AddProduct

    [HttpPost("[action]")]
    public async Task<IActionResult> AddProduct([FromQuery] string accesstoken, [FromBody] ProductDTO productDTO) {
        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var category = await _readCategoryRepository.GetCategoryByName(productDTO.Category);

            if (category == null)
                return BadRequest("Category was not found");

            var newProduct = new Product() {
                Name = productDTO.Name,
                Price = productDTO.Price,
                ImageUrl = productDTO.ImageUrl,
                Description = productDTO.Description,
                CategoryId = category.Id
            };

            await _writeProductRepository.AddAsync(newProduct);

            return Ok("Product added successfully");
        }
        else return Unauthorized("You don't have access to this operation!");
    }
    #endregion

    #region GetAll

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() {
        var result = await _readProductRepository.GetAllAsync();
        if (result == null)
            return NotFound("Product not found");

        var products = result.ToList();
        var allProductVm = products.Select(p => new GetProductVM() {
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            CategoryName = p.Category.Name,
            ImageUrl = p.ImageUrl
        }).ToList();

        return Ok(allProductVm);
    }

    #endregion

    #region GetProductByCategoryName

    [HttpGet("GetProductByCategoryName/{name}")]
    public async Task<IActionResult> GetProductByCategoryName(string name) {

        var result = await _readCategoryRepository.GetAllProductsByName(name);
        if (result == null)
            return NotFound("Product not found");

        var products = result.ToList();
        var allProductVm = products.Select(p => new GetProductVM() {
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            CategoryName = p.Category.Name,
            ImageUrl = p.ImageUrl
        }).ToList();

        return Ok(allProductVm);
    }

    #endregion

    #region GetProductById

    [HttpGet("GetProductById/{id}")]
    public async Task<IActionResult> GetProductById(int id, [FromQuery] string accesstoken) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var result = await _readProductRepository.GetAllAsync();
            if (result == null)
                return NotFound("Product not found");

            var product = result.ToList().FirstOrDefault(p => p.Id == id);
            var productVM = new GetProductVM() {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryName = product.Category.Name,
                ImageUrl = product.ImageUrl
            };
            return Ok(productVM);
        }
        else return Unauthorized("You don't have access to this operation!");
    }

    #endregion

    #region Delete

    [HttpDelete("DeleteProduct")]
    public async Task<IActionResult> DeleteProduct([FromQuery] string accesstoken, [FromBody] int id) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {
            await _writeProductRepository.DeleteAsync(id);
            return Ok("Product was deleted");
        }
        else return Unauthorized();
    }

    #endregion

    #region UpdateProduct

    [HttpPut("UpdateProduct/{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromQuery] string accesstoken, [FromBody] UpdateProductVM productVM) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var product = await _readProductRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound("Product not found");

            var category = await _readCategoryRepository.GetByIdAsync(productVM.CategoryId);
            if (category == null)
                return NotFound("Category not found");

            product.Name = productVM.Name;
            product.Price = productVM.Price;
            product.Description = productVM.Description;
            product.CategoryId = productVM.CategoryId;

            await _writeProductRepository.UpdateAsync(product);

            return Ok("Product updated successfully");
        }
        else return Unauthorized("You don't have access to this operation!");
    }
    #endregion
}
