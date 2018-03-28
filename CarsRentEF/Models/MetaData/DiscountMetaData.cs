using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsRentEF.Models.MetaData
{
    public class DiscountMetaData
    {
        [Display(Name = "Наименование")]
        public string НаименованиеСкидки { get; set; }
    }
}
