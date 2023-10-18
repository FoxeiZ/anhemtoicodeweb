using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace anhemtoicodeweb.Models
{
    public class Products
    {
        [Key]
        [Column(Order = 1)]
        public int ProductId {  get; set; }

        [StringLength(255)]
        public string ProductName {  get; set; }

        public string ProductDescription { get; set; }

        [Range(0, 255)]
        public decimal ? ProductPrice { get; set; }

        public int CategoryId { get; set; }

        public virtual Categories Categories { get; set; }
    }
}