using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsRentEF.Models
{
    [Table("ДопОборудование")]
    public partial class Option
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ОборудованиеID")]
        public int OptionId { get; set; }

        [Required, Column("Наименование")]
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Required, Column("ИнвНомер")]
        [Range(1, int.MaxValue, ErrorMessage = "Введите положительное целое число больше 0")]
        [Display(Name = "Инвентарный номер")]
        public int InvNumber { get; set; }

        [Required, Column("Стоимость")]
        [Display(Name = "Полная стоимость")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите положительное целое число больше 0")]
        public double FullCost { get; set; }

        [Required, Column("СтоимостьПрокатаВСут")]
        [Display(Name = "Стоимость аренды в сут.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите положительное целое число больше 0")]
        public double RentDayCost { get; set; }

        [NotMapped]
        public double PeriodProfit { get; set; } = 0; // Доход за определенный период.

        [NotMapped]
        public int PeriodDays { get; set; } = 0; // Кол-во дней в периоде.

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
