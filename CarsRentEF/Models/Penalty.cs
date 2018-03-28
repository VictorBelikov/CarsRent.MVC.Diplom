using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarsRentEF.Models.MetaData;

namespace CarsRentEF.Models
{
    [Table("Штрафы")]
    [MetadataType(typeof(PenaltyMetaData))]
    public partial class Penalty
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ШтрафID { get; set; }

        [Required, StringLength(30)]
        public string НаименованиеШтрафа { get; set; }

        [Required, DefaultValue(0)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите положительное целое число больше 0")]
        public double СуммаШтрафа { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
