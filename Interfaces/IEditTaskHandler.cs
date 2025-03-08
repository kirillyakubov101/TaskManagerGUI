using SharedModels;

namespace TaskManagerGUI.Interfaces;

public interface IEditTaskHandler
{
    Task<bool> EditTask(int  taskId, UserTaskEditDto userTaskEditDto);
}
