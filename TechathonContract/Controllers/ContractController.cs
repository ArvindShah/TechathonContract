using BusinessLayer;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using TechathonContract.Models;

namespace TechathonContract.Controllers
{
    public class ContractController : Controller
    {
        private IBAO _BAO;
        UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ContractController> _logger;

        public ContractController(IBAO BAO, ILogger<ContractController> logger, UserManager<ApplicationUser> userManager)
        {
            _BAO = BAO;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("contract/{clientId}")]
        public IActionResult GetClientRequestParameters([FromRoute] string clientId)
        {
            //var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok("as");
        }

        [HttpPost("contract/UploadFiles")]
        public IActionResult UploadFiles()
        {
            IFormFile formFile = HttpContext.Request.Form.Files[0];
            if (formFile == null)
                return BadRequest(new { message = "No file was received by server." });

            if (formFile.Length == 0)
                return BadRequest(new { message = "File size was 0." });

            Stream stream = new MemoryStream();
            formFile.CopyTo(stream);
            stream.Seek(0, SeekOrigin.Begin);
            _BAO.UploadFileToDatalake(stream, formFile.FileName, 12, 13);
            //var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok("Successful");
        }
    }
}
