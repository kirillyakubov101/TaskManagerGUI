namespace TaskManagerGUI.Interfaces;

public interface IDeleteTaskHander
{
    Task<bool> Delete(int taskId);
}
