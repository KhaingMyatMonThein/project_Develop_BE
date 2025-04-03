using Microsoft.AspNetCore.Mvc;
using PD.Services;
using System;
using System.Threading.Tasks;

namespace PD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly DialogflowService _dialogflowService;

        public ChatController(DialogflowService dialogflowService)
        {
            _dialogflowService = dialogflowService;
        }

        [HttpPost("dialogflow")]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            try
            {
                string response = await _dialogflowService.DetectIntentAsync(message);
                return Ok(new { response });
            }
            catch (Exception ex)
            {  
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }

}

