namespace anhemtoicodeweb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Price = 0;
            ImagePro = "~/Image/Product/CuonTuiRac.jpg";
        }

        public int ProductID { get; set; }

        public string NamePro { get; set; }

        public int InvQuantity { get; set; }

        public string DecriptionPro { get; set; }

        [StringLength(20)]
        public string IDCate { get; set; }

        public decimal? Price { get; set; }

        public string ImagePro { get; set; }

        [NotMapped]
        public string OldImagePro { get; set; }

        [NotMapped]
        public HttpPostedFileBase UploadImage {  get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
