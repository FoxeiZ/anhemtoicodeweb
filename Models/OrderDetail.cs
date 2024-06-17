namespace anhemtoicodeweb.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public int ID { get; set; }
        public int? IDProduct { get; set; }
        public int? IDOrder { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }

        public virtual OrderPro OrderPro { get; set; }

        public virtual Product Product { get; set; }
    }
}
