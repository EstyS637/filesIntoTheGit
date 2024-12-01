using ex1.Repositories;
using ex2.Models;
using ex2.Repositories;
using System.Data;

namespace ex2.Services
{
    public class ServiceWithAdo:IServiceWithAdo
    {
        private readonly IRepositoryWithAdo _IRepositoryWithAdo;

        public ServiceWithAdo(IRepositoryWithAdo iRepositoryWithAdo)
        {
            _IRepositoryWithAdo = iRepositoryWithAdo;
        }

        //ADO - EX4 - PROCEDURE
        public DataTable addAttachment(string Route, string AttachmentName, string Description, string Size, string EndingAttacment)
        {
            return _IRepositoryWithAdo.addAttachment(Route, AttachmentName, Description, Size, EndingAttacment);
        }

        //ADO - EX5
        public DataTable getTasksbyProjectId(int ProjectId)
        {
            return _IRepositoryWithAdo.getTasksbyProjectId(ProjectId);
        }


        //transaction
        public bool transaction_AddingTaskAndAttachment(AttachmentAndTask attachmentAndTask)
        {
            return _IRepositoryWithAdo.transaction_AddingTaskAndAttachment(attachmentAndTask);
        }
    }

}
