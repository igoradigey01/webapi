
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using EmailService;
using ShopApi.Dto;

namespace ShopApi.Controllers
{


    
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        private readonly IEmailSender _emailSender;

        public EmailController(
           IEmailSender emailSender
            )
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(EmailMessageDto mess)
        {    // {"кому"} ,"тема письма" ,"содержание письма"
            var message = new Message(new string[] { mess.To }, mess.Subject,mess.Content, null);
            _emailSender.SendEmail(message);

            return Ok();

        }
    }
}
