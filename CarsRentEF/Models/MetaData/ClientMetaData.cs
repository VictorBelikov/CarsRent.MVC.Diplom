using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsRentEF.Models.MetaData
{
    public class ClientMetaData
    {
        [Display(Name = "Номер Паспорта")]
        public string НомерПаспорта { get; set; }

        [Display(Name = "Номер Вод. Уд.")]
        public string НомерВодУд { get; set; }
    }
}
