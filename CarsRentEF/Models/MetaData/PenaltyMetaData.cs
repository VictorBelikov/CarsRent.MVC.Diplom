using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsRentEF.Models.MetaData
{
    public class PenaltyMetaData
    {
        [Display(Name = "Наименование")]
        public string НаименованиеШтрафа { get; set; }

        [Display(Name = "Сумма")]
        public double СуммаШтрафа { get; set; }
    }
}
