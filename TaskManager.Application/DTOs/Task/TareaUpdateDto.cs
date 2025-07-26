

namespace TaskManager.Application.DTOs.Task
{
    public record class TareaUpdateDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateOnly DueDate { get; set; }
        public string Status { get; set; }
        public string AditionalData { get; set; }
        public bool Active { get; set; }

    }
}
