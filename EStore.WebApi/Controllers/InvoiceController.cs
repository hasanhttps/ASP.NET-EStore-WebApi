using EStore.Domain.DTO;
using EStore.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EStore.Application.Repositories;
using EStore.Domain.Entities.Concretes;

namespace EStore.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase {

    // Fields

    private readonly IReadUserRepository _readUserRepository;
    private readonly IReadInvoiceRepository _readInvoiceRepository;
    private readonly IWriteInvoiceRepository _writeInvoiceRepository;

    // Constructor

    public InvoiceController(IReadInvoiceRepository readInvoiceRepository, IWriteInvoiceRepository writeInvoiceRepository, IReadUserRepository readUserRepository) {
        _readUserRepository = readUserRepository;
        _readInvoiceRepository = readInvoiceRepository;
        _writeInvoiceRepository = writeInvoiceRepository;
    }

    // Methods

    #region AddInvoice

    [HttpPost("[action]")]
    public async Task<IActionResult> AddInvoice([FromQuery] string accesstoken, [FromBody] InvoiceDTO invoiceDTO) {
        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var cashier = await _readUserRepository.GetByIdAsync(invoiceDTO.CashierId);
            if (cashier == null && cashier.Role.RoleName != "Customer")
                return BadRequest("Cashier was not found");

            var customer = await _readUserRepository.GetByIdAsync(invoiceDTO.CustomerId);
            if (customer == null && cashier.Role.RoleName == "Customer")
                return BadRequest("Customer was not found");

            var newInvoice = new Invoice() {
                Barcode = invoiceDTO.Barcode,
                InvoiceBarcode = invoiceDTO.InvoiceBarcode,
                InvoiceType = invoiceDTO.InvoiceType,
                CashierId = invoiceDTO.CashierId,
                CustomerId = invoiceDTO.CustomerId
            };

            await _writeInvoiceRepository.AddAsync(newInvoice);

            return Ok("Invoice added successfully");
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

            var result = await _readInvoiceRepository.GetAllAsync();
            if (result == null)
                return NotFound("Invoice was not found");

            var invoices = result.ToList();
            var allInvoicesVM = invoices.Where(p => p.IsDeleted == false).Select(p => new GetInvoiceVM() {
                Barcode = p.Barcode,
                InvoiceBarcode = p.InvoiceBarcode,
                InvoiceType = p.InvoiceType.ToString()
            }).ToList();

            return Ok(allInvoicesVM);
        }
        else return Unauthorized("You don't have access to this operation!");
    }

    #endregion

    #region GetInvoiceById

    [HttpGet("GetInvoiceById/{id}")]
    public async Task<IActionResult> GetInvoiceById(int id, [FromQuery] string accesstoken) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var result = await _readInvoiceRepository.GetAllAsync();
            if (result == null)
                return NotFound("Invoices not found");

            var invoice = result.ToList().Where(p => p.IsDeleted == false).FirstOrDefault(p => p.Id == id);

            if (invoice == null)
                return NotFound("Invoice was not found");

            var invoiceItemVM = new GetInvoiceVM() {
                Barcode = invoice.Barcode,
                InvoiceBarcode = invoice.InvoiceBarcode,
                InvoiceType = invoice.InvoiceType.ToString()
            };
            return Ok(invoiceItemVM);
        }
        else return Unauthorized("You don't have access to this operation!");
    }

    #endregion

    #region Delete

    [HttpDelete("DeleteInvoice")]
    public async Task<IActionResult> DeleteInvoice([FromQuery] string accesstoken, [FromBody] int id) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var invoice = await _readInvoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                return BadRequest($"There is not any invoice with {id} id.");

            invoice.IsDeleted = true;
            await _writeInvoiceRepository.UpdateAsync(invoice);
            return Ok("Invoice Item was deleted");
        }
        else return Unauthorized();
    }

    #endregion

    #region UpdateInvoice

    [HttpPut("UpdateInvoice/{id}")]
    public async Task<IActionResult> UpdateInvoice(int id, [FromQuery] string accesstoken, [FromBody] InvoiceDTO invoiceDTO) {

        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "Admin" || user.Role.RoleName == "SuperAdmin" || user.Role.RoleName == "Cashier") {

            var invoice = await _readInvoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                return NotFound("Invoice was not found");

            invoice.Barcode = invoiceDTO.Barcode;
            invoice.InvoiceBarcode = invoiceDTO.InvoiceBarcode;
            invoice.InvoiceType = invoiceDTO.InvoiceType;
            invoice.CashierId = invoiceDTO.CashierId;
            invoice.CustomerId = invoiceDTO.CustomerId;

            await _writeInvoiceRepository.UpdateAsync(invoice);

            return Ok("Invoice updated successfully");
        }
        else return Unauthorized("You don't have access to this operation!");
    }
    #endregion
}
