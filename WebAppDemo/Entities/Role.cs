using System.ComponentModel.DataAnnotations;

    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
    }