using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bushmakina.Domain.Entities
{
    [Table("partners_bushmakina", Schema = "app")]
    public class PartnerEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("type_id")]
        public int TypeId { get; set; }

        [ForeignKey(nameof(TypeId))]
        public PartnerTypeEntity? Type { get; set; }

        [Column("inn")]
        public string? INN { get; set; }

        [Column("rating")]
        public int Rating { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("director_name")]
        public string? DirectorName { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("sales_places")]
        public string? SalesPlaces { get; set; }

        public ICollection<SalesRecordEntity> SalesRecords { get; set; } = new List<SalesRecordEntity>();

        [NotMapped]
        public int CurrentBonus { get; set; } = 0;
    }
}