using SharedModels;

namespace TaskManagerGUI.Interfaces;

public interface ILoginEnterHandler
{
    Task<IEnumerable<UserTaskDto>> GetAllUserTasks();
    Task<UserInfoDto> GetUserInfo();
}
