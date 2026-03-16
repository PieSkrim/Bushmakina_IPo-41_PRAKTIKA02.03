using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bushmakina.Domain.Entities
{
    [Table("sales_records_bushmakina", Schema = "app")]
    public class SalesRecordEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("partner_id")]
        public int PartnerId { get; set; }

        [ForeignKey(nameof(PartnerId))]
        public PartnerEntity? Partner { get; set; }

        [Required]
        [Column("product_name")]
        public string ProductName { get; set; } = string.Empty;

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("sale_date")]
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    }
}