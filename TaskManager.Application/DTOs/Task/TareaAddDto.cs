
namespace TaskManager.Application.DTOs.Task
{
    public record class TareaAddDto
    {
        public string Description { get; init; }
        public DateOnly DueDate { get; init; }
        public string Status { get; init; }
        public string AditionalData { get; init; }
        public bool Active  { get; init; }
        public int DaysLeft { get; init; }

    }
}
