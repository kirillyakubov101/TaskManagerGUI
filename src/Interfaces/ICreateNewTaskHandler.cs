using TaskManagerGUI.Models.Entities;

namespace TaskManagerGUI.Interfaces
{
    public interface ICreateNewTaskHandler
    {
        Task<bool> CreateNewTask(CreateNewTaskDto createNewTaskDto);
    }
}
