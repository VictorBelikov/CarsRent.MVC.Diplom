using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarsRentEF.Models.MetaData;

namespace CarsRentEF.Models
{
    [MetadataType(typeof(DiscountMetaData))]
    [Table("Скидки")]
    public partial class Discount
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int СкидкаID { get; set; }

        [Required, StringLength(30)]
        public string НаименованиеСкидки { get; set; }

        [Required, DefaultValue(0)]
        [Range(1, int.MaxValue, ErrorMessage = "Введите положительное целое число больше 0")]
        public int Процент { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
