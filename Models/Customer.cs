namespace anhemtoicodeweb.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Customer")]
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            OrderProes = new HashSet<OrderPro>();
        }

        [Key]
        public int IDCus { get; set; }

        public string NameCus { get; set; }

        [StringLength(15)]
        public string PhoneCus { get; set; }

        public string EmailCus { get; set; }

        public string AddressName { get; set; }

        public string PasswordCus { get; set; }

        [NotMapped]
        public string ConfirmPasswordCus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderPro> OrderProes { get; set; }
    }
}
