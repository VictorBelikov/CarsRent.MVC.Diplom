using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsRentEF.Models
{
    [Table("Отчеты")]
    public partial class Report
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ОтчетID")]
        public int ReportId { get; set; }

        [Required, StringLength(50), Column("Наименование")]
        [Display(Name = "Краткое имя")]
        public string ShortName { get; set; }

        [Required, Column("ДатаФормирования", TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата формирования")]
        public DateTime DateOfFormation { get; set; } // Дата формирования

        [Required, Column("НачалоПериода", TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Начало периода")]
        public DateTime StartDate { get; set; } // Начало периода

        [Required, Column("КонецПериода", TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Конец периода")]
        public DateTime EndDate { get; set; } // Конец периода

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
