using EStore.Domain.DTO;
using EStore.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;

namespace EStore.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceItemController : ControllerBase {

    // Fields

    private readonly IReadUserRepository _readUserRepository;
    private readonly IReadProductRepository _readProductRepository;
    private readonly IReadInvoiceRepository _readInvoiceRepository;
    private readonly IReadInvoiceItemRepository _readInvoiceItemRepository;
    private readonly IWriteInvoiceItemRepository _writeInvoiceItemRepository;

    // Constructor

    public InvoiceItemController(IReadInvoiceItemRepository readInvoiceItemRepository, IWriteInvoiceItemRepository writeInvoiceItemRepository, IReadUserRepository readUserRepository, IReadProductRepository readProductRepository, IReadInvoiceRepository readInvoiceRepository) {
        _readUserRepository = readUserRepository;
        _readProductRepository = readProductRepository;
        _readInvoiceRepository = readInvoiceRepository;
        _readInvoiceItemRepository = readInvoiceItemRepository;
        _writeInvoiceItemRepository = writeInvoiceItemRepository;
    }

    // Methods

    #region AddInvoiceItem

    [HttpPost("[action]")]
    public async Task<IActionResult> AddInvoiceItem([FromQuery] string accesstoken, [FromBody] InvoiceItemDTO invoiceItemDTO) {
        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var product = await _readProductRepository.GetByIdAsync(invoiceItemDTO.ProductId);
            if (product == null)
                return BadRequest("Product was not found");

            var invoice = await _readInvoiceRepository.GetByIdAsync(invoiceItemDTO.InvoiceId);
            if (invoice == null)
                return BadRequest("Invoice was not found");

            var newInvoiceItem = new InvoiceItem() {
                Quantity = invoiceItemDTO.Quantity,
                ProductId = invoiceItemDTO.ProductId,
                InvoiceId = invoiceItemDTO.InvoiceId
            };

            await _writeInvoiceItemRepository.AddAsync(newInvoiceItem);

            return Ok("Invoice item added successfully");
        }
        else return Unauthorized("You don't have access to this operation!");
    }
    #endregion

    #region GetAll

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] string accesstoken) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var result = await _readInvoiceItemRepository.GetAllAsync();
            if (result == null)
                return NotFound("Invoice item was not found");

            var invoiceItems = result.ToList();
            var allInvoiceItemsVM = invoiceItems.Select(p => new GetInvoiceItemVM() {
                Quantity = p.Quantity,
                Product = new GetProductVM() { Name = p.Product.Name, CategoryName = p.Product.Category.Name, Description = p.Product.Description, ImageUrl = p.Product.ImageUrl, Price = p.Product.Price },
                Invoice = new GetInvoiceVM() { Barcode = p.Invoice.Barcode, InvoiceBarcode = p.Invoice.InvoiceBarcode, InvoiceType = p.Invoice.InvoiceType.ToString() }
            }).ToList();

            return Ok(allInvoiceItemsVM);
        }
        else return Unauthorized("You don't have access to this operation!");
    }

    #endregion

    #region GetInvoiceItemById

    [HttpGet("GetInvoiceItemById/{id}")]
    public async Task<IActionResult> GetInvoiceItemById(int id, [FromQuery] string accesstoken) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var result = await _readInvoiceItemRepository.GetAllAsync();
            if (result == null)
                return NotFound("InvoiceItems not found");

            var invoiceItem = result.ToList().FirstOrDefault(p => p.Id == id);

            if (invoiceItem == null)
                return NotFound("Invoice Item was not found");

            var invoiceItemVM = new GetInvoiceItemVM() {
                Quantity = invoiceItem.Quantity,
                Product = new GetProductVM() { Name = invoiceItem.Product.Name, CategoryName = invoiceItem.Product.Category.Name, Description = invoiceItem.Product.Description, ImageUrl = invoiceItem.Product.ImageUrl, Price = invoiceItem.Product.Price },
                Invoice = new GetInvoiceVM() { Barcode = invoiceItem.Invoice.Barcode, InvoiceBarcode = invoiceItem.Invoice.InvoiceBarcode, InvoiceType = invoiceItem.Invoice.InvoiceType.ToString() }
            };
            return Ok(invoiceItemVM);
        }
        else return Unauthorized("You don't have access to this operation!");
    }

    #endregion

    #region Delete

    [HttpDelete("DeleteInvoiceItem")]
    public async Task<IActionResult> DeleteInvoiceItem([FromQuery] string accesstoken, [FromBody] int id) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var invoiceitem = _readInvoiceItemRepository.GetByIdAsync(id);
            if (invoiceitem == null)
                return BadRequest($"There is not any invoice item with {id} id.");

            await _writeInvoiceItemRepository.DeleteAsync(id);
            return Ok("Invoice Item was deleted");
        }
        else return Unauthorized();
    }

    #endregion

    #region UpdateInvoiceItem

    [HttpPut("UpdateInvoiceItem/{id}")]
    public async Task<IActionResult> UpdateInvoiceItem(int id, [FromQuery] string accesstoken, [FromBody] InvoiceItemDTO invoiceItemDTO) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var invoiceItem = await _readInvoiceItemRepository.GetByIdAsync(id);
            if (invoiceItem == null)
                return NotFound("Invoice Item not found");

            invoiceItem.Quantity = invoiceItemDTO.Quantity;
            invoiceItem.ProductId = invoiceItemDTO.ProductId;
            invoiceItem.InvoiceId = invoiceItemDTO.InvoiceId;

            await _writeInvoiceItemRepository.UpdateAsync(invoiceItem);

            return Ok("Invoice Item updated successfully");
        }
        else return Unauthorized("You don't have access to this operation!");
    }
    #endregion
}
