namespace anhemtoicodeweb.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            OrderProes = new HashSet<OrderPro>();
            Products = new HashSet<Product>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        [Column("Role")]
        [Required]
        public virtual int RoleEnumId { get; set; }

        [NotMapped]
        [EnumDataType(typeof(Enums.Role))]
        public Enums.Role Role
        {
            get
            {
                return (Enums.Role)this.RoleEnumId;
            }
            set
            {
                this.RoleEnumId = (int)value;
            }
        }

        [StringLength(15)]
        public string Phone { get; set; }

        public string Email { get; set; }

        public string AddressName { get; set; }

        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderPro> OrderProes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
