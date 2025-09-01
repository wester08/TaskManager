

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Domain.Entities
{
    [Table("Users", Schema = "Security")]
    public class User 
    {
        [Key]
        public int IdUser { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Password { get; set; }

        public int RolId { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserCreateId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UserUpdateId { get; set; }

        public bool Active { get; set; }

    }
}
