using System.Collections.Generic;
using CarsRentEF.Models;

namespace CarsRentMVC.Models.ViewModels
{
    public class CarListViewModel
    {
        public IEnumerable<Car> Cars { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}