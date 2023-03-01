using MyInsta.Api.Models.Mail;
using MyInsta.Common.Services;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Pkcs;
using System.Collections.Generic;
using MyInsta.Api.Services;

namespace MyInsta.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly EmailService _mailService;
        public MailController(EmailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMailAsync(MailModel model)
        {
            bool result = await _mailService.SendEmailAsync(model, new CancellationToken());
            if (result)
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
        }

        [HttpPost("sendemailusingtemplate")]
        public async Task<IActionResult> SendEmailUsingTemplate(VerifiedMailModel model)
        {
            // Create MailData object
            MailModel mail = new(
                new List<string> { model.Email },
                "Welcome to the MailKit Demo",
                _mailService.GetEmailTemplate("welcome", model));


            bool sendResult = await _mailService.SendEmailAsync(mail, new CancellationToken());

            if (sendResult)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent using template.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }
    }
}
