using EStore.Domain.DTO;
using EStore.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;

namespace EStore.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase {

    // Fields

    private readonly IReadUserRepository _readUserRepository;
    private readonly IReadCategoryRepository _readCategoryRepository;
    private readonly IWriteCategoryRepository _writeCategoryRepository;

    // Constructor

    public CategoryController(IReadCategoryRepository readCategoryRepository, IWriteCategoryRepository writeCategoryRepository, IReadUserRepository readUserRepository) { 
        _readUserRepository = readUserRepository;
        _readCategoryRepository = readCategoryRepository;
        _writeCategoryRepository = writeCategoryRepository;
    }

    // Methods

    #region AddCategory

    [HttpPost("[action]")]
    public async Task<IActionResult> AddCategory([FromQuery] string accesstoken, [FromBody] CategoryDTO categoryDTO) {
        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var newCategory = new Category() {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description
            };

            await _writeCategoryRepository.AddAsync(newCategory);

            return Ok("Category added successfully");
        }
        else return Unauthorized("You don't have access to this operation!");
    }
    #endregion

    #region GetAll

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() {
        var result = await _readCategoryRepository.GetAllAsync();
        if (result == null)
            return NotFound("Category was not found");

        var categories = result.ToList();
        var allCategoriesVM = categories.Select(p => new GetCategoryVM() {
            Name = p.Name,
            Description = p.Description
        }).ToList();

        return Ok(allCategoriesVM);
    }

    #endregion

    #region GetCategoryById

    [HttpGet("GetCategoryById/{id}")]
    public async Task<IActionResult> GetCategoryById(int id, [FromQuery] string accesstoken) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var result = await _readCategoryRepository.GetAllAsync();
            if (result == null)
                return NotFound("Category not found");

            var category = result.ToList().FirstOrDefault(p => p.Id == id);
            var categoryVM = new GetCategoryVM() {
                Name = category.Name,
                Description = category.Description
            };
            return Ok(categoryVM);
        }
        else return Unauthorized("You don't have access to this operation!");
    }

    #endregion

    #region Delete

    [HttpDelete("DeleteCategory")]
    public async Task<IActionResult> DeleteCategory([FromQuery] string accesstoken, [FromBody] int id) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {
            await _writeCategoryRepository.DeleteAsync(id);
            return Ok("Category was deleted");
        }
        else return Unauthorized();
    }

    #endregion

    #region UpdateCategory

    [HttpPut("UpdateCategory/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromQuery] string accesstoken, [FromBody] CategoryDTO categoryDTO) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var category = await _readCategoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound("Category not found");

            category.Name = categoryDTO.Name;
            category.Description = categoryDTO.Description;

            await _writeCategoryRepository.UpdateAsync(category);

            return Ok("Category updated successfully");
        }
        else return Unauthorized("You don't have access to this operation!");
    }
    #endregion
}
