
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Domain.Entities
{
    [Table("Tarea", Schema = "TaskManager")]
    public class Tarea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }
        public string Description { get; set; }
        public DateOnly DueDate { get; set; } 
        public string Status {  get; set; }
        public  string AditionalData { get; set; }
        public bool Active { get; set; }




    }
}
