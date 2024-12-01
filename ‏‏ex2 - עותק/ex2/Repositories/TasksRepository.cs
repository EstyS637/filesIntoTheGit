using ex1.Models;
using ex2.Models;
using ex2.Repositories;
using ex2.Services.Logger;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.Json;
using TasksApi.Services.Logger;

namespace ex1.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private readonly DBLoggerService _dBLoggerService;
        private string _filePath = "tasks.json";
        private readonly TasksdbContext _context;
        private readonly TasksApi.Services.Logger.LoggerFactory loggerFactory;
        private ILoggerService logger;

       



        public TasksRepository(TasksdbContext context, DBLoggerService dBLoggerService, TasksApi.Services.Logger.LoggerFactory loggerFactory1)
        {
            _context = context;
            _dBLoggerService = dBLoggerService;
            loggerFactory = loggerFactory1;

        }
      


        public List<Tasks> GetAllTasks()
        {
            return _context.Tasks.ToList();
        }

        public Tasks GetTaskById(int Id)
        {
            return _context.Tasks.Find(Id);
        }

        public Tasks? CreateNewTask(Tasks Task)
        {
            User? isExistUser = _context.Users.ToList().FirstOrDefault(u => u.UserId == Task.UserId);
            Project? isExistProject = _context.Projects.ToList().FirstOrDefault(p => p.ProjectId == Task.ProjectId);

            if(isExistUser!=null && isExistProject!=null){
                _context.Tasks.Add(Task);
                _context.SaveChanges();
                logger = loggerFactory.GetLogger(1);
                logger.Log("the task created successfully");
                return Task;
            }
            else
                return null;
        }
        public void DeleteTaskById(int id)
        {
            Tasks? task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }

        public void UpdateTask(Tasks task)
        {
            try
            {
                 _context.Tasks.Update(task);
                   _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<Tasks>? GetTasksOfUserByUserName(string UserName)
        {
            User? currUser = _context.Users.FirstOrDefault(u => u.UserName == UserName);
            if (currUser != null)
            {
                List<Tasks>? tasksOfUser = _context.Tasks.Where(t => t.UserId == currUser.UserId).ToList();
                return tasksOfUser;
            }
            return null;
        }

        public void logIntoDB(string message)
        {
            Messages newMessage = new Messages();
            newMessage.Description = message;
            newMessage.Update_Date = DateTime.Now;
            _context.Messages.Add(newMessage);
        }




    }
}
