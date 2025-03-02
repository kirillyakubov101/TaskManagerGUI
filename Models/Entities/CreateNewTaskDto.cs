using SharedModels;

namespace TaskManagerGUI.Models.Entities
{
    public class CreateNewTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public UserTaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
