namespace anhemtoicodeweb.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;
    using System.Web.Mvc;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Name = "None";
            Decription = "None";

            Quantity = 0;
            Price = 0;
            Discount = 0;
            Tax = 0;

            ImageLocation = "~/Image/Product/banphim.png";
        }

        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        [AllowHtml]
        public string Decription { get; set; }

        [Column("State")]
        [Required]
        public virtual int StateEnumId { get; set; }
        [NotMapped]
        [EnumDataType(typeof(Enums.ProductState))]
        public Enums.ProductState State
        {
            get
            {
                return (Enums.ProductState)this.StateEnumId;
            }
            set
            {
                this.StateEnumId = (int)value;
            }
        }

        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal? FinalPrice { get; set; }

        public string ImageLocation { get; set; }
        [NotMapped]
        public string OldImageLocation { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }

        [StringLength(20)]
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int? SellerId { get; set; }
        public virtual User Seller { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
