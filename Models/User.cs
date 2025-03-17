namespace anhemtoicodeweb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Role = Enums.Role.Customer;
            OrderProes = new HashSet<OrderPro>();
            Products = new HashSet<Product>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string Name { get; set; }

        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Chức vụ")]
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

        [Display(Name = "Số điện thoại")]
        [StringLength(15)]
        public string Phone { get; set; }

        public string Email { get; set; }

        [Display(Name = "Địa chỉ")]
        public string AddressName { get; set; }


        [NotMapped]
        private string password;
        [NotMapped]
        public string Password
        {
            get => password; set
            {
                password = value;
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
                    HashPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                }
            }
        }

        [Display(Name = "Mật khẩu băm")]
        public string HashPassword { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderPro> OrderProes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
