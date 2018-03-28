using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsRentEF.Models
{
    [Table("Автомобили")]
    public partial class Car
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int АвтоID { get; set; }

        [Required, StringLength(10)]
        [Index("IDX_Автомобили_Госномер", IsUnique = true)]
        public string Госномер { get; set; }

        [Required, StringLength(20)]
        [Index("IDX_Автомобили_VIN", IsUnique = true)]
        [RegularExpression("[\\d\\w]+", ErrorMessage = "Пожалуйста введите валидный VIN номер")]
        public string VIN { get; set; }

        [Required, StringLength(20)]
        public string Марка { get; set; }

        [Required, StringLength(20)]
        public string Модель { get; set; }

        [Required]
        [Range(1920, int.MaxValue, ErrorMessage = "Введите положительное целое число больше 1920")]
        public int ГодВыпуска { get; set; }

        [Required, StringLength(20), DefaultValue("Седан")]
        public string ТипКузова { get; set; }

        [Required, StringLength(20)]
        public string ТипДвигателя { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите вещественное число больше 0")]
        public double ОбъемДвигателя{ get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Введите целое число больше 0")]
        public int Мощность{ get; set; }

        [StringLength(20)]
        public string Цвет { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите вещественное число больше 0")]
        public double Стоимость { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите вещественное число больше 0")]
        public double СтоимостПрокатаВСут { get; set; }

        [Required, StringLength(10), DefaultValue("Свободен")]
        public string Статус { get; set; }

        public virtual ICollection<Rent> Аренды { get; set; } = new HashSet<Rent>();

        [ScaffoldColumn(false)]
        public byte[] Image { get; set; }       // Данные изображения

        [ScaffoldColumn(false)]
        public string MimeType { get; set; }    // Mime - тип данных изображения

        [NotMapped]
        public double PeriodProfit { get; set; } = 0; // Доход за определенный период.

        [NotMapped]
        public int PeriodDays { get; set; } = 0; // Кол-во дней в периоде.

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
