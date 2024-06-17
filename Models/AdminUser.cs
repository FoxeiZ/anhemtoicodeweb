namespace anhemtoicodeweb.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AdminUser")]
    public partial class AdminUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public string NameUser { get; set; }

        public int? IDCus { get; set; }
    }
}
