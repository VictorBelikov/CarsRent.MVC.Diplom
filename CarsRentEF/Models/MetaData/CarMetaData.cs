using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsRentEF.Models.MetaData
{
    public class CarMetaData
    {
        [Display(Name = "Год выпуска")]
        public int ГодВыпуска { get; set; }

        [Display(Name = "Тип кузова")]
        public string ТипКузова { get; set; }

        [Display(Name = "Стоимость в сутки")]
        public decimal СтоимостПрокатаВСут { get; set; }

        [Display(Name = "VIN номер")]
        public string VIN { get; set; }
    }
}
