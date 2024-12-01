using ex1.Repositories;
using TasksApi.Services.Logger;

namespace ex2.Services.Logger
{
    public class DBLoggerService: ILoggerService
    {
       
        private readonly ITasksRepository _ITasksRepository;

        public DBLoggerService(ITasksRepository ITasksRepository)
        {
            _ITasksRepository = ITasksRepository;
        }

        public void Log(string message)
        {
            try
            {
                _ITasksRepository.logIntoDB(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"failed to log message: {ex.Message}");
            }
        }
    }
}
