using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsRentEF.Models.MetaData;

namespace CarsRentEF.Models
{
    [Table("Клиенты")]
    [MetadataType(typeof(ClientMetaData))]
    public partial class Client
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int КлиентID { get; set; }

        [Required, StringLength(50)]
        [RegularExpression("[^\\d]+", ErrorMessage = "Пожалуйста введите валидный корректное имя клиента")]
        public string ФИО { get; set; }

        [Required, StringLength(50)]
        public string Адрес { get; set; }

        [Required, StringLength(15)]
        [RegularExpression("[\\d\\-]+", ErrorMessage = "Пожалуйста введите валидный корректный телефонный номер")]
        public string Телефон { get; set; }

        [Required, StringLength(20)]
        [Index("IDX_Клиенты_НомерПаспорта", IsUnique = true)]
        [RegularExpression("[^\\s]+", ErrorMessage = "Пожалуйста введите корректный номер паспорта")]
        public string НомерПаспорта { get; set; }

        [Required, StringLength(20)]
        [Index("IDX_Клиенты_НомерВодУд", IsUnique = true)]
        public string НомерВодУд { get; set; }

        public virtual ICollection<Rent> Аренды { get; set; } = new HashSet<Rent>();

        [NotMapped]
        public string FullClientData => ФИО + " " + НомерПаспорта; // Вычисляемое поле

        [Required, StringLength(3)]
        public string ЧерныйСписок { get; set; }

        [ScaffoldColumn(false)]
        public byte[] Image { get; set; }       // Данные изображения

        [ScaffoldColumn(false)]
        public string MimeType { get; set; }    // Mime - тип данных изображения

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
