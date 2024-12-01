using ex2.Models;
using System.Data;

namespace ex2.Services
{
    public interface IServiceWithAdo
    {
        //ADO - EX4 - PROCEDURE
        DataTable addAttachment(string Route, string AttachmentName, string Description, string Size, string EndingAttacment);


        //ADO - EX5
       DataTable getTasksbyProjectId(int ProjectId);


        //transaction
        bool transaction_AddingTaskAndAttachment(AttachmentAndTask attachmentAndTask);
    }
}
