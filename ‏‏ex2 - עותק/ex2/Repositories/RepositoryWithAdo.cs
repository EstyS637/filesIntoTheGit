using ex1.Models;
using ex1.Repositories;
using ex2.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Net.Mail;

namespace ex2.Repositories
{
    public class RepositoryWithAdo: IRepositoryWithAdo
    {
        string Cnn;
        public RepositoryWithAdo(IConfiguration configuration)
        {
            Cnn = configuration.GetConnectionString("DefaultConnection");
        }

        //ADO - EX4 - PROCEDURE
        public DataTable addAttachment(string Route, string AttachmentName, string Description, string Size, string EndingAttacment)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(Cnn))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    
                    command.CommandText = "Attachment_AddAttachment";
                    command.CommandType = CommandType.StoredProcedure;
                    
                    SqlParameter sqlParameter1 = new SqlParameter("@Route", Route);
                    SqlParameter sqlParameter2 = new SqlParameter("@AttachmentName", AttachmentName);
                    SqlParameter sqlParameter3 = new SqlParameter("@Description", Description);
                    SqlParameter sqlParameter4 = new SqlParameter("@Size", Size);
                    SqlParameter sqlParameter5 = new SqlParameter("@EndingAttacment", EndingAttacment);
                    command.Parameters.Add(sqlParameter1);
                    command.Parameters.Add(sqlParameter2);
                    command.Parameters.Add(sqlParameter3);
                    command.Parameters.Add(sqlParameter4);
                    command.Parameters.Add(sqlParameter5);

                    connection.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }


        //ADO - EX5
        public DataTable getTasksbyProjectId(int projectId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(Cnn))
            {
                using (SqlCommand command = new SqlCommand("select * from Tasks where ProjectId = @projectId", connection))
                {
                    command.CommandType = CommandType.Text;
                    SqlParameter sqlParameter1 = new SqlParameter("@ProjectId", projectId);
                    command.Parameters.Add(sqlParameter1);
                    connection.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }


        //transaction
        public bool transaction_AddingTaskAndAttachment(AttachmentAndTask attachmentAndTask)
        {
            using (SqlConnection connect = new SqlConnection(Cnn))
            {

                connect.Open();

                SqlTransaction transactionAddingAttachmentAndTasks = connect.BeginTransaction();

                try
                {
                    using (SqlCommand command = new SqlCommand("insert into Attachments (Route,Description,Size,EndingAttacment,AttachmentName)" + "values(@Route, @Description, @Size, @EndingAttacment, @AttachmentName)", connect, transactionAddingAttachmentAndTasks))
                    {
                        command.Parameters.AddWithValue("@Route", attachmentAndTask.attachment.Route);
                        command.Parameters.AddWithValue("@Description", attachmentAndTask.attachment.Description);
                        command.Parameters.AddWithValue("@Size", attachmentAndTask.attachment.Size);
                        command.Parameters.AddWithValue("@EndingAttacment", attachmentAndTask.attachment.EndingAttacment);
                        command.Parameters.AddWithValue("@AttachmentName", attachmentAndTask.attachment.AttachmentName);
                        command.ExecuteNonQuery();


                    }
                    

                    using (SqlCommand command = new SqlCommand("insert into Tasks (Priority,DueDate,Status,ProjectId,UserId)" + "values(@Priority, @DueDate, @Status, @ProjectId, @UserId)", connect, transactionAddingAttachmentAndTasks))
                    {
                        command.Parameters.AddWithValue("@Priority", attachmentAndTask.task.Priority);
                        command.Parameters.AddWithValue("@DueDate", attachmentAndTask.task.DueDate);
                        command.Parameters.AddWithValue("@Status", attachmentAndTask.task.Status);
                        command.Parameters.AddWithValue("@ProjectId", attachmentAndTask.task.ProjectId);
                        command.Parameters.AddWithValue("@UserId", attachmentAndTask.task.UserId);
                        command.ExecuteNonQuery();
                    }

                    transactionAddingAttachmentAndTasks.Commit();
                    return true;
                }

                catch
                {
                    transactionAddingAttachmentAndTasks.Rollback();
                    return false;
                }







            }
        }
    }
}
