using Microsoft.AspNetCore.Mvc;
using EStore.Application.Services;
using EStore.Application.Repositories;

namespace EStore.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SendEmailController : ControllerBase {

    // Fields

    private readonly IEmailService _emailService;
    private readonly IReadRoleRepository _readRoleRepository;
    private readonly IReadUserRepository _readUserRepository;

    // Constructor

    public SendEmailController(IEmailService emailService, IReadRoleRepository readRoleRepository, IReadUserRepository readUserRepository) {
        _emailService = emailService;
        _readRoleRepository = readRoleRepository;
        _readUserRepository = readUserRepository;
    }

    // Methods

    [HttpPost("[action]/{whom}")]
    public async Task<IActionResult> SendEmail(string whom,[FromQuery] string message, [FromQuery] string accesstoken) {
        var user = await _readUserRepository.GetUserByAccessToken(accesstoken);

        if (user == null)
            return BadRequest("Access Token is not proper");

        if (user.Role.RoleName == "SuperAdmin") {

            if (whom == "Admin" || whom == "Cashier" || whom == "Customer") {
                var admins = await _readRoleRepository.GetUsersByRoleName(whom);
                foreach(var admin in admins) {
                    var result = await _emailService.sendMailAsync(admin.Email, "Message from SuperAdmin", message, false);
                }
            }
            else {
                var users = await _readUserRepository.GetAllAsync();
                foreach (var item in users) {
                    var result = await _emailService.sendMailAsync(item.Email, "Message from SuperAdmin", message, false);
                }
            }
            return Ok("Email sent");
        }
        else return BadRequest("You don't have permission to this operation!");
    }
}
