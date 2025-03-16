namespace anhemtoicodeweb.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Discount")]
    public partial class Discount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Discount()
        {
            //NameDiscount;
            //CodeDiscount
            ValueDiscount = 0;
            Unit = "percent";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            //Description = String.Empty;
        }

        [Key]
        public int DiscountID { get; set; }

        public string Owner { get; set; }

        [Display(Name = "Tên")]
        public string NameDiscount { get; set; }

        [Display(Name = "Loại giảm giá")]
        [DefaultValue("product")]
        [RegularExpression("^(product|code|other)$", ErrorMessage = "TypeDiscount must be 'product', 'code', or 'other'.")]
        public string TypeDiscount { get; set; }

        [Display(Name = "Mã giảm giá")]
        [Required(ErrorMessage = "CodeDiscount must not be null")]
        public string CodeDiscount { get; set; }

        [Display(Name = "Giá trị mã giảm")]
        [Required]
        public decimal ValueDiscount { get; set; }

        [Display(Name = "Đơn vị mã giảm")]
        [Required]
        [RegularExpression("^(percent|fixed)$", ErrorMessage = "Unit must be 'percent' or 'fixed'.")]
        [DefaultValue("percent")]
        public string Unit { get; set; }

        [Display(Name = "Số lượng mã giảm")]
        [Required]
        public int CodeQuantity { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        [Required]
        public DateTime EndDate { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [NotMapped]
        public bool IsEnd
        {
            get
            {
                return DateTime.Now.CompareTo(EndDate) > 0;
            }
        }

        [NotMapped]
        public bool IsStart
        {
            get
            {
                return DateTime.Now.CompareTo(StartDate) > 0;
            }
        }

        [Display(Name = "Đang hoạt động")]
        [NotMapped]
        public bool IsActive
        {
            get
            {
                return IsStart && !IsEnd;
            }
        }

    }
}