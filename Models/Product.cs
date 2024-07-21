namespace anhemtoicodeweb.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            NamePro = "None";
            DecriptionPro = "None";

            InvQuantity = 0;
            Price = 0;
            Discount = 0;
            Tax = 0;

            ImagePro = "~/Image/Product/banphim.png";
        }

        public int ProductID { get; set; }

        public string NamePro { get; set; }

        public int InvQuantity { get; set; }

        public string DecriptionPro { get; set; }

        [StringLength(20)]
        public string IDCate { get; set; }

        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal FinalPrice { get; set; }

        public string ImagePro { get; set; }

        [NotMapped]
        public string OldImagePro { get; set; }

        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
