using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarsRentEF.Models.MetaData;

namespace CarsRentEF.Models
{
    [Table("Аренда")]
    [MetadataType(typeof(RentMetaData))]
    public partial class Rent
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int АрендаID { get; set; }
        
        public int КлиентID { get; set; }
        [ForeignKey("КлиентID")]
        public virtual Client Client { get; set; } // Поддерживающее поле

        public int АвтоID { get; set; }
        [ForeignKey("АвтоID")]
        public virtual Car Car { get; set; } // Поддерживающее поле

        [Required, Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ДатаВыдачи { get; set; }

        [Required, Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ДатаВозврата { get; set; }

        [Required, DefaultValue(0)]
        public double ИтоговаяСумма { get; set; }

        [Required, StringLength(20), DefaultValue("Бронь")]
        public string Состояние { get; set; }

        [NotMapped]
        public IEnumerable<Discount> DiscountsForRent { get; set; } = new HashSet<Discount>();
        public string Скидки { get; set; } = ""; // Строковое представление скидок добавленных в аренду.

        [NotMapped]
        public IEnumerable<Option> OptionsForRent { get; set; } = new HashSet<Option>();
        public string ДопОборудование { get; set; } = ""; // Строковое представление доп.опций добавленных в аренду.

        [NotMapped]
        public IEnumerable<Penalty> PenaltyForRent { get; set; } = new HashSet<Penalty>();
        public string Штрафы { get; set; } = ""; // Строковое представление штрафов добавленных в аренду.

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
