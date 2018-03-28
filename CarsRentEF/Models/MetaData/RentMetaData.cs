using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsRentEF.Models.MetaData
{
    public class RentMetaData
    {
        [Display(Name = "Дата Выдачи")]
        public DateTime ДатаВыдачи { get; set; }

        [Display(Name = "Дата Возврата")]
        public DateTime ДатаВозврата { get; set; }

        [Display(Name = "Стоимость")]
        public double ИтоговаяСумма { get; set; }

        [Display(Name = "ID Аренды")]
        public int АрендаID { get; set; }
    }
}
