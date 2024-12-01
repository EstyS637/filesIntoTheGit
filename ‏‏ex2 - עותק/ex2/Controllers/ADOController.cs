using ex1.Services;
using ex2.Models;
using ex2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ex2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ADOController : ControllerBase
    {
        private readonly IServiceWithAdo _IServiceWithAdo;
        public ADOController(IServiceWithAdo iServiceWithAdo)
        {
            _IServiceWithAdo = iServiceWithAdo;
        }

        //ADO - EX4 - PROCEDURE
        [HttpPost]
        public IActionResult addAttachment(string Route, string AttachmentName, string Description, string Size, string EndingAttacment)
        {
            return (IActionResult)_IServiceWithAdo.addAttachment(Route, AttachmentName, Description, Size, EndingAttacment);
        }

        //ADO - EX5
        [HttpGet("{ProjectId}")]

        public IActionResult getTasksbyProjectId(int ProjectId)
        {
            DataTable res= _IServiceWithAdo.getTasksbyProjectId(ProjectId);
            if (res != null)
                return Ok(res);
            return BadRequest();

        }


        //transaction
        [Route("api/Tasks/transaction_AddingTaskAndAttachment")]
        [HttpPost]
        public ActionResult<bool> transaction_AddingTaskAndAttachment([FromBody] AttachmentAndTask attachmentAndTask)
        {
            return _IServiceWithAdo.transaction_AddingTaskAndAttachment(attachmentAndTask);
        }

    }
}
