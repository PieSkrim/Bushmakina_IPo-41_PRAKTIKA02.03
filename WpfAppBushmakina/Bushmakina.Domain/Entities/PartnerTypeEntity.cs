using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bushmakina.Domain.Entities
{
    [Table("partner_types_bushmakina", Schema = "app")]
    public class PartnerTypeEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("type_name")]
        public string TypeName { get; set; } = string.Empty;
    }
}